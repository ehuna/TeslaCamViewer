using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaCamViewer
{
    /// <summary>
    /// A single TeslaCam File
    /// </summary>
    public class TeslaCamFile
    {
        public enum CameraType
        {
            UNKNOWN,
            LEFT_REPEATER,
            FRONT,
            RIGHT_REPEATER
        }
        private readonly string FileNameRegex = "[0-9]{4}-[0-9]{2}-[0-9]{2}_[0-9]{2}-[0-9]{2}-[a-z]*.mp4";
        public string FilePath { get; private set; }
        public string FileName { get { return System.IO.Path.GetFileName(FilePath); } }
        public TeslaCamDate Date { get; private set; }
        public CameraType CameraLocation { get; private set; }
        public string FileDirectory { get { return System.IO.Path.GetDirectoryName(FilePath); } }
        public Uri FileURI { get { return new Uri(this.FilePath); } }

        // D:\TeslaCam\RecentClips\2019-08-15_20-10-57-front.mp4
        // 2019-08-15_20-10-57-front.mp4

        public TeslaCamFile(string FilePath)
        {
            this.FilePath = FilePath;

            var fileName = 
                    FilePath
                    .Replace(@"D:\TeslaCam\RecentClips\", "")
                    .Replace(@"D:\TeslaCam\SavedClips\", "");

            var date = fileName.Substring(0, 10);
            var time = fileName.Substring(11, 8);

            var cameraType =
                    fileName
                    .Replace(date, "")
                    .Replace("_", "")
                    .Replace(time, "")
                    .Replace("-", "")
                    .Replace(".mp4", "")
                    .ToLowerInvariant();

            time = time.Replace("-", ":");
            this.Date = new TeslaCamDate($"{date}T{time}");

            if (cameraType == "front")
                CameraLocation = CameraType.FRONT;
            else if (cameraType == "leftrepeater")
                CameraLocation = CameraType.LEFT_REPEATER;
            else if (cameraType == "rightrepeater")
                CameraLocation = CameraType.RIGHT_REPEATER;
            else
                throw new Exception("Invalid Camera Type: '" + cameraType + "'");
        }

    }
}
