using System;

namespace DebugLogReader
{
    public class CameraDirectory
    {
        public CameraDirectory(int cameraNumber, String cameraName)
        {
            CameraName = cameraName;
            CameraNumber = cameraNumber;
        }

        public int CameraNumber { get; set; }
        public String CameraName { get; set; }
    }
}
