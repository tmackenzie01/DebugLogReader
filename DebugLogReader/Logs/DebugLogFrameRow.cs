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
                totTimestamp = $"0:0:{totTimestamp}";
                m_totalFrameProcessing = TimeSpan.Parse(totTimestamp, DateTimeFormatInfo.InvariantInfo);

                m_rvException = !String.IsNullOrEmpty(match.Groups["rvException"].Value);

                if (m_totalFrameProcessing.TotalSeconds == 0.0f)
                {
                    // If the the total processing time is zero we don't need all the text
                    int iBracket = text.IndexOf("(");
                    if (iBracket > 0)
                    {
                        m_text = text.Substring(0, (iBracket - 1));
                    }
                    else
                    {
                        m_text = "";
                    }
                }
                else
                {
                    m_text = text;
                }
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

        public bool RVException
        {
            get
            {
                return m_rvException;
            }
        }

        private TimeSpan m_totalFrameProcessing;
        private bool m_rvException;
    }

}
