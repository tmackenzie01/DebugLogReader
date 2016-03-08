using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogCSRow : DebugLogRow
    {
        public DebugLogCSRow()
        {
        }

        public DebugLogCSRow(int cameraNumber, String text, Regex r) : base(cameraNumber, text, r)
        {
            Initialise(cameraNumber, text, r, null, DateTime.MaxValue);
        }

        public DebugLogCSRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex) : base(cameraNumber, text, r, wroteDataRegex)
        {
            Initialise(cameraNumber, text, r, wroteDataRegex, DateTime.MaxValue);
        }

        public DebugLogCSRow(int cameraNumber, String text, Regex r, DateTime previousTimestamp) : base(cameraNumber, text, r, previousTimestamp)
        {
            Initialise(cameraNumber, text, r, null, previousTimestamp);
        }

        public DebugLogCSRow(int cameraNumber, String text, Regex r, Regex wroteDataRegex, DateTime previousTimestamp) : base(cameraNumber, text, r, wroteDataRegex, previousTimestamp)
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
