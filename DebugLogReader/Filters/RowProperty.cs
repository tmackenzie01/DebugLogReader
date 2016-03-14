using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLogReader
{
    public class RowProperty
    {
        public RowProperty(Type classType, String propertyName, Type propertyType)
        {
            m_classType = classType;
            m_propertyName = propertyName;
            m_propertyType = propertyType;

            m_shortClassName = ShortenClassName(m_classType);
            m_shortPropertyClassName = ShortenClassName(m_propertyType);
        }

        private String ShortenClassName(Type classType)
        {
            String[] splitClassName = classType.ToString().Split('.');
            if (splitClassName.Length > 0)
            {
                return splitClassName[splitClassName.Length - 1];
            }
            else
            {
                return classType.ToString();
            }
        }

        // Like this for debugging - will be changed later
        public override string ToString()
        {
            return $"[{m_shortClassName}] {m_shortPropertyClassName} {m_propertyName}";
        }

        Type m_classType;
        String m_propertyName;
        Type m_propertyType;

        String m_shortClassName;
        String m_shortPropertyClassName;
    }
}
