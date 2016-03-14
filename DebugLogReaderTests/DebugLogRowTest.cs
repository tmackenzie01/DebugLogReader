﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugLogReader;
using System.Text.RegularExpressions;

namespace DebugLogReaderTests
{
    [TestClass]
    public class DebugLogRowTest
    {
        [TestMethod]
        public void CameraNumberTests()
        {
            // Get the compiled Regex
            Regex pushedRegex = frmDebugLogReader.m_pushedRegex;
            Regex poppedRegex = frmDebugLogReader.m_poppedRegex;

            DebugLogPushRow standardPushRow1 = new DebugLogPushRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(1, standardPushRow1.CameraNumber);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 0, 11).AddMilliseconds(72), standardPushRow1.Timestamp);

            DebugLogPushRow standardPushRow2 = new DebugLogPushRow(2, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(2, standardPushRow2.CameraNumber);

            DebugLogPushRow standardPushRow546 = new DebugLogPushRow(546, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(546, standardPushRow546.CameraNumber);

            DebugLogPopRow standardPopRow1 = new DebugLogPopRow(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0", poppedRegex);
            Assert.AreEqual(1, standardPopRow1.CameraNumber);

            DebugLogPopRow standardPopRow44 = new DebugLogPopRow(44, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0", poppedRegex);
            Assert.AreEqual(44, standardPopRow44.CameraNumber);
        }

        [TestMethod]
        public void QueueCountTests()
        {
            // Get the compiled Regex
            Regex pushedRegex = frmDebugLogReader.m_pushedRegex;
            Regex poppedRegex = frmDebugLogReader.m_poppedRegex;

            DebugLogPopRow standardPushRow1 = new DebugLogPopRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(0, standardPushRow1.QueueCount);

            DebugLogPopRow standardPopRow1 = new DebugLogPopRow(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0", poppedRegex);
            Assert.AreEqual(1, standardPopRow1.QueueCount);

            DebugLogPopRow pushRow = new DebugLogPopRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:747 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(747, pushRow.QueueCount);

            DebugLogPopRow pushRowNegativeFrame = new DebugLogPopRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (5.010 seconds) Q:0 F:-1, 0, 4", pushedRegex);
            Assert.AreEqual(0, pushRowNegativeFrame.QueueCount);

            DebugLogPopRow pushRowNullFrame = new DebugLogPopRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (0.000 seconds) Q:0 F:Null", pushedRegex);
            Assert.AreEqual(0, pushRowNullFrame.QueueCount);

            DebugLogPopRow pushRowZeroes = new DebugLogPopRow(1, "Pushed - 02/03/2016 17:09:37.973 Q:0 F: 0, 0, 0", pushedRegex);
            Assert.AreEqual(0, pushRowZeroes.QueueCount);
        }

        [TestMethod]
        public void PushTests()
        {
            // Get the compiled Regex
            Regex pushedRegex = frmDebugLogReader.m_pushedRegex;

            DebugLogPushRow pushRow = new DebugLogPushRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 0, 11).AddMilliseconds(72), pushRow.Timestamp);

            DebugLogPushRow pushRowLargerTime = new DebugLogPushRow(33, "Pushed - 02/03/2016 17:36:15.325 ---  (76.810 seconds) Q:0 F:1242, 5806, 0", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 36, 15).AddMilliseconds(325), pushRowLargerTime.Timestamp);

            DebugLogPushRow pushRowNegativeFrame = new DebugLogPushRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (5.010 seconds) Q:0 F:-1, 0, 4", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 8, 47).AddMilliseconds(343), pushRowNegativeFrame.Timestamp);

            DebugLogPushRow pushRowNullFrame = new DebugLogPushRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (0.000 seconds) Q:0 F:Null", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 8, 47).AddMilliseconds(343), pushRowNullFrame.Timestamp);

            DebugLogPushRow pushRowZeroes = new DebugLogPushRow(1, "Pushed - 02/03/2016 17:09:37.973 Q:0 F: 0, 0, 0", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 9, 37).AddMilliseconds(973), pushRowZeroes.Timestamp);
        }

        [TestMethod]
        public void PoppedTests()
        {
            // Get the compiled Regex
            Regex poppedRegex = frmDebugLogReader.m_poppedRegex;
            Regex wroteDataRegex = frmDebugLogReader.m_wroteDataRegex;

            DebugLogPopRow popRow = new DebugLogPopRow(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0", poppedRegex);
            Assert.AreEqual(1, popRow.CameraNumber);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 1, 48).AddMilliseconds(412), popRow.Timestamp);

            // If we don't pass in any time "Wrote data" default to max time
            DebugLogPopRow popRowWroteDataNoTime = new DebugLogPopRow(1, "Wrote data", poppedRegex, wroteDataRegex);
            Assert.AreEqual(DateTime.MaxValue, popRowWroteDataNoTime.Timestamp); ;
            Assert.AreEqual(true, popRowWroteDataNoTime.WroteData);

