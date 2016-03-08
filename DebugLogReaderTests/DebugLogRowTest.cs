using System;
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

            DebugLogRow standardPushRow1 = new DebugLogRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(1, standardPushRow1.CameraNumber);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 0, 11).AddMilliseconds(72), standardPushRow1.Timestamp);

            DebugLogRow standardPushRow2 = new DebugLogRow(2, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(2, standardPushRow2.CameraNumber);

            DebugLogRow standardPushRow546 = new DebugLogRow(546, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(546, standardPushRow546.CameraNumber);

            DebugLogRow standardPopRow1 = new DebugLogRow(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0", poppedRegex);
            Assert.AreEqual(1, standardPopRow1.CameraNumber);

            DebugLogRow standardPopRow44 = new DebugLogRow(44, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0", poppedRegex);
            Assert.AreEqual(44, standardPopRow44.CameraNumber);
        }

        [TestMethod]
        public void QueueCountTests()
        {
            // Get the compiled Regex
            Regex pushedRegex = frmDebugLogReader.m_pushedRegex;
            Regex poppedRegex = frmDebugLogReader.m_poppedRegex;

            DebugLogRow standardPushRow1 = new DebugLogRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(0, standardPushRow1.QueueCount);

            DebugLogRow standardPopRow1 = new DebugLogRow(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0", poppedRegex);
            Assert.AreEqual(1, standardPopRow1.QueueCount);

            DebugLogRow pushRow = new DebugLogRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:747 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(747, pushRow.QueueCount);

            DebugLogRow pushRowNegativeFrame = new DebugLogRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (5.010 seconds) Q:0 F:-1, 0, 4", pushedRegex);
            Assert.AreEqual(0, pushRowNegativeFrame.QueueCount);

            DebugLogRow pushRowNullFrame = new DebugLogRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (0.000 seconds) Q:0 F:Null", pushedRegex);
            Assert.AreEqual(0, pushRowNullFrame.QueueCount);

            DebugLogRow pushRowZeroes = new DebugLogRow(1, "Pushed - 02/03/2016 17:09:37.973 Q:0 F: 0, 0, 0", pushedRegex);
            Assert.AreEqual(0, pushRowZeroes.QueueCount);
        }

        [TestMethod]
        public void PushTests()
        {
            // Get the compiled Regex
            Regex pushedRegex = frmDebugLogReader.m_pushedRegex;

            DebugLogRow pushRow = new DebugLogRow(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 0, 11).AddMilliseconds(72), pushRow.Timestamp);

            DebugLogRow pushRowLargerTime = new DebugLogRow(33, "Pushed - 02/03/2016 17:36:15.325 ---  (76.810 seconds) Q:0 F:1242, 5806, 0", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 36, 15).AddMilliseconds(325), pushRowLargerTime.Timestamp);

            DebugLogRow pushRowNegativeFrame = new DebugLogRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (5.010 seconds) Q:0 F:-1, 0, 4", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 8, 47).AddMilliseconds(343), pushRowNegativeFrame.Timestamp);

            DebugLogRow pushRowNullFrame = new DebugLogRow(1, "Pushed - 02/03/2016 17:08:47.343 ---  (0.000 seconds) Q:0 F:Null", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 8, 47).AddMilliseconds(343), pushRowNullFrame.Timestamp);

            DebugLogRow pushRowZeroes = new DebugLogRow(1, "Pushed - 02/03/2016 17:09:37.973 Q:0 F: 0, 0, 0", pushedRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 9, 37).AddMilliseconds(973), pushRowZeroes.Timestamp);
        }

        [TestMethod]
        public void PoppedTests()
        {
            // Get the compiled Regex
            Regex poppedRegex = frmDebugLogReader.m_poppedRegex;
            Regex wroteDataRegex = frmDebugLogReader.m_wroteDataRegex;

            DebugLogRow popRow = new DebugLogRow(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0", poppedRegex);
            Assert.AreEqual(1, popRow.CameraNumber);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 1, 48).AddMilliseconds(412), popRow.Timestamp);

            // If we don't pass in any time "Wrote data" default to max time
            DebugLogRow popRowWroteDataNoTime = new DebugLogRow(1, "Wrote data", poppedRegex, wroteDataRegex);
            Assert.AreEqual(DateTime.MaxValue, popRowWroteDataNoTime.Timestamp); ;
            Assert.AreEqual(true, popRowWroteDataNoTime.WroteData);

            // If we pass in a previous timestamp, "Wrote data" will use that time
            DebugLogRow popRowWroteDataTime = new DebugLogRow(1, "Wrote data", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(DateTime.MinValue, popRowWroteDataTime.Timestamp);
            Assert.AreEqual(true, popRowWroteDataTime.WroteData);

            DebugLogRow popRowZeroes = new DebugLogRow(1, "Popped - 02/03/2016 17:09:37.973 Q:1 F:0, 0, 0", poppedRegex, wroteDataRegex);
            Assert.AreEqual(new DateTime(2016, 3, 2, 17, 9, 37).AddMilliseconds(973), popRowZeroes.Timestamp);

            // If we pass in a previous timestamp, "Wrote data" will use that time
            DebugLogRow popRowWroteDataTimeColdstoreInfo = new DebugLogRow(1, "Wrote data C:2 P:50336", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(DateTime.MinValue, popRowWroteDataTimeColdstoreInfo.Timestamp);
            Assert.AreEqual(2, popRowWroteDataTimeColdstoreInfo.ColdstoreId);
            Assert.AreEqual(50336, popRowWroteDataTimeColdstoreInfo.ColdstorePort);

            DebugLogRow popRowWithTimingsABCD = new DebugLogRow(1, "Popped - 08/03/2016 11:58:16.698 --- (0.030 seconds) Q:1 F:83, 6711, 0 T:A 11:58:16.668 B 11:58:16.668 C 11:58:16.698 D 11:58:16.698 ", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(new DateTime(2016, 3, 8, 11, 58, 16).AddMilliseconds(698), popRowWithTimingsABCD.Timestamp);
            Assert.AreEqual(0, popRowWithTimingsABCD.TimeElapsedB.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsABCD.TimeElapsedB.Milliseconds);
            Assert.AreEqual(0.03, popRowWithTimingsABCD.TimeElapsedC.TotalSeconds);
            Assert.AreEqual(30, popRowWithTimingsABCD.TimeElapsedC.Milliseconds);
            Assert.AreEqual(0, popRowWithTimingsABCD.TimeElapsedD.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsABCD.TimeElapsedD.Milliseconds);

            DebugLogRow popRowWithTimingsACD = new DebugLogRow(1, "Popped - 08/03/2016 11:58:18.518 --- (0.010 seconds) Q:1 F:128, 6345, 0 T:A 11:58:18.518 C 11:58:18.518 D 11:58:20.518 ", poppedRegex, wroteDataRegex, DateTime.MinValue);
            Assert.AreEqual(new DateTime(2016, 3, 8, 11, 58, 18).AddMilliseconds(518), popRowWithTimingsACD.Timestamp);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedB.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedB.Milliseconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedC.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedC.Milliseconds);
            Assert.AreEqual(2, popRowWithTimingsACD.TimeElapsedD.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedD.Milliseconds);
        }


        [TestMethod]
        public void CSTests()
        {
            // Get the compiled Regex
            Regex csRegex = frmDebugLogReader.m_csRegex;

            //csRegex = new Regex("(?<timestamp>[0-9]+.[0-9]+.[0-9]+.[0-9]+).*[0-9]+.[0-9]+.(.)*[0-9]+.[0-9]+$",
            //RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

            DebugLogRow csBasicRow = new DebugLogCSRow(1, "12:23:01.374 2 157906 bytes:con-STC 0.005--ws-Above 0.565", csRegex, null, DateTime.MinValue);
            Assert.AreEqual(false, csBasicRow.WroteData);
            Assert.AreEqual("12:23:01.374", csBasicRow.Timestamp.ToString("HH:mm:ss.fff"));
        }
    }
}
