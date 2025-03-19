using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Network_Manager_GUI
{
    public partial class NetworkStatistics : Form
    {
        #region Variables/Instances Declaration and Initialization
        #region Timer
        private static int stopWatchTimerElapsedHour = -1;
        private static int stopWatchTimerElapsedMinute = -1;
        private static int stopWatchTimerElapsedSecond = -1;
        #endregion Timer

        #region Thread
        //initialize thread to null
        Thread getNetworkStatisticsDataThread = null;
        #endregion Thread
        #endregion Variables/Instances Declaration and Initialization

        public NetworkStatistics()
        {
            InitializeComponent();
        }

        #region GUI Form
        /// <summary>
        /// Function used when the Statistics GUI is starting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Statistics_Load(object sender, EventArgs e)
        {
            //Start timer to display the elapsed time since the Statistics GUI has been opened
            StartStopWatchTimer();
            //Disable the main (Network Manager GUI) form
            Common.Network_Manager_GUI_Enabled(false);
            //Set controls' ToolTip
            SetControlsToolTip();
            //Get all the network statistics data
            NetworkStatisticsDataAcquisition();
        }

        /// <summary>
        /// Function used when the Network Statistics GUI is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Statistics_FormClosed(object sender, FormClosedEventArgs e)
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
            controlsToolTip.SetToolTip(labelStopWatchTimer, "This label shows the elapsed time since the Network Statistics GUI has been opened.");

            #region Mote
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
            #region textBoxReceivedMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxReceivedMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxReceivedMote.Name.Contains("textBoxReceivedMote"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(textBoxReceivedMote.Name.Replace("textBoxReceivedMote", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxReceivedMote, "This screen shows the Received value from mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion textBoxReceivedMote
            #region textBoxLostMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxLostMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxLostMote.Name.Contains("textBoxLostMote"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(textBoxLostMote.Name.Replace("textBoxLostMote", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxLostMote, "This screen shows the Lost value from mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion textBoxLostMote
            #region textBoxReliabilityMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxReliabilityMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxReliabilityMote.Name.Contains("textBoxReliabilityMote"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(textBoxReliabilityMote.Name.Replace("textBoxReliabilityMote", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxReliabilityMote, "This screen shows the Reliability value from mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion textBoxReliabilityMote
            #region textBoxLatencyMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxLatencyMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxLatencyMote.Name.Contains("textBoxLatencyMote"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(textBoxLatencyMote.Name.Replace("textBoxLatencyMote", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxLatencyMote, "This screen shows the Latency value from mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion textBoxLatencyMote
            #region textBoxHopsMote
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxHopsMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxHopsMote.Name.Contains("textBoxHopsMote"))
                    {
                        //get the mote number
                        Common.moteNumber = Convert.ToInt16(textBoxHopsMote.Name.Replace("textBoxHopsMote", ""));
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxHopsMote, "This screen shows the Hops value from mote #" + Common.moteNumber + ".");
                    }
                }
            }
            #endregion textBoxHopsMote
            #endregion Mote

            #region Network Manager
            #region textBoxMacNetworkManager
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxMacNetworkManager in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxMacNetworkManager.Name.Contains("textBoxMacNetworkManager"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxMacNetworkManager, "This screen shows the MAC Address of the network manager.");
                    }
                }
            }
            #endregion textBoxMacNetworkManager
            #region textBoxEstablishedConnections
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxEstablishedConnections in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxEstablishedConnections.Name.Contains("textBoxEstablishedConnections"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxEstablishedConnections, "This screen shows the Established Connections value from the network manager.");
                    }
                }
            }
            #endregion textBoxEstablishedConnections
            #region textBoxDroppedConnections
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxDroppedConnections in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxDroppedConnections.Name.Contains("textBoxDroppedConnections"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxDroppedConnections, "This screen shows the Dropped Connections value from the network manager.");
                    }
                }
            }
            #endregion textBoxDroppedConnections
            #region textBoxTransmitOK
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxTransmitOK in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxTransmitOK.Name.Contains("textBoxTransmitOK"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxTransmitOK, "This screen shows the Transmit OK value from the network manager.");
                    }
                }
            }
            #endregion textBoxTransmitOK
            #region textBoxTransmitError
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxTransmitError in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxTransmitError.Name.Contains("textBoxTransmitError"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxTransmitError, "This screen shows the Transmit Error value from the network manager.");
                    }
                }
            }
            #endregion textBoxTransmitError
            #region textBoxTransmitRepeat
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxTransmitRepeat in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxTransmitRepeat.Name.Contains("textBoxTransmitRepeat"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxTransmitRepeat, "This screen shows the Transmit Repeat value from the network manager.");
                    }
                }
            }
            #endregion textBoxTransmitRepeat
            #region textBoxReceiveOK
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxReceiveOK in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxReceiveOK.Name.Contains("textBoxReceiveOK"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxReceiveOK, "This screen shows the Receive OK value from the network manager.");
                    }
                }
            }
            #endregion textBoxReceiveOK
            #region textBoxReceiveError
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxReceiveError in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxReceiveError.Name.Contains("textBoxReceiveError"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxReceiveError, "This screen shows the Receive Error value from the network manager.");
                    }
                }
            }
            #endregion textBoxReceiveError
            #region textBoxAckDelayAvrg
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxAckDelayAvrg in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxAckDelayAvrg.Name.Contains("textBoxAckDelayAvrg"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxAckDelayAvrg, "This screen shows the Acknowledge Delay Average value from the network manager.");
                    }
                }
            }
            #endregion textBoxAckDelayAvrg
            #region textBoxAckDelayMax
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxAckDelayMax in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxAckDelayMax.Name.Contains("textBoxAckDelayMax"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxAckDelayMax, "This screen shows the Acknowledge Delay Maximum value from the network manager.");
                    }
                }
            }
            #endregion textBoxAckDelayMax
            #endregion Network Manager

            #region Network Statistics            
            #region textBoxReliability
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxReliability in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxReliability.Name == "textBoxReliability")
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxReliability, "This screen shows the overall Reliability value of the SmartMesh IP network.");
                    }
                }
            }
            #endregion textBoxReliability
            #region textBoxStability
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxStability in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxStability.Name.Contains("textBoxStability"))
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxStability, "This screen shows the overall Stability value of the SmartMesh IP network.");
                    }
                }
            }
            #endregion textBoxStability
            #region textBoxLatency
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxLatency in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxLatency.Name == "textBoxLatency")
                    {
                        //set tooltip
                        controlsToolTip.SetToolTip(textBoxLatency, "This screen shows the overall Latency value of the SmartMesh IP network.");
                    }
                }
            }
            #endregion textBoxLatency
            #endregion Network Statistics
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
        /// Function used to change the text of a given Received textbox.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <param name="text"></param>
        private void TextBoxReceivedMoteText(int moteNumber, string text)
        {
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxReceivedMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxReceivedMote.Name == "textBoxReceivedMote" + moteNumber.ToString())
                    {
                        TextBoxText(textBoxReceivedMote, text, Color.WhiteSmoke);
                    }
                }
            }
        }

        /// <summary>
        /// Function used to change the text of a given Lost textbox.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <param name="text"></param>
        private void TextBoxLostMoteText(int moteNumber, string text)
        {
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxLostMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxLostMote.Name == "textBoxLostMote" + moteNumber.ToString())
                    {
                        TextBoxText(textBoxLostMote, text, Color.WhiteSmoke);
                    }
                }
            }
        }

        /// <summary>
        /// Function used to change the text of a given Reliability textbox.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <param name="text"></param>
        private void TextBoxReliabilityMoteText(int moteNumber, string text)
        {
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxReliabilityMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxReliabilityMote.Name == "textBoxReliabilityMote" + moteNumber.ToString())
                    {
                        TextBoxText(textBoxReliabilityMote, text, Color.WhiteSmoke);
                    }
                }
            }
        }

        /// <summary>
        /// Function used to change the text of a given Latency textbox.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <param name="text"></param>
        private void TextBoxLatencyMoteText(int moteNumber, string text)
        {
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxLatencyMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxLatencyMote.Name == "textBoxLatencyMote" + moteNumber.ToString())
                    {
                        TextBoxText(textBoxLatencyMote, text, Color.WhiteSmoke);
                    }
                }
            }
        }

        /// <summary>
        /// Function used to change the text of a given Hops textbox.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <param name="text"></param>
        private void TextBoxHopsMoteText(int moteNumber, string text)
        {
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (TextBox textBoxHopsMote in groupBoxMote.Controls.OfType<TextBox>())
                {
                    if (textBoxHopsMote.Name == "textBoxHopsMote" + moteNumber.ToString())
                    {
                        TextBoxText(textBoxHopsMote, text, Color.WhiteSmoke);
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

        /// <summary>
        /// Function called when the 'textBoxMacNetworkManager' Text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxMacNetworkManager_TextChanged(object sender, EventArgs e)
        {
            //display the number of characters in the actual textBoxMacNetworkManager
            foreach (GroupBox groupBoxMote in Controls.OfType<GroupBox>())
            {
                foreach (Label labelMacCharactersCountNetworkManager in groupBoxMote.Controls.OfType<Label>())
                {
                    if (labelMacCharactersCountNetworkManager.Name == "labelMacCharactersCountNetworkManager")
                    {
                        LabelText(labelMacCharactersCountNetworkManager, textBoxMacNetworkManager.Text.Length + " character(s)");
                    }
                }
            }
        }
        #endregion TextChanged Control

        #region Statistics Data Acquisition
        /// <summary>
        /// Function used to get all the network statistics data.
        /// </summary>
        private void NetworkStatisticsDataAcquisition()
        {
            //Create a thread to avoid GUI hang
            getNetworkStatisticsDataThread = new Thread(() =>
            {
                try
                {
                    //get all the network statistics data through multithreading
                    GetNetworkStatisticsDataThread();
                }
                catch { }
            })
            {
                //set thread as a background thread
                IsBackground = true
            };
            //start thread to get the mote(s)' statistics
            getNetworkStatisticsDataThread.Start();
        }

        /// <summary>
        /// Function used to get all the network statistics data through multithreading.
        /// </summary>
        private void GetNetworkStatisticsDataThread()
        {
            //Get all the network statistics about every 45 seconds
            if (stopWatchTimerElapsedSecond < 15)
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
                            //keep looping as long as the Network Statistics GUI is open
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
                                //keep looping as long as the Network Statistics GUI is open
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
                                                             //Check if the current mote's state is operational
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

                #region Get All Network Statistics Data
                Here3:
                //Append text in output_screen
                Output_ScreenText("\r\nGetting all the SmartMesh-IP network statistics data...\r\n");
                //Get all the SmartMesh-IP network statistics data
                Common.GetOverallNetworkStatistics();
                Here4:
                //Delay to gather all the SmartMesh-IP network statistics data
                if (Common.numberOfFoundMotes == 0)
                {
                    Delay_ms(5000, 10); //set delay to 5000ms
                }
                else //if (Common.numberOfFoundMotes != 0)
                {
                    Delay_ms(Common.numberOfFoundMotes * 2000, 10); //set delay based on the number of found motes
                }
                //Check if the GET_NETWORKSTATISTICS task is successful
                if (Common.taskName == EnumTaskName.GET_NETWORKSTATISTICS)
                {
                    if (Common.taskStatus == EnumTaskStatus.STARTED)
                    {
                        //keep looping as long as the Network Statistics GUI is open
                        if (Visible)
                        {
                            //keep waiting until task is done
                            goto Here3;
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
                            //keep looping as long as the Network Statistics GUI is open
                            if (Visible)
                            {
                                //look for motes again
                                goto Here4;
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
                Output_ScreenText("\r\nAll the SmartMesh-IP network statistics data may have been gathered!\r\n");
                #endregion Get All Network Statistics Data

                #region Process Network's Statistics Data
                //process the overall network's statistics data
                ProcessNetworkStatisticsData();
                #endregion Process Network's Statistics Data

                #region Process Network Manager's Statistics Data
                //Process the network manager's statistics data
                ProcessNetworkManagerStatisticsData();
                #endregion Process Network Manager's Statistics Data

                #region Process Mote(s)' Statistics Data
                //Get the mote(s)' statistics data
                foreach (Common.MoteStruct mote in Common.mote)
                {
                    //Make sure the current mote's state is not null
                    if (!string.IsNullOrEmpty(mote.state))
                    {
                        //Update Common.moteNumber
                        Common.moteNumber = mote.moteID - 1; //subtract 1 since the network manager's moteID = 1
                                                             //Check if the current mote's state is operational
                        if (mote.state.ToUpper() == Common.moteStates[2].ToUpper())
                        {
                            //Process the mote(s)' statistics data
                            ProcessMoteStatisticsData(Common.moteNumber);
                        }
                    }
                }
                #endregion Process Mote(s)' Statistics Data
            }
        }

        #region Mote(s)' Statistics Data Processing
        /// <summary>
        /// Function used to process the mote(s)' statistics data.
        /// </summary>
        /// <param name="moteNumber"></param>
        /// <returns></returns>
        private void ProcessMoteStatisticsData(int moteNumber)
        {
            //Append text in output_screen
            Output_ScreenText("\r\nDisplaying the statistics data from mote #" + moteNumber + "...\r\n");
            //Display the mote's Received value 
            TextBoxReceivedMoteText(moteNumber, Common.moteStatistics[moteNumber].received.ToString());
            //Display the mote's Lost value 
            TextBoxLostMoteText(moteNumber, Common.moteStatistics[moteNumber].lost.ToString());
            //Display the mote's Reliability value 
            TextBoxReliabilityMoteText(moteNumber, Common.moteStatistics[moteNumber].reliability);
            //Display the mote's Latency value 
            TextBoxLatencyMoteText(moteNumber, Common.moteStatistics[moteNumber].latency.ToString() + Common.latencyUnit);
            //Display the mote's Hops value 
            TextBoxHopsMoteText(moteNumber, Common.moteStatistics[moteNumber].hops.ToString());
            //Append text in output_screen
            Output_ScreenText("\r\nSuccessfully displayed the statistics data from mote #" + moteNumber + "!\r\n");
        }
        #endregion Mote(s)' Statistics Data Processing

        #region Network Manager's Statistics Data Processing
        /// <summary>
        /// Function used to process the network manager's statistics data.
        /// </summary>
        private void ProcessNetworkManagerStatisticsData()
        {
            #region Mac Address
            //Display the network manager's Mac Address
            TextBoxText(textBoxMacNetworkManager, NetworkManager.macAddress.ToUpper(), Color.LightGreen);
            #endregion Mac Address

            //Append text in output_screen
            Output_ScreenText("\r\nDisplaying the statistics data from the network manager...\r\n");
            //Display the network manager's Established Connections value 
            TextBoxText(textBoxEstablishedConnections, Common.networkManagerStatistics.establishedConnections.ToString(), Color.WhiteSmoke);
            //Display the network manager's Dropped Connections value 
            TextBoxText(textBoxDroppedConnections, Common.networkManagerStatistics.droppedConnections.ToString(), Color.WhiteSmoke);
            //Display the network manager's Transmit OK value 
            TextBoxText(textBoxTransmitOK, Common.networkManagerStatistics.transmitOK.ToString(), Color.WhiteSmoke);
            //Display the network manager's Transmit Error value 
            TextBoxText(textBoxTransmitError, Common.networkManagerStatistics.transmitError.ToString(), Color.WhiteSmoke);
            //Display the network manager's Transmit Repeat value 
            TextBoxText(textBoxTransmitRepeat, Common.networkManagerStatistics.transmitRepeat.ToString(), Color.WhiteSmoke);
            //Display the network manager's Receive OK value 
            TextBoxText(textBoxReceiveOK, Common.networkManagerStatistics.receiveOK.ToString(), Color.WhiteSmoke);
            //Display the network manager's Receive Error value 
            TextBoxText(textBoxReceiveError, Common.networkManagerStatistics.receiveError.ToString(), Color.WhiteSmoke);
            //Display the network manager's Ack Delay Avrg value 
            TextBoxText(textBoxAckDelayAvrg, Common.networkManagerStatistics.acknowledgeDelayAvrg, Color.WhiteSmoke);
            //Display the network manager's Ack Delay Max value 
            TextBoxText(textBoxAckDelayMax, Common.networkManagerStatistics.acknowledgeDelayMax, Color.WhiteSmoke);
            //Append text in output_screen
            Output_ScreenText("\r\nSuccessfully displayed the statistics data from the network manager!\r\n");
        }
        #endregion Network Manager's Statistics Data Processing

        #region Network's Statistics Data Processing
        /// <summary>
        /// Function used to process the overall network's statistics data.
        /// </summary>
        private void ProcessNetworkStatisticsData()
        {
            //Append text in output_screen
            Output_ScreenText("\r\nDisplaying the overall statistics data of the SmartMesh IP network...\r\n");
            //Display the network's Reliability value 
            TextBoxText(textBoxReliability, Common.networkStatistics.reliability, Color.WhiteSmoke);
            //Display the network's Stability value 
            TextBoxText(textBoxStability, Common.networkStatistics.stability, Color.WhiteSmoke);
            //Display the network's Latency value 
            TextBoxText(textBoxLatency, Common.networkStatistics.latency, Color.WhiteSmoke);
            //Append text in output_screen
            Output_ScreenText("\r\nSuccessfully displayed the overall statistics data of the SmartMesh IP network!\r\n");
        }
        #endregion Network's Statistics Data Processing
        #endregion Statistics Data Acquisition

        #region Other Functions
        /// <summary>
        /// Function used to execute a delay while checking that the Network Statistics GUI is open (visible).
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

            //Check if the Network Statistics GUI is open
            if (Visible)
            {
                //Check if getNetworkStatisticsDataThread is still alive
                if (!getNetworkStatisticsDataThread.IsAlive)
                {
                    //Get all the network statistics data
                    NetworkStatisticsDataAcquisition();
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
