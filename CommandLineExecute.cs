using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Network_Manager_GUI
{
    public class CommandLineExecute
    {
        #region Constructor
        /// <summary>
        /// Constructor used to create a new instance of the 'CommandLineExecute' class.
        /// The parameter 'waitForTime' is in milliseconds.
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="waitForTime"></param>
        public CommandLineExecute(string programName, int waitForTime)
        {
            _programName = programName;
            _waitfor = waitForTime;
        }
        #endregion Constructor

        #region Member Variables
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr mainWindowHandle, Int32 windowStyle);
        private readonly Int32 SW_MINIMIZE = 2;
        private readonly string _programName = String.Empty;
        private readonly int _waitfor = 1200;
        public string OutputString { get; private set; } = "";
        public string OutputError { get; private set; } = "";
        public ArrayList OutputList { get; } = new ArrayList();
        public ArrayList OutputErrorList { get; } = new ArrayList();
        #endregion  Member Variables

        #region Member Functions
        /// <summary>
        /// Function used to update 'OutputString'.
        /// </summary>
        /// <param name="programOutputString"></param>
        private void Update_OutputString(string programOutputString)
        {
            OutputString += programOutputString;
            OutputList.Add(programOutputString);
        }

        /// <summary>
        /// Function used to update 'OutputError'.
        /// </summary>
        /// <param name="programOutputError"></param>
        private void Update_OutputError(string programOutputError)
        {
            OutputError += programOutputError;
            OutputErrorList.Add(programOutputError);
        }

        /// <summary>
        /// Function used to process the data received through Windows cmd.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="outLine"></param>
        private void Process_DataReceived(object sender, DataReceivedEventArgs outLine)
        {
            if (outLine.Data != null)
            {
                Update_OutputString(outLine.Data);
            }
        }

        /// <summary>
        /// Function used to process the error received through Windows cmd.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="error"></param>
        private void Process_ErrorRecevied(object sender, DataReceivedEventArgs error)
        {
            if (error.Data != null)
            {
                Update_OutputError(error.Data);
            }
        }
        #endregion Member Functions

        #region ICommandLineExecute
        /// <summary>
        /// Function used to execute a process.
        /// </summary>
        /// <param name="programArguments"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        public int Start(string programArguments, string workingDirectory)
        {
            //Declare and initialize a process
            Process process = new Process();
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.StartInfo.FileName = _programName;
            process.StartInfo.Arguments = programArguments;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.OutputDataReceived += new DataReceivedEventHandler(Process_DataReceived);
            process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorRecevied);

            //Execute the process
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit(_waitfor);
                if (process.HasExited)
                {
                    //Assign return value
                    return 0;
                }
                else
                {
                    //Assign return value
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Capture the error details
                ErrorValueString = " Message - " + System.Environment.NewLine + ex.Message + "Source - " + System.Environment.NewLine + ex.Source;
                //Display the error message
                System.Windows.Forms.MessageBox.Show(ex.Message, "Command Line Execution Error");
                //Dispose and close process
                DisposeCloseProcess(process);
                //Assign return value
                return 6;
            }
            finally
            {
                //Dispose and close process
                DisposeCloseProcess(process);
            }
        }

        /// <summary>
        /// Function used to execute a process.
        /// </summary>
        /// <param name="programArguments"></param>
        /// <param name="stringToWrite"></param>
        /// <param name="windowStyle"></param>
        /// <returns></returns>
        public int Start1(string programArguments, string workingDirectory, string stringToWrite, bool createNoWindow = true, bool closeProcess = true, ProcessWindowStyle windowStyle = ProcessWindowStyle.Minimized)
        {
            //Declare and initialize a process
            ProcessStartInfo start = new ProcessStartInfo
            {
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = createNoWindow,
                UseShellExecute = false,
                WindowStyle = windowStyle,
                FileName = _programName,
                Arguments = programArguments,
                WorkingDirectory = workingDirectory
            };
            //Execute the process
            using (Process process = Process.Start(start))
            {
                try
                {
                    //Check if the process window is to be minimized
                    if (createNoWindow == false && start.WindowStyle == ProcessWindowStyle.Minimized)
                    {
                        //Get the recently started process
                        Process[] myProcess = Process.GetProcessesByName(process.ProcessName);
                        //Declare a variable to contain the window handle of the process
                        IntPtr mainWindowHandle;
                        //Declare a counter to prevent infinite do-while loop
                        int counter = 0;
                        //Keep looping until the window is detected
                        do
                        {
                            //Get the window handle of the process
                            mainWindowHandle = myProcess[0].MainWindowHandle;
                            //Minimize the process window
                            ShowWindow(mainWindowHandle, SW_MINIMIZE);
                            //Increment counter by 1
                            counter++;
                        } while ((mainWindowHandle.ToInt32() == 0) && (counter < 200)); //under normal circumstances, counter should be between 30 and 70
                    }
                    //Configure output and error events
                    process.OutputDataReceived += new DataReceivedEventHandler(Process_DataReceived);
                    process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorRecevied);
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    //Write within process
                    using (StreamWriter sw = process.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            sw.WriteLine(stringToWrite + "\r\n");
                        }
                    }
                    //Wait for process to exit
                    process.WaitForExit(_waitfor);
                    //Close process by default, unless otherwise requested
                    if (closeProcess == true)
                    {
                        process.CloseMainWindow();
                    }
                    //Delay used to collect the output
                    Thread.Sleep(300);
                    //Exit process
                    if (process.HasExited)
                    {
                        //Assign return value
                        return 0;
                    }
                    else
                    {
                        //Assign return value
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    //Capture the error details
                    ErrorValueString = " Message - " + System.Environment.NewLine + ex.Message + "Source - " + System.Environment.NewLine + ex.Source;
                    //Display the error message
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Command Line Execution Error");
                    //Dispose and close process
                    DisposeCloseProcess(process);
                    //Assign return value
                    return 6;
                }
                finally
                {
                    //Dispose and close process
                    DisposeCloseProcess(process);
                }
            }
        }

        /// <summary>
        /// Function used to dispose and close the process.
        /// </summary>
        /// <param name="process"></param>
        private void DisposeCloseProcess(Process process)
        {
            process.Dispose();
            GC.SuppressFinalize(process);
            process.Close();
        }
        #endregion  ICommandLineExecute

        #region IError
        public int ErrorValue { get; } = 0;
        public string ErrorValueString { get; private set; } = " ";
        #endregion IError
    }
}