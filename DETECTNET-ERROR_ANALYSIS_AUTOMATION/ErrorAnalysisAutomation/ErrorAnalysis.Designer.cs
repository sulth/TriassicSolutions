namespace ErrorDigits
{
    partial class ErrorAnalysis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorAnalysis));
            this.label1 = new System.Windows.Forms.Label();
            this.txtJobId = new System.Windows.Forms.TextBox();
            this.txtImageFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCheckError = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.lblJobIdNote = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Job Id *";
            // 
            // txtJobId
            // 
            this.txtJobId.Location = new System.Drawing.Point(279, 34);
            this.txtJobId.Name = "txtJobId";
            this.txtJobId.Size = new System.Drawing.Size(250, 20);
            this.txtJobId.TabIndex = 0;
            // 
            // txtImageFolder
            // 
            this.txtImageFolder.Location = new System.Drawing.Point(93, 74);
            this.txtImageFolder.Name = "txtImageFolder";
            this.txtImageFolder.ReadOnly = true;
            this.txtImageFolder.Size = new System.Drawing.Size(354, 20);
            this.txtImageFolder.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Image Folder *";
            // 
            // btnCheckError
            // 
            this.btnCheckError.BackColor = System.Drawing.Color.Teal;
            this.btnCheckError.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheckError.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCheckError.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckError.ForeColor = System.Drawing.Color.White;
            this.btnCheckError.Location = new System.Drawing.Point(390, 112);
            this.btnCheckError.Name = "btnCheckError";
            this.btnCheckError.Size = new System.Drawing.Size(139, 26);
            this.btnCheckError.TabIndex = 3;
            this.btnCheckError.Text = "Analyse";
            this.btnCheckError.UseVisualStyleBackColor = false;
            this.btnCheckError.Click += new System.EventHandler(this.btnCheckError_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOutput);
            this.groupBox1.Location = new System.Drawing.Point(12, 196);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 348);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // txtOutput
            // 
            this.txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOutput.Location = new System.Drawing.Point(6, 18);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(534, 316);
            this.txtOutput.TabIndex = 10;
            this.txtOutput.Text = "";
            this.txtOutput.TextChanged += new System.EventHandler(this.txtOutput_TextChanged);
            // 
            // btnChoose
            // 
            this.btnChoose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChoose.Location = new System.Drawing.Point(454, 73);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(75, 23);
            this.btnChoose.TabIndex = 2;
            this.btnChoose.Text = "Choose...";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblJobIdNote);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtIPAddress);
            this.groupBox2.Controls.Add(this.txtImageFolder);
            this.groupBox2.Controls.Add(this.btnChoose);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtJobId);
            this.groupBox2.Controls.Add(this.btnCheckError);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(544, 171);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Error Analysis Automation";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP Address *";
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(94, 33);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(122, 20);
            this.txtIPAddress.TabIndex = 6;
            this.txtIPAddress.Text = "192.168.2.60";
            // 
            // lblJobIdNote
            // 
            this.lblJobIdNote.AutoSize = true;
            this.lblJobIdNote.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblJobIdNote.Location = new System.Drawing.Point(12, 141);
            this.lblJobIdNote.Name = "lblJobIdNote";
            this.lblJobIdNote.Size = new System.Drawing.Size(359, 13);
            this.lblJobIdNote.TabIndex = 7;
            this.lblJobIdNote.Text = "Note : Job-ID (DIGITS) name, in the “jobs” folder found under “digits” folder";
            // 
            // ErrorAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 555);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ErrorAnalysis";
            this.Text = "Error Analysis Automation";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtJobId;
        private System.Windows.Forms.TextBox txtImageFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCheckError;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label lblJobIdNote;
    }
}

