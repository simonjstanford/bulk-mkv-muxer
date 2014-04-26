using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace BulkMkvMuxer
{
    class MkvMergeConnector
    {
        public string MkvMergePath { get; private set; }
        public int ErrorLevel { get; private set; }

        public MkvMergeConnector(string path)
        {
            MkvMergePath = path;
        }

        public int RemoveAllSubtitles(string fileName, int audioStreamUid, string output, BackgroundWorker worker)
        {
            string command = "--output \"" + Tools.GetUniqueName(output + "\\" + Path.GetFileName(fileName)) + "\" --atracks " + audioStreamUid + " --no-subtitles \"" + fileName + "\"";
            return executeMkvMerge(command, worker);
        }

        public int KeepOnlyForcedSubtitles(string fileName, int audioStreamUid, string output, BackgroundWorker worker, string subtitlePath)
        {
            string command = "--output \"" + Tools.GetUniqueName(output + "\\" + Path.GetFileName(fileName)) + "\" --atracks " + audioStreamUid + " --no-subtitles \"" + fileName + "\" \"" + subtitlePath + "\"";
            return executeMkvMerge(command, worker);
        }

        public int KeepOnlySelectedStreams(string fileName, string output, BackgroundWorker worker, int[] videoStreams, int[] audioStreams, int[] subtitleStreams, string[] additionalStreams, bool keepChapters, int[] newDefaultStreams)
        {
            string command = "--output \"" + Tools.GetUniqueName(output + "\\" + Path.GetFileName(fileName)) + "\""; 

            if (videoStreams.Count() > 0)
            {
                command += " --vtracks ";
                for (int i = 0; i < videoStreams.Count(); i++)
                {
                    command += videoStreams[i].ToString();
                    if (i < videoStreams.Count() - 1)
                        command += ",";
                }
            }
            else
                command += " --no-video";

            if (audioStreams.Count() > 0)
            {
                command += " --atracks ";
                for (int i = 0; i < audioStreams.Count(); i++)
                {
                    command += audioStreams[i].ToString();
                    if (i < audioStreams.Count() - 1)
                        command += ",";
                }
            }
            else
                command += " --no-audio";

            if (subtitleStreams.Count() > 0)
            {
                command += " --subtitle-tracks ";
                for (int i = 0; i < subtitleStreams.Count(); i++)
                {
                    command += subtitleStreams[i].ToString();
                    if (i < subtitleStreams.Count() - 1)
                        command += ",";
                }
            }
            else
                command += " --no-subtitles";

            if (!keepChapters)
                command += " --no-chapters";

            if (newDefaultStreams.Count() > 0)
            {
                for (int i = 0; i < newDefaultStreams.Count(); i++)
                    command += " --default-track " + newDefaultStreams[i];
            }

            command += " \"" + fileName + "\" ";

            if (additionalStreams.Count() > 0)
            {
                for (int i = 0; i < additionalStreams.Count(); i++)
                {
                    command += "\"" + additionalStreams[i] + "\" ";
                }
            }

            return executeMkvMerge(command, worker);
        }

        private int executeMkvMerge(string command, BackgroundWorker worker)
        {
            Process p = new Process();
            int exitCode = 3;
            try
            {
                p.StartInfo = new ProcessStartInfo(MkvMergePath, command); //assign the argument
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                StreamReader reader = p.StandardOutput;
                Tools.WriteLogLine("MkvMerge Command: " + MkvMergePath + command);

                string lastLine = "";
                //add each line of the output to the output list
                while (!reader.EndOfStream)
                {

                    string line = reader.ReadLine();
                    if (line.Length > 0)
                    {
                        int n = 0;
                        Match match = Regex.Match(line, @"^.+: (?<percentage>.*)%$");
                        if (match.Success)
                            n = Convert.ToInt32(match.Groups["percentage"].Value);

                        if ((n < 6 || n > 94) && line != lastLine)
                            Tools.WriteLogLine(line);

                        lastLine = line;

                        try
                        {
                            worker.ReportProgress(Convert.ToInt32(n));
                        }
                        catch { }

                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to execute MkvMerge:" + Environment.NewLine + ex.ToString());
            }
            finally
            {
                exitCode = p.ExitCode;
            }

            return exitCode;
        }

        private int readErrorLevel()
        {
            int errorLevel = 3;
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("cmd", "echo %ERRORLEVEL%"); //assign the argument
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                StreamReader reader = p.StandardOutput;
                Tools.WriteLogLine("MkvMerge Command: " + "cmd echo %ERRORLEVEL%");

                //add each line of the output to the output list
                while (!reader.EndOfStream)
                {

                    string line = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to execute MkvMerge:" + Environment.NewLine + ex.ToString());
            }
            
            return errorLevel;

        }
    }
}
