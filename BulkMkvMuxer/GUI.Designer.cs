namespace BulkMkvMuxer
{
    partial class GUI
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
            this.components = new System.ComponentModel.Container();
            this.buttonAddDirectory = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonMux = new System.Windows.Forms.Button();
            this.groupBoxMux = new System.Windows.Forms.GroupBox();
            this.radioButtonKeepChecked = new System.Windows.Forms.RadioButton();
            this.radioButtonKeepForced = new System.Windows.Forms.RadioButton();
            this.radioButtonRemoveAll = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTrackToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.oPenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addTrackToFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonClear = new System.Windows.Forms.Button();
            this.groupBoxMux.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAddDirectory
            // 
            this.buttonAddDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddDirectory.Location = new System.Drawing.Point(343, 659);
            this.buttonAddDirectory.Name = "buttonAddDirectory";
            this.buttonAddDirectory.Size = new System.Drawing.Size(90, 23);
            this.buttonAddDirectory.TabIndex = 1;
            this.buttonAddDirectory.Text = "Add Directory";
            this.buttonAddDirectory.UseVisualStyleBackColor = true;
            this.buttonAddDirectory.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonMux
            // 
            this.buttonMux.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMux.Location = new System.Drawing.Point(262, 659);
            this.buttonMux.Name = "buttonMux";
            this.buttonMux.Size = new System.Drawing.Size(75, 23);
            this.buttonMux.TabIndex = 3;
            this.buttonMux.Text = "Mux!";
            this.buttonMux.UseVisualStyleBackColor = true;
            this.buttonMux.Click += new System.EventHandler(this.buttonMux_Click);
            // 
            // groupBoxMux
            // 
            this.groupBoxMux.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMux.Controls.Add(this.radioButtonKeepChecked);
            this.groupBoxMux.Controls.Add(this.radioButtonKeepForced);
            this.groupBoxMux.Controls.Add(this.radioButtonRemoveAll);
            this.groupBoxMux.Location = new System.Drawing.Point(12, 560);
            this.groupBoxMux.Name = "groupBoxMux";
            this.groupBoxMux.Size = new System.Drawing.Size(421, 93);
            this.groupBoxMux.TabIndex = 4;
            this.groupBoxMux.TabStop = false;
            this.groupBoxMux.Text = "Mux Options";
            // 
            // radioButtonKeepChecked
            // 
            this.radioButtonKeepChecked.AutoSize = true;
            this.radioButtonKeepChecked.Location = new System.Drawing.Point(6, 65);
            this.radioButtonKeepChecked.Name = "radioButtonKeepChecked";
            this.radioButtonKeepChecked.Size = new System.Drawing.Size(205, 17);
            this.radioButtonKeepChecked.TabIndex = 6;
            this.radioButtonKeepChecked.TabStop = true;
            this.radioButtonKeepChecked.Text = "Keep only the checked media streams";
            this.radioButtonKeepChecked.UseVisualStyleBackColor = true;
            // 
            // radioButtonKeepForced
            // 
            this.radioButtonKeepForced.AutoSize = true;
            this.radioButtonKeepForced.Location = new System.Drawing.Point(6, 42);
            this.radioButtonKeepForced.Name = "radioButtonKeepForced";
            this.radioButtonKeepForced.Size = new System.Drawing.Size(329, 17);
            this.radioButtonKeepForced.TabIndex = 5;
            this.radioButtonKeepForced.TabStop = true;
            this.radioButtonKeepForced.Text = "Keep only forced subtitles and the most appropriate audio stream";
            this.radioButtonKeepForced.UseVisualStyleBackColor = true;
            // 
            // radioButtonRemoveAll
            // 
            this.radioButtonRemoveAll.AutoSize = true;
            this.radioButtonRemoveAll.Location = new System.Drawing.Point(6, 19);
            this.radioButtonRemoveAll.Name = "radioButtonRemoveAll";
            this.radioButtonRemoveAll.Size = new System.Drawing.Size(311, 17);
            this.radioButtonRemoveAll.TabIndex = 4;
            this.radioButtonRemoveAll.TabStop = true;
            this.radioButtonRemoveAll.Text = "Remove all subtitles, keep the most appropriate audio stream";
            this.radioButtonRemoveAll.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 685);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(445, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.CheckBoxes = true;
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Location = new System.Drawing.Point(12, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(420, 527);
            this.treeView1.TabIndex = 6;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.openToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem,
            this.addTrackToFileToolStripMenuItem,
            this.defaultToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 136);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.selectNoneToolStripMenuItem_Click);
            // 
            // addTrackToFileToolStripMenuItem
            // 
            this.addTrackToFileToolStripMenuItem.Name = "addTrackToFileToolStripMenuItem";
            this.addTrackToFileToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addTrackToFileToolStripMenuItem.Text = "Add Track to File...";
            this.addTrackToFileToolStripMenuItem.Click += new System.EventHandler(this.addTrackToFileToolStripMenuItem_Click);
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.CheckOnClick = true;
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.defaultToolStripMenuItem.Text = "Default";
            this.defaultToolStripMenuItem.Click += new System.EventHandler(this.defaultToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(445, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.muxToolStripMenuItem,
            this.addFileToolStripMenuItem,
            this.addDirectoryToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // muxToolStripMenuItem
            // 
            this.muxToolStripMenuItem.Name = "muxToolStripMenuItem";
            this.muxToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.muxToolStripMenuItem.Text = "Mux!";
            this.muxToolStripMenuItem.Click += new System.EventHandler(this.muxToolStripMenuItem_Click);
            // 
            // addFileToolStripMenuItem
            // 
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.addFileToolStripMenuItem.Text = "Add Files...";
            this.addFileToolStripMenuItem.Click += new System.EventHandler(this.addFileToolStripMenuItem_Click);
            // 
            // addDirectoryToolStripMenuItem
            // 
            this.addDirectoryToolStripMenuItem.Name = "addDirectoryToolStripMenuItem";
            this.addDirectoryToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.addDirectoryToolStripMenuItem.Text = "Add Directory...";
            this.addDirectoryToolStripMenuItem.Click += new System.EventHandler(this.addDirectoryToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem1,
            this.oPenToolStripMenuItem1,
            this.selectAllToolStripMenuItem1,
            this.selectNoneToolStripMenuItem1,
            this.addTrackToFileToolStripMenuItem1,
            this.defaultToolStripMenuItem1,
            this.optionsToolStripMenuItem1});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.DropDownOpening += new System.EventHandler(this.editToolStripMenuItem_DropDownOpening);
            // 
            // playToolStripMenuItem1
            // 
            this.playToolStripMenuItem1.Name = "playToolStripMenuItem1";
            this.playToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.playToolStripMenuItem1.Text = "Play";
            this.playToolStripMenuItem1.Click += new System.EventHandler(this.playToolStripMenuItem1_Click);
            // 
            // oPenToolStripMenuItem1
            // 
            this.oPenToolStripMenuItem1.Name = "oPenToolStripMenuItem1";
            this.oPenToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.oPenToolStripMenuItem1.Text = "Open";
            this.oPenToolStripMenuItem1.Click += new System.EventHandler(this.oPenToolStripMenuItem1_Click);
            // 
            // selectAllToolStripMenuItem1
            // 
            this.selectAllToolStripMenuItem1.Name = "selectAllToolStripMenuItem1";
            this.selectAllToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.selectAllToolStripMenuItem1.Text = "Select All";
            this.selectAllToolStripMenuItem1.Click += new System.EventHandler(this.selectAllToolStripMenuItem1_Click);
            // 
            // selectNoneToolStripMenuItem1
            // 
            this.selectNoneToolStripMenuItem1.Name = "selectNoneToolStripMenuItem1";
            this.selectNoneToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.selectNoneToolStripMenuItem1.Text = "Select None";
            this.selectNoneToolStripMenuItem1.Click += new System.EventHandler(this.selectNoneToolStripMenuItem1_Click);
            // 
            // addTrackToFileToolStripMenuItem1
            // 
            this.addTrackToFileToolStripMenuItem1.Name = "addTrackToFileToolStripMenuItem1";
            this.addTrackToFileToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.addTrackToFileToolStripMenuItem1.Text = "Add Track to File...";
            // 
            // defaultToolStripMenuItem1
            // 
            this.defaultToolStripMenuItem1.CheckOnClick = true;
            this.defaultToolStripMenuItem1.Name = "defaultToolStripMenuItem1";
            this.defaultToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.defaultToolStripMenuItem1.Text = "Default";
            this.defaultToolStripMenuItem1.Click += new System.EventHandler(this.defaultToolStripMenuItem1_Click);
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.optionsToolStripMenuItem1.Text = "Options...";
            this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLogToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.viewLogToolStripMenuItem.Text = "View Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(181, 659);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 8;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 707);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.groupBoxMux);
            this.Controls.Add(this.buttonAddDirectory);
            this.Controls.Add(this.buttonMux);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GUI";
            this.Text = "Bulk MKV Muxer 1.15";
            this.groupBoxMux.ResumeLayout(false);
            this.groupBoxMux.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button buttonMux;
        private System.Windows.Forms.GroupBox groupBoxMux;
        private System.Windows.Forms.RadioButton radioButtonKeepForced;
        private System.Windows.Forms.RadioButton radioButtonRemoveAll;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.RadioButton radioButtonKeepChecked;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.ToolStripMenuItem addTrackToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem oPenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addTrackToFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem muxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem1;
    }
}

