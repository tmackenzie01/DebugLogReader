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
                case eFilterBy.ColdstoreId:
                    m_filterData = (int)filterData;
                    break;
                case eFilterBy.StartTime:
                case eFilterBy.EndTime:
                    m_filterData = (DateTime)filterData;
                    break;
                case eFilterBy.CameraNumber:
                    m_filterData = (List<int>)filterData;
                    break;
                case eFilterBy.LastWroteElapsed:
                    m_filterData = (TimeSpan)filterData;
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
                    break;
                case eFilterBy.QueueCount:
                    int queueCount = (int)m_filterData;
                    conditionsMet = CompareObjects(m_filterBy, m_filterComparision, row.QueueCount, queueCount);
                    break;
                case eFilterBy.ColdstoreId:
                    int coldstoreId = (int)m_filterData;
                    conditionsMet = CompareObjects(m_filterBy, m_filterComparision, row.ColdstoreId, coldstoreId);
                    break;
                case eFilterBy.StartTime:
                case eFilterBy.EndTime:
                    DateTime startTime = (DateTime)m_filterData;
                    conditionsMet = CompareObjects(m_filterBy, m_filterComparision, row.Timestamp, startTime);
                    break;
                case eFilterBy.LastWroteElapsed:
                    TimeSpan lastWroteElapsed = (TimeSpan)m_filterData;
                    conditionsMet = CompareObjects(m_filterBy, m_filterComparision, row.LastWroteDataElapsed, lastWroteElapsed);
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
                    else if ((rowData is TimeSpan) && (filterData is TimeSpan))
                    {
                        conditionsMet = PerformComparision2(filterComparision, (TimeSpan)rowData, (TimeSpan)filterData);
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

        private bool PerformComparision2(eFilterComparision filterComparision, TimeSpan rowDate, TimeSpan filterDate)
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
                    throw new Exception($"Invalid comparision {filterComparision} - TimeSpan");
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
            StringBuilder text = new StringBuilder($"_{m_filterBy}");
            switch (m_filterBy)
            {
                case eFilterBy.CameraNumber:
                    List<int> cameras = (List<int>)m_filterData;
                    text.Append($"{m_filterComparision}{frmCameraSelection.CameraListToCSV(cameras)}");
                    break;
                case eFilterBy.QueueCount:
                case eFilterBy.ColdstoreId:
                    int queueCount = (int)m_filterData;
                    text.Append($"{m_filterComparision}{queueCount}");
                    break;
                case eFilterBy.StartTime:
                case eFilterBy.EndTime:
                    DateTime time = (DateTime)m_filterData;
                    text.Append($"{m_filterComparision}{time.ToString("HHmmss")}");
                    break;
                case eFilterBy.LastWroteElapsed:
                    TimeSpan elapsed = (TimeSpan)m_filterData;
                    text.Append($"{m_filterComparision}{(int)elapsed.TotalSeconds}");
                    break;
                default:
                    break;
            }

            return text.ToString();
        }

        Object m_filterData;
        eFilterBy m_filterBy;
        eFilterComparision m_filterComparision;
    }

    // Only filter by (equal to camera number / greater than queue count)
    // If we need less than/equal to then add another enum and change MeetsConditions
    public enum eFilterBy { CameraNumber, QueueCount, StartTime, EndTime, LastWroteElapsed, ColdstoreId }

    public enum eFilterComparision { LessThan, EqualTo, GreaterThan, MemberOf }
}
