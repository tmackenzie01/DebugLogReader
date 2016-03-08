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
        public DebugLogRow()
        {
        }

        public DebugLogRow(int cameraNumber, String text, Regex r)
        {
            Initialise(cameraNumber, text, r, null, DateTime.MaxValue);
        }

        public DebugLogRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex)
        {
            Initialise(cameraNumber, text, r, wroteDataRegex, DateTime.MaxValue);
        }

        public DebugLogRow(int cameraNumber, String text, Regex r, DateTime previousTimestamp)
        {
            Initialise(cameraNumber, text, r, null, previousTimestamp);
        }

        public DebugLogRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex, DateTime previousTimestamp)
        {
            Initialise(cameraNumber, text, r, wroteDataRegex, previousTimestamp);
        }

        protected virtual void Initialise(int cameraNumber, String text, Regex r, Regex wroteDataRegex, DateTime previousTimestamp)
        {
            m_cameraNumber = cameraNumber;

            // Try the Regex
            if (wroteDataRegex != null)
            {
                Match match = wroteDataRegex.Match(text);
                if (match.Success)
                {
                    m_bWroteData = true;
                    m_timestamp = previousTimestamp;
                    m_text = $"Wrote data - {m_timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff")}";
                    String coldstoreId = match.Groups["coldstoreId"].Value;

                    if (!String.IsNullOrEmpty(coldstoreId))
                    {
                        Int32.TryParse(coldstoreId, out m_coldstoreId);
                    }
                    String coldstorePort = match.Groups["coldstorePort"].Value;
                    if (!String.IsNullOrEmpty(coldstorePort))
                    {
                        Int32.TryParse(coldstorePort, out m_coldstorePort);
                    }
                }
            }

            if (!m_bWroteData)
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
                    if (!Int32.TryParse(match.Groups["pushedPopped"].Value, out m_dataPushedPopped))
                    {
                        m_dataPushedPopped = -1;
                    }
                    if (!Int32.TryParse(match.Groups["queueCount"].Value, out m_queueCount))
                    {
                        m_queueCount = -1;
                    }
                    m_timestamp = DateTime.ParseExact(timestamp, @"dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);

                    DateTime timeA = DateTime.MinValue;
                    DateTime timeB = DateTime.MinValue;
                    DateTime timeC = DateTime.MinValue;
                    DateTime timeD = DateTime.MinValue;

                    String timeAText = match.Groups["timeA"].Value;
                    String timeBText = match.Groups["timeB"].Value;
                    String timeCText = match.Groups["timeC"].Value;
                    String timeDText = match.Groups["timeD"].Value;
                    if (!String.IsNullOrEmpty(timeAText))
                    {
                        timeA = DateTime.ParseExact(timeAText, @"HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        // Make sure we put something in B, C & D so we don't get crazy values later
                        timeB = timeA;
                        timeC = timeA;
                        timeD = timeA;
                        if (!String.IsNullOrEmpty(timeBText))
                        {
                            timeB = DateTime.ParseExact(timeBText, @"HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        }
                        if (!String.IsNullOrEmpty(timeCText))
                        {
                            timeC = DateTime.ParseExact(timeCText, @"HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        }
                        if (!String.IsNullOrEmpty(timeDText))
                        {
                            timeD = DateTime.ParseExact(timeDText, @"HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        }
                    }

                    m_timeElapsedB = timeB - timeA;
                    m_timeElapsedC = timeC - timeB;
                    m_timeElapsedD = timeD - timeC;

                    text = text.Replace($"A {timeAText} B {timeBText}", $"A {m_timeElapsedB.TotalSeconds.ToString("f3")}");
                    text = text.Replace($"C {timeCText}", $"C {m_timeElapsedC.TotalSeconds.ToString("f3")}");
                    text = text.Replace($"D {timeDText}", $"D {m_timeElapsedD.TotalSeconds.ToString("f3")}");
                    m_text = text;
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
                return $"{CameraNumber} {m_timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff")} ({m_lastWroteElapsed.TotalSeconds.ToString("f3")} seconds since last wrote data - {dataWritten.ToString("f3")}Kb) {m_coldstoreId}:{m_coldstorePort}";
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

        public TimeSpan LastWroteDataElapsed
        {
            get
            {
                return m_lastWroteElapsed;
            }
        }

        public int ColdstoreId
        {
            get
            {
                return m_coldstoreId;
            }
        }

        public int ColdstorePort
        {
            get
            {
                return m_coldstorePort;
            }
        }

        public TimeSpan TimeElapsedB
        {
            get
            {
                return m_timeElapsedB;
            }
        }

        public TimeSpan TimeElapsedC
        {
            get
            {
                return m_timeElapsedC;
            }
        }

        public TimeSpan TimeElapsedD
        {
            get
            {
                return m_timeElapsedD;
            }
        }

        protected String m_text;
        protected int m_cameraNumber;
        bool m_bWroteData;
        TimeSpan m_lastWroteElapsed;
        int m_dataWritten;
        int m_dataPushedPopped;
        int m_queueCount;
        protected DateTime m_timestamp;

        int m_coldstoreId;
        int m_coldstorePort;

        TimeSpan m_timeElapsedB;
        TimeSpan m_timeElapsedC;
        TimeSpan m_timeElapsedD;

        // Not storing any of these at the moment
        //int m_frameNumber;
        //int m_frameSize;
        //int m_flags;
        //String m_elapsed;
    }
}
