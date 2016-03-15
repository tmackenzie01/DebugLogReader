using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;

namespace DebugLogReader
{
    public partial class ucFilter : UserControl
    {
        public ucFilter()
        {
            InitializeComponent();

            // List the properties of the DebugLogRow class using reflection
            List<RowProperty> baseClassProperties = new List<RowProperty>();
            List<RowProperty> subClassProperties = new List<RowProperty>();
            DebugLogRow row = new DebugLogRow();
            m_rowTypeObject = row.GetType();

            // Get all the subclasses of DebugLogRow
            Type baseType = typeof(DebugLogRow);
            Assembly thisAssembly = typeof(DebugLogRow).Assembly;
            var types = thisAssembly.GetTypes().Where(t => t.BaseType == baseType);

            // Get the properties from each subclass (no inherited properties)
            foreach (Type subClassType in types)
            {
                m_properties = subClassType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in m_properties)
                {
                    subClassProperties.Add(new RowProperty(subClassType, prop.Name, prop.PropertyType));
                    Debug.WriteLine($"{subClassType.Name} {prop.Name} {prop.PropertyType}");
                }
            }

            // Get the base class properties
            m_properties = m_rowTypeObject.GetProperties();
            foreach (PropertyInfo prop in m_properties)
            {
                subClassProperties.Add(new RowProperty(m_rowTypeObject, prop.Name, prop.PropertyType));
                Debug.WriteLine($"{prop.Name} {prop.PropertyType}");
            }

            foreach (RowProperty rowProp in subClassProperties)
            {
                cbxProperty.Items.Add(rowProp);
            }

            foreach (RowProperty rowProp in baseClassProperties)
            {
                cbxProperty.Items.Add(rowProp);
            }

            // We will use these properties to create automatic filters
            // probably need to create a user control that builds the filters, then once the filter has been created it added to a list of current filters
            // the current filters should be editable and you are able to delete each one
        }

        private void cbxProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedProperty = cbxProperty.SelectedItem.ToString();

            PropertyInfo selectedPropertyInfo = m_rowTypeObject.GetProperty(selectedProperty);

            if (selectedProperty != null)
            {
                Type propertyType = selectedPropertyInfo.PropertyType;
                cbxOperator.Items.Clear();

                if ((propertyType.Equals(typeof(int))) || (propertyType.Equals(typeof(DateTime))) || (propertyType.Equals(typeof(TimeSpan))))
                {
                    cbxOperator.Items.Add(eFilterComparision.EqualTo);
                    cbxOperator.Items.Add(eFilterComparision.LessThan);
                    cbxOperator.Items.Add(eFilterComparision.GreaterThan);
                }
                else if (propertyType.Equals(typeof(bool)))
                {
                    cbxOperator.Items.Add(eFilterComparision.EqualTo);
                }

                txtOperand.Tag = propertyType;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CaptureFilter();
        }

        private Object CaptureOperand()
        {
            Type operandType = (Type)txtOperand.Tag;
            Object operand = Activator.CreateInstance(operandType);

            if (operand is DateTime)
            {
                // Try date and time, then just time
                bool timeParsed = false;
                try
                {
                    DateTime startTime = DateTime.ParseExact(txtOperand.Text, @"dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    operand = startTime;
                    timeParsed = true;
                }
                catch
                {
                    timeParsed = false;
                }

                if (!timeParsed)
                {
                    DateTime startTime = DateTime.ParseExact(txtOperand.Text, @"HH:mm:ss", CultureInfo.InvariantCulture);
                    operand = startTime;
                }
            }
            else if (operand is TimeSpan)
            {
                // Only support seconds for now
                int valueInt = 0;
                if (Int32.TryParse(txtOperand.Text, out valueInt))
                {
                    if (valueInt > 0)
                    {
                        operand = new TimeSpan(0, 0, valueInt);
                    }
                }
            }
            else if (operand is int)
            {
                int valueInt = 0;
                if (Int32.TryParse(txtOperand.Text, out valueInt))
                {
                    operand = valueInt;
                }
            }
            else
            {
                throw new Exception($"Operand type {operand.GetType().ToString()} not supported");
            }

            return operand;
        }

        private void CaptureFilter()
        {
            Object operand = CaptureOperand();

            if (operand != null)
            {
                String selectedProperty = cbxProperty.SelectedItem.ToString();
                eFilterComparision selectedComparision = (eFilterComparision)cbxOperator.SelectedItem;
                DebugLogFilter testFilter = new DebugLogFilter(selectedProperty, selectedComparision, operand);
            }
        }

        Type m_rowTypeObject;
        PropertyInfo[] m_properties;
    }
}
