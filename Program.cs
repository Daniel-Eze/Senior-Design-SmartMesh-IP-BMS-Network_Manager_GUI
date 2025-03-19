using System;
using System.Windows.Forms;

namespace Network_Manager_GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// 1. Have Python 2.7.x installed on the PC and make sure the folder "C:\Python27\" has been created.
        /// 2. Copy the Dust "SmartMeshSDK-1.3.0.1-win\win" folder into the "C:\Python27\" folder so that the path
        /// "C:\Python27\SmartMeshSDK-1.3.0.1-win\win" exits with all the Dust related files in it.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Network_Manager_GUI());
        }
    }
}