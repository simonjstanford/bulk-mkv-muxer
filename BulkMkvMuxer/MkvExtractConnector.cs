using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BulkMkvMuxer
{
    class MkvExtractConnector
    {
        public string Path { get; private set; } //the path of the mkvExtract program
        private string mkvPath; //the path of the associated mkv file
        public List<MkvExtractSubtitleInfo> Subtitles { get; private set; }
        private List<MkvInfoStreamInfo> subtitleInfo;//a list of the subtitle streams in the mkv
        private BackgroundWorker worker;

        public MkvExtractConnector(string path, string mkvPath, List<MkvInfoStreamInfo> subtitleInfo, BackgroundWorker worker)
        {
            Path = path;
            this.mkvPath = mkvPath;
            Subtitles = new List<MkvExtractSubtitleInfo>();
            this.subtitleInfo = subtitleInfo;
            this.worker = worker;
        }

        public void ExtractSubtitles()
        {
            string argument = "--ui-language en tracks \"" + mkvPath + "\"";
            foreach (MkvInfoStreamInfo sub in subtitleInfo)
            {
                if (sub.Language == "eng" || sub.Language == "Unknown")
                    argument += " " + sub.TID + ":" + "\"" + mkvPath + "." + sub.TID + ".srt\"";
            }

            try
            {
                //create the process that will execute mkvinfo.  Hide the cmd window and redirect the output
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(Path, argument); //assign the argument
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                StreamReader reader = p.StandardOutput;
                Tools.WriteLogLine("MkvExtract Command: " + Path + argument);
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
                System.Windows.Forms.MessageBox.Show("Unable to execute MkvExtract:" + Environment.NewLine + ex.ToString());
            }


            foreach (MkvInfoStreamInfo sub in subtitleInfo)
            {
                if (File.Exists(mkvPath + "." + sub.TID + ".idx"))
                {
                    Subtitles.Add(new MkvExtractSubtitleInfo(new FileInfo(mkvPath + "." + sub.TID + ".idx"), sub.TID, sub.Language, sub.Codec, sub.IsDefault));
                    Tools.WriteLogLine("Subtitles saved at " + mkvPath + "." + sub.TID + ".idx");
                }
                else if (File.Exists(mkvPath + "." + sub.TID + ".srt"))
                {
                    Subtitles.Add(new MkvExtractSubtitleInfo(new FileInfo(mkvPath + "." + sub.TID + ".srt"), sub.TID, sub.Language, sub.Codec, sub.IsDefault));
                    Tools.WriteLogLine("Subtitles saved at " + mkvPath + "." + sub.TID + ".srt");
                }
            }
        }
    }

    class MkvExtractSubtitleInfo
    {
        public FileInfo File { get; private set; }
        public int UID { get; private set; }
        public string Language { get; private set; }
        public string Extension { get { return File.Extension; } }
        public string Codec { get; private set; }
        public bool IsDefault { get; private set; }

        public MkvExtractSubtitleInfo(FileInfo file, int uid, string language, string codec, bool isDefault)
        {
            File = file;
            UID = uid;
            Language = language;
            Codec = codec;
            this.IsDefault = isDefault;
        }
    }
}
