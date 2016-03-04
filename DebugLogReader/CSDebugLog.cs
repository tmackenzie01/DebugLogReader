using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class CSDebugLog : DebugLog
    {
        public CSDebugLog(int cameraNumber, List<DebugLogRowFilter> filters) : base(cameraNumber, filters)
        {
        }

        protected override void InitialiseRegex()
        {
            m_rowRegex = frmDebugLogReader.m_csRegex;
        }
    }
}
