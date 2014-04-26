namespace BulkMkvMuxer
{
    partial class Options
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
            this.labelJavaPath = new System.Windows.Forms.Label();
            this.labelMkvExtractPath = new System.Windows.Forms.Label();
            this.labelMkvInfoPath = new System.Windows.Forms.Label();
            this.labelMkvMergePath = new System.Windows.Forms.Label();
            this.textBoxJavaPath = new System.Windows.Forms.TextBox();
            this.textBoxMkvExtractPath = new System.Windows.Forms.TextBox();
            this.textBoxMkvInfoPath = new System.Windows.Forms.TextBox();
            this.textBoxMkvMergePath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonAddMkvMergePath = new System.Windows.Forms.Button();
            this.buttonAddMkvInfoPath = new System.Windows.Forms.Button();
            this.buttonAddMkvExtractPath = new System.Windows.Forms.Button();
            this.buttonAddJavaPath = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelJavaPath
            // 
            this.labelJavaPath.AutoSize = true;
            this.labelJavaPath.Location = new System.Drawing.Point(6, 16);
            this.labelJavaPath.Name = "labelJavaPath";
            this.labelJavaPath.Size = new System.Drawing.Size(33, 13);
            this.labelJavaPath.TabIndex = 0;
            this.labelJavaPath.Text = "Java:";
            // 
            // labelMkvExtractPath
            // 
            this.labelMkvExtractPath.AutoSize = true;
            this.labelMkvExtractPath.Location = new System.Drawing.Point(6, 55);
            this.labelMkvExtractPath.Name = "labelMkvExtractPath";
            this.labelMkvExtractPath.Size = new System.Drawing.Size(64, 13);
            this.labelMkvExtractPath.TabIndex = 1;
            this.labelMkvExtractPath.Text = "MkvExtract:";
            // 
            // labelMkvInfoPath
            // 
            this.labelMkvInfoPath.AutoSize = true;
            this.labelMkvInfoPath.Location = new System.Drawing.Point(6, 94);
            this.labelMkvInfoPath.Name = "labelMkvInfoPath";
            this.labelMkvInfoPath.Size = new System.Drawing.Size(49, 13);
            this.labelMkvInfoPath.TabIndex = 2;
            this.labelMkvInfoPath.Text = "MkvInfo:";
            // 
            // labelMkvMergePath
            // 
            this.labelMkvMergePath.AutoSize = true;
            this.labelMkvMergePath.Location = new System.Drawing.Point(6, 133);
            this.labelMkvMergePath.Name = "labelMkvMergePath";
            this.labelMkvMergePath.Size = new System.Drawing.Size(61, 13);
            this.labelMkvMergePath.TabIndex = 3;
            this.labelMkvMergePath.Text = "MkvMerge:";
            // 
            // textBoxJavaPath
            // 
            this.textBoxJavaPath.Location = new System.Drawing.Point(9, 32);
            this.textBoxJavaPath.Name = "textBoxJavaPath";
            this.textBoxJavaPath.Size = new System.Drawing.Size(299, 20);
            this.textBoxJavaPath.TabIndex = 4;
            // 
            // textBoxMkvExtractPath
            // 
            this.textBoxMkvExtractPath.Location = new System.Drawing.Point(9, 71);
            this.textBoxMkvExtractPath.Name = "textBoxMkvExtractPath";
            this.textBoxMkvExtractPath.Size = new System.Drawing.Size(299, 20);
            this.textBoxMkvExtractPath.TabIndex = 5;
            // 
            // textBoxMkvInfoPath
            // 
            this.textBoxMkvInfoPath.Location = new System.Drawing.Point(9, 110);
            this.textBoxMkvInfoPath.Name = "textBoxMkvInfoPath";
            this.textBoxMkvInfoPath.Size = new System.Drawing.Size(299, 20);
            this.textBoxMkvInfoPath.TabIndex = 6;
            // 
            // textBoxMkvMergePath
            // 
            this.textBoxMkvMergePath.Location = new System.Drawing.Point(9, 149);
            this.textBoxMkvMergePath.Name = "textBoxMkvMergePath";
            this.textBoxMkvMergePath.Size = new System.Drawing.Size(299, 20);
            this.textBoxMkvMergePath.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonAddMkvMergePath);
            this.groupBox1.Controls.Add(this.buttonAddMkvInfoPath);
            this.groupBox1.Controls.Add(this.buttonAddMkvExtractPath);
            this.groupBox1.Controls.Add(this.buttonAddJavaPath);
            this.groupBox1.Controls.Add(this.labelJavaPath);
            this.groupBox1.Controls.Add(this.textBoxMkvMergePath);
            this.groupBox1.Controls.Add(this.labelMkvExtractPath);
            this.groupBox1.Controls.Add(this.textBoxMkvInfoPath);
            this.groupBox1.Controls.Add(this.labelMkvInfoPath);
            this.groupBox1.Controls.Add(this.textBoxMkvExtractPath);
            this.groupBox1.Controls.Add(this.labelMkvMergePath);
            this.groupBox1.Controls.Add(this.textBoxJavaPath);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 185);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "External Program Paths";
            // 
            // buttonAddMkvMergePath
            // 
            this.buttonAddMkvMergePath.Location = new System.Drawing.Point(314, 147);
            this.buttonAddMkvMergePath.Name = "buttonAddMkvMergePath";
            this.buttonAddMkvMergePath.Size = new System.Drawing.Size(25, 23);
            this.buttonAddMkvMergePath.TabIndex = 11;
            this.buttonAddMkvMergePath.Text = "+";
            this.buttonAddMkvMergePath.UseVisualStyleBackColor = true;
            this.buttonAddMkvMergePath.Click += new System.EventHandler(this.buttonAddMkvMergePath_Click);
            // 
            // buttonAddMkvInfoPath
            // 
            this.buttonAddMkvInfoPath.Location = new System.Drawing.Point(314, 108);
            this.buttonAddMkvInfoPath.Name = "buttonAddMkvInfoPath";
            this.buttonAddMkvInfoPath.Size = new System.Drawing.Size(25, 23);
            this.buttonAddMkvInfoPath.TabIndex = 10;
            this.buttonAddMkvInfoPath.Text = "+";
            this.buttonAddMkvInfoPath.UseVisualStyleBackColor = true;
            this.buttonAddMkvInfoPath.Click += new System.EventHandler(this.buttonAddMkvInfoPath_Click);
            // 
            // buttonAddMkvExtractPath
            // 
            this.buttonAddMkvExtractPath.Location = new System.Drawing.Point(314, 69);
            this.buttonAddMkvExtractPath.Name = "buttonAddMkvExtractPath";
            this.buttonAddMkvExtractPath.Size = new System.Drawing.Size(25, 23);
            this.buttonAddMkvExtractPath.TabIndex = 9;
            this.buttonAddMkvExtractPath.Text = "+";
            this.buttonAddMkvExtractPath.UseVisualStyleBackColor = true;
            this.buttonAddMkvExtractPath.Click += new System.EventHandler(this.buttonAddMkvExtractPath_Click);
            // 
            // buttonAddJavaPath
            // 
            this.buttonAddJavaPath.Location = new System.Drawing.Point(314, 30);
            this.buttonAddJavaPath.Name = "buttonAddJavaPath";
            this.buttonAddJavaPath.Size = new System.Drawing.Size(25, 23);
            this.buttonAddJavaPath.TabIndex = 8;
            this.buttonAddJavaPath.Text = "+";
            this.buttonAddJavaPath.UseVisualStyleBackColor = true;
            this.buttonAddJavaPath.Click += new System.EventHandler(this.buttonAddJavaPath_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(210, 203);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(291, 203);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 242);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Options";
            this.Text = "Options";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelJavaPath;
        private System.Windows.Forms.Label labelMkvExtractPath;
        private System.Windows.Forms.Label labelMkvInfoPath;
        private System.Windows.Forms.Label labelMkvMergePath;
        private System.Windows.Forms.TextBox textBoxJavaPath;
        private System.Windows.Forms.TextBox textBoxMkvExtractPath;
        private System.Windows.Forms.TextBox textBoxMkvInfoPath;
        private System.Windows.Forms.TextBox textBoxMkvMergePath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonAddMkvMergePath;
        private System.Windows.Forms.Button buttonAddMkvInfoPath;
        private System.Windows.Forms.Button buttonAddMkvExtractPath;
        private System.Windows.Forms.Button buttonAddJavaPath;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
    }
}