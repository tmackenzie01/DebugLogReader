using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogAviRow : DebugLogRow
    {
        public DebugLogAviRow()
        {
        }

        public DebugLogAviRow(int cameraNumber, String text, Regex r) : base(cameraNumber, text, r)
        {
            Initialise(cameraNumber, text, r, null, DateTime.MaxValue);
        }

        public DebugLogAviRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex) : base(cameraNumber, text, r, wroteDataRegex)
        {
            Initialise(cameraNumber, text, r, wroteDataRegex, DateTime.MaxValue);
        }

        public DebugLogAviRow(int cameraNumber, String text, Regex r, DateTime previousTimestamp) : base(cameraNumber, text, r, previousTimestamp)
        {
            Initialise(cameraNumber, text, r, null, previousTimestamp);
        }

        public DebugLogAviRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex, DateTime previousTimestamp) : base(cameraNumber, text, r, wroteDataRegex, previousTimestamp)
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
