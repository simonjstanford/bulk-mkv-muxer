This a C#, multithreaded, time saving application that allows for batch muxing of 
multiple MKVs.  Typically, this application would be used following the creation 
of a set of MKVs from MakeMKV (http://makemkv.com/). A user would select the 
appropriate tracks to extract, and keep all streams with the tracks selected. Bulk 
MKV Muxer would then be used to scan the resulting MKVs to determine if any forced
captions are present and to select the most appropriate video, audio and subtitle
streams to keep in the MKV.


Features:

-Subtitles are automatically extracted and scanned for forced captions. 
-Video, audio and subtitle streams can be easily muxed out.
-External video, audio or subtitle files can be easily muxed in.  The file must be 
 compatible with MKV Merge (see below).
-Software logic suggests the most suitable streams to keep; a DTS audio stream 
 would be chosen over Dolby Digital, and most subtitle streams would be muxed out. 
 The only subtitle streams that would be kept in the MKVs would be English 
 language streams that only contain a few captions.  If forced captions are found 
 in any of the English language streams, these are added as a separate stream. 
 This suggestion is easily overridden.
-A default stream can be specified when an MKV has multiple video, audio or 
 subtitle streams.
-Drag and drop functionality for files and directories.
-Writes all output to a log file.


CLI Implementations

-Implements the MKV Info CLI to read and display info for the component video, 
 audio and subtitle streams of an MKV.
-Implements the MKV Extract CLI to extract all English subtitles from an MKV.
-Implements an embedded version of BDSp2Sub to determine the number of subtitle 
 captions for all English subtitles, and to extract all forced captions.
-Implements the MKV Merge CLI for bulk muxing.


Prerequisites

-MKVToolNix must be installed, and the paths to relevant components must be 
 specified in the MKV Bulk Muxer options menu. MKVToolNix can be downloaded from 
 http://www.bunkus.org/videotools/mkvtoolnix/downloads.html


File types compatible with MKV Merge that can be muxed into MKVs:

ac3, eac3, aac, m4a, mp4, 264, avc, h264, x264, avi, caf, m4a, mp4, drc, thd, 
thd+ac3, truehd, true-hd, dts, dtshd, dts-hd, flac, ogg, flv, ivf, mp4., m4v, mp2, 
mp3, mpg, mpeg, m2v, mpv, evo, evob, vob, ts, m2ts, mts, m1v, m2v, mpv, mpls, mka, 
mks, mkv, mk3d, webm, webmv, webma, sup, mov, ogg, ogm, ogv, opus, ogg, ra, ram, 
rm, rmvb, rv, srt, ass, ssa, tta, usf, xml,vc1, btn, idx, wav, wv, webm, webmv, 
webma.
