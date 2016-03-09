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

        public DebugLogReaderResult(int cameraNumber, List<DebugLog> logs)
        {
            m_success = true;
            m_cameraNumber = cameraNumber;
            m_logs = logs;
        }

        public override string ToString()
        {
            if (m_success)
            {
                StringBuilder logSummary = new StringBuilder();
                if (m_logs.Count > 0)
                {
                    logSummary.Append(m_logs[0].SummaryText());
                }

                for (int i = 1; i < m_logs.Count; i++)
                {
                    logSummary.Append($", {m_logs[i].SummaryText()}");
                }
                
                return $"Camera {m_cameraNumber.ToString()} logs read, {logSummary}";
            }
            else
            {
                return $"Camera {m_cameraNumber.ToString()} logs failed to read";
            }
        }    
        
        public List<DebugLog> Logs
        {
            get
            {
                return m_logs;
            }
        }

        bool m_success;
        int m_cameraNumber;
        List<DebugLog> m_logs;

    }
}
