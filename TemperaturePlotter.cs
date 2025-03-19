using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace Network_Manager_GUI
{
    public partial class TemperaturePlotter : Form
    {
        #region Variables/Instances Declaration and Initialization
        #region Timer
        private static int stopWatchTimerElapsedHour = -1;
        private static int stopWatchTimerElapsedMinute = -1;
        private static int stopWatchTimerElapsedSecond = -1;
        #endregion Timer

        #region Thread
        //initialize thread to null
        Thread getMoteDataThread = null;
        #endregion Thread

        #region ZedGraph
        readonly PointPairList[] list = new PointPairList[Common.motesMaxNumber + 1];
        public double time;
        #endregion ZedGraph
        #endregion Variables/Instances Declaration and Initialization

        public TemperaturePlotter()
        {
            InitializeComponent();
        }

        #region GUI Form
        /// <summary>
        /// Function used when the Temperature Plotter GUI is starting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemperaturePlotter_Load(object sender, EventArgs e)
        {
            //Start timer to display the elapsed time since the Temperature Plotter GUI has been opened
            StartStopWatchTimer();
            //Disable the main (Network Manager GUI) form
            Common.Network_Manager_GUI_Enabled(false);
            //Set controls' ToolTip
            SetControlsToolTip();
            #region Setup ZedGraphs
            //Setup ZedGraphs
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (ZedGraphControl zedGraphControl in groupBoxMote.Controls.OfType<ZedGraphControl>())
                {
                    if (zedGraphControl.Name.Contains("zedGraphControl"))
                    {
                        //Get the mote number
                        Common.moteNumber = Convert.ToInt16(zedGraphControl.Name.Replace("zedGraphControl", ""));
                        //Initialize list
                        list[Common.moteNumber] = new PointPairList();
                        //Setup ZedGraph environment
                        CreateChart(zedGraphControl, Common.moteNumber);
                    }
                }
            }
            #endregion Setup ZedGraphs
            //Get the mote(s)' temperature data
            MoteDataAcquisition();
        }

        /// <summary>
        /// Function used when the Temperature Plotter GUI is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemperaturePlotter_FormClosed(object sender, FormClosedEventArgs e)
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
            controlsToolTip.SetToolTip(labelStopWatchTimer, "This label shows the elapsed time since the Temperature Plotter GUI has been opened.");
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
                foreach (System.Windows.Forms.Label labelMacCharactersCountMote in groupBoxMote.Controls.OfType<System.Windows.Forms.Label>())
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
            #region textBoxDigitalInputPin
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxDigitalInputPin in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxDigitalInputPin.Name.Contains("textBoxDigitalInputPin"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(textBoxDigitalInputPin.Name.Replace("textBoxDigitalInputPin", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxDigitalInputPin, "This screen shows the status of the digital input pin used on mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion textBoxDigitalInputPin
            #region zedGraphControl
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (ZedGraphControl zedGraphControl in groupBoxMote.Controls.OfType<ZedGraphControl>())
                {
                    if (zedGraphControl.Name.Contains("zedGraphControl"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(zedGraphControl.Name.Replace("zedGraphControl", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(zedGraphControl, "This screen shows the temperature graph of mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion zedGraphControl
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
        private void LabelText(System.Windows.Forms.Label label, string text)
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

        /// <summary>
        /// Function used to change the text of a given digital ipnut pin level textbox.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <param name="text"></param>
        private void TextBoxDigitalInputPinText(int moteNumber, string text, Color backColor)
        {
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxDigitalInputPin in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxDigitalInputPin.Name == "textBoxDigitalInputPin" + moteNumber.ToString())
                    {
                        TextBoxText(textBoxDigitalInputPin, text, backColor);
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
                foreach (System.Windows.Forms.Label labelMacCharactersCountMote in groupBoxMote.Controls.OfType<System.Windows.Forms.Label>())
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
        private void MoteDataAcquisition()
        {
            //Create a thread to avoid GUI hang
            getMoteDataThread = new Thread(() =>
            {
                try
                {
                    //get the mote(s)' data through multithreading
                    GetMoteDataThread();
                }
                catch (Exception e)
                {
                    string message = e.Message;
                }
            })
            {
                //set thread as a background thread
                IsBackground = true
            };
            //start thread to get the mote(s)' data
            getMoteDataThread.Start();
        }

        /// <summary>
        /// Function used to get the mote(s)' data through multithreading.
        /// </summary>
        private void GetMoteDataThread()
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
                        //keep looping as long as the Temperature Plotter GUI is open
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
                            //keep looping as long as the Temperature Plotter GUI is open
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

            #region Get Mote(s)' Data
            //Check if the number of operational motes is greater than 0
            if (Common.numberOfOperationalMotes > 0)
            {
                //Check if the PublishToWeb.exe application is already running
                bool isPublishToWebAppRunning = Common.IsProgramRunning(Common.publishToWebProcessName);
                //Start the PublishToWeb.exe application if it is not already running
                if (isPublishToWebAppRunning == false)
                {
                    //Append text in output_screen
                    Output_ScreenText("\r\nAttempting to execute the " + Common.publishToWebProcessName + " application...\r\n");
                    //Execute the PublishToWeb.exe application
                    isPublishToWebAppRunning = Common.ExecutePublishToWebApp(1000);
                    //Append text in output_screen
                    Output_ScreenText("\r\n" + Common.commandLine.OutputString + "\r\n");
                    //Check if the PublishToWeb.exe application has been successfully executed
                    if (isPublishToWebAppRunning == true)
                    {
                        //Append text in output_screen
                        Output_ScreenText("\r\nSuccessfully started the " + Common.publishToWebProcessName + " application!\r\n");
                    }
                    else
                    {
                        //Append text in output_screen
                        Output_ScreenText("\r\nCould not start the " + Common.publishToWebProcessName + " application!\r\n");
                    }
                }
                else //the PublishToWeb.exe application is already running
                {
                    //Append text in output_screen
                    Output_ScreenText("\r\nThe " + Common.publishToWebProcessName + " application is already running!\r\n");
                }
            }
            //Get Mote(s)' Data
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
                        //Close any open PublishToWeb.exe so that its log file can be accessed
                        Common.CloseProgram(Common.publishToWebProcessName);
                        //Set delay to 100ms to make sure that any open PublishToWeb.exe app is closed
                        Delay_ms(100, 1);
                        //Extract "most" recent available packet
                        string moteDataPacketLine = Common.ExtractFromPublishToWebLog(Common.ConvertMacAddressFromHexToDec(mote.mac));
                        //Gather all the fields of the packet
                        bool getPacketFieldsResult = Common.GetPacketFields(Common.moteNumber, moteDataPacketLine);
                        //Get a mote's temperature data
                        GetMoteTemperature(Common.moteNumber);
                        //Get digital input pin level
                        GetDigitalInputPinLevel(Common.moteNumber);
                    }
                }
            }
            #endregion Get Mote(s)' Data
        }

        /// <summary>
        /// Function used to get a mote's temperature data.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <returns></returns>
        private bool GetMoteTemperature(int moteNumber)
        {
            #region Temperature Data Acquisition
            //Merge 2 bytes of data into 1 16-bit number
            int rawTemperatureData1 = Common.packetStatistics[moteNumber].data[1]; //store 1st byte (high byte)
            int rawTemperatureData2 = Common.packetStatistics[moteNumber].data[2]; //store 2nd byte (low byte)
            int rawTemperatureData = (rawTemperatureData1 << 8) | (rawTemperatureData2); //merge the 2 bytes into 1 16-bit number
            //Get the mote's Celsius temperature, if available
            string celsiusTemperature = (rawTemperatureData / 100.0).ToString("#.##"); /*Why dividing by 100.0? Because 1 degree 
            Celsius corresponds to 10mV and rawTemperatureData is expressed in 10E-4V (0.1mV); therefore, 10mV/0.1mV = 100.*/
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

                #region Plotting
                foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
                {
                    foreach (ZedGraphControl zedGraphControl in groupBoxMote.Controls.OfType<ZedGraphControl>())
                    {
                        if (zedGraphControl.Name == "zedGraphControl" + moteNumber)
                        {
                            //Update ZedGraph with new temperature data
                            list[moteNumber].Add(time, Common.mote[moteNumber].fahrenheitTemperature);
                            zedGraphControl.Invalidate();
                        }
                    }
                }
                #endregion Plotting
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

        /// <summary>
        /// Function used to get a mote's digital input pin level.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <returns></returns>
        private int GetDigitalInputPinLevel(int moteNumber)
        {
            //Store the mote's digital input pin level
            int digitalInputPinLevel = Common.packetStatistics[moteNumber].data[0];
            //Display the  mote's digital input pin level
            string message = "The mote's digital input pin level is " + digitalInputPinLevel + ".";
            //Depending on the value of digitalInputPinLevel, the digitalInputPinLevel message will get displayed
            if (digitalInputPinLevel == 0)
            {
                TextBoxDigitalInputPinText(moteNumber, message, Color.LightPink); //Pin level 0 => LightPink
            }
            else //if (digitalInputPinLevel == 1)
            {
                TextBoxDigitalInputPinText(moteNumber, message, Color.Orange); //Pin level 1 => Orange
            }
            //Return digitalInputPinLevel
            return digitalInputPinLevel;
        }
        #endregion Application CommandLine Execution

        #region ZedGraph
        /// <summary>
        /// Function used to setup the Zedgraph(s).
        /// </summary>
        /// <param name="zedGraph"></param>
        /// <param name="moteNumber"></param>
        private void CreateChart(ZedGraphControl zedGraph, int moteNumber)
        {
            //Declare a new GraphPane object 
            GraphPane myPane = zedGraph.GraphPane;
            //Set the titles and axis labels
            myPane.Title.Text = "Plot of Actual Temperature";
            myPane.XAxis.Title.Text = "Time Elapsed (s)";
            myPane.YAxis.Title.Text = "Temperature  (ºF)";

            //Generate a red curve with diamond symbols, and "Alpha" in the legend
            LineItem myCurve = myPane.AddCurve("", list[moteNumber], Color.Red, SymbolType.None);

            //Just manually control the X axis range so it scrolls continuously
            //instead of discrete step-sized jumps
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 30;
            myPane.XAxis.Scale.MinorStep = 1;
            myPane.XAxis.Scale.MajorStep = 5;
            //Y axis settings
            myPane.YAxis.Scale.Min = -20;
            myPane.YAxis.Scale.Max = 160;
            myPane.YAxis.Scale.MinorStep = 1;
            myPane.YAxis.Scale.MajorStep = 5;

            //Scale the axes
            zedGraph.AxisChange();

            //Save the beginning time for reference
            //tickStart = Environment.TickCount;

            //activate the cardinal spline smoothing
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.5F;

            //Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;
            //Make the Y axis scale blue
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Blue;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Blue;
            //Make the X axis scale blue
            myPane.XAxis.Scale.FontSpec.FontColor = Color.Blue;
            myPane.XAxis.Title.FontSpec.FontColor = Color.Blue;
            //turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            //Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            //Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;

            //Fill the axis background with a gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.Gray, 45.0f);
        }
        #endregion ZedGraph

        #region Other Functions
        /// <summary>
        /// Function used to execute a delay while checking that the Temperature Plotter GUI is open (visible).
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


            //Check if the Temperature Plotter GUI is open
            if (Visible)
            {
                //Check if getMoteDataThread is still alive
                if (!getMoteDataThread.IsAlive)
                {
                    //Get the mote(s)' temperature data
                    MoteDataAcquisition();
                }
            }
            else
            {
                //Reset task settings
                Common.UpdateTaskSettings(EnumTaskName.NA, EnumTaskStatus.NA, EnumTaskResult.NA, Common.deviceType, 0);
                //Exit function
                return;
            }

            //Update ZedGraph environment
            foreach (Common.MoteStruct mote in Common.mote)
            {
                //Make sure the current mote's state is not null
                if (!string.IsNullOrEmpty(mote.state))
                {
                    //Declare moteNumber
                    int moteNumber = mote.moteID - 1; //subtract 1 since the network manager's moteID = 1
                    //Check if the current mote's state is "operational"
                    if (mote.state.ToUpper() == Common.moteStates[2].ToUpper())
                    {
                        foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
                        {
                            foreach (ZedGraphControl zedGraphControl in groupBoxMote.Controls.OfType<ZedGraphControl>())
                            {
                                if (zedGraphControl.Name == "zedGraphControl" + moteNumber)
                                {
                                    // Time is measured in seconds
                                    time = (Environment.TickCount) / 1000.0;

                                    // Keep the X scale at a rolling 30 second interval, with one
                                    // major step between the max X value and the end of the axis
                                    Scale xScale = zedGraphControl.GraphPane.XAxis.Scale;
                                    if (time > xScale.Max - xScale.MajorStep)
                                    {
                                        xScale.Max = time + xScale.MajorStep;
                                        xScale.Min = xScale.Max - 30.0;
                                    }
                                    //Update zedGraph environment
                                    zedGraphControl.Refresh();
                                    zedGraphControl.AxisChange();
                                    zedGraphControl.Invalidate();
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion StopWatchTimer

        #region Delegates
        private delegate void Output_ScreenTextDelegate(string text);
        private delegate void TextBoxTexttDelegate(TextBox textBox, string text, Color backColor);
        private delegate void LabelTexttDelegate(System.Windows.Forms.Label label, string text);
        private delegate void StopWatchTimerLabelTextDelegate(object sender, EventArgs e);
        #endregion Delegates
    }
}