using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogReadResult
    {
        public DebugLogReadResult(int cameraNumber)
        {
            m_success = false;
        }

        public DebugLogReadResult(int cameraNumber, DebugLog pushLog, DebugLog popLog)
        {
            m_success = true;
            m_cameraNumber = cameraNumber;
            m_pushLog = pushLog;
            m_popLog = popLog;
        }

        public override string ToString()
        {
            if (m_success)
            {
                return $"Camera {m_cameraNumber.ToString()} logs read, push log {m_pushLog.SummaryText()}, pop log {m_popLog.SummaryText()}";
            }
            else
            {
                return $"Camera {m_cameraNumber.ToString()} logs failed to read";
            }
        }

        public DebugLog PushLog
        {
            get
            {
                return m_pushLog;
            }
        }

        public DebugLog PopLog
        {
            get
            {
                return m_popLog;
            }
        }

        bool m_success;
        int m_cameraNumber;
        DebugLog m_pushLog;
        DebugLog m_popLog;

    }
}
