﻿using System;

namespace DebugLogReader
{
    public class DebugLogRowBase
    {
        public DebugLogRowBase()
        {
        }

        public DebugLogRowBase(int cameraNumber, String text)
        {
            Initialise(cameraNumber, text, DateTime.MaxValue);
        }

        public DebugLogRowBase(int cameraNumber, String text, DateTime previousTimestamp)
        {
            Initialise(cameraNumber, text, previousTimestamp);
        }

        protected virtual void Initialise(int cameraNumber, String text, DateTime previousTimestamp)
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
