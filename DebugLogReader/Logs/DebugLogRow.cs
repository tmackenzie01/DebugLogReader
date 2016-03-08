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
                m_bWroteData = true;
                m_timestamp = previousTimestamp;
                m_text = $"Wrote data - {m_timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff")}";
            }
            else
            {
                m_text = text;
                // Can't figure out how else to do this
                if (text.EndsWith("F:Null"))
                {
                    text = text.Replace("F:Null", "F:0, 0, 0");
                }

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
                }
                else
                {
                    throw new Exception("Ooops");
                }
            }

            if (String.IsNullOrEmpty(m_text))
            {
                throw new Exception("Ooops");
            }
        }

        public void SetWroteDataElapsed(TimeSpan lastWroteElapsed)
        {
            m_lastWroteElapsed = lastWroteElapsed;
            if (lastWroteElapsed.TotalSeconds > 10)
            {
                System.Diagnostics.Debug.WriteLine(this.ToString());
            }
        }

        public void SetWroteDataWritten(int dataWritten)
        {
            m_dataWritten = dataWritten;
        }

        public String SortingText
        {
            get
            {
                return m_text;
            }
        }

        public override string ToString()
        {
            if (!m_bWroteData)
            {
                return $"{m_cameraNumber.ToString()} {m_text}";
            }
            else
            {
                double dataWritten = m_dataWritten;
                dataWritten = dataWritten / 1024.0;
                return $"{CameraNumber} {m_timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff")} ({m_lastWroteElapsed.TotalSeconds.ToString("f3")} seconds since last wrote data - {dataWritten.ToString("f3")}Kb)";
            }
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

        public int QueueCount
        {
            get
            {
                return m_queueCount;
            }
        }

        public bool WroteData
        {
            get
            {
                return m_bWroteData;
            }
        }

        public int DataPopped
        {
            get
            {
                return m_dataPushedPopped;
            }
        }

        String m_text;
        int m_cameraNumber;
        bool m_bWroteData;
        TimeSpan m_lastWroteElapsed;
        int m_dataWritten;
        int m_dataPushedPopped;
        int m_queueCount;
        DateTime m_timestamp;

        // Not storing any of these at the moment
        //int m_frameNumber;
        //int m_frameSize;
        //int m_flags;
        //String m_elapsed;
    }
}
