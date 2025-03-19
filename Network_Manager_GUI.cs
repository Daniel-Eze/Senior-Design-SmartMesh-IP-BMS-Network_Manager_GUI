using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Network_Manager_GUI
{
    public partial class Network_Manager_GUI : Form
    {
        private SerialPort serialPort;

        public Network_Manager_GUI()
        {
            InitializeComponent();
            serialPort1 = new SerialPort();
        }
        private void LoadMoteIDs()
        {
            if (serialPort.IsOpen)
            {
                serialPort.WriteLine("minfo");
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort1.ReadLine().Trim();  // ✅ Read response from serial port

                this.Invoke((MethodInvoker)delegate
                {
                    output_screen.AppendText(data + Environment.NewLine);

                    // ✅ Check if we received any Mote ID
                    if (data.StartsWith("MoteID:")) // Assuming "minfo" returns "MoteID: 001A"
                    {
                        string moteId = data.Split(':')[1].Trim();

                        // ✅ Ensure the comboBox remains empty if no motes are found
                        if (!comboBox1.Items.Contains(moteId))
                        {
                            comboBox1.Items.Add(moteId);
                        }
                    }
                    else if (data.Contains("No Motes Found") || string.IsNullOrWhiteSpace(data))
                    {
                        comboBox1.Items.Clear(); // ✅ Clear dropdown if no motes are detected
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading from serial port: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        #region GUI Form
        /// <summary>
        /// Function used when the Network Manager GUI is starting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Network_Manager_GUI_Load(object sender, EventArgs e)
        {
            //Close some programs
            Common.CloseSomeProgramsAtStartup();
            //Allow key events, each key event is passed to the control that has focus
            KeyPreview = true;
            //Assign default serial port settings
            AssignDefaultSerialPortSettings();
            //Initialize task settings
            UpdateTaskSettings(EnumTaskName.NA, EnumTaskStatus.NA, EnumTaskResult.NA, Common.deviceType, 0);
            //Set controls' ToolTip
            SetControlsToolTip();
            //Disable 'disconnectDevicesToolStripMenuItem' button
            DisconnectDevicesToolStripMenuItemEnabled(false);
            //Enable 'firmwareToolStripMenuItem' button
            FirmwareToolStripMenuItemEnabled(true);
            //Disable 'applicationsToolStripMenuItem' button
            ApplicationsToolStripMenuItemEnabled(false);
            //Disable 'temperatureLoggerToolStripMenuItem' button
            TemperatureLoggerToolStripMenuItemEnabled(false);
            //Disable 'temperaturePlotterToolStripMenuItem' button
            TemperaturePlotterToolStripMenuItemEnabled(false);
            //Disable 'oscilloscopeToolStripMenuItem' button
            OscilloscopeToolStripMenuItemEnabled(false);
            //Disable 'networkStatisticsToolStripMenuItem' button
            NetworkStatisticsToolStripMenuItemEnabled(false);
            //Disable Network Manager Task buttons
            NetworkManager_TaskButtons_Enabled(false);
            //Disable Mote Task buttons
            Mote_TaskButtons_Enabled(false);
            //Disable SendData button
            ButtonSendDataEnabled(false);
            //Enable Connection button
            ButtonConnectionEnabled(true);
            //Select the Connection button
            ButtonConnectionSelect();
        }

        /// <summary>
        /// Function used when attempting to close the Network Manager GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Network_Manager_GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //display message box requesting user input
            if (MessageBox.Show("You’re about to exit. Continue?", "SmartMesh IP Battery Management System GUI", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true; //sets a value indicating whether the event should be canceled => GUI will not be closed
            }
        }

        /// <summary>
        /// Function used when the Network Manager GUI is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Network_Manager_GUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Display message box
            MessageBox.Show("Thank you for using our Battery Management System GUI application", "SmartMesh IP Battery Management System GUI");

            #region Close this Project and its resources
            //Close the serial port interface
            CloseSerialPortInterface();
            //Dispose and close the application
            Dispose(true);
            GC.SuppressFinalize(this);
            Close();
            #endregion Close this Project and its resources
        }

        /// <summary>
        /// Function used when the main form has focus and a key is pressed and released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Network_Manager_GUI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ActiveControl == combo_PortNetworkManager || ActiveControl == combo_PortMote)
            {
                //Exit function
                return;
            }
            else if (ActiveControl == input_screen)
            {
                //this is needed to properly handle when the cursor is in the input screen.
            }
            else if (ActiveControl == textBoxNetworkIdNetworkManager)
            {
                //select the 'textBoxNetworkIdNetworkManager' button
                TextBoxNetworkIdNetworkManagerSelect();
            }
            else if (ActiveControl == textBoxJoinKeyNetworkManager)
            {
                //select the 'textBoxJoinKeyNetworkManager' button
                TextBoxJoinKeyNetworkManagerSelect();
            }
            else if (ActiveControl == textBoxNetworkIdMote)
            {
                //select the 'textBoxNetworkIdMote' button
                TextBoxNetworkIdMoteSelect();
            }
            else if (ActiveControl == textBoxJoinKeyMote)
            {
                //select the 'textBoxJoinKeyMote' button
                TextBoxJoinKeyMoteSelect();
            }
            else
            {
                if (char.IsLetterOrDigit(e.KeyChar) == true) //check if the pressed key is a letter or number
                {
                    Input_ScreenText(input_screen.Text += e.KeyChar.ToString()); //enter key pressed in input screen textbox
                    ButtonSendDataSelect(); //select the SendData button
                }
                else if (e.KeyChar == '\b') //check if the pressed key is backspace
                {
                    if (string.IsNullOrEmpty(input_screen.Text) == false) //check if the input screen is empty or not
                    {
                        Input_ScreenText(input_screen.Text.Substring(0, input_screen.Text.Length - 1)); //delete most recent character
                    }
                }
                else if (e.KeyChar == ' ') //check if the pressed key is space
                {
                    Input_ScreenText(input_screen.Text += " "); //add space
                    Output_ScreenSelect(); //this is to avoid SendData button press simulation
                }
                else if (e.KeyChar == '\r') //check if the pressed key is enter
                {
                    if (Common.taskStatus != EnumTaskStatus.STARTED)
                    {
                        ButtonSendData_Click(sender, e); //simulate SendData button click
                    }
                }
            }
        }

        /// <summary>
        /// Function used when the main form has focus and a key is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Network_Manager_GUI_KeyUp(object sender, KeyEventArgs e)
        {
            //get and process KeyCode of key released
            //the below 'Replace' call is useful when pressing and releasing numbers on right side of PC keyboard
            string keyCode = e.KeyCode.ToString().Replace("NumPad", "");
            if (keyCode.Length > 1) //check if keyCode has more than 1 character 
            {
                //useful when pressing numbers on top side of PC keyboard
                keyCode = keyCode.Replace("D", "");
            }

            //if the active control is 'textBoxNetworkIdNetworkManager'
            if (ActiveControl == textBoxNetworkIdNetworkManager)
            {
                //check if the pressed key is a number
                if (Common.AreDigitsOnly(keyCode) == false)
                {
                    //check if textBoxNetworkIdNetworkManager is empty or not
                    if (string.IsNullOrEmpty(textBoxNetworkIdNetworkManager.Text) == false)
                    {
                        //find the textBoxNetworkIdNetworkManager cursor location
                        int textBoxNetworkIdNetworkManagerCursorLocation = textBoxNetworkIdNetworkManager.SelectionStart;
                        //check if 'textBoxNetworkIdNetworkManager.Text' contains the KeyCode of the released key
                        if (textBoxNetworkIdNetworkManager.Text.ToUpper().Contains(keyCode.ToUpper()))
                        {
                            //delete all invalid characters
                            TextBoxNetworkIdNetworkManagerText(textBoxNetworkIdNetworkManager.Text.Replace(keyCode.ToLower(), ""));
                            TextBoxNetworkIdNetworkManagerText(textBoxNetworkIdNetworkManager.Text.Replace(keyCode.ToUpper(), ""));
                            //assign the textBoxNetworkIdNetworkManager cursor location
                            if (textBoxNetworkIdNetworkManagerCursorLocation > 0)
                            {
                                textBoxNetworkIdNetworkManager.SelectionStart = textBoxNetworkIdNetworkManagerCursorLocation - 1;
                            }
                            else if (textBoxNetworkIdNetworkManagerCursorLocation == 0)
                            {
                                textBoxNetworkIdNetworkManager.SelectionStart = textBoxNetworkIdNetworkManagerCursorLocation;
                            }
                        }
                    }
                }
            }

            //if the active control is 'textBoxNetworkIdMote'
            if (ActiveControl == textBoxNetworkIdMote)
            {
                //check if the pressed key is a number
                if (Common.AreDigitsOnly(keyCode) == false)
                {
                    //check if textBoxNetworkIdMote is empty or not
                    if (string.IsNullOrEmpty(textBoxNetworkIdMote.Text) == false)
                    {
                        //find the textBoxNetworkIdMote cursor location
                        int textBoxNetworkIdMoteCursorLocation = textBoxNetworkIdMote.SelectionStart;
                        //check if 'textBoxNetworkIdMote.Text' contains the KeyCode of the released key
                        if (textBoxNetworkIdMote.Text.ToUpper().Contains(keyCode.ToUpper()))
                        {
                            //delete all invalid characters
                            TextBoxNetworkIdMoteText(textBoxNetworkIdMote.Text.Replace(keyCode.ToLower(), ""));
                            TextBoxNetworkIdMoteText(textBoxNetworkIdMote.Text.Replace(keyCode.ToUpper(), ""));
                            //assign the textBoxNetworkIdMote cursor location
                            if (textBoxNetworkIdMoteCursorLocation > 0)
                            {
                                textBoxNetworkIdMote.SelectionStart = textBoxNetworkIdMoteCursorLocation - 1;
                            }
                            else if (textBoxNetworkIdMoteCursorLocation == 0)
                            {
                                textBoxNetworkIdMote.SelectionStart = textBoxNetworkIdMoteCursorLocation;
                            }
                        }
                    }
                }
            }

            //if the active control is 'textBoxJoinKeyNetworkManager'
            if (ActiveControl == textBoxJoinKeyNetworkManager)
            {
                //check if the pressed key is a number
                if (Common.OnlyHexInString(keyCode) == false)
                {
                    //check if textBoxJoinKeyNetworkManager is empty or not
                    if (string.IsNullOrEmpty(textBoxJoinKeyNetworkManager.Text) == false)
                    {
                        //find the textBoxJoinKeyNetworkManager cursor location
                        int textBoxJoinKeyNetworkManagerCursorLocation = textBoxJoinKeyNetworkManager.SelectionStart;
                        //check if 'textBoxJoinKeyNetworkManager.Text' contains the KeyCode of the released key
                        if (textBoxJoinKeyNetworkManager.Text.ToUpper().Contains(keyCode.ToUpper()))
                        {
                            //delete all invalid characters
                            TextBoxJoinKeyNetworkManagerText(textBoxJoinKeyNetworkManager.Text.Replace(keyCode.ToLower(), ""));
                            TextBoxJoinKeyNetworkManagerText(textBoxJoinKeyNetworkManager.Text.Replace(keyCode.ToUpper(), ""));
                            //assign the textBoxJoinKeyNetworkManager cursor location
                            if (textBoxJoinKeyNetworkManagerCursorLocation > 0)
                            {
                                textBoxJoinKeyNetworkManager.SelectionStart = textBoxJoinKeyNetworkManagerCursorLocation - 1;
                            }
                            else if (textBoxJoinKeyNetworkManagerCursorLocation == 0)
                            {
                                textBoxJoinKeyNetworkManager.SelectionStart = textBoxJoinKeyNetworkManagerCursorLocation;
                            }
                        }
                    }
                }
            }

            //if the active control is 'textBoxJoinKeyMote'
            if (ActiveControl == textBoxJoinKeyMote)
            {
                //check if the pressed key is a number
                if (Common.OnlyHexInString(keyCode) == false)
                {
                    //check if textBoxJoinKeyMote is empty or not
                    if (string.IsNullOrEmpty(textBoxJoinKeyMote.Text) == false)
                    {
                        //find the textBoxJoinKeyMote cursor location
                        int textBoxJoinKeyMoteCursorLocation = textBoxJoinKeyMote.SelectionStart;
                        //check if 'textBoxJoinKeyMote.Text' contains the KeyCode of the released key
                        if (textBoxJoinKeyMote.Text.ToUpper().Contains(keyCode.ToUpper()))
                        {
                            //delete all invalid characters
                            TextBoxJoinKeyMoteText(textBoxJoinKeyMote.Text.Replace(keyCode.ToLower(), ""));
                            TextBoxJoinKeyMoteText(textBoxJoinKeyMote.Text.Replace(keyCode.ToUpper(), ""));
                            //assign the textBoxJoinKeyMote cursor location
                            if (textBoxJoinKeyMoteCursorLocation > 0)
                            {
                                textBoxJoinKeyMote.SelectionStart = textBoxJoinKeyMoteCursorLocation - 1;
                            }
                            else if (textBoxJoinKeyMoteCursorLocation == 0)
                            {
                                textBoxJoinKeyMote.SelectionStart = textBoxJoinKeyMoteCursorLocation;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Function used when the main form's enabled state changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Network_Manager_GUI_EnabledChanged(object sender, EventArgs e)
        {
            //Check if the main form is enabled
            if (Enabled)
            {
                //Assign device type to match the device type ComboBox text
                Enum.TryParse(comboBoxDeviceType.SelectedItem.ToString(), out Common.deviceType);
            }
        }
        #endregion GUI Form

        #region Serial Port
        /// <summary>
        /// Function used to find and display the last successfully used serial port in proper comboBox.
        /// </summary>
        /// <returns></returns>
        private bool FindAndDisplayLastUsedSerialPortName()
        {
            //Populate COM Port Box
            Common.PopulateComPortBox(Name);
            //Grab the proper COM Port comboBox
            Common.comboBox_Port = Common.GetProperSerialPortComboBox();
            //Assign current (last successfully used) serial port name
            string lastSuccessfullyUsedSerialPortName = string.Empty;
            #region NETWORK_MANAGER
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                //checking if the last used serial port for Network Manager is still available
                lastSuccessfullyUsedSerialPortName = Common.CurrentNetworkManagerSerialPortName;
            }
            #endregion NETWORK_MANAGER
            #region MOTE
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                //checking if the last used serial port for Mote is still available
                lastSuccessfullyUsedSerialPortName = Common.CurrentMoteSerialPortName;
            }
            #endregion MOTE
            //Search the comboBox_Port's items for the last used serial port
            foreach (string availableSerialPort in Common.comboBox_Port.Items)
            {
                //checking if the last used serial port for Network Manager or Mote is still available
                if (lastSuccessfullyUsedSerialPortName == availableSerialPort)
                {
                    //Assign last successfully used serial port
                    Common.comboBox_Port.SelectedItem = availableSerialPort;
                    //Assign Serial Port settings
                    UpdateSerialPortSettings(Common.comboBox_Port.SelectedItem.ToString(), serialPort1.BaudRate, serialPort1.DataBits, serialPort1.Parity, serialPort1.StopBits, serialPort1.Handshake);
                    //Attempt to open assigned serial port
                    return OpenSerialPortThread(Common.serialPortThreadTimeout);
                }
                Application.DoEvents(); //refresh GUI
            }
            return false; //exit loop, the serial port has not been found
        }

        /// <summary>
        /// Function used to assign the default Serial Port settings.
        /// </summary>
        public void AssignDefaultSerialPortSettings()
        {
            //Find and display the last successfully used serial port in COM Port Box
            if (FindAndDisplayLastUsedSerialPortName())
            {
                //Initialize Serial Port settings
                UpdateSerialPortSettings(Common.comboBox_Port.SelectedItem.ToString(), 9600, 8, Parity.None, StopBits.One, Handshake.None);
            }
        }

        /// <summary>
        /// Function used to update Serial Port settings.
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="dataBits"></param>
        /// <param name="parity"></param>
        /// <param name="stopBits"></param>
        /// <param name="handShake"></param>
        public void UpdateSerialPortSettings(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits, Handshake handShake)
        {
            //Dispose and close serial port, in case it is open
            CloseSerialPortThread(Common.serialPortThreadTimeout);
            //Create a new serial port instance
            serialPort1 = new System.IO.Ports.SerialPort
            {
                PortName = portName.ToUpper(),
                BaudRate = baudRate,
                DataBits = dataBits,
                Parity = parity,
                StopBits = stopBits,
                Handshake = handShake,
                ReadTimeout = 500,
                WriteTimeout = 500,
                RtsEnable = true,
                DtrEnable = true
            };
            //Create Serial Port Data Received Event
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(SerialPort1_DataReceived);
        }

        /// <summary>
        /// Function used to start the thread for opening serial port.
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        private bool OpenSerialPortThread(int millisecondsTimeout)
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to open " + serialPort1.PortName + "...\r\n");
            //open serial port in new thread to avoid hang
            Thread openSerialPortThread = new Thread(new ThreadStart(OpenSerialPort));
            openSerialPortThread.Start(); //start thread to open serial port
            openSerialPortThread.Join(millisecondsTimeout); //wait specified time for thread to finish
            Application.DoEvents(); //refresh GUI
            //check if the designated serial port is open
            if (serialPort1.IsOpen == true)
            {
                Output_ScreenText("\r\n" + serialPort1.PortName + " is open!\r\n"); //append text in output_screen
            }
            else //if (serialPort1.IsOpen == false)
            {
                Output_ScreenText("\r\n" + serialPort1.PortName + " could not be opened!\r\n"); //append text in output_screen
            }
            return serialPort1.IsOpen; //return a boolean value indicating whether the serial port is open (true) or not (false)
        }

        /// <summary>
        /// Function used to open a specific serial port.
        /// </summary>
        /// <returns></returns>
        private void OpenSerialPort()
        {
            try
            {
                serialPort1.Open(); //try to open serial port
            }
            catch (Exception ex)
            {
                //display Exception message
                Output_ScreenText("\r\n" + ex.Message + "\r\n");
                //MessageBox.Show(ex.Message, "Serial Port Opening Error"); //display message box
            }
        }

        /// <summary>
        /// Function used to ensure the serial port interface is closed.
        /// </summary>
        private bool CloseSerialPortInterface()
        {
            if (serialPort1.IsOpen) //check if designated serial port is open
            {
                serialPort1.DiscardInBuffer(); //Discard data from the serial driver's receive buffer.
                serialPort1.DiscardOutBuffer(); //Discard data from the serial driver's transmit buffer.
                if (NetworkManager.isConnectedToGUI == true) //check if the Network Manager is connected to the GUI
                {
                    WriteLineToSerialPortThread("\r\nlogout", Common.serialPortThreadTimeout); //log out of Network Manager
                }
                CloseSerialPortThread(Common.serialPortThreadTimeout); //dispose and close serial port
            }
            else //if (!serialPort1.IsOpen)
            {
                Output_ScreenText("\r\n" + serialPort1.PortName + " is already closed!\r\n"); //append text in output_screen
            }
            return serialPort1.IsOpen; //return a value indicating the open or closed status of the serialPort1 object
        }

        /// <summary>
        /// Function used to start the thread for disposing and closing serial port.
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        private void CloseSerialPortThread(int millisecondsTimeout)
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to close " + serialPort1.PortName + "...\r\n");
            //close serial port in new thread to avoid hang
            Thread closeSerialPortThread = new Thread(new ThreadStart(CloseSerialPort));
            closeSerialPortThread.Start(); //start thread to close serial port
            closeSerialPortThread.Join(millisecondsTimeout); //wait specified time for thread to finish
            Application.DoEvents(); //refresh GUI
            //check if the designated serial port is closed
            if (serialPort1.IsOpen == false)
            {
                Output_ScreenText("\r\n" + serialPort1.PortName + " is closed!\r\n"); //append text in output_screen
            }
            else //if (serialPort1.IsOpen == true)
            {
                Output_ScreenText("\r\n" + serialPort1.PortName + " is not closed!\r\n"); //append text in output_screen
            }
        }

        /// <summary>
        /// Function used to dispose and close a specific serial port.
        /// </summary>
        private void CloseSerialPort()
        {
            try
            {
                //Dispose and close the serial port
                serialPort1.Dispose();
                GC.SuppressFinalize(serialPort1);
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                //display Exception message
                Output_ScreenText("\r\n" + ex.Message + "\r\n");
                //MessageBox.Show(ex.Message, "Serial Port Closing Error"); //catch any serial port closing error messages
            }
        }

        /// <summary>
        /// Function used to start the thread for writing to serial port.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        private bool WriteLineToSerialPortThread(string str, int millisecondsTimeout)
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to write to " + serialPort1.PortName +
                " the following string '" + str + "'...\r\n");
            //declare and initialize a variable to hold the write-to-serial port outcome
            bool isWriteLineSuccessful = false;
            //write to serial port in new thread to avoid hang
            Thread writeLineToSerialPortThread = new Thread(() => { isWriteLineSuccessful = WriteLineToSerial(str); });
            writeLineToSerialPortThread.Start(); //start thread to write to serial port
            writeLineToSerialPortThread.Join(millisecondsTimeout); //wait specified time for thread to finish
            Application.DoEvents(); //refresh GUI
            //check if the serial port write operation was successful
            if (isWriteLineSuccessful == true)
            {
                Output_ScreenText("\r\nSuccessfully wrote to " + serialPort1.PortName +
                " the following string '" + str + "'!\r\n"); //append text in output_screen
            }
            else //if (isWriteLineSuccessful == false)
            {
                Output_ScreenText("\r\nCould not write to " + serialPort1.PortName +
               " the following string '" + str + "'!\r\n"); //append text in output_screen
            }
            //return a boolean value indicating whether data was successfully written to serial port
            return isWriteLineSuccessful;
        }

        /// <summary>
        /// Writes the specified string and the System.IO.Ports.SerialPort.NewLine value to the output buffer.
        /// </summary>
        /// <param name="str"></param>
        private bool WriteLineToSerial(string str)
        {
            try
            {
                serialPort1.WriteLine(str); //writes the specified string to the serial port.
                return true; //successful writeline operation
            }
            catch (Exception ex)
            {
                //display Exception message
                Output_ScreenText("\r\n" + ex.Message + "\r\n");
                //MessageBox.Show(ex.Message, "Serial Port Writing Error"); //display message box
            }
            return false; //unsuccessful writeline operation
        }

        /// <summary>
        /// Function called everytime data has been received through the serial port.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                ReadandStoreExistingFromSerial(); //read and store incoming serial port data
                SerialDataProcessing(); //process incoming serial port data
                Application.DoEvents(); //refresh GUI
            }
            catch (Exception ex)
            {
                //display Exception message
                Output_ScreenText("\r\n" + ex.Message + "\r\n");
                //MessageBox.Show(ex.Message, "Serial Port Reading Error"); //display message box
            }
        }
        #endregion Serial Port

        #region Serial Port Data Processing
        /// <summary>
        /// Function used to read and store all immediately available serial port data.
        /// </summary>
        private void ReadandStoreExistingFromSerial()
        {
            string incomingData = string.Empty; //declare and initialize the variable to hold the incoming serial port data
            try
            {
                incomingData = serialPort1.ReadExisting(); //read all immediately available Serial Port bytes
                int readSerialPortDataCounter = 0; //declare and initialize readSerialPortDataCounter
                do //do while loop used to get all available Serial Port bytes, resulting from command execution, in one attempt
                {
                    Thread.Sleep(Common.serialPortTaskReadTimeout); //needed delay to stabilize serial port data traffic
                    Application.DoEvents(); //refresh GUI
                    incomingData += serialPort1.ReadExisting(); //read all immediately available Serial Port bytes
                    Thread.Sleep(Common.serialPortTaskReadTimeout); //needed delay to stabilize serial port data traffic
                    Application.DoEvents(); //refresh GUI
                    readSerialPortDataCounter++; //increment readSerialPortDataCounter by 1
                } while ((serialPort1.BytesToRead != 0) && (readSerialPortDataCounter < Common.serialPortReadLoopCounter));
            }
            catch (Exception ex)
            {
                //display Exception message
                Output_ScreenText("\r\n" + ex.Message + "\r\n");
                //MessageBox.Show(ex.Message, "Serial Port Reading Error");  //display message box
            }
            StoreExistingFromSerial(incomingData); //update log text 
        }

        /// <summary>
        /// Stores all immediately available serial port data and updates output screen log.
        /// </summary>
        /// <param name="incomingData"></param>
        private void StoreExistingFromSerial(string incomingData)
        {
            //append text in output_screen
            Output_ScreenText(incomingData);
            //store serial port incoming data
            Common.immediateSerialOutputLog = incomingData;
            Common.taskSerialOutputLog += incomingData;
        }

        /// <summary>
        /// Function used to process incoming serial port data.
        /// </summary>
        private void SerialDataProcessing()
        {
            #region CONNECTION Task
            if (Common.taskName == EnumTaskName.CONNECTION && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the CONNECTION task
                ConnectionTaskSerialDataProcessing();
            }
            #endregion CONNECTION Task

            #region RESTART Task
            else if (Common.taskName == EnumTaskName.RESTART && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the RESTART task
                RestartTaskSerialDataProcessing();
            }
            #endregion RESTART Task

            #region FACTORYRESET Task
            else if (Common.taskName == EnumTaskName.FACTORYRESET && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the FACTORYRESET task
                FactoryResetTaskSerialDataProcessing();
            }
            #endregion FACTORYRESET Task

            #region MODE Task
            else if (Common.taskName == EnumTaskName.MODE && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the MODE task
                MoteModeTaskSerialDataProcessing();
            }
            #endregion MODE Task

            #region AUTOJOIN Task
            else if (Common.taskName == EnumTaskName.AUTOJOIN && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the AUTOJOIN task
                MoteAutoJoinTaskSerialDataProcessing();
            }
            #endregion AUTOJOIN Task

            #region RADIOTEST Task
            else if (Common.taskName == EnumTaskName.RADIOTEST && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the RADIOTEST task
                if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                {
                    NetworkManagerRadiotestTaskSerialDataProcessing();
                }
                else if (Common.deviceType == EnumDeviceType.MOTE)
                {
                    MoteRadiotestTaskSerialDataProcessing();
                }
            }
            #endregion RADIOTEST Task

            #region SET_NETWORKID Task
            else if (Common.taskName == EnumTaskName.SET_NETWORKID && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the SET_NETWORKID task
                if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                {
                    NetworkManagerSetNetworkIdTaskSerialDataProcessing();
                }
                else if (Common.deviceType == EnumDeviceType.MOTE)
                {
                    MoteSetNetworkIdTaskSerialDataProcessing();
                }
            }
            #endregion SET_NETWORKID Task

            #region GET_NETWORKID Task
            else if (Common.taskName == EnumTaskName.GET_NETWORKID && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the GET_NETWORKID task
                GetNetworkIdTaskSerialDataProcessing();
            }
            #endregion GET_NETWORKID Task

            #region SET_JOINKEY Task
            else if (Common.taskName == EnumTaskName.SET_JOINKEY && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the SET_JOINKEY task
                if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                {
                    NetworkManagerSetJoinKeyTaskSerialDataProcessing();
                }
                else if (Common.deviceType == EnumDeviceType.MOTE)
                {
                    MoteSetJoinKeyTaskSerialDataProcessing();
                }
            }
            #endregion SET_JOINKEY Task

            #region GET_MOTESNUMBER Task
            else if (Common.taskName == EnumTaskName.GET_MOTESNUMBER && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the GET_MOTESNUMBER task
                GetMotesNumberTaskSerialDataProcessing();
            }
            #endregion GET_MOTESNUMBER Task

            #region GET_NETWORKSTATISTICS Task
            else if (Common.taskName == EnumTaskName.GET_NETWORKSTATISTICS && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //process incoming serial port data in respect to the GET_NETWORKSTATISTICS task
                GetNetworkStatisticsTaskSerialDataProcessing();
            }
            #endregion GET_NETWORKSTATISTICS Task
        }
        #endregion Serial Port Data Processing

        #region Tasks Serial Port Data Processing
        #region CONNECTION Task
        /// <summary>
        /// Function used to process incoming serial port data in respect to the CONNECTION task.
        /// </summary>
        private void ConnectionTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the CONNECTION task
                //Firstly, make sure the device has successfully received the login command
                if (Common.isFirstPartSuccessful == false)
                {
                    //Check if the device could be logged in
                    if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                    {
                        Common.isFirstPartSuccessful = Common.LookForString(NetworkManager.connectionTaskDesiredStringToLookFor, NetworkManager.connectionTaskUndesiredStringArrayToLookFor, Common.immediateSerialOutputLog);
                    }
                    else if (Common.deviceType == EnumDeviceType.MOTE)
                    {
                        Common.isFirstPartSuccessful = Common.LookForString(Mote.connectionTaskDesiredStringToLookFor, Mote.connectionTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    }
                    if (Common.isFirstPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " was successfully logged in!\r\n");
                        //attempt to access the mac address
                        if (AccessMacAddress() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + "'s Mac Address could not be accessed! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                    else //if (Common.isFirstPartSuccessful == false)
                    {
                        //Check the device type
                        if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe radiotest feature of the " + Common.deviceType + " needs to be turned off!\r\n");
                            //turn off the radiotest feature of the Network Manager
                            if (AssignRadiotest("off") == false)
                            {
                                //append text in output_screen
                                Output_ScreenText("\r\nThe " + Common.taskName + " task failed because the radiotest feature of the " +
                                    Common.deviceType + " could not be turned off!!\r\n");
                                //update task settings
                                UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                            }
                            else // if (AssignRadiotest("off") == true)
                            {
                                //append text in output_screen
                                Output_ScreenText("\r\nRestarting the " + Common.deviceType + "...\r\n");
                                //Attempt to assign the radiotest value of the Network Manager or Mote
                                WriteLineToSerialPortThread("reset", Common.serialPortThreadTimeout);
                                //append text in output_screen
                                Output_ScreenText("\r\nWaiting for the " + Common.deviceType + " to restart. Will take about " +
                                    NetworkManager.restartTaskTimeout / 1000 + " seconds...\r\n");
                                Thread.Sleep(NetworkManager.restartTaskTimeout); //wait
                                //attempt to log in to Network Manager
                                LoginToDevice();
                            }
                        }
                    }
                }
                //Lastly, access the Mac Address value
                if (Common.isFirstPartSuccessful == true)
                {
                    //access the Mac Address value
                    bool isTaskSuccessful = false;
                    if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                    {
                        isTaskSuccessful = Common.LookForString(NetworkManager.getMacAddressDesiredStringToLookFor, NetworkManager.connectionTaskUndesiredStringArrayToLookFor, Common.immediateSerialOutputLog);
                    }
                    else if (Common.deviceType == EnumDeviceType.MOTE)
                    {
                        isTaskSuccessful = Common.LookForString(Mote.getMacAddressDesiredStringToLookFor, Mote.connectionTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    }

                    //Check if task is successful
                    if (isTaskSuccessful)
                    {
                        //get, store, and display the Mac Address value
                        if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                        {
                            //get and store the Network Manager's Mac Address value
                            NetworkManager.macAddress = Common.GetStringBetweenTwoStrings(NetworkManager.getMacAddressDesiredStringToLookFor, "moteid", Common.taskSerialOutputLog);
                            NetworkManager.macAddress = NetworkManager.macAddress.Replace(":", "-").ToUpper();
                            //validate the retrieved mac address
                            if (Common.ValidateMacAddress(NetworkManager.macAddress) == false)
                            {
                                NetworkManager.macAddress = "invalid";
                            }
                            //display the Network Manager's Mac Address value                            
                            Output_ScreenText("\r\nThe " + Common.deviceType + "'s Mac Address is " + NetworkManager.macAddress + "!\r\n");
                        }
                        else if (Common.deviceType == EnumDeviceType.MOTE)
                        {
                            //get and store the Mote's Mac Address value
                            Mote.macAddress = Common.GetStringBetweenTwoStrings(Mote.getMacAddressDesiredStringToLookFor, "moteid", Common.taskSerialOutputLog);
                            Mote.macAddress = Mote.macAddress.Replace(":", "-").ToUpper();
                            //validate the retrieved mac address
                            if (Common.ValidateMacAddress(Mote.macAddress) == false)
                            {
                                Mote.macAddress = "invalid";
                            }
                            //display the Mote's Mac Address value                            
                            Output_ScreenText("\r\nThe " + Common.deviceType + "'s Mac Address is " + Mote.macAddress + "!\r\n");
                        }
                        //append text in output_screen
                        Status_System_StatusText("Connected to " + serialPort1.PortName.ToString() + "!"); //update connection status text
                        Status_System_StatusForeColor(Color.Green); //update connection status text color
                        //update task settings
                        UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                    }
                }
            }
        }
        #endregion CONNECTION Task

        #region RESTART Task
        /// <summary>
        /// Function used to process incoming serial port data in respect to the RESTART task.
        /// </summary>
        private void RestartTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the RESTART task
                bool isTaskSuccessful = false;
                if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                {
                    isTaskSuccessful = Common.LookForStringArray(NetworkManager.restartTaskDesiredStringArrayToLookFor, NetworkManager.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                }
                else if (Common.deviceType == EnumDeviceType.MOTE)
                {
                    isTaskSuccessful = Common.LookForStringArray(Mote.restartTaskDesiredStringArrayToLookFor, Mote.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                }

                //Check if task is successful
                if (isTaskSuccessful)
                {
                    //append text in output_screen
                    Output_ScreenText("\r\nThe " + Common.deviceType + " was successfully restarted!\r\n");
                    //attempt to log in to Network Manager/Mote
                    if (LoginToDevice() == false)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " could not be logged in! Therefore, the " + Common.taskName +
                            " task failed!\r\n");
                        //update task settings
                        UpdateTaskSettings(EnumTaskName.RESTART, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                    }
                    //update task settings
                    UpdateTaskSettings(EnumTaskName.RESTART, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                }
            }
        }
        #endregion RESTART Task

        #region FACTORYRESET Task
        /// <summary>
        /// Function used to process incoming serial port data in respect to the FACTORYRESET task.
        /// </summary>
        private void FactoryResetTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, use the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the FACTORYRESET task
                bool isTaskSuccessful = false;
                if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                {
                    isTaskSuccessful = Common.LookForString(NetworkManager.factoryResetTaskDesiredStringToLookFor, NetworkManager.factoryResetTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                }
                else if (Common.deviceType == EnumDeviceType.MOTE)
                {
                    isTaskSuccessful = Common.LookForString(Mote.factoryResetTaskDesiredStringToLookFor, Mote.factoryResetTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                }

                //Check if task is successful
                if (isTaskSuccessful)
                {
                    //append text in output_screen
                    Output_ScreenText("\r\nThe " + Common.deviceType + " was successfully factory reset!\r\n");
                    //update task settings
                    UpdateTaskSettings(EnumTaskName.FACTORYRESET, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                }
            }
        }
        #endregion FACTORYRESET Task

        #region MODE Task
        /// <summary>
        /// Function used to process incoming serial port data in respect to the MODE task.
        /// </summary>
        private void MoteModeTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the MODE task
                //Firstly, make sure the mote has successfully received the mode setting command
                if (Common.isFirstPartSuccessful == false)
                {
                    Common.isFirstPartSuccessful = Common.LookForString(Mote.modeTaskDesiredStringToLookFor1 + Common.ComboBoxModeMoteText, Mote.modeTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isFirstPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " has successfully received the mode setting command!\r\n");
                        //attempt to restart the Mote
                        if (RestartDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be restarted! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.MODE, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Secondly, make sure the mote has successfully restarted
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == false)
                {
                    Common.isSecondPartSuccessful = Common.LookForStringArray(Mote.restartTaskDesiredStringArrayToLookFor, Mote.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isSecondPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " successfully restarted!\r\n");
                    }
                }
                //Lastly, access and verify the mote's mode
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true)
                {
                    bool isTaskSuccessful = Common.LookForString(Mote.modeTaskDesiredStringToLookFor2 + Common.ComboBoxModeMoteText, Mote.modeTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);

                    //Check if task is successful
                    if (isTaskSuccessful == false)
                    {
                        //attempt to access the mote's mode
                        if (AccessMoteMode() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + "'s mode could not be accessed! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.MODE, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                    if (isTaskSuccessful == true)
                    {
                        //update task settings
                        UpdateTaskSettings(EnumTaskName.MODE, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                    }
                }
            }
        }
        #endregion MODE Task

        #region AUTOJOIN Task
        /// <summary>
        /// Function used to process incoming serial port data in respect to the AUTOJOIN task.
        /// </summary>
        private void MoteAutoJoinTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the AUTOJOIN task
                //Firstly, make sure the mote has successfully received the autoJoin setting command
                if (Common.isFirstPartSuccessful == false)
                {
                    Common.isFirstPartSuccessful = Common.LookForString(Mote.autoJoinTaskCommandString1 + Common.ComboBoxAutoJoinMoteValue, Mote.autoJoinTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isFirstPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " has successfully received the autoJoin setting command!\r\n");
                        //attempt to restart the Mote
                        if (RestartDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be restarted! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.AUTOJOIN, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Secondly, make sure the mote has successfully restarted
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == false)
                {
                    Common.isSecondPartSuccessful = Common.LookForStringArray(Mote.restartTaskDesiredStringArrayToLookFor, Mote.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isSecondPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " successfully restarted!\r\n");
                    }
                }
                //Lastly, access and verify the mote's autoJoin value
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true)
                {
                    bool isTaskSuccessful = Common.LookForString(Mote.autoJoinTaskDesiredStringToLookFor2 + Common.ComboBoxAutoJoinMoteValue, Mote.autoJoinTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);

                    //Check if task is successful
                    if (isTaskSuccessful == false)
                    {
                        //attempt to access the mote's autoJoin value
                        if (AccessMoteAutoJoin() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + "'s autoJoin value could not be accessed! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.AUTOJOIN, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                    if (isTaskSuccessful == true)
                    {
                        //update task settings
                        UpdateTaskSettings(EnumTaskName.AUTOJOIN, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                    }
                }
            }
        }
        #endregion AUTOJOIN Task

        #region RADIOTEST Task
        #region NETWORK_MANAGER
        /// <summary>
        /// Function used to process incoming serial port data in respect to the network manager's RADIOTEST task.
        /// </summary>
        private void NetworkManagerRadiotestTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the RADIOTEST task
                //Firstly, make sure the Network manager has successfully received the radiotest setting command
                if (Common.isFirstPartSuccessful == false)
                {
                    Common.isFirstPartSuccessful = Common.LookForString(NetworkManager.radiotestTaskCommandString1 + Common.ComboBoxRadiotestNetworkManagerText, NetworkManager.radiotestTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isFirstPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " has successfully received the radiotest setting command!\r\n");
                        //attempt to restart the Network manager
                        if (RestartDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be restarted! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Secondly, make sure the Network manager has successfully restarted
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == false)
                {
                    Common.isSecondPartSuccessful = Common.LookForStringArray(NetworkManager.restartTaskDesiredStringArrayToLookFor, NetworkManager.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isSecondPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " successfully restarted!\r\n");
                        //attempt to log in to Network Manager
                        if (LoginToDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be logged in! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Lastly, access and verify the Network manager's radiotest value
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true)
                {
                    bool isTaskSuccessful = false;
                    if (Common.ComboBoxRadiotestNetworkManagerText == "on")
                    {
                        isTaskSuccessful = Common.LookForString(NetworkManager.radiotestTaskDesiredStringToLookFor1, NetworkManager.radiotestTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    }
                    else if (Common.ComboBoxRadiotestNetworkManagerText == "off")
                    {
                        isTaskSuccessful = Common.LookForString(NetworkManager.radiotestTaskDesiredStringToLookFor2, NetworkManager.radiotestTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    }

                    //Check if task is successful
                    if (isTaskSuccessful == false)
                    {
                        //attempt to access the Network manager's radiotest value
                        if (AccessRadiotest() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + "'s radiotest value could not be accessed! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                    if (isTaskSuccessful == true)
                    {
                        //update task settings
                        UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                    }
                }
            }
        }
        #endregion NETWORK_MANAGER

        #region MOTE
        /// <summary>
        /// Function used to process incoming serial port data in respect to the mote's RADIOTEST task.
        /// </summary>
        private void MoteRadiotestTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the RADIOTEST task
                //Firstly, make sure the mote has successfully received the radiotest setting command
                if (Common.isFirstPartSuccessful == false)
                {
                    Common.isFirstPartSuccessful = Common.LookForString(Mote.radiotestTaskCommandString + Common.ComboBoxRadiotestMoteText, Mote.radiotestTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isFirstPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " has successfully received the radiotest setting command!\r\n");
                        //attempt to restart the Mote
                        if (RestartDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be restarted! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Secondly, make sure the mote has successfully restarted
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == false)
                {
                    Common.isSecondPartSuccessful = Common.LookForStringArray(Mote.restartTaskDesiredStringArrayToLookFor, Mote.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isSecondPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " successfully restarted!\r\n");
                    }
                }
                //Lastly, access and verify the mote's radiotest value
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true)
                {
                    bool isTaskSuccessful = Common.LookForString(Mote.radiotestTaskDesiredStringToLookFor + Common.ComboBoxRadiotestMoteText, Mote.radiotestTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);

                    //Check if task is successful
                    if (isTaskSuccessful == false)
                    {
                        //attempt to access the mote's radiotest value
                        if (AccessRadiotest() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + "'s radiotest value could not be accessed! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                    if (isTaskSuccessful == true)
                    {
                        //update task settings
                        UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                    }
                }
            }
        }
        #endregion MOTE
        #endregion RADIOTEST Task

        #region SET_NETWORKID Task
        #region NETWORK_MANAGER
        /// <summary>
        /// Function used to process incoming serial port data in respect to the network manager's SET_NETWORKID task.
        /// </summary>
        private void NetworkManagerSetNetworkIdTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the SET_NETWORKID task
                //Firstly, make sure the Network manager has successfully received the Network ID setting command
                if (Common.isFirstPartSuccessful == false)
                {
                    Common.isFirstPartSuccessful = Common.LookForString(NetworkManager.setNetworkIdTaskDesiredStringToLookFor + Common.NetworkIdNetworkManagerText, NetworkManager.setNetworkIdTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isFirstPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " has successfully received the Network ID setting command!\r\n");
                        //attempt to restart the Network manager
                        if (RestartDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be restarted! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Secondly, make sure the Network manager has successfully restarted
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == false)
                {
                    Common.isSecondPartSuccessful = Common.LookForStringArray(NetworkManager.restartTaskDesiredStringArrayToLookFor, NetworkManager.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isSecondPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " successfully restarted!\r\n");
                        //attempt to log in to Network Manager
                        if (LoginToDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be logged in! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Lastly, access and verify the Network manager's Network ID value
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true)
                {
                    bool isTaskSuccessful = Common.LookForString(NetworkManager.getNetworkIdTaskDesiredStringToLookFor + Common.NetworkIdNetworkManagerText, NetworkManager.getNetworkIdTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);

                    //Check if task is successful
                    if (isTaskSuccessful == false)
                    {
                        //attempt to access the Network manager's Network ID value
                        if (AccessNetworkId() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + "'s Network ID value could not be accessed! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                    if (isTaskSuccessful == true)
                    {
                        //update task settings
                        UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                    }
                }
            }
        }
        #endregion NETWORK_MANAGER

        #region MOTE
        /// <summary>
        /// Function used to process incoming serial port data in respect to the mote's SET_NETWORKID task.
        /// </summary>
        private void MoteSetNetworkIdTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the SET_NETWORKID task
                //Firstly, make sure the mote has successfully received the Network ID setting command
                if (Common.isFirstPartSuccessful == false)
                {
                    Common.isFirstPartSuccessful = Common.LookForString(Mote.setNetworkIdTaskDesiredStringToLookFor + Common.NetworkIdMoteText, Mote.setNetworkIdTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isFirstPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " has successfully received the Network ID setting command!\r\n");
                        //attempt to restart the Mote
                        if (RestartDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be restarted! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Secondly, make sure the mote has successfully restarted
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == false)
                {
                    Common.isSecondPartSuccessful = Common.LookForStringArray(Mote.restartTaskDesiredStringArrayToLookFor, Mote.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isSecondPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " successfully restarted!\r\n");
                    }
                }
                //Lastly, access and verify the mote's Network ID value
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true)
                {
                    bool isTaskSuccessful = Common.LookForString(Mote.getNetworkIdTaskDesiredStringToLookFor + Common.NetworkIdMoteText, Mote.getNetworkIdTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);

                    //Check if task is successful
                    if (isTaskSuccessful == false)
                    {
                        //attempt to access the mote's Network ID value
                        if (AccessNetworkId() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + "'s Network ID value could not be accessed! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                    if (isTaskSuccessful == true)
                    {
                        //update task settings
                        UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                    }
                }
            }
        }
        #endregion MOTE
        #endregion SET_NETWORKID Task

        #region GET_NETWORKID Task
        /// <summary>
        /// Function used to process incoming serial port data in respect to the GET_NETWORKID task.
        /// </summary>
        private void GetNetworkIdTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, use the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the GET_NETWORKID task
                bool isTaskSuccessful = false;
                if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                {
                    isTaskSuccessful = Common.LookForString(NetworkManager.getNetworkIdTaskDesiredStringToLookFor, NetworkManager.getNetworkIdTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                }
                else if (Common.deviceType == EnumDeviceType.MOTE)
                {
                    isTaskSuccessful = Common.LookForString(Mote.getNetworkIdTaskDesiredStringToLookFor, Mote.getNetworkIdTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                }

                //Check if task is successful
                if (isTaskSuccessful)
                {
                    //get, store, and display the Network ID value
                    if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                    {
                        //get and store the Network Manager's Network ID value
                        Common.NetworkIdNetworkManagerText = Common.GetStringBetweenTwoStrings(NetworkManager.getNetworkIdTaskDesiredStringToLookFor, ">", Common.taskSerialOutputLog);
                        //display the Network Manager's Network ID value
                        TextBoxNetworkIdNetworkManagerText(Common.NetworkIdNetworkManagerText);
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + "'s Network ID is " + Common.NetworkIdNetworkManagerText + "!\r\n");
                    }
                    else if (Common.deviceType == EnumDeviceType.MOTE)
                    {
                        //get and store the Mote's Network ID value
                        Common.NetworkIdMoteText = Common.GetStringBetweenTwoStrings(Mote.getNetworkIdTaskDesiredStringToLookFor, ">", Common.taskSerialOutputLog);
                        //display the Mote's Network ID value
                        TextBoxNetworkIdMoteText(Common.NetworkIdMoteText);
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + "'s Network ID is " + Common.NetworkIdMoteText + "!\r\n");
                    }
                    //update task settings
                    UpdateTaskSettings(EnumTaskName.GET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                }
            }
        }
        #endregion GET_NETWORKID Task

        #region SET_JOINKEY Task
        #region NETWORK_MANAGER
        /// <summary>
        /// Function used to process incoming serial port data in respect to the network manager's SET_JOINKEY task.
        /// </summary>
        private void NetworkManagerSetJoinKeyTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the SET_JOINKEY task
                //Firstly, make sure the Network manager has successfully received the Join Key setting command
                if (Common.isFirstPartSuccessful == false)
                {
                    Common.isFirstPartSuccessful = Common.LookForString(NetworkManager.setJoinKeyTaskDesiredStringToLookFor + Common.JoinKeyNetworkManagerText, NetworkManager.setJoinKeyTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isFirstPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " has successfully received the Join Key setting command!\r\n");
                        //attempt to restart the Network manager
                        if (RestartDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be restarted! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Secondly, make sure the Network manager has successfully restarted
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == false)
                {
                    Common.isSecondPartSuccessful = Common.LookForStringArray(NetworkManager.restartTaskDesiredStringArrayToLookFor, NetworkManager.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isSecondPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " successfully restarted!\r\n");
                        //attempt to log in to Network Manager
                        if (LoginToDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be logged in! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Lastly, access and verify the Network manager's Join Key value
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true)
                {
                    //append text in output_screen
                    Output_ScreenText("\r\nThe " + Common.deviceType + "'s Join Key value has been set to " + Common.JoinKeyNetworkManagerText + "!\r\n");
                    //update task settings
                    UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                }
            }
        }
        #endregion NETWORK_MANAGER

        #region MOTE
        /// <summary>
        /// Function used to process incoming serial port data in respect to the mote's SET_JOINKEY task.
        /// </summary>
        private void MoteSetJoinKeyTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, process the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the SET_JOINKEY task
                //Firstly, make sure the mote has successfully received the Join Key setting command
                if (Common.isFirstPartSuccessful == false)
                {
                    Common.isFirstPartSuccessful = Common.LookForString(Mote.setJoinKeyTaskDesiredStringToLookFor + Common.JoinKeyMoteText, Mote.setJoinKeyTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isFirstPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " has successfully received the Join Key setting command!\r\n");
                        //attempt to restart the Mote
                        if (RestartDevice() == false)
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be restarted! Therefore, the " + Common.taskName +
                                " task failed!\r\n");
                            //update task settings
                            UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                        }
                    }
                }
                //Secondly, make sure the mote has successfully restarted
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == false)
                {
                    Common.isSecondPartSuccessful = Common.LookForStringArray(Mote.restartTaskDesiredStringArrayToLookFor, Mote.restartTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                    if (Common.isSecondPartSuccessful == true)
                    {
                        //append text in output_screen
                        Output_ScreenText("\r\nThe " + Common.deviceType + " successfully restarted!\r\n");
                    }
                }
                //Lastly, access and verify the Mote's Join Key value
                if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true)
                {
                    //append text in output_screen
                    Output_ScreenText("\r\nThe " + Common.deviceType + "'s Join Key value has been set to " + Common.JoinKeyMoteText + "!\r\n");
                    //update task settings
                    UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                }
            }
        }
        #endregion MOTE
        #endregion SET_JOINKEY Task

        #region GET_MOTESNUMBER Task
        /// <summary>
        /// Function used to process incoming serial port data in respect to the GET_MOTESNUMBER task.
        /// </summary>
        private void GetMotesNumberTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, use the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the GET_MOTESNUMBER task
                bool isTaskSuccessful = false;
                if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                {
                    isTaskSuccessful = Common.LookForStringArray(NetworkManager.getMotesNumberTaskDesiredStringArrayToLookFor, NetworkManager.getMotesNumberTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                }

                //Check if task is successful
                if (isTaskSuccessful)
                {
                    //get, store, and display the number of motes found in the SmartMesh IP network
                    if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                    {
                        //get and store the number of motes found in the SmartMesh IP network
                        int.TryParse(Common.GetStringBetweenTwoStringsLastIndex(NetworkManager.getMotesNumberTaskDesiredStringArrayToLookFor[0],
                            NetworkManager.getMotesNumberTaskDesiredStringArrayToLookFor[1]/*", Live"*/,
                            NetworkManager.getMotesNumberTaskDesiredStringArrayToLookFor[0].Length, Common.taskSerialOutputLog),
                        out Common.numberOfFoundMotes);
                        //subtract 1 to omit the network manager in the counting
                        Common.numberOfFoundMotes -= 1;
                        //append text in output_screen
                        Output_ScreenText("\r\nThe number of motes found in the SmartMesh IP network is " + Common.numberOfFoundMotes + "!\r\n");
                        //get the Mac Address and other info of all motes
                        if (Common.GetInfoOfFoundMotes(Common.taskSerialOutputLog))
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe basic information of each mote has been successfully recorded!\r\n");
                        }
                    }
                    //update task settings
                    UpdateTaskSettings(EnumTaskName.GET_MOTESNUMBER, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                }
            }
        }
        #endregion GET_MOTESNUMBER Task

        #region GET_NETWORKSTATISTICS Task
        /// <summary>
        /// Function used to process incoming serial port data in respect to the GET_NETWORKSTATISTICS task.
        /// </summary>
        private void GetNetworkStatisticsTaskSerialDataProcessing()
        {
            //As long as taskTimer is running, use the read and stored incoming serial port data
            if (serialPort1.BytesToRead == 0 && Common.taskTimer.Enabled)
            {
                //Check the outcome of the GET_NETWORKSTATISTICS task
                bool isTaskSuccessful = false;
                if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                {
                    isTaskSuccessful = Common.LookForStringArray(NetworkManager.getNetworkStatisticsTaskDesiredStringArrayToLookFor, NetworkManager.getNetworkStatisticsTaskUndesiredStringArrayToLookFor, Common.taskSerialOutputLog);
                }

                //Check if task is successful
                if (isTaskSuccessful)
                {
                    //get, store, and display the number of motes found in the SmartMesh IP network
                    if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                    {
                        //get the network manager statistics
                        if (Common.GetNetworkManagerStatistics(Common.taskSerialOutputLog))
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe SmartMesh IP network manager statistics have been successfully recorded!\r\n");
                        }
                        //get the network statistics
                        if (Common.GetNetworkStatistics(Common.taskSerialOutputLog))
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe SmartMesh IP network statistics have been successfully recorded!\r\n");
                        }
                        //get the mote statistics
                        if (Common.GetMoteStatistics(Common.taskSerialOutputLog))
                        {
                            //append text in output_screen
                            Output_ScreenText("\r\nThe SmartMesh IP mote statistics have been successfully recorded!\r\n");
                        }
                    }
                    //update task settings
                    UpdateTaskSettings(EnumTaskName.GET_NETWORKSTATISTICS, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, Common.deviceType, Common.taskTimeout);
                }
            }
        }
        #endregion GET_NETWORKSTATISTICS Task
        #endregion Tasks Serial Port Data Processing

        #region ToolTip
        /// <summary>
        /// Function used to set controls' ToolTip.
        /// </summary>
        private void SetControlsToolTip()
        {
            // Create the ToolTip and associate with the Form controls.
            ToolTip controlsToolTip = new ToolTip();
            controlsToolTip.SetToolTip(buttonConnectionNetworkManager, "Connection task => log in to the Network Manager. Click to run.");
            controlsToolTip.SetToolTip(buttonRestartNetworkManager, "Restart task => restart the Network Manager. Click to run.");
            controlsToolTip.SetToolTip(buttonFactoryResetNetworkManager, "Factory Reset task => factory reset the Network Manager. Click to run.");
            controlsToolTip.SetToolTip(comboBoxRadiotestNetworkManager, "Click to turn on or off the radiotest feature of the Network Manager.");
            controlsToolTip.SetToolTip(buttonSetNetworkIdNetworkManager, "Click to assign the Network Manager's Network ID value.");
            controlsToolTip.SetToolTip(buttonGetNetworkIdNetworkManager, "Click to view the Network Manager's Network ID value.");
            controlsToolTip.SetToolTip(buttonSetJoinKeyNetworkManager, "Click to assign the Network Manager's Join Key value.");
            controlsToolTip.SetToolTip(buttonSetNetworkIdMote, "Click to assign the Mote's Network ID value.");
            controlsToolTip.SetToolTip(buttonGetNetworkIdMote, "Click to view the Mote's Network ID value.");
            controlsToolTip.SetToolTip(buttonSetJoinKeyMote, "Click to assign the Mote's Join Key value.");
            controlsToolTip.SetToolTip(buttonConnectionMote, "Connection task => log in to the Mote. Click to run.");
            controlsToolTip.SetToolTip(buttonRestartMote, "Restart task => restart the Mote. Click to run.");
            controlsToolTip.SetToolTip(buttonFactoryResetMote, "Factory Reset task => factory reset the Mote. Click to run.");
            controlsToolTip.SetToolTip(comboBoxModeMote, "Click to assign the mode of the Mote.");
            controlsToolTip.SetToolTip(comboBoxAutoJoinMote, "Click to turn on or off the autoJoin feature of the Mote.");
            controlsToolTip.SetToolTip(comboBoxRadiotestMote, "Click to turn on or off the radiotest feature of the Mote.");
            controlsToolTip.SetToolTip(buttonSendData, "Click to send command typed in by the user.");
            controlsToolTip.SetToolTip(buttonClearInputBox, "Click to clear the input screen.");
            controlsToolTip.SetToolTip(buttonClearOutputBox, "Click to clear the output screen.");
            controlsToolTip.SetToolTip(input_screen, "This screen shows the command typed in by the user.");
            controlsToolTip.SetToolTip(output_screen, "This screen shows a log of events.");
            controlsToolTip.SetToolTip(textBoxNetworkIdNetworkManager, "This screen shows the Network Manager's Network ID value.");
            controlsToolTip.SetToolTip(textBoxJoinKeyNetworkManager, "This screen shows the Network Manager's Join Key value.");
            controlsToolTip.SetToolTip(textBoxNetworkIdMote, "This screen shows the Mote's Network ID value.");
            controlsToolTip.SetToolTip(textBoxJoinKeyMote, "This screen shows the Mote's Join Key value.");
            portSettingsToolStripMenuItem1.ToolTipText = "Click to view or change serial port settings.";
            status_system_status.ToolTipText = "Displaying the serial port connection status.";
            disconnectDevicesToolStripMenuItem.ToolTipText = "Click to disconnect the Network Manager and the Mote from this GUI.";
            firmwareToolStripMenuItem.ToolTipText = "Click to view the firmware-related option(s).";
            loadFirmwareToolStripMenuItem.ToolTipText = "Click to view the GUI for loading firmware on Network Manager, Mote, or Access Point.";
            networkStatisticsToolStripMenuItem.ToolTipText = "Click to view the SmartMesh IP Network Performance Statistics.";
            networkViewToolStripMenuItem.ToolTipText = "Click to view the SmartMesh IP Network Topology Diagram.";
            applicationsToolStripMenuItem.ToolTipText = "Click to view the applications-related option(s).";
            temperatureLoggerToolStripMenuItem.ToolTipText = "Click to start the Temperature Logger application\r\nusing the mote(s)'s internal temperature sensor.";
            temperaturePlotterToolStripMenuItem.ToolTipText = "Click to start the Temperature Plotter application\r\nusing the mote(s)'s external temperature sensor.";
            oscilloscopeToolStripMenuItem.ToolTipText = "Click to start the Oscilloscope application.";
            controlsToolTip.SetToolTip(textBoxTaskOutcomeName, "Displaying the current task's name.");
            controlsToolTip.SetToolTip(textBoxTaskOutcomeStatus, "Displaying the current task's status.");
            controlsToolTip.SetToolTip(textBoxTaskOutcomeResult, "Displaying the current task's result.");
            controlsToolTip.SetToolTip(textBoxTaskOutcomeMessage, "This screen shows a message related to the current task.");
            controlsToolTip.SetToolTip(combo_PortNetworkManager, "Click to choose the serial/COM port to connect to the Network Manager.");
            controlsToolTip.SetToolTip(combo_PortMote, "Click to choose the serial/COM port to connect to the Mote.");
            controlsToolTip.SetToolTip(comboBoxDeviceType, "Click to select the device type.");
            controlsToolTip.SetToolTip(pictureBoxOutcome, "This picture shows the outcome of the current or last-executed task.");
            controlsToolTip.SetToolTip(labelStopWatchTimer, "This label shows the elapsed time of the current or last-executed task.");
            controlsToolTip.SetToolTip(labelInputScreenCharactersCount, "This label shows the current number of characters in the input screen.");
            controlsToolTip.SetToolTip(labelOutputScreenCharactersCount, "This label shows the current number of characters  in the output screen.");
        }
        #endregion ToolTip

        #region Text Control
        /// <summary>
        /// Function used to change 'status_system_status' Text.
        /// </summary>
        /// <param name="text"></param>
        private void Status_System_StatusText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new Status_System_StatusTextDelegate(Status_System_StatusText), new object[] { text });
                return; // Important
            }

            // Assign Text
            status_system_status.Text = text;
        }

        /// <summary>
        /// Function used to change 'output_screen' Text.
        /// </summary>
        /// <param name="text"></param>
        public void Output_ScreenText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                try
                {
                    // Create a delegate of this method and let the form run it.
                    Invoke(new Output_ScreenTextDelegate(Output_ScreenText), new object[] { text });
                }
                catch { }
                return; // Important
            }

            // Assign Text
            try
            {

                //clear 'output_screen' RichTextBox if its text length threshold is exceeded
                if (output_screen.TextLength > Common.Output_ScreenLengthThreshold)
                {
                    output_screen.Clear(); //clear 'output_screen' RichTextBox
                }
                output_screen.AppendText(text);  //Appends new text to the current text of the output textbox 
                output_screen.ScrollToCaret(); //scroll down automatically
            }
            catch { }
        }

        /// <summary>
        /// Function used to change 'input_screen' Text.
        /// </summary>
        /// <param name="text"></param>
        private void Input_ScreenText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new Input_ScreenTextDelegate(Input_ScreenText), new object[] { text });
                return; // Important
            }

            // Assign Text
            input_screen.Text = text;
        }

        /// <summary>
        /// Function used to change 'combo_Port' Text.
        /// </summary>
        /// <param name="text"></param>
        public void Combo_PortText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new Combo_PortTextDelegate(Combo_PortText), new object[] { text });
                return; // Important
            }

            //Grab the proper COM Port comboBox
            Common.comboBox_Port = Common.GetProperSerialPortComboBox();
            // Assign Text
            Common.comboBox_Port.Text = text;
        }

        /// <summary>
        /// Function used to change 'textBoxTaskOutcomeName' Text.
        /// </summary>
        /// <param name="text"></param>
        private void TaskOutcomeNameText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TaskOutcomeNameTextDelegate(TaskOutcomeNameText), new object[] { text });
                return; // Important
            }

            // Assign Text
            textBoxTaskOutcomeName.Text = text;
        }

        /// <summary>
        /// Function used to change 'textBoxTaskOutcomeStatus' Text.
        /// </summary>
        /// <param name="text"></param>
        private void TaskOutcomeStatusText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TaskOutcomeStatusTextDelegate(TaskOutcomeStatusText), new object[] { text });
                return; // Important
            }

            // Assign Text
            textBoxTaskOutcomeStatus.Text = text;
        }

        /// <summary>
        /// Function used to change 'textBoxTaskOutcomeResult' Text.
        /// </summary>
        /// <param name="text"></param>
        private void TaskOutcomeResultText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TaskOutcomeResultTextDelegate(TaskOutcomeResultText), new object[] { text });
                return; // Important
            }

            // Assign Text
            textBoxTaskOutcomeResult.Text = text;
        }

        /// <summary>
        /// Function used to change 'textBoxTaskOutcomeMessage' Text.
        /// </summary>
        /// <param name="text"></param>
        private void TaskOutcomeMessageText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TaskOutcomeMessageTextDelegate(TaskOutcomeMessageText), new object[] { text });
                return; // Important
            }

            // Assign Text
            textBoxTaskOutcomeMessage.Text = text;
        }

        /// <summary>
        /// Function used to change 'comboBoxTaskOutcomeDeviceType' Text.
        /// </summary>
        /// <param name="text"></param>
        private void TaskOutcomeDeviceTypeText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TaskOutcomeDeviceTypeTextDelegate(TaskOutcomeDeviceTypeText), new object[] { text });
                return; // Important
            }

            // Assign Text
            comboBoxDeviceType.SelectedItem = text;
        }

        /// <summary>
        /// Function used to change 'comboBoxModeMote' Text.
        /// </summary>
        /// <param name="text"></param>
        private void SetComboBoxModeMoteText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new SetComboBoxModeMoteTextDelegate(SetComboBoxModeMoteText), new object[] { text });
                return; // Important
            }

            // Assign Text
            comboBoxModeMote.SelectedItem = text;
        }

        /// <summary>
        /// Function used to get 'comboBoxModeMote' Text.
        /// </summary>
        /// <returns></returns>
        private string GetComboBoxModeMoteText()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new GetComboBoxModeMoteTextDelegate(GetComboBoxModeMoteText));
                // return 'comboBoxModeMote' Text
                return comboBoxModeMote.SelectedItem.ToString();
            }

            // return 'comboBoxModeMote' Text
            return comboBoxModeMote.SelectedItem.ToString();
        }

        /// <summary>
        /// Function used to change 'labelStopWatchTimer' Text.
        /// </summary>
        /// <param name="text"></param>
        private void LabelStopWatchTimerText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new LabelStopWatchTimerTextDelegate(LabelStopWatchTimerText), new object[] { text });
                return; // Important
            }

            // Assign Text
            if (labelStopWatchTimer.Visible == false)
            {
                labelStopWatchTimer.Visible = true;
            }
            labelStopWatchTimer.Text = text;
        }

        /// <summary>
        /// Function used to change 'textBoxNetworkIdNetworkManager' Text.
        /// </summary>
        /// <param name="text"></param>
        private void TextBoxNetworkIdNetworkManagerText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TextBoxNetworkIdNetworkManagerTextDelegate(TextBoxNetworkIdNetworkManagerText), new object[] { text });
                return; // Important
            }

            // Assign Text
            textBoxNetworkIdNetworkManager.Text = text;
        }

        /// <summary>
        /// Function used to change 'textBoxNetworkIdMote' Text.
        /// </summary>
        /// <param name="text"></param>
        private void TextBoxNetworkIdMoteText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TextBoxNetworkIdMoteTextDelegate(TextBoxNetworkIdMoteText), new object[] { text });
                return; // Important
            }

            // Assign Text
            textBoxNetworkIdMote.Text = text;
        }

        /// <summary>
        /// Function used to change 'textBoxJoinKeyNetworkManager' Text.
        /// </summary>
        /// <param name="text"></param>
        private void TextBoxJoinKeyNetworkManagerText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TextBoxNetworkIdNetworkManagerTextDelegate(TextBoxJoinKeyNetworkManagerText), new object[] { text });
                return; // Important
            }

            // Assign Text
            textBoxJoinKeyNetworkManager.Text = text;
        }

        /// <summary>
        /// Function used to change 'textBoxJoinKeyMote' Text.
        /// </summary>
        /// <param name="text"></param>
        private void TextBoxJoinKeyMoteText(string text)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TextBoxNetworkIdMoteTextDelegate(TextBoxJoinKeyMoteText), new object[] { text });
                return; // Important
            }

            // Assign Text
            textBoxJoinKeyMote.Text = text;
        }

        /// <summary>
        /// Function used to get 'combo_PortNetworkManager' Text.
        /// </summary>
        /// <returns></returns>
        private string GetComboBoxPortNetworkManagerText()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new GetComboBoxPortNetworkManagerTextDelegate(GetComboBoxPortNetworkManagerText));
                // return 'combo_PortNetworkManager' Text
                if (combo_PortNetworkManager.SelectedItem != null)
                {
                    return combo_PortNetworkManager.SelectedItem.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }

            // return 'combo_PortNetworkManager' Text
            if (combo_PortNetworkManager.SelectedItem != null)
            {
                return combo_PortNetworkManager.SelectedItem.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion Text Control

        #region TextChanged Control
        /// <summary>
        /// Function called when the 'input_screen' Text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_screen_TextChanged(object sender, EventArgs e)
        {
            labelInputScreenCharactersCount.Text = input_screen.Text.Length + " character(s)";
        }

        /// <summary>
        /// Function called when the 'output_screen' Text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Output_screen_TextChanged(object sender, EventArgs e)
        {
            labelOutputScreenCharactersCount.Text = output_screen.Text.Length + " character(s)";
        }

        /// <summary>
        /// Function called when the 'textBoxNetworkIdNetworkManager' Text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxNetworkIdNetworkManager_TextChanged(object sender, EventArgs e)
        {
            labelNetworkIdCharactersCountNetworkManager.Text = textBoxNetworkIdNetworkManager.Text.Length + " character(s)";
        }

        /// <summary>
        /// Function called when the 'textBoxJoinKeyNetworkManager' Text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxJoinKeyNetworkManager_TextChanged(object sender, EventArgs e)
        {
            labelJoinKeyCharactersCountNetworkManager.Text = textBoxJoinKeyNetworkManager.Text.Length + " character(s)";
        }

        /// <summary>
        /// Function called when the 'textBoxNetworkIdMote' Text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxNetworkIdMote_TextChanged(object sender, EventArgs e)
        {
            labelNetworkIdCharactersCountMote.Text = textBoxNetworkIdMote.Text.Length + " character(s)";
        }

        /// <summary>
        /// Function called when the 'textBoxJoinKeyMote' Text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxJoinKeyMote_TextChanged(object sender, EventArgs e)
        {
            labelJoinKeyCharactersCountMote.Text = textBoxJoinKeyMote.Text.Length + " character(s)";
        }
        #endregion TextChanged Control

        #region ForeColor Control
        /// <summary>
        /// Function used to change 'status_system_status' ForeColor.
        /// </summary>
        /// <param name="foreColor"></param>
        private void Status_System_StatusForeColor(Color foreColor)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new Status_System_StatusForeColorDelegate(Status_System_StatusForeColor), new object[] { foreColor });
                return; // Important
            }

            // Assign ForeColor
            status_system_status.ForeColor = foreColor;
        }
        #endregion ForeColor Control

        #region Enabled Control
        /// <summary>
        /// Function used to change 'buttonConnection' and 'combo_Port' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        private void ButtonConnectionEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new ButtonConnectionEnabledDelegate(ButtonConnectionEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            #region NETWORK_MANAGER
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                buttonConnectionNetworkManager.Enabled = enabled;
                combo_PortNetworkManager.Enabled = enabled;
            }
            #endregion NETWORK_MANAGER
            #region MOTE
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                buttonConnectionMote.Enabled = enabled;
                combo_PortMote.Enabled = enabled;
            }
            #endregion MOTE
        }

        /// <summary>
        /// Function used to change 'buttonSendData' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        private void ButtonSendDataEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new ButtonSendDataEnabledDelegate(ButtonSendDataEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            //Check if any device is connected to the GUI
            buttonSendData.Enabled = enabled;
        }

        /// <summary>
        /// Function used to change the Enabled feature of buttons used for serial port data inputting.
        /// </summary>
        /// <param name="enabled"></param>
        private void SerialPort_DataInputButtons_Enabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new SerialPort_DataInputButtons_EnabledDelegate(SerialPort_DataInputButtons_Enabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            buttonSendData.Enabled = enabled;
            #region NETWORK_MANAGER
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER && !(NetworkManager.isConnectedToGUI == false && enabled == true))
            {
                combo_PortNetworkManager.Enabled = enabled;
                comboBoxRadiotestNetworkManager.Enabled = enabled;
                textBoxNetworkIdNetworkManager.Enabled = enabled;
                textBoxJoinKeyNetworkManager.Enabled = enabled;
                foreach (Button button in groupBoxTasks.Controls.OfType<Button>())
                {
                    button.Enabled = enabled; //enable the retrieved button
                    Application.DoEvents(); //refresh GUI
                }
            }
            #endregion NETWORK_MANAGER
            #region MOTE
            else if (Common.deviceType == EnumDeviceType.MOTE && !(Mote.isConnectedToGUI == false && enabled == true))
            {
                combo_PortMote.Enabled = enabled;
                comboBoxModeMote.Enabled = enabled;
                comboBoxAutoJoinMote.Enabled = enabled;
                comboBoxRadiotestMote.Enabled = enabled;
                textBoxNetworkIdMote.Enabled = enabled;
                textBoxJoinKeyMote.Enabled = enabled;
                foreach (Button button in groupBoxTasksMote.Controls.OfType<Button>())
                {
                    button.Enabled = enabled; //enable the retrieved button
                    Application.DoEvents(); //refresh GUI
                }
            }
            #endregion MOTE
        }

        /// <summary>
        /// Function used to change the Enabled feature of buttons in Network_Manager Tasks GroupBox.
        /// </summary>
        /// <param name="enabled"></param>
        private void NetworkManager_TaskButtons_Enabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new NetworkManager_TaskButtons_EnabledDelegate(NetworkManager_TaskButtons_Enabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled  
            combo_PortNetworkManager.Enabled = enabled;
            comboBoxRadiotestNetworkManager.Enabled = enabled;
            textBoxNetworkIdNetworkManager.Enabled = enabled;
            textBoxJoinKeyNetworkManager.Enabled = enabled;
            foreach (Button button in groupBoxTasks.Controls.OfType<Button>())
            {
                button.Enabled = enabled; //enable the retrieved button
                Application.DoEvents(); //refresh GUI
            }
        }

        /// <summary>
        /// Function used to change the Enabled feature of buttons in Mote Tasks GroupBox.
        /// </summary>
        /// <param name="enabled"></param>
        private void Mote_TaskButtons_Enabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new Mote_TaskButtons_EnabledDelegate(Mote_TaskButtons_Enabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            combo_PortMote.Enabled = enabled;
            comboBoxModeMote.Enabled = enabled;
            comboBoxAutoJoinMote.Enabled = enabled;
            comboBoxRadiotestMote.Enabled = enabled;
            textBoxNetworkIdMote.Enabled = enabled;
            textBoxJoinKeyMote.Enabled = enabled;
            foreach (Button button in groupBoxTasksMote.Controls.OfType<Button>())
            {
                button.Enabled = enabled; //enable the retrieved button
                Application.DoEvents(); //refresh GUI
            }
        }

        /// <summary>
        /// Function used to change 'comboBoxDeviceType' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        private void ComboBoxDeviceTypeEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new ComboBoxDeviceTypeEnabledDelegate(ComboBoxDeviceTypeEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            comboBoxDeviceType.Enabled = enabled;
        }

        /// <summary>
        /// Function used to change 'disconnectDevicesToolStripMenuItem' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        private void DisconnectDevicesToolStripMenuItemEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new DisconnectDevicesToolStripMenuItemEnabledDelegate(DisconnectDevicesToolStripMenuItemEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            disconnectDevicesToolStripMenuItem.Enabled = enabled;
        }

        /// <summary>
        /// Function used to change 'firmwareToolStripMenuItem' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        private void FirmwareToolStripMenuItemEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new FirmwareToolStripMenuItemEnabledDelegate(FirmwareToolStripMenuItemEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            firmwareToolStripMenuItem.Enabled = enabled;
        }

        /// <summary>
        /// Function used to change 'applicationsToolStripMenuItem' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        public void ApplicationsToolStripMenuItemEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new ApplicationsToolStripMenuItemEnabledDelegate(ApplicationsToolStripMenuItemEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            applicationsToolStripMenuItem.Enabled = enabled;
        }

        /// <summary>
        /// Function used to change 'temperatureLoggerToolStripMenuItem' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        public void TemperatureLoggerToolStripMenuItemEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TemperatureLoggerToolStripMenuItemEnabledDelegate(TemperatureLoggerToolStripMenuItemEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            temperatureLoggerToolStripMenuItem.Enabled = enabled;
        }

        /// <summary>
        /// Function used to change 'temperaturePlotterToolStripMenuItem' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        public void TemperaturePlotterToolStripMenuItemEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TemperaturePlotterToolStripMenuItemEnabledDelegate(TemperaturePlotterToolStripMenuItemEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            temperaturePlotterToolStripMenuItem.Enabled = enabled;
        }

        /// <summary>
        /// Function used to change 'oscilloscopeToolStripMenuItem' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        public void OscilloscopeToolStripMenuItemEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new OscilloscopeToolStripMenuItemEnabledDelegate(OscilloscopeToolStripMenuItemEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            oscilloscopeToolStripMenuItem.Enabled = enabled;
        }

        /// <summary>
        /// Function used to change 'networkStatisticsToolStripMenuItem' Enabled.
        /// </summary>
        /// <param name="enabled"></param>
        public void NetworkStatisticsToolStripMenuItemEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new NetworkStatisticsToolStripMenuItemEnabledDelegate(NetworkStatisticsToolStripMenuItemEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            networkStatisticsToolStripMenuItem.Enabled = enabled;
        }
        #endregion Enabled Control

        #region Select Control
        /// <summary>
        /// Function used to select 'input_screen'.
        /// </summary>
        private void Input_ScreenSelect()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new Input_ScreenSelectDelegate(Input_ScreenSelect));
                return; // Important
            }

            // Select the control
            input_screen.Select();
        }

        /// <summary>
        /// Function used to select 'output_screen'.
        /// </summary>
        private void Output_ScreenSelect()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new Output_ScreenSelectDelegate(Output_ScreenSelect));
                return; // Important
            }

            // Select the control
            output_screen.Select();
        }

        /// <summary>
        /// Function used to select 'buttonConnection'.
        /// </summary>
        private void ButtonConnectionSelect()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new ButtonConnectionSelectDelegate(ButtonConnectionSelect));
                return; // Important
            }

            // Select the control
            #region NETWORK_MANAGER
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                buttonConnectionNetworkManager.Select();
            }
            #endregion NETWORK_MANAGER
            #region MOTE
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                buttonConnectionMote.Select();
            }
            #endregion MOTE
        }

        /// <summary>
        /// Function used to select 'buttonSendData'.
        /// </summary>
        private void ButtonSendDataSelect()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new ButtonSendDataSelectDelegate(ButtonSendDataSelect));
                return; // Important
            }

            // Select the control
            buttonSendData.Select();
        }

        /// <summary>
        /// Function used to select 'textBoxNetworkIdNetworkManager'.
        /// </summary>
        private void TextBoxNetworkIdNetworkManagerSelect()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TextBoxNetworkIdNetworkManagerSelectDelegate(TextBoxNetworkIdNetworkManagerSelect));
                return; // Important
            }

            // Select the control
            textBoxNetworkIdNetworkManager.Select();
        }

        /// <summary>
        /// Function used to select 'textBoxJoinKeyNetworkManager'.
        /// </summary>
        private void TextBoxJoinKeyNetworkManagerSelect()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TextBoxJoinKeyNetworkManagerSelectDelegate(TextBoxJoinKeyNetworkManagerSelect));
                return; // Important
            }

            // Select the control
            textBoxJoinKeyNetworkManager.Select();
        }

        /// <summary>
        /// Function used to select 'textBoxNetworkIdMote'.
        /// </summary>
        private void TextBoxNetworkIdMoteSelect()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TextBoxNetworkIdMoteSelectDelegate(TextBoxNetworkIdMoteSelect));
                return; // Important
            }

            // Select the control
            textBoxNetworkIdMote.Select();
        }

        /// <summary>
        /// Function used to select 'textBoxJoinKeyMote'.
        /// </summary>
        private void TextBoxJoinKeyMoteSelect()
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new TextBoxJoinKeyMoteSelectDelegate(TextBoxJoinKeyMoteSelect));
                return; // Important
            }

            // Select the control
            textBoxJoinKeyMote.Select();
        }
        #endregion Select Control

        #region PictureBox Control
        /// <summary>
        /// Function used to change 'pictureBoxOutcome' image.
        /// </summary>
        /// <param name="image"></param>
        private void PictureBoxOutcomeImage(Image image)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new PictureBoxOutcomeImageDelegate(PictureBoxOutcomeImage), new object[] { image });
                return; // Important
            }

            // Assign Image
            if (image == null)
            {
                pictureBoxOutcome.Visible = false;
            }
            else
            {
                pictureBoxOutcome.Visible = true;
            }
            pictureBoxOutcome.Image = image;
        }
        #endregion PictureBox Control

        #region ComboBox Item Selection Control
        /// <summary>
        /// Device Type comboBox item selection execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxDeviceType_SelectedValueChanged(object sender, EventArgs e)
        {
            //Assign device type
            Enum.TryParse(comboBoxDeviceType.SelectedItem.ToString(), out Common.deviceType);
            //Check if any device is connected to the GUI
            if (NetworkManager.isConnectedToGUI == true || Mote.isConnectedToGUI == true)
            {
                //Grab the proper COM Port comboBox
                Common.comboBox_Port = Common.GetProperSerialPortComboBox();
                //Find and display the last successfully used serial port in COM Port Box
                FindAndDisplayLastUsedSerialPortName();
            }
            //Enable device related task button
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                if (NetworkManager.isConnectedToGUI == true)
                {
                    NetworkManager_TaskButtons_Enabled(true); //enable Network Manager Task buttons
                }
                else //if (NetworkManager.isConnectedToDevice == false)
                {
                    NetworkManager_TaskButtons_Enabled(false); //disable Network Manager Task buttons
                    ButtonConnectionEnabled(true); //enable the Network Manager Connection buttons
                }
                Mote_TaskButtons_Enabled(false); //disable Mote Task buttons
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                if (Mote.isConnectedToGUI == true)
                {
                    Mote_TaskButtons_Enabled(true); //enable Mote Task buttons
                }
                else //if (Mote.isConnectedToGUI == false)
                {
                    Mote_TaskButtons_Enabled(false); //disable Mote Task buttons
                    ButtonConnectionEnabled(true); //enable the Mote Connection buttons
                }
                NetworkManager_TaskButtons_Enabled(false); //disable Network Manager Task buttons
            }
        }

        /// <summary>
        /// Network Manager's COM Port Box item selection execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_Port_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //perform Network Manager's CONNECTION task
            ButtonConnection_Click(sender, e);
        }

        /// <summary>
        /// Mote's COM Port Box item selection execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_PortMote_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //perform Mote's CONNECTION task
            ButtonConnectionMote_Click(sender, e);
        }
        #endregion ComboBox Item Selection Control

        #region Task Clicks/Execution
        #region NETWORK_MANAGER
        /// <summary>
        /// Network Manager Connection button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonConnection_Click(object sender, EventArgs e)
        {
            if (serialPort1 == null)
            {
                serialPort1 = new SerialPort();
            }
            //update task settings
            UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.NETWORK_MANAGER, NetworkManager.connectionTaskTimeout);
            Status_System_StatusText("Not Connected!");   //display "Not Connected"
            Status_System_StatusForeColor(Color.DarkRed); //update connection status text color
            if (string.IsNullOrEmpty(GetComboBoxPortNetworkManagerText()) == true) //check if the COM Port Box is empty or not
            {
                //append text in output_screen
                Output_ScreenText("\r\nIn order to properly execute the " + Common.taskName +
                   " task, please choose the proper COM port in the COM Port Box.\r\n");
                //update task settings
                UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                return; // exit function
            }
            //Assign Serial Port settings
            UpdateSerialPortSettings(GetComboBoxPortNetworkManagerText(), serialPort1.BaudRate, serialPort1.DataBits, serialPort1.Parity, serialPort1.StopBits, serialPort1.Handshake);
            //Attempt to open and write to the assigned serial port
            if (OpenSerialPortThread(Common.serialPortThreadTimeout) == true) //if serial port is open
            {
                //attempt to log in to Network Manager
                if (LoginToDevice() == false)
                {
                    //append text in output_screen
                    Output_ScreenText("\r\nThe " + Common.taskName + " task failed because either " + serialPort1.PortName +
                    " could not receive the sent data or the login credentials used are not valid!\r\n");
                    //update task settings
                    UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                }
            }
            else //if serial port is not open
            {
                Output_ScreenText("\r\nThe " + Common.taskName + " task failed because " + serialPort1.PortName +
                    " could not be opened!\r\n"); //append text in output_screen
                //update task settings
                UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Network Manager Restart button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRestart_Click(object sender, EventArgs e)
        {
            //Check if the Network Manager and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }
            //update task settings
            UpdateTaskSettings(EnumTaskName.RESTART, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.NETWORK_MANAGER, NetworkManager.restartTaskTimeout);
            //attempt to restart the Network Manager
            if (RestartDevice() == false)
            {
                //update task settings
                UpdateTaskSettings(EnumTaskName.RESTART, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Network Manager Factory Reset button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonFactoryReset_Click(object sender, EventArgs e)
        {
            //Check if the Network Manager and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }
            //update task settings
            UpdateTaskSettings(EnumTaskName.FACTORYRESET, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.NETWORK_MANAGER, NetworkManager.factoryResetTaskTimeout);
            //attempt to factory reset the Network Manager
            if (FactoryResetDevice() == false)
            {
                UpdateTaskSettings(EnumTaskName.FACTORYRESET, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Network Manager Radiotest comboBox item selection execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxRadiotestNetworkManager_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Check if the Network Manager and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }
            //update task settings
            UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.NETWORK_MANAGER, NetworkManager.radiotestTaskTimeout);
            //Assign an integer value to the selected 'comboBoxRadiotestNetworkManager' item and store the value
            if (comboBoxRadiotestNetworkManager.SelectedItem.ToString().ToUpper() == "OFF")
            {
                Common.ComboBoxRadiotestNetworkManagerText = "off";
            }
            else if (comboBoxRadiotestNetworkManager.SelectedItem.ToString().ToUpper() == "ON")
            {
                Common.ComboBoxRadiotestNetworkManagerText = "on";
            }
            //attempt to assign the Network Manager's radiotest value
            if (AssignRadiotest(Common.ComboBoxRadiotestNetworkManagerText) == false)
            {
                //update task settings
                UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Network Manager Set Network ID button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSetNetworkIdNetworkManager_Click(object sender, EventArgs e)
        {
            //Check if the Network Manager and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }

            //update task settings
            UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.NETWORK_MANAGER, NetworkManager.setNetworkIdTaskTimeout);
            //check if the Network Manager's Network ID textbox is empty and its content is an integer
            if (string.IsNullOrEmpty(textBoxNetworkIdNetworkManager.Text) == true || int.TryParse(textBoxNetworkIdNetworkManager.Text, out int networkIdValue) == false)
            {
                //append text in output_screen
                Output_ScreenText("\r\nIn order to properly execute the " + Common.taskName +
                   " task, please enter an integer between " + NetworkManager.networkIdLowerLimit + " and " + NetworkManager.networkIdUpperLimit +
                   " in the " + Common.deviceType + "'s Network ID textbox.\r\n");
                //update task settings
                UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                return; // exit function
            }
            //check if the Network Manager's Network ID textbox content is an integer between the defined limits
            if (networkIdValue < NetworkManager.networkIdLowerLimit || networkIdValue > NetworkManager.networkIdUpperLimit)
            {
                //append text in output_screen
                Output_ScreenText("\r\nIn order to properly execute the " + Common.taskName +
                   " task, please enter an integer between " + NetworkManager.networkIdLowerLimit + " and " + NetworkManager.networkIdUpperLimit +
                   " in the " + Common.deviceType + "'s Network ID textbox.\r\n");
                //update task settings
                UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                return; // exit function
            }
            //store the entered Network Manager's Network ID value
            Common.NetworkIdNetworkManagerText = textBoxNetworkIdNetworkManager.Text;
            //attempt to set the Network ID on the Network Manager
            if (AssignNetworkId(Common.NetworkIdNetworkManagerText) == false)
            {
                UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Network Manager Get Network ID button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGetNetworkIdNetworkManager_Click(object sender, EventArgs e)
        {
            //check if the Network Manager and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }

            //update task settings
            UpdateTaskSettings(EnumTaskName.GET_NETWORKID, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.NETWORK_MANAGER, NetworkManager.getNetworkIdTaskTimeout);
            //reset 'textBoxNetworkIdNetworkManager' cursor at initial position
            TextBoxNetworkIdNetworkManagerText("\r");
            //clear 'textBoxNetworkIdNetworkManager'
            textBoxNetworkIdNetworkManager.Clear();
            //attempt to get the Network ID on the Network Manager
            if (AccessNetworkId() == false)
            {
                UpdateTaskSettings(EnumTaskName.GET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Network Manager Set Join Key button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSetJoinKeyNetworkManager_Click(object sender, EventArgs e)
        {
            //Check if the Network Manager and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }

            //update task settings
            UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.NETWORK_MANAGER, NetworkManager.setJoinKeyTaskTimeout);
            //check if the Network Manager's Join Key textbox is empty, and Join Key textbox content's characters are all hex, and Join Key textbox content has 32 characters
            if (string.IsNullOrEmpty(textBoxJoinKeyNetworkManager.Text) == true || Common.OnlyHexInString(textBoxJoinKeyNetworkManager.Text) == false || textBoxJoinKeyNetworkManager.Text.Length != NetworkManager.joinKeyCharacterCountLimit)
            {
                //append text in output_screen
                Output_ScreenText("\r\nIn order to properly execute the " + Common.taskName +
                   " task, please enter a " + NetworkManager.joinKeyCharacterCountLimit + "-character Hex value (without the '0x' prefix) in the " + Common.deviceType + "'s Join Key textbox.\r\n");
                //update task settings
                UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                return; // exit function
            }
            //store the entered Network Manager's Join Key value
            Common.JoinKeyNetworkManagerText = textBoxJoinKeyNetworkManager.Text;
            //attempt to set the Join Key on the Network Manager
            if (AssignJoinKey(Common.JoinKeyNetworkManagerText) == false)
            {
                UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Get Motes Number task execution.
        /// </summary>
        public void GetMotesNumberTask()
        {
            //check if the Network Manager and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }

            //update task settings
            UpdateTaskSettings(EnumTaskName.GET_MOTESNUMBER, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.NETWORK_MANAGER, NetworkManager.getMotesNumberTaskTimeout);
            //attempt to get the number of motes found in the SmartMesh IP network
            if (AccessMotesNumber() == false)
            {
                UpdateTaskSettings(EnumTaskName.GET_MOTESNUMBER, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Get Network Statistics task execution.
        /// </summary>
        public void GetNetworkStatisticsTask()
        {
            //check if the Network Manager and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }

            //update task settings
            UpdateTaskSettings(EnumTaskName.GET_NETWORKSTATISTICS, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.NETWORK_MANAGER, NetworkManager.getNetworkStatisticsTaskTimeout);
            //attempt to get the network statistics data
            if (AccessNetworkStatistics() == false)
            {
                UpdateTaskSettings(EnumTaskName.GET_NETWORKSTATISTICS, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }
        #endregion NETWORK_MANAGER

        #region MOTE
        /// <summary>
        /// Mote Connection button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonConnectionMote_Click(object sender, EventArgs e)
        {
            //update task settings
            UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.MOTE, Mote.connectionTaskTimeout);
            Status_System_StatusText("Not Connected!");   //display "Not Connected"
            Status_System_StatusForeColor(Color.DarkRed); //update connection status text color
            if (string.IsNullOrEmpty(combo_PortMote.Text.ToString()) == true) //check if the COM Port Box is empty or not
            {
                //append text in output_screen
                Output_ScreenText("\r\nIn order to properly execute the " + Common.taskName +
                   " task, please choose the proper COM port in the COM Port Box.\r\n");
                //update task settings
                UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                return; // exit function
            }
            //Assign Serial Port settings
            UpdateSerialPortSettings(combo_PortMote.SelectedItem.ToString(), serialPort1.BaudRate, serialPort1.DataBits, serialPort1.Parity, serialPort1.StopBits, serialPort1.Handshake);
            //Attempt to open and write to the assigned serial port
            if (OpenSerialPortThread(Common.serialPortThreadTimeout) == true) //if serial port is open
            {
                //attempt to log in to mote
                if (LoginToDevice() == false)
                {
                    //append text in output_screen
                    Output_ScreenText("\r\nThe " + Common.taskName + " task failed because either " + serialPort1.PortName +
                    " could not receive the sent data or the login credentials used are not valid!\r\n");
                    //update task settings
                    UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                }
            }
            else //if serial port is not open
            {
                Output_ScreenText("\r\nThe " + Common.taskName + " task failed because " + serialPort1.PortName +
                    " could not be opened!\r\n"); //append text in output_screen
                //update task settings
                UpdateTaskSettings(EnumTaskName.CONNECTION, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Mote Restart button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRestartMote_Click(object sender, EventArgs e)
        {
            //Check if the Mote and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }
            //update task settings
            UpdateTaskSettings(EnumTaskName.RESTART, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.MOTE, Mote.restartTaskTimeout);
            //attempt to restart the mote
            if (RestartDevice() == false)
            {
                //update task settings
                UpdateTaskSettings(EnumTaskName.RESTART, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Mote Factory Reset button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonFactoryResetMote_Click(object sender, EventArgs e)
        {
            //Check if the Mote and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }
            //update task settings
            UpdateTaskSettings(EnumTaskName.FACTORYRESET, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.MOTE, Mote.factoryResetTaskTimeout);
            //attempt to factory reset the mote
            if (FactoryResetDevice() == false)
            {
                UpdateTaskSettings(EnumTaskName.FACTORYRESET, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Mote Mode comboBox item selection execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxModeMote_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Check if the Mote and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }
            //update task settings
            UpdateTaskSettings(EnumTaskName.MODE, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.MOTE, Mote.modeTaskTimeout);
            //store the 'comboBoxModeMote' text
            Common.ComboBoxModeMoteText = comboBoxModeMote.SelectedItem.ToString();
            //attempt to assign the mote's mode
            if (AssignMoteMode(Common.ComboBoxModeMoteText) == false)
            {
                UpdateTaskSettings(EnumTaskName.MODE, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Mote AutoJoin comboBox item selection execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxAutoJoinMote_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Check if the Mote and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }
            //update task settings
            UpdateTaskSettings(EnumTaskName.AUTOJOIN, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.MOTE, Mote.autoJoinTaskTimeout);
            //Assign an integer value to the selected 'comboBoxAutoJoinMote' item and store the value
            if (comboBoxAutoJoinMote.SelectedItem.ToString().ToUpper() == "OFF")
            {
                Common.ComboBoxAutoJoinMoteValue = 0;
            }
            else if (comboBoxAutoJoinMote.SelectedItem.ToString().ToUpper() == "ON")
            {
                Common.ComboBoxAutoJoinMoteValue = 1;
            }
            //attempt to assign the mote's autoJoin
            if (AssignMoteAutoJoin(Common.ComboBoxAutoJoinMoteValue.ToString()) == false)
            {
                UpdateTaskSettings(EnumTaskName.AUTOJOIN, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Mote Radiotest comboBox item selection execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxRadiotestMote_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Check if the Mote and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }
            //update task settings
            UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.MOTE, Mote.radiotestTaskTimeout);
            //Assign an integer value to the selected 'comboBoxRadiotestMote' item and store the value
            if (comboBoxRadiotestMote.SelectedItem.ToString().ToUpper() == "OFF")
            {
                Common.ComboBoxRadiotestMoteText = "off";
            }
            else if (comboBoxRadiotestMote.SelectedItem.ToString().ToUpper() == "ON")
            {
                Common.ComboBoxRadiotestMoteText = "on";
            }
            //attempt to assign the mote's radiotest value
            if (AssignRadiotest(Common.ComboBoxRadiotestMoteText) == false)
            {
                UpdateTaskSettings(EnumTaskName.RADIOTEST, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Mote Set Network ID button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSetNetworkIdMote_Click(object sender, EventArgs e)
        {
            //Check if the Mote and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }

            //update task settings
            UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.MOTE, Mote.setNetworkIdTaskTimeout);
            //check if the Mote's Network ID textbox is empty and its content is an integer
            if (string.IsNullOrEmpty(textBoxNetworkIdMote.Text) == true || int.TryParse(textBoxNetworkIdMote.Text, out int networkIdValue) == false)
            {
                //append text in output_screen
                Output_ScreenText("\r\nIn order to properly execute the " + Common.taskName +
                   " task, please enter an integer between " + Mote.networkIdLowerLimit + " and " + Mote.networkIdUpperLimit +
                   " in the " + Common.deviceType + "'s Network ID textbox.\r\n");
                //update task settings
                UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                return; // exit function
            }
            //check if the Mote's Network ID textbox content is an integer between the defined limits
            if (networkIdValue < Mote.networkIdLowerLimit || networkIdValue > Mote.networkIdUpperLimit)
            {
                //append text in output_screen
                Output_ScreenText("\r\nIn order to properly execute the " + Common.taskName +
                   " task, please enter an integer between " + Mote.networkIdLowerLimit + " and " + Mote.networkIdUpperLimit +
                   " in the " + Common.deviceType + "'s Network ID textbox.\r\n");
                //update task settings
                UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                return; // exit function
            }
            //store the entered Mote's Network ID value
            Common.NetworkIdMoteText = textBoxNetworkIdMote.Text;
            //attempt to set the Network ID on the Mote
            if (AssignNetworkId(Common.NetworkIdMoteText) == false)
            {
                UpdateTaskSettings(EnumTaskName.SET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Mote Get Network ID button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGetNetworkIdMote_Click(object sender, EventArgs e)
        {
            //Check if the Mote and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }

            //update task settings
            UpdateTaskSettings(EnumTaskName.GET_NETWORKID, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.MOTE, Mote.getNetworkIdTaskTimeout);
            //reset 'textBoxNetworkIdMote' cursor at initial position
            TextBoxNetworkIdMoteText("\r");
            //clear 'textBoxNetworkIdMote'
            textBoxNetworkIdMote.Clear();
            //attempt to get the Network ID on the Mote
            if (AccessNetworkId() == false)
            {
                UpdateTaskSettings(EnumTaskName.GET_NETWORKID, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }

        /// <summary>
        /// Mote Set Join Key button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSetJoinKeyMote_Click(object sender, EventArgs e)
        {
            //Check if the Mote and GUI are connected via serial port
            if (IsConnectedToDevice() == false)
            {
                return;
            }

            //update task settings
            UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, EnumDeviceType.MOTE, Mote.setJoinKeyTaskTimeout);
            //check if the Mote's Join Key textbox is empty, and Join Key textbox content's characters are all hex, and Join Key textbox content has 32 characters
            if (string.IsNullOrEmpty(textBoxJoinKeyMote.Text) == true || Common.OnlyHexInString(textBoxJoinKeyMote.Text) == false || textBoxJoinKeyMote.Text.Length != Mote.joinKeyCharacterCountLimit)
            {
                //append text in output_screen
                Output_ScreenText("\r\nIn order to properly execute the " + Common.taskName +
                   " task, please enter a " + Mote.joinKeyCharacterCountLimit + "-character Hex value (without the '0x' prefix) in the " + Common.deviceType + "'s Join Key textbox.\r\n");
                //update task settings
                UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
                return; // exit function
            }
            //store the entered Mote's Join Key value
            Common.JoinKeyMoteText = textBoxJoinKeyMote.Text;
            //attempt to set the Join Key on the Mote
            if (AssignJoinKey(Common.JoinKeyMoteText) == false)
            {
                UpdateTaskSettings(EnumTaskName.SET_JOINKEY, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }
        #endregion MOTE
        #endregion Task Clicks/Execution

        #region ToolStripMenuItem Clicks
        /// <summary>
        /// PortSettings button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortSettingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Check to make sure that the Serial Port Settings GUI gets opened only when no task except CONNECTION is running
            if (Common.taskName != EnumTaskName.CONNECTION && Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //display message box
                MessageBox.Show("The " + Common.taskName + " task has been " + Common.taskStatus +
                        ", please wait for it to be completed.", "SmartMesh IP GUI");
                //Exit function
                return;
            }
            //Open the Serial Port Settings GUI
            Common.FrmPortSettings_NewInstance();
            Common.portSettingsForm.Show();
        }

        /// <summary>
        /// LoadFirmware button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFirmwareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check to make sure that the Load Firmware GUI gets opened only when no task is running
            if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //display message box
                MessageBox.Show("The " + Common.taskName + " task has been " + Common.taskStatus +
                        ", please wait for it to be completed.", "SmartMesh IP GUI");
                //Exit function
                return;
            }
            //Open the Load Firmware GUI
            Common.LoadFirmware_NewInstance();
            Common.loadFirmwareForm.Show();
        }

        /// <summary>
        /// Network Statistics button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NetworkStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check to make sure that the Network Statistics GUI gets opened only when no task is running
            if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //display message box
                MessageBox.Show("The " + Common.taskName + " task has been " + Common.taskStatus +
                        ", please wait for it to be completed.", "SmartMesh IP GUI");
                //Exit function
                return;
            }
            //Close some programs
            Common.CloseSomeProgramsAtStartup();
            //Open the Network Statistics GUI
            Common.NetworkStatistics_NewInstance();
            Common.networkStatisticsForm.Show();
        }

        /// <summary>
        /// Temperature Logger button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemperatureLoggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check to make sure that the Temperature Logger GUI gets opened only when no task is running
            if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //display message box
                MessageBox.Show("The " + Common.taskName + " task has been " + Common.taskStatus +
                        ", please wait for it to be completed.", "SmartMesh IP GUI");
                //Exit function
                return;
            }
            //display firmware message
            MessageBox.Show("To successfully execute the Temperature Logger application, the mote needs to run the " +
                "default firmware and needs to be in Master mode." +
                "\r\nThe default firmware can be loaded on the mote using the 'Firmware' and 'Load Firmware' menu buttons " +
                "on the main GUI. You will need to first click on 'Disconnect Devices -> Yes' before loading the default firmware " +
                "on the mote from the main GUI.", "SmartMesh IP GUI");
            //Close some programs
            Common.CloseSomeProgramsAtStartup();
            //Open the Temperature Logger GUI
            Common.TemperatureLogger_NewInstance();
            Common.temperatureLoggerForm.Show();
        }

        /// <summary>
        /// Temperature Plotter button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemperaturePlotterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check to make sure that the Temperature Plotter GUI gets opened only when no task is running
            if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //display message box
                MessageBox.Show("The " + Common.taskName + " task has been " + Common.taskStatus +
                        ", please wait for it to be completed.", "SmartMesh IP GUI");
                //Exit function
                return;
            }
            //display firmware message
            MessageBox.Show("To successfully execute the Temperature Plotter application, please make sure the mote is " +
                @"running the proper firmware (02-gpio_net @ C:\Marc_Kamsu\Fall2020\ECE699\Software\onchipsdk-master_Modified_Temperature).", "SmartMesh IP GUI");
            //Close some programs
            Common.CloseSomeProgramsAtStartup();
            //Open the Temperature Plotter GUI
            Common.TemperaturePlotter_NewInstance();
            Common.temperaturePlotterForm.Show();
        }

        /// <summary>
        /// Oscilloscope button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OscilloscopeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check to make sure that the Oscilloscope GUI gets opened only when no task is running
            if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //display message box
                MessageBox.Show("The " + Common.taskName + " task has been " + Common.taskStatus +
                        ", please wait for it to be completed.", "SmartMesh IP GUI");
                //Exit function
                return;
            }
            //display firmware message
            MessageBox.Show("To successfully execute the Oscilloscope application, please make sure the mote is " +
                @"running the proper firmware (02-gpio_net @ C:\Marc_Kamsu\Fall2020\ECE699\Software\onchipsdk-master_Modified_Oscilloscope).", "SmartMesh IP GUI");
            //Close some programs
            Common.CloseSomeProgramsAtStartup();
            //Open the Oscilloscope GUI
            Common.Oscilloscope_NewInstance();
            Common.oscilloscopeForm.Show();
        }

        /// <summary>
        /// Network Topology button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NetworkTopologyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Display message box requesting user input
            if (MessageBox.Show("In order to view the SmartMesh IP Network Topology, Google Chrome needs to be installed " +
                "on this computer. Please make sure that Google Chrome is installed on this computer. \r\nWould you like to continue?",
                "SmartMesh IP GUI", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                //Exit function
                return;
            }
            //Append text in output_screen
            Output_ScreenText("\r\nOpening the SmartMesh IP Network Topology...\r\n");
            //Close any open SeeTheMesh.exe application
            Common.CloseProgram(Common.seeTheMeshProcessName);
            //Set delay to 100ms to make sure that any open SeeTheMesh.exe app is closed
            Thread.Sleep(100);
            //Append text in output_screen
            Output_ScreenText("\r\nStarting the SeeTheMesh application...\r\n");
            //Select the cmd.exe program to be run in Windows cmd
            Common.commandLine = new CommandLineExecute(@"cmd.exe", 100);
            //Send command to run the SeeTheMesh.exe file
            if (Common.commandLine.Start("/C " + Common.seeTheMeshApplicationProcessName, Common.applicationFilesDirectoryPath) == 0)
            {
                //Append text in output_screen
                Output_ScreenText("\r\nThe SeeTheMesh application has successfully started!\r\n");
                try
                {
                    /*Open the SmartMesh IP Network Topology in Google Chrome (opening in 2 Google Chrome tabs to "speed up" the loading)*/
                    Process.Start("chrome.exe", Common.seeTheMeshUrl); //1st tab
                    Process.Start("chrome.exe", Common.seeTheMeshUrl); //2nd tab
                }
                catch (Exception ex)
                {
                    //Append text in output_screen
                    Output_ScreenText("\r\n" + ex.Message + "\r\n");
                    //Exit function
                    return;
                }
                //Append text in output_screen
                Output_ScreenText("\r\nThe SmartMesh IP Network Topology is being displayed in Google Chrome...This process may take up to about 30 seconds.\r\n");
            }
            else
            {
                //Append text in output_screen
                Output_ScreenText("\r\nThe SeeTheMesh application could not be successfully started!" +
                    " Therefore, the SmartMesh IP Network Topology cannot be viewed!\r\n");
            }
        }

        /// <summary>
        /// DisconnectDevices button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Display message box requesting user input
            if (MessageBox.Show("Are you sure you want to disconnect the Network Manager and the Mote from the GUI?", "SmartMesh IP GUI", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return; //Devices Disconnection has been cancelled, exit function
            }
            //Disable 'disconnectDevicesToolStripMenuItem' button
            DisconnectDevicesToolStripMenuItemEnabled(false);
            //Enable 'firmwareToolStripMenuItem' button
            FirmwareToolStripMenuItemEnabled(true);
            //Close the serial port interface
            if (CloseSerialPortInterface() == true) //check if the serialPort1 object is open
            {
                //Since the serialPort1 object is open...
                //Enable 'disconnectDevicesToolStripMenuItem' button
                DisconnectDevicesToolStripMenuItemEnabled(true);
                //Disable 'firmwareToolStripMenuItem' button
                FirmwareToolStripMenuItemEnabled(false);
                //Exit function
                return;
            }
            //Disable Network Manager Task buttons
            NetworkManager_TaskButtons_Enabled(false);
            //Disable Mote Task buttons
            Mote_TaskButtons_Enabled(false);
            //Disable SendData button
            ButtonSendDataEnabled(false);
            //Enable Connection button
            ButtonConnectionEnabled(true);
            //Select the Connection button
            ButtonConnectionSelect();
            //Display "Not Connected"
            Status_System_StatusText("Not Connected!");
            //Update connection status text color
            Status_System_StatusForeColor(Color.DarkRed);
            //Disable 'applicationsToolStripMenuItem' button
            ApplicationsToolStripMenuItemEnabled(false);
            //Disable 'temperatureLoggerToolStripMenuItem' button
            TemperatureLoggerToolStripMenuItemEnabled(false);
            //Disable 'temperaturePlotterToolStripMenuItem' button
            TemperaturePlotterToolStripMenuItemEnabled(false);
            //Disable 'oscilloscopeToolStripMenuItem' button
            OscilloscopeToolStripMenuItemEnabled(false);
            //Disable 'networkStatisticsToolStripMenuItem' button
            NetworkStatisticsToolStripMenuItemEnabled(false);
            //Enable the Device Type comboBox
            ComboBoxDeviceTypeEnabled(true);
            //Reset since GUI is no longer connected to the Network Manager
            NetworkManager.isConnectedToGUI = false;
            //Reset since GUI is no longer connected to the Mote
            Mote.isConnectedToGUI = false;
            //Check if a task is running and cancel any running task
            if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //Display message
                Output_ScreenText("\r\nNo device is currently connnected to the GUI; therefore, the " + Common.taskName +
                    " task is being auto-cancelled!\r\n");
                //update task settings
                UpdateTaskSettings(Common.taskName, EnumTaskStatus.CANCELED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }
        #endregion ToolStripMenuItem Clicks

        #region Other Clicks
        /// <summary>
        /// Network Manager's COM Port Box click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_Port_Click(object sender, EventArgs e)
        {
            //Populate COM Port Box
            Common.PopulateComPortBox(Name);
        }

        /// <summary>
        /// Mote's COM Port Box click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_PortMote_Click(object sender, EventArgs e)
        {
            //Populate COM Port Box
            Common.PopulateComPortBox(Name);
        }

        /// <summary>
        /// SendData button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSendData_Click(object sender, EventArgs e)
        {
            if (input_screen.TextLength > 0)
            {
                //Send user input to Network Manager/Mote
                WriteLineToSerialPortThread(input_screen.Text, Common.serialPortThreadTimeout);
            }
        }

        /// <summary>
        /// ClearInputBox button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClearInputBox_Click(object sender, EventArgs e)
        {
            Input_ScreenText("\r"); //reset cursor at initial position
            input_screen.Clear(); //clear input textbox
        }

        /// <summary>
        /// ClearOutputBox button click execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClearOutputBox_Click(object sender, EventArgs e)
        {
            Output_ScreenText("\r"); //reset cursor at initial position
            output_screen.Clear(); //clear output textbox
        }
        #endregion Other Clicks

        #region Task Settings
        /// <summary>
        /// Function used to update Task Settings.
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="taskStatus"></param>
        /// <param name="taskResult"></param>
        /// <param name="deviceType"></param>
        /// <param name="taskTimeout"></param>
        public void UpdateTaskSettings(EnumTaskName taskName, EnumTaskStatus taskStatus, EnumTaskResult taskResult, EnumDeviceType deviceType, int taskTimeout)
        {
            //Update task settings variables
            Common.taskName = taskName;
            Common.taskStatus = taskStatus;
            Common.taskResult = taskResult;
            Common.deviceType = deviceType;
            Common.taskTimeout = taskTimeout;
            Common.serialPortReadLoopCounter = Convert.ToInt16(taskTimeout * 0.004);

            #region Common.taskStatus
            //Check the status of the current task
            if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //Disable buttons used for serial port data inputting
                SerialPort_DataInputButtons_Enabled(false);
                //Disable 'firmwareToolStripMenuItem' button
                FirmwareToolStripMenuItemEnabled(false);
                //Disable the Device Type comboBox
                ComboBoxDeviceTypeEnabled(false);
                //Reset the flags used for multi-step tasks
                Common.isFirstPartSuccessful = false;
                Common.isSecondPartSuccessful = false;
                Common.isThirdPartSuccessful = false;
                //Reset the task serial output log
                Common.taskSerialOutputLog = string.Empty;
                //Start timer to wait for task completion
                StartTaskExecutionTimer();
                //Start timer to display elapsed time during task execution
                StartStopWatchTimer();
                //Display task execution start message
                Output_ScreenText("\r\nThe " + Common.taskName + " task has been " + Common.taskStatus + "!\r\n");
            }
            else if (Common.taskStatus == EnumTaskStatus.COMPLETED || Common.taskStatus == EnumTaskStatus.CANCELED ||
                Common.taskStatus == EnumTaskStatus.NA)
            {
                //Stop the 'taskTimer' timer
                StopTaskExecutionTimer();
                //Stop the 'stopWatchTimer' timer
                StopStopWatchTimer();
                #region TaskName Switch Statement
                //Check the current task's name
                switch (Common.taskName)
                {
                    #region CONNECTION Task
                    case EnumTaskName.CONNECTION:
                        if (Common.taskResult == EnumTaskResult.UNSUCCESSFUL)
                        {
                            //Disable buttons used for serial port data inputting
                            SerialPort_DataInputButtons_Enabled(false);
                            //Disable 'disconnectDevicesToolStripMenuItem' button
                            DisconnectDevicesToolStripMenuItemEnabled(false);
                            //Enable 'firmwareToolStripMenuItem' button
                            FirmwareToolStripMenuItemEnabled(true);
                            //Check if device type is NETWORK_MANAGER
                            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                            {
                                //Disable 'applicationsToolStripMenuItem' button
                                ApplicationsToolStripMenuItemEnabled(false);
                                //Disable 'temperatureLoggerToolStripMenuItem' button
                                TemperatureLoggerToolStripMenuItemEnabled(false);
                                //Disable 'temperaturePlotterToolStripMenuItem' button
                                TemperaturePlotterToolStripMenuItemEnabled(false);
                                //Disable 'oscilloscopeToolStripMenuItem' button
                                OscilloscopeToolStripMenuItemEnabled(false);
                                //Disable 'networkStatisticsToolStripMenuItem' button
                                NetworkStatisticsToolStripMenuItemEnabled(false);
                            }
                            //Enable Connection button
                            ButtonConnectionEnabled(true);
                            //Select the Connection button  
                            ButtonConnectionSelect();
                            //Update on which device did not get connected to the GUI
                            Common.UpdateOnDeviceConnection(false);
                        }
                        else if (Common.taskResult == EnumTaskResult.SUCCESSFUL)
                        {
                            //Enable 'disconnectDevicesToolStripMenuItem' button
                            DisconnectDevicesToolStripMenuItemEnabled(true);
                            //Disable 'firmwareToolStripMenuItem' button
                            FirmwareToolStripMenuItemEnabled(false);
                            //Check if device type is NETWORK_MANAGER
                            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
                            {
                                //Enable 'applicationsToolStripMenuItem' button
                                ApplicationsToolStripMenuItemEnabled(true);
                                //Enable 'temperatureLoggerToolStripMenuItem' button
                                TemperatureLoggerToolStripMenuItemEnabled(true);
                                //Enable 'temperaturePlotterToolStripMenuItem' button
                                TemperaturePlotterToolStripMenuItemEnabled(true);
                                //Enable 'oscilloscopeToolStripMenuItem' button
                                OscilloscopeToolStripMenuItemEnabled(true);
                                //Enable 'networkStatisticsToolStripMenuItem' button
                                NetworkStatisticsToolStripMenuItemEnabled(true);
                            }
                            //Select the SendData button
                            ButtonSendDataSelect();
                            //Store the current Network Manager's or Mote's Serial Port name
                            Common.StoreLastUsedSerialPortName(serialPort1.PortName);
                            //Update on which device got connected to the GUI
                            Common.UpdateOnDeviceConnection(true);
                        }
                        break;
                        #endregion CONNECTION Task
                }
                #endregion TaskName Switch Statement
                //Check if any device is connected to the GUI
                if (NetworkManager.isConnectedToGUI == true || Mote.isConnectedToGUI == true)
                {
                    //Enable buttons used for serial port data inputting
                    SerialPort_DataInputButtons_Enabled(true);
                }
                //Enable the Device Type comboBox
                ComboBoxDeviceTypeEnabled(true);
                //Check if an actual task has been assigned
                if (Common.taskName != EnumTaskName.NA)
                {
                    //Display task execution outcome message
                    Output_ScreenText("\r\nThe " + Common.taskName + " task has been " + Common.taskStatus +
                        " and " + Common.taskResult + "!\r\n");
                }
            }
            #endregion Common.taskStatus

            //Update task outcome objects
            UpdateTaskOutcomeObjects();
        }
        #endregion Task Settings

        #region Task Outcome
        /// <summary>
        /// Function used to update task outcome objects.
        /// </summary>
        private void UpdateTaskOutcomeObjects()
        {
            #region Textboxes
            //Update textboxes text
            TaskOutcomeDeviceTypeText(Common.deviceType.ToString());
            TaskOutcomeNameText(Common.taskName.ToString());
            TaskOutcomeStatusText(Common.taskStatus.ToString());
            TaskOutcomeResultText(Common.taskResult.ToString());
            if (Common.taskStatus == EnumTaskStatus.NA)
            {
                TaskOutcomeMessageText("Ready!");
            }
            else if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                TaskOutcomeResultText(string.Empty); //clear TaskOutcomeResult textbox
                TaskOutcomeMessageText("The " + Common.taskName + " task has been " + Common.taskStatus + "!");
            }
            else
            {
                TaskOutcomeMessageText("The " + Common.taskName + " task has been " + Common.taskStatus +
                        " and " + Common.taskResult + "!");
            }
            #endregion Textboxes

            #region PictureBox
            //Update PictureBox image
            if (Common.taskStatus == EnumTaskStatus.NA)
            {
                PictureBoxOutcomeImage(null);
            }
            else if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                PictureBoxOutcomeImage(ImageResource.loading);
            }
            else if (Common.taskStatus == EnumTaskStatus.CANCELED)
            {
                PictureBoxOutcomeImage(ImageResource.alertR);
            }
            else if (Common.taskStatus == EnumTaskStatus.COMPLETED)
            {
                if (Common.taskResult == EnumTaskResult.SUCCESSFUL)
                {
                    PictureBoxOutcomeImage(ImageResource.check);
                }
                else if (Common.taskResult == EnumTaskResult.UNSUCCESSFUL)
                {
                    PictureBoxOutcomeImage(ImageResource.cross1_1);
                }
            }
            #endregion PictureBox

            #region StopWatch Timer
            //Update StopWatch Timer label
            if (Common.taskStatus == EnumTaskStatus.NA)
            {
                //Reset the 'stopWatchTimer' parameters.
                ResetStopWatchTimer();
            }
            #endregion StopWatch Timer
        }
        #endregion Task Outcome

        #region Other Task Functions
        /// <summary>
        /// Function used to check if the selected device (Network Manager or Mote) is connected to the GUI
        /// via the serial port interface.
        /// </summary>
        /// <returns></returns>
        private bool IsConnectedToDevice()
        {
            //Declare and initialize a boolean variable indicating whether the selected device is connected to the GUI or not
            bool isConnectedToDevice = false;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                //true if the NETWORK_MANAGER and GUI are connected via serial port, else false
                isConnectedToDevice = NetworkManager.isConnectedToGUI;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                //true if the MOTE and GUI are connected via serial port, else false
                isConnectedToDevice = Mote.isConnectedToGUI;
            }
            //Check if the selected device is connected to the GUI or not
            if (isConnectedToDevice == false)
            {
                //append text in output_screen
                Output_ScreenText("\r\nPlease first run the " + EnumTaskName.CONNECTION + " task before running any other task so that " +
                    "the " + Common.deviceType + " gets connected to the GUI via the serial port interface!\r\n");
            }
            //return a boolean variable indicating whether the selected device is connected to the GUI or not
            return isConnectedToDevice;
        }

        /// <summary>
        /// Function used to log in to the Network Manager or Mote.
        /// </summary>
        /// <returns></returns>
        private bool LoginToDevice()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to log in to the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.connectionTaskCommandString;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                commandString = Mote.connectionTaskCommandString;
            }
            //Attempt to log in to the Network Manager
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nLogging in to the " + Common.deviceType + "...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + " was not successfully logged in!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to restart the Network Manager or Mote.
        /// </summary>
        /// <returns></returns>
        private bool RestartDevice()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to restart the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.restartTaskCommandString;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                commandString = Mote.restartTaskCommandString;
            }
            //Attempt to restart the Network Manager or mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + " may be restarting...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be restarted!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to factory reset the Network Manager or Mote.
        /// </summary>
        /// <returns></returns>
        private bool FactoryResetDevice()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to factory reset the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.factoryResetTaskCommandString;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                commandString = Mote.factoryResetTaskCommandString;
            }
            //Attempt to factory reset the Network Manager or mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + " may be factory resetting...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + " could not be factory reset!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to assign the mode of the Mote.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private bool AssignMoteMode(string mode)
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to assign the mode of the " + Common.deviceType + "...\r\n");
            //get command string
            string commandString = Mote.modeTaskCommandString1 + " " + mode;
            //Attempt to assign the mode of the mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s mode is being assigned...\r\n");
                //check if the displayed text of 'comboBoxModeMote' matches the assigned mode
                if (GetComboBoxModeMoteText() != mode)
                {
                    //display the assigned mode of mote in 'comboBoxModeMote'
                    SetComboBoxModeMoteText(mode);
                }
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s mode could not be assigned!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to access the mode of the Mote.
        /// </summary>
        /// <returns></returns>
        private bool AccessMoteMode()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to access the mode of the " + Common.deviceType + "...\r\n");
            //get command string
            string commandString = Mote.modeTaskCommandString2;
            //Attempt to access the mode of the mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s mode is being accessed...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s mode could not be accessed!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to assign the autoJoin value of the Mote.
        /// </summary>
        /// <param name="autoJoinValue"></param>
        /// <returns></returns>
        private bool AssignMoteAutoJoin(string autoJoinValue)
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to assign the autoJoin value of the " + Common.deviceType + "...\r\n");
            //get command string
            string commandString = Mote.autoJoinTaskCommandString1 + " " + autoJoinValue;
            //Attempt to assign the autoJoin of the mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s autoJoin value is being assigned...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s autoJoin value could not be assigned!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to access the autoJoin value (1(ON)/0(OFF)) of the Mote.
        /// </summary>
        /// <returns></returns>
        private bool AccessMoteAutoJoin()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to access the autoJoin value of the " + Common.deviceType + "...\r\n");
            //get command string
            string commandString = Mote.autoJoinTaskCommandString2;
            //Attempt to access the autoJoin value of the mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s autoJoin value is being accessed...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s autoJoin value could not be accessed!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to assign the radiotest value (ON/OFF) of the Network Manager or Mote.
        /// </summary>
        /// <param name="radiotestValue"></param>
        /// <returns></returns>
        private bool AssignRadiotest(string radiotestValue)
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to assign the radiotest value of the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.radiotestTaskCommandString1 + " " + radiotestValue;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                commandString = Mote.radiotestTaskCommandString + " " + radiotestValue;
            }
            //Attempt to assign the radiotest value of the Network Manager or Mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s radiotest value is being assigned...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s radiotest value could not be assigned!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to access the radiotest value (ON/OFF) of the Network Manager or Mote.
        /// </summary>
        /// <returns></returns>
        private bool AccessRadiotest()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to access the radiotest value of the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.radiotestTaskCommandString2;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                commandString = Mote.connectionTaskCommandString;
            }
            //Attempt to access the radiotest value of the Network Manager or Mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s radiotest value is being accessed...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s radiotest value could not be accessed!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to assign the Network ID value of the Network Manager or Mote.
        /// </summary>
        /// <param name="networkIdValue"></param>
        /// <returns></returns>
        private bool AssignNetworkId(string networkIdValue)
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to assign the Network ID value of the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.setNetworkIdTaskCommandString + networkIdValue;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                commandString = Mote.setNetworkIdTaskCommandString + " " + networkIdValue;
            }
            //Attempt to assign the Network ID value of the Network Manager or Mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s Network ID value is being assigned...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s Network ID value could not be assigned!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to access the Network ID value of the Network Manager or Mote.
        /// </summary>
        /// <returns></returns>
        private bool AccessNetworkId()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to access the Network ID value of the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.getNetworkIdTaskCommandString;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                commandString = Mote.getNetworkIdTaskCommandString;
            }
            //Attempt to access the Network ID value of the Network Manager or Mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s Network ID value is being accessed...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s Network ID value could not be accessed!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to assign the Join Key value of the Network Manager or Mote.
        /// </summary>
        /// <param name="joinKeyValue"></param>
        /// <returns></returns>
        private bool AssignJoinKey(string joinKeyValue)
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to assign the Join Key value of the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.setJoinKeyTaskCommandString + joinKeyValue;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                commandString = Mote.setJoinKeyTaskCommandString + " " + joinKeyValue;
            }
            //Attempt to assign the Join Key value of the Network Manager or Mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s Join Key value is being assigned...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s Join Key value could not be assigned!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to access the Mac Address value of the Network Manager or Mote.
        /// </summary>
        /// <returns></returns>
        private bool AccessMacAddress()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to access the Mac Address value of the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.macAddressCommandString;
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                commandString = Mote.macAddressCommandString;
            }
            //Attempt to access the Mac Address value of the Network Manager or Mote
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s Mac Address value is being accessed...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s Mac Address value could not be accessed!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to access the number of motes found in the SmartMesh IP network.
        /// </summary>
        /// <returns></returns>
        private bool AccessMotesNumber()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to access the number of motes connected to the " + Common.deviceType + "...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.getMotesNumberTaskCommandString;
            }
            //Attempt to access the number of motes found in the SmartMesh IP network
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe number of motes connected to the " + Common.deviceType + " is being accessed...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe number of motes connected to the " + Common.deviceType + " could not be accessed!\r\n");
            return false;
        }

        /// <summary>
        /// Function used to access the network statistics data.
        /// </summary>
        /// <returns></returns>
        private bool AccessNetworkStatistics()
        {
            //append text in output_screen
            Output_ScreenText("\r\nAttempting to access the SmartMesh IP network statistics data...\r\n");
            //get command string based on the device type
            string commandString = string.Empty;
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                commandString = NetworkManager.getNetworkStatisticsTaskCommandString;
            }
            //Attempt to access the SmartMesh IP network statistics
            if (WriteLineToSerialPortThread(commandString, Common.serialPortThreadTimeout))
            {
                //append text in output_screen
                Output_ScreenText("\r\nThe SmartMesh IP network statistics data is being accessed...\r\n");
                return true;
            }
            //append text in output_screen
            Output_ScreenText("\r\nThe SmartMesh IP network statistics data could not be accessed!\r\n");
            return false;
        }
        #endregion Other Task Functions

        #region Timer
        #region TaskTimer
        /// <summary>
        /// Function used to start the task execution timer.
        /// </summary>
        private void StartTaskExecutionTimer()
        {
            //Create a timer with the specified interval
            Common.taskTimer = new System.Timers.Timer(Common.taskTimeout);
            Common.taskTimer.Elapsed += OnTimedEvent; //Hook up the Elapsed event for the timer
            Common.taskTimer.AutoReset = false; //the Elapsed event is to be raised only once
            Common.taskTimer.Start(); //start the timer
            //Display message
            Output_ScreenText("\r\nThe " + Common.taskName + " task will run for up to " + Common.taskTimeout / 1000 +
                " seconds before auto-cancellation. The task timer has started!\r\n");
        }

        /// <summary>
        /// Function used to stop the task execution timer.
        /// </summary>
        private void StopTaskExecutionTimer()
        {
            //Stop the timer
            if (Common.taskTimer != null)
            {
                Common.taskTimer.Stop();
                Common.taskTimer.Dispose();
                GC.SuppressFinalize(Common.taskTimer);
                Common.taskTimer.Close();
                //Display message
                Output_ScreenText("\r\nThe task timer has stopped!\r\n");
            }
        }

        /// <summary>
        /// Function used to handle the 'taskTimer' timer elapsed event.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //Check if a task is running and cancel any running task
            if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //Display message
                Output_ScreenText("\r\nThe " + Common.taskName + " task ran for more than the allocated " + Common.taskTimeout / 1000 +
                    " seconds threshold. Therefore, this task is being auto-cancelled!\r\n");
                //update task settings
                UpdateTaskSettings(Common.taskName, EnumTaskStatus.CANCELED, EnumTaskResult.UNSUCCESSFUL, Common.deviceType, Common.taskTimeout);
            }
        }
        #endregion TaskTimer

        #region StopWatchTimer
        /// <summary>
        /// Function used to start the 'stopWatchTimer' timer.
        /// </summary>
        private void StartStopWatchTimer()
        {
            //Reset the 'stopWatchTimer' parameters.
            ResetStopWatchTimer();
            //Create a new instance of stopWatchTimer
            stopWatchTimer = new System.Windows.Forms.Timer
            {
                //Set the time, in milliseconds, before the Tick event is raised
                Interval = 1000
            };
            //StopWatchTimerTick will handle the event raised when the specified timer interval has elapsed and the timer is enabled.
            stopWatchTimer.Tick += new System.EventHandler(StopWatchTimerTick);
            //start (enable) the 'stopWatchTimer' timer
            stopWatchTimer.Start();
        }

        /// <summary>
        /// Function used to stop the 'stopWatchTimer' timer.
        /// </summary>
        private void StopStopWatchTimer()
        {
            //Stop the 'stopWatchTimer' timer
            if (stopWatchTimer != null)
            {
                stopWatchTimer.Stop();
                stopWatchTimer.Dispose();
                GC.SuppressFinalize(stopWatchTimer);
            }
        }

        /// <summary>
        /// Function used to handle the 'stopWatchTimer' elapsed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopWatchTimerTick(object sender, EventArgs e)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new StopWatchTimerLabelTextDelegate(StopWatchTimerTick), new object[] { sender, e });
                return; // Important
            }

            //Adjust stopWatchTimer related flags
            Common.stopWatchTimerElapsedSecond++;
            if (Common.stopWatchTimerElapsedSecond > 59)
            {
                Common.stopWatchTimerElapsedMinute++;
                Common.stopWatchTimerElapsedSecond = 0;
            }
            if (Common.stopWatchTimerElapsedMinute > 59)
            {
                Common.stopWatchTimerElapsedHour++;
                Common.stopWatchTimerElapsedMinute = 0;
            }

            //Assign 'labelStopWatchTimer' Text
            LabelStopWatchTimerText(Common.stopWatchTimerElapsedHour + ":" + Common.stopWatchTimerElapsedMinute +
                ":" + Common.stopWatchTimerElapsedSecond);
        }

        /// <summary>
        /// Function used to reset the 'stopWatchTimer' parameters.
        /// </summary>
        private void ResetStopWatchTimer()
        {
            //Reset stopWatchTimer related flags
            Common.stopWatchTimerElapsedHour = 0;
            Common.stopWatchTimerElapsedMinute = 0;
            Common.stopWatchTimerElapsedSecond = 0;
            //Reset 'labelStopWatchTimer' text
            LabelStopWatchTimerText(Common.stopWatchTimerElapsedHour + ":" + Common.stopWatchTimerElapsedMinute +
                ":" + Common.stopWatchTimerElapsedSecond);
        }
        #endregion StopWatchTimer
        #endregion Timer

        #region Delegates
        private delegate void Status_System_StatusTextDelegate(string text);
        private delegate void Output_ScreenTextDelegate(string text);
        private delegate void Status_System_StatusForeColorDelegate(Color foreColor);
        private delegate void ButtonConnectionEnabledDelegate(bool enabled);
        private delegate void ButtonConnectionSelectDelegate();
        private delegate void Input_ScreenSelectDelegate();
        private delegate void Output_ScreenSelectDelegate();
        private delegate void ButtonSendDataSelectDelegate();
        private delegate void TextBoxNetworkIdNetworkManagerSelectDelegate();
        private delegate void TextBoxJoinKeyNetworkManagerSelectDelegate();
        private delegate void TextBoxNetworkIdMoteSelectDelegate();
        private delegate void TextBoxJoinKeyMoteSelectDelegate();
        private delegate void Input_ScreenTextDelegate(string text);
        private delegate void Combo_PortTextDelegate(string text);
        private delegate void SerialPort_DataInputButtons_EnabledDelegate(bool enabled);
        private delegate void TaskOutcomeNameTextDelegate(string text);
        private delegate void TaskOutcomeStatusTextDelegate(string text);
        private delegate void TaskOutcomeResultTextDelegate(string text);
        private delegate void TaskOutcomeMessageTextDelegate(string text);
        private delegate void PictureBoxOutcomeImageDelegate(Image image);
        private delegate void TaskOutcomeDeviceTypeTextDelegate(string text);
        private delegate void NetworkManager_TaskButtons_EnabledDelegate(bool enabled);
        private delegate void Mote_TaskButtons_EnabledDelegate(bool enabled);
        private delegate void ComboBoxDeviceTypeEnabledDelegate(bool enabled);
        private delegate void ButtonSendDataEnabledDelegate(bool enabled);
        private delegate void SetComboBoxModeMoteTextDelegate(string text);
        private delegate string GetComboBoxModeMoteTextDelegate();
        private delegate void StopWatchTimerLabelTextDelegate(object sender, EventArgs e);
        private delegate void LabelStopWatchTimerTextDelegate(string text);
        private delegate void DisconnectDevicesToolStripMenuItemEnabledDelegate(bool enabled);
        private delegate void FirmwareToolStripMenuItemEnabledDelegate(bool enabled);
        private delegate void ApplicationsToolStripMenuItemEnabledDelegate(bool enabled);
        private delegate void TemperatureLoggerToolStripMenuItemEnabledDelegate(bool enabled);
        private delegate void TemperaturePlotterToolStripMenuItemEnabledDelegate(bool enabled);
        private delegate void OscilloscopeToolStripMenuItemEnabledDelegate(bool enabled);
        private delegate void NetworkStatisticsToolStripMenuItemEnabledDelegate(bool enabled);
        private delegate void TextBoxNetworkIdNetworkManagerTextDelegate(string text);
        private delegate void TextBoxNetworkIdMoteTextDelegate(string text);
        private delegate void TextBoxJoinKeyNetworkManagerTextDelegate(string text);
        private delegate void TextBoxJoinKeyMoteTextDelegate(string text);
        private delegate string GetComboBoxPortNetworkManagerTextDelegate();
        #endregion Delegates

        private void labelOutputScreen_Click(object sender, EventArgs e)
        {

        }

        private void labelOutputScreenCharactersCount_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxTasksMote_Enter(object sender, EventArgs e)
        {

        }

        private void labelNetworkIdNetworkManager_Click(object sender, EventArgs e)
        {

        }

        private void labelNetworkIdMote_Click(object sender, EventArgs e)
        {

        }

        private void labelNetworkIdCharactersCountMote_Click(object sender, EventArgs e)
        {

        }

        private void labelTaskOutcomeName_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedMoteId = comboBox1.SelectedItem.ToString();
                string pingCommand = $"ping {selectedMoteId}";

                if (serialPort1 != null && serialPort1.IsOpen)
                {
                    serialPort1.WriteLine(pingCommand);
                    output_screen.AppendText($"Sent: {pingCommand}\r\n");
                }
                else
                {
                    MessageBox.Show("Serial port is not open.");
                }
            }
            else
            {
                MessageBox.Show("Please select a Mote ID first.");
            }
        }


        private void combo_PortMote_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}