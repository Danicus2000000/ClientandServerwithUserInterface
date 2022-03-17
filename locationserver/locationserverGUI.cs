using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace locationserver
{
    public partial class locationserverGUI : Form
    {
        public locationserverGUI(string loglocation,string databaselocation,int timeout)
        {
            InitializeComponent();
            logfilelocation_txt.Text = loglocation;
            databasefilelocation_txt.Text = databaselocation;
            timeout_txt.Text = timeout.ToString();
        }

        private void startserver_txt_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Application.Exit();
        }
    }
}
