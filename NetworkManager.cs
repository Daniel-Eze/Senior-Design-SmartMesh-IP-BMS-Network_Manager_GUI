namespace Network_Manager_GUI
{
    public class NetworkManager
    {
        #region Variables/Instances Declaration and Initialization
        #region ID
        public static string macAddress = string.Empty;
        #endregion ID

        #region Timeouts
        /// <summary>
        /// restartTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the RESTART task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int restartTaskTimeout = 30000;
        /// <summary>
        /// connectionTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the CONNECTION task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int connectionTaskTimeout = 3000 + restartTaskTimeout;
        /// <summary>
        /// factoryResetTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the FACTORYRESET task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int factoryResetTaskTimeout = 3000;
        /// <summary>
        /// radiotestTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the RADIOTEST task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int radiotestTaskTimeout = 35000;
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
        public static readonly int setNetworkIdTaskTimeout = 35000;
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
        public static readonly int setJoinKeyTaskTimeout = 35000;
        /// <summary>
        /// getMotesNumberTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the GET_MOTESNUMBER task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int getMotesNumberTaskTimeout = 6000;
        /// <summary>
        /// getNetworkStatisticsTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the GET_NETWORKSTATISTICS task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int getNetworkStatisticsTaskTimeout = 6000;
        #endregion Timeouts

        #region Limits
        public static readonly int networkIdLowerLimit = 1;
        public static readonly int networkIdUpperLimit = 65534;
        public static readonly int joinKeyCharacterCountLimit = 32;
        #endregion Limits

        #region Task Command Strings
        public static readonly string connectionTaskCommandString = "\r\nlogin user";
        public static readonly string restartTaskCommandString = "\r\nreset system";
        public static readonly string factoryResetTaskCommandString = "\r\nexec restore";
        public static readonly string radiotestTaskCommandString1 = "\r\nradiotest";
        public static readonly string radiotestTaskCommandString2 = "\r\nshow status";
        public static readonly string setNetworkIdTaskCommandString = "\r\nset config netid=";
        public static readonly string getNetworkIdTaskCommandString = "\r\nmget netid";
        public static readonly string setJoinKeyTaskCommandString = "\r\nset config commonjoinkey=";
        public static readonly string macAddressCommandString = "\r\nminfo";
        public static readonly string getMotesNumberTaskCommandString = "\r\nsm";
        public static readonly string getNetworkStatisticsTaskCommandString = "\r\nshow stat";
        #endregion Task Command Strings

        #region Task Validation Strings
        public static readonly string connectionTaskDesiredStringToLookFor = connectionTaskCommandString;
        public static readonly string getMacAddressDesiredStringToLookFor = "mac:";
        public static readonly string[] connectionTaskUndesiredStringArrayToLookFor = new string[] { "error", "wrong", "access denied", "Radio Test Mode" };
        public static readonly string[] restartTaskDesiredStringArrayToLookFor = new string[] { "SmartMesh IP Manager" };
        public static readonly string[] restartTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string factoryResetTaskDesiredStringToLookFor = factoryResetTaskCommandString;
        public static readonly string[] factoryResetTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string radiotestTaskDesiredStringToLookFor1 = "Radio Test Mode";
        public static readonly string radiotestTaskDesiredStringToLookFor2 = "Network Regular Mode";
        public static readonly string[] radiotestTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string setNetworkIdTaskDesiredStringToLookFor = setNetworkIdTaskCommandString;
        public static readonly string[] setNetworkIdTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string getNetworkIdTaskDesiredStringToLookFor = getNetworkIdTaskCommandString + "netid =";
        public static readonly string[] getNetworkIdTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string setJoinKeyTaskDesiredStringToLookFor = setJoinKeyTaskCommandString;
        public static readonly string[] setJoinKeyTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string[] getMotesNumberTaskDesiredStringArrayToLookFor = new string[] { "Number of motes (max 101): Total",
        ", Live", "Joining", "Blink" };
        public static readonly string[] getMotesNumberTaskUndesiredStringArrayToLookFor = new string[] { "error" };
        public static readonly string[] getNetworkStatisticsTaskDesiredStringArrayToLookFor = new string[] { "Manager Statistics",
        "Network Statistics", "Motes Statistics" };
        public static readonly string[] getNetworkStatisticsTaskUndesiredStringArrayToLookFor = new string[] {  };
        #endregion Task Validation Strings

        #region Task Outcome
        /// <summary>
        /// Variable used to indicate whether the CONNECTION task passed. 
        /// That is, the network manager has been successfully accessed via serial port.
        /// GUI is connected to the network manager.
        /// </summary>
        public static bool isConnectedToGUI = false;
        #endregion Task Outcome

        #region ESP CommandLine
        public static string networkManagerFirmwareFilename = "FullManagerImage.bin";
        public static string[] networkManagerID = new string[] { "devString = DC2274 WITH MEMORY A",
            "DC2274 WITH MEMORY B" };
        public static string[] networkManagerEraseFirmwareResponse = new string[] {  Common.standardEspCommandLineResponse[0],
            Common.standardEspCommandLineResponse[1], "devName = " + networkManagerID[1], "Erase chip" };
        public static string[] networkManagerLoadFirmwareResponse = new string[] {  Common.standardEspCommandLineResponse[0],
            Common.standardEspCommandLineResponse[1], "devName = " + networkManagerID[1], "Fast program: fileName = " +
            networkManagerFirmwareFilename + ", offset = 0x0", "Verify: reference fileName = " + networkManagerFirmwareFilename +
            ", offset = 0x0", "Verify: PASS" };
        #endregion ESP CommandLine
        #endregion Variables/Instances Declaration and Initialization
    }
}