namespace Network_Manager_GUI
{
    partial class Network_Manager_GUI
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
            this.input_screen = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.portSettingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.firmwareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFirmwareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.networkStatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.networkViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureLoggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperaturePlotterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oscilloscopeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.status_system_status = new System.Windows.Forms.ToolStripLabel();
            this.buttonSendData = new System.Windows.Forms.Button();
            this.buttonClearInputBox = new System.Windows.Forms.Button();
            this.buttonClearOutputBox = new System.Windows.Forms.Button();
            this.output_screen = new System.Windows.Forms.RichTextBox();
            this.groupBoxTasks = new System.Windows.Forms.GroupBox();
            this.labelNetworkIdCharactersCountNetworkManager = new System.Windows.Forms.Label();
            this.buttonSetJoinKeyNetworkManager = new System.Windows.Forms.Button();
            this.buttonGetNetworkIdNetworkManager = new System.Windows.Forms.Button();
            this.buttonSetNetworkIdNetworkManager = new System.Windows.Forms.Button();
            this.labelJoinKeyCharactersCountNetworkManager = new System.Windows.Forms.Label();
            this.labelJoinKeyNetworkManager = new System.Windows.Forms.Label();
            this.textBoxJoinKeyNetworkManager = new System.Windows.Forms.TextBox();
            this.labelNetworkIdNetworkManager = new System.Windows.Forms.Label();
            this.textBoxNetworkIdNetworkManager = new System.Windows.Forms.TextBox();
            this.comboBoxRadiotestNetworkManager = new System.Windows.Forms.ComboBox();
            this.labelRadiotestNetworkManager = new System.Windows.Forms.Label();
            this.buttonFactoryResetNetworkManager = new System.Windows.Forms.Button();
            this.buttonRestartNetworkManager = new System.Windows.Forms.Button();
            this.labelComPortBox = new System.Windows.Forms.Label();
            this.combo_PortNetworkManager = new System.Windows.Forms.ComboBox();
            this.buttonConnectionNetworkManager = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.labelInputScreen = new System.Windows.Forms.Label();
            this.labelOutputScreen = new System.Windows.Forms.Label();
            this.groupBoxTaskOutcome = new System.Windows.Forms.GroupBox();
            this.labelStopWatchTimer = new System.Windows.Forms.Label();
            this.textBoxTaskOutcomeMessage = new System.Windows.Forms.TextBox();
            this.labelTaskOutcomeInfo = new System.Windows.Forms.Label();
            this.pictureBoxOutcome = new System.Windows.Forms.PictureBox();
            this.textBoxTaskOutcomeResult = new System.Windows.Forms.TextBox();
            this.textBoxTaskOutcomeStatus = new System.Windows.Forms.TextBox();
            this.textBoxTaskOutcomeName = new System.Windows.Forms.TextBox();
            this.labelTaskOutcomeResult = new System.Windows.Forms.Label();
            this.labelTaskOutcomeStatus = new System.Windows.Forms.Label();
            this.labelTaskOutcomeName = new System.Windows.Forms.Label();
            this.comboBoxDeviceType = new System.Windows.Forms.ComboBox();
            this.labelDeviceType = new System.Windows.Forms.Label();
            this.groupBoxTasksMote = new System.Windows.Forms.GroupBox();
            this.labelNetworkIdCharactersCountMote = new System.Windows.Forms.Label();
            this.buttonSetJoinKeyMote = new System.Windows.Forms.Button();
            this.buttonGetNetworkIdMote = new System.Windows.Forms.Button();
            this.buttonSetNetworkIdMote = new System.Windows.Forms.Button();
            this.labelJoinKeyCharactersCountMote = new System.Windows.Forms.Label();
            this.labelJoinKeyMote = new System.Windows.Forms.Label();
            this.textBoxJoinKeyMote = new System.Windows.Forms.TextBox();
            this.labelNetworkIdMote = new System.Windows.Forms.Label();
            this.textBoxNetworkIdMote = new System.Windows.Forms.TextBox();
            this.comboBoxRadiotestMote = new System.Windows.Forms.ComboBox();
            this.labelRadiotestMote = new System.Windows.Forms.Label();
            this.comboBoxAutoJoinMote = new System.Windows.Forms.ComboBox();
            this.labelAutoJoinMote = new System.Windows.Forms.Label();
            this.comboBoxModeMote = new System.Windows.Forms.ComboBox();
            this.labelModeMote = new System.Windows.Forms.Label();
            this.buttonFactoryResetMote = new System.Windows.Forms.Button();
            this.buttonRestartMote = new System.Windows.Forms.Button();
            this.labelComPortBoxMote = new System.Windows.Forms.Label();
            this.combo_PortMote = new System.Windows.Forms.ComboBox();
            this.buttonConnectionMote = new System.Windows.Forms.Button();
            this.stopWatchTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBoxDeviceType = new System.Windows.Forms.GroupBox();
            this.labelInputScreenCharactersCount = new System.Windows.Forms.Label();
            this.labelOutputScreenCharactersCount = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            this.groupBoxTasks.SuspendLayout();
            this.groupBoxTaskOutcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutcome)).BeginInit();
            this.groupBoxTasksMote.SuspendLayout();
            this.groupBoxDeviceType.SuspendLayout();
            this.SuspendLayout();
            // 
            // input_screen
            // 
            this.input_screen.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.input_screen.Location = new System.Drawing.Point(5, 54);
            this.input_screen.Name = "input_screen";
            this.input_screen.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.input_screen.Size = new System.Drawing.Size(280, 24);
            this.input_screen.TabIndex = 0;
            this.input_screen.TextChanged += new System.EventHandler(this.Input_screen_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.menuStrip1.ForeColor = System.Drawing.Color.White;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.portSettingsToolStripMenuItem1,
            this.firmwareToolStripMenuItem,
            this.networkStatisticsToolStripMenuItem,
            this.networkViewToolStripMenuItem,
            this.applicationsToolStripMenuItem,
            this.disconnectDevicesToolStripMenuItem,
            this.status_system_status});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.ShowItemToolTips = true;
            this.menuStrip1.Size = new System.Drawing.Size(1160, 30);
            this.menuStrip1.TabIndex = 88;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // portSettingsToolStripMenuItem1
            // 
            this.portSettingsToolStripMenuItem1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portSettingsToolStripMenuItem1.Name = "portSettingsToolStripMenuItem1";
            this.portSettingsToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+Shift+Z";
            this.portSettingsToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
            this.portSettingsToolStripMenuItem1.Size = new System.Drawing.Size(172, 26);
            this.portSettingsToolStripMenuItem1.Text = "Serial Port Settings";
            this.portSettingsToolStripMenuItem1.Click += new System.EventHandler(this.PortSettingsToolStripMenuItem1_Click);
            // 
            // firmwareToolStripMenuItem
            // 
            this.firmwareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFirmwareToolStripMenuItem});
            this.firmwareToolStripMenuItem.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firmwareToolStripMenuItem.Name = "firmwareToolStripMenuItem";
            this.firmwareToolStripMenuItem.Size = new System.Drawing.Size(98, 26);
            this.firmwareToolStripMenuItem.Text = "Firmware";
            // 
            // loadFirmwareToolStripMenuItem
            // 
            this.loadFirmwareToolStripMenuItem.Name = "loadFirmwareToolStripMenuItem";
            this.loadFirmwareToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.loadFirmwareToolStripMenuItem.Text = "Load Firmware";
            this.loadFirmwareToolStripMenuItem.Click += new System.EventHandler(this.LoadFirmwareToolStripMenuItem_Click);
            // 
            // networkStatisticsToolStripMenuItem
            // 
            this.networkStatisticsToolStripMenuItem.Font = new System.Drawing.Font("Garamond", 12F);
            this.networkStatisticsToolStripMenuItem.Name = "networkStatisticsToolStripMenuItem";
            this.networkStatisticsToolStripMenuItem.Size = new System.Drawing.Size(165, 26);
            this.networkStatisticsToolStripMenuItem.Text = "Network Statistics";
            this.networkStatisticsToolStripMenuItem.Click += new System.EventHandler(this.NetworkStatisticsToolStripMenuItem_Click);
            // 
            // networkViewToolStripMenuItem
            // 
            this.networkViewToolStripMenuItem.Font = new System.Drawing.Font("Garamond", 12F);
            this.networkViewToolStripMenuItem.Name = "networkViewToolStripMenuItem";
            this.networkViewToolStripMenuItem.Size = new System.Drawing.Size(171, 26);
            this.networkViewToolStripMenuItem.Text = "Network Topology";
            this.networkViewToolStripMenuItem.Click += new System.EventHandler(this.NetworkTopologyToolStripMenuItem_Click);
            // 
            // applicationsToolStripMenuItem
            // 
            this.applicationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.temperatureLoggerToolStripMenuItem,
            this.temperaturePlotterToolStripMenuItem,
            this.oscilloscopeToolStripMenuItem});
            this.applicationsToolStripMenuItem.Font = new System.Drawing.Font("Garamond", 12F);
            this.applicationsToolStripMenuItem.Name = "applicationsToolStripMenuItem";
            this.applicationsToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.applicationsToolStripMenuItem.Text = "Applications";
            // 
            // temperatureLoggerToolStripMenuItem
            // 
            this.temperatureLoggerToolStripMenuItem.Name = "temperatureLoggerToolStripMenuItem";
            this.temperatureLoggerToolStripMenuItem.Size = new System.Drawing.Size(321, 26);
            this.temperatureLoggerToolStripMenuItem.Text = "Internal Temperature Sensor";
            this.temperatureLoggerToolStripMenuItem.Click += new System.EventHandler(this.TemperatureLoggerToolStripMenuItem_Click);
            // 
            // temperaturePlotterToolStripMenuItem
            // 
            this.temperaturePlotterToolStripMenuItem.Name = "temperaturePlotterToolStripMenuItem";
            this.temperaturePlotterToolStripMenuItem.Size = new System.Drawing.Size(321, 26);
            this.temperaturePlotterToolStripMenuItem.Text = "External Temperature Sensor";
            this.temperaturePlotterToolStripMenuItem.Click += new System.EventHandler(this.TemperaturePlotterToolStripMenuItem_Click);
            // 
            // oscilloscopeToolStripMenuItem
            // 
            this.oscilloscopeToolStripMenuItem.Name = "oscilloscopeToolStripMenuItem";
            this.oscilloscopeToolStripMenuItem.Size = new System.Drawing.Size(321, 26);
            this.oscilloscopeToolStripMenuItem.Text = "Oscilloscope";
            this.oscilloscopeToolStripMenuItem.Click += new System.EventHandler(this.OscilloscopeToolStripMenuItem_Click);
            // 
            // disconnectDevicesToolStripMenuItem
            // 
            this.disconnectDevicesToolStripMenuItem.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disconnectDevicesToolStripMenuItem.Name = "disconnectDevicesToolStripMenuItem";
            this.disconnectDevicesToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.disconnectDevicesToolStripMenuItem.Text = "Disconnect Devices";
            this.disconnectDevicesToolStripMenuItem.Click += new System.EventHandler(this.DisconnectDevicesToolStripMenuItem_Click);
            // 
            // status_system_status
            // 
            this.status_system_status.BackColor = System.Drawing.Color.Red;
            this.status_system_status.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status_system_status.ForeColor = System.Drawing.Color.Red;
            this.status_system_status.Name = "status_system_status";
            this.status_system_status.Size = new System.Drawing.Size(143, 23);
            this.status_system_status.Text = "Not Connected!";
            // 
            // buttonSendData
            // 
            this.buttonSendData.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSendData.Location = new System.Drawing.Point(228, 98);
            this.buttonSendData.Name = "buttonSendData";
            this.buttonSendData.Size = new System.Drawing.Size(120, 38);
            this.buttonSendData.TabIndex = 1;
            this.buttonSendData.Text = "Send Data";
            this.buttonSendData.UseVisualStyleBackColor = true;
            this.buttonSendData.Click += new System.EventHandler(this.ButtonSendData_Click);
            // 
            // buttonClearInputBox
            // 
            this.buttonClearInputBox.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClearInputBox.Location = new System.Drawing.Point(291, 44);
            this.buttonClearInputBox.Name = "buttonClearInputBox";
            this.buttonClearInputBox.Size = new System.Drawing.Size(134, 38);
            this.buttonClearInputBox.TabIndex = 90;
            this.buttonClearInputBox.Text = "Clear Input Screen";
            this.buttonClearInputBox.UseVisualStyleBackColor = true;
            this.buttonClearInputBox.Click += new System.EventHandler(this.ButtonClearInputBox_Click);
            // 
            // buttonClearOutputBox
            // 
            this.buttonClearOutputBox.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClearOutputBox.Location = new System.Drawing.Point(1010, 32);
            this.buttonClearOutputBox.Name = "buttonClearOutputBox";
            this.buttonClearOutputBox.Size = new System.Drawing.Size(134, 38);
            this.buttonClearOutputBox.TabIndex = 91;
            this.buttonClearOutputBox.Text = "Clear Output Screen";
            this.buttonClearOutputBox.UseVisualStyleBackColor = true;
            this.buttonClearOutputBox.Click += new System.EventHandler(this.ButtonClearOutputBox_Click);
            // 
            // output_screen
            // 
            this.output_screen.BackColor = System.Drawing.Color.Black;
            this.output_screen.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output_screen.ForeColor = System.Drawing.Color.White;
            this.output_screen.Location = new System.Drawing.Point(469, 70);
            this.output_screen.Name = "output_screen";
            this.output_screen.ReadOnly = true;
            this.output_screen.Size = new System.Drawing.Size(675, 281);
            this.output_screen.TabIndex = 92;
            this.output_screen.Text = "";
            this.output_screen.TextChanged += new System.EventHandler(this.Output_screen_TextChanged);
            // 
            // groupBoxTasks
            // 
            this.groupBoxTasks.Controls.Add(this.labelNetworkIdCharactersCountNetworkManager);
            this.groupBoxTasks.Controls.Add(this.buttonSetJoinKeyNetworkManager);
            this.groupBoxTasks.Controls.Add(this.buttonGetNetworkIdNetworkManager);
            this.groupBoxTasks.Controls.Add(this.buttonSetNetworkIdNetworkManager);
            this.groupBoxTasks.Controls.Add(this.labelJoinKeyCharactersCountNetworkManager);
            this.groupBoxTasks.Controls.Add(this.labelJoinKeyNetworkManager);
            this.groupBoxTasks.Controls.Add(this.textBoxJoinKeyNetworkManager);
            this.groupBoxTasks.Controls.Add(this.labelNetworkIdNetworkManager);
            this.groupBoxTasks.Controls.Add(this.textBoxNetworkIdNetworkManager);
            this.groupBoxTasks.Controls.Add(this.comboBoxRadiotestNetworkManager);
            this.groupBoxTasks.Controls.Add(this.labelRadiotestNetworkManager);
            this.groupBoxTasks.Controls.Add(this.buttonFactoryResetNetworkManager);
            this.groupBoxTasks.Controls.Add(this.buttonRestartNetworkManager);
            this.groupBoxTasks.Controls.Add(this.labelComPortBox);
            this.groupBoxTasks.Controls.Add(this.combo_PortNetworkManager);
            this.groupBoxTasks.Controls.Add(this.buttonConnectionNetworkManager);
            this.groupBoxTasks.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxTasks.Location = new System.Drawing.Point(0, 173);
            this.groupBoxTasks.Name = "groupBoxTasks";
            this.groupBoxTasks.Size = new System.Drawing.Size(456, 196);
            this.groupBoxTasks.TabIndex = 93;
            this.groupBoxTasks.TabStop = false;
            this.groupBoxTasks.Text = "Network Manager";
            // 
            // labelNetworkIdCharactersCountNetworkManager
            // 
            this.labelNetworkIdCharactersCountNetworkManager.AutoSize = true;
            this.labelNetworkIdCharactersCountNetworkManager.Font = new System.Drawing.Font("Garamond", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNetworkIdCharactersCountNetworkManager.Location = new System.Drawing.Point(129, 138);
            this.labelNetworkIdCharactersCountNetworkManager.Name = "labelNetworkIdCharactersCountNetworkManager";
            this.labelNetworkIdCharactersCountNetworkManager.Size = new System.Drawing.Size(91, 16);
            this.labelNetworkIdCharactersCountNetworkManager.TabIndex = 108;
            this.labelNetworkIdCharactersCountNetworkManager.Text = "0 character(s)";
            // 
            // buttonSetJoinKeyNetworkManager
            // 
            this.buttonSetJoinKeyNetworkManager.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetJoinKeyNetworkManager.Location = new System.Drawing.Point(348, 157);
            this.buttonSetJoinKeyNetworkManager.Name = "buttonSetJoinKeyNetworkManager";
            this.buttonSetJoinKeyNetworkManager.Size = new System.Drawing.Size(94, 26);
            this.buttonSetJoinKeyNetworkManager.TabIndex = 107;
            this.buttonSetJoinKeyNetworkManager.Text = "Set";
            this.buttonSetJoinKeyNetworkManager.UseVisualStyleBackColor = true;
            this.buttonSetJoinKeyNetworkManager.Click += new System.EventHandler(this.ButtonSetJoinKeyNetworkManager_Click);
            // 
            // buttonGetNetworkIdNetworkManager
            // 
            this.buttonGetNetworkIdNetworkManager.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetNetworkIdNetworkManager.Location = new System.Drawing.Point(177, 157);
            this.buttonGetNetworkIdNetworkManager.Name = "buttonGetNetworkIdNetworkManager";
            this.buttonGetNetworkIdNetworkManager.Size = new System.Drawing.Size(54, 26);
            this.buttonGetNetworkIdNetworkManager.TabIndex = 106;
            this.buttonGetNetworkIdNetworkManager.Text = "Get";
            this.buttonGetNetworkIdNetworkManager.UseVisualStyleBackColor = true;
            this.buttonGetNetworkIdNetworkManager.Click += new System.EventHandler(this.ButtonGetNetworkIdNetworkManager_Click);
            // 
            // buttonSetNetworkIdNetworkManager
            // 
            this.buttonSetNetworkIdNetworkManager.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetNetworkIdNetworkManager.Location = new System.Drawing.Point(130, 157);
            this.buttonSetNetworkIdNetworkManager.Name = "buttonSetNetworkIdNetworkManager";
            this.buttonSetNetworkIdNetworkManager.Size = new System.Drawing.Size(43, 26);
            this.buttonSetNetworkIdNetworkManager.TabIndex = 105;
            this.buttonSetNetworkIdNetworkManager.Text = "Set";
            this.buttonSetNetworkIdNetworkManager.UseVisualStyleBackColor = true;
            this.buttonSetNetworkIdNetworkManager.Click += new System.EventHandler(this.ButtonSetNetworkIdNetworkManager_Click);
            // 
            // labelJoinKeyCharactersCountNetworkManager
            // 
            this.labelJoinKeyCharactersCountNetworkManager.AutoSize = true;
            this.labelJoinKeyCharactersCountNetworkManager.Font = new System.Drawing.Font("Garamond", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelJoinKeyCharactersCountNetworkManager.Location = new System.Drawing.Point(346, 138);
            this.labelJoinKeyCharactersCountNetworkManager.Name = "labelJoinKeyCharactersCountNetworkManager";
            this.labelJoinKeyCharactersCountNetworkManager.Size = new System.Drawing.Size(91, 16);
            this.labelJoinKeyCharactersCountNetworkManager.TabIndex = 104;
            this.labelJoinKeyCharactersCountNetworkManager.Text = "0 character(s)";
            // 
            // labelJoinKeyNetworkManager
            // 
            this.labelJoinKeyNetworkManager.AutoSize = true;
            this.labelJoinKeyNetworkManager.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelJoinKeyNetworkManager.Location = new System.Drawing.Point(262, 83);
            this.labelJoinKeyNetworkManager.Name = "labelJoinKeyNetworkManager";
            this.labelJoinKeyNetworkManager.Size = new System.Drawing.Size(87, 22);
            this.labelJoinKeyNetworkManager.TabIndex = 103;
            this.labelJoinKeyNetworkManager.Text = "Join Key:";
            // 
            // textBoxJoinKeyNetworkManager
            // 
            this.textBoxJoinKeyNetworkManager.Font = new System.Drawing.Font("Garamond", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxJoinKeyNetworkManager.Location = new System.Drawing.Point(349, 80);
            this.textBoxJoinKeyNetworkManager.Multiline = true;
            this.textBoxJoinKeyNetworkManager.Name = "textBoxJoinKeyNetworkManager";
            this.textBoxJoinKeyNetworkManager.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxJoinKeyNetworkManager.Size = new System.Drawing.Size(99, 58);
            this.textBoxJoinKeyNetworkManager.TabIndex = 102;
            this.textBoxJoinKeyNetworkManager.TextChanged += new System.EventHandler(this.TextBoxJoinKeyNetworkManager_TextChanged);
            // 
            // labelNetworkIdNetworkManager
            // 
            this.labelNetworkIdNetworkManager.AutoSize = true;
            this.labelNetworkIdNetworkManager.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNetworkIdNetworkManager.Location = new System.Drawing.Point(11, 116);
            this.labelNetworkIdNetworkManager.Name = "labelNetworkIdNetworkManager";
            this.labelNetworkIdNetworkManager.Size = new System.Drawing.Size(118, 22);
            this.labelNetworkIdNetworkManager.TabIndex = 101;
            this.labelNetworkIdNetworkManager.Text = "Network ID:";
            this.labelNetworkIdNetworkManager.Click += new System.EventHandler(this.labelNetworkIdNetworkManager_Click);
            // 
            // textBoxNetworkIdNetworkManager
            // 
            this.textBoxNetworkIdNetworkManager.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNetworkIdNetworkManager.Location = new System.Drawing.Point(133, 111);
            this.textBoxNetworkIdNetworkManager.Name = "textBoxNetworkIdNetworkManager";
            this.textBoxNetworkIdNetworkManager.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxNetworkIdNetworkManager.Size = new System.Drawing.Size(90, 29);
            this.textBoxNetworkIdNetworkManager.TabIndex = 100;
            this.textBoxNetworkIdNetworkManager.TextChanged += new System.EventHandler(this.TextBoxNetworkIdNetworkManager_TextChanged);
            // 
            // comboBoxRadiotestNetworkManager
            // 
            this.comboBoxRadiotestNetworkManager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRadiotestNetworkManager.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxRadiotestNetworkManager.FormattingEnabled = true;
            this.comboBoxRadiotestNetworkManager.Items.AddRange(new object[] {
            "ON",
            "OFF"});
            this.comboBoxRadiotestNetworkManager.Location = new System.Drawing.Point(133, 75);
            this.comboBoxRadiotestNetworkManager.Name = "comboBoxRadiotestNetworkManager";
            this.comboBoxRadiotestNetworkManager.Size = new System.Drawing.Size(119, 30);
            this.comboBoxRadiotestNetworkManager.TabIndex = 66;
            this.comboBoxRadiotestNetworkManager.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxRadiotestNetworkManager_SelectionChangeCommitted);
            // 
            // labelRadiotestNetworkManager
            // 
            this.labelRadiotestNetworkManager.AutoSize = true;
            this.labelRadiotestNetworkManager.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRadiotestNetworkManager.Location = new System.Drawing.Point(34, 83);
            this.labelRadiotestNetworkManager.Name = "labelRadiotestNetworkManager";
            this.labelRadiotestNetworkManager.Size = new System.Drawing.Size(95, 22);
            this.labelRadiotestNetworkManager.TabIndex = 65;
            this.labelRadiotestNetworkManager.Text = "Radiotest:";
            // 
            // buttonFactoryResetNetworkManager
            // 
            this.buttonFactoryResetNetworkManager.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFactoryResetNetworkManager.Location = new System.Drawing.Point(348, 42);
            this.buttonFactoryResetNetworkManager.Name = "buttonFactoryResetNetworkManager";
            this.buttonFactoryResetNetworkManager.Size = new System.Drawing.Size(102, 27);
            this.buttonFactoryResetNetworkManager.TabIndex = 60;
            this.buttonFactoryResetNetworkManager.Text = "Factory Reset";
            this.buttonFactoryResetNetworkManager.UseVisualStyleBackColor = true;
            this.buttonFactoryResetNetworkManager.Click += new System.EventHandler(this.ButtonFactoryReset_Click);
            // 
            // buttonRestartNetworkManager
            // 
            this.buttonRestartNetworkManager.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestartNetworkManager.Location = new System.Drawing.Point(259, 42);
            this.buttonRestartNetworkManager.Name = "buttonRestartNetworkManager";
            this.buttonRestartNetworkManager.Size = new System.Drawing.Size(87, 26);
            this.buttonRestartNetworkManager.TabIndex = 59;
            this.buttonRestartNetworkManager.Text = "Restart";
            this.buttonRestartNetworkManager.UseVisualStyleBackColor = true;
            this.buttonRestartNetworkManager.Click += new System.EventHandler(this.ButtonRestart_Click);
            // 
            // labelComPortBox
            // 
            this.labelComPortBox.AutoSize = true;
            this.labelComPortBox.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelComPortBox.Location = new System.Drawing.Point(129, 23);
            this.labelComPortBox.Name = "labelComPortBox";
            this.labelComPortBox.Size = new System.Drawing.Size(127, 21);
            this.labelComPortBox.TabIndex = 58;
            this.labelComPortBox.Text = "COM Port Box";
            // 
            // combo_PortNetworkManager
            // 
            this.combo_PortNetworkManager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_PortNetworkManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_PortNetworkManager.FormattingEnabled = true;
            this.combo_PortNetworkManager.Location = new System.Drawing.Point(133, 40);
            this.combo_PortNetworkManager.Name = "combo_PortNetworkManager";
            this.combo_PortNetworkManager.Size = new System.Drawing.Size(101, 28);
            this.combo_PortNetworkManager.TabIndex = 57;
            this.combo_PortNetworkManager.SelectionChangeCommitted += new System.EventHandler(this.Combo_Port_SelectionChangeCommitted);
            this.combo_PortNetworkManager.Click += new System.EventHandler(this.Combo_Port_Click);
            // 
            // buttonConnectionNetworkManager
            // 
            this.buttonConnectionNetworkManager.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnectionNetworkManager.Location = new System.Drawing.Point(11, 43);
            this.buttonConnectionNetworkManager.Name = "buttonConnectionNetworkManager";
            this.buttonConnectionNetworkManager.Size = new System.Drawing.Size(116, 26);
            this.buttonConnectionNetworkManager.TabIndex = 0;
            this.buttonConnectionNetworkManager.Text = "Connection";
            this.buttonConnectionNetworkManager.UseVisualStyleBackColor = true;
            this.buttonConnectionNetworkManager.Click += new System.EventHandler(this.ButtonConnection_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM12";
            // 
            // labelInputScreen
            // 
            this.labelInputScreen.AutoSize = true;
            this.labelInputScreen.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInputScreen.ForeColor = System.Drawing.Color.Black;
            this.labelInputScreen.Location = new System.Drawing.Point(1, 29);
            this.labelInputScreen.Name = "labelInputScreen";
            this.labelInputScreen.Size = new System.Drawing.Size(117, 22);
            this.labelInputScreen.TabIndex = 94;
            this.labelInputScreen.Text = "Input Screen";
            // 
            // labelOutputScreen
            // 
            this.labelOutputScreen.AutoSize = true;
            this.labelOutputScreen.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputScreen.ForeColor = System.Drawing.Color.Black;
            this.labelOutputScreen.Location = new System.Drawing.Point(465, 34);
            this.labelOutputScreen.Name = "labelOutputScreen";
            this.labelOutputScreen.Size = new System.Drawing.Size(131, 22);
            this.labelOutputScreen.TabIndex = 95;
            this.labelOutputScreen.Text = "Output Screen";
            this.labelOutputScreen.Click += new System.EventHandler(this.labelOutputScreen_Click);
            // 
            // groupBoxTaskOutcome
            // 
            this.groupBoxTaskOutcome.BackColor = System.Drawing.Color.Black;
            this.groupBoxTaskOutcome.Controls.Add(this.labelStopWatchTimer);
            this.groupBoxTaskOutcome.Controls.Add(this.textBoxTaskOutcomeMessage);
            this.groupBoxTaskOutcome.Controls.Add(this.labelTaskOutcomeInfo);
            this.groupBoxTaskOutcome.Controls.Add(this.pictureBoxOutcome);
            this.groupBoxTaskOutcome.Controls.Add(this.textBoxTaskOutcomeResult);
            this.groupBoxTaskOutcome.Controls.Add(this.textBoxTaskOutcomeStatus);
            this.groupBoxTaskOutcome.Controls.Add(this.textBoxTaskOutcomeName);
            this.groupBoxTaskOutcome.Controls.Add(this.labelTaskOutcomeResult);
            this.groupBoxTaskOutcome.Controls.Add(this.labelTaskOutcomeStatus);
            this.groupBoxTaskOutcome.Controls.Add(this.labelTaskOutcomeName);
            this.groupBoxTaskOutcome.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxTaskOutcome.ForeColor = System.Drawing.Color.White;
            this.groupBoxTaskOutcome.Location = new System.Drawing.Point(751, 355);
            this.groupBoxTaskOutcome.Name = "groupBoxTaskOutcome";
            this.groupBoxTaskOutcome.Size = new System.Drawing.Size(393, 230);
            this.groupBoxTaskOutcome.TabIndex = 96;
            this.groupBoxTaskOutcome.TabStop = false;
            this.groupBoxTaskOutcome.Text = "Task Outcome";
            // 
            // labelStopWatchTimer
            // 
            this.labelStopWatchTimer.AutoSize = true;
            this.labelStopWatchTimer.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStopWatchTimer.Location = new System.Drawing.Point(342, 202);
            this.labelStopWatchTimer.Name = "labelStopWatchTimer";
            this.labelStopWatchTimer.Size = new System.Drawing.Size(41, 22);
            this.labelStopWatchTimer.TabIndex = 98;
            this.labelStopWatchTimer.Text = "-:-:-";
            this.labelStopWatchTimer.Visible = false;
            // 
            // textBoxTaskOutcomeMessage
            // 
            this.textBoxTaskOutcomeMessage.Font = new System.Drawing.Font("Garamond", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTaskOutcomeMessage.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.textBoxTaskOutcomeMessage.Location = new System.Drawing.Point(6, 136);
            this.textBoxTaskOutcomeMessage.Multiline = true;
            this.textBoxTaskOutcomeMessage.Name = "textBoxTaskOutcomeMessage";
            this.textBoxTaskOutcomeMessage.ReadOnly = true;
            this.textBoxTaskOutcomeMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTaskOutcomeMessage.Size = new System.Drawing.Size(319, 87);
            this.textBoxTaskOutcomeMessage.TabIndex = 8;
            // 
            // labelTaskOutcomeInfo
            // 
            this.labelTaskOutcomeInfo.AutoSize = true;
            this.labelTaskOutcomeInfo.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTaskOutcomeInfo.Location = new System.Drawing.Point(5, 115);
            this.labelTaskOutcomeInfo.Name = "labelTaskOutcomeInfo";
            this.labelTaskOutcomeInfo.Size = new System.Drawing.Size(128, 22);
            this.labelTaskOutcomeInfo.TabIndex = 7;
            this.labelTaskOutcomeInfo.Text = "Info Message:";
            // 
            // pictureBoxOutcome
            // 
            this.pictureBoxOutcome.Location = new System.Drawing.Point(338, 160);
            this.pictureBoxOutcome.Name = "pictureBoxOutcome";
            this.pictureBoxOutcome.Size = new System.Drawing.Size(47, 38);
            this.pictureBoxOutcome.TabIndex = 6;
            this.pictureBoxOutcome.TabStop = false;
            // 
            // textBoxTaskOutcomeResult
            // 
            this.textBoxTaskOutcomeResult.Font = new System.Drawing.Font("Garamond", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTaskOutcomeResult.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.textBoxTaskOutcomeResult.Location = new System.Drawing.Point(67, 82);
            this.textBoxTaskOutcomeResult.Name = "textBoxTaskOutcomeResult";
            this.textBoxTaskOutcomeResult.ReadOnly = true;
            this.textBoxTaskOutcomeResult.Size = new System.Drawing.Size(300, 29);
            this.textBoxTaskOutcomeResult.TabIndex = 5;
            // 
            // textBoxTaskOutcomeStatus
            // 
            this.textBoxTaskOutcomeStatus.Font = new System.Drawing.Font("Garamond", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTaskOutcomeStatus.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.textBoxTaskOutcomeStatus.Location = new System.Drawing.Point(67, 52);
            this.textBoxTaskOutcomeStatus.Name = "textBoxTaskOutcomeStatus";
            this.textBoxTaskOutcomeStatus.ReadOnly = true;
            this.textBoxTaskOutcomeStatus.Size = new System.Drawing.Size(300, 29);
            this.textBoxTaskOutcomeStatus.TabIndex = 4;
            // 
            // textBoxTaskOutcomeName
            // 
            this.textBoxTaskOutcomeName.Font = new System.Drawing.Font("Garamond", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTaskOutcomeName.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.textBoxTaskOutcomeName.Location = new System.Drawing.Point(67, 22);
            this.textBoxTaskOutcomeName.Name = "textBoxTaskOutcomeName";
            this.textBoxTaskOutcomeName.ReadOnly = true;
            this.textBoxTaskOutcomeName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTaskOutcomeName.Size = new System.Drawing.Size(300, 29);
            this.textBoxTaskOutcomeName.TabIndex = 3;
            // 
            // labelTaskOutcomeResult
            // 
            this.labelTaskOutcomeResult.AutoSize = true;
            this.labelTaskOutcomeResult.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTaskOutcomeResult.Location = new System.Drawing.Point(4, 88);
            this.labelTaskOutcomeResult.Name = "labelTaskOutcomeResult";
            this.labelTaskOutcomeResult.Size = new System.Drawing.Size(69, 22);
            this.labelTaskOutcomeResult.TabIndex = 2;
            this.labelTaskOutcomeResult.Text = "Result:";
            // 
            // labelTaskOutcomeStatus
            // 
            this.labelTaskOutcomeStatus.AutoSize = true;
            this.labelTaskOutcomeStatus.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTaskOutcomeStatus.Location = new System.Drawing.Point(4, 58);
            this.labelTaskOutcomeStatus.Name = "labelTaskOutcomeStatus";
            this.labelTaskOutcomeStatus.Size = new System.Drawing.Size(66, 22);
            this.labelTaskOutcomeStatus.TabIndex = 1;
            this.labelTaskOutcomeStatus.Text = "Status:";
            // 
            // labelTaskOutcomeName
            // 
            this.labelTaskOutcomeName.AutoSize = true;
            this.labelTaskOutcomeName.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTaskOutcomeName.Location = new System.Drawing.Point(4, 26);
            this.labelTaskOutcomeName.Name = "labelTaskOutcomeName";
            this.labelTaskOutcomeName.Size = new System.Drawing.Size(67, 22);
            this.labelTaskOutcomeName.TabIndex = 0;
            this.labelTaskOutcomeName.Text = "Name:";
            this.labelTaskOutcomeName.Click += new System.EventHandler(this.labelTaskOutcomeName_Click);
            // 
            // comboBoxDeviceType
            // 
            this.comboBoxDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDeviceType.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDeviceType.FormattingEnabled = true;
            this.comboBoxDeviceType.Items.AddRange(new object[] {
            "NETWORK_MANAGER",
            "MOTE"});
            this.comboBoxDeviceType.Location = new System.Drawing.Point(8, 22);
            this.comboBoxDeviceType.Name = "comboBoxDeviceType";
            this.comboBoxDeviceType.Size = new System.Drawing.Size(188, 26);
            this.comboBoxDeviceType.TabIndex = 58;
            this.comboBoxDeviceType.SelectedIndexChanged += new System.EventHandler(this.comboBoxDeviceType_SelectedIndexChanged);
            this.comboBoxDeviceType.SelectedValueChanged += new System.EventHandler(this.ComboBoxDeviceType_SelectedValueChanged);
            // 
            // labelDeviceType
            // 
            this.labelDeviceType.AutoSize = true;
            this.labelDeviceType.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeviceType.Location = new System.Drawing.Point(42, 471);
            this.labelDeviceType.Name = "labelDeviceType";
            this.labelDeviceType.Size = new System.Drawing.Size(0, 22);
            this.labelDeviceType.TabIndex = 9;
            // 
            // groupBoxTasksMote
            // 
            this.groupBoxTasksMote.Controls.Add(this.comboBox1);
            this.groupBoxTasksMote.Controls.Add(this.button1);
            this.groupBoxTasksMote.Controls.Add(this.labelNetworkIdCharactersCountMote);
            this.groupBoxTasksMote.Controls.Add(this.buttonSetJoinKeyMote);
            this.groupBoxTasksMote.Controls.Add(this.buttonGetNetworkIdMote);
            this.groupBoxTasksMote.Controls.Add(this.buttonSetNetworkIdMote);
            this.groupBoxTasksMote.Controls.Add(this.labelJoinKeyCharactersCountMote);
            this.groupBoxTasksMote.Controls.Add(this.labelJoinKeyMote);
            this.groupBoxTasksMote.Controls.Add(this.textBoxJoinKeyMote);
            this.groupBoxTasksMote.Controls.Add(this.labelNetworkIdMote);
            this.groupBoxTasksMote.Controls.Add(this.textBoxNetworkIdMote);
            this.groupBoxTasksMote.Controls.Add(this.comboBoxRadiotestMote);
            this.groupBoxTasksMote.Controls.Add(this.labelRadiotestMote);
            this.groupBoxTasksMote.Controls.Add(this.comboBoxAutoJoinMote);
            this.groupBoxTasksMote.Controls.Add(this.labelAutoJoinMote);
            this.groupBoxTasksMote.Controls.Add(this.comboBoxModeMote);
            this.groupBoxTasksMote.Controls.Add(this.labelModeMote);
            this.groupBoxTasksMote.Controls.Add(this.buttonFactoryResetMote);
            this.groupBoxTasksMote.Controls.Add(this.buttonRestartMote);
            this.groupBoxTasksMote.Controls.Add(this.labelComPortBoxMote);
            this.groupBoxTasksMote.Controls.Add(this.combo_PortMote);
            this.groupBoxTasksMote.Controls.Add(this.buttonConnectionMote);
            this.groupBoxTasksMote.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxTasksMote.Location = new System.Drawing.Point(0, 384);
            this.groupBoxTasksMote.Name = "groupBoxTasksMote";
            this.groupBoxTasksMote.Size = new System.Drawing.Size(706, 201);
            this.groupBoxTasksMote.TabIndex = 97;
            this.groupBoxTasksMote.TabStop = false;
            this.groupBoxTasksMote.Text = "Mote";
            this.groupBoxTasksMote.Enter += new System.EventHandler(this.groupBoxTasksMote_Enter);
            // 
            // labelNetworkIdCharactersCountMote
            // 
            this.labelNetworkIdCharactersCountMote.AutoSize = true;
            this.labelNetworkIdCharactersCountMote.Font = new System.Drawing.Font("Garamond", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNetworkIdCharactersCountMote.Location = new System.Drawing.Point(10, 156);
            this.labelNetworkIdCharactersCountMote.Name = "labelNetworkIdCharactersCountMote";
            this.labelNetworkIdCharactersCountMote.Size = new System.Drawing.Size(91, 16);
            this.labelNetworkIdCharactersCountMote.TabIndex = 110;
            this.labelNetworkIdCharactersCountMote.Text = "0 character(s)";
            this.labelNetworkIdCharactersCountMote.Click += new System.EventHandler(this.labelNetworkIdCharactersCountMote_Click);
            // 
            // buttonSetJoinKeyMote
            // 
            this.buttonSetJoinKeyMote.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetJoinKeyMote.Location = new System.Drawing.Point(365, 155);
            this.buttonSetJoinKeyMote.Name = "buttonSetJoinKeyMote";
            this.buttonSetJoinKeyMote.Size = new System.Drawing.Size(93, 26);
            this.buttonSetJoinKeyMote.TabIndex = 109;
            this.buttonSetJoinKeyMote.Text = "Set";
            this.buttonSetJoinKeyMote.UseVisualStyleBackColor = true;
            this.buttonSetJoinKeyMote.Click += new System.EventHandler(this.ButtonSetJoinKeyMote_Click);
            // 
            // buttonGetNetworkIdMote
            // 
            this.buttonGetNetworkIdMote.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetNetworkIdMote.Location = new System.Drawing.Point(210, 168);
            this.buttonGetNetworkIdMote.Name = "buttonGetNetworkIdMote";
            this.buttonGetNetworkIdMote.Size = new System.Drawing.Size(55, 26);
            this.buttonGetNetworkIdMote.TabIndex = 108;
            this.buttonGetNetworkIdMote.Text = "Get";
            this.buttonGetNetworkIdMote.UseVisualStyleBackColor = true;
            this.buttonGetNetworkIdMote.Click += new System.EventHandler(this.ButtonGetNetworkIdMote_Click);
            // 
            // buttonSetNetworkIdMote
            // 
            this.buttonSetNetworkIdMote.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetNetworkIdMote.Location = new System.Drawing.Point(166, 168);
            this.buttonSetNetworkIdMote.Name = "buttonSetNetworkIdMote";
            this.buttonSetNetworkIdMote.Size = new System.Drawing.Size(43, 26);
            this.buttonSetNetworkIdMote.TabIndex = 107;
            this.buttonSetNetworkIdMote.Text = "Set";
            this.buttonSetNetworkIdMote.UseVisualStyleBackColor = true;
            this.buttonSetNetworkIdMote.Click += new System.EventHandler(this.ButtonSetNetworkIdMote_Click);
            // 
            // labelJoinKeyCharactersCountMote
            // 
            this.labelJoinKeyCharactersCountMote.AutoSize = true;
            this.labelJoinKeyCharactersCountMote.Font = new System.Drawing.Font("Garamond", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelJoinKeyCharactersCountMote.Location = new System.Drawing.Point(273, 140);
            this.labelJoinKeyCharactersCountMote.Name = "labelJoinKeyCharactersCountMote";
            this.labelJoinKeyCharactersCountMote.Size = new System.Drawing.Size(91, 16);
            this.labelJoinKeyCharactersCountMote.TabIndex = 105;
            this.labelJoinKeyCharactersCountMote.Text = "0 character(s)";
            // 
            // labelJoinKeyMote
            // 
            this.labelJoinKeyMote.AutoSize = true;
            this.labelJoinKeyMote.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelJoinKeyMote.Location = new System.Drawing.Point(270, 116);
            this.labelJoinKeyMote.Name = "labelJoinKeyMote";
            this.labelJoinKeyMote.Size = new System.Drawing.Size(87, 22);
            this.labelJoinKeyMote.TabIndex = 101;
            this.labelJoinKeyMote.Text = "Join Key:";
            // 
            // textBoxJoinKeyMote
            // 
            this.textBoxJoinKeyMote.Font = new System.Drawing.Font("Garamond", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxJoinKeyMote.Location = new System.Drawing.Point(365, 114);
            this.textBoxJoinKeyMote.Multiline = true;
            this.textBoxJoinKeyMote.Name = "textBoxJoinKeyMote";
            this.textBoxJoinKeyMote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxJoinKeyMote.Size = new System.Drawing.Size(91, 41);
            this.textBoxJoinKeyMote.TabIndex = 100;
            this.textBoxJoinKeyMote.TextChanged += new System.EventHandler(this.TextBoxJoinKeyMote_TextChanged);
            // 
            // labelNetworkIdMote
            // 
            this.labelNetworkIdMote.AutoSize = true;
            this.labelNetworkIdMote.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNetworkIdMote.Location = new System.Drawing.Point(8, 134);
            this.labelNetworkIdMote.Name = "labelNetworkIdMote";
            this.labelNetworkIdMote.Size = new System.Drawing.Size(118, 22);
            this.labelNetworkIdMote.TabIndex = 99;
            this.labelNetworkIdMote.Text = "Network ID:";
            this.labelNetworkIdMote.Click += new System.EventHandler(this.labelNetworkIdMote_Click);
            // 
            // textBoxNetworkIdMote
            // 
            this.textBoxNetworkIdMote.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNetworkIdMote.Location = new System.Drawing.Point(127, 138);
            this.textBoxNetworkIdMote.Name = "textBoxNetworkIdMote";
            this.textBoxNetworkIdMote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxNetworkIdMote.Size = new System.Drawing.Size(135, 29);
            this.textBoxNetworkIdMote.TabIndex = 98;
            this.textBoxNetworkIdMote.TextChanged += new System.EventHandler(this.TextBoxNetworkIdMote_TextChanged);
            // 
            // comboBoxRadiotestMote
            // 
            this.comboBoxRadiotestMote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRadiotestMote.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxRadiotestMote.FormattingEnabled = true;
            this.comboBoxRadiotestMote.Items.AddRange(new object[] {
            "ON",
            "OFF"});
            this.comboBoxRadiotestMote.Location = new System.Drawing.Point(127, 105);
            this.comboBoxRadiotestMote.Name = "comboBoxRadiotestMote";
            this.comboBoxRadiotestMote.Size = new System.Drawing.Size(135, 30);
            this.comboBoxRadiotestMote.TabIndex = 66;
            this.comboBoxRadiotestMote.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxRadiotestMote_SelectionChangeCommitted);
            // 
            // labelRadiotestMote
            // 
            this.labelRadiotestMote.AutoSize = true;
            this.labelRadiotestMote.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRadiotestMote.Location = new System.Drawing.Point(31, 97);
            this.labelRadiotestMote.Name = "labelRadiotestMote";
            this.labelRadiotestMote.Size = new System.Drawing.Size(95, 22);
            this.labelRadiotestMote.TabIndex = 65;
            this.labelRadiotestMote.Text = "Radiotest:";
            // 
            // comboBoxAutoJoinMote
            // 
            this.comboBoxAutoJoinMote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAutoJoinMote.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxAutoJoinMote.FormattingEnabled = true;
            this.comboBoxAutoJoinMote.Items.AddRange(new object[] {
            "ON",
            "OFF"});
            this.comboBoxAutoJoinMote.Location = new System.Drawing.Point(352, 81);
            this.comboBoxAutoJoinMote.Name = "comboBoxAutoJoinMote";
            this.comboBoxAutoJoinMote.Size = new System.Drawing.Size(104, 30);
            this.comboBoxAutoJoinMote.TabIndex = 64;
            this.comboBoxAutoJoinMote.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxAutoJoinMote_SelectionChangeCommitted);
            // 
            // labelAutoJoinMote
            // 
            this.labelAutoJoinMote.AutoSize = true;
            this.labelAutoJoinMote.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAutoJoinMote.Location = new System.Drawing.Point(267, 84);
            this.labelAutoJoinMote.Name = "labelAutoJoinMote";
            this.labelAutoJoinMote.Size = new System.Drawing.Size(90, 22);
            this.labelAutoJoinMote.TabIndex = 63;
            this.labelAutoJoinMote.Text = "AutoJoin:";
            // 
            // comboBoxModeMote
            // 
            this.comboBoxModeMote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxModeMote.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxModeMote.FormattingEnabled = true;
            this.comboBoxModeMote.Items.AddRange(new object[] {
            "master",
            "slave"});
            this.comboBoxModeMote.Location = new System.Drawing.Point(127, 72);
            this.comboBoxModeMote.Name = "comboBoxModeMote";
            this.comboBoxModeMote.Size = new System.Drawing.Size(135, 30);
            this.comboBoxModeMote.TabIndex = 62;
            this.comboBoxModeMote.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxModeMote_SelectionChangeCommitted);
            // 
            // labelModeMote
            // 
            this.labelModeMote.AutoSize = true;
            this.labelModeMote.Font = new System.Drawing.Font("Garamond", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModeMote.Location = new System.Drawing.Point(63, 71);
            this.labelModeMote.Name = "labelModeMote";
            this.labelModeMote.Size = new System.Drawing.Size(63, 22);
            this.labelModeMote.TabIndex = 61;
            this.labelModeMote.Text = "Mode:";
            // 
            // buttonFactoryResetMote
            // 
            this.buttonFactoryResetMote.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFactoryResetMote.Location = new System.Drawing.Point(341, 41);
            this.buttonFactoryResetMote.Name = "buttonFactoryResetMote";
            this.buttonFactoryResetMote.Size = new System.Drawing.Size(97, 27);
            this.buttonFactoryResetMote.TabIndex = 60;
            this.buttonFactoryResetMote.Text = "Factory Reset";
            this.buttonFactoryResetMote.UseVisualStyleBackColor = true;
            this.buttonFactoryResetMote.Click += new System.EventHandler(this.ButtonFactoryResetMote_Click);
            // 
            // buttonRestartMote
            // 
            this.buttonRestartMote.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestartMote.Location = new System.Drawing.Point(250, 40);
            this.buttonRestartMote.Name = "buttonRestartMote";
            this.buttonRestartMote.Size = new System.Drawing.Size(87, 26);
            this.buttonRestartMote.TabIndex = 59;
            this.buttonRestartMote.Text = "Restart";
            this.buttonRestartMote.UseVisualStyleBackColor = true;
            this.buttonRestartMote.Click += new System.EventHandler(this.ButtonRestartMote_Click);
            // 
            // labelComPortBoxMote
            // 
            this.labelComPortBoxMote.AutoSize = true;
            this.labelComPortBoxMote.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelComPortBoxMote.Location = new System.Drawing.Point(124, 20);
            this.labelComPortBoxMote.Name = "labelComPortBoxMote";
            this.labelComPortBoxMote.Size = new System.Drawing.Size(127, 21);
            this.labelComPortBoxMote.TabIndex = 58;
            this.labelComPortBoxMote.Text = "COM Port Box";
            // 
            // combo_PortMote
            // 
            this.combo_PortMote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_PortMote.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_PortMote.FormattingEnabled = true;
            this.combo_PortMote.Location = new System.Drawing.Point(127, 40);
            this.combo_PortMote.Name = "combo_PortMote";
            this.combo_PortMote.Size = new System.Drawing.Size(96, 28);
            this.combo_PortMote.TabIndex = 57;
            this.combo_PortMote.SelectedIndexChanged += new System.EventHandler(this.combo_PortMote_SelectedIndexChanged);
            this.combo_PortMote.SelectionChangeCommitted += new System.EventHandler(this.Combo_PortMote_SelectionChangeCommitted);
            this.combo_PortMote.Click += new System.EventHandler(this.Combo_PortMote_Click);
            // 
            // buttonConnectionMote
            // 
            this.buttonConnectionMote.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnectionMote.Location = new System.Drawing.Point(7, 40);
            this.buttonConnectionMote.Name = "buttonConnectionMote";
            this.buttonConnectionMote.Size = new System.Drawing.Size(115, 26);
            this.buttonConnectionMote.TabIndex = 0;
            this.buttonConnectionMote.Text = "Connection";
            this.buttonConnectionMote.UseVisualStyleBackColor = true;
            this.buttonConnectionMote.Click += new System.EventHandler(this.ButtonConnectionMote_Click);
            // 
            // stopWatchTimer
            // 
            this.stopWatchTimer.Interval = 1000;
            // 
            // groupBoxDeviceType
            // 
            this.groupBoxDeviceType.Controls.Add(this.comboBoxDeviceType);
            this.groupBoxDeviceType.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDeviceType.Location = new System.Drawing.Point(5, 84);
            this.groupBoxDeviceType.Name = "groupBoxDeviceType";
            this.groupBoxDeviceType.Size = new System.Drawing.Size(208, 55);
            this.groupBoxDeviceType.TabIndex = 98;
            this.groupBoxDeviceType.TabStop = false;
            this.groupBoxDeviceType.Text = "Device";
            // 
            // labelInputScreenCharactersCount
            // 
            this.labelInputScreenCharactersCount.AutoSize = true;
            this.labelInputScreenCharactersCount.Font = new System.Drawing.Font("Garamond", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInputScreenCharactersCount.Location = new System.Drawing.Point(137, 34);
            this.labelInputScreenCharactersCount.Name = "labelInputScreenCharactersCount";
            this.labelInputScreenCharactersCount.Size = new System.Drawing.Size(91, 16);
            this.labelInputScreenCharactersCount.TabIndex = 106;
            this.labelInputScreenCharactersCount.Text = "0 character(s)";
            // 
            // labelOutputScreenCharactersCount
            // 
            this.labelOutputScreenCharactersCount.AutoSize = true;
            this.labelOutputScreenCharactersCount.Font = new System.Drawing.Font("Garamond", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutputScreenCharactersCount.Location = new System.Drawing.Point(602, 39);
            this.labelOutputScreenCharactersCount.Name = "labelOutputScreenCharactersCount";
            this.labelOutputScreenCharactersCount.Size = new System.Drawing.Size(91, 16);
            this.labelOutputScreenCharactersCount.TabIndex = 107;
            this.labelOutputScreenCharactersCount.Text = "0 character(s)";
            this.labelOutputScreenCharactersCount.Click += new System.EventHandler(this.labelOutputScreenCharactersCount_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(596, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 36);
            this.button1.TabIndex = 111;
            this.button1.Text = "DPT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.Enter += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(572, 59);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 30);
            this.comboBox1.TabIndex = 112;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Network_Manager_GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.ClientSize = new System.Drawing.Size(1160, 586);
            this.Controls.Add(this.labelOutputScreenCharactersCount);
            this.Controls.Add(this.labelInputScreenCharactersCount);
            this.Controls.Add(this.groupBoxDeviceType);
            this.Controls.Add(this.labelDeviceType);
            this.Controls.Add(this.groupBoxTasksMote);
            this.Controls.Add(this.groupBoxTaskOutcome);
            this.Controls.Add(this.labelOutputScreen);
            this.Controls.Add(this.labelInputScreen);
            this.Controls.Add(this.groupBoxTasks);
            this.Controls.Add(this.output_screen);
            this.Controls.Add(this.buttonClearOutputBox);
            this.Controls.Add(this.buttonClearInputBox);
            this.Controls.Add(this.buttonSendData);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.input_screen);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Network_Manager_GUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SmartMesh IP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Network_Manager_GUI_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Network_Manager_GUI_FormClosed);
            this.Load += new System.EventHandler(this.Network_Manager_GUI_Load);
            this.EnabledChanged += new System.EventHandler(this.Network_Manager_GUI_EnabledChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Network_Manager_GUI_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Network_Manager_GUI_KeyUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxTasks.ResumeLayout(false);
            this.groupBoxTasks.PerformLayout();
            this.groupBoxTaskOutcome.ResumeLayout(false);
            this.groupBoxTaskOutcome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutcome)).EndInit();
            this.groupBoxTasksMote.ResumeLayout(false);
            this.groupBoxTasksMote.PerformLayout();
            this.groupBoxDeviceType.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox input_screen;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem portSettingsToolStripMenuItem1;
        private System.Windows.Forms.Button buttonSendData;
        private System.Windows.Forms.Button buttonClearInputBox;
        private System.Windows.Forms.Button buttonClearOutputBox;
        private System.Windows.Forms.RichTextBox output_screen;
        private System.Windows.Forms.GroupBox groupBoxTasks;
        private System.Windows.Forms.Button buttonConnectionNetworkManager;
        public System.Windows.Forms.ComboBox combo_PortNetworkManager;
        public System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label labelComPortBox;
        private System.Windows.Forms.Label labelInputScreen;
        private System.Windows.Forms.Label labelOutputScreen;
        private System.Windows.Forms.GroupBox groupBoxTaskOutcome;
        private System.Windows.Forms.Label labelTaskOutcomeInfo;
        private System.Windows.Forms.PictureBox pictureBoxOutcome;
        private System.Windows.Forms.TextBox textBoxTaskOutcomeResult;
        private System.Windows.Forms.TextBox textBoxTaskOutcomeStatus;
        private System.Windows.Forms.TextBox textBoxTaskOutcomeName;
        private System.Windows.Forms.Label labelTaskOutcomeResult;
        private System.Windows.Forms.Label labelTaskOutcomeStatus;
        private System.Windows.Forms.Label labelTaskOutcomeName;
        private System.Windows.Forms.TextBox textBoxTaskOutcomeMessage;
        private System.Windows.Forms.Button buttonFactoryResetNetworkManager;
        private System.Windows.Forms.Button buttonRestartNetworkManager;
        private System.Windows.Forms.GroupBox groupBoxTasksMote;
        private System.Windows.Forms.Button buttonFactoryResetMote;
        private System.Windows.Forms.Button buttonRestartMote;
        private System.Windows.Forms.Label labelComPortBoxMote;
        public System.Windows.Forms.ComboBox combo_PortMote;
        private System.Windows.Forms.Button buttonConnectionMote;
        public System.Windows.Forms.ComboBox comboBoxDeviceType;
        private System.Windows.Forms.Label labelDeviceType;
        public System.Windows.Forms.ComboBox comboBoxModeMote;
        private System.Windows.Forms.Label labelModeMote;
        public System.Windows.Forms.ComboBox comboBoxAutoJoinMote;
        private System.Windows.Forms.Label labelAutoJoinMote;
        public System.Windows.Forms.ComboBox comboBoxRadiotestNetworkManager;
        private System.Windows.Forms.Label labelRadiotestNetworkManager;
        public System.Windows.Forms.ComboBox comboBoxRadiotestMote;
        private System.Windows.Forms.Label labelRadiotestMote;
        private System.Windows.Forms.Label labelStopWatchTimer;
        private System.Windows.Forms.Timer stopWatchTimer;
        private System.Windows.Forms.ToolStripMenuItem disconnectDevicesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem firmwareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFirmwareToolStripMenuItem;
        private System.Windows.Forms.Label labelJoinKeyNetworkManager;
        private System.Windows.Forms.TextBox textBoxJoinKeyNetworkManager;
        private System.Windows.Forms.Label labelNetworkIdNetworkManager;
        private System.Windows.Forms.TextBox textBoxNetworkIdNetworkManager;
        private System.Windows.Forms.Label labelJoinKeyMote;
        private System.Windows.Forms.TextBox textBoxJoinKeyMote;
        private System.Windows.Forms.Label labelNetworkIdMote;
        private System.Windows.Forms.TextBox textBoxNetworkIdMote;
        private System.Windows.Forms.GroupBox groupBoxDeviceType;
        private System.Windows.Forms.Label labelJoinKeyCharactersCountNetworkManager;
        private System.Windows.Forms.Label labelJoinKeyCharactersCountMote;
        private System.Windows.Forms.Label labelInputScreenCharactersCount;
        private System.Windows.Forms.Label labelOutputScreenCharactersCount;
        private System.Windows.Forms.Button buttonSetJoinKeyNetworkManager;
        private System.Windows.Forms.Button buttonGetNetworkIdNetworkManager;
        private System.Windows.Forms.Button buttonSetNetworkIdNetworkManager;
        private System.Windows.Forms.Button buttonSetJoinKeyMote;
        private System.Windows.Forms.Button buttonGetNetworkIdMote;
        private System.Windows.Forms.Button buttonSetNetworkIdMote;
        private System.Windows.Forms.Label labelNetworkIdCharactersCountNetworkManager;
        private System.Windows.Forms.Label labelNetworkIdCharactersCountMote;
        private System.Windows.Forms.ToolStripLabel status_system_status;
        private System.Windows.Forms.ToolStripMenuItem networkViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem networkStatisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem temperatureLoggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem temperaturePlotterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oscilloscopeToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

