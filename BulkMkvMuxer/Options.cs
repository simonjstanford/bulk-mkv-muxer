using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BulkMkvMuxer
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            textBoxJavaPath.Text = Properties.Settings.Default.JavaPath;
            textBoxMkvExtractPath.Text = Properties.Settings.Default.MkvExtractPath;
            textBoxMkvInfoPath.Text = Properties.Settings.Default.MkvInfoPath;
            textBoxMkvMergePath.Text = Properties.Settings.Default.MkvMergePath;
        }

        private void buttonAddJavaPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select the Java executable";
                dialog.InitialDirectory = Properties.Settings.Default.JavaPath;
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxJavaPath.Text = dialog.FileName;
            }
        }

        private void buttonAddMkvExtractPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select the MkvExtract executable";
                dialog.InitialDirectory = Properties.Settings.Default.MkvExtractPath;
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxMkvExtractPath.Text = dialog.FileName;
            }
        }

        private void buttonAddMkvInfoPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select the MkvInfo executable";
                dialog.InitialDirectory = Properties.Settings.Default.MkvInfoPath;
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxMkvInfoPath.Text = dialog.FileName;
            }
        }

        private void buttonAddMkvMergePath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select the MkvMerge executable";
                dialog.InitialDirectory = Properties.Settings.Default.MkvMergePath;
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBoxMkvMergePath.Text = dialog.FileName;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.JavaPath = textBoxJavaPath.Text;
            Properties.Settings.Default.MkvExtractPath = textBoxMkvExtractPath.Text;
            Properties.Settings.Default.MkvInfoPath = textBoxMkvInfoPath.Text;
            Properties.Settings.Default.MkvMergePath = textBoxMkvMergePath.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
