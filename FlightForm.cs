using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSUIPC;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;


namespace MBVFlightManager
{
    public partial class FlightForm : Form
    {
        private DateTime flightStartTime;
        private bool isFlightActive = false;

        public FlightForm()
        {
            InitializeComponent();
            flightTimer.Tick += FlightTimer_Tick;
            fsuipcCheckTimer.Tick += FsuipcCheckTimer_Tick;

            // Configurar Timers
            flightTimer.Interval = 1000;
            fsuipcCheckTimer.Interval = 5000;

            // Associar eventos para forçar UPPERCASE e buscar aeronaves ao digitar
            txtDeparture.TextChanged += TxtDeparture_TextChanged;
            txtArrival.TextChanged += ForceUpperCase;
            txtFlightNumber.TextChanged += ForceUpperCase;
        }

        // Método que transforma qualquer entrada em maiúscula automaticamente
        private void ForceUpperCase(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                int cursorPosition = textBox.SelectionStart; // Salvar posição do cursor
                textBox.Text = textBox.Text.ToUpper(); // Converter para maiúsculas
                textBox.SelectionStart = cursorPosition; // Restaurar posição do cursor
            }
        }

        // Evento acionado ao alterar o campo de ICAO
        private void TxtDeparture_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                int cursorPosition = textBox.SelectionStart; // Salvar posição do cursor
                textBox.Text = textBox.Text.ToUpper(); // Converter para uppercase
                textBox.SelectionStart = cursorPosition; // Restaurar posição do cursor
            }

            string departureIcao = txtDeparture.Text.Trim().ToUpper();

            if (!string.IsNullOrEmpty(departureIcao) && departureIcao.Length == 4)
            {
                cmbAircraft.DataSource = null;
                cmbAircraft.Items.Clear();
                cmbAircraft.SelectedIndex = -1;
                cmbAircraft.Text = "Carregando...";

                LoadAircraftByLocation(departureIcao);
            }
            else
            {
                cmbAircraft.DataSource = null;
                cmbAircraft.Items.Clear();
                cmbAircraft.Items.Add("Nenhuma aeronave disponível");
                cmbAircraft.SelectedIndex = 0;
            }
        }

        private async void LoadAircraftByLocation(string icao)
        {
            try
            {
                cmbAircraft.DataSource = null;
                cmbAircraft.Items.Clear();
                cmbAircraft.SelectedIndex = -1;
                cmbAircraft.Text = "Carregando...";

                using (HttpClient client = new HttpClient())
                {
                    string url = $"{Config.ApiBaseUrl}get_aircrafts.php?icao={icao}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseString);

                        if (result.status == "success" && result.data != null && result.data.Count > 0)
                        {
                            var aircraftList = new List<KeyValuePair<int, string>>();

                            foreach (var aircraft in result.data)
                            {
                                int aircraftId = aircraft.id;
                                string aircraftName = $"{aircraft.matricula} - {aircraft.modelo}";

                                aircraftList.Add(new KeyValuePair<int, string>(aircraftId, aircraftName));
                            }

                            if (aircraftList.Count > 0)
                            {
                                cmbAircraft.DataSource = new BindingSource(aircraftList, null);
                                cmbAircraft.DisplayMember = "Value";
                                cmbAircraft.ValueMember = "Key";
                                cmbAircraft.SelectedIndex = 0;
                                cmbAircraft.Refresh();
                                return;
                            }
                        }

                        cmbAircraft.DataSource = null;
                        cmbAircraft.Items.Clear();
                        cmbAircraft.Items.Add("Nenhuma aeronave disponível");
                        cmbAircraft.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("Erro ao carregar a lista de aeronaves.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar aeronaves: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnStartFlight_Click(object sender, EventArgs e)
        {
            string departure = txtDeparture.Text.Trim();
            string arrival = txtArrival.Text.Trim();
            string flightNumber = txtFlightNumber.Text.Trim();

            // Validar códigos ICAO
            if (!IsValidICAOCode(departure) || !IsValidICAOCode(arrival))
            {
                MessageBox.Show("Por favor, insira códigos ICAO válidos (4 letras).", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Removida a validação que impedia o mesmo ICAO para origem e destino

            if (string.IsNullOrWhiteSpace(flightNumber))
            {
                MessageBox.Show("Por favor, insira o número do voo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbAircraft.SelectedValue == null || !int.TryParse(cmbAircraft.SelectedValue.ToString(), out int aircraftId))
            {
                MessageBox.Show("Erro ao obter o ID da aeronave. Verifique se uma aeronave foi selecionada corretamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var (latitude, longitude) = GetAircraftCoordinates();

            if (latitude == 0 && longitude == 0)
            {
                MessageBox.Show("Não foi possível determinar as coordenadas da aeronave. Verifique se o FSUIPC está conectado e a aeronave está no solo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblCoordinates.Text = $"Coordenadas:\nLatitude: {latitude:F6}\nLongitude: {longitude:F6}";

            flightStartTime = DateTime.Now;
            flightTimer.Start();
            fsuipcCheckTimer.Start();
            isFlightActive = true;

            await UpdateAircraftStatus(aircraftId, "Ocupada");

            txtDeparture.Enabled = false;
            txtArrival.Enabled = false;
            cmbAircraft.Enabled = false;
            txtFlightNumber.Enabled = false;

            btnStartFlight.Enabled = false;
            btnEndFlight.Enabled = true;

            MessageBox.Show($"Voo iniciado!\nNúmero do Voo: {flightNumber}\nSaída: {departure}\nDestino: {arrival}\nAeronave ID: {aircraftId}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool IsValidICAOCode(string code)
        {
            // Verifica se o código tem exatamente 4 letras alfabéticas
            return !string.IsNullOrEmpty(code) && Regex.IsMatch(code.Trim(), @"^[A-Za-z]{4}$");
        }

        private (double Latitude, double Longitude) GetAircraftCoordinates()
        {
            try
            {
                // Abrir conexão com FSUIPC
                FSUIPC.FSUIPCConnection.Open();

                // Offsets para Latitude e Longitude
                FSUIPC.Offset<long> latOffset = new FSUIPC.Offset<long>(0x0560);
                FSUIPC.Offset<long> lonOffset = new FSUIPC.Offset<long>(0x0568);

                // Processar os valores
                FSUIPC.FSUIPCConnection.Process();

                // Converter para graus
                double latitude = latOffset.Value * (90.0 / (10001750.0 * 65536.0 * 65536.0));
                double longitude = lonOffset.Value * (360.0 / (65536.0 * 65536.0 * 65536.0 * 65536.0));

                // Fechar conexão
                FSUIPC.FSUIPCConnection.Close();

                return (latitude, longitude);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao obter coordenadas: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (0, 0);
            }
        }

        private void btnEndFlight_Click(object sender, EventArgs e)
        {
            // Parar os timers e registrar o tempo de voo
            flightTimer.Stop();
            fsuipcCheckTimer.Stop();
            isFlightActive = false;

            TimeSpan elapsed = DateTime.Now - flightStartTime;

            // 🔹 Converter para total de segundos para armazenamento no banco de dados
            int flightSeconds = (int)elapsed.TotalSeconds;

            // 🔹 Evita registrar menos de 1 segundo
            if (flightSeconds < 1)
            {
                flightSeconds = 1;
            }

            // 🔹 Converter para HH:mm:ss para exibição ao piloto
            string flightFormatted = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";

            // Mostrar uma MessageBox para confirmação
            DialogResult result = MessageBox.Show(
                $"Tempo de voo: {flightFormatted}\nDeseja enviar os dados do voo?",
                "Finalizar Voo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                EnviarVoo(flightSeconds.ToString());

            }
            else
            {
                MessageBox.Show("Voo cancelado. Os dados não foram enviados.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Resetar a interface
            txtDeparture.Enabled = true;
            txtArrival.Enabled = true;
            cmbAircraft.Enabled = true;
            txtFlightNumber.Enabled = true;
            btnStartFlight.Enabled = true;
            btnEndFlight.Enabled = false;
            lblTimer.Text = "00:00:00";
            lblCoordinates.Text = "Coordenadas:\nLatitude: N/A\nLongitude: N/A";
        }

        private async void EnviarVoo(string flightDuration)
        {
            string flightNumber = txtFlightNumber.Text.Trim();
            string departureAirport = txtDeparture.Text.Trim();
            string arrivalAirport = txtArrival.Text.Trim();

            if (cmbAircraft.SelectedValue == null || !int.TryParse(cmbAircraft.SelectedValue.ToString(), out int aircraftId))
            {
                MessageBox.Show("Erro ao obter o ID da aeronave.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pilotId = LoginForm.LoggedPilotId;
            double verticalSpeed = GetVerticalSpeed();


            //MessageBox.Show($"Debug:\nNúmero do voo: {flightNumber}\nDuração: {flightDuration}\nOrigem: {departureAirport}\nDestino: {arrivalAirport}\nPiloto ID: {pilotId}\nAeronave ID: {aircraftId}\nVertical Speed: {verticalSpeed}");

            // Enviar os dados ao PHP
            await UpdateFlightData(flightNumber, flightDuration, departureAirport, arrivalAirport, pilotId, aircraftId.ToString(), verticalSpeed);

            // Atualizar a localização da aeronave no banco de dados
            await UpdateAircraftLocation(aircraftId, arrivalAirport);

            // Atualizar o status da aeronave para "Livre"
            await UpdateAircraftStatus(aircraftId, "Livre");

            MessageBox.Show($"Voo enviado com sucesso! Aeronave {aircraftId} movida para {arrivalAirport}.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task UpdateAircraftStatus(int aircraftId, string status)
        {
            string url = $"{Config.ApiBaseUrl}update_aircraft_status.php";


            var postData = new Dictionary<string, string>
            {
                { "aircraft_id", aircraftId.ToString() },
                { "status", status }
            };

            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await client.PostAsync(url, content);

                string responseString = await response.Content.ReadAsStringAsync();
                //MessageBox.Show($"Resposta do Servidor (Update Status): {responseString}", "Debug");
            }
        }

        // Método para obter a velocidade vertical no pouso (Exemplo de FSUIPC)
        private int GetVerticalSpeed()
        {
            try
            {
                FSUIPC.FSUIPCConnection.Open();
                FSUIPC.Offset<int> vsOffset = new FSUIPC.Offset<int>(0x030C); // Offset do FSUIPC para velocidade vertical
                FSUIPC.FSUIPCConnection.Process();

                int verticalSpeed = (int)(vsOffset.Value * 60.0 / 256.0); // Conversão correta para int

                FSUIPC.FSUIPCConnection.Close();
                return verticalSpeed; // Retorna diretamente como inteiro
            }
            catch
            {
                return 0; // Retorna 0 em caso de erro
            }
        }

        // Enviar os dados para o banco via PHP
        private async Task UpdateFlightData(string flightNumber, string flightDuration, string departure, string arrival, int pilotId, string aircraftId, double verticalSpeed)
        {
            string url = $"{Config.ApiBaseUrl}update_flight.php";

            var postData = new Dictionary<string, string>
            {
                { "numero_voo", flightNumber },
                { "horas", flightDuration },  // Enviado corretamente como string
                { "origem", departure },
                { "destino", arrival },
                { "piloto_id", pilotId.ToString() },
                { "aeronave_id", aircraftId },
                { "vertical_speed", verticalSpeed.ToString("F2") }
            };

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpContent content = new FormUrlEncodedContent(postData);
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseString);

                        if (Convert.ToString(result.status) == "success")
                        {
                            //MessageBox.Show("Voo registrado com sucesso no banco de dados.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Erro ao registrar voo: {Convert.ToString(result.message)}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erro na comunicação com o servidor.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar voo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RegisterFlight(int pilotId, string flightNumber, string origin, string destination, int aircraftId, double flightHours, double verticalSpeed)
        {
            string url = $"{Config.ApiBaseUrl}register_flight.php";

            var flightData = new Dictionary<string, string>
            {
                { "piloto_id", pilotId.ToString() },
                { "numero_voo", flightNumber },
                { "origem", origin },
                { "destino", destination },
                { "aeronave_id", aircraftId.ToString() },
                { "horas", flightHours.ToString("F2") },
                { "vertical_speed", verticalSpeed.ToString("F2") }
            };

            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(flightData);
                HttpResponseMessage response = await client.PostAsync(url, content);

                string responseString = await response.Content.ReadAsStringAsync();
                //MessageBox.Show($"Resposta do Servidor: {responseString}", "Debug");
            }
        }

        // Método para capturar a velocidade vertical do pouso (exemplo)
        private async Task UpdateAircraftLocation(int aircraftId, string icao)
        {
            string url = $"{Config.ApiBaseUrl}update_aircraft_location.php";

            var postData = new Dictionary<string, string>
            {
                { "aircraft_id", aircraftId.ToString() },
                { "icao", icao }
            };

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpContent content = new FormUrlEncodedContent(postData);
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    string responseString = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseString);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar a posição da aeronave: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FlightTimer_Tick(object sender, EventArgs e)
        {
            if (isFlightActive)
            {
                TimeSpan elapsed = DateTime.Now - flightStartTime;
                lblTimer.Text = elapsed.ToString(@"hh\:mm\:ss");
            }
        }

        private void FsuipcCheckTimer_Tick(object sender, EventArgs e)
        {
            // Simulação da conexão com o FSUIPC
            bool isConnected = CheckFsuipcConnection();

            if (!isConnected)
            {
                fsuipcCheckTimer.Stop();
                flightTimer.Stop();
                isFlightActive = false;

                MessageBox.Show("Conexão com FSUIPC perdida! O voo foi encerrado automaticamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnStartFlight.Enabled = true;
                btnEndFlight.Enabled = false;
            }
        }

        private bool CheckFsuipcConnection()
        {
            // Simula a verificação de conexão com FSUIPC
            return true; // Alterar para false para testar desconexão automática
        }

    }
}

