using System;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Network_Manager_GUI
{
    public partial class LoadFirmware : Form
    {
        public LoadFirmware()
        {
            InitializeComponent();
        }

        #region GUI Form
        /// <summary>
        /// Function used when the Load Firmware GUI is starting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFirmware_Load(object sender, EventArgs e)
        {
            //Disable the main (Network Manager GUI) form
            Common.Network_Manager_GUI_Enabled(false);
            //Set controls' ToolTip
            SetControlsToolTip();
            //Update task settings
            UpdateTaskSettings(EnumTaskName.NA, EnumTaskStatus.NA, EnumTaskResult.NA, Common.deviceType, 0);
        }

        /// <summary>
        /// Function used when the Load Firmware GUI is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFirmware_FormClosed(object sender, FormClosedEventArgs e)
        {
            Common.Network_Manager_GUI_Enabled(true); //enable the main (Network Manager GUI) form
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
            controlsToolTip.SetToolTip(buttonNetworkManager, "Click to load the firmware on the Network Manager.");
            controlsToolTip.SetToolTip(buttonMote, "Click to load the firmware on the Mote.");
            controlsToolTip.SetToolTip(buttonAccessPoint, "Click to load the firmware on the Access Point.");
            controlsToolTip.SetToolTip(output_screen, "This screen shows a log of events.");
            controlsToolTip.SetToolTip(pictureBoxOutcome, "This picture shows the outcome of the current or last-executed task.");
            controlsToolTip.SetToolTip(labelStopWatchTimer, "This label shows the elapsed time of the current or last-executed task.");
            controlsToolTip.SetToolTip(labelOutputScreenCharactersCount, "This label shows the current number of characters  in the output screen.");
        }
        #endregion ToolTip

        #region Button Clicks
        /// <summary>
        /// Function used to load the proper firmware on the Network Manager.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNetworkManager_Click(object sender, EventArgs e)
        {
            //Load firmware on Network Manager
            FirmwareLoad(EnumDeviceType.NETWORK_MANAGER, NetworkManager.networkManagerID, NetworkManager.networkManagerEraseFirmwareResponse, NetworkManager.networkManagerLoadFirmwareResponse, NetworkManager.firmwareLoadTaskTimeout, NetworkManager.networkManagerFirmwareFilename);
        }

        /// <summary>
        /// Function used to load the proper firmware on the Mote.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMote_Click(object sender, EventArgs e)
        {
            //Load firmware on Mote
            FirmwareLoad(EnumDeviceType.MOTE, Mote.validMoteID, Mote.moteEraseFirmwareResponse, Mote.moteLoadFirmwareResponse, Mote.firmwareLoadTaskTimeout, Mote.moteFirmwareFilename);
        }

        /// <summary>
        /// Function used to load the proper firmware on the Access Point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAccessPoint_Click(object sender, EventArgs e)
        {
            //Load firmware on Access Point
            FirmwareLoad(EnumDeviceType.ACCESS_POINT, AccessPoint.accessPointID, AccessPoint.accessPointEraseFirmwareResponse, AccessPoint.accessPointLoadFirmwareResponse, AccessPoint.firmwareLoadTaskTimeout, AccessPoint.accessPointFirmwareFilename);
        }
        #endregion Button Clicks

        #region Firmware Load Function(s)
        /// <summary>
        /// Function used to load firmware on Network Manager, Mote, or Access Point
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="iD"></param>
        /// <param name="eraseFirmwareResponse"></param>
        /// <param name="loadFirmwareResponse"></param>
        /// <param name="firmwareLoadTaskTimeout"></param>
        /// <param name="firmwareFilename"></param>
        private void FirmwareLoad(EnumDeviceType deviceType, string[] iD, string[] eraseFirmwareResponse, string[] loadFirmwareResponse, int firmwareLoadTaskTimeout, string firmwareFilename)
        {
            //Update task settings
            UpdateTaskSettings(EnumTaskName.FIRMWARELOAD, EnumTaskStatus.STARTED, EnumTaskResult.UNSUCCESSFUL, deviceType, firmwareLoadTaskTimeout);
            //Create a thread to avoid GUI hang
            Thread firmwareLoadThread = new Thread(() =>
            {
                //Check if the task is supposed to be running
                if (Common.taskTimer.Enabled)
                {
                    //Firstly, find the Smartmesh Ip device(s)
                    Common.isFirstPartSuccessful = Common.LookForSmartmeshIpDevices(deviceType, iD, Common.undesiredEspCommandLineResponse, Common.commandLineProcessTimeout);
                }

                //Check if the task is supposed to be running
                if (Common.taskTimer.Enabled)
                {
                    //Secondly, erase old firmware
                    if (Common.isFirstPartSuccessful == true) //check if 1st step succeeded
                    {
                        Common.isSecondPartSuccessful = Common.EraseOldFirmware(deviceType, eraseFirmwareResponse, Common.undesiredEspCommandLineResponse, Common.commandLineProcessTimeout);
                    }
                }

                //Check if the task is supposed to be running
                if (Common.taskTimer.Enabled)
                {
                    //Thirdly and finally, load new firmware
                    if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true) //check if 1st and 2nd steps succeeded
                    {
                        Common.isThirdPartSuccessful = Common.LoadNewFirmware(deviceType, loadFirmwareResponse, Common.undesiredEspCommandLineResponse, Common.commandLineProcessTimeout, firmwareFilename);
                    }
                }

                //Check if the task is supposed to be running
                if (Common.taskTimer.Enabled)
                {
                    //Update task settings
                    if (Common.isFirstPartSuccessful == true && Common.isSecondPartSuccessful == true && Common.isThirdPartSuccessful == true)
                    {
                        UpdateTaskSettings(EnumTaskName.FIRMWARELOAD, EnumTaskStatus.COMPLETED, EnumTaskResult.SUCCESSFUL, deviceType, firmwareLoadTaskTimeout);
                    }
                    else
                    {
                        UpdateTaskSettings(EnumTaskName.FIRMWARELOAD, EnumTaskStatus.COMPLETED, EnumTaskResult.UNSUCCESSFUL, deviceType, firmwareLoadTaskTimeout);
                    }
                }
            });
            //Start thread for loading firmware
            firmwareLoadThread.Start();
        }
        #endregion Firmware Load Function(s)

        #region Text Control
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
        #endregion TextChanged Control

        #region Enabled Control
        /// <summary>
        /// Function used to change the Enabled feature of the buttons on the GUI.
        /// </summary>
        /// <param name="enabled"></param>
        private void ButtonsEnabled(bool enabled)
        {
            // If this returns true, it means it was called from an external thread.
            if (InvokeRequired)
            {
                // Create a delegate of this method and let the form run it.
                Invoke(new ButtonsEnabledDelegate(ButtonsEnabled), new object[] { enabled });
                return; // Important
            }

            // Assign Enabled
            buttonNetworkManager.Enabled = enabled;
            buttonMote.Enabled = enabled;
            buttonAccessPoint.Enabled = enabled;
        }
        #endregion Enabled Control

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

        #region Task Settings
        /// <summary>
        /// Function used to update Task Settings.
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="taskStatus"></param>
        /// <param name="taskResult"></param>
        /// <param name="deviceType"></param>
        /// <param name="taskTimeout"></param>
        private void UpdateTaskSettings(EnumTaskName taskName, EnumTaskStatus taskStatus, EnumTaskResult taskResult, EnumDeviceType deviceType, int taskTimeout)
        {
            //Update task settings variables
            Common.taskName = taskName;
            Common.taskStatus = taskStatus;
            Common.taskResult = taskResult;
            Common.deviceType = deviceType;
            Common.taskTimeout = taskTimeout;

            if (Common.taskStatus == EnumTaskStatus.STARTED)
            {
                //Disable GUI buttons
                ButtonsEnabled(false);
                //Reset the flags used for multi-step tasks
                Common.isFirstPartSuccessful = false;
                Common.isSecondPartSuccessful = false;
                Common.isThirdPartSuccessful = false;
                //Start timer to wait for task completion
                StartTaskExecutionTimer();
                //Start timer to display elapsed time during task execution
                StartStopWatchTimer();
                //Display task execution start message
                Output_ScreenText("\r\nThe " + Common.taskName + " task has been " + Common.taskStatus + "!\r\n");
            }
            else if (Common.taskStatus == EnumTaskStatus.COMPLETED || Common.taskStatus == EnumTaskStatus.CANCELED)
            {
                //Enable GUI buttons
                ButtonsEnabled(true);
                //Stop the 'taskTimer' timer
                StopTaskExecutionTimer();
                //Stop the 'stopWatchTimer' timer
                StopStopWatchTimer();
                //Display task execution outcome message
                Output_ScreenText("\r\nThe " + Common.taskName + " task has been " + Common.taskStatus +
                        " and " + Common.taskResult + "!\r\n");
            }
            //Update task outcome objects
            UpdateTaskOutcomeObjects();
        }
        #endregion Task Settings

        #region Task Outcome
        /// <summary>
        ///  Function used to update task outcome objects.
        /// </summary>
        private void UpdateTaskOutcomeObjects()
        {
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
        }
        #endregion Task Outcome

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
            Output_ScreenText("\r\nThe " + Common.deviceType + "'s " + Common.taskName + " task will run for up to " +
                Common.taskTimeout / 1000 + " seconds before auto-cancellation. The task timer has started!\r\n");
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
                Output_ScreenText("\r\nThe " + Common.deviceType + "'s " + Common.taskName + " task ran for more than the allocated " +
                Common.taskTimeout / 1000 + " seconds threshold. Therefore, this task is being auto-cancelled!\r\n");
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
            //Reset stopWatchTimer related flags
            Common.stopWatchTimerElapsedHour = 0;
            Common.stopWatchTimerElapsedMinute = 0;
            Common.stopWatchTimerElapsedSecond = 0;
            //Reset 'labelStopWatchTimer' text
            LabelStopWatchTimerText(Common.stopWatchTimerElapsedHour + ":" + Common.stopWatchTimerElapsedMinute +
                ":" + Common.stopWatchTimerElapsedSecond);
            //Create a new instance of stopWatchTimer
            stopWatchTimer = new System.Windows.Forms.Timer
            {
                //Set the time, in milliseconds, before the Tick event is raised
                Interval = 1000
            };
            //StopWatchTimerTick will handle the event raised when the specified timer interval has elapsed and the timer is enabled.
            stopWatchTimer.Tick += new EventHandler(StopWatchTimerTick);
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
        #endregion StopWatchTimer
        #endregion Timer

        #region Delegates
        private delegate void Output_ScreenTextDelegate(string text);
        private delegate void ButtonsEnabledDelegate(bool enabled);
        private delegate void PictureBoxOutcomeImageDelegate(Image image);
        private delegate void StopWatchTimerLabelTextDelegate(object sender, EventArgs e);
        private delegate void LabelStopWatchTimerTextDelegate(string text);
        #endregion Delegates
    }
}