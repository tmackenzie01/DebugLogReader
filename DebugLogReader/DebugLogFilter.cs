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
        public DebugLogFilter(eFilterBy filterBy, String filterData)
        {
            m_filterBy = filterBy;
            m_filterDataText = ""; // Not used so far

            switch (filterBy)
            {
                case eFilterBy.QueueCount:
                    Int32.TryParse(filterData, out m_filterDataInt);
                    break;
                case eFilterBy.StartTime:
                    m_filterDataDateTime = DateTime.ParseExact(filterData, @"dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    break;
            }
        }
        public DebugLogFilter(eFilterBy filterBy, List<int> filterData)
        {
            m_filterBy = filterBy;
            m_filterDataText = ""; // Not used so far

            switch (filterBy)
            {
                case eFilterBy.CameraNumber:
                    m_filterData = filterData;
                    break;
            }
        }

        public bool MeetsConditions(DebugLog log)
        {
            bool conditionsMet = false;

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
                    conditionsMet = (row.QueueCount > m_filterDataInt);
                    break;
                case eFilterBy.StartTime:
                    conditionsMet = (row.Timestamp > m_filterDataDateTime);
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
                    return $"_QueueCountGreaterThan{m_filterDataInt}";
                case eFilterBy.StartTime:
                    return $"_StartTimeGreaterThan{m_filterDataDateTime.ToString("HH:mm:ss")}";
                default:
                    return "";
            }
        }

        String m_filterDataText;
        Object m_filterData;
        int m_filterDataInt;
        DateTime m_filterDataDateTime;
        eFilterBy m_filterBy;
    }
    
    // Only filter by (equal to camera number / greater than queue count)
    // If we need less than/equal to then add another enum and change MeetsConditions
    public enum eFilterBy { CameraNumber, QueueCount, StartTime}
}
