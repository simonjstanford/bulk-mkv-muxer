using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace BulkMkvMuxer
{
    class MKV
    {
        public string FullPath { get; private set; }      
        public List<Subtitle> Subtitles
        {
            get 
            {
                try
                {
                    return bdSup2Sub.Subtitles;
                }
                catch
                {
                   return new List<Subtitle>();
                }                   
            }          
        }
        public List<MkvInfoStreamInfo> AudioStreams
        {
            get 
            {
                try
                {
                    return mkvInfo.AudioStreams;
                }
                catch
                {
                   return new List<MkvInfoStreamInfo>();
                }                  
            }
        }
        public MkvInfoStreamInfo BestAudioStream
        {
            get 
            {
                MkvInfoStreamInfo dtsStream = null;
                MkvInfoStreamInfo defaultStream = null;

                foreach (MkvInfoStreamInfo stream in mkvInfo.AudioStreams)
                {
                    if (stream.IsDefault)
                        defaultStream = stream;
                    if (stream.Codec.Contains("DTS"))
                        dtsStream = stream;
                }

                if (dtsStream != null)
                    return dtsStream;
                else if (defaultStream != null)
                    return defaultStream;
                else if (mkvInfo.AudioStreams.Count > 0)
                    return mkvInfo.AudioStreams[0];
                else
                    return null;
            }
        }
        public List<MkvInfoStreamInfo> VideoStreams
        {
            get
            {
                try
                {
                    return mkvInfo.VideoStreams;
                }
                catch
                {
                    return new List<MkvInfoStreamInfo>();
                }
            }
        }
        public bool HasForcedSubtitles
        {
            get
            {
                foreach (Subtitle subtitle in Subtitles)
                {
                    if (subtitle.HasForcedSubtitles)
                        return true;
                }
                return false;
            }
        }      
        public ForcedSubtitles GetForcedSubtitles
        {
            get
            {
                foreach (Subtitle subtitle in Subtitles)
                {
                    if (subtitle.HasForcedSubtitles)
                        return subtitle.ForcedSubtitles;
                }
                return null;
            }
        }

        public bool HasChapters
        {
            get
            {
                if (mkvInfo.Chapters.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public Chapters Chapters
        {
            get
            {
                return mkvInfo.Chapters;
            }
        }

        public bool HasNewDefaultSubtitles
        {
            get
            {
                foreach (Subtitle stream in bdSup2Sub.Subtitles)
                    if (stream.IsNewDefault)
                        return true;
                return false;
            }
        }

        public bool HasNewDefaultAudio 
        {
            get
            {
                foreach (MkvInfoStreamInfo stream in mkvInfo.AudioStreams)
                    if (stream.IsNewDefault)
                        return true;
                return false;
            }
        }

        public bool HasNewDefaultVideo
        {
            get
            {
                foreach (MkvInfoStreamInfo stream in mkvInfo.VideoStreams)
                    if (stream.IsNewDefault)
                        return true;
                return false;
            }
        }

        public List<string> AdditionalStreams { get; private set; }

        private MkvInfoConnector mkvInfo;
        private MkvExtractConnector mkvExtract;
        private BDSup2SubConnector bdSup2Sub;
        private MkvMergeConnector mkvMerge;
        private string javaPath;

        public MKV(string filePath, string mkvInfoPath, string mkvExtractPath, string javaPath, string mkvMergePath, BackgroundWorker worker)
        {
            FullPath = filePath;
            this.javaPath = javaPath;
            AdditionalStreams = new List<string>();

            if (File.Exists(filePath))
            {
                //get track UIDs and details
                Tools.WriteLogLine("Executing MkvInfo:" + Environment.NewLine + "MkvInfo Path: " + mkvInfoPath + Environment.NewLine + "File: " + FullPath);
                mkvInfo = new MkvInfoConnector(mkvInfoPath, FullPath);
                mkvInfo.ReadStreamInfo();

                //extract the subtitles
                Tools.WriteLogLine("Executing MkvExtract:" + Environment.NewLine + "MkvExtract Path: " + mkvExtractPath + Environment.NewLine + "File: " + FullPath);
                mkvExtract = new MkvExtractConnector(mkvExtractPath, FullPath, mkvInfo.Subtitles, worker);    
                mkvExtract.ExtractSubtitles();

                Tools.WriteLogLine("Executing BDSup2Sub:" + Environment.NewLine + "Java Path: " + javaPath);
                bdSup2Sub = new BDSup2SubConnector(javaPath, mkvExtract.Subtitles);
                bdSup2Sub.ExtractForcedSubtitles();

                mkvMerge = new MkvMergeConnector(mkvMergePath);
            }
            else
            {
                MessageBox.Show(FullPath + " does not exist!");
            }
        }

        public int RemoveAllSubtitles(string outputDirectory, BackgroundWorker worker)
        {
            Tools.WriteLogLine("Executing MKVMerge to remove all subtitles:");
            return mkvMerge.RemoveAllSubtitles(FullPath, BestAudioStream.TID, outputDirectory, worker);
        }

        public int KeepOnlyForcedSubtitles(string outputDirectory, BackgroundWorker worker)
        {
            if (HasForcedSubtitles)
            {
                Tools.WriteLogLine("Executing MKVMerge to keep only forced subtitles:");
                return mkvMerge.KeepOnlyForcedSubtitles(FullPath, BestAudioStream.TID, outputDirectory, worker, GetForcedSubtitles.FullName);
            }
            else
                return mkvMerge.RemoveAllSubtitles(FullPath, BestAudioStream.TID, outputDirectory, worker);
        }

        internal int KeepUserSelectedStreams(string outputDirectory, int[] videoStreams, int[] audioStreams, int[] subtitleStreams, string[] additionalStreams, bool keepChapters, int[] newDefaultStreams, BackgroundWorker worker)
        {
            Tools.WriteLogLine("Executing MKVMerge to keep only user selected tracks:");
            return mkvMerge.KeepOnlySelectedStreams(FullPath, outputDirectory, worker, videoStreams, audioStreams, subtitleStreams, additionalStreams, keepChapters, newDefaultStreams);
        }

        internal void SetNewDefaultAudio(int newDefaultAudioTID)
        {
            foreach (MkvInfoStreamInfo stream in mkvInfo.AudioStreams)
            {
                if (stream.TID == newDefaultAudioTID)
                    stream.MakeNewDefult(true);
                else
                    stream.MakeNewDefult(false);
            }
        }

        internal void SetNewDefaultVideo(int newDefaultVideoTID)
        {
            foreach (MkvInfoStreamInfo stream in mkvInfo.VideoStreams)
            {
                if (stream.TID == newDefaultVideoTID)
                    stream.MakeNewDefult(true);
                else
                    stream.MakeNewDefult(false);
            }
        }

        internal void SetNewDefaultSubtitle(int newDefaultSubtitleTID)
        {
            foreach (Subtitle stream in bdSup2Sub.Subtitles)
            {
                if (stream.TID == newDefaultSubtitleTID)
                    stream.MakeNewDefult(true);
                else
                    stream.MakeNewDefult(false);
            }
        }
    }
}
