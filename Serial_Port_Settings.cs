using System;
using System.Linq;
using System.Windows.Forms;

namespace Network_Manager_GUI
{
    public partial class FrmPortSettings : Form
    {
        public FrmPortSettings()
        {
            InitializeComponent();
        }

        #region GUI Form        
        /// <summary>
        /// Function used when the Serial Port Settings GUI is starting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPortSettings_Load(object sender, EventArgs e)
        {
            Common.Network_Manager_GUI_Enabled(false); //disable the main (Network Manager GUI) form
            Populate_Serial_Port_Settings_ComboBoxes(); //populate serial port settings's combo boxes
            CurrentSerialPortSettings(); //show the current serial port settings
        }

        /// <summary>
        /// Function used when the Serial Port Settings GUI is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPortSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Common.Network_Manager_GUI_Enabled(true); //enable the main (Network Manager GUI) form
        }
        #endregion GUI Form

        #region Control Clicks
        /// <summary>
        /// Function used to apply the displayed serial port settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonApplySettings(object sender, EventArgs e)
        {
            //Make sure all combo boxes are filled
            if (AreAllComboBoxesFilled(out string whichComBoxIsEmpty) == false)
            {
                //Show message
                MessageBox.Show("The '" + whichComBoxIsEmpty +
                "' box is empty! Please make sure all entry boxes are filled properly.", "Serial Port Settings GUI");
                //Exit function
                return;
            }

            //Update Serial Port settings based on the user input
            Common.mainForm.UpdateSerialPortSettings(
                combo_PortName.SelectedItem.ToString(),
                int.Parse(combo_BaudRate.SelectedItem.ToString()),
                int.Parse(combo_DataBits.SelectedItem.ToString()),
                (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), combo_Parity.SelectedItem.ToString()),
                (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), combo_StopBits.SelectedItem.ToString()),
                (System.IO.Ports.Handshake)Enum.Parse(typeof(System.IO.Ports.Handshake), combo_Handshake.SelectedItem.ToString()));

            //Populate COM Port Box of 'Network_Manager_GUI' form
            Common.PopulateComPortBox("Network_Manager_GUI");
            //Assign selected COM Port in 'Network_Manager_GUI' COM Port ComboBox
            Common.mainForm.Combo_PortText(combo_PortName.SelectedItem.ToString());
            //Perform Connection task based on device type
            if (Common.deviceType == EnumDeviceType.NETWORK_MANAGER)
            {
                Common.mainForm.ButtonConnection_Click(sender, e);
            }
            else if (Common.deviceType == EnumDeviceType.MOTE)
            {
                Common.mainForm.ButtonConnectionMote_Click(sender, e);
            }

            //Close the Serial_Port_Settings form
            Close();
        }

        /// <summary>
        /// Function used to reset serial ports settings to defalut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonResetToDefault_Click(object sender, EventArgs e)
        {
            //Grab default serial port settings
            Common.mainForm.AssignDefaultSerialPortSettings();
            //Assign default serial port settings to the Serial Port Settings GUI
            CurrentSerialPortSettings();
        }

        /// <summary>
        /// Function used to refresh 'combo_PortName' items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_Port_Click(object sender, EventArgs e)
        {
            //Populate COM Port Box
            Common.PopulateComPortBox(Name);
        }
        #endregion Control Clicks

        #region Other Functions
        /// <summary>
        /// Function used to populate Serial Port Settings's combo boxes.
        /// </summary>
        private void Populate_Serial_Port_Settings_ComboBoxes()
        {
            //Populate COM Port Box
            Common.PopulateComPortBox(Name);

            // Get Available Parity options
            foreach (string s in Enum.GetNames(typeof(System.IO.Ports.Parity)))
            {
                combo_Parity.Items.Add(s); //fill the available parity options in related combo box        
                Application.DoEvents(); //refresh GUI
            }

            // Get Available StopBits options
            foreach (string s in Enum.GetNames(typeof(System.IO.Ports.StopBits)))
            {
                combo_StopBits.Items.Add(s); //fill the available stopbits options in related combo box    
                Application.DoEvents(); //refresh GUI
            }

            // Get Available Handshake options
            foreach (string s in Enum.GetNames(typeof(System.IO.Ports.Handshake)))
            {
                combo_Handshake.Items.Add(s); //fill the available handshake options in related combo box     
                Application.DoEvents(); //refresh GUI
            }
        }

        /// <summary>
        /// Function used to display current serial port settings.
        /// </summary>
        private void CurrentSerialPortSettings()
        {
            //Grab and Show current serial port settings
            combo_PortName.SelectedItem = Common.mainForm.serialPort1.PortName.ToString();
            combo_BaudRate.SelectedItem = Common.mainForm.serialPort1.BaudRate.ToString();
            combo_DataBits.SelectedItem = Common.mainForm.serialPort1.DataBits.ToString();
            combo_Parity.SelectedItem = Common.mainForm.serialPort1.Parity.ToString();
            combo_StopBits.SelectedItem = Common.mainForm.serialPort1.StopBits.ToString();
            combo_Handshake.SelectedItem = Common.mainForm.serialPort1.Handshake.ToString();
        }

        /// <summary>
        /// Function used to check that all ComboBoxes are filled.
        /// </summary>
        /// <param name="whichComBoxIsEmpty"></param>
        /// <returns></returns>
        private bool AreAllComboBoxesFilled(out string whichComBoxIsEmpty)
        {
            //Initialize the 'whichComBoxIsEmpty' variable
            whichComBoxIsEmpty = string.Empty;
            //Check if any combo box is empty
            foreach (ComboBox comboBox in Controls.OfType<ComboBox>())
            {
                if (string.IsNullOrEmpty(comboBox.Text.ToString()))
                {
                    //Get the combo box which is empty
                    whichComBoxIsEmpty = comboBox.Name;
                    //Exit function
                    return false;
                }
                Application.DoEvents(); //refresh GUI
            }
            return true;
        }
        #endregion Other Functions

    }
}