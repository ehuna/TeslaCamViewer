using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TeslaCamViewer
{
    /// <summary>
    /// Contains multiple TeslaCam File Sets making up one event
    /// Ex. A single Sentry Mode event
    /// </summary>
    public class TeslaCamEventCollection
    {
        public TeslaCamDate StartDate { get; private set; }

        public TeslaCamDate EndDate { get; private set; }

        public List<TeslaCamFileSet> Recordings { get; set; }

        public TeslaCamFile ThumbnailVideo
        {
            get
            {
                var recording = Recordings.FirstOrDefault();

                if (recording == default(TeslaCamFileSet))
                    return new TeslaCamFile("");

                return recording.ThumbnailVideo;
            }
        }

        public TeslaCamEventCollection()
        {
            this.Recordings = new List<TeslaCamFileSet>();
        }

        public bool BuildFromDirectory(string Directory)
        {
            // Get list of raw files
            var files = System.IO.Directory.GetFiles(Directory, "*.mp4").OrderBy(x=>x).ToArray();

            // Make sure there's at least one valid file
            if (files.Length < 1) { return false; }

            // Create a list of cam files
            var teslaCamFileList = new List<TeslaCamFile>(files.Length);

            // Convert raw file to cam file
            foreach (var file in files)
            {
                try
                {
                    var teslaCamFile = new TeslaCamFile(file);
                    teslaCamFileList.Add(teslaCamFile);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error adding '{file}': '{ex.Message}'");
                }
            }

            // Now get list of only distinct events
            List<string> DistinctEvents = teslaCamFileList.Select(e => e.Date.UTCDateString).Distinct().ToList();

            // Find the files that match the distinct event
            foreach (var CurrentEvent in DistinctEvents)
            {
                List<TeslaCamFile> MatchedFiles = teslaCamFileList.Where(e => e.Date.UTCDateString == CurrentEvent).ToList();
                TeslaCamFileSet CurrentFileSet = new TeslaCamFileSet();

                CurrentFileSet.SetCollection(MatchedFiles);
                this.Recordings.Add(CurrentFileSet);
            }

            if (this.Recordings.Count == 0)
                return false;

            // Set metadata
            this.Recordings = Recordings.OrderBy(e => e.Date.UTCDateString).ToList();
            this.StartDate = Recordings.First().Date;
            this.EndDate = Recordings.Last().Date;

            // Success
            return true;
        }
    }
}
