namespace LabelConversionTool
{
    partial class ConversionTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConversionTool));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFolderName = new System.Windows.Forms.TextBox();
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnConverter = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutputRootFolder = new System.Windows.Forms.TextBox();
            this.btnChooseOpFol = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnChooseOpFol);
            this.groupBox2.Controls.Add(this.txtOutputRootFolder);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtFolderName);
            this.groupBox2.Controls.Add(this.txtRootFolder);
            this.groupBox2.Controls.Add(this.btnChoose);
            this.groupBox2.Controls.Add(this.btnConverter);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(14, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(551, 167);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Label Conversion";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output Folder *";
            // 
            // txtFolderName
            // 
            this.txtFolderName.Location = new System.Drawing.Point(122, 91);
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.Size = new System.Drawing.Size(407, 20);
            this.txtFolderName.TabIndex = 6;
            this.txtFolderName.Text = "VOC2007";
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Location = new System.Drawing.Point(122, 25);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.ReadOnly = true;
            this.txtRootFolder.Size = new System.Drawing.Size(325, 20);
            this.txtRootFolder.TabIndex = 1;
            // 
            // btnChoose
            // 
            this.btnChoose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChoose.Location = new System.Drawing.Point(454, 24);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(75, 23);
            this.btnChoose.TabIndex = 2;
            this.btnChoose.Text = "Choose...";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // btnConverter
            // 
            this.btnConverter.BackColor = System.Drawing.Color.SteelBlue;
            this.btnConverter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConverter.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnConverter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConverter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConverter.ForeColor = System.Drawing.Color.White;
            this.btnConverter.Location = new System.Drawing.Point(390, 121);
            this.btnConverter.Name = "btnConverter";
            this.btnConverter.Size = new System.Drawing.Size(139, 26);
            this.btnConverter.TabIndex = 3;
            this.btnConverter.Text = "Convert";
            this.btnConverter.UseVisualStyleBackColor = false;
            this.btnConverter.Click += new System.EventHandler(this.btnConverter_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Input Root Folder *";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Output Root Folder *";
            // 
            // txtOutputRootFolder
            // 
            this.txtOutputRootFolder.Location = new System.Drawing.Point(122, 58);
            this.txtOutputRootFolder.Name = "txtOutputRootFolder";
            this.txtOutputRootFolder.ReadOnly = true;
            this.txtOutputRootFolder.Size = new System.Drawing.Size(325, 20);
            this.txtOutputRootFolder.TabIndex = 8;
            // 
            // btnChooseOpFol
            // 
            this.btnChooseOpFol.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChooseOpFol.Location = new System.Drawing.Point(454, 56);
            this.btnChooseOpFol.Name = "btnChooseOpFol";
            this.btnChooseOpFol.Size = new System.Drawing.Size(75, 23);
            this.btnChooseOpFol.TabIndex = 9;
            this.btnChooseOpFol.Text = "Choose...";
            this.btnChooseOpFol.UseVisualStyleBackColor = true;
            this.btnChooseOpFol.Click += new System.EventHandler(this.btnChooseOpFol_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOutput);
            this.groupBox1.Location = new System.Drawing.Point(14, 187);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(551, 348);
            this.groupBox1.TabIndex = 10;
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
            // ConversionTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 549);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConversionTool";
            this.Text = "Label Conversion Tool";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFolderName;
        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.Button btnConverter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChooseOpFol;
        private System.Windows.Forms.TextBox txtOutputRootFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox txtOutput;
    }
}

