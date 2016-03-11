using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogPushRow : DebugLogRow
    {
        public DebugLogPushRow()
        {
        }

        public DebugLogPushRow(int cameraNumber, String text, Regex r) : base(cameraNumber, text, r)
        {
            Initialise(cameraNumber, text, r, null, DateTime.MaxValue);
        }

        public DebugLogPushRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex) : base(cameraNumber, text, r, wroteDataRegex)
        {
            Initialise(cameraNumber, text, r, wroteDataRegex, DateTime.MaxValue);
        }

        public DebugLogPushRow(int cameraNumber, String text, Regex r, DateTime previousTimestamp) : base(cameraNumber, text, r, previousTimestamp)
        {
            Initialise(cameraNumber, text, r, null, previousTimestamp);
        }

        public DebugLogPushRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex, DateTime previousTimestamp) : base(cameraNumber, text, r, wroteDataRegex, previousTimestamp)
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
                if (!Int32.TryParse(match.Groups["pushedPopped"].Value, out m_dataPushedPopped))
                {
                    m_dataPushedPopped = -1;
                }
                if (!Int32.TryParse(match.Groups["queueCount"].Value, out m_queueCount))
                {
                    m_queueCount = -1;
                }
                m_timestamp = DateTime.ParseExact(timestamp, @"dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);

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
