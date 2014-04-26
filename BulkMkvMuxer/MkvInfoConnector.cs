using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace BulkMkvMuxer
{
    class MkvInfoConnector
    {
        public string Path { get; private set; } //the path of the mkvInfo program
        public List<MkvInfoStreamInfo> Subtitles { get; private set; } //a list of the subtitle streams in the mkv
        public List<MkvInfoStreamInfo> AudioStreams { get; private set; } //a list of the audio streams in the mkv
        public List<MkvInfoStreamInfo> VideoStreams { get; private set; } //a list of the audio streams in the mkv
        private string mkvPath; //the path of the associated mkv file
        public Chapters Chapters { get; private set; }
        public MkvInfoConnector(string mkvInfopath, string mkvPath)
        {
            //instantiate and assign properties
            Subtitles = new List<MkvInfoStreamInfo>();
            AudioStreams = new List<MkvInfoStreamInfo>();
            VideoStreams = new List<MkvInfoStreamInfo>();
            Chapters = new BulkMkvMuxer.Chapters(0);
            Path = mkvInfopath;
            this.mkvPath = mkvPath;
        }

        public void ReadStreamInfo()
        {
            List<string> output = new List<string>(); //the list that holds the mkvInfo output

            try
            {
                //create the process that will execute mkvinfo.  Hide the cmd window and redirect the output
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(Path, "\"" + mkvPath + "\""); //assign the argument
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                StreamReader reader = p.StandardOutput;
                Tools.WriteLogLine("Mkv Command: " + Path + " \"" + mkvPath + "\"");

                //add each line of the output to the output list
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.Length > 0)
                    {
                        output.Add(line);
                        Tools.WriteLogLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to execute MkvInfo:" + Environment.NewLine + ex.ToString());
            }
                

            //search the output list for audio and subtitle streams
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].Contains("Track type: subtitles") || output[i].Contains("Track type: audio") || output[i].Contains("Track type: video"))
                {
                    //instantiate temporary variables
                    int uid = -1;
                    string language = "Unknown";
                    bool isDefault = true;
                    StreamType stream = StreamType.Unknown;
                    string codec = "Unknown";

                    if (output[i].Contains("Track type: subtitles"))
                        stream = StreamType.Subtitles;

                    if (output[i].Contains("Track type: audio"))
                        stream = StreamType.Audio;

                    if (output[i].Contains("Track type: video"))
                        stream = StreamType.Video;

                    //parse the id number
                    Match trackNumberMatch = Regex.Match(output[i-2], @"Track number: \d+ \(track ID for mkvmerge & mkvextract: (?<trackNumber>\d+)\)");
                    if (trackNumberMatch.Success)
                    {
                        uid = Convert.ToInt32(trackNumberMatch.Groups["trackNumber"].Value);
                    }
                    else
                    {
                        Match trackNumberMatch2 = Regex.Match(output[i - 3], @"Track number: \d+ \(track ID for mkvmerge & mkvextract: (?<trackNumber>\d+)\)");
                        if (trackNumberMatch2.Success)
                            uid = Convert.ToInt32(trackNumberMatch2.Groups["trackNumber"].Value);                 
                    }

                    //get other stream info
                    for (int j = i; j < output.Count; j++)
                    {
                        try
                        {
                            if (output[j].Contains("+ Language:"))
                            {
                                Match languageMatch = Regex.Match(output[j], @"^.+: (?<language>.*$)");
                                if (languageMatch.Success)
                                    language = languageMatch.Groups["language"].Value;
                            }
                        }
                        catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.ToString()); }

                        try
                        {
                            if (output[j].Contains("+ Codec ID:"))
                                codec = output[j].Substring(15);
                                //System.Windows.Forms.MessageBox.Show(j.ToString());
                        }
                        catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.ToString()); }

                        try
                        {
                            if (output[j].Contains("+ Default flag: 0"))
                                isDefault = false;
                        }
                        catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.ToString()); }

                        //when the search reaches the next stream, stop
                        if (output[j].Contains("A track"))
                        {
                            i = j;
                            break;
                        }
                    }

                    //add the stream info to the appropriate list
                    if (stream == StreamType.Subtitles)
                        Subtitles.Add(new MkvInfoStreamInfo(uid, codec, language, isDefault, stream));

                    if (stream == StreamType.Audio)
                        AudioStreams.Add(new MkvInfoStreamInfo(uid, codec, language, isDefault, stream));

                    if (stream == StreamType.Video)
                        VideoStreams.Add(new MkvInfoStreamInfo(uid, codec, language, isDefault, stream));
                }
                if (output[i].Contains("+ Chapters"))
                {
                    for (int j = i; j < output.Count; j++)
                    {
                        if (output[j].Contains("+ ChapterAtom"))
                        {
                            Chapters.AddChapter();
                        }
                
                    }
                }

            }
        }
    }

    class MkvInfoStreamInfo
    {
        public int TID { get; private set; }
        public string Codec { get; private set; }
        public string Language { get; private set; }
        public StreamType Type { get; private set; }
        public bool IsDefault { get; private set; }
        public bool IsNewDefault { get; set; }

        public MkvInfoStreamInfo(int uid, string codec, string language, bool isDefault, StreamType type)
        {
            TID = uid;
            Codec = codec;
            Language = language;
            IsDefault = isDefault;
            IsNewDefault = false;
            Type = type;
        }

        public void MakeNewDefult(bool isDefault)
        {
            this.IsNewDefault = isDefault;
        }

        public override string ToString()
        {
            return "UID: " + TID + Environment.NewLine +
                   "Codec: " + Codec + Environment.NewLine +
                   "Language: " + Language + Environment.NewLine +
                   "Default: " + IsDefault;
        }
    }

    enum StreamType
    {
        Subtitles,
        Audio,
        Video,
        Unknown,
    }

    class Chapters
    {
        public int Count { get; private set; }

        public Chapters(int count)
        {
            Count = count;
        }

        public void AddChapter()
        {
            Count++;
        }
    }
}
