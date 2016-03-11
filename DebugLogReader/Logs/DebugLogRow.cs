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
            throw new Exception("Not implemented");
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
            return $"{m_cameraNumber.ToString()} {m_text}";
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

        public int DataPopped
        {
            get
            {
                return m_dataPushedPopped;
            }
        }

        public bool NullFrameDetected
        {
            get
            {
                return m_nullFrameDetected;
            }
        }

        protected String m_text;
        protected int m_cameraNumber;

        protected int m_dataPushedPopped;
        protected int m_queueCount;
        protected DateTime m_timestamp;

        protected bool m_nullFrameDetected;

        // Not storing any of these at the moment
        //int m_frameNumber;
        //int m_frameSize;
        //int m_flags;
        //String m_elapsed;
    }
}
