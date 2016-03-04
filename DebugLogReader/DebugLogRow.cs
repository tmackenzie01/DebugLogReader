using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogRow
    {
        public DebugLogRow(int cameraNumber, String text, Regex r)
        {
            Initialise(cameraNumber, text, r, DateTime.MaxValue);
        }

        public DebugLogRow(int cameraNumber, String text, Regex r, DateTime previousTimestamp)
        {
            Initialise(cameraNumber, text, r, previousTimestamp);
        }

        private void Initialise(int cameraNumber, String text, Regex r, DateTime previousTimestamp)
        {
            m_cameraNumber = cameraNumber;

            // Try the Regex
            if (text.Equals("Wrote data"))
            {
                m_timestamp = previousTimestamp;
                m_text = $"{m_cameraNumber.ToString()} Wrote data - {m_timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff")}";
            }
            else
            {
                // Can't figure out how else to do this
                if (text.EndsWith("F:Null"))
                {
                    text = text.Replace("F:Null", "F:0, 0, 0");
                }

                Match match = r.Match(text);
                if (match.Success)
                {
                    String timestamp = match.Groups["timestamp"].Value;
                    m_timestamp = DateTime.ParseExact(timestamp, @"dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    m_text = $"{m_cameraNumber.ToString()} {text}";
                }
                else
                {
                    throw new Exception("Ooops");
                }
            }

            if (String.IsNullOrEmpty(m_text))
            {
                int dfgd = 0;
            }
        }

        public override string ToString()
        {
            return m_text;
        }

        public DateTime Timestamp
        {
            get
            {
                return m_timestamp;
            }
        }

        public int CameraNumber
        {
            get
            {
                return m_cameraNumber;
            }
        }

        String m_text;
        int m_cameraNumber;

        // Not storing any of these at the moment
        int m_queue;
        int m_frameNumber;
        int m_frameSize;
        int m_flags;
        String m_elapsed;
        DateTime m_timestamp;
    }
}
