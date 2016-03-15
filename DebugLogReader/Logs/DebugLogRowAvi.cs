using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DebugLogReader
{
    public class DebugLogAviRow : DebugLogRow
    {
        public DebugLogAviRow()
        {
        }

        public DebugLogAviRow(int cameraNumber, String text) : base(cameraNumber, text)
        {
            Initialise(cameraNumber, text, DateTime.MaxValue);
        }

        public DebugLogAviRow(int cameraNumber, String text, DateTime previousTimestamp) : base(cameraNumber, text, previousTimestamp)
        {
            Initialise(cameraNumber, text, previousTimestamp);
        }

        protected override void Initialise(int cameraNumber, String text, DateTime previousTimestamp)
        {
            Regex r = LogRegex.m_aviRegex;

            m_cameraNumber = cameraNumber;
            Match match = r.Match(text);
            if (match.Success)
            {
                String timestamp = match.Groups["timestamp"].Value;
                m_timestamp = DateTime.ParseExact(timestamp, @"HH:mm:ss.fff", CultureInfo.InvariantCulture);

                // Can't get match crException to work so just search for CRX
                m_crException = text.Contains("CRX:");
                m_crError = !String.IsNullOrEmpty(match.Groups["creTimestamp"].Value);

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

        public bool CRException
        {
            get
            {
                return m_crException;
            }
        }

        public bool CRError
        {
            get
            {
                return m_crError;
            }
        }

        bool m_crException;
        bool m_crError;
    }
}
