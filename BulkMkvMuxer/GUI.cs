using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace BulkMkvMuxer
{
    public partial class GUI : Form
    {
        List<MKV> MKVs; //The list of MKVs that the user ammends and muxes
        BackgroundWorker mkvExtractWorker; //the background worker thread for MkvExtract
        BackgroundWorker mkvMergeWorker; //the background worker thread for MkvMerge
        int progressCounter; //int to track the progress of an asynchronous operation - used for the progress bar
        int totalNumberOfFiles; //int to track the total number of files being worked with in an asynchronous operation - used for the progress bar

        //file types compatible with MkvMerge formatted in a string used to filter file types in an OpenFileDialog
        string compatibleFileTypes = "Media Files|*.ac3;*.eac3;*.aac;*.m4a;*.mp4;*.264;*.avc*.h264;*.x264;*.avi;" + 
                                     "*.caf;*.m4a;*.mp4;*.drc;*.thd;*.thd+ac3;*.truehd;*.true-hd;*.dts;*.dtshd;" + 
                                     "*.dts-hd;*.flac;*.ogg;*.flv;*.ivf;*.mp4.;*.m4v;*.mp2;*.mp3;*.mpg;*.mpeg;" + 
                                     "*.m2v;*.mpv;*.evo;*.evob;*.vob;*.ts;*.m2ts;*.mts;*.m1v;*.m2v;*.mpv;*.mpls;" + 
                                     "*.mka;*.mks;*.mkv;*.mk3d;*.webm;*.webmv;*.webma;*.sup;*.mov;*.ogg;*.ogm;*.ogv;" + 
                                     "*.opus;*.ogg;*.ra;*.ram;*.rm;*.rmvb;*.rv;*.srt;*.ass;*.ssa;*.tta;*.usf;*.xml;" + 
                                     "*.vc1;*.btn;*.idx;*.wav;*.wv;*.webm;*.webmv;*.webma" + 
                                     "|AllFiles(*.*)|*.*";

        /// <summary>
        /// The constructor
        /// </summary>
        public GUI()
        {
            InitializeComponent();
            //initialise objects and set default values
            progressCounter = 0;
            totalNumberOfFiles = 0;
            toolStripStatusLabel1.Text = "Ready";
            groupBoxMux.Enabled = false;
            buttonMux.Enabled = false;
            muxToolStripMenuItem.Enabled = false;
            buttonClear.Enabled = false;
            MKVs = new List<MKV>();                       
        }

        /// <summary>
        /// Adds MKVs to the application by scanning a directory that the user has selected
        /// </summary>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            openDirectory();
        }

        /// <summary>
        /// Adds MKVs to the application by scanning a directory that the user has selected
        /// </summary>
        private void addDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDirectory();
        }

        /// <summary>
        /// Adds MKVs to the application by scanning a directory that the user has selected
        /// </summary>
        private void openDirectory()
        {
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //recursively search for MKVs in the directory
                DirectoryInfo directory = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                totalNumberOfFiles = directory.GetFiles("*.mkv", SearchOption.AllDirectories).Count();
                //execute MkvExtract scanning asynchronously
                executeMkvExtractAsync(directory.GetFiles("*.mkv", SearchOption.AllDirectories));
            }
        }

        /// <summary>
        /// Reads the object dropped into the treeview from Windows and adds them to 
        /// the list of MKVs if appropriate
        /// </summary>
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] items = (string[])e.Data.GetData(DataFormats.FileDrop); //read the object dropped
            List<FileInfo> individualFiles = new List<FileInfo>(); //a collection used to hold individual MKVs
            foreach (string item in items)
            {
                //if the object is a directory, get all MKVs in the directory and asynchronously add them
                FileAttributes attr = File.GetAttributes(item);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    DirectoryInfo directory = new DirectoryInfo(item);
                    individualFiles.AddRange(directory.GetFiles("*.mkv", SearchOption.AllDirectories));
                }
                else
                {
                    //if the object is a file, check if it is an MKV then add it the the local collection
                    if (Path.GetExtension(item) == ".mkv")
                        individualFiles.Add(new FileInfo(item));
                }
            }

            //Add the individual MKVs to the application asynchronously
            if (individualFiles.Count > 0)
            {
                totalNumberOfFiles = individualFiles.Count();
                executeMkvExtractAsync(individualFiles.ToArray());
            }
        }

        /// <summary>
        /// Changes the mouse icon when the user drags an object into the application
        /// </summary>
        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// Adds MKVs to the application by scanning individual file(s) that the user has selected
        /// </summary>
        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open an openFileDialog that only shows MKVs
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            List<FileInfo> individualFiles = new List<FileInfo>();
            openFileDialog1.Filter = "MKV|*.mkv";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = true; //allow multiple selections
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //Create FileInfo objects out of all the files selected and add them to a local collection
                foreach (string path in openFileDialog1.FileNames)
                    individualFiles.Add(new FileInfo(path));

                //Add the individual MKVs to the application asynchronously
                if (individualFiles.Count > 0)
                {
                    totalNumberOfFiles = individualFiles.Count();
                    executeMkvExtractAsync(individualFiles.ToArray());
                }
            }
        }

        /// <summary>
        /// Asynchronously adds MKVs the application
        /// </summary>
        /// <param name="files"></param>
        private void executeMkvExtractAsync(FileInfo[] files)
        {
            //disable inappropriate buttons
            toolStripProgressBar1.Enabled = true;
            buttonAddDirectory.Enabled = false;
            buttonClear.Enabled = false;
            buttonMux.Enabled = false;
            groupBoxMux.Enabled = false;
            muxToolStripMenuItem.Enabled = false;
            addFileToolStripMenuItem.Enabled = false;
            addDirectoryToolStripMenuItem.Enabled = false;
            clearToolStripMenuItem.Enabled = false;
            muxToolStripMenuItem.Enabled = false;

            //initialise and execute the background worker, passing to it an array of FileInfo files 
            mkvExtractWorker = new BackgroundWorker();
            mkvExtractWorker.WorkerReportsProgress = true;
            mkvExtractWorker.DoWork += new DoWorkEventHandler(mkvExtractWorker_DoWork);
            mkvExtractWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mkvExtractWorker_RunWorkerCompleted);
            mkvExtractWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            mkvExtractWorker.RunWorkerAsync(files);
        }

        /// <summary>
        /// MkvExtractWorker asynchronous method - adds MKVs the application
        /// </summary>
        void mkvExtractWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //name the thread
            if (Thread.CurrentThread.Name == null)
                Thread.CurrentThread.Name = "NewMkvScannerThread";
            //reset the progress counter and inform the user what is happening
            progressCounter = 0; 
            toolStripStatusLabel1.Text = "Extracting subtitles and scanning...";
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);

            //Asynchronously add all MKVs to the application
            FileInfo[] data = e.Argument as FileInfo[];
            foreach (FileInfo file in data)
            {
                //pass the paths of all the different files and executables required to read the MKV
                MKVs.Add(new MKV(file.FullName, "\"" + Properties.Settings.Default.MkvInfoPath + "\"", "\"" + Properties.Settings.Default.MkvExtractPath + "\"", "\"" + Properties.Settings.Default.JavaPath + "\"", "\"" + Properties.Settings.Default.MkvMergePath + "\"", mkvExtractWorker));
                progressCounter++; //increase the progress bar value
            }
        }

        /// <summary>
        /// Increases the progress bar value
        /// </summary>
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //calculate the percentage done
            int n = Convert.ToInt32(((double)e.ProgressPercentage / (double)totalNumberOfFiles)) + ((100 / totalNumberOfFiles) * progressCounter);

            //assign the percentage done to both taskbar and application progress bars
            if (n > 0 && n < 101)
            {
                toolStripProgressBar1.Value = n;
                TaskbarManager.Instance.SetProgressValue(n, 100);
            }
                
        }

        /// <summary>
        /// Displays all scanned MKVs - executes after MkvExtractWorker has completed
        /// </summary>
        void mkvExtractWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            displayMKVs(); //display all MKVs in the treeview

            //re-enable buttons
            groupBoxMux.Enabled = true;
            buttonMux.Enabled = true;
            muxToolStripMenuItem.Enabled = true;
            buttonAddDirectory.Enabled = true;
            buttonClear.Enabled = true;
            addFileToolStripMenuItem.Enabled = true;
            addDirectoryToolStripMenuItem.Enabled = true;
            clearToolStripMenuItem.Enabled = true;
            muxToolStripMenuItem.Enabled = true;

            //reset progress bar and status
            toolStripProgressBar1.Value = 0;
            toolStripStatusLabel1.Text = "Finished reading MKVs";
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        /// <summary>
        /// Displays all scanned MKVs in the treeview and selects the most appropriate muxing option
        /// </summary>
        private void displayMKVs()
        {
            treeView1.Nodes.Clear(); //clear the treeview                  

            //these bools are used to make sure the appropriate radio button is selected for the user by default
            bool hasForced = false; //bool to signal if MKV contains forced captions
            bool hasSmallSubFileOrDTS = false; //bool to signal if MKV has a DTS track or a subtitle stream with only a few subtitles in it

            //enter each MKV into the treeview
            foreach (MKV mkv in MKVs)
            {
                //create a new base node for the MKV
                TreeNode node = new TreeNode(mkv.FullPath);
                node.Tag = mkv; //add a reference to the MKV object in the treeview
                node.Checked = true; //select it for muxing by default
                treeView1.Nodes.Add(node); //add the node to the treeview

                //add all video streams in the MKV to the node
                foreach (MkvInfoStreamInfo video in mkv.VideoStreams)
                {
                    //create the node text
                    string text = video.Codec + " (ID " + video.TID + ", type: video)";

                    //if stream is the default stream, show this
                    if (video.IsDefault)
                        text += " - default";

                    //add the video stream to the node
                    node.Nodes.Add(Tools.CreateTreeNode(text, Color.Black, true, video));
                }

                //add all audio streams in the MKV to the node
                foreach (MkvInfoStreamInfo audio in mkv.AudioStreams)
                {
                    //change the colour of the node if the audio stream is DTS
                    Color colour = Color.Black;
                    if (audio.Codec.Contains("DTS")) colour = Color.Red;

                    //create the node text
                    string text = audio.Codec + " (ID " + audio.TID + ", type: audio, language: " + audio.Language.ToLower() + ")";

                    //decide if the node should be ticked or not.
                    //if a DTS stream is present, it should be ticked. Else, the default audio stream is ticked
                    bool nodeChecked = false; //flag to signify if node is ticked or not
                    if (mkv.BestAudioStream.Codec.Contains("DTS") && audio.Codec.Contains("DTS"))
                        nodeChecked = true;
                    else if (mkv.BestAudioStream.Codec.Contains("DTS") && !audio.Codec.Contains("DTS"))
                        nodeChecked = false;
                    else if (!mkv.BestAudioStream.Codec.Contains("DTS") && audio.IsDefault)
                        nodeChecked = true;
                    else if (!mkv.BestAudioStream.Codec.Contains("DTS") && !audio.IsDefault)
                        nodeChecked = false;

                    if (audio.IsDefault) text += " - default"; //if the stream is the default audio stream, add this to the text
                    if (mkv.BestAudioStream.Codec.Contains("DTS")) hasSmallSubFileOrDTS = true; //if DTS present, mark that the radio button needs to be changed to "mux only checked streams"

                    node.Nodes.Add(Tools.CreateTreeNode(text, colour, nodeChecked, audio)); //add the audio stream to the node
                }

                //add all english subtitles in the mkv to the node 
                foreach (Subtitle subtitle in mkv.Subtitles)
                {
                    //create the node text
                    string text = subtitle.Codec + " (ID " + subtitle.TID + ", type: subtitles, language: " + subtitle.Language.ToLower() + ", " + subtitle.Count + " captions)";
                    //if stream is the default stream, show this
                    if (subtitle.IsDefault) text += " - default";

                    //create the node
                    TreeNode subNode = Tools.CreateTreeNode(text, Color.Black, false, subtitle);

                    //if the subtitle contains forced subtitles add this as a subnode of the subtitle node
                    if (subtitle.HasForcedSubtitles)
                        subNode.Nodes.Add(Tools.CreateTreeNode("Forced captions", Color.Red, true, subtitle.ForcedSubtitles));
                    
                    //if the subtitle only has a few captions change its colour to red - it could contain only forced subtitles
                    if (subtitle.Count < 150)
                    {
                        subNode.Checked = true; //make sure it is ticked
                        subNode.ForeColor = Color.Red;
                        hasSmallSubFileOrDTS = true; //and mark that it has been found - this changes the radio button to "mux only checked"
                    }

                    node.Nodes.Add(subNode); //finally, add the subtitle node to the mkv node
                }

                if (mkv.HasForcedSubtitles)
                    hasForced = true; //if there is forced subtitles, flag this - this changes the radio button to "mux only forced"

                //add any additional streams that the user has selected for the file
                foreach (string filePath in mkv.AdditionalStreams)
                    node.Nodes.Add(Tools.CreateTreeNode(filePath, Color.Black, true, filePath));

                //add any chapters as a new stream
                if (mkv.HasChapters)
                    node.Nodes.Add(Tools.CreateTreeNode("Chapters (" + mkv.Chapters.Count + " entries)", Color.Black, true, mkv.Chapters));
            }


            //select the correct radio buton
            //if there are forced subtitles enable the 'keep forced subtitles' radio button and select it
            if (hasForced)
            {
                radioButtonKeepForced.Enabled = true;
                radioButtonKeepForced.Checked = true;
            }
            else
                //else disable the 'keep forced subtitles' radio button
                radioButtonKeepForced.Enabled = false;

            //if a small subtitle track or DTS stream has been detected select the 'keep only checked' radio button
            if (hasSmallSubFileOrDTS)
                radioButtonKeepChecked.Checked = true;
            
            if (!hasSmallSubFileOrDTS && !hasForced)
                //if none of the above found, just select the 'remove all' radio button
                radioButtonRemoveAll.Checked = true;

            treeView1.ExpandAll(); //expand all nodes
        }

        /// <summary>
        /// Executes muxing
        /// </summary>
        private void buttonMux_Click(object sender, EventArgs e)
        {
            mux();
        }

        /// <summary>
        /// Executes muxing
        /// </summary>
        private void muxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mux();
        }

        /// <summary>
        /// Asynchronously executes muxing using the option selected by the user
        /// </summary>
        private void mux()
        {
            string outputDirectory = "";

            //show the folder browser for the user to select a folder to save the output to
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //disable inappropriate buttons, etc.
                toolStripProgressBar1.Enabled = true;
                groupBoxMux.Enabled = false;
                buttonMux.Enabled = false;
                muxToolStripMenuItem.Enabled = false;
                buttonAddDirectory.Enabled = false;
                buttonClear.Enabled = false;
                totalNumberOfFiles = MKVs.Count;
                toolStripStatusLabel1.Text = "Muxing tracks...";

                outputDirectory = folderBrowserDialog1.SelectedPath; //set the output directory

                //instantiate the asynchronous thread
                mkvMergeWorker = new BackgroundWorker();
                mkvMergeWorker.WorkerReportsProgress = true;
                mkvMergeWorker.DoWork += new DoWorkEventHandler(mkvMergeWorker_DoWork);
                mkvMergeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mkvMergeWorker_RunWorkerCompleted);
                mkvMergeWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);

                //execute the asynchronous thread - pass the user selected option as an argument
                if (radioButtonRemoveAll.Checked)
                    mkvMergeWorker.RunWorkerAsync(new MuxArguments(outputDirectory, MuxOptions.RemoveAll));
                else if (radioButtonKeepForced.Checked)
                    mkvMergeWorker.RunWorkerAsync(new MuxArguments(outputDirectory, MuxOptions.KeepForced));
                else if (radioButtonKeepChecked.Checked)
                {
                    treeView1.Enabled = false; //disable the treeview - stops the user changing selections mid execution
                    mkvMergeWorker.RunWorkerAsync(new MuxArguments(outputDirectory, MuxOptions.UserChecked));
                }
            }
        }

        /// <summary>
        /// Asynchronously executes muxing using the option selected by the user - called by the mux() method   
        /// </summary>
        void mkvMergeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //name the thread
            if (Thread.CurrentThread.Name == null)
                Thread.CurrentThread.Name = "MkvMerge Thread";
            MuxArguments args = e.Argument as MuxArguments; //read the arguments
            
            //instantiate local variables
            progressCounter = 0; //tracks the progress - used for the progress bar
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal); //turn on the taskbar progress bar
            int result = -1; //holds the result code of the mkvmerge execution

            //take a different action dependent on the user selection
            //as mkv merge is executed for each mkv, some may succeed and some may fail
            //result codes are: 0 - success, 1 - completed with warnings, 2 - failed
            //this code displays the worst (highest number) result code to the user
            switch (args.Option)
            {
                case MuxOptions.RemoveAll: //remove all subtitles and keep only the best audio stream
                    foreach (MKV mkv in MKVs)
                    {
                        //remove all subtitles and read the result code
                        int errorCode = mkv.RemoveAllSubtitles(args.OutputDirectory, mkvMergeWorker);
                        if (errorCode > result) result = errorCode; //if this error code is worse than any before it record the error code 
                        progressCounter++;
                    }
                    break;
                case MuxOptions.KeepForced: //keep only forced subtitles and the best audio stream
                    foreach (MKV mkv in MKVs)
                    {
                        //remove all subtitles and read the result code
                        int errorCode = mkv.KeepOnlyForcedSubtitles(args.OutputDirectory, mkvMergeWorker);
                        if (errorCode > result) result = errorCode; //if this error code is worse than any before it record the error code
                        progressCounter++;
                    }
                    break;
                case MuxOptions.UserChecked: //keep only streams checked by the user
                    foreach (TreeNode node in treeView1.Nodes)
                        if (node.Checked && node.Tag is MKV) //find all checked MKVs
                        {
                            //instantiate local variables
                            MKV mkv = node.Tag as MKV; //the current MKV reference
                            List<int> audioStreams = new List<int>(); //the list of audio stream IDs to keep
                            List<int> videoStreams = new List<int>(); //the list of video stream IDs to keep
                            List<int> subtitleStreams = new List<int>(); //the list of subtitles stream IDs to keep
                            List<string> additionalStreams = new List<string>(); //the list of additional streams and extracted forced subtitles added by the user
                            List<int> newDefaultStreams = new List<int>();  //the list of stream IDs that will be changed to the default stream for its type (video, audio, subtitle)
                            bool keepChapters = true; //flag to indicate if existing chapters will be muxed out of the file

                            //search each stream (video, audio & subtitle) within the MKV
                            foreach (TreeNode streamNode in node.Nodes)
                            {
                                //if audio or video node is checked then the user wants it kept in the MKV - all unchecked streams are removed
                                if (streamNode.Tag is MkvInfoStreamInfo && streamNode.Checked)
                                {
                                    MkvInfoStreamInfo stream = streamNode.Tag as MkvInfoStreamInfo; //read the stream object
                                    if (stream.Type == StreamType.Audio) audioStreams.Add(stream.TID); //if stream is audio, add the ID to the audioStream collection
                                    if (stream.Type == StreamType.Video) videoStreams.Add(stream.TID); //if stream is video, add the ID to the videoStream collection
                                    if (stream.IsNewDefault) newDefaultStreams.Add(stream.TID); //if stream has been marked as a new default stream, add the ID to the newDefaultStreams collection
                                }

                                //if subtitle node is checked then the user wants it kept in the MKV - all unchecked streams are removed
                                if (streamNode.Tag is Subtitle)
                                {
                                    Subtitle subtitle = streamNode.Tag as Subtitle; //read the stream object
                                    if (streamNode.Checked)
                                        subtitleStreams.Add(subtitle.TID); //add the subtitle ID to the subtitleStreams collection

                                    //check if there are any forced subtitles within this subtitle stream
                                    foreach (TreeNode forcedNode in streamNode.Nodes)
                                    {
                                        //if there are forced subtitles, and the node has been checked...
                                        if (forcedNode.Tag is ForcedSubtitles && forcedNode.Checked)
                                        {
                                            //...read the object and add the path of the extracted forced subtitles to the addionalstreams collection
                                            ForcedSubtitles forcedSubs = forcedNode.Tag as ForcedSubtitles;
                                            additionalStreams.Add(forcedSubs.FullName);
                                        }
                                    }
                                    if (subtitle.IsNewDefault) newDefaultStreams.Add(subtitle.TID); //if stream has been marked as a new default stream, add the ID to the newDefaultStreams collection
                                }

                                //if the node is an additional file added by the user (signified by type string in the Tag property), add this path to the additionalStreams collection
                                if (streamNode.Tag is string && streamNode.Checked) additionalStreams.Add(streamNode.Tag.ToString());

                                //if the user has deselected the chapters, flag that they should be muxed out
                                if (streamNode.Tag is Chapters && !streamNode.Checked) keepChapters = false;
                            }

                            //pass all the info collected from the treeview to MkvMerge, which then keeps/removes the necessary streams.  Record the return value
                            int errorCode = mkv.KeepUserSelectedStreams(args.OutputDirectory, 
                                                                        videoStreams.ToArray(), 
                                                                        audioStreams.ToArray(), 
                                                                        subtitleStreams.ToArray(), 
                                                                        additionalStreams.ToArray(), 
                                                                        keepChapters, 
                                                                        newDefaultStreams.ToArray(), 
                                                                        mkvMergeWorker);

                            //if this error code is worse than any before it record the error code
                            if (errorCode > result) result = errorCode;
                            progressCounter++;
                        }
                    break;
                default:
                    break;
            }
            e.Result = result; //when complete, return the worst return value to the UI thread
        }

        /// <summary>
        /// Executes after MkvMerge has executed
        /// </summary>
        void mkvMergeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //reset the progress bar and enable buttons
            toolStripProgressBar1.Value = 0;
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            treeView1.Enabled = true;
            groupBoxMux.Enabled = true;
            buttonMux.Enabled = true;
            muxToolStripMenuItem.Enabled = true;
            buttonAddDirectory.Enabled = true;
            buttonClear.Enabled = true;

            //read and display the mkvmerge return code 
            int result = Convert.ToInt32(e.Result.ToString());
            toolStripStatusLabel1.Text = "Finished code " + result.ToString();
            switch (result)
            {

                case 0:
                    toolStripStatusLabel1.Text = "Completed successfully (0)";
                    break;
                case 1:
                    toolStripStatusLabel1.Text = "Completed with warnings (1)";
                    break;
                case 2:
                    toolStripStatusLabel1.Text = "Muxing FAILED (2) - check log";
                    break;
                default:
                    toolStripStatusLabel1.Text = "Finished - Success unknown";
                    break;
            }
        }

        /// <summary>
        /// Selects all sibling nodes.
        /// </summary>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectAll(true);
        }

        /// <summary>
        /// Selects all sibling nodes.
        /// </summary>
        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectAll(true);
        }

        /// <summary>
        /// Deselects all sibling nodes
        /// </summary>
        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectAll(false);
        }

        /// <summary>
        /// Deselects all sibling nodes
        /// </summary>
        private void selectNoneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectAll(false);
        }

        /// <summary>
        /// Selects/Deselects all sibling nodes. 
        /// If used on an MKV node, all MKVs are selected/de-selected. 
        /// If used on a stream in an MKV, all streams in that MKV are selected/de-selected
        /// </summary>
        private void selectAll(bool check)
        {
            try
            {
                if (treeView1.SelectedNode != null) //only work if a node has been selected
                {
                    if (treeView1.SelectedNode.Parent != null)
                        //if there is a parent, then this is a stream node within an MKV
                        foreach (TreeNode node in treeView1.SelectedNode.Parent.Nodes)
                            node.Checked = check; //check all other streams in that MKV
                    else
                        //if there is no parent, this is an MKV node
                        foreach (TreeNode node in treeView1.Nodes)
                            node.Checked = check; //check all MKV nodes
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Makes a node clicked by the mouse the selected node
        /// </summary>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
        }

        /// <summary>
        /// Shows/hides the context menu items
        /// </summary>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            //if no node selected, hide all items
            if (treeView1.SelectedNode == null)
            {
                contextMenuStrip1.Items["playToolStripMenuItem"].Enabled = false; //play file
                contextMenuStrip1.Items["openToolStripMenuItem"].Enabled = false; //open subtitle
                contextMenuStrip1.Items["selectAllToolStripMenuItem"].Enabled = false; //select all
                contextMenuStrip1.Items["selectNoneToolStripMenuItem"].Enabled = false; //select none
                contextMenuStrip1.Items["addTrackToFileToolStripMenuItem"].Enabled = false; //add track to file
                contextMenuStrip1.Items["defaultToolStripMenuItem"].Enabled = false; //add track to file
            }
            else
            {
                //if any node is selected, show select/deselect all
                contextMenuStrip1.Items["selectAllToolStripMenuItem"].Enabled = true;
                contextMenuStrip1.Items["selectNoneToolStripMenuItem"].Enabled = true;

                //and then turn on stream specific options
                if (treeView1.SelectedNode.Tag is Subtitle)
                {
                    contextMenuStrip1.Items["playToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["openToolStripMenuItem"].Enabled = true;
                    contextMenuStrip1.Items["addTrackToFileToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["defaultToolStripMenuItem"].Enabled = true;
                }
                else if (treeView1.SelectedNode.Tag is ForcedSubtitles)
                {
                    contextMenuStrip1.Items["playToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["openToolStripMenuItem"].Enabled = true;
                    contextMenuStrip1.Items["addTrackToFileToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["defaultToolStripMenuItem"].Enabled = false;
                }
                else if (treeView1.SelectedNode.Tag is MKV)
                {
                    contextMenuStrip1.Items["playToolStripMenuItem"].Enabled = true;
                    contextMenuStrip1.Items["openToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["addTrackToFileToolStripMenuItem"].Enabled = true;
                    contextMenuStrip1.Items["defaultToolStripMenuItem"].Enabled = false;
                }
                else if (treeView1.SelectedNode.Tag is MkvInfoStreamInfo)
                {
                    contextMenuStrip1.Items["playToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["openToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["addTrackToFileToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["defaultToolStripMenuItem"].Enabled = true;
                }
                else
                {
                    contextMenuStrip1.Items["playToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["openToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["addTrackToFileToolStripMenuItem"].Enabled = false;
                    contextMenuStrip1.Items["defaultToolStripMenuItem"].Enabled = false;
                }
            }
        }

        /// <summary>
        /// Shows/hides the toolstrip 'edit' menu items
        /// </summary>
        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            //if no node selected, hide all items
            if (treeView1.SelectedNode == null)
            {
                editToolStripMenuItem.DropDown.Items["playToolStripMenuItem1"].Enabled = false; //play file
                editToolStripMenuItem.DropDown.Items["oPenToolStripMenuItem1"].Enabled = false; //open subtitle
                editToolStripMenuItem.DropDown.Items["selectAllToolStripMenuItem1"].Enabled = false; //select all
                editToolStripMenuItem.DropDown.Items["selectNoneToolStripMenuItem1"].Enabled = false; //select none
                editToolStripMenuItem.DropDown.Items["addTrackToFileToolStripMenuItem1"].Enabled = false; //add track to file
                editToolStripMenuItem.DropDown.Items["defaultToolStripMenuItem1"].Enabled = false; //add track to file
            }
            else
            {
                //if any node is selected, show select/deselect all
                editToolStripMenuItem.DropDown.Items["selectAllToolStripMenuItem1"].Enabled = true;
                editToolStripMenuItem.DropDown.Items["selectNoneToolStripMenuItem1"].Enabled = true;

                //and then turn on stream specific options
                if (treeView1.SelectedNode.Tag is Subtitle)
                {
                    editToolStripMenuItem.DropDown.Items["playToolStripMenuItem1"].Enabled = false;
                    editToolStripMenuItem.DropDown.Items["oPenToolStripMenuItem1"].Enabled = true;
                    editToolStripMenuItem.DropDown.Items["addTrackToFileToolStripMenuItem1"].Enabled = false;
                    editToolStripMenuItem.DropDown.Items["defaultToolStripMenuItem1"].Enabled = true;
                }
                else if (treeView1.SelectedNode.Tag is MKV)
                {
                    editToolStripMenuItem.DropDown.Items["playToolStripMenuItem1"].Enabled = true;
                    editToolStripMenuItem.DropDown.Items["oPenToolStripMenuItem1"].Enabled = false;
                    editToolStripMenuItem.DropDown.Items["addTrackToFileToolStripMenuItem1"].Enabled = true;
                    editToolStripMenuItem.DropDown.Items["defaultToolStripMenuItem1"].Enabled = false;
                }
                else if (treeView1.SelectedNode.Tag is MkvInfoStreamInfo)
                {
                    editToolStripMenuItem.DropDown.Items["playToolStripMenuItem1"].Enabled = false;
                    editToolStripMenuItem.DropDown.Items["oPenToolStripMenuItem1"].Enabled = false;
                    editToolStripMenuItem.DropDown.Items["addTrackToFileToolStripMenuItem1"].Enabled = false;
                    editToolStripMenuItem.DropDown.Items["defaultToolStripMenuItem1"].Enabled = true;
                }
                else
                {
                    editToolStripMenuItem.DropDown.Items["playToolStripMenuItem1"].Enabled = false;
                    editToolStripMenuItem.DropDown.Items["oPenToolStripMenuItem1"].Enabled = false;
                    editToolStripMenuItem.DropDown.Items["addTrackToFileToolStripMenuItem1"].Enabled = false;
                    editToolStripMenuItem.DropDown.Items["defaultToolStripMenuItem1"].Enabled = false;
                }
            }
        }

        /// <summary>
        /// Opens the selected subtitle stream in BDSup2Sub
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Subtitle)
            {
                Subtitle subtitle = treeView1.SelectedNode.Tag as Subtitle;
                BDSup2SubConnector.OpenSubtitles(subtitle);
            }
            else if (treeView1.SelectedNode.Tag is ForcedSubtitles)
            {
                ForcedSubtitles subtitle = treeView1.SelectedNode.Tag as ForcedSubtitles;
                BDSup2SubConnector.OpenSubtitles(subtitle);
            }
        }

        /// <summary>
        /// Opens the selected subtitle stream in BDSup2Sub
        /// </summary>
        private void oPenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Subtitle subtitle = treeView1.SelectedNode.Tag as Subtitle;
            BDSup2SubConnector.OpenSubtitles(subtitle);
        }

        /// <summary>
        /// Plays the selected MKV using the default application
        /// </summary>
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MKV mkv = treeView1.SelectedNode.Tag as MKV;
            Tools.OpenFile(mkv.FullPath);
        }

        /// <summary>
        /// Plays the selected MKV using the default application
        /// </summary>
        private void playToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MKV mkv = treeView1.SelectedNode.Tag as MKV;
            Tools.OpenFile(mkv.FullPath);
        }

        /// <summary>
        /// Clear the current list of MKVs
        /// </summary>
        private void buttonClear_Click(object sender, EventArgs e)
        {
            MKVs = new List<MKV>();
            displayMKVs();
            buttonMux.Enabled = false;
            buttonClear.Enabled = false;
        }

        /// <summary>
        /// Clear the current list of MKVs
        /// </summary>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MKVs = new List<MKV>();
            displayMKVs();
        }

        /// <summary>
        /// Adds an external file as a new stream in the selected MKV
        /// </summary>
        private void addTrackToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //display the open file dialog for the user to select a file to add as a stream
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = compatibleFileTypes;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //add each external file to the MKVs AdditionalStreams collection for muxing
                MKV mkv = treeView1.SelectedNode.Tag as MKV;
                foreach (string path in openFileDialog1.FileNames)
                    mkv.AdditionalStreams.Add(path);
                displayMKVs(); //update the view
                radioButtonKeepChecked.Checked = true; //select the most appropriate muxing option
            }
        }

        /// <summary>
        /// Selects the 'muxing only selected' muxing option after the user changes the check boxes
        /// </summary>
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            radioButtonKeepChecked.Checked = true;
        }

        /// <summary>
        /// Highlights which streams are the default audio/video/subtitle streams by ticking the 
        /// 'Default' option in both the context and edit menu
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //get the context menu default button
            ToolStripMenuItem menuItem = (ToolStripMenuItem)contextMenuStrip1.Items["defaultToolStripMenuItem"];

            if (e.Node.Tag is MkvInfoStreamInfo)
            {
                //enable the default button for audio and video streams
                contextMenuStrip1.Items["defaultToolStripMenuItem"].Enabled = true;
                MkvInfoStreamInfo stream = e.Node.Tag as MkvInfoStreamInfo;

                MKV mkv = treeView1.SelectedNode.Parent.Tag as MKV; //get the parent MKV
                switch (stream.Type)
                {
                    case StreamType.Audio:
                        //if the parent MKV has a new default audio stream waiting to be muxed..
                        if (mkv.HasNewDefaultAudio) 
                        {
                            //...use the IsNewDefault property to show if an audio stream is the default stream
                            menuItem.Checked = stream.IsNewDefault;
                            defaultToolStripMenuItem1.Checked = stream.IsNewDefault;
                        }
                        //if the parent MKV does not have a new default audio stream waiting to be muxed...
                        else
                        {
                            //...use the IsDefault property instead to show if the stream is the default audio stream
                            menuItem.Checked = stream.IsDefault;
                            defaultToolStripMenuItem1.Checked = stream.IsDefault;
                        }
                        break;
                    case StreamType.Video:
                        //if the parent MKV has a new default video stream waiting to be muxed..
                        if (mkv.HasNewDefaultVideo)
                        {
                            //...use the IsNewDefault property to show if a video stream is the default stream
                            menuItem.Checked = stream.IsNewDefault;
                            defaultToolStripMenuItem1.Checked = stream.IsNewDefault;
                        }
                        //if the parent MKV does not have a new default video stream waiting to be muxed...
                        else
                        {
                            //...use the IsDefault property instead to show if the stream is the default video stream
                            menuItem.Checked = stream.IsDefault;
                            defaultToolStripMenuItem1.Checked = stream.IsDefault;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (e.Node.Tag is Subtitle)
            {
                //enable the default button for subtitle streams 
                contextMenuStrip1.Items["defaultToolStripMenuItem"].Enabled = true;
                Subtitle stream = e.Node.Tag as Subtitle;
                MKV mkv = treeView1.SelectedNode.Parent.Tag as MKV;
                //if the parent MKV has a new default subtitles waiting to be muxed..
                if (mkv.HasNewDefaultSubtitles)
                {
                    //...use the IsNewDefault property to show if subtitles are the default stream
                    menuItem.Checked = stream.IsNewDefault;
                    defaultToolStripMenuItem1.Checked = stream.IsNewDefault;
                }
                else
                {
                    //...use the IsDefault property instead to show if subtitles are the default subtitles
                    menuItem.Checked = stream.IsDefault;
                    defaultToolStripMenuItem1.Checked = stream.IsDefault;
                }
            }
            else
                //disable the default button for all other stream types
                menuItem.Checked = false;
        }

        /// <summary>
        /// Changes the default stream to the stream selected
        /// </summary>
        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)contextMenuStrip1.Items["defaultToolStripMenuItem"];
            setDefault(menuItem);
        }

        /// <summary>
        /// Changes the default stream to the stream selected
        /// </summary>
        private void defaultToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setDefault(defaultToolStripMenuItem1);
        }

        /// <summary>
        /// Changes the default stream to the stream selected
        /// </summary>
        private void setDefault(ToolStripMenuItem menuItem)
        {
            radioButtonKeepChecked.Checked = true; //change the mux option to the most appropriate
            MKV mkv = treeView1.SelectedNode.Parent.Tag as MKV;

            //set the selected node as the default stream for the corresponding stream type
            if (treeView1.SelectedNode.Tag is MkvInfoStreamInfo)
            {
                MkvInfoStreamInfo stream = treeView1.SelectedNode.Tag as MkvInfoStreamInfo;
                if (menuItem.Checked && stream.Type == StreamType.Audio)
                {
                    mkv.SetNewDefaultAudio(stream.TID);
                    treeView1.SelectedNode.Checked = true; //include the new default stream in the new mux
                }
                else if (menuItem.Checked && stream.Type == StreamType.Video)
                {
                    mkv.SetNewDefaultVideo(stream.TID);
                    treeView1.SelectedNode.Checked = true;//include the new default stream in the new mux
                }
            }
            if (treeView1.SelectedNode.Tag is Subtitle)
            {
                Subtitle stream = treeView1.SelectedNode.Tag as Subtitle;
                if (menuItem.Checked)
                {
                    mkv.SetNewDefaultSubtitle(stream.TID);
                    treeView1.SelectedNode.Checked = true;//include the new default stream in the new mux
                }
            }
        }

        /// <summary>
        /// Show the options window
        /// </summary>
        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Options options = new Options();
            options.Show();
        }

        /// <summary>
        /// Close the application
        /// </summary>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Open the log file
        /// </summary>
        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Log.txt");
        }
    }

    /// <summary>
    /// The three different mux options
    /// </summary>
    enum MuxOptions
    {
        RemoveAll, //remove all unnecessary subtitles and audio streams
        KeepForced, //keep only forced subtitles and best audio stream
        UserChecked, //let the user select which streams to keep
    }

    /// <summary>
    /// A class containing the necessary information needed for muxing.  Is sent as an 
    /// parameter to the method that muxes asynchronously.
    /// </summary>
    class MuxArguments
    {
        public string OutputDirectory { get; private set; } //the directory to output the new files to
        public MuxOptions Option { get; private set; } //the type of muxing needed to be done

        public MuxArguments(string outputDirectory, MuxOptions option)
        {
            OutputDirectory = outputDirectory;
            Option = option;
        }
    }
}
