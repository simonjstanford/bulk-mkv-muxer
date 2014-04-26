using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BulkMkvMuxer
{
    class BDSup2SubConnector
    {
        public string JavaPath { get; private set; }
        public List<Subtitle> Subtitles { get; private set; }

        List<MkvExtractSubtitleInfo> subtitleInfoFromMkvExtract;
        public BDSup2SubConnector(string javaPath, List<MkvExtractSubtitleInfo> mkvExtractSubtitleInfo)
        {
            JavaPath = javaPath;
            subtitleInfoFromMkvExtract = mkvExtractSubtitleInfo;
            Subtitles = new List<Subtitle>();
        }

        public void ExtractForcedSubtitles()
        {
            //extract all forced subtitles
            foreach (MkvExtractSubtitleInfo sub in subtitleInfoFromMkvExtract)
            {
                List<string> output = new List<string>(); //the list that holds the mkvInfo output

                try
                {
                    //create the process that will execute BDSup2Sub.  Hide the cmd window and redirect the output
                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo(JavaPath, "-jar BDSup2Sub.jar " + "\"" + sub.File.FullName + "\" \"" + Path.GetDirectoryName(sub.File.FullName) + "\\" + Path.GetFileNameWithoutExtension(sub.File.FullName) + "-forced.idx\" /forced"); //assign the argument
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.Start();
                    StreamReader reader = p.StandardOutput;
                    Tools.WriteLogLine("BDSup2Sub Command: " + JavaPath + " -jar BDSup2Sub.jar " + "\"" + sub.File.FullName + "\" \"" + Path.GetDirectoryName(sub.File.FullName) + "\\" + Path.GetFileNameWithoutExtension(sub.File.FullName) + "-forced.idx\" /forced");
                    string subs = "";
                    ////add each line of the output to the output list
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (line != "")
                        {
                            output.Add(line);
                            if (line.Contains('#'))
                                subs += line;
                            else
                                Tools.WriteLogLine(line);
                        }
                    }
                    Tools.WriteLogLine(subs);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Unable to execute BDSup2Sub:" + Environment.NewLine + ex.ToString());
                }
    
                //search the output list for the number of subtitles
                int n = -1;
                for (int i = output.Count - 1; i > 0; i--)
                {
                    if (output[i].Contains('#'))
                    {
                        Match subtitleCountMatch1 = Regex.Match(output[i], @"# (?<numberOfSubs>\d+)");
                        if (subtitleCountMatch1.Success)
                        {
                            n = Convert.ToInt32(subtitleCountMatch1.Groups["numberOfSubs"].Value);
                            break;
                        }
                        else
                        {
                            Match subtitleCountMatch2 = Regex.Match(output[i], @"^#> (?<numberOfSubs>\d+) .+$");
                            if (subtitleCountMatch2.Success)
                            {
                                n = Convert.ToInt32(subtitleCountMatch2.Groups["numberOfSubs"].Value);
                                break;
                            }
                        }                      
                    }
                }

                if (File.Exists(Path.GetDirectoryName(sub.File.FullName) + "\\" + Path.GetFileNameWithoutExtension(sub.File.FullName) + "-forced.idx"))
                {
                    Subtitles.Add(new Subtitle(new FileInfo(sub.File.FullName), sub.UID, n, sub.Language, sub.Codec, sub.IsDefault, new ForcedSubtitles(new FileInfo(Path.GetDirectoryName(sub.File.FullName) + "\\" + Path.GetFileNameWithoutExtension(sub.File.FullName) + "-forced.idx"))));
                    Tools.WriteLogLine("Forced subtitles found: " + Path.GetDirectoryName(sub.File.FullName) + "\\" + Path.GetFileNameWithoutExtension(sub.File.FullName) + "-forced.idx");
                }
                else
                {
                    Subtitles.Add(new Subtitle(new FileInfo(sub.File.FullName), sub.UID, n, sub.Language, sub.Codec, sub.IsDefault));
                }
            }
        }

        public static void OpenSubtitles(Subtitle subtitle)
        {
            openSubtitle(subtitle.File.FullName);
        }

        public static void OpenSubtitles(ForcedSubtitles subtitle)
        {
            openSubtitle(subtitle.File.FullName);
        } 

        private static void openSubtitle(String path)
        {
            if (path != null)
            {
                try
                {
                    string argument = "-jar BDSup2Sub.jar " + "\"" + path + "\"";
                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo(Properties.Settings.Default.JavaPath, argument);
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    Tools.WriteLogLine("MkvExtract Command: " + Properties.Settings.Default.JavaPath + argument);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Unable to open BDSup2Sub:" + Environment.NewLine + ex.ToString());
                }

            }
        }
    }

    class Subtitle
    {
        public int Count { get; private set; }
        public FileInfo File { get; private set; }
        public ForcedSubtitles ForcedSubtitles { get; private set; }
        public int TID { get; private set; }
        public string Language { get; private set; }
        public string Codec { get; private set; }
        public bool IsDefault { get; private set; }
        public bool IsNewDefault { get; set; }

        public bool HasForcedSubtitles
        {
            get
            {
                if (ForcedSubtitles != null)
                    return true;
                else
                    return false;
            }
        }

        public Subtitle(FileInfo file, int uid, int count, string language, string codec, bool isDefault, ForcedSubtitles forcedSubtitles = null)
        {
            Count = count;
            File = file;
            ForcedSubtitles = forcedSubtitles;
            TID = uid;
            Language = language;
            Codec = codec;
            this.IsDefault = isDefault;
            IsNewDefault = false;
        }

        public void MakeNewDefult(bool isDefault)
        {
            this.IsNewDefault = isDefault;
        }
    }

    class ForcedSubtitles
    {
        public FileInfo File { get; private set; }
        public string FullName { get { return File.FullName; } }
        public ForcedSubtitles(FileInfo file)
        {
            File = file;
        }
    }
}
