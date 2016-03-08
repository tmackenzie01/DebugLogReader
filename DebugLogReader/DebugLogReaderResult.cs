using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogReaderResult
    {
        public DebugLogReaderResult(int cameraNumber)
        {
            m_success = false;
        }

        public DebugLogReaderResult(int cameraNumber, DebugLog pushLog, DebugLog popLog)
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
                String pushLogSummary = "no push log";
                if (m_pushLog != null)
                {
                    pushLogSummary = $"push log {m_pushLog.SummaryText()}";
                }
                String popLogSummary = "no push log";
                if (m_popLog != null)
                {
                    popLogSummary = $"pop log {m_popLog.SummaryText()}";
                }
                return $"Camera {m_cameraNumber.ToString()} logs read, {pushLogSummary}, {popLogSummary}";
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
