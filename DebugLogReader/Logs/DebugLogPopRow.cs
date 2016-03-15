using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DebugLogReader
{
    public class DebugLogPopRow : DebugLogRow
    {
        public DebugLogPopRow()
        {
        }

        public DebugLogPopRow(int cameraNumber, String text) : base(cameraNumber, text)
        {
            Initialise(cameraNumber, text, DateTime.MaxValue);
        }

        public DebugLogPopRow(int cameraNumber, String text, DateTime previousTimestamp) : base(cameraNumber, text, previousTimestamp)
        {
            Initialise(cameraNumber, text, previousTimestamp);
        }

        protected override void Initialise(int cameraNumber, String text, DateTime previousTimestamp)
        {
            Regex wroteDataRegex = LogRegex.m_wroteDataRegex;
            Regex r = LogRegex.m_poppedRegex;

            m_cameraNumber = cameraNumber;
            m_coldstoreId = -1; // Coldstore Id 0 is valid so make sure it's not that by default
            
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

                        if ((timeA.Equals(timeB)) && (timeA.Equals(timeC)) && (timeA.Equals(timeD)))
                        {
                            // Clear all these times no need to display them the intervals between all are zero
                            text = text.Replace($" T:A {timeAText} B {timeBText}", "");
                            text = text.Replace($" C {timeCText}", "");
                            text = text.Replace($" D {timeDText}", "");
                        }
                        else
                        {
                            m_timeElapsedB = timeB - timeA;
                            m_timeElapsedC = timeC - timeB;
                            m_timeElapsedD = timeD - timeC;

                            text = text.Replace($"A {timeAText} B {timeBText}", $"A {m_timeElapsedB.TotalSeconds.ToString("f3")}");
                            text = text.Replace($"C {timeCText}", $"C {m_timeElapsedC.TotalSeconds.ToString("f3")}");
                            text = text.Replace($"D {timeDText}", $"D {m_timeElapsedD.TotalSeconds.ToString("f3")}");
                        }
                    }

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
            }

            if (String.IsNullOrEmpty(m_text))
            {
                throw new Exception("Ooops");
            }
        }

        public override string ToString()
        {
            if (!m_bWroteData)
            {
                return base.ToString();
            }
            else
            {
                String nullFrameDetected = "";
                if (m_newWroteData)
                {
                    nullFrameDetected = " NULL frame previously";
                }
                double dataWritten = m_dataWritten;
                dataWritten = dataWritten / 1024.0;
                return $"{CameraNumber} {m_timestamp.ToString("HH:mm:ss.fff")} ({m_lastWroteElapsed.TotalSeconds.ToString("f3")} seconds since last wrote data - {dataWritten.ToString("f3")}Kb {nullFrameDetected}) {m_coldstoreId}:{m_coldstorePort}";
            }
        }

        public void SetWroteDataElapsed(TimeSpan lastWroteElapsed)
        {
            m_lastWroteElapsed = lastWroteElapsed;
        }

        public void SetColdstoreId(int coldstoreId)
        {
            m_coldstoreId = coldstoreId;
        }

        public void SetWroteDataWritten(int dataWritten)
        {
            m_dataWritten = dataWritten;
        }

        public bool WroteData
        {
            get
            {
                return m_bWroteData;
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

        public bool ColdstoreInformationDetected
        {
            get
            {
                return (m_coldstoreId != -1);
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



        public bool NewWroteData
        {
            set
            {
                m_newWroteData = value;
            }
        }

        int m_coldstoreId;
        int m_coldstorePort;

        TimeSpan m_timeElapsedB;
        TimeSpan m_timeElapsedC;
        TimeSpan m_timeElapsedD;

        TimeSpan m_lastWroteElapsed;

        bool m_bWroteData;      // Whenever we write data to the Coldstore
        bool m_newWroteData;    // First write data after a recording stop (detected by a null frame)
        int m_dataWritten;
    }
}
