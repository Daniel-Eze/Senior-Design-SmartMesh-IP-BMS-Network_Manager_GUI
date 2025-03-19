namespace Network_Manager_GUI
{
    public class Mote
    {
        #region Variables/Instances Declaration and Initialization
        #region ID
        public static string macAddress = string.Empty;
        #endregion ID

        #region Timeouts
        /// <summary>
        /// connectionTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the CONNECTION task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int connectionTaskTimeout = 10000;
        /// <summary>
        /// restartTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the RESTART task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int restartTaskTimeout = 8000;
        /// <summary>
        /// factoryResetTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the FACTORYRESET task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int factoryResetTaskTimeout = 3000;
        /// <summary>
        /// modeTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the MODE task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int modeTaskTimeout = 15000;
        /// <summary>
        /// autoJoinTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the AUTOJOIN task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int autoJoinTaskTimeout = 15000;
        /// <summary>
        /// radiotestTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the RADIOTEST task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int radiotestTaskTimeout = 15000;
        /// <summary>
        /// firmwareLoadTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the FIRMWARELOAD task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int firmwareLoadTaskTimeout = 45000;
        /// <summary>
        /// setNetworkIdTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the SET_NETWORKID task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int setNetworkIdTaskTimeout = 15000;
        /// <summary>
        /// getNetworkIdTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the GET_NETWORKID task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int getNetworkIdTaskTimeout = 3000;
        /// <summary>
        /// setJoinKeyTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the SET_JOINKEY task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int setJoinKeyTaskTimeout = 15000;
        #endregion Timeouts

        #region Limits
        public static readonly int networkIdLowerLimit = 1;
        public static readonly int networkIdUpperLimit = 65534;
        public static readonly int joinKeyCharacterCountLimit = 32;
        #endregion Limits

        #region Task Command Strings
        public static readonly string connectionTaskCommandString = "\r\ninfo";
        public static readonly string restartTaskCommandString = "\r\nreset";
        public static readonly string factoryResetTaskCommandString = "\r\nrestore";
        public static readonly string modeTaskCommandString1 = "\r\nset mode";
        public static readonly string modeTaskCommandString2 = "\r\nget mode";
        public static readonly string autoJoinTaskCommandString1 = "\r\nmset autojoin";
        public static readonly string autoJoinTaskCommandString2 = "\r\nmget autojoin";
        public static readonly string radiotestTaskCommandString = "\r\nradiotest";
        public static readonly string setNetworkIdTaskCommandString = "\r\nmset netid";
        public static readonly string getNetworkIdTaskCommandString = "\r\nmget netid";
        public static readonly string setJoinKeyTaskCommandString = "\r\nmset jkey";
        public static readonly string macAddressCommandString = "\r\nminfo";
        #endregion Task Command Strings

        #region Task Validation Strings
        public static readonly string connectionTaskDesiredStringToLookFor = "IP Mote";
        public static readonly string getMacAddressDesiredStringToLookFor = "mac:";
        public static readonly string[] connectionTaskUndesiredStringArrayToLookFor = new string[] { "error", "access denied" };
        public static readonly string[] restartTaskDesiredStringArrayToLookFor = new string[] { "SmartMesh IP mote" };
        public static readonly string[] restartTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string factoryResetTaskDesiredStringToLookFor = "restoring main module";
        public static readonly string[] factoryResetTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string modeTaskDesiredStringToLookFor1 = modeTaskCommandString1;
        public static readonly string modeTaskDesiredStringToLookFor2 = modeTaskCommandString2;
        public static readonly string[] modeTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string autoJoinTaskDesiredStringToLookFor1 = autoJoinTaskCommandString1;
        public static readonly string autoJoinTaskDesiredStringToLookFor2 = "autojoin =";
        public static readonly string[] autoJoinTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string radiotestTaskDesiredStringToLookFor = "Radio Test:";
        public static readonly string[] radiotestTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string setNetworkIdTaskDesiredStringToLookFor = setNetworkIdTaskCommandString;
        public static readonly string[] setNetworkIdTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string getNetworkIdTaskDesiredStringToLookFor = getNetworkIdTaskCommandString + "netid =";
        public static readonly string[] getNetworkIdTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string setJoinKeyTaskDesiredStringToLookFor = setJoinKeyTaskCommandString;
        public static readonly string[] setJoinKeyTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        #endregion Task Validation Strings

        #region Task Outcome
        /// <summary>
        /// Variable used to indicate whether the CONNECTION task passed. 
        /// That is, the mote has been successfully accessed via serial port.
        /// GUI is connected to the mote.
        /// </summary>
        public static bool isConnectedToGUI = false;
        #endregion Task Outcome

        #region ESP CommandLine
        public static string moteFirmwareFilename = "FullMoteImage.bin";
        public static string[] validMoteID = new string[] { "devString = Dust Interface Board A",
            "Dust Interface Board B" };
        public static string hartMoteID = "devString = Quad RS232-HS";
        public static string[] moteEraseFirmwareResponse = new string[] { Common.standardEspCommandLineResponse[0],
            Common.standardEspCommandLineResponse[1], "devName = " + validMoteID[1], "Erase chip" };
        public static string[] moteLoadFirmwareResponse = new string[] {  Common.standardEspCommandLineResponse[0],
            Common.standardEspCommandLineResponse[1], "devName = " + validMoteID[1], "Fast program: fileName = " +
            moteFirmwareFilename + ", offset = 0x0", "Verify: reference fileName = " + moteFirmwareFilename +
            ", offset = 0x0", "Verify: PASS" };
        #endregion ESP CommandLine
        #endregion Variables/Instances Declaration and Initialization
    }
}