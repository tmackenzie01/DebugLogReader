﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DebugLogReader
{
    public class DebugLogRowCS : DebugLogRowBase
    {
        public DebugLogRowCS()
        {
        }

        public DebugLogRowCS(int cameraNumber, String text) : base(cameraNumber, text)
        {
            Initialise(cameraNumber, text, DateTime.MaxValue);
        }

        public DebugLogRowCS(int cameraNumber, String text, DateTime previousTimestamp) : base(cameraNumber, text, previousTimestamp)
        {
            Initialise(cameraNumber, text, previousTimestamp);
        }

        protected override void Initialise(int cameraNumber, String text, DateTime previousTimestamp)
        {
            Regex r = LogRegex.m_csRegex;

            m_cameraNumber = cameraNumber;
            Match match = r.Match(text);
            if (match.Success)
            {
                String timestamp = match.Groups["timestamp"].Value;
                if (!String.IsNullOrEmpty(timestamp))
                {
                    m_timestamp = DateTime.ParseExact(timestamp, @"HH:mm:ss.fff", CultureInfo.InvariantCulture);
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
