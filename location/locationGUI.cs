using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace location
{
    public partial class locationGUI : Form
    {
        public locationGUI()
        {
            InitializeComponent();
            requesttype_cmb.SelectedIndex = 0;
        }
        private static int port = 43;
        private static int timeout = 1000;
        private void sendrequest_btn_Click(object sender, EventArgs e)
        {
            if(port_txt.Text=="") 
            {
                port_txt.Text = "43";
            }
            if (timeout_txt.Text == "") 
            {
                timeout_txt.Text = "1000";
            }
            if(int.TryParse(port_txt.Text,out port) && int.TryParse(timeout_txt.Text,out timeout) && host_txt.Text!="")// && name_txt.Text!="") 
            {
                this.DialogResult = DialogResult.OK;
                Application.Exit();
            }
        }
    }
}
