
namespace locationserver
{
    partial class locationserverGUI
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
            this.logfilelocation_lbl = new System.Windows.Forms.Label();
            this.logfilelocation_txt = new System.Windows.Forms.TextBox();
            this.databasefilelocation_lbl = new System.Windows.Forms.Label();
            this.databasefilelocation_txt = new System.Windows.Forms.TextBox();
            this.timeout_lbl = new System.Windows.Forms.Label();
            this.timeout_txt = new System.Windows.Forms.TextBox();
            this.startserver_txt = new System.Windows.Forms.Button();
            this.log_rtb = new System.Windows.Forms.RichTextBox();
            this.log_lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // logfilelocation_lbl
            // 
            this.logfilelocation_lbl.AutoSize = true;
            this.logfilelocation_lbl.Location = new System.Drawing.Point(12, 85);
            this.logfilelocation_lbl.Name = "logfilelocation_lbl";
            this.logfilelocation_lbl.Size = new System.Drawing.Size(100, 15);
            this.logfilelocation_lbl.TabIndex = 0;
            this.logfilelocation_lbl.Text = "Log File Location:";
            // 
            // logfilelocation_txt
            // 
            this.logfilelocation_txt.Location = new System.Drawing.Point(118, 82);
            this.logfilelocation_txt.Name = "logfilelocation_txt";
            this.logfilelocation_txt.Size = new System.Drawing.Size(343, 23);
            this.logfilelocation_txt.TabIndex = 1;
            // 
            // databasefilelocation_lbl
            // 
            this.databasefilelocation_lbl.AutoSize = true;
            this.databasefilelocation_lbl.Location = new System.Drawing.Point(3, 136);
            this.databasefilelocation_lbl.Name = "databasefilelocation_lbl";
            this.databasefilelocation_lbl.Size = new System.Drawing.Size(128, 15);
            this.databasefilelocation_lbl.TabIndex = 2;
            this.databasefilelocation_lbl.Text = "Database File Location:";
            // 
            // databasefilelocation_txt
            // 
            this.databasefilelocation_txt.Location = new System.Drawing.Point(137, 133);
            this.databasefilelocation_txt.Name = "databasefilelocation_txt";
            this.databasefilelocation_txt.Size = new System.Drawing.Size(324, 23);
            this.databasefilelocation_txt.TabIndex = 3;
            // 
            // timeout_lbl
            // 
            this.timeout_lbl.AutoSize = true;
            this.timeout_lbl.Location = new System.Drawing.Point(77, 175);
            this.timeout_lbl.Name = "timeout_lbl";
            this.timeout_lbl.Size = new System.Drawing.Size(54, 15);
            this.timeout_lbl.TabIndex = 4;
            this.timeout_lbl.Text = "Timeout:";
            // 
            // timeout_txt
            // 
            this.timeout_txt.Location = new System.Drawing.Point(137, 175);
            this.timeout_txt.Name = "timeout_txt";
            this.timeout_txt.Size = new System.Drawing.Size(324, 23);
            this.timeout_txt.TabIndex = 5;
            // 
            // startserver_txt
            // 
            this.startserver_txt.Location = new System.Drawing.Point(244, 204);
            this.startserver_txt.Name = "startserver_txt";
            this.startserver_txt.Size = new System.Drawing.Size(83, 27);
            this.startserver_txt.TabIndex = 6;
            this.startserver_txt.Text = "Start Server";
            this.startserver_txt.UseVisualStyleBackColor = true;
            this.startserver_txt.Click += new System.EventHandler(this.startserver_txt_Click);
            // 
            // log_rtb
            // 
            this.log_rtb.Enabled = false;
            this.log_rtb.Location = new System.Drawing.Point(519, 82);
            this.log_rtb.Name = "log_rtb";
            this.log_rtb.ReadOnly = true;
            this.log_rtb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.log_rtb.Size = new System.Drawing.Size(269, 288);
            this.log_rtb.TabIndex = 7;
            this.log_rtb.Text = "";
            // 
            // log_lbl
            // 
            this.log_lbl.AutoSize = true;
            this.log_lbl.Location = new System.Drawing.Point(519, 64);
            this.log_lbl.Name = "log_lbl";
            this.log_lbl.Size = new System.Drawing.Size(30, 15);
            this.log_lbl.TabIndex = 8;
            this.log_lbl.Text = "Log:";
            // 
            // locationserverGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.log_lbl);
            this.Controls.Add(this.log_rtb);
            this.Controls.Add(this.startserver_txt);
            this.Controls.Add(this.timeout_txt);
            this.Controls.Add(this.timeout_lbl);
            this.Controls.Add(this.databasefilelocation_txt);
            this.Controls.Add(this.databasefilelocation_lbl);
            this.Controls.Add(this.logfilelocation_txt);
            this.Controls.Add(this.logfilelocation_lbl);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "locationserverGUI";
            this.Text = "Location Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label logfilelocation_lbl;
        private System.Windows.Forms.TextBox logfilelocation_txt;
        private System.Windows.Forms.Label databasefilelocation_lbl;
        private System.Windows.Forms.TextBox databasefilelocation_txt;
        private System.Windows.Forms.Label timeout_lbl;
        private System.Windows.Forms.TextBox timeout_txt;
        private System.Windows.Forms.Button startserver_txt;
        private System.Windows.Forms.RichTextBox log_rtb;
        private System.Windows.Forms.Label log_lbl;
    }
}