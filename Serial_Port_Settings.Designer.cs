namespace Network_Manager_GUI
{
    partial class FrmPortSettings
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
            this.combo_BaudRate = new System.Windows.Forms.ComboBox();
            this.combo_Handshake = new System.Windows.Forms.ComboBox();
            this.buttonResetToDefault = new System.Windows.Forms.Button();
            this.buttonApplySettings = new System.Windows.Forms.Button();
            this.combo_StopBits = new System.Windows.Forms.ComboBox();
            this.combo_Parity = new System.Windows.Forms.ComboBox();
            this.combo_DataBits = new System.Windows.Forms.ComboBox();
            this.combo_PortName = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // combo_BaudRate
            // 
            this.combo_BaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_BaudRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_BaudRate.FormattingEnabled = true;
            this.combo_BaudRate.Items.AddRange(new object[] {
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.combo_BaudRate.Location = new System.Drawing.Point(113, 43);
            this.combo_BaudRate.Margin = new System.Windows.Forms.Padding(4);
            this.combo_BaudRate.Name = "combo_BaudRate";
            this.combo_BaudRate.Size = new System.Drawing.Size(187, 24);
            this.combo_BaudRate.TabIndex = 63;
            // 
            // combo_Handshake
            // 
            this.combo_Handshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Handshake.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_Handshake.FormattingEnabled = true;
            this.combo_Handshake.Location = new System.Drawing.Point(113, 191);
            this.combo_Handshake.Margin = new System.Windows.Forms.Padding(4);
            this.combo_Handshake.Name = "combo_Handshake";
            this.combo_Handshake.Size = new System.Drawing.Size(187, 24);
            this.combo_Handshake.TabIndex = 62;
            // 
            // buttonResetToDefault
            // 
            this.buttonResetToDefault.Location = new System.Drawing.Point(130, 229);
            this.buttonResetToDefault.Margin = new System.Windows.Forms.Padding(4);
            this.buttonResetToDefault.Name = "buttonResetToDefault";
            this.buttonResetToDefault.Size = new System.Drawing.Size(170, 28);
            this.buttonResetToDefault.TabIndex = 61;
            this.buttonResetToDefault.Text = "Reset to Default Settings";
            this.buttonResetToDefault.UseVisualStyleBackColor = true;
            this.buttonResetToDefault.Click += new System.EventHandler(this.ButtonResetToDefault_Click);
            // 
            // buttonApplySettings
            // 
            this.buttonApplySettings.Location = new System.Drawing.Point(5, 229);
            this.buttonApplySettings.Margin = new System.Windows.Forms.Padding(4);
            this.buttonApplySettings.Name = "buttonApplySettings";
            this.buttonApplySettings.Size = new System.Drawing.Size(117, 28);
            this.buttonApplySettings.TabIndex = 60;
            this.buttonApplySettings.Text = "Apply Settings";
            this.buttonApplySettings.UseVisualStyleBackColor = true;
            this.buttonApplySettings.Click += new System.EventHandler(this.ButtonApplySettings);
            // 
            // combo_StopBits
            // 
            this.combo_StopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_StopBits.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_StopBits.FormattingEnabled = true;
            this.combo_StopBits.Location = new System.Drawing.Point(113, 154);
            this.combo_StopBits.Margin = new System.Windows.Forms.Padding(4);
            this.combo_StopBits.Name = "combo_StopBits";
            this.combo_StopBits.Size = new System.Drawing.Size(187, 24);
            this.combo_StopBits.TabIndex = 59;
            // 
            // combo_Parity
            // 
            this.combo_Parity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Parity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_Parity.FormattingEnabled = true;
            this.combo_Parity.Location = new System.Drawing.Point(113, 118);
            this.combo_Parity.Margin = new System.Windows.Forms.Padding(4);
            this.combo_Parity.Name = "combo_Parity";
            this.combo_Parity.Size = new System.Drawing.Size(187, 24);
            this.combo_Parity.TabIndex = 58;
            // 
            // combo_DataBits
            // 
            this.combo_DataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_DataBits.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_DataBits.FormattingEnabled = true;
            this.combo_DataBits.Items.AddRange(new object[] {
            "7",
            "8"});
            this.combo_DataBits.Location = new System.Drawing.Point(113, 80);
            this.combo_DataBits.Margin = new System.Windows.Forms.Padding(4);
            this.combo_DataBits.Name = "combo_DataBits";
            this.combo_DataBits.Size = new System.Drawing.Size(187, 24);
            this.combo_DataBits.TabIndex = 57;
            // 
            // combo_PortName
            // 
            this.combo_PortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_PortName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_PortName.FormattingEnabled = true;
            this.combo_PortName.Location = new System.Drawing.Point(113, 6);
            this.combo_PortName.Margin = new System.Windows.Forms.Padding(4);
            this.combo_PortName.Name = "combo_PortName";
            this.combo_PortName.Size = new System.Drawing.Size(187, 24);
            this.combo_PortName.TabIndex = 56;
            this.combo_PortName.Click += new System.EventHandler(this.Combo_Port_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(2, 196);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 16);
            this.label6.TabIndex = 55;
            this.label6.Text = "Handshake:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(2, 158);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 54;
            this.label5.Text = "Stop Bits:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(2, 121);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 16);
            this.label4.TabIndex = 53;
            this.label4.Text = "Parity:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(2, 84);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 16);
            this.label3.TabIndex = 52;
            this.label3.Text = "Data Bits:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 51;
            this.label2.Text = "Baud Rate:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "Port Name:";
            // 
            // FrmPortSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(313, 264);
            this.Controls.Add(this.combo_BaudRate);
            this.Controls.Add(this.combo_Handshake);
            this.Controls.Add(this.buttonResetToDefault);
            this.Controls.Add(this.buttonApplySettings);
            this.Controls.Add(this.combo_StopBits);
            this.Controls.Add(this.combo_Parity);
            this.Controls.Add(this.combo_DataBits);
            this.Controls.Add(this.combo_PortName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmPortSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial Port Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPortSettings_FormClosed);
            this.Load += new System.EventHandler(this.FrmPortSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox combo_BaudRate;
        private System.Windows.Forms.ComboBox combo_Handshake;
        private System.Windows.Forms.Button buttonResetToDefault;
        private System.Windows.Forms.Button buttonApplySettings;
        private System.Windows.Forms.ComboBox combo_StopBits;
        private System.Windows.Forms.ComboBox combo_Parity;
        private System.Windows.Forms.ComboBox combo_DataBits;
        public System.Windows.Forms.ComboBox combo_PortName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}