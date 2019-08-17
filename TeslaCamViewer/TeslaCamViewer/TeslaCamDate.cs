using System;

namespace TeslaCamViewer
{
    public class TeslaCamDate
    {
        private const string FileFormat = "yyyy-MM-ddThh:mm:ss";
        private const string DisplayFormat = "M/d/yyyy h:mm tt";

        public string UTCDateString { get; private set; }
        public string DisplayValue
        {
            get
            {

                return LocalTimeStamp.ToString(DisplayFormat);
            }
        }
        public DateTime UTCTimeStamp
        {
            get
            {
                DateTime dt;
                var result = DateTime.TryParse(UTCDateString, out dt);

                return dt;
            }
        }
        public DateTime LocalTimeStamp
        {
            get
            {

                return UTCTimeStamp;
            }
        }

        public TeslaCamDate(string DateString)
        {
            UTCDateString = DateString;
        }

    }
}
