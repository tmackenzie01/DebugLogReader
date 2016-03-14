using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebugLogReader
{
    public partial class frmFilters : Form
    {
        public frmFilters()
        {
            InitializeComponent();

            ucFilter ucStartFilter = new ucFilter();
            this.Controls.Add(ucStartFilter);
        }
    }
}
