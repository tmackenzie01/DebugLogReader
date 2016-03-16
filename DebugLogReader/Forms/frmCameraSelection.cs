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
    public partial class frmCameraSelection : Form
    {
        public frmCameraSelection(String parentLogDirectory, String selectedCamerasCSV)
        {
            InitializeComponent();
            m_parentLogDirectory = parentLogDirectory;
            m_selectedCameras = CameraCSVToList(selectedCamerasCSV);
        }

        private async void frmCameraSelection_Load(object sender, EventArgs e)
        {
            await LoadCameraControls();
        }

        private async Task LoadCameraControls()
        {
            List<int> cameras = await frmDebugLogReader.GetCameraNumbersAsync(m_parentLogDirectory);

            CheckBox chkCam = null;
            int camCount = 0;
            foreach (int camera in cameras)
            {
                chkCam = new CheckBox();
                chkCam.Text = $"Cam {camera}";
                chkCam.CheckedChanged += chkCam_CheckedChanged;
                chkCam.Tag = camera;

                chkCam.Checked = m_selectedCameras.Contains(camera);

                PositionCheckBox(chkCam, camCount);
                grpCameras.Controls.Add(chkCam);
                camCount++;
            }
        }

        private void PositionCheckBox(CheckBox chk, int camCount)
        {
            int leftMargin = 10;
            int topMargin = 20;
            // Included the margin at first, but then removed it, still feel it should be there
            int widthWithMargin = chk.Width;
            int heightWithMargin = chk.Height;

            // How may can we fit in one row
            int rowMax = grpCameras.Width / widthWithMargin;
            int rowAdjust = camCount % rowMax;
            int colAdjust = camCount / rowMax;

            chk.Left = leftMargin + (rowAdjust * widthWithMargin);
            chk.Top = topMargin + (colAdjust * heightWithMargin);
        }

        private void chkCam_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            int camera = (int)chk.Tag;
            if (chk.Checked)
            {
                if (!m_selectedCameras.Contains(camera))
                {
                    m_selectedCameras.Add(camera);
                }
            }
            else
            {
                if (m_selectedCameras.Contains(camera))
                {
                    m_selectedCameras.Remove(camera);
                }
            }
        }

        public String SelectedCameraCSV
        {
            get
            {
                return CameraListToCSV(m_selectedCameras);
            }
        }

        public static List<int> CameraCSVToList(String selectedCamerasCSV)
        {
            List<int> selectedCameras = new List<int>();

            if (!String.IsNullOrEmpty(selectedCamerasCSV))
            {
                String[] cameras = selectedCamerasCSV.Split(',');
                int cameraNumber;
                foreach (String camera in cameras)
                {
                    if (Int32.TryParse(camera, out cameraNumber))
                    {
                        selectedCameras.Add(cameraNumber);
                    }
                }
            }

            return selectedCameras;
        }

        public static String CameraListToCSV(List<int> selectedCameras)
        {
            StringBuilder csv = new StringBuilder();
            selectedCameras.Sort();

            if (selectedCameras.Count > 0)
            {
                csv.Append(selectedCameras[0]);

                for (int i = 1; i < selectedCameras.Count; i++)
                {
                    csv.Append($",{selectedCameras[i]}");
                }
            }

            return csv.ToString();
        }


        List<int> m_selectedCameras;
        String m_parentLogDirectory;
    }
}
