using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogFilter
    {
        public DebugLogFilter(eFilterBy filterBy, Object filterData)
        {
            m_filterBy = filterBy;

            // Change each object into what it's supposed to be then stick in a object
            switch (filterBy)
            {
                case eFilterBy.QueueCount:
                    String queueCountText = (String)filterData;
                    int queueCount = 0;
                    if (Int32.TryParse(queueCountText, out queueCount))
                    {
                        m_filterData = queueCount;
                    }
                    break;
                case eFilterBy.StartTime:
                    String startTimeText = (String)filterData;
                    m_filterData = DateTime.ParseExact(startTimeText, @"dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    break;
                case eFilterBy.CameraNumber:
                    m_filterData = filterData;
                    break;
            }
        }

        public bool MeetsConditions(DebugLog log)
        {
            // Default is true as most conditions we can't check at the debug log stage only at the row stage
            bool conditionsMet = true;

            switch (m_filterBy)
            {
                case eFilterBy.CameraNumber:
                    List<int> cameras = (List<int>)m_filterData;
                    conditionsMet = cameras.Contains(log.CameraNumber);
                    break;
            }

            return conditionsMet;
        }

        public bool MeetsConditions(DebugLogRow row)
        {
            bool conditionsMet = false;

            switch (m_filterBy)
            {
                case eFilterBy.CameraNumber:
                    List<int> cameras = (List<int>)m_filterData;
                    conditionsMet = cameras.Contains(row.CameraNumber);
                    break;
                case eFilterBy.QueueCount:
                    int queueCount = (int)m_filterData;
                    conditionsMet = (row.QueueCount > queueCount);
                    break;
                case eFilterBy.StartTime:
                    DateTime startTime = (DateTime)m_filterData;
                    conditionsMet = (row.Timestamp > startTime);
                    break;
            }

            return conditionsMet;
        }

        public override string ToString()
        {
            switch (m_filterBy)
            {
                case eFilterBy.CameraNumber:
                    List<int> cameras = (List<int>)m_filterData;
                    return $"_CamNumEqualTo{frmCameraSelection.CameraListToCSV(cameras)}";
                case eFilterBy.QueueCount:
                    int queueCount = (int)m_filterData;
                    return $"_QueueCountGreaterThan{queueCount}";
                case eFilterBy.StartTime:
                    DateTime startTime = (DateTime)m_filterData;
                    return $"_StartTimeGreaterThan{startTime.ToString("HH:mm:ss")}";
                default:
                    return "";
            }
        }
        
        Object m_filterData;
        eFilterBy m_filterBy;
    }
    
    // Only filter by (equal to camera number / greater than queue count)
    // If we need less than/equal to then add another enum and change MeetsConditions
    public enum eFilterBy { CameraNumber, QueueCount, StartTime}
}
