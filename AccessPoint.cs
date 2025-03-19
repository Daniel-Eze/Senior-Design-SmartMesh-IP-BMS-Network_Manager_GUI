namespace Network_Manager_GUI
{
    public class AccessPoint
    {
        #region Variables/Instances Declaration and Initialization
        #region Timeouts
        /// <summary>
        /// firmwareLoadTaskTimeout (millisecondsTimeout):
        /// The number of milliseconds to wait for the FIRMWARELOAD task to terminate.
        /// Otherwise, the task execution will be automatically canceled.
        /// </summary>
        public static readonly int firmwareLoadTaskTimeout = 45000;
        #endregion Timeouts

        #region ESP CommandLine
        public static string accessPointFirmwareFilename = "FullAccessPointImage.bin";
        public static string[] accessPointID = new string[] { "devString = DC2274 WITHOUT MEMORY 31C666 A",
            "DC2274 WITHOUT MEMORY 31C666 B" };
        public static string[] accessPointEraseFirmwareResponse = new string[] {  Common.standardEspCommandLineResponse[0],
            Common.standardEspCommandLineResponse[1], "devName = " + accessPointID[1], "Erase chip" };
        public static string[] accessPointLoadFirmwareResponse = new string[] {  Common.standardEspCommandLineResponse[0],
            Common.standardEspCommandLineResponse[1], "devName = " + accessPointID[1], "Fast program: fileName = " +
            accessPointFirmwareFilename + ", offset = 0x0", "Verify: reference fileName = " + accessPointFirmwareFilename +
            ", offset = 0x0", "Verify: PASS" };
        #endregion ESP CommandLine
        #endregion Variables/Instances Declaration and Initialization
    }
}