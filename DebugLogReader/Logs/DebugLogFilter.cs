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
        public DebugLogFilter(eFilterBy filterBy, eFilterComparision filterComparison, Object filterData)
        {
            m_filterBy = filterBy;
            m_filterComparision = filterComparison;

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
                case eFilterBy.EndTime:
                    String timeText = (String)filterData;
                    bool timeParsed = false;
                    // Try it with the milliseconds
                    try
                    {
                        m_filterData = DateTime.ParseExact(timeText, @"dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        timeParsed = true;
                    }
                    catch
                    {
                        timeParsed = false;
                    }

                    if (!timeParsed)
                    {
                        m_filterData = DateTime.ParseExact(timeText, @"dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        timeParsed = true;
                    }
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
                    conditionsMet = CompareObjects(m_filterBy, m_filterComparision, log.CameraNumber, cameras);
                    //conditionsMet = cameras.Contains(log.CameraNumber);
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
                    conditionsMet = CompareObjects(m_filterBy, m_filterComparision, row.CameraNumber, cameras);
                    //conditionsMet = cameras.Contains(row.CameraNumber);
                    break;
                case eFilterBy.QueueCount:
                    int queueCount = (int)m_filterData;
                    conditionsMet = CompareObjects(m_filterBy, m_filterComparision, row.QueueCount, queueCount);
                    //conditionsMet = (row.QueueCount > queueCount);
                    break;
                case eFilterBy.StartTime:
                case eFilterBy.EndTime:
                    DateTime startTime = (DateTime)m_filterData;
                    conditionsMet = CompareObjects(m_filterBy, m_filterComparision, row.Timestamp, startTime);
                    //conditionsMet = (row.Timestamp > startTime);
                    break;
            }

            return conditionsMet;
        }

        private bool CompareObjects(eFilterBy filterBy, eFilterComparision filterComparision, Object rowData, Object filterData)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.GreaterThan:
                case eFilterComparision.LessThan:
                case eFilterComparision.EqualTo:
                    if ((rowData is DateTime) && (filterData is DateTime))
                    {
                        conditionsMet = PerformComparision2(filterComparision, (DateTime)rowData, (DateTime)filterData);
                    }
                    else if ((rowData is int) && (filterData is int))
                    {
                        conditionsMet = PerformComparision2(filterComparision, (int)rowData, (int)filterData);
                    }
                    else
                    {
                        throw new Exception($"Invalid comparision {filterComparision}");
                    }
                    break;
                case eFilterComparision.MemberOf:
                    if ((rowData is int) && (filterData is List<int>))
                    {
                        conditionsMet = PerformComparision2(filterComparision, (int)rowData, (List<int>)filterData);
                    }
                    else
                    {
                        throw new Exception($"Invalid comparision {filterComparision}");
                    }
                    break;
            }

            return conditionsMet;
        }

        private bool PerformComparision2(eFilterComparision filterComparision, DateTime rowDate, DateTime filterDate)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.LessThan:
                    conditionsMet = (rowDate < filterDate);
                    break;
                case eFilterComparision.EqualTo:
                    conditionsMet = (rowDate == filterDate);
                    break;
                case eFilterComparision.GreaterThan:
                    conditionsMet = (rowDate > filterDate);
                    break;
                default:
                    throw new Exception($"Invalid comparision {filterComparision} - DateTime");
            }

            return conditionsMet;
        }

        private bool PerformComparision2(eFilterComparision filterComparision, int rowInt, int filterInt)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.LessThan:
                    conditionsMet = (rowInt < filterInt);
                    break;
                case eFilterComparision.EqualTo:
                    conditionsMet = (rowInt == filterInt);
                    break;
                case eFilterComparision.GreaterThan:
                    conditionsMet = (rowInt > filterInt);
                    break;
                default:
                    throw new Exception($"Invalid comparision {filterComparision} - DateTime");
            }

            return conditionsMet;
        }

        private bool PerformComparision2(eFilterComparision filterComparision, int rowInt, List<int> filterInts)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.MemberOf:
                    conditionsMet = filterInts.Contains(rowInt);
                    break;
                default:
                    throw new Exception($"Invalid comparision {filterComparision} - DateTime");
            }

            return conditionsMet;
        }

        public override string ToString()
        {
            switch (m_filterBy)
            {
                case eFilterBy.CameraNumber:
                    List<int> cameras = (List<int>)m_filterData;
                    return $"_CamNum{m_filterComparision}{frmCameraSelection.CameraListToCSV(cameras)}";
                case eFilterBy.QueueCount:
                    int queueCount = (int)m_filterData;
                    return $"_QueueCount{m_filterComparision}{queueCount}";
                case eFilterBy.StartTime:
                case eFilterBy.EndTime:
                    DateTime time = (DateTime)m_filterData;
                    return $"_StartTime{m_filterComparision}{time.ToString("HHmmss")}";
                default:
                    return "";
            }
        }

        Object m_filterData;
        eFilterBy m_filterBy;
        eFilterComparision m_filterComparision;
    }

    // Only filter by (equal to camera number / greater than queue count)
    // If we need less than/equal to then add another enum and change MeetsConditions
    public enum eFilterBy { CameraNumber, QueueCount, StartTime, EndTime }

    public enum eFilterComparision { LessThan, EqualTo, GreaterThan, MemberOf }
}
