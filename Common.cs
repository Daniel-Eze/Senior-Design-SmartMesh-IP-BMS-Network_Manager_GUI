using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Network_Manager_GUI
{
    #region Enums
    public enum EnumDeviceType { NA, NETWORK_MANAGER, MOTE, ACCESS_POINT };
    public enum EnumTaskName
    {
        NA, CONNECTION, RESTART, FACTORYRESET, MODE, AUTOJOIN, RADIOTEST, FIRMWARELOAD,
        SET_NETWORKID, GET_NETWORKID, SET_JOINKEY, GET_MOTESNUMBER, GET_NETWORKSTATISTICS
    };
    public enum EnumTaskStatus { NA, STARTED, CANCELED, COMPLETED };
    public enum EnumTaskResult { NA, SUCCESSFUL, UNSUCCESSFUL };
    #endregion Enums

    public class Common
    {
        #region Variables/Instances Declaration and Initialization
        #region Network Setup
        /// <summary>
        /// Variable used to define the maximum number of motes to
        /// interact within the SmartMesh-IP network.
        /// </summary>
        public static readonly int motesMaxNumber = 10;
        #endregion Network Setup

        #region Timer
        public static System.Timers.Timer taskTimer = null;
        public static int stopWatchTimerElapsedHour = -1;
        public static int stopWatchTimerElapsedMinute = -1;
        public static int stopWatchTimerElapsedSecond = -1;
        #endregion Timer

        #region Forms
        public static Network_Manager_GUI mainForm = null;
        public static FrmPortSettings portSettingsForm = null;
        public static LoadFirmware loadFirmwareForm = null;
        public static TemperatureLogger temperatureLoggerForm = null;
        public static Oscilloscope oscilloscopeForm = null;
        public static TemperaturePlotter temperaturePlotterForm = null;
        public static NetworkStatistics networkStatisticsForm = null;
        #endregion Forms

        #region Controls
        public static ComboBox comboBox_Port = null;
        public static string ComboBoxModeMoteText = string.Empty;
        /// <summary>
        /// The initial value is -1; OFF = 0; ON = 1.
        /// </summary>
        public static int ComboBoxAutoJoinMoteValue = -1;
        public static string ComboBoxRadiotestNetworkManagerText = string.Empty;
        public static string ComboBoxRadiotestMoteText = string.Empty;
        public static string NetworkIdNetworkManagerText = string.Empty;
        public static string NetworkIdMoteText = string.Empty;
        public static string JoinKeyNetworkManagerText = string.Empty;
        public static string JoinKeyMoteText = string.Empty;
        public static int Output_ScreenLengthThreshold = 500000;
        #endregion Controls

        #region Logs
        public static string immediateSerialOutputLog = string.Empty;
        public static string taskSerialOutputLog = string.Empty;
        #endregion Logs

        #region Devices
        #region Network Manager
        #region Network Manager Statistics
        /// <summary>
        /// Struct used to store network manager statistics.
        /// </summary>
        public struct NetworkManagerStatisticsStruct
        {
            /* established connections: 1
               dropped connections    : 0
               transmit OK            : 0
               transmit error         : 0
               transmit repeat        : 0
               receive  OK            : 330
               receive  error         : 0
               acknowledge delay avrg : 0 msec
               acknowledge delay max  : 0 msec
            */
            public int establishedConnections;
            public int droppedConnections;
            public int transmitOK;
            public int transmitError;
            public int transmitRepeat;
            public int receiveOK;
            public int receiveError;
            public string acknowledgeDelayAvrg;
            public string acknowledgeDelayMax;
        };
        /// <summary>
        /// Variable to contain the statistics of the network manager in the network.
        /// </summary>
        public static NetworkManagerStatisticsStruct networkManagerStatistics = new NetworkManagerStatisticsStruct();
        /// <summary>
        /// Variable containing a string identification of network manager statistics.
        /// </summary>
        public static readonly string networkManagerStatisticsIdentifier = "Manager Statistics --------------------------------";
        /// <summary>
        /// Array containing all the network manager statistics strings.
        /// </summary>
        public static readonly string[] networkManagerStatisticsStrings = new string[] { "establishedconnections:",
        "droppedconnections", "transmitOK", "transmiterror", "transmitrepeat", "receiveOK", "receiveerror", "acknowledgedelayavrg",
            "acknowledgedelaymax" };
        #endregion Network Manager Statistics

        #region Network Statistics
        /// <summary>
        /// Struct used to store network statistics.
        /// </summary>
        public struct NetworkStatisticsStruct
        {
            /* Network Statistics --------------------------------
               reliability:    0% (Arrived/Lost:   0/0)
               stability:      0% (Transmit/Fails: 0/0)
               latency:        0 msec
            */
            public string reliability;
            /// <summary>
            /// Its unit is %.
            /// </summary>
            public double reliabilityNumericalValue;
            public string stability;
            /// <summary>
            /// Its unit is %.
            /// </summary>
            public double stabilityNumericalValue;
            public string latency;
            /// <summary>
            /// Its default unit is msec.
            /// </summary>
            public double latencyNumericalValue;
        };
        /// <summary>
        /// Variable to contain the network statistics.
        /// </summary>
        public static NetworkStatisticsStruct networkStatistics = new NetworkStatisticsStruct();
        /// <summary>
        /// Variable containing a string identification of network statistics.
        /// </summary>
        public static readonly string networkStatisticsIdentifier = "Network Statistics --------------------------------";
        /// <summary>
        /// Array containing all the network statistics strings.
        /// </summary>
        public static readonly string[] networkStatisticsStrings = new string[] { "reliability:", "stability:", "latency:" };
        /// <summary>
        /// Variable holding the latency unit.
        /// </summary>
        public static readonly string latencyUnit = "msec";
        #endregion Network Statistics

        #region Packet Statistics
        /// <summary>
        /// Struct used to store Packet statistics.
        /// </summary>
        public struct PacketStatisticsStruct
        {
            /* 
            returnFields:     {'macAddress': [0, 23, 13, 0, 0, 90, 92, 36], 
            'srcPort': 61624, 'utcUsecs': 465500, 'utcSecs': 1025674831L, 
            'dstPort': 61624, 'data': [0, 9, 174]}
            */
            public string macAddress;
            public int srcPort;
            public int utcUsecs;
            public string utcSecs;
            public int dstPort;
            public string dataString;
            public int[] data;
        };
        /// <summary>
        /// Array to contain the statistics of packets received by the network manager.
        /// </summary>
        public static PacketStatisticsStruct[] packetStatistics = new PacketStatisticsStruct[motesMaxNumber + 1];
        /// <summary>
        /// Array containing string fields of packet statistics.
        /// </summary>
        public static readonly string[] packetFieldNames = new string[] { "'macAddress'", ", 'srcPort'",
            ", 'utcUsecs'", ", 'utcSecs'", ", 'dstPort'", ", 'data'" };
        #endregion Packet Statistics
        #endregion Network Manager

        #region Mote
        #region Mote Parameters
        /// <summary>
        /// Variable used to define the number of found 
        /// motes in the SmartMesh-IP network.
        /// </summary>
        public static int numberOfFoundMotes = 0;
        /// <summary>
        /// Variable used to define the number of operational 
        /// motes in the SmartMesh-IP network.
        /// </summary>
        public static int numberOfOperationalMotes = 0;
        /// <summary>
        /// Variable used to define the number of lost 
        /// motes in the SmartMesh-IP network.
        /// </summary>
        public static int numberOfLostMotes = 0;
        /// <summary>
        /// Variable used to define the number of connecting 
        /// motes in the SmartMesh-IP network.
        /// </summary>
        public static int numberOfConnectingMotes = 0;
        /// <summary>
        /// Variable used to define the number of a mote in relation to
        /// other motes interacting within the SmartMesh-IP network.
        /// </summary>
        public static int moteNumber = 0;
        /// <summary>
        /// Struct used to store mote parameters.
        /// </summary>
        public struct MoteStruct
        {
            /* The following parameters are derived from executing the 'sm' command through
             * serial port on the network manager:
             * MAC: EUI-64 of the mote
               MoteID: short address assigned to this mote by the manager. MoteID 1 is always the AP.
               State: Current state of each mote (Negot, Conn, Oper, Lost)
               Nbrs: Number of neighbors with which this mote has active links.
               Links: Total number of links, compressed and normal.
               Joins: Shows how many times the mote has advanced to the Operational state.
               Age: Seconds since the most recent packet was received by the manager from this mote.
               StateTime: Time (d-hh:mm:ss) since the mote was advanced to its current state. When a mote is Operational,
               StateTime shows how long the mote has been in the network
            */
            public string mac;
            public int moteID;
            public string state;
            public int nbrs;
            public int links;
            public int joins;
            public int age;
            public string stateTime;
            public double celsiusTemperature;
            public double fahrenheitTemperature;
        };
        /// <summary>
        /// Array to contain the parameters of all motes in the network.
        /// </summary>
        public static MoteStruct[] mote = new MoteStruct[motesMaxNumber + 1];
        /// <summary>
        /// Variable containing a string identification of a mote's Mac Address.
        /// </summary>
        public static readonly string moteMacAddressIdentifier = "00-17-0D";
        /// <summary>
        /// Array containing the possible mote' states (Negot, Conn, Oper, Lost).
        /// </summary>
        public static readonly string[] moteStates = new string[] { "Negot", "Conn", "Oper", "Lost" };
        #endregion Mote Parameters

        #region Mote Statistics
        /// <summary>
        /// Struct used to store mote statistics.
        /// </summary>
        public struct MoteStatisticsStruct
        {
            /* 
            Motes Statistics -----------------------------------
            Mote    Received   Lost  Reliability Latency Hops
            */
            public int moteID;
            public int received;
            public int lost;
            public string reliability;
            public int latency;
            public double hops;
        };
        /// <summary>
        /// Array to contain the statistics data of all motes in the network.
        /// </summary>
        public static MoteStatisticsStruct[] moteStatistics = new MoteStatisticsStruct[motesMaxNumber + 1];
        /// <summary>
        /// Array containing string identifications of mote statistics.
        /// </summary>
        public static readonly string[] moteStatisticsIdentifierArray = new string[] { "Motes Statistics -----------------------------------",
            "Mote    Received   Lost  Reliability Latency Hops", "#" };
        #endregion Mote Statistics
        #endregion Mote

        /// <summary>
        /// Variable used to set the default or current device.
        /// </summary>
        public static EnumDeviceType deviceType = EnumDeviceType.NETWORK_MANAGER;
        #endregion Devices

        #region Current Network Manager's and Mote's Serial Port Settings
        public static string CurrentNetworkManagerSerialPortName = string.Empty;
        public static string CurrentNetworkManagerApiPortName
        {
            get
            {
                //get the integer part of the serial port name
                int.TryParse(CurrentNetworkManagerSerialPortName.ToUpper().Replace("COM", ""), out int apiPortNumber);
                //increment the integer part of the serial port name by 1
                apiPortNumber += 1;
                //return the full Network Manager Api Port Name
                return "COM" + apiPortNumber.ToString();
            }
        }
        public static string CurrentMoteSerialPortName = string.Empty;
        #endregion Current Network Manager's and Mote's Serial Port Settings

        #region Task Settings
        public static EnumTaskName taskName = EnumTaskName.NA;
        public static EnumTaskStatus taskStatus = EnumTaskStatus.NA;
        public static EnumTaskResult taskResult = EnumTaskResult.NA;
        #endregion Task Settings

        #region Task Outcome
        /// <summary>
        /// Variable used to validate the first step of multi-step tasks.
        /// </summary>
        public static bool isFirstPartSuccessful = false;
        /// <summary>
        /// Variable used to validate the second step of some multi-step tasks.
        /// </summary>
        public static bool isSecondPartSuccessful = false;
        /// <summary>
        /// Variable used to validate the third step of some multi-step tasks.
        /// </summary>
        public static bool isThirdPartSuccessful = false;
        #endregion Task Outcome

        #region Timeouts
        /// <summary>
        /// serialPortThreadTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for a write-to, open,
        /// or close serial port thread to terminate.
        /// </summary>
        public static readonly int serialPortThreadTimeout = 700;
        /// <summary>
        /// serialPortTaskReadTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the serial port output
        /// of a task to be read.
        /// </summary>
        public static int serialPortTaskReadTimeout = 100;
        /// <summary>
        /// taskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for a specific task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// This variable holds the timeout specified for the task to be executed.
        /// </summary>
        public static int taskTimeout = 0;
        /// <summary>
        /// commandLineProcessTimeout (millisecondsTimeout):
        /// Variable used to instruct the System.Diagnostics.Process component to wait the specified number
        /// of milliseconds for the associated process to exit.
        /// </summary>
        public static int commandLineProcessTimeout = 15000;
        #endregion Timeouts

        #region Serial Port
        /// <summary>
        /// This is the maximum number of loop iterations used to get all available Serial Port bytes,
        /// resulting from command execution, in one attempt.
        /// </summary>
        public static int serialPortReadLoopCounter = 10;

        /// <summary>
        /// This is the actual calculated data rate when running applications.
        /// </summary>
        public static double applicationsDataRate = 0;
        #endregion Serial Port

        #region CommandLine
        public static CommandLineExecute commandLine = null;
        #endregion CommandLine

        #region ESP CommandLine
        public static string espFilesDirectoryPath = @"Firmware Files\";
        public static string espApplicationName = "ESP.exe";
        public static string[] undesiredEspCommandLineResponse = new string[] { "HSP_ERR_OPENDEV", "HSP_ERR_NODEV",
            "HSP_ERR_LOADLIB", "HSP_ERR_FLASHID", "HSP_ERR_MAXNUMDEV", "HSP_ERR_BADTYPE", "HSP_ERR_NOCOM", "HSP_ERR_NOTLOCKED",
            "HSP_ERR_NOTUNLOCKED", "HSP_ERR_BADPARAM" };
        public static string[] standardEspCommandLineResponse = new string[] { "Open ok, flash emulation mode enabled,",
            "ESP exiting: tries remaining = 0, err = 0" };
        #endregion ESP CommandLine

        #region SmartMesh-SDK CommandLine
        public static string applicationFilesDirectoryPath = @"C:\Python27\SmartMeshSDK-1.3.0.1-win\win\";
        public static string[] undesiredApplicationCommandLineResponse = new string[] { "Error" };
        #region TempLogger
        public static string tempLoggerProcessName = "TempLogger";
        public static string tempLoggerApplicationProcessName = applicationFilesDirectoryPath + tempLoggerProcessName + ".exe";
        public static string temperatureLoggerArguments = "";
        public static string[] desiredTemperatureLoggerCommandLineResponse = new string[] { "TempLogger - (c) Dust Networks",
        "SmartMesh IP manager's API serial port", "Connected to" };
        public static string celsiusTemperatureDelimiter1 = "t=";
        public static string celsiusTemperatureDelimiter2 = "C at ";
        #endregion TempLogger

        #region PublishToWeb
        public static string publishToWebProcessName = "PublishToWeb";
        public static string publishToWebApplicationProcessName = applicationFilesDirectoryPath + publishToWebProcessName + ".exe";
        public static string publishToWebArguments = "";
        public static string[] desiredPublishToWebCommandLineResponse = new string[] { "PublishToWeb - (c) Dust Networks",
        "SmartMesh IP manager's API serial port", "Connected to" };
        #endregion PublishToWeb

        #region SeeTheMesh
        public static string seeTheMeshUrl = "http://127.0.0.1:8081";
        public static string seeTheMeshProcessName = "SeeTheMesh";
        public static string seeTheMeshApplicationProcessName = seeTheMeshProcessName + ".exe";
        public static string seeTheMeshArguments = "";
        public static string[] desiredSeeTheMeshCommandLineResponse = new string[] { "Web interface started at http://127.0.0.1:8081" };
        #endregion SeeTheMesh
        #endregion SmartMesh-SDK CommandLine

        #region Programs
        /// <summary>
        /// Array containing all the programs to be closed at startup.
        /// </summary>
        public static readonly string[] programsToCloseAtStartup = new string[] { "python", "ttermpro", "APIExplorer",
            "SensorDataReceiver", "TempMonitor", tempLoggerProcessName, publishToWebProcessName, seeTheMeshProcessName };
        #endregion Programs
        #endregion Variables/Instances Declaration and Initialization

        #region GUI Forms Access
        /// <summary>
        /// Function used to generate a new instance of 'Network_Manager_GUI'.
        /// </summary>
        private static void Network_Manager_GUI_NewInstance()
        {
            //Generate a new instance of 'Network_Manager_GUI'
            if (mainForm == null || mainForm.IsDisposed == true)
            {
                mainForm = new Network_Manager_GUI();
            }
        }

        /// <summary>
        /// Function used to generate a new instance of 'FrmPortSettings'.
        /// </summary>
        public static void FrmPortSettings_NewInstance()
        {
            //Generate a new instance of 'FrmPortSettings'
            if (portSettingsForm == null || portSettingsForm.IsDisposed == true)
            {
                portSettingsForm = new FrmPortSettings();
            }
        }

        /// <summary>
        /// Function used to generate a new instance of 'LoadFirmware'.
        /// </summary>
        public static void LoadFirmware_NewInstance()
        {
            //Generate a new instance of 'LoadFirmware'
            if (loadFirmwareForm == null || loadFirmwareForm.IsDisposed == true)
            {
                loadFirmwareForm = new LoadFirmware();
            }
        }

        /// <summary>
        /// Function used to generate a new instance of 'NetworkStatistics'.
        /// </summary>
        public static void NetworkStatistics_NewInstance()
        {
            //Generate a new instance of 'NetworkStatistics'
            if (networkStatisticsForm == null || networkStatisticsForm.IsDisposed == true)
            {
                networkStatisticsForm = new NetworkStatistics();
            }
        }

        /// <summary>
        /// Function used to generate a new instance of 'TemperatureLogger'.
        /// </summary>
        public static void TemperatureLogger_NewInstance()
        {
            //Generate a new instance of 'TemperatureLogger'
            if (temperatureLoggerForm == null || temperatureLoggerForm.IsDisposed == true)
            {
                temperatureLoggerForm = new TemperatureLogger();
            }
        }

        /// <summary>
        /// Function used to generate a new instance of 'TemperaturePlotter'.
        /// </summary>
        public static void TemperaturePlotter_NewInstance()
        {
            //Generate a new instance of 'TemperaturePlotter'
            if (temperaturePlotterForm == null || temperaturePlotterForm.IsDisposed == true)
            {
                temperaturePlotterForm = new TemperaturePlotter();
            }
        }

        /// <summary>
        /// Function used to generate a new instance of 'Oscilloscope'.
        /// </summary>
        public static void Oscilloscope_NewInstance()
        {
            //Generate a new instance of 'Oscilloscope'
            if (oscilloscopeForm == null || oscilloscopeForm.IsDisposed == true)
            {
                oscilloscopeForm = new Oscilloscope();
            }
        }

        /// <summary>
        /// Function used to find opened form 'Network_Manager_GUI'.
        /// </summary>
        private static void Find_Network_Manager_GUI_OpenedForm()
        {
            //Generate a new instance of 'Network_Manager_GUI'
            Network_Manager_GUI_NewInstance();
            //Assign the generated instance to the related opened form
            foreach (object relatedForm in Application.OpenForms)
            {
                if (relatedForm.GetType() == mainForm.GetType()) //same-type check
                {
                    mainForm = (Network_Manager_GUI)relatedForm; //grab the opened 'Network_Manager_GUI' form
                    break; //exit loop
                }
                Application.DoEvents(); //refresh GUI
            }
        }

        /// <summary>
        /// Function used to find opened form 'FrmPortSettings'.
        /// </summary>
        private static void Find_FrmPortSettings_OpenedForm()
        {
            //Generate a new instance of 'FrmPortSettings'
            FrmPortSettings_NewInstance();
            //Assign the generated instance to the related opened form
            foreach (object relatedForm in Application.OpenForms)
            {
                if (relatedForm.GetType() == portSettingsForm.GetType()) //same-type check
                {
                    portSettingsForm = (FrmPortSettings)relatedForm; //grab the opened 'FrmPortSettings' form
                    break; //exit loop
                }
                Application.DoEvents(); //refresh GUI
            }
        }

        /// <summary>
        /// Function used to find opened form 'LoadFirmware'.
        /// </summary>
        private static void Find_LoadFirmware_OpenedForm()
        {
            //Generate a new instance of 'LoadFirmware'
            LoadFirmware_NewInstance();
            //Assign the generated instance to the related opened form
            foreach (object relatedForm in Application.OpenForms)
            {
                if (relatedForm.GetType() == loadFirmwareForm.GetType()) //same-type check
                {
                    loadFirmwareForm = (LoadFirmware)relatedForm; //grab the opened 'LoadFirmware' form
                    break; //exit loop
                }
                Application.DoEvents(); //refresh GUI
            }
        }

        /// <summary>
        /// Function used to control the Enabled feature of the opened form 'Network_Manager_GUI'.
        /// </summary>
        /// <param name="enabled"></param>
        public static void Network_Manager_GUI_Enabled(bool enabled)
        {
            //Find opened form 'Network_Manager_GUI'
            Find_Network_Manager_GUI_OpenedForm();
            //Control the Enabled feature of the 'Network_Manager_GUI' form
            mainForm.Enabled = enabled;
        }
        #endregion GUI Forms Access

        #region Serial Port Functions
        /// <summary>
        /// Function used to get all available serial ports.
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllAvailableSerialPorts()
        {
            // Get All Available Serial Ports
            return System.IO.Ports.SerialPort.GetPortNames();
        }

        /// <summary>
        /// Function used to populate the COM Port Box.
        /// </summary>
        /// <param name="whichFormToControl"></param>
        public static void PopulateComPortBox(string whichFormToControl)
        {
            //Check which form to control and grab the related COM Port ComboBox
            if (whichFormToControl == "Network_Manager_GUI")
            {
                //Grab the COM Port ComboBox from 'Network_Manager_GUI'
                Find_Network_Manager_GUI_OpenedForm();
                comboBox_Port = GetProperSerialPortComboBox();
            }
            else if (whichFormToControl == "FrmPortSettings")
            {
                //Grab the combo_Port ComboBox from 'FrmPortSettings'
                Find_FrmPortSettings_OpenedForm();
                comboBox_Port = portSettingsForm.combo_PortName;
            }
            //Store current selected item
            string currentCOMPortBoxSelectedItem = string.Empty;
            if (comboBox_Port.SelectedItem != null)
            {
                currentCOMPortBoxSelectedItem = comboBox_Port.SelectedItem.ToString();
            }
            //Clear COM Port Box items
            comboBox_Port.Items.Clear();
            //Get Available Serial Ports and Populate COM port comboBox
            foreach (string availableSerialPort in Common.GetAllAvailableSerialPorts())
            {
                comboBox_Port.Items.Add(availableSerialPort); //fill the available port name in the combo box options
                //Re-Assign current selected item if it is still available
                if (currentCOMPortBoxSelectedItem == availableSerialPort)
                {
                    comboBox_Port.SelectedItem = currentCOMPortBoxSelectedItem;
                }
                Application.DoEvents(); //refresh GUI
            }
        }

        /// <summary>
        /// Function used to get the proper Serial/COM Port comboBox in Network Manager GUI.
        /// </summary>
        /// <returns></returns>
        public static ComboBox GetProperSerialPortComboBox()
        {
            //Grab the COM Port from 'Network_Manager_GUI' based on device type
            Find_Network_Manager_GUI_OpenedForm();
            if (deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                return mainForm.combo_PortNetworkManager;
            }
            else if (deviceType == EnumDeviceType.MOTE)
            {
                return mainForm.combo_PortMote;
            }
            return null;
        }

        /// <summary>
        /// Function used to store the last used Network Manager's or Mote's Serial Port name.
        /// </summary>
        /// <param name="serialPortName"></param>
        public static void StoreLastUsedSerialPortName(string serialPortName)
        {
            //Store the current Network Manager's or Mote's Serial Port name
            if (deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                CurrentNetworkManagerSerialPortName = serialPortName;
            }
            else if (deviceType == EnumDeviceType.MOTE)
            {
                CurrentMoteSerialPortName = serialPortName;
            }
        }
        #endregion Serial Port Functions

        #region Task Related Functions
        /// <summary>
        /// Function used to update Task Settings.
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="taskStatus"></param>
        /// <param name="taskResult"></param>
        /// <param name="deviceType"></param>
        /// <param name="taskTimeout"></param>
        public static void UpdateTaskSettings(EnumTaskName taskName, EnumTaskStatus taskStatus, EnumTaskResult taskResult, EnumDeviceType deviceType, int taskTimeout)
        {
            //Find opened form 'Network_Manager_GUI'
            Find_Network_Manager_GUI_OpenedForm();
            //Control the Enabled feature of the 'Network_Manager_GUI' form
            mainForm.UpdateTaskSettings(taskName, taskStatus, taskResult, deviceType, taskTimeout);
        }

        /// <summary>
        /// Function used to look for a string in a string.
        /// </summary>
        /// <param name="desiredString"></param>
        /// <param name="undesiredStringArray"></param>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static bool LookForString(string desiredString, string[] undesiredStringArray, string stringToBeSearched)
        {
            //Remove some characters from original stringToBeSearched
            stringToBeSearched = stringToBeSearched.Replace(" ", "").Replace(">", "").Replace("\r", "").Replace("\n", "");
            #region Desired Strings
            //Remove some characters from original desiredString
            string desiredStringToLookFor = desiredString.Replace(" ", "").Replace(">", "").Replace("\r", "").Replace("\n", "");
            //Check if the desired string being looked for is found in the string to be searched
            if (stringToBeSearched.Contains(desiredStringToLookFor) == false)
            {
                return false; //the desired string being looked for is not found in the string to be searched
            }

            #endregion Desired Strings
            #region Undesired Strings
            //Check if undesired strings appear in stringToBeSearched
            foreach (string undesiredStringToLookFor in undesiredStringArray) //grab a string from the undesiredStringArray array
            {
                //Remove some characters from original stringToLookFor
                string updatedUndesiredStringToLookFor = undesiredStringToLookFor.Replace(" ", "").Replace(">", "").Replace("\r", "").Replace("\n", "");
                //Check if the undesired string being looked for is found in the string to be searched
                if (stringToBeSearched.Contains(updatedUndesiredStringToLookFor) == true)
                {
                    return false; //the undesired string being looked for is found in the string to be searched
                }
                Application.DoEvents(); //refresh GUI
            }
            #endregion Undesired Strings
            return true; //the desired string was found in the string to be searched, and the undesired strings were not found
        }

        /// <summary>
        /// Function used to look for a string or set of strings in a string.
        /// </summary>
        /// <param name="desiredStringArray"></param>
        /// <param name="undesiredStringArray"></param>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static bool LookForStringArray(string[] desiredStringArray, string[] undesiredStringArray, string stringToBeSearched)
        {
            //Remove some characters from original stringToBeSearched
            stringToBeSearched = stringToBeSearched.Replace(" ", "").Replace(">", "").Replace("\r", "").Replace("\n", "");
            #region Desired Strings
            //Check if desired strings appear in stringToBeSearched
            foreach (string desiredStringToLookFor in desiredStringArray) //grab a string from the desiredStringArray array
            {
                //Remove some characters from original desiredStringToLookFor
                string updatedDesiredStringToLookFor = desiredStringToLookFor.Replace(" ", "").Replace(">", "").Replace("\r", "").Replace("\n", "");
                //Check if the desired string being looked for is found in the string to be searched
                if (stringToBeSearched.Contains(updatedDesiredStringToLookFor) == false)
                {
                    return false; //the desired string being looked for is not found in the string to be searched
                }
                Application.DoEvents(); //refresh GUI
            }
            #endregion Desired Strings
            #region Undesired Strings
            //Check if undesired strings appear in stringToBeSearched
            foreach (string undesiredStringToLookFor in undesiredStringArray) //grab a string from the undesiredStringArray array
            {
                //Remove some characters from original undesiredStringToLookFor
                string updatedUndesiredStringToLookFor = undesiredStringToLookFor.Replace(" ", "").Replace(">", "").Replace("\r", "").Replace("\n", "");
                //Check if the undesired string being looked for is found in the string to be searched
                if (stringToBeSearched.Contains(updatedUndesiredStringToLookFor) == true)
                {
                    return false; //the undesired string being looked for is found in the string to be searched
                }
                Application.DoEvents(); //refresh GUI
            }
            #endregion Undesired Strings
            return true; //all desired strings were found in the string to be searched, and undesired strings were not found
        }

        /// <summary>
        /// Function used to add a new string item to a string array.
        /// </summary>
        /// <param name="itemTobeAdded"></param>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string[] AddItemToStringArray(string itemTobeAdded, string[] stringArray)
        {
            //Declare list
            List<string> list = new List<string>();
            //Convert original string array to list
            foreach (string stringItem in stringArray)
            {
                list.Add(stringItem); //add retrieved string to list
                Application.DoEvents(); //refresh GUI
            }
            //add new item to the list
            list.Add(itemTobeAdded);
            //return original string array with new item added
            return list.ToArray();
        }

        /// <summary>
        /// Function used to get the string in-between two strings.
        /// </summary>
        /// <param name="fromString"></param>
        /// <param name="toString"></param>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static string GetStringBetweenTwoStrings(string fromString, string toString, string stringToBeSearched)
        {
            if (!string.IsNullOrEmpty(stringToBeSearched)) //check if stringToBeSearched is empty or not
            {
                try
                {
                    //Remove some characters from original stringToBeSearched
                    stringToBeSearched = stringToBeSearched.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    //Remove some characters from original fromString
                    fromString = fromString.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    //Remove some characters from original toString
                    toString = toString.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    //Get index of beginning of the "from" string
                    int fromStringIndex = stringToBeSearched.IndexOf(fromString);
                    //Remove text before "from" string
                    stringToBeSearched = stringToBeSearched.Substring(fromStringIndex);
                    //Get index of end of the "from" string
                    fromStringIndex = stringToBeSearched.IndexOf(fromString) + fromString.Length;
                    //Get index of beginning of the "to" string
                    int toStringIndex = stringToBeSearched.IndexOf(toString);
                    //Return the string in-between the "from" and "to" strings
                    return stringToBeSearched.Substring(fromStringIndex, toStringIndex - fromStringIndex);
                }
                catch
                {
                    return string.Empty; //return empty
                }
            }
            else //meaning that stringToBeSearched is empty
            {
                return string.Empty; //return empty
            }
        }

        /// <summary>
        /// Function used to get the string in-between two strings.
        /// </summary>
        /// <param name="fromString"></param>
        /// <param name="toString"></param>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static string GetStringBetweenTwoStringsLastIndex(string fromString, string toString, int searchStartIndexToSubtract, string stringToBeSearched)
        {
            if (!string.IsNullOrEmpty(stringToBeSearched)) //check if stringToBeSearched is empty or not
            {
                try
                {
                    //Remove some characters from original stringToBeSearched
                    stringToBeSearched = stringToBeSearched.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    //Remove some characters from original fromString
                    fromString = fromString.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    //Remove some characters from original toString
                    toString = toString.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                    //Get last index of the "to" string
                    int toStringLastIndex = stringToBeSearched.LastIndexOf(toString);
                    //Set search start index
                    int searchStartIndex = toStringLastIndex - searchStartIndexToSubtract;
                    //Remove text before "from" string
                    stringToBeSearched = stringToBeSearched.Substring(searchStartIndex);
                    //Get index of end of the "from" string
                    searchStartIndex = stringToBeSearched.IndexOf(fromString) + fromString.Length;
                    //Get last index of the "to" string
                    int toStringIndex = stringToBeSearched.IndexOf(toString);
                    //Return the string in-between the "from" and "to" strings
                    return stringToBeSearched.Substring(searchStartIndex, toStringIndex - searchStartIndex);
                }
                catch
                {
                    return string.Empty; //return empty
                }
            }
            else //meaning that stringToBeSearched is empty
            {
                return string.Empty; //return empty
            }
        }

        /// <summary>
        /// Function used to remove a set of strings from a string.
        /// </summary>
        /// <param name="undesiredStringArray"></param>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static string RemoveStringArray(string[] undesiredStringArray, string stringToBeSearched)
        {
            //Store stringToBeSearched into outputString
            string outputString = stringToBeSearched;
            //Check if undesired strings appear in stringToBeSearched
            foreach (string undesiredStringToLookFor in undesiredStringArray) //grab a string from the undesiredStringArray array
            {
                //Remove the undesired string
                outputString = outputString.Replace(undesiredStringToLookFor, "");
                Application.DoEvents(); //refresh GUI
            }
            return outputString; //return the resulting string
        }

        /// <summary>
        /// Function used to count the number of occurrences of a word in a string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int CountOccurences(string word, string stringToBeSearched)
        {
            //split the string by spaces 
            string[] a = stringToBeSearched.Split(' ');

            //search for pattern in string 
            int count = 0;
            for (int i = 0; i < a.Length; i++)
            {
                // if match found increase count 
                if (word.Equals(a[i]))
                    count++;
            }
            //return the number of occurrences
            return count;
        }

        /// <summary>
        /// Function used to check if there are only Hex characters in a string.
        /// </summary>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static bool OnlyHexInString(string stringToBeSearched)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(stringToBeSearched, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        /// <summary>
        /// Function used to determine if all characters in a string are digits only.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static bool AreDigitsOnly(string inputString)
        {
            if (!(string.IsNullOrEmpty(inputString))) //check if inputString is empty
            {
                foreach (char character in inputString)
                {
                    if (character < '0' || character > '9')
                    {
                        return false; //a non-digit character has been detected
                    }
                }
                return true; //all characters in inputString are digits
            }
            else //meaning that inputString is empty
            {
                return false; //since inputString is empty
            }
        }

        /// <summary>
        /// Function used to update on which device is/isn't connected to the GUI.
        /// The CONNECTION task outcome reflects whether the Network Manager or Mote
        /// is/isn't connected to the GUI.
        /// </summary>
        /// <param name="isConnectedToGUI"></param>
        public static void UpdateOnDeviceConnection(bool isConnectedToGUI)
        {
            if (deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                //update on whether GUI is or isn't connected to the Network Manager
                NetworkManager.isConnectedToGUI = isConnectedToGUI;
            }
            else if (deviceType == EnumDeviceType.MOTE)
            {
                //update on whether GUI is or isn't connected to the Mote
                Mote.isConnectedToGUI = isConnectedToGUI;
            }
        }

        /// <summary>
        /// Function used to validate a given Mac Address.
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        public static bool ValidateMacAddress(string macAddress)
        {
            //defining what a valid Mac Address should look like
            Regex r = new Regex("^(?:[0-9a-fA-F]{2}:){5}[0-9a-fA-F]{2}|" +
                                 "(?:[0-9a-fA-F]{2}-){5}[0-9a-fA-F]{2}|" +
                                 "(?:[0-9a-fA-F]{2}){5}[0-9a-fA-F]{2}$");
            //checking if the given Mac Address is valid
            if (r.IsMatch(macAddress))
            {
                //valid Mac Address
                return true;
            }
            //invalid Mac Address
            return false;
        }
        #endregion Task Related Functions

        #region ESP CommandLine Related Functions
        /// <summary>
        /// Function used to find all Smartmesh IP devices plugged-in to the computer.
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="desiredStringArray"></param>
        /// <param name="undesiredStringArray"></param>
        /// <param name="waitForTime"></param>
        /// <returns></returns>
        public static bool LookForSmartmeshIpDevices(EnumDeviceType deviceType, string[] desiredStringArray, string[] undesiredStringArray, int waitForTime)
        {
            //Append text in output_screen
            loadFirmwareForm.Output_ScreenText("\r\nLooking for the " + deviceType + "...\r\n");
            //Access the opened 'LoadFirmware' form
            Find_LoadFirmware_OpenedForm();
            //Select the ESP.exe program to be run in Windows cmd
            commandLine = new CommandLineExecute(espFilesDirectoryPath + espApplicationName, waitForTime);
            //Check if the task is supposed to be running
            if (!taskTimer.Enabled)
            {
                //Exit function since the task is not supposed to be running
                return false;
            }
            //Send command to show SmartMesh IP plugged-in devices
            commandLine.Start(" -L", espFilesDirectoryPath);
            //Check if the task is supposed to be running
            if (!taskTimer.Enabled)
            {
                //Exit function since the task is not supposed to be running
                return false;
            }
            //Append text in output_screen
            loadFirmwareForm.Output_ScreenText("\r\n" + commandLine.OutputString + "\r\n");
            //Check if the device is plugged in to PC
            if (LookForStringArray(desiredStringArray, undesiredStringArray, commandLine.OutputString) == false)
            {
                //Special Mote LookUp error
                if (deviceType == EnumDeviceType.MOTE)
                {
                    //check if the HART programmer board (DC9007A-04) has been detected.
                    if (LookForString(Mote.hartMoteID, undesiredStringArray, commandLine.OutputString) == true)
                    {
                        //Display message box
                        MessageBox.Show("The HART programmer board (DC9007A-04) has been detected." +
                            "\r\nPlease use the SmartMesh IP programmer board (DC9021B-05) instead." +
                            "\r\nDisconnect the HART programmer board (DC9007A-04) from the computer and mote, " +
                            "connect the SmartMesh IP programmer board (DC9021B-05) to the computer and mote, " +
                            "and try again.", "Load Firmware GUI");
                        //Exit function
                        return false;
                    }
                }
                //Display message box
                MessageBox.Show("The " + deviceType + " cannot be found! Please make sure the " + deviceType + " is properly plugged " +
            "in to the computer, and it is the only SmartMesh IP device plugged in. Then try again!", "Load Firmware GUI");
                //Append text in output_screen
                loadFirmwareForm.Output_ScreenText("\r\nThe " + deviceType + " cannot be found! Please make sure the " + deviceType + " is properly plugged " +
                    "in to the computer, and it is the only SmartMesh IP device plugged in. Then try again!\r\n");
                //Exit function
                return false;
            }
            else
            {
                //Append text in output_screen
                loadFirmwareForm.Output_ScreenText("\r\nThe " + deviceType + " has been found!\r\n");
                //Exit function
                return true;
            }
        }

        /// <summary>
        /// Function used to erase old firmware.
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="desiredStringArray"></param>
        /// <param name="undesiredStringArray"></param>
        /// <param name="waitForTime"></param>
        /// <returns></returns>
        public static bool EraseOldFirmware(EnumDeviceType deviceType, string[] desiredStringArray, string[] undesiredStringArray, int waitForTime)
        {
            //Append text in output_screen
            loadFirmwareForm.Output_ScreenText("\r\nAttempting to erase the " + deviceType + "'s old firmware...\r\n");
            //Access the opened 'LoadFirmware' form
            Find_LoadFirmware_OpenedForm();
            //Select the ESP.exe program to be run in Windows cmd
            commandLine = new CommandLineExecute(espFilesDirectoryPath + espApplicationName, waitForTime);
            //Check if the task is supposed to be running
            if (!taskTimer.Enabled)
            {
                //Exit function since the task is not supposed to be running
                return false;
            }
            //Send command to erase old firmware
            commandLine.Start(" -E", espFilesDirectoryPath);
            //Check if the task is supposed to be running
            if (!taskTimer.Enabled)
            {
                //Exit function since the task is not supposed to be running
                return false;
            }
            //Append text in output_screen
            loadFirmwareForm.Output_ScreenText("\r\n" + commandLine.OutputString + "\r\n");
            //Check if the device's old firmware has been erased
            if (LookForStringArray(desiredStringArray, undesiredStringArray, commandLine.OutputString) == false)
            {
                //Display message box
                MessageBox.Show("The " + deviceType + "'s old firmware could not be erased! Please make sure the " + deviceType + " is properly plugged " +
                    "in to the computer, and it is the only SmartMesh IP device plugged in. Then try again!", "Load Firmware GUI");
                //Append text in output_screen
                loadFirmwareForm.Output_ScreenText("\r\nThe " + deviceType + "'s old firmware could not be erased! Please make sure the " + deviceType +
                    " is properly plugged " + "in to the computer, and it is the only SmartMesh IP device plugged in. Then try again!");
                //Exit function
                return false;
            }
            else
            {
                //Append text in output_screen
                loadFirmwareForm.Output_ScreenText("\r\nThe " + deviceType + "'s old firmware could be erased successfully!\r\n");
                //Exit function
                return true;
            }
        }

        /// <summary>
        /// Function used to load new firmware.
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="desiredStringArray"></param>
        /// <param name="undesiredStringArray"></param>
        /// <param name="waitForTime"></param>
        /// <param name="firmwareFile"></param>
        /// <returns></returns>
        public static bool LoadNewFirmware(EnumDeviceType deviceType, string[] desiredStringArray, string[] undesiredStringArray, int waitForTime, string firmwareFile)
        {
            //Append text in output_screen
            loadFirmwareForm.Output_ScreenText("\r\nAttempting to load the " + deviceType + "'s new firmware...\r\n");
            //Access the opened 'LoadFirmware' form
            Find_LoadFirmware_OpenedForm();
            //Select the ESP.exe program to be run in Windows cmd
            commandLine = new CommandLineExecute(espFilesDirectoryPath + espApplicationName, waitForTime);
            //Check if the task is supposed to be running
            if (!taskTimer.Enabled)
            {
                //Exit function since the task is not supposed to be running
                return false;
            }
            //Send command to load new firmware
            commandLine.Start(" -P " + firmwareFile + " 0", espFilesDirectoryPath);
            //Check if the task is supposed to be running
            if (!taskTimer.Enabled)
            {
                //Exit function since the task is not supposed to be running
                return false;
            }
            //Append text in output_screen
            loadFirmwareForm.Output_ScreenText("\r\n" + commandLine.OutputString + "\r\n");
            //Check if the device's new firmware has been loaded
            if (LookForStringArray(desiredStringArray, undesiredStringArray, commandLine.OutputString) == false)
            {
                //Display message box
                MessageBox.Show("The " + deviceType + "'s new firmware could not be loaded! Please make sure the " + deviceType + " is properly plugged " +
                    "in to the computer, and it is the only SmartMesh IP device plugged in. Then try again!", "Load Firmware GUI");
                //Append text in output_screen
                loadFirmwareForm.Output_ScreenText("\r\nThe " + deviceType + "'s new firmware could not be loaded! Please make sure the " + deviceType +
                    " is properly plugged " + "in to the computer, and it is the only SmartMesh IP device plugged in. Then try again!");
                //Exit function
                return false;
            }
            else
            {
                //Append text in output_screen
                loadFirmwareForm.Output_ScreenText("\r\nThe " + deviceType + "'s new firmware could be loaded successfully!\r\n");
                //Exit function
                return true;
            }
        }
        #endregion ESP CommandLine Related Functions

        #region SmartMesh SDK
        /// <summary>
        /// Function used to execute the PublishToWeb.exe application.
        /// </summary>
        /// <param name="waitForTime"></param>
        /// <returns></returns>
        public static bool ExecutePublishToWebApp(int waitForTime)
        {
            //Select the process to be run
            Common.commandLine = new CommandLineExecute(Common.publishToWebApplicationProcessName, waitForTime);
            //Execute the process
            int result = Common.commandLine.Start1(Common.publishToWebArguments, Common.applicationFilesDirectoryPath, Common.CurrentNetworkManagerApiPortName, true, false);
            //Check if the PublishToWeb.exe application has been successfully started
            if (result == 0)
            {
                //Exit function
                return true;
            }
            else
            {
                //Exit function
                return false;
            }
        }

        /// <summary>
        /// Function used to read and extract some data from the PublishToWeb.log file.
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        public static string ExtractFromPublishToWebLog(string macAddress)
        {
            //Read the PublishToWeb.log file as one string.
            string publishToWebLog = System.IO.File.ReadAllText(Common.applicationFilesDirectoryPath + Common.publishToWebProcessName + ".log");
            //Get last index of the mote's Mac Address
            int macAddressLastIndex = publishToWebLog.LastIndexOf(macAddress);
            //Retrieve a string containing the last data packet received by the network manager from the mote
            string moteDataPacketLines = publishToWebLog.Substring(macAddressLastIndex);
            //Get only the line containing the last data packet received by the network manager from the mote
            string moteDataPacketLine = moteDataPacketLines.Substring(0, moteDataPacketLines.IndexOf("}"));
            //Return moteDataPacketLine
            return moteDataPacketLine;
        }

        /// <summary>
        /// Function used to gather all the fields of packets sent to the network manager.
        /// </summary>
        /// <param name="moteDataPacketLine"></param>
        /// <returns></returns>
        public static bool GetPacketFields(int moteNumber, string moteDataPacketLine)
        {
            //Declare and initialize returning bool variable
            bool result = true;
            //Remove unnecessary packetField names
            foreach (string packetFieldName in packetFieldNames)
            {
                //Check if moteDataPacketLine contains packetField
                if (moteDataPacketLine.Contains(packetFieldName))
                {
                    //Replace unnecessary packet field with empty space
                    moteDataPacketLine = moteDataPacketLine.Replace(packetFieldName, "");
                }
            }
            //Remove empty space from moteDataPacketLine
            moteDataPacketLine = moteDataPacketLine.Replace(" ", "");
            //Group moteDataPacketLine string in an array by delimiting
            string[] moteDataPacketLineArray = moteDataPacketLine.Split(':');

            #region macAddress
            //Retrieve the value of macAddress
            packetStatistics[moteNumber].macAddress = moteDataPacketLineArray[0];
            #endregion macAddress

            #region srcPort
            //Retrieve the value of srcPort
            result = int.TryParse(moteDataPacketLineArray[1], out packetStatistics[moteNumber].srcPort);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion srcPort

            #region utcUsecs
            //Retrieve the value of utcUsecs
            result = int.TryParse(moteDataPacketLineArray[2], out packetStatistics[moteNumber].utcUsecs);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion utcUsecs

            #region utcSecs
            //Retrieve the value of utcSecs
            packetStatistics[moteNumber].utcSecs = moteDataPacketLineArray[3];
            #endregion utcSecs

            #region dstPort
            //Retrieve the value of dstPort
            result = int.TryParse(moteDataPacketLineArray[4], out packetStatistics[moteNumber].dstPort);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion dstPort

            #region dataString
            //Retrieve the value of dataString
            packetStatistics[moteNumber].dataString = moteDataPacketLineArray[5];
            #endregion dataString

            #region data
            //Remove some characters from dataString
            string dataString = packetStatistics[moteNumber].dataString.Replace("[", "").Replace("]", "");
            //Group dataString string in an array by delimiting
            string[] dataStringArray = dataString.Split(',');
            //Retrieve the data bytes
            packetStatistics[moteNumber].data = Array.ConvertAll(dataStringArray, int.Parse);
            #endregion data

            //Return result
            return result;
        }
        #endregion SmartMesh SDK

        #region SmartMesh-IP Related Functions
        /// <summary>
        /// Function used to get the number of motes found in the SmartMesh-IP network.
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfFoundMotes()
        {
            //Find opened form 'Network_Manager_GUI'
            Common.Find_Network_Manager_GUI_OpenedForm();
            //Get the number of motes in the SmartMesh-IP network
            Common.mainForm.GetMotesNumberTask();
            //Return the number of motes in the SmartMesh-IP network
            return numberOfFoundMotes;
        }

        /// <summary>
        /// Function used to get the info of motes found in the SmartMesh-IP network.
        /// </summary>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static bool GetInfoOfFoundMotes(string stringToBeSearched)
        {
            //Find the index of the first moteMacAddressIdentifier
            int moteMacAddressIdentifierIndex = stringToBeSearched.IndexOf(moteMacAddressIdentifier);

            //Declare and initialize some variables
            int currentMoteNumber = 0;
            bool foundNetworkManager = false;
            bool result = false;

            //Reset some variables
            numberOfOperationalMotes = 0;
            numberOfLostMotes = 0;
            numberOfConnectingMotes = 0;

            //Gather motes' info
            for (int moteNumber = 1; moteNumber <= numberOfFoundMotes; moteNumber++)
            {
                //The below is called to not count the network manager
                Here:
                if (foundNetworkManager == true)
                {
                    //update moteNumber
                    moteNumber = currentMoteNumber;
                    //reset foundNetworkManager
                    foundNetworkManager = false;
                }

                //Find the index of the next new line symbol
                int newLineSymbolIndex = stringToBeSearched.IndexOf("\r\n", moteMacAddressIdentifierIndex);
                //Get the string line containing the info of the current mote
                string moteInfoLine = stringToBeSearched.Substring(moteMacAddressIdentifierIndex, newLineSymbolIndex - moteMacAddressIdentifierIndex);
                //Remove some characters from moteInfoLine
                moteInfoLine = moteInfoLine.Replace("\r", "").Replace("\n", "");
                //Group the mote's parameters in an array by delimiting using empty space
                string[] moteInfoArray = moteInfoLine.Split(null);
                //Remove all empty space members from moteInfoArray
                moteInfoArray = moteInfoArray.Where(w => w != "").ToArray();
                //Find the index of the next moteMacAddressIdentifier
                moteMacAddressIdentifierIndex = stringToBeSearched.IndexOf(moteMacAddressIdentifier, newLineSymbolIndex);

                #region Mac
                //Retrieve the current mote's mac address
                string macAddress = moteInfoArray[0];

                //Validate the retrieved mac address
                if (ValidateMacAddress(macAddress) == true)
                {
                    mote[moteNumber].mac = macAddress;
                }
                else
                {
                    mote[moteNumber].mac = "invalid";
                }

                //Make sure to not count the network manager
                if (mote[moteNumber].mac.ToUpper() == NetworkManager.macAddress.ToUpper())
                {
                    //Store the current mote number
                    currentMoteNumber = moteNumber;
                    //Indicate that the network manager has been found
                    foundNetworkManager = true;
                    //Remove the network manager
                    goto Here;
                }
                #endregion Mac

                #region MoteID
                //Retrieve the current mote's ID
                result = int.TryParse(moteInfoArray[1], out mote[moteNumber].moteID);
                //Check if result is false
                if (result == false)
                {
                    //Return result
                    return result;
                }
                #endregion MoteID

                #region State
                //Retrieve the current mote' state
                mote[moteNumber].state = moteInfoArray[2];
                //Update numberOfOperationalMotes
                if (mote[moteNumber].state.ToUpper() == moteStates[2].ToUpper())
                {
                    //increment numberOfOperationalMotes by 1
                    numberOfOperationalMotes++;
                }
                //Update numberOfLostMotes
                else if (mote[moteNumber].state.ToUpper() == moteStates[3].ToUpper())
                {
                    //increment numberOfLostMotes by 1
                    numberOfLostMotes++;
                }
                //Update numberOfConnectingMotes
                else if (mote[moteNumber].state.ToUpper().Contains(moteStates[0].ToUpper()) ||
                    mote[moteNumber].state.ToUpper().Contains(moteStates[1].ToUpper()))
                {
                    //increment numberOfConnectingMotes by 1
                    numberOfConnectingMotes++;
                }
                #endregion State

                #region Nbrs
                //Retrieve the current mote's nbrs
                result = int.TryParse(moteInfoArray[3], out mote[moteNumber].nbrs);
                //Check if result is false
                if (result == false)
                {
                    //Return result
                    return result;
                }
                #endregion Nbrs

                #region Links
                //Retrieve the current mote's links
                result = int.TryParse(moteInfoArray[4], out mote[moteNumber].links);
                //Check if result is false
                if (result == false)
                {
                    //Return result
                    return result;
                }
                #endregion Links

                #region Joins
                //Retrieve the current mote's joins
                result = int.TryParse(moteInfoArray[5], out mote[moteNumber].joins);
                //Check if result is false
                if (result == false)
                {
                    //Return result
                    return result;
                }
                #endregion Joins

                #region Age
                //Retrieve the current mote's age
                result = int.TryParse(moteInfoArray[6], out mote[moteNumber].age);
                //Check if result is false
                if (result == false)
                {
                    //Return result
                    return result;
                }
                #endregion Age

                #region StateTime
                //Retrieve the current mote' stateTime
                mote[moteNumber].stateTime = moteInfoArray[7];
                #endregion StateTime
            }

            //Return result
            return result;
        }

        /// <summary>
        /// Function used to get all the SmartMesh-IP network statistics data.
        /// </summary>
        /// <returns></returns>
        public static void GetOverallNetworkStatistics()
        {
            //Find opened form 'Network_Manager_GUI'
            Common.Find_Network_Manager_GUI_OpenedForm();
            //Get the SmartMesh-IP network statistics data
            Common.mainForm.GetNetworkStatisticsTask();
        }

        /// <summary>
        /// Function used to get the network manager statistics.
        /// </summary>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static bool GetNetworkManagerStatistics(string stringToBeSearched)
        {
            //Declare and initialize returning bool variable
            bool result = true;
            //Find the index of end of networkManagerStatisticsIdentifier
            int networkManagerStatisticsIdentifierIndex = stringToBeSearched.IndexOf(networkManagerStatisticsIdentifier) + networkManagerStatisticsIdentifier.Length;
            //Find the index of beginning of networkStatisticsIdentifier
            int networkStatisticsIdentifierIndex = stringToBeSearched.IndexOf(networkStatisticsIdentifier);
            //Get the string containing the network manager statistics
            string networkManagerStatisticsString = stringToBeSearched.Substring(networkManagerStatisticsIdentifierIndex, networkStatisticsIdentifierIndex - networkManagerStatisticsIdentifierIndex);
            //Remove some characters from networkManagerStatisticsString
            networkManagerStatisticsString = networkManagerStatisticsString.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //Remove some strings from networkManagerStatisticsString
            networkManagerStatisticsString = RemoveStringArray(networkManagerStatisticsStrings, networkManagerStatisticsString);
            //Group the network manager statistics in an array by delimiting using empty space
            string[] networkManagerStatisticsArray = networkManagerStatisticsString.Split(':');
            //Remove all empty space members from networkManagerStatisticsArray
            networkManagerStatisticsArray = networkManagerStatisticsArray.Where(w => w != "").ToArray();

            #region establishedConnections
            //Retrieve the value of establishedConnections
            result = int.TryParse(networkManagerStatisticsArray[0], out networkManagerStatistics.establishedConnections);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion establishedConnections

            #region droppedConnections
            //Retrieve the value of droppedConnections
            result = int.TryParse(networkManagerStatisticsArray[1], out networkManagerStatistics.droppedConnections);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion droppedConnections

            #region transmitOK
            //Retrieve the value of transmitOK
            result = int.TryParse(networkManagerStatisticsArray[2], out networkManagerStatistics.transmitOK);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion transmitOK

            #region transmitError
            //Retrieve the value of transmitError
            result = int.TryParse(networkManagerStatisticsArray[3], out networkManagerStatistics.transmitError);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion transmitError

            #region transmitRepeat
            //Retrieve the value of transmitRepeat
            result = int.TryParse(networkManagerStatisticsArray[4], out networkManagerStatistics.transmitRepeat);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion transmitRepeat

            #region receiveOK
            //Retrieve the value of receiveOK
            result = int.TryParse(networkManagerStatisticsArray[5], out networkManagerStatistics.receiveOK);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion receiveOK

            #region receiveError
            //Retrieve the value of receiveError
            result = int.TryParse(networkManagerStatisticsArray[6], out networkManagerStatistics.receiveError);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion receiveError

            #region acknowledgeDelayAvrg
            //Retrieve the value of acknowledgeDelayAvrg
            networkManagerStatistics.acknowledgeDelayAvrg = networkManagerStatisticsArray[7];
            #endregion acknowledgeDelayAvrg

            #region acknowledgeDelayMax
            //Retrieve the value of acknowledgeDelayMax
            networkManagerStatistics.acknowledgeDelayMax = networkManagerStatisticsArray[8];
            #endregion acknowledgeDelayMax

            //Return result
            return result;
        }

        /// <summary>
        /// Function used to get the network statistics.
        /// </summary>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static bool GetNetworkStatistics(string stringToBeSearched)
        {
            //Declare and initialize returning bool variable
            bool result = true;
            //Find the index of end of networkStatisticsIdentifier
            int networkStatisticsIdentifierIndex = stringToBeSearched.IndexOf(networkStatisticsIdentifier) + networkStatisticsIdentifier.Length;
            //Find the index of beginning of moteStatisticsIdentifier
            int moteStatisticsIdentifierIndex = stringToBeSearched.IndexOf(moteStatisticsIdentifierArray[0]);
            //Get the string containing the network statistics
            string networkStatisticsString = stringToBeSearched.Substring(networkStatisticsIdentifierIndex, moteStatisticsIdentifierIndex - networkStatisticsIdentifierIndex);
            //Remove some characters from networkStatisticsString
            networkStatisticsString = networkStatisticsString.Replace("\r", "").Replace(" ", "");
            //Remove some strings from networkStatisticsString
            networkStatisticsString = RemoveStringArray(networkStatisticsStrings, networkStatisticsString);
            //Group the network statistics in an array by delimiting using empty space
            string[] networkStatisticsArray = networkStatisticsString.Split('\n');
            //Remove all empty space members from networkStatisticsArray
            networkStatisticsArray = networkStatisticsArray.Where(w => w != "").ToArray();

            #region reliability
            //Retrieve the value of reliability
            networkStatistics.reliability = networkStatisticsArray[0];
            //Find the index of "%"
            int index = networkStatistics.reliability.IndexOf("%");
            //Store the numerical value of reliability
            result = double.TryParse(networkStatistics.reliability.Substring(0, index), out networkStatistics.reliabilityNumericalValue);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion reliability

            #region stability
            //Retrieve the value of stability
            networkStatistics.stability = networkStatisticsArray[1];
            //Find the index of "%"
            index = networkStatistics.stability.IndexOf("%");
            //Store the numerical value of stability
            result = double.TryParse(networkStatistics.stability.Substring(0, index), out networkStatistics.stabilityNumericalValue);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion stability

            #region latency
            //Retrieve the value of latency
            networkStatistics.latency = networkStatisticsArray[2];
            //Find the index of latency unit
            index = networkStatistics.latency.IndexOf(latencyUnit);
            //Store the numerical value of latency
            result = double.TryParse(networkStatistics.latency.Substring(0, index), out networkStatistics.latencyNumericalValue);
            //Check if result is false
            if (result == false)
            {
                //Return result
                return result;
            }
            #endregion latency

            //Return result
            return result;
        }

        /// <summary>
        /// Function used to get the mote statistics.
        /// </summary>
        /// <param name="stringToBeSearched"></param>
        /// <returns></returns>
        public static bool GetMoteStatistics(string stringToBeSearched)
        {
            //Declare and initialize returning bool variable
            bool result = true;
            //Find the index of end of moteStatisticsIdentifierArray[1]
            int moteStatisticsIdentifierIndex = stringToBeSearched.IndexOf(moteStatisticsIdentifierArray[1]) + moteStatisticsIdentifierArray[1].Length;
            //Get the string containing the mote statistics
            string moteStatisticsString = stringToBeSearched.Substring(moteStatisticsIdentifierIndex);
            //Check if there is any mote statistics data available
            if (!moteStatisticsString.Contains(moteStatisticsIdentifierArray[2]))
            {
                //Set result to false
                result = false;
            }
            else
            {
                //Find the first index of end of moteStatisticsIdentifierArray[2]
                moteStatisticsIdentifierIndex = stringToBeSearched.IndexOf(moteStatisticsIdentifierArray[2]) + moteStatisticsIdentifierArray[2].Length;
                //Get the mote(s)' statistics data
                foreach (Common.MoteStruct mote in Common.mote)
                {
                    //Make sure the current mote's state is not null
                    if (!string.IsNullOrEmpty(mote.state))
                    {
                        //Update moteNumber
                        int moteNumber = mote.moteID - 1; //subtract 1 since the network manager's moteID = 1
                        //Check if the current mote's state is operational
                        if (mote.state.ToUpper() == Common.moteStates[2].ToUpper())
                        {
                            //Find the index of the next new line symbol
                            int newLineSymbolIndex = stringToBeSearched.IndexOf("\r\n", moteStatisticsIdentifierIndex);
                            //Get the string containing the mote statistics
                            moteStatisticsString = stringToBeSearched.Substring(moteStatisticsIdentifierIndex, newLineSymbolIndex - moteStatisticsIdentifierIndex);
                            //Group the mote statistics in an array by delimiting using empty space
                            string[] moteStatisticsArray = moteStatisticsString.Split(null);
                            //Remove all empty space members from networkStatisticsArray
                            moteStatisticsArray = moteStatisticsArray.Where(w => w != "").ToArray();
                            //Find the index of the next moteStatisticsIdentifierArray[2]
                            moteStatisticsIdentifierIndex = stringToBeSearched.IndexOf(moteStatisticsIdentifierArray[2], newLineSymbolIndex) + moteStatisticsIdentifierArray[2].Length;

                            #region moteID
                            //Retrieve the value of moteID
                            result = int.TryParse(moteStatisticsArray[0], out moteStatistics[moteNumber].moteID);
                            //Check if result is false
                            if (result == false)
                            {
                                //Return result
                                return result;
                            }
                            #endregion moteID

                            #region received
                            //Retrieve the value of received
                            result = int.TryParse(moteStatisticsArray[1], out moteStatistics[moteNumber].received);
                            //Check if result is false
                            if (result == false)
                            {
                                //Return result
                                return result;
                            }
                            #endregion received

                            #region lost
                            //Retrieve the value of lost
                            result = int.TryParse(moteStatisticsArray[2], out moteStatistics[moteNumber].lost);
                            //Check if result is false
                            if (result == false)
                            {
                                //Return result
                                return result;
                            }
                            #endregion lost

                            #region reliability
                            //Retrieve the value of reliability
                            moteStatistics[moteNumber].reliability = moteStatisticsArray[3];
                            #endregion reliability

                            #region latency
                            //Retrieve the value of latency
                            result = int.TryParse(moteStatisticsArray[4], out moteStatistics[moteNumber].latency);
                            //Check if result is false
                            if (result == false)
                            {
                                //Return result
                                return result;
                            }
                            #endregion latency

                            #region hops
                            //Retrieve the value of hops
                            result = double.TryParse(moteStatisticsArray[5], out moteStatistics[moteNumber].hops);
                            //Check if result is false
                            if (result == false)
                            {
                                //Return result
                                return result;
                            }
                            #endregion hops
                        }
                    }
                }
            }

            //Return result
            return result;
        }
        #endregion SmartMesh-IP Related Functions

        #region Conversion Function(s)
        /// <summary>
        /// Function used to convert from Celsius to Fahrenheit.
        /// </summary>
        /// <param name="celsius"></param>
        /// <returns></returns>
        public static double ConvertFromCelsiusToFahrenheit(double celsius)
        {
            //convert from Celsius to Fahrenheit
            return ((celsius * 9.0) / 5.0 + 32.0);
        }

        /// <summary>
        /// Function used to convert the mac address from decimal to hexadecimal.
        /// For example, from [0, 23, 13, 0, 0, 90, 92, 36] to 00-17-0D-00-00-5A-5C-24.
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        public static string ConvertMacAddressFromDecToHex(string macAddress)
        {
            //Declare and initialize variables
            string result = string.Empty;
            int counter = 0;
            //Remove '[', ']', and empty space from macAddress
            macAddress = macAddress.Replace("[", "").Replace("]", "").Replace(" ", "");
            //Group macAddress string in an array by delimiting
            string[] macAddressArray = macAddress.Split(',');
            //Convert values from Decimal to HexaDecimal
            foreach (string value in macAddressArray)
            {
                //Convert from string to integer
                int decValue = Convert.ToUInt16(value);
                //Convert from Decimal to HexaDecimal
                string hexValue = decValue.ToString("X2");
                //Increment counter by 1
                counter++;
                //Check if this is the last iteration
                if (counter == macAddressArray.Length)
                {
                    //Add the HexaDecimal value into result
                    result += hexValue;
                }
                else //this is NOT the last iteration
                {
                    //Add the HexaDecimal value into result
                    result += hexValue + "-";
                }
            }
            //Return result
            return result;
        }

        /// <summary>
        /// Function used to convert the mac address from hexadecimal to decimal.
        /// For example, from 00-17-0D-00-00-5A-5C-24 to [0, 23, 13, 0, 0, 90, 92, 36].
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        public static string ConvertMacAddressFromHexToDec(string macAddress)
        {
            //Declare and initialize variables
            string result = "[";
            int counter = 0;
            //Group macAddress string in an array by delimiting
            string[] macAddressArray = macAddress.Split('-');
            //Convert values from HexaDecimal to Decimal
            foreach (string value in macAddressArray)
            {
                //Convert from HexaDecimal to Decimal
                int decValue = int.Parse(value, System.Globalization.NumberStyles.HexNumber);
                //Increment counter by 1
                counter++;
                //Check if this is the last iteration
                if (counter == macAddressArray.Length)
                {
                    //Add the Decimal value into result
                    result += decValue + "]";
                }
                else //this is NOT the last iteration
                {
                    //Add the Decimal value into result
                    result += decValue + ", ";
                }
            }
            //Return result
            return result;
        }
        #endregion Conversion Function(s)

        #region Programs
        /// <summary>
        /// Function used to terminate some programs at startup.
        /// </summary>
        public static void CloseSomeProgramsAtStartup()
        {
            foreach (string program in programsToCloseAtStartup)
            {
                foreach (Process process in Process.GetProcessesByName(program))
                {
                    process.Kill(); //close the process
                }
            }
        }

        /// <summary>
        /// Function used to terminate a program.
        /// </summary>
        /// <param name="program"></param>
        public static void CloseProgram(string program)
        {
            foreach (Process process in Process.GetProcessesByName(program))
            {
                process.Kill(); //close the process
            }
        }

        /// <summary>
        /// Function used to check if a program is running or not.
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public static bool IsProgramRunning(string program)
        {
            //check if a program is running or not.
            Process[] programName = Process.GetProcessesByName(program);
            if (programName.Length > 0)
            {
                return true; //program is running
            }
            else
            {
                return false; //program is NOT running
            }
        }
        #endregion Programs
    }
}