using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq; // Para manipular JSON

namespace MBVFlightManager
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Impede redimensionamento
            this.MaximizeBox = false; // Remove botão de maximizar
            this.MinimizeBox = true;  // Permite minimizar
            this.SizeGripStyle = SizeGripStyle.Hide; // Esconde a alça de redimensionamento no canto inferior direito

            // **Carregar credenciais salvas**
            LoadSavedCredentials();
        }

        public static int LoggedPilotId = 0; // Variável global para armazenar o ID do piloto

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            // Capturar os valores dos campos
            string email = txtUsername.Text.Trim();
            string senha = txtPassword.Text.Trim();

            // Verificar se os campos estão vazios
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Erro no login: Preencha todos os campos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Criar os dados do login como JSON
                    var loginData = new Dictionary<string, string>
                    {
                        { "email", email },
                        { "password", senha }
                    };

                    HttpContent content = new FormUrlEncodedContent(loginData);
                    //HttpResponseMessage response = await client.PostAsync("http://localhost/mbv/login.php", content);
                    HttpResponseMessage response = await client.PostAsync(Config.ApiBaseUrl + "login.php", content);


                    // Mostrar resposta bruta do servidor para depuração
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseString);

                        if (result != null && Convert.ToString(result.status) == "success")
                        {
                            // Guardar o ID do piloto para uso posterior
                            LoginForm.LoggedPilotId = int.Parse(result.user_id.ToString());

                            MessageBox.Show("Login realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // **Salvar credenciais se a opção "Lembrar" estiver ativada**
                            SaveCredentials(email, senha, checkBoxRememberMe.Checked);

                            // Abrir FlightForm e fechar LoginForm
                            FlightForm flightForm = new FlightForm();
                            this.Hide();
                            flightForm.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Erro no login: {Convert.ToString(result?.message ?? "Resposta inesperada")}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erro ao conectar ao servidor.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // **Método para salvar as credenciais**
        private void SaveCredentials(string email, string password, bool remember)
        {
            if (remember)
            {
                Properties.Settings.Default["username"] = email;
                Properties.Settings.Default["password"] = password;
                Properties.Settings.Default["RememberMe"] = true;
            }
            else
            {
                Properties.Settings.Default["username"] = "";
                Properties.Settings.Default["password"] = "";
                Properties.Settings.Default["RememberMe"] = false;
            }

            Properties.Settings.Default.Save();
        }

        // **Método para carregar credenciais salvas ao iniciar o login**
        private void LoadSavedCredentials()
        {
            if (Properties.Settings.Default["RememberMe"] != null &&
                (bool)Properties.Settings.Default["RememberMe"])
            {
                txtUsername.Text = Properties.Settings.Default["username"].ToString();
                txtPassword.Text = Properties.Settings.Default["password"].ToString();
                checkBoxRememberMe.Checked = true;
            }
            else
            {
                checkBoxRememberMe.Checked = false;
            }
        }
    }
}