            // If we pass in a previous timestamp, "Wrote data" will use that time
            DebugLogPopRow popRowWroteDataTime = new DebugLogPopRow(1, "Wrote data", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(DateTime.MinValue, popRowWroteDataTime.Timestamp);
            Assert.AreEqual(true, popRowWroteDataTime.WroteData);

            DebugLogPopRow popRowZeroes = new DebugLogPopRow(1, "Popped - 02/03/2016 17:09:37.973 Q:1 F:0, 0, 0", poppedRegex, wroteDataRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 9, 37).AddMilliseconds(973), popRowZeroes.Timestamp);

            // If we pass in a previous timestamp, "Wrote data" will use that time
            DebugLogPopRow popRowWroteDataTimeColdstoreInfo = new DebugLogPopRow(1, "Wrote data C:2 P:50336", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(DateTime.MinValue, popRowWroteDataTimeColdstoreInfo.Timestamp);
            Assert.AreEqual(2, popRowWroteDataTimeColdstoreInfo.ColdstoreId);
            Assert.AreEqual(50336, popRowWroteDataTimeColdstoreInfo.ColdstorePort);

            DebugLogPopRow popRowWroteDataTimeColdstoreInfo2 = new DebugLogPopRow(1, "11/03/2016 13:12:25.893 Wrote data C:1 P:55055", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(1, popRowWroteDataTimeColdstoreInfo2.ColdstoreId);
            Assert.AreEqual(55055, popRowWroteDataTimeColdstoreInfo2.ColdstorePort);

            DebugLogPopRow popRowWithTimingsABCD1 = new DebugLogPopRow(1, "Popped - 08/03/2016 11:58:16.698 --- (0.030 seconds) " +
                "Q:1 F:83, 6711, 0 T:A 11:58:16.668 B 11:58:16.668 C 11:58:16.698 D 11:58:16.698 ", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(new DateTime(2016, 3, 8, 11, 58, 16).AddMilliseconds(698), popRowWithTimingsABCD1.Timestamp);
            Assert.AreEqual(0, popRowWithTimingsABCD1.TimeElapsedB.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsABCD1.TimeElapsedB.Milliseconds);
            Assert.AreEqual(30, popRowWithTimingsABCD1.TimeElapsedC.Milliseconds);
            Assert.AreEqual(0, popRowWithTimingsABCD1.TimeElapsedD.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsABCD1.TimeElapsedD.Milliseconds);

            DebugLogPopRow popRowWithTimingsABCD2 = new DebugLogPopRow(1, "Popped - 10/03/2016 12:40:04.137 --- (0.200 seconds) " +
                "Q:1 F:1122, 6293, 0 T:A 12:40:03.937 B 12:40:03.937 C 12:40:04.137 D 12:40:04.137 ", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(new DateTime(2016, 3, 10, 12, 40, 4).AddMilliseconds(137), popRowWithTimingsABCD2.Timestamp);
            Assert.AreEqual(0, popRowWithTimingsABCD2.TimeElapsedB.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsABCD2.TimeElapsedB.Milliseconds);
            Assert.AreEqual(200, popRowWithTimingsABCD2.TimeElapsedC.Milliseconds);
            Assert.AreEqual(0, popRowWithTimingsABCD2.TimeElapsedD.Milliseconds);

            DebugLogPopRow popRowWithTimingsACD = new DebugLogPopRow(1, "Popped - 08/03/2016 11:58:18.518 --- (0.010 seconds) Q:1 F:128, 6345, 0 T:A 11:58:18.518 C 11:58:18.518 D 11:58:20.518 ", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(new DateTime(2016, 3, 8, 11, 58, 18).AddMilliseconds(518), popRowWithTimingsACD.Timestamp);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedB.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedB.Milliseconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedC.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedC.Milliseconds);
            Assert.AreEqual(2, popRowWithTimingsACD.TimeElapsedD.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedD.Milliseconds);

            DebugLogPopRow popRowNullFrame = new DebugLogPopRow(1, "Popped - 08/03/2016 13:50:44.814 --- (0.030 seconds) Q:1 F:Null T:A 13:50:44.814 C 13:50:44.814 D 13:50:44.814 ", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(new DateTime(2016, 3, 8, 13, 50, 44).AddMilliseconds(814), popRowNullFrame.Timestamp);

        }

        [TestMethod]
        public void CSTests()
        {
            // Get the compiled Regex
            Regex csRegex = frmDebugLogReader.m_csRegex;

            DebugLogCSRow csBasicRow1 = new DebugLogCSRow(1, "12:23:01.374 2 157906 bytes:con-STC 0.005--ws-Above 0.565", csRegex, null, DateTime.MinValue);
            Assert.AreEqual("12:23:01.374", csBasicRow1.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogCSRow csBasicRow2 = new DebugLogCSRow(1, "12:32:55.017 40 128702 bytes:con-STC 0.003--ws-Above 0.644 ", csRegex, null, DateTime.MinValue);
            Assert.AreEqual("12:32:55.017", csBasicRow2.Timestamp.ToString("HH:mm:ss.fff"));
        }

        [TestMethod]
        public void FrameTests()
        {
            // Get the compiled Regex
            Regex frameRegex = frmDebugLogReader.m_frameRegex;

            DebugLogFrameRow frameRow = new DebugLogFrameRow(1, "Record 15:08:44.779 (RV:0.000 ) TOT:0.000 ", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("15:08:44.779", frameRow.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogFrameRow frameRow2 = new DebugLogFrameRow(2, "Record 14:56:07.972 (C:0.000 O:0.009 RV:0.000 ) TOT:0.009 ", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("14:56:07.972", frameRow2.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogFrameRow frameRow3 = new DebugLogFrameRow(2, "Record 16:27:36.195 (C:0.021 ) TOT:0.021 ", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("16:27:36.195", frameRow3.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogFrameRow frameRow4 = new DebugLogFrameRow(2, "Record 11:58:45.219 " +
                "(C:0.000 O:0.008 RV:0.000 ) TOT:0.008 ", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("11:58:45.219", frameRow4.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogFrameRow frameRow5 = new DebugLogFrameRow(2, "Record 11:58:45.219 " +
                "(C:0.000 O:0.008 MPEG4-AA:0.000 BB:0.000 RV:0.000 ) TOT:0.008 ", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("11:58:45.219", frameRow5.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogFrameRow frameRow6 = new DebugLogFrameRow(2, "Record 11:58:45.219 " +
                "(C:0.000 O:0.008 MPEG4-AA:0.000 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.000 ) TOT:0.008 ", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("11:58:45.219", frameRow6.Timestamp.ToString("HH:mm:ss.fff"));

            // HH can be HH:13:04:15.469 (full time) or HH:0.206 (duration)
            DebugLogFrameRow frameRowHH1 = new DebugLogFrameRow(2, "Record 13:04:15.059 " +
                "(MPEG4-AA:0.206 HH:13:04:15.469 RVE:0.425 ) TOT:0.425 ", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("13:04:15.059", frameRowHH1.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(true, frameRowHH1.RVException);

            DebugLogFrameRow frameRowHH2 = new DebugLogFrameRow(2, "Record 13:04:15.059 " +
                "(MPEG4-AA:0.206 HH:0.206 RVE:0.425 ) TOT:0.425 ", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("13:04:15.059", frameRowHH2.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(true, frameRowHH2.RVException);

            DebugLogFrameRow frameRow8 = new DebugLogFrameRow(2, "Record 13:05:44.719 " +
                "(C:0.000 O:0.007 MPEG4-AA:0.000 BB:0.000 CR:0.000 RV:0.000 ) TOT:0.007", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("13:05:44.719", frameRow8.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(false, frameRow8.RVException);

            DebugLogFrameRow frameRowWithRTSPErrorCount = new DebugLogFrameRow(2, "Record 09:52:05.767 " +
                "RT:0 (MPEG4-AA:0.000 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.000 ) TOT:0.000 ", frameRegex, null, DateTime.MinValue);
            Assert.AreEqual("09:52:05.767", frameRowWithRTSPErrorCount.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(false, frameRowWithRTSPErrorCount.RVException);

        }

        [TestMethod]
        public void AviTests()
        {
            // Get the compiled Regex
            Regex aviRegex = frmDebugLogReader.m_aviRegex;

            DebugLogAviRow aviRow1 = new DebugLogAviRow(1, "Create 12:21:23.578 CR1:0.000 CR2:0.000 CR3:0.008 CR4:0.000 CR5:0.000 CR7:0.000 ", aviRegex, null, DateTime.MinValue);
            Assert.AreEqual(false, aviRow1.CRException);
            Assert.AreEqual(false, aviRow1.CRError);
            Assert.AreEqual("12:21:23.578", aviRow1.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogAviRow aviRow2 = new DebugLogAviRow(1, "Create 13:48:24.660 CR1:0.000 CR2:0.000 CR3:20.152 CRE:0.000 " +
                "CRX:An I/O error occured on Coldstore. Object reference not set to an instance of an object.", aviRegex, null, DateTime.MinValue);
            Assert.AreEqual(true, aviRow2.CRException);
            Assert.AreEqual(true, aviRow2.CRError);
            Assert.AreEqual("13:48:24.660", aviRow2.Timestamp.ToString("HH:mm:ss.fff"));

        }

    }
}
