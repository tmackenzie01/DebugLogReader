using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogFrameRow : DebugLogRow
    {
        public DebugLogFrameRow()
        {
        }

        public DebugLogFrameRow(int cameraNumber, String text, Regex r) : base(cameraNumber, text, r)
        {
            Initialise(cameraNumber, text, r, null, DateTime.MaxValue);
        }

        public DebugLogFrameRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex) : base(cameraNumber, text, r, wroteDataRegex)
        {
            Initialise(cameraNumber, text, r, wroteDataRegex, DateTime.MaxValue);
        }

        public DebugLogFrameRow(int cameraNumber, String text, Regex r, DateTime previousTimestamp) : base(cameraNumber, text, r, previousTimestamp)
        {
            Initialise(cameraNumber, text, r, null, previousTimestamp);
        }

        public DebugLogFrameRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex, DateTime previousTimestamp) : base(cameraNumber, text, r, wroteDataRegex, previousTimestamp)
        {
            Initialise(cameraNumber, text, r, wroteDataRegex, previousTimestamp);
        }

        protected override void Initialise(int cameraNumber, String text, Regex r, Regex wroteDataRegex, DateTime previousTimestamp)
        {
            m_cameraNumber = cameraNumber;
            Match match = r.Match(text);
            if (match.Success)
            {
                String timestamp = match.Groups["timestamp"].Value;
                m_timestamp = DateTime.ParseExact(timestamp, @"HH:mm:ss.fff", CultureInfo.InvariantCulture);

                String totTimestamp = match.Groups["totTimestamp"].Value;

                String[] splitTime = totTimestamp.Split('.');
                if (splitTime.Length == 2)
                {
                    int secs = -1;
                    int ms = -1;
                    if (Int32.TryParse(splitTime[0], out secs))
                    {
                        if (Int32.TryParse(splitTime[1], out ms))
                        {
                            m_totalFrameProcessing = new TimeSpan(0, 0, 0, secs, ms);
                        }
                    }
                }
                else
                {
                    throw new Exception("Ooops");
                }

                m_text = text;
            }
            else
            {
                throw new Exception("Ooops");
            }

            if (String.IsNullOrEmpty(m_text))
            {
                throw new Exception("Ooops");
            }
        }

        public TimeSpan TotalFrameProcessing
        {
            get
            {
                return m_totalFrameProcessing;
            }
        }

        private TimeSpan m_totalFrameProcessing;
    }

}
