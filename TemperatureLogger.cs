using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Network_Manager_GUI
{
    public partial class TemperatureLogger : Form
    {
        #region Variables/Instances Declaration and Initialization
        #region Timer
        public static int stopWatchTimerElapsedHour = -1;
        public static int stopWatchTimerElapsedMinute = -1;
        public static int stopWatchTimerElapsedSecond = -1;
        #endregion Timer

        #region Thread
        //initialize thread to null
        Thread getMoteTemperatureThread = null;
        #endregion Thread
        #endregion Variables/Instances Declaration and Initialization

        public TemperatureLogger()
        {
            InitializeComponent();
        }

        #region GUI Form
        /// <summary>
        /// Function used when the Temperature Logger GUI is starting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemperatureLogger_Load(object sender, EventArgs e)
        {
            //Start timer to display the elapsed time since the Temperature Logger GUI has been opened
            StartStopWatchTimer();
            //Disable the main (Network Manager GUI) form
            Common.Network_Manager_GUI_Enabled(false);
            //Set controls' ToolTip
            SetControlsToolTip();
            //Get the mote(s)' temperature data
            MoteTemperatureDataAcquisition(sender, e);
        }

        /// <summary>
        /// Function used when the Temperature Logger GUI is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemperatureLogger_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Stop the 'stopWatchTimer' timer
            StopStopWatchTimer();
            //Enable the main (Network Manager GUI) form
            Common.Network_Manager_GUI_Enabled(true);
        }
        #endregion GUI Form

        #region ToolTip
        /// <summary>
        /// Function used to set controls' ToolTip.
        /// </summary>
        private void SetControlsToolTip()
        {
            // Create the ToolTip and associate with the Form controls.
            ToolTip controlsToolTip = new ToolTip();
            controlsToolTip.SetToolTip(output_screen, "This screen shows a log of events.");
            controlsToolTip.SetToolTip(labelOutputScreenCharactersCount, "This label shows the current number of characters in the output screen.");
            controlsToolTip.SetToolTip(textBoxNumberofFoundMotes, "This screen shows the number of found motes in the SmartMesh IP network.");
            controlsToolTip.SetToolTip(textBoxNumberofOperationalMotes, "This screen shows the number of operational motes in the SmartMesh IP network.");
            controlsToolTip.SetToolTip(textBoxNumberofLostMotes, "This screen shows the number of lost motes in the SmartMesh IP network.");
            controlsToolTip.SetToolTip(textBoxNumberofConnectingMotes, "This screen shows the number of connecting motes in the SmartMesh IP network.");
            controlsToolTip.SetToolTip(labelStopWatchTimer, "This label shows the elapsed time since the Temperature Logger GUI has been opened.");
            #region textBoxMacMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxMacMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxMacMote.Name.Contains("textBoxMacMote"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(textBoxMacMote.Name.Replace("textBoxMacMote", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxMacMote, "This screen shows the MAC Address of mote #" + Common.moteNumber + "." +
                            "\r\nBackground color explanation: Green = operational, Yellow = connecting, and Red = lost.");
                    }
                }
            }
            #endregion textBoxMacMote
            #region labelMacCharactersCountMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (Label labelMacCharactersCountMote in groupBoxMote.Controls.OfType<Label>())
                {
                    if (labelMacCharactersCountMote.Name.Contains("labelMacCharactersCountMote"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(labelMacCharactersCountMote.Name.Replace("labelMacCharactersCountMote", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(labelMacCharactersCountMote, "This label shows the current number of characters in the above MAC Address textbox of mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion labelMacCharactersCountMote
            #region textBoxCelsiusTemperatureMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxCelsiusTemperatureMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxCelsiusTemperatureMote.Name.Contains("textBoxCelsiusTemperatureMote"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(textBoxCelsiusTemperatureMote.Name.Replace("textBoxCelsiusTemperatureMote", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxCelsiusTemperatureMote, "This screen shows the Celsius temperature from mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion textBoxCelsiusTemperatureMote
            #region textBoxFahrenheitTemperatureMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxFahrenheitTemperatureMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxFahrenheitTemperatureMote.Name.Contains("textBoxFahrenheitTemperatureMote"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(textBoxFahrenheitTemperatureMote.Name.Replace("textBoxFahrenheitTemperatureMote", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxFahrenheitTemperatureMote, "This screen shows the Fahrenheit temperature from mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion textBoxFahrenheitTemperatureMote
        }
        #endregion ToolTip

        #region Text Control
        /// <summary>
        /// Function used to change 'output_screen' Text.
        /// </summary>
        /// <param name="text"></param>
        private void Output_ScreenText(string text)
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
                output_screen.AppendText(text); //append new text to the current text of the output textbox 
                output_screen.ScrollToCaret(); //scroll down automatically
                ActiveControl = output_screen; //set 'output_screen' as the active control
            }
            catch { }
        }

        /// <summary>
        /// Function used to change the text and/or background color of a given textbox.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="text"></param>
        /// <param name="backColor"></param>
        private void TextBoxText(TextBox textBox, string text, Color backColor)
        {
            try
            {
                // If this returns true, it means it was called from an external thread.
                if (InvokeRequired)
                {
                    // Create a delegate of this method and let the form run it.
                    Invoke(new TextBoxTexttDelegate(TextBoxText), new object[] { textBox, text, backColor });
                    return; // Important
                }

                // Assign Text
                textBox.Text = text;
                // Assign BackColor
                textBox.BackColor = backColor;
            }
            catch { }
        }

        /// <summary>
        /// Function used to change the text of a given label.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="text"></param>
        private void LabelText(Label label, string text)
        {
            try
            {
                // If this returns true, it means it was called from an external thread.
                if (InvokeRequired)
                {
                    // Create a delegate of this method and let the form run it.
                    Invoke(new LabelTexttDelegate(LabelText), new object[] { label, text });
                    return; // Important
                }

                // Assign Text
                label.Text = text;
            }
            catch { }
        }

        /// <summary>
        /// Function used to change the text of a given Celsius temperature textbox.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <param name="text"></param>
        private void TextBoxCelsiusTemperatureText(int moteNumber, string text)
        {
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxCelsiusTemperatureMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxCelsiusTemperatureMote.Name == "textBoxCelsiusTemperatureMote" + moteNumber.ToString())
                    {
                        TextBoxText(textBoxCelsiusTemperatureMote, text, Color.WhiteSmoke);
                    }
                }
            }
        }

        /// <summary>
        /// Function used to change the text of a given Fahrenheit temperature textbox.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <param name="text"></param>
        private void TextBoxFahrenheitTemperatureText(int moteNumber, string text)
        {
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxFahrenheitTemperatureMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxFahrenheitTemperatureMote.Name == "textBoxFahrenheitTemperatureMote" + moteNumber.ToString())
                    {
                        TextBoxText(textBoxFahrenheitTemperatureMote, text, Color.WhiteSmoke);
                    }
                }
            }
        }
        #endregion Text Control

        #region TextChanged Control
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
        /// Function called when the 'textBoxMacMote' Text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxMacMote_TextChanged(object sender, EventArgs e)
        {
            //get the actual textBoxMacMote
            TextBox textBoxMacMote = (TextBox)sender;
            //get the mote number
            int moteNumber = Convert.ToInt16(textBoxMacMote.Name.Substring(textBoxMacMote.Name.Length - 1));
            //display the number of characters in the actual textBoxMacMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (Label labelMacCharactersCountMote in groupBoxMote.Controls.OfType<Label>())
                {
                    if (labelMacCharactersCountMote.Name == "labelMacCharactersCountMote" + moteNumber.ToString())
                    {
                        LabelText(labelMacCharactersCountMote, textBoxMacMote.Text.Length + " character(s)");
                    }
                }
            }
        }
        #endregion TextChanged Control

        #region Application CommandLine Execution
        /// <summary>
        /// Function used to get the mote(s)' temperature data.
        /// </summary>
        private void MoteTemperatureDataAcquisition(object sender, EventArgs e)
        {
            //Create a thread to avoid GUI hang
            getMoteTemperatureThread = new Thread(() =>
            {
                try
                {
                    //get the mote(s)' temperature through multithreading
                    GetMoteTemperatureThread(sender, e);
                }
                catch (Exception ex)
                {
                    string exception = ex.Message;
                }
            })
            {
                //set thread as a background thread
                IsBackground = true
            };
            //start thread to get the mote(s)' temperature
            getMoteTemperatureThread.Start();
        }

        /// <summary>
        /// Function used to get the mote(s)' temperature data through multithreading.
        /// </summary>
        private void GetMoteTemperatureThread(object sender, EventArgs e)
        {
            #region Find Motes
            //Find motes about every 45 seconds
            if (stopWatchTimerElapsedSecond < 15)
            {
                Here1:
                //Append text in output_screen
                Output_ScreenText("\r\nLooking for all motes in the SmartMesh IP network...\r\n");
                //Find all the motes in the SmartMesh IP network
                Common.GetNumberOfFoundMotes();
                Here2:
                //Delay to gather all motes' info including the Mac Address
                if (Common.numberOfFoundMotes == 0)
                {
                    Delay_ms(5000, 10); //set delay to 5000ms
                }
                else //if (Common.numberOfFoundMotes != 0)
                {
                    Delay_ms(Common.numberOfFoundMotes * 500, 10); //set delay based on the number of found motes
                }
                //Check if the GET_MOTESNUMBER task is successful
                if (Common.taskName == EnumTaskName.GET_MOTESNUMBER)
                {
                    if (Common.taskStatus == EnumTaskStatus.STARTED)
                    {
                        //keep looping as long as the Temperature Logger GUI is open
                        if (Visible)
                        {
                            //keep waiting until task is done
                            goto Here2;
                        }
                        else
                        {
                            //Reset task settings
                            Common.UpdateTaskSettings(EnumTaskName.NA, EnumTaskStatus.NA, EnumTaskResult.NA, Common.deviceType, 0);
                            //Exit function
                            return;
                        }
                    }
                    else if (Common.taskStatus == EnumTaskStatus.COMPLETED)
                    {
                        if (Common.taskResult == EnumTaskResult.UNSUCCESSFUL)
                        {
                            //keep looping as long as the Temperature Logger GUI is open
                            if (Visible)
                            {
                                //look for motes again
                                goto Here1;
                            }
                            else
                            {
                                //Reset task settings
                                Common.UpdateTaskSettings(EnumTaskName.NA, EnumTaskStatus.NA, EnumTaskResult.NA, Common.deviceType, 0);
                                //Exit function
                                return;
                            }
                        }
                    }
                    else
                    {
                        //exit function
                        return;
                    }
                }
                //Append text in output_screen
                Output_ScreenText("\r\nFound " + Common.numberOfFoundMotes + " motes: " +
                    Common.numberOfOperationalMotes + " operational, " +
                    Common.numberOfConnectingMotes + " connecting, and " +
                        Common.numberOfLostMotes + " lost.\r\n");
                //Get and display the number of found motes
                TextBoxText(textBoxNumberofFoundMotes, Common.numberOfFoundMotes.ToString(), Color.WhiteSmoke);
                //Get and display the number of operational motes
                TextBoxText(textBoxNumberofOperationalMotes, Common.numberOfOperationalMotes.ToString(), Color.WhiteSmoke);
                //Get and display the number of connecting motes
                TextBoxText(textBoxNumberofConnectingMotes, Common.numberOfConnectingMotes.ToString(), Color.WhiteSmoke);
                //Get and display the number of lost motes
                TextBoxText(textBoxNumberofLostMotes, Common.numberOfLostMotes.ToString(), Color.WhiteSmoke);
            }
            #endregion Find Motes

            #region Display Mote(s)' Mac Address
            //Display Mote(s)' Mac Address
            foreach (Common.MoteStruct mote in Common.mote)
            {
                //Make sure the current mote's state is not null
                if (!string.IsNullOrEmpty(mote.state))
                {
                    //Update Common.moteNumber
                    Common.moteNumber = mote.moteID - 1; //subtract 1 since the network manager's moteID = 1
                                                         //Check if the current mote's state is "operational"
                    if (mote.state.ToUpper() == Common.moteStates[2].ToUpper())
                    {
                        //Display the mote(s)' Mac Address
                        DisplayMoteMacAddress(Common.moteNumber, Color.LightGreen);
                    }
                    //Check if the current mote's state is "lost"
                    else if (mote.state.ToUpper() == Common.moteStates[3].ToUpper())
                    {
                        //Display the mote(s)' Mac Address
                        DisplayMoteMacAddress(Common.moteNumber, Color.Red);
                    }
                    //Check if the current mote's state is "Negot" or "Conn"
                    else if (mote.state.ToUpper().Contains(Common.moteStates[0].ToUpper()) ||
                             mote.state.ToUpper().Contains(Common.moteStates[1].ToUpper()))
                    {
                        //Display the mote(s)' Mac Address
                        DisplayMoteMacAddress(Common.moteNumber, Color.Yellow);
                    }
                }
            }
            #endregion Display Mote(s)' Mac Address

            #region Get Mote(s)' Temperature Data
            //Check if the number of operational motes is greater than 0
            if (Common.numberOfOperationalMotes > 0)
            {
                //Execute the TempLogger.exe application
                ExecuteTempLoggerApp(Common.desiredTemperatureLoggerCommandLineResponse, Common.undesiredApplicationCommandLineResponse, 2000);
            }
            //Get Mote(s)' Temperature Data
            foreach (Common.MoteStruct mote in Common.mote)
            {
                //Make sure the current mote's state is not null
                if (!string.IsNullOrEmpty(mote.state))
                {
                    //Update Common.moteNumber
                    Common.moteNumber = mote.moteID - 1; //subtract 1 since the network manager's moteID = 1
                    //Check if the current mote's state is "operational"
                    if (mote.state.ToUpper() == Common.moteStates[2].ToUpper())
                    {
                        //Get a mote's temperature data
                        GetMoteTemperature(Common.moteNumber);
                    }
                }
            }
            #endregion Get Mote(s)' Temperature Data
        }

        /// <summary>
        /// Function used to execute the TempLogger.exe application.
        /// </summary>
        /// <param name="desiredStringArray"></param>
        /// <param name="undesiredStringArray"></param>
        /// <param name="waitForTime"></param>
        /// <returns></returns>
        private bool ExecuteTempLoggerApp(string[] desiredStringArray, string[] undesiredStringArray, int waitForTime)
        {
            //Append text in output_screen
            Output_ScreenText("\r\nAttempting to execute the " + Common.tempLoggerProcessName + " application...\r\n");
            //Close any open TempLogger.exe
            Common.CloseProgram(Common.tempLoggerProcessName);
            //Set delay to 100ms to make sure that any open TempLogger.exe app is closed
            Delay_ms(100, 1);
            //Select the process to be run
            Common.commandLine = new CommandLineExecute(Common.tempLoggerApplicationProcessName, waitForTime);
            //Check if the task is supposed to be running
            if (!Visible)
            {
                //Exit function since the task is not supposed to be running
                return false;
            }
            //Execute the process
            Common.commandLine.Start1(Common.temperatureLoggerArguments, Common.applicationFilesDirectoryPath, Common.CurrentNetworkManagerApiPortName, false);
            //Check if the task is supposed to be running
            if (!Visible)
            {
                //Exit function since the task is not supposed to be running
                return false;
            }
            //Append text in output_screen
            Output_ScreenText("\r\n" + Common.commandLine.OutputString + "\r\n");
            //Check if the TempLogger.exe application has been successfully started
            if (Common.LookForStringArray(desiredStringArray, undesiredStringArray, Common.commandLine.OutputString) == false)
            {
                //Append text in output_screen
                Output_ScreenText("\r\nCould not execute the " + Common.tempLoggerProcessName + " application!\r\n");
                //Exit function
                return false;
            }
            else
            {
                //Append text in output_screen
                Output_ScreenText("\r\nSuccessfully executed the " + Common.tempLoggerProcessName + " application!\r\n");
                //Exit function
                return true;
            }
        }

        /// <summary>
        /// Function used to get a mote's temperature data.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <returns></returns>
        private bool GetMoteTemperature(int moteNumber)
        {
            #region Temperature Data Acquisition
            //Get the mote's Celsius temperature, if available
            string celsiusTemperature = Common.GetStringBetweenTwoStringsLastIndex(Common.celsiusTemperatureDelimiter1,
                Common.celsiusTemperatureDelimiter2 + Common.mote[moteNumber].mac.ToLower(), 10, Common.commandLine.OutputString);
            //Check if celsiusTemperature is empty or not
            if (!string.IsNullOrEmpty(celsiusTemperature))
            {
                #region Celsius Temperature
                //Store the mote's Celsius temperature
                double.TryParse(celsiusTemperature, out Common.mote[moteNumber].celsiusTemperature);
                //Display the mote's Celsius temperature
                TextBoxCelsiusTemperatureText(moteNumber, Common.mote[moteNumber].celsiusTemperature.ToString("#.##"));
                #endregion Celsius Temperature

                #region Fahrenheit Temperature
                //Store the mote's Fahrenheit temperature
                Common.mote[moteNumber].fahrenheitTemperature = Common.ConvertFromCelsiusToFahrenheit(Common.mote[moteNumber].celsiusTemperature);
                //Display the mote's Fahrenheit temperature
                TextBoxFahrenheitTemperatureText(moteNumber, Common.mote[moteNumber].fahrenheitTemperature.ToString("#.##"));
                #endregion Fahrenheit Temperature

                //Append text in output_screen
                Output_ScreenText("\r\nSuccessfully retrieved the temperature data from mote #" + moteNumber + "!\r\n");
                //Exit function
                return true;
            }
            else
            {
                #region Celsius Temperature
                //Empty out the mote's Celsius temperature textbox
                TextBoxCelsiusTemperatureText(moteNumber, "");
                #endregion Celsius Temperature

                #region Fahrenheit Temperature
                //Empty out the mote's Fahrenheit temperature textbox
                TextBoxFahrenheitTemperatureText(moteNumber, "");
                #endregion Fahrenheit Temperature

                //Append text in output_screen
                Output_ScreenText("\r\nThere is no temperature data available from mote #" + moteNumber + "!\r\n");
                //Exit function
                return false;
            }
            #endregion Temperature Data Acquisition              
        }
        #endregion Application CommandLine Execution

        #region Other Functions
        /// <summary>
        /// Function used to execute a delay while checking that the Temperature Logger GUI is open (visible).
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="dividingCoefficient"></param>
        private void Delay_ms(int millisecondsTimeout, int dividingCoefficient)
        {
            //start loop to execute delay
            for (int counter = 1; counter <= dividingCoefficient; counter++)
            {
                //execute delay
                Thread.Sleep(Convert.ToInt16(millisecondsTimeout / dividingCoefficient));
            }
        }

        /// <summary>
        /// Function used to display the mote(s)' Mac Address.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <param name="backColor"></param>
        private void DisplayMoteMacAddress(int moteNumber, Color backColor)
        {
            //Display the mote's Mac Address
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxMacMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxMacMote.Name == "textBoxMacMote" + moteNumber.ToString())
                    {
                        TextBoxText(textBoxMacMote, Common.mote[moteNumber].mac, backColor);
                    }
                }
            }
        }
        #endregion Other Functions

        #region StopWatchTimer
        /// <summary>
        /// Function used to start the 'stopWatchTimer' timer.
        /// </summary>
        private void StartStopWatchTimer()
        {
            //Reset stopWatchTimer related flags
            stopWatchTimerElapsedHour = 0;
            stopWatchTimerElapsedMinute = 0;
            stopWatchTimerElapsedSecond = 0;
            //Reset 'labelStopWatchTimer' text
            LabelText(labelStopWatchTimer, stopWatchTimerElapsedHour + ":" + stopWatchTimerElapsedMinute +
                ":" + stopWatchTimerElapsedSecond);
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
            stopWatchTimer.Stop();
            stopWatchTimer.Dispose();
            GC.SuppressFinalize(stopWatchTimer);
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
            stopWatchTimerElapsedSecond++;
            if (stopWatchTimerElapsedSecond > 59)
            {
                stopWatchTimerElapsedMinute++;
                stopWatchTimerElapsedSecond = 0;
            }
            if (stopWatchTimerElapsedMinute > 59)
            {
                stopWatchTimerElapsedHour++;
                stopWatchTimerElapsedMinute = 0;
            }

            //Assign 'labelStopWatchTimer' Text
            LabelText(labelStopWatchTimer, stopWatchTimerElapsedHour + ":" + stopWatchTimerElapsedMinute +
                ":" + stopWatchTimerElapsedSecond);


            //Check if the Temperature Logger GUI is open
            if (Visible)
            {
                //Check if getMoteTemperatureThread is still alive
                if (!getMoteTemperatureThread.IsAlive)
                {
                    //Get the mote(s)' temperature data
                    MoteTemperatureDataAcquisition(sender, e);
                }
            }
            else
            {
                //Reset task settings
                Common.UpdateTaskSettings(EnumTaskName.NA, EnumTaskStatus.NA, EnumTaskResult.NA, Common.deviceType, 0);
                //Exit function
                return;
            }
        }
        #endregion StopWatchTimer

        #region Delegates
        private delegate void Output_ScreenTextDelegate(string text);
        private delegate void TextBoxTexttDelegate(TextBox textBox, string text, Color backColor);
        private delegate void LabelTexttDelegate(Label label, string text);
        private delegate void StopWatchTimerLabelTextDelegate(object sender, EventArgs e);
        #endregion Delegates
    }
}