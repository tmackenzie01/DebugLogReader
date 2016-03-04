using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogRowFilter
    {
        public DebugLogRowFilter(eFilterBy filterBy, String filterData)
        {
            m_filterBy = filterBy;
            m_filterDataText = ""; // Not used so far

            switch (filterBy)
            {
                case eFilterBy.CameraNumber:
                case eFilterBy.QueueCount:
                    Int32.TryParse(filterData, out m_filterDataInt);
                    break;
                case eFilterBy.StartTime:
                    m_filterDataDateTime = DateTime.ParseExact(filterData, @"dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    break;
            }
        }

        public bool MeetsConditions(DebugLogRow row)
        {
            bool conditionsMet = false;

            switch (m_filterBy)
            {
                case eFilterBy.CameraNumber:
                    conditionsMet = (row.CameraNumber == m_filterDataInt);
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
                    return $"_CamNumEqualTo{m_filterDataInt}";
                case eFilterBy.QueueCount:
                    return $"_QueueCountGreaterThan{m_filterDataInt}";
                case eFilterBy.StartTime:
                    return $"_StartTimeGreaterThan{m_filterDataDateTime.ToString("HH:mm:ss")}";
                default:
                    return "";
            }
        }

        String m_filterDataText;
        int m_filterDataInt;
        DateTime m_filterDataDateTime;
        eFilterBy m_filterBy;
    }
    
    // Only filter by (equal to camera number / greater than queue count)
    // If we need less than/equal to then add another enum and change MeetsConditions
    public enum eFilterBy { CameraNumber, QueueCount, StartTime}
}
