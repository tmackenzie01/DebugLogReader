using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogReaderArgs
    {
        public DebugLogReaderArgs(String directory, CameraDirectory camera)
        {
            m_directory = directory;
            m_camera = camera;
            m_filters = null;
        }

        public void AddFilters(List<DebugLogFilter> filter)
        {
            if (m_filters == null)
            {
                m_filters = new List<DebugLogFilter>();
            }

            if (filter != null)
            {
                m_filters.AddRange(filter);
            }
        }

        public String LogDirectory()
        {
            return $"{m_directory}\\{m_camera.CameraName.ToString()}_{m_camera.CameraNumber.ToString()}";
        }

        public CameraDirectory Camera
        {
            get
            {
                return m_camera;
            }
        }

        public String Directory
        {
            get
            {
                return m_directory;
            }
        }

        public List<DebugLogFilter> Filters
        {
            get
            {
                return m_filters;
            }
        }

        CameraDirectory m_camera;
        String m_directory;
        List<DebugLogFilter> m_filters;
    }
}
