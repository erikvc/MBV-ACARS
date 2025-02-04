namespace MBVFlightManager
{
    partial class FlightForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlightForm));
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFlightNumber = new System.Windows.Forms.TextBox();
            this.btnEndFlight = new System.Windows.Forms.Button();
            this.btnStartFlight = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAircraft = new System.Windows.Forms.ComboBox();
            this.txtArrival = new System.Windows.Forms.TextBox();
            this.txtDeparture = new System.Windows.Forms.TextBox();
            this.lblArrival = new System.Windows.Forms.Label();
            this.lblDeparture = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCoordinates = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            this.flightTimer = new System.Windows.Forms.Timer(this.components);
            this.fsuipcCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label4.Location = new System.Drawing.Point(260, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Sistema de Reporte de Vôo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label3.Location = new System.Drawing.Point(219, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(249, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "MARINHA DO BRASIL VIRTUAL";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtFlightNumber);
            this.groupBox1.Controls.Add(this.btnEndFlight);
            this.groupBox1.Controls.Add(this.btnStartFlight);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbAircraft);
            this.groupBox1.Controls.Add(this.txtArrival);
            this.groupBox1.Controls.Add(this.txtDeparture);
            this.groupBox1.Controls.Add(this.lblArrival);
            this.groupBox1.Controls.Add(this.lblDeparture);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox1.Location = new System.Drawing.Point(38, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 319);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gerenciamento de Vôo";
            // 
            // txtFlightNumber
            // 
            this.txtFlightNumber.Location = new System.Drawing.Point(22, 59);
            this.txtFlightNumber.Name = "txtFlightNumber";
            this.txtFlightNumber.Size = new System.Drawing.Size(100, 22);
            this.txtFlightNumber.TabIndex = 8;
            // 
            // btnEndFlight
            // 
            this.btnEndFlight.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnEndFlight.Location = new System.Drawing.Point(137, 206);
            this.btnEndFlight.Name = "btnEndFlight";
            this.btnEndFlight.Size = new System.Drawing.Size(100, 25);
            this.btnEndFlight.TabIndex = 7;
            this.btnEndFlight.Text = "Finalizar Vôo";
            this.btnEndFlight.UseVisualStyleBackColor = true;
            this.btnEndFlight.Click += new System.EventHandler(this.btnEndFlight_Click);
            // 
            // btnStartFlight
            // 
            this.btnStartFlight.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnStartFlight.Location = new System.Drawing.Point(22, 206);
            this.btnStartFlight.Name = "btnStartFlight";
            this.btnStartFlight.Size = new System.Drawing.Size(100, 25);
            this.btnStartFlight.TabIndex = 6;
            this.btnStartFlight.Text = "Iniciar Vôo";
            this.btnStartFlight.UseVisualStyleBackColor = true;
            this.btnStartFlight.Click += new System.EventHandler(this.btnStartFlight_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Selecione a Aeronave";
            // 
            // cmbAircraft
            // 
            this.cmbAircraft.FormattingEnabled = true;
            this.cmbAircraft.Location = new System.Drawing.Point(22, 151);
            this.cmbAircraft.Name = "cmbAircraft";
            this.cmbAircraft.Size = new System.Drawing.Size(215, 24);
            this.cmbAircraft.TabIndex = 4;
            // 
            // txtArrival
            // 
            this.txtArrival.Location = new System.Drawing.Point(137, 104);
            this.txtArrival.Name = "txtArrival";
            this.txtArrival.Size = new System.Drawing.Size(100, 22);
            this.txtArrival.TabIndex = 3;
            // 
            // txtDeparture
            // 
            this.txtDeparture.Location = new System.Drawing.Point(22, 104);
            this.txtDeparture.Name = "txtDeparture";
            this.txtDeparture.Size = new System.Drawing.Size(100, 22);
            this.txtDeparture.TabIndex = 2;
            // 
            // lblArrival
            // 
            this.lblArrival.AutoSize = true;
            this.lblArrival.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArrival.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblArrival.Location = new System.Drawing.Point(134, 88);
            this.lblArrival.Name = "lblArrival";
            this.lblArrival.Size = new System.Drawing.Size(64, 16);
            this.lblArrival.TabIndex = 1;
            this.lblArrival.Text = "Chegada";
            // 
            // lblDeparture
            // 
            this.lblDeparture.AutoSize = true;
            this.lblDeparture.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeparture.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblDeparture.Location = new System.Drawing.Point(19, 88);
            this.lblDeparture.Name = "lblDeparture";
            this.lblDeparture.Size = new System.Drawing.Size(44, 16);
            this.lblDeparture.TabIndex = 0;
            this.lblDeparture.Text = "Saída";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblCoordinates);
            this.groupBox2.Controls.Add(this.lblTimer);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox2.Location = new System.Drawing.Point(311, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 319);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Informações";
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.AutoSize = true;
            this.lblCoordinates.Location = new System.Drawing.Point(16, 94);
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(91, 16);
            this.lblCoordinates.TabIndex = 2;
            this.lblCoordinates.Text = "Lat/Long: n/a";
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new System.Drawing.Point(16, 36);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(127, 16);
            this.lblTimer.TabIndex = 0;
            this.lblTimer.Text = "Tempo de Vôo: n/a";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Número do Võo";
            // 
            // FlightForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(692, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FlightForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MBV - Gerenciamento de Vôo";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtArrival;
        private System.Windows.Forms.TextBox txtDeparture;
        private System.Windows.Forms.Label lblArrival;
        private System.Windows.Forms.Label lblDeparture;
        private System.Windows.Forms.ComboBox cmbAircraft;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblCoordinates;
        private System.Windows.Forms.Button btnEndFlight;
        private System.Windows.Forms.Button btnStartFlight;
        private System.Windows.Forms.Timer flightTimer;
        private System.Windows.Forms.Timer fsuipcCheckTimer;
        private System.Windows.Forms.TextBox txtFlightNumber;
        private System.Windows.Forms.Label label2;
    }
}