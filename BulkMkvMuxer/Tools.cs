using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace BulkMkvMuxer
{
    static class Tools
    {
        public static TreeNode CreateTreeNode(string text, Color colour, bool boxChecked, Object mediaStream)
        {
            TreeNode node = new TreeNode();
            node.ForeColor = colour;
            node.Text = text;
            node.Checked = boxChecked;
            node.Tag = mediaStream;
            return node;
        }

        public static void OpenFile(string path)
        {
            if (path != null)
            {
                try
                {
                    System.Diagnostics.Process.Start(path);
                    Tools.WriteLogLine("Execute command: " + path);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Unable to execute command:" + Environment.NewLine + ex.ToString());
                }

            }
        }

        public static string GetUniqueName(string newName)
        {
            int n = 1;
            string uniqueName = newName;
            while (File.Exists(uniqueName))
            {
                uniqueName = Path.GetDirectoryName(uniqueName) + "/" +
                             Path.GetFileNameWithoutExtension(uniqueName) +
                             "(" + n + ")" +
                             Path.GetExtension(uniqueName);
            }
            return uniqueName;
        }

        public static void WriteLogLine(string text)
        {
            using (StreamWriter sw = File.AppendText("Log.txt"))
            {
                sw.WriteLine(DateTime.Now.ToString() + "\t" + text);                          
            }
        }
    }

    
}
