using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public DebugLogFrameRow(int cameraNumber, String text) : base(cameraNumber, text)
        {
            Initialise(cameraNumber, text, DateTime.MaxValue);
        }

        public DebugLogFrameRow(int cameraNumber, String text, DateTime previousTimestamp) : base(cameraNumber, text, previousTimestamp)
        {
            Initialise(cameraNumber, text, previousTimestamp);
        }

        protected override void Initialise(int cameraNumber, String text, DateTime previousTimestamp)
        {
            Regex r = LogRegex.m_frameRegex;

            m_cameraNumber = cameraNumber;
            Match match = r.Match(text);
            if (match.Success)
            {
                String timestamp = match.Groups["timestamp"].Value;
                m_timestamp = DateTime.ParseExact(timestamp, @"HH:mm:ss.fff", CultureInfo.InvariantCulture);

                String totTimestamp = match.Groups["totTimestamp"].Value;
                bool timeParsed = false;
                try
                {
                    String totTimestampWithHoursMins = $"0:0:{totTimestamp}";
                    m_totalFrameProcessing = TimeSpan.Parse(totTimestampWithHoursMins, DateTimeFormatInfo.InvariantInfo);
                    timeParsed = true;
                }
                catch (OverflowException)
                {
                    // the totTimestamp is built from totalSeconds, most of the time it will be under a minute so the above easy way to 
                    // get the TimeSpan using Parse will work ok, but if the totalSeconds is above 60 then we'll get here instead
                    timeParsed = false;
                    Debug.WriteLine("Handling OverflowException");
                }

                if (!timeParsed)
                {
                    // Split the string and do it manually
                    String[] timestampSplit = totTimestamp.Split('.');
                    if (timestampSplit.Length == 2)
                    {
                        int secs = 0;
                        if (Int32.TryParse(timestampSplit[0], out secs))
                        {
                            int ms = 0;
                            if (Int32.TryParse(timestampSplit[1], out ms))
                            {
                                m_totalFrameProcessing = new TimeSpan(0, 0, 0, secs, ms);
                            }
                        }
                    }
                }

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
