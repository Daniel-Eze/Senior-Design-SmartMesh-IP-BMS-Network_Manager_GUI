namespace Network_Manager_GUI
{
    partial class LoadFirmware
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
            this.output_screen = new System.Windows.Forms.RichTextBox();
            this.buttonNetworkManager = new System.Windows.Forms.Button();
            this.buttonMote = new System.Windows.Forms.Button();
            this.buttonAccessPoint = new System.Windows.Forms.Button();
            this.labelStopWatchTimer = new System.Windows.Forms.Label();
            this.pictureBoxOutcome = new System.Windows.Forms.PictureBox();
            this.stopWatchTimer = new System.Windows.Forms.Timer(this.components);
            this.labelOutputScreenCharactersCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutcome)).BeginInit();
            this.SuspendLayout();
            // 
            // output_screen
            // 
            this.output_screen.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output_screen.Location = new System.Drawing.Point(5, 56);
            this.output_screen.Name = "output_screen";
            this.output_screen.ReadOnly = true;
            this.output_screen.Size = new System.Drawing.Size(486, 338);
            this.output_screen.TabIndex = 93;
            this.output_screen.Text = "";
            this.output_screen.TextChanged += new System.EventHandler(this.Output_screen_TextChanged);
            // 
            // buttonNetworkManager
            // 
            this.buttonNetworkManager.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNetworkManager.Location = new System.Drawing.Point(6, 12);
            this.buttonNetworkManager.Name = "buttonNetworkManager";
            this.buttonNetworkManager.Size = new System.Drawing.Size(134, 38);
            this.buttonNetworkManager.TabIndex = 94;
            this.buttonNetworkManager.Text = "Network Manager";
            this.buttonNetworkManager.UseVisualStyleBackColor = true;
            this.buttonNetworkManager.Click += new System.EventHandler(this.ButtonNetworkManager_Click);
            // 
            // buttonMote
            // 
            this.buttonMote.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMote.Location = new System.Drawing.Point(141, 12);
            this.buttonMote.Name = "buttonMote";
            this.buttonMote.Size = new System.Drawing.Size(122, 38);
            this.buttonMote.TabIndex = 95;
            this.buttonMote.Text = "Mote";
            this.buttonMote.UseVisualStyleBackColor = true;
            this.buttonMote.Click += new System.EventHandler(this.ButtonMote_Click);
            // 
            // buttonAccessPoint
            // 
            this.buttonAccessPoint.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAccessPoint.Location = new System.Drawing.Point(265, 12);
            this.buttonAccessPoint.Name = "buttonAccessPoint";
            this.buttonAccessPoint.Size = new System.Drawing.Size(119, 38);
            this.buttonAccessPoint.TabIndex = 96;
            this.buttonAccessPoint.Text = "Access Point";
            this.buttonAccessPoint.UseVisualStyleBackColor = true;
            this.buttonAccessPoint.Click += new System.EventHandler(this.ButtonAccessPoint_Click);
            // 
            // labelStopWatchTimer
            // 
            this.labelStopWatchTimer.AutoSize = true;
            this.labelStopWatchTimer.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStopWatchTimer.Location = new System.Drawing.Point(446, 35);
            this.labelStopWatchTimer.Name = "labelStopWatchTimer";
            this.labelStopWatchTimer.Size = new System.Drawing.Size(31, 18);
            this.labelStopWatchTimer.TabIndex = 100;
            this.labelStopWatchTimer.Text = "-:-:-";
            this.labelStopWatchTimer.Visible = false;
            // 
            // pictureBoxOutcome
            // 
            this.pictureBoxOutcome.Location = new System.Drawing.Point(390, 12);
            this.pictureBoxOutcome.Name = "pictureBoxOutcome";
            this.pictureBoxOutcome.Size = new System.Drawing.Size(47, 38);
            this.pictureBoxOutcome.TabIndex = 99;
            this.pictureBoxOutcome.TabStop = false;
            this.pictureBoxOutcome.Visible = false;
            // 
            // stopWatchTimer
            // 
            this.stopWatchTimer.Interval = 1000;
            // 
            // labelOutputScreenCharactersCount
            // 
            this.labelOutputScreenCharactersCount.AutoSize = true;
            this.labelOutputScreenCharactersCount.Font = new System.Drawing.Font("Garamond", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputScreenCharactersCount.Location = new System.Drawing.Point(208, 397);
            this.labelOutputScreenCharactersCount.Name = "labelOutputScreenCharactersCount";
            this.labelOutputScreenCharactersCount.Size = new System.Drawing.Size(68, 12);
            this.labelOutputScreenCharactersCount.TabIndex = 108;
            this.labelOutputScreenCharactersCount.Text = "0 character(s)";
            // 
            // LoadFirmware
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(497, 415);
            this.Controls.Add(this.labelOutputScreenCharactersCount);
            this.Controls.Add(this.labelStopWatchTimer);
            this.Controls.Add(this.pictureBoxOutcome);
            this.Controls.Add(this.buttonAccessPoint);
            this.Controls.Add(this.buttonMote);
            this.Controls.Add(this.buttonNetworkManager);
            this.Controls.Add(this.output_screen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "LoadFirmware";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Firmware Load";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoadFirmware_FormClosed);
            this.Load += new System.EventHandler(this.LoadFirmware_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutcome)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox output_screen;
        private System.Windows.Forms.Button buttonNetworkManager;
        private System.Windows.Forms.Button buttonMote;
        private System.Windows.Forms.Button buttonAccessPoint;
        private System.Windows.Forms.Label labelStopWatchTimer;
        private System.Windows.Forms.PictureBox pictureBoxOutcome;
        private System.Windows.Forms.Timer stopWatchTimer;
        private System.Windows.Forms.Label labelOutputScreenCharactersCount;
    }
}