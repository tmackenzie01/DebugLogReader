using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DebugLogReader
{
    public class DebugLogPushRow : DebugLogRow
    {
        public DebugLogPushRow()
        {
        }

        public DebugLogPushRow(int cameraNumber, String text) : base(cameraNumber, text)
        {
            Initialise(cameraNumber, text, DateTime.MaxValue);
        }

        public DebugLogPushRow(int cameraNumber, String text, DateTime previousTimestamp) : base(cameraNumber, text, previousTimestamp)
        {
            Initialise(cameraNumber, text, previousTimestamp);
        }

        protected override void Initialise(int cameraNumber, String text, DateTime previousTimestamp)
        {
            Regex r = LogRegex.m_pushedRegex;

            m_cameraNumber = cameraNumber;
            Match match = r.Match(text);
            if (match.Success)
            {
                String timestamp = match.Groups["timestamp"].Value;
                if (!Int32.TryParse(match.Groups["pushedPopped"].Value, out m_dataPushedPopped))
                {
                    m_dataPushedPopped = -1;
                }
                if (!Int32.TryParse(match.Groups["queueCount"].Value, out m_queueCount))
                {
                    m_queueCount = -1;
                }
                m_timestamp = DateTime.ParseExact(timestamp, @"HH:mm:ss.fff", CultureInfo.InvariantCulture);

                String frameNo = match.Groups["frameNo"].Value;
                if (!String.IsNullOrEmpty(frameNo))
                {
                    // The N of Null matches to a . in the Regex
                    if (frameNo.Equals("ull"))
                    {
                        m_nullFrameDetected = true;
                    }
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
    }
}
