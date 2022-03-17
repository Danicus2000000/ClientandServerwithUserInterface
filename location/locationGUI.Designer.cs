
namespace location
{
    partial class locationGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.requesttype_lbl = new System.Windows.Forms.Label();
            this.requesttype_cmb = new System.Windows.Forms.ComboBox();
            this.host_lbl = new System.Windows.Forms.Label();
            this.host_txt = new System.Windows.Forms.TextBox();
            this.port_lbl = new System.Windows.Forms.Label();
            this.port_txt = new System.Windows.Forms.TextBox();
            this.timeout_lbl = new System.Windows.Forms.Label();
            this.timeout_txt = new System.Windows.Forms.TextBox();
            this.name_lbl = new System.Windows.Forms.Label();
            this.name_txt = new System.Windows.Forms.TextBox();
            this.response_lbl = new System.Windows.Forms.Label();
            this.response_rtb = new System.Windows.Forms.RichTextBox();
            this.location_lbl = new System.Windows.Forms.Label();
            this.location_txt = new System.Windows.Forms.TextBox();
            this.sendrequest_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // requesttype_lbl
            // 
            this.requesttype_lbl.AutoSize = true;
            this.requesttype_lbl.Location = new System.Drawing.Point(36, 64);
            this.requesttype_lbl.Name = "requesttype_lbl";
            this.requesttype_lbl.Size = new System.Drawing.Size(79, 15);
            this.requesttype_lbl.TabIndex = 0;
            this.requesttype_lbl.Text = "Request Type:";
            // 
            // requesttype_cmb
            // 
            this.requesttype_cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.requesttype_cmb.FormattingEnabled = true;
            this.requesttype_cmb.Items.AddRange(new object[] {
            "WhoIs",
            "HTTP 0.9",
            "HTTP 1.0",
            "HTTP 1.1"});
            this.requesttype_cmb.Location = new System.Drawing.Point(121, 61);
            this.requesttype_cmb.Name = "requesttype_cmb";
            this.requesttype_cmb.Size = new System.Drawing.Size(121, 23);
            this.requesttype_cmb.TabIndex = 1;
            // 
            // host_lbl
            // 
            this.host_lbl.AutoSize = true;
            this.host_lbl.Location = new System.Drawing.Point(80, 113);
            this.host_lbl.Name = "host_lbl";
            this.host_lbl.Size = new System.Drawing.Size(35, 15);
            this.host_lbl.TabIndex = 2;
            this.host_lbl.Text = "Host:";
            // 
            // host_txt
            // 
            this.host_txt.Location = new System.Drawing.Point(121, 110);
            this.host_txt.Name = "host_txt";
            this.host_txt.Size = new System.Drawing.Size(121, 23);
            this.host_txt.TabIndex = 3;
            this.host_txt.Text = "whois.hull.ac.uk";
            // 
            // port_lbl
            // 
            this.port_lbl.AutoSize = true;
            this.port_lbl.Location = new System.Drawing.Point(83, 159);
            this.port_lbl.Name = "port_lbl";
            this.port_lbl.Size = new System.Drawing.Size(32, 15);
            this.port_lbl.TabIndex = 4;
            this.port_lbl.Text = "Port:";
            // 
            // port_txt
            // 
            this.port_txt.Location = new System.Drawing.Point(121, 156);
            this.port_txt.Name = "port_txt";
            this.port_txt.Size = new System.Drawing.Size(121, 23);
            this.port_txt.TabIndex = 5;
            this.port_txt.Text = "43";
            // 
            // timeout_lbl
            // 
            this.timeout_lbl.AutoSize = true;
            this.timeout_lbl.Location = new System.Drawing.Point(61, 208);
            this.timeout_lbl.Name = "timeout_lbl";
            this.timeout_lbl.Size = new System.Drawing.Size(54, 15);
            this.timeout_lbl.TabIndex = 6;
            this.timeout_lbl.Text = "Timeout:";
            // 
            // timeout_txt
            // 
            this.timeout_txt.Location = new System.Drawing.Point(121, 205);
            this.timeout_txt.Name = "timeout_txt";
            this.timeout_txt.Size = new System.Drawing.Size(121, 23);
            this.timeout_txt.TabIndex = 7;
            this.timeout_txt.Text = "1000";
            // 
            // name_lbl
            // 
            this.name_lbl.AutoSize = true;
            this.name_lbl.Location = new System.Drawing.Point(73, 253);
            this.name_lbl.Name = "name_lbl";
            this.name_lbl.Size = new System.Drawing.Size(42, 15);
            this.name_lbl.TabIndex = 8;
            this.name_lbl.Text = "Name:";
            // 
            // name_txt
            // 
            this.name_txt.Location = new System.Drawing.Point(121, 250);
            this.name_txt.Name = "name_txt";
            this.name_txt.Size = new System.Drawing.Size(121, 23);
            this.name_txt.TabIndex = 9;
            // 
            // response_lbl
            // 
            this.response_lbl.AutoSize = true;
            this.response_lbl.Location = new System.Drawing.Point(447, 47);
            this.response_lbl.Name = "response_lbl";
            this.response_lbl.Size = new System.Drawing.Size(60, 15);
            this.response_lbl.TabIndex = 10;
            this.response_lbl.Text = "Response:";
            // 
            // response_rtb
            // 
            this.response_rtb.Enabled = false;
            this.response_rtb.Location = new System.Drawing.Point(447, 64);
            this.response_rtb.Name = "response_rtb";
            this.response_rtb.ReadOnly = true;
            this.response_rtb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.response_rtb.Size = new System.Drawing.Size(341, 248);
            this.response_rtb.TabIndex = 11;
            this.response_rtb.Text = "";
            // 
            // location_lbl
            // 
            this.location_lbl.AutoSize = true;
            this.location_lbl.Location = new System.Drawing.Point(59, 292);
            this.location_lbl.Name = "location_lbl";
            this.location_lbl.Size = new System.Drawing.Size(56, 15);
            this.location_lbl.TabIndex = 12;
            this.location_lbl.Text = "Location:";
            // 
            // location_txt
            // 
            this.location_txt.Location = new System.Drawing.Point(121, 289);
            this.location_txt.Name = "location_txt";
            this.location_txt.Size = new System.Drawing.Size(121, 23);
            this.location_txt.TabIndex = 13;
            // 
            // sendrequest_btn
            // 
            this.sendrequest_btn.Location = new System.Drawing.Point(267, 289);
            this.sendrequest_btn.Name = "sendrequest_btn";
            this.sendrequest_btn.Size = new System.Drawing.Size(102, 23);
            this.sendrequest_btn.TabIndex = 14;
            this.sendrequest_btn.Text = "Send Request";
            this.sendrequest_btn.UseVisualStyleBackColor = true;
            this.sendrequest_btn.Click += new System.EventHandler(this.sendrequest_btn_Click);
            // 
            // locationGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(800, 323);
            this.Controls.Add(this.sendrequest_btn);
            this.Controls.Add(this.location_txt);
            this.Controls.Add(this.location_lbl);
            this.Controls.Add(this.response_rtb);
            this.Controls.Add(this.response_lbl);
            this.Controls.Add(this.name_txt);
            this.Controls.Add(this.name_lbl);
            this.Controls.Add(this.timeout_txt);
            this.Controls.Add(this.timeout_lbl);
            this.Controls.Add(this.port_txt);
            this.Controls.Add(this.port_lbl);
            this.Controls.Add(this.host_txt);
            this.Controls.Add(this.host_lbl);
            this.Controls.Add(this.requesttype_cmb);
            this.Controls.Add(this.requesttype_lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "locationGUI";
            this.Text = "Location Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label requesttype_lbl;
        private System.Windows.Forms.ComboBox requesttype_cmb;
        private System.Windows.Forms.Label host_lbl;
        private System.Windows.Forms.TextBox host_txt;
        private System.Windows.Forms.Label port_lbl;
        private System.Windows.Forms.TextBox port_txt;
        private System.Windows.Forms.Label timeout_lbl;
        private System.Windows.Forms.TextBox timeout_txt;
        private System.Windows.Forms.Label name_lbl;
        private System.Windows.Forms.TextBox name_txt;
        private System.Windows.Forms.Label response_lbl;
        private System.Windows.Forms.RichTextBox response_rtb;
        private System.Windows.Forms.Label location_lbl;
        private System.Windows.Forms.TextBox location_txt;
        private System.Windows.Forms.Button sendrequest_btn;
    }
}