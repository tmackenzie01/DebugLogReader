using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class DebugLogFilter
    {
        public DebugLogFilter(String propertyName, eFilterComparision filterComparison, Object filterData)
        {
            m_filterPropertyName = propertyName;
            m_filterComparision = filterComparison;
            m_filterData = filterData;
        }

        public bool MeetsConditions(DebugLog log)
        {
            // Default is true as most conditions we can't check at the debug log stage only at the row stage
            bool conditionsMet = true;

            Type logTypeObject = log.GetType();
            PropertyInfo selectedPropertyInfo = logTypeObject.GetProperty(m_filterPropertyName);
            // null means the property is not supported
            if (selectedPropertyInfo != null)
            {
                Type selectedPropertyType = selectedPropertyInfo.PropertyType;
                Object selectedPropertyValue = selectedPropertyInfo.GetValue(log);

                if (m_filterPropertyName.Equals("CameraNumber"))
                {
                    List<int> cameras = (List<int>)m_filterData;
                    conditionsMet = CompareObjects44(m_filterComparision, selectedPropertyValue, m_filterData, selectedPropertyType);
            }
            }

            return conditionsMet;
        }

        public bool MeetsConditions(DebugLogRow row)
        {
            bool conditionsMet = false;

            Type rowTypeObject = row.GetType();
            PropertyInfo selectedPropertyInfo = rowTypeObject.GetProperty(m_filterPropertyName);

            // null means the property is not supported
            if (selectedPropertyInfo != null)
            {
                Type selectedPropertyType = selectedPropertyInfo.PropertyType;
                var selectedPropertyValue = selectedPropertyInfo.GetValue(row);

                conditionsMet = CompareObjects44(m_filterComparision, selectedPropertyValue, m_filterData, selectedPropertyType);
            }

            return conditionsMet;
        }

        private bool CompareObjects44(eFilterComparision filterComparision, Object actualData, Object filterData, Type propertyType)
        {
            bool conditionsMet = false;

            if ((propertyType.Equals(typeof(int))) && (filterData is int))
            {
                conditionsMet = PerformComparision2(filterComparision, (int)actualData, (int)filterData);
            }
            else if ((propertyType.Equals(typeof(int))) && (filterData is List<int>))
            {
                conditionsMet = PerformComparision2(filterComparision, (int)actualData, (List<int>)filterData);
            }
            else if ((propertyType.Equals(typeof(DateTime))) && (filterData is DateTime))
            {
                conditionsMet = PerformComparision2(filterComparision, (DateTime)actualData, (DateTime)filterData);
            }
            else if ((propertyType.Equals(typeof(TimeSpan))) && (filterData is TimeSpan))
            {
                conditionsMet = PerformComparision2(filterComparision, (TimeSpan)actualData, (TimeSpan)filterData);
            }
            else
            {
                throw new Exception("Oops");
            }

            return conditionsMet;
        }

        private bool CompareObjects(eFilterComparision filterComparision, Object actualData, Object filterData)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.GreaterThan:
                case eFilterComparision.LessThan:
                case eFilterComparision.EqualTo:
                    if ((actualData is DateTime) && (filterData is DateTime))
                    {
                        conditionsMet = PerformComparision2(filterComparision, (DateTime)actualData, (DateTime)filterData);
                    }
                    else if ((actualData is int) && (filterData is int))
                    {
                        conditionsMet = PerformComparision2(filterComparision, (int)actualData, (int)filterData);
                    }
                    else if ((actualData is TimeSpan) && (filterData is TimeSpan))
                    {
                        conditionsMet = PerformComparision2(filterComparision, (TimeSpan)actualData, (TimeSpan)filterData);
                    }
                    else if ((rowData is bool) && (filterData is bool))
                    {
                        conditionsMet = PerformComparision2(filterComparision, (bool)rowData, (bool)filterData);
                    }
                    else
                    {
                        throw new Exception($"Invalid comparision {filterComparision}");
                    }
                    break;
                case eFilterComparision.MemberOf:
                    if ((actualData is int) && (filterData is List<int>))
                    {
                        conditionsMet = PerformComparision2(filterComparision, (int)actualData, (List<int>)filterData);
                    }
                    else
                    {
                        throw new Exception($"Invalid comparision {filterComparision}");
                    }
                    break;
            }

            return conditionsMet;
        }

        private bool PerformComparision2(eFilterComparision filterComparision, DateTime actualDate, DateTime filterDate)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.LessThan:
                    conditionsMet = (actualDate < filterDate);
                    break;
                case eFilterComparision.EqualTo:
                    conditionsMet = (actualDate == filterDate);
                    break;
                case eFilterComparision.GreaterThan:
                    conditionsMet = (actualDate > filterDate);
                    break;
                default:
                    throw new Exception($"Invalid comparision {filterComparision} - DateTime");
            }

            return conditionsMet;
        }

        private bool PerformComparision2(eFilterComparision filterComparision, TimeSpan actualTime, TimeSpan filterTime)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.LessThan:
                    conditionsMet = (actualTime < filterTime);
                    break;
                case eFilterComparision.EqualTo:
                    conditionsMet = (actualTime == filterTime);
                    break;
                case eFilterComparision.GreaterThan:
                    conditionsMet = (actualTime > filterTime);
                    break;
                default:
                    throw new Exception($"Invalid comparision {filterComparision} - TimeSpan");
            }

            return conditionsMet;
        }

        private bool PerformComparision2(eFilterComparision filterComparision, int actualInt, int filterInt)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.LessThan:
                    conditionsMet = (actualInt < filterInt);
                    break;
                case eFilterComparision.EqualTo:
                    conditionsMet = (actualInt == filterInt);
                    break;
                case eFilterComparision.GreaterThan:
                    conditionsMet = (actualInt > filterInt);
                    break;
                default:
                    throw new Exception($"Invalid comparision {filterComparision} - int");
            }

            return conditionsMet;
        }

        private bool PerformComparision2(eFilterComparision filterComparision, int actualInt, List<int> filterInts)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.MemberOf:
                    conditionsMet = filterInts.Contains(actualInt);
                    break;
                default:
                    throw new Exception($"Invalid comparision {filterComparision} - DateTime");
            }

            return conditionsMet;
        }

        private bool PerformComparision2(eFilterComparision filterComparision, bool rowVal, bool filterVal)
        {
            bool conditionsMet = false;

            switch (filterComparision)
            {
                case eFilterComparision.EqualTo:
                    conditionsMet = (rowVal == filterVal);
                    break;
                default:
                    throw new Exception($"Invalid comparision {filterComparision} - bool");
            }

            return conditionsMet;
        }
        String m_filterPropertyName;
        Object m_filterData;
        eFilterComparision m_filterComparision;
    }

    public enum eFilterComparision { LessThan, EqualTo, GreaterThan, MemberOf }
}
