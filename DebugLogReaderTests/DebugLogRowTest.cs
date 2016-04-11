using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugLogReader;

namespace DebugLogReaderTests
{
    [TestClass]
    public class DebugLogRowTest
    {
        [TestMethod]
        public void CameraNumberTests()
        {
            DebugLogRowPush standardPushRow1 = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0");
            Assert.AreEqual(1, standardPushRow1.CameraNumber);
            Assert.AreEqual("17:00:11.072", standardPushRow1.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowPush standardPushRow2 = new DebugLogRowPush(2, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0");
            Assert.AreEqual(2, standardPushRow2.CameraNumber);

            DebugLogRowPush standardPushRow546 = new DebugLogRowPush(546, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0");
            Assert.AreEqual(546, standardPushRow546.CameraNumber);

            DebugLogRowPop standardPopRow1 = new DebugLogRowPop(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0");
            Assert.AreEqual(1, standardPopRow1.CameraNumber);

            DebugLogRowPop standardPopRow44 = new DebugLogRowPop(44, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0");
            Assert.AreEqual(44, standardPopRow44.CameraNumber);
        }

        [TestMethod]
        public void QueueCountTests()
        {
            DebugLogRowPush standardPushRow1 = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0");
            Assert.AreEqual(0, standardPushRow1.QueueCount);

            DebugLogRowPop standardPopRow1 = new DebugLogRowPop(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0");
            Assert.AreEqual(1, standardPopRow1.QueueCount);

            DebugLogRowPush pushRow = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:747 F:470, 7730, 0");
            Assert.AreEqual(747, pushRow.QueueCount);

            DebugLogRowPush pushRowNegativeFrame = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:08:47.343 ---  (5.010 seconds) Q:0 F:-1, 0, 4");
            Assert.AreEqual(0, pushRowNegativeFrame.QueueCount);

            DebugLogRowPush pushRowNullFrame = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:08:47.343 ---  (0.000 seconds) Q:0 F:Null");
            Assert.AreEqual(0, pushRowNullFrame.QueueCount);

            DebugLogRowPush pushRowZeroes = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:09:37.973 Q:0 F: 0, 0, 0");
            Assert.AreEqual(0, pushRowZeroes.QueueCount);
        }

        [TestMethod]
        public void PushTests()
        {
            DebugLogRowPush pushRow = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:00:11.072 ---  (0.040 seconds) Q:0 F:470, 7730, 0");
            Assert.AreEqual("17:00:11.072", pushRow.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowPush pushRowLargerTime = new DebugLogRowPush(33, "Pushed - 02/03/2016 17:36:15.325 ---  (76.810 seconds) Q:0 F:1242, 5806, 0");
            Assert.AreEqual("17:36:15.325", pushRowLargerTime.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowPush pushRowNegativeFrame = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:08:47.343 ---  (5.010 seconds) Q:0 F:-1, 0, 4");
            Assert.AreEqual("17:08:47.343", pushRowNegativeFrame.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowPush pushRowNullFrame = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:08:47.343 ---  (0.000 seconds) Q:0 F:Null");
            Assert.AreEqual("17:08:47.343", pushRowNullFrame.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowPush pushRowZeroes = new DebugLogRowPush(1, "Pushed - 02/03/2016 17:09:37.973 Q:0 F: 0, 0, 0");
            Assert.AreEqual("17:09:37.973", pushRowZeroes.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowPush pushRowSpecialDiag = new DebugLogRowPush(1, "Pushed - 01/04/2016 14:32:54.270 Q:0 F:18, 82762, 24 **asd**");
            Assert.AreEqual("14:32:54.270", pushRowSpecialDiag.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowPush pushRowSpecialDiag2 = new DebugLogRowPush(1, "Pushed - 01/04/2016 14:32:54.270 Q:0 F:18, 82762, 24 **SendReceiverReportGF:8/1**");
            Assert.AreEqual("14:32:54.270", pushRowSpecialDiag2.Timestamp.ToString("HH:mm:ss.fff"));
        }

        [TestMethod]
        public void PoppedTests()
        {
            DebugLogRowPop popRow = new DebugLogRowPop(1, "Popped - 02/03/2016 17:01:48.412 --- (0.000 seconds) Q:1 F:745, 5234, 0");
            Assert.AreEqual(1, popRow.CameraNumber);
            Assert.AreEqual("17:01:48.412", popRow.Timestamp.ToString("HH:mm:ss.fff"));

            // If we don't pass in any time "Wrote data" default to max time
            DebugLogRowPop popRowWroteDataNoTime = new DebugLogRowPop(1, "Wrote data");
            Assert.AreEqual(DateTime.MaxValue, popRowWroteDataNoTime.Timestamp);
            Assert.AreEqual(true, popRowWroteDataNoTime.WroteData);

            // If we pass in a previous timestamp, "Wrote data" will use that time
            DebugLogRowPop popRowWroteDataTime = new DebugLogRowPop(1, "Wrote data", DateTime.MinValue);
            Assert.AreEqual(DateTime.MinValue, popRowWroteDataTime.Timestamp);
            Assert.AreEqual(true, popRowWroteDataTime.WroteData);

            DebugLogRowPop popRowZeroes = new DebugLogRowPop(1, "Popped - 02/03/2016 17:09:37.973 Q:1 F:0, 0, 0");
            Assert.AreEqual("17:09:37.973", popRowZeroes.Timestamp.ToString("HH:mm:ss.fff"));

            // If we pass in a previous timestamp, "Wrote data" will use that time
            DebugLogRowPop popRowWroteDataTimeColdstoreInfo = new DebugLogRowPop(1, "Wrote data C:2 P:50336");
            Assert.AreEqual(DateTime.MaxValue, popRowWroteDataTimeColdstoreInfo.Timestamp);
            Assert.AreEqual(2, popRowWroteDataTimeColdstoreInfo.ColdstoreId);
            Assert.AreEqual(50336, popRowWroteDataTimeColdstoreInfo.ColdstorePort);

            DebugLogRowPop popRowWroteDataTimeColdstoreInfo2 = new DebugLogRowPop(1, "11/03/2016 13:12:25.893 Wrote data C:1 P:55055");
            Assert.AreEqual(1, popRowWroteDataTimeColdstoreInfo2.ColdstoreId);
            Assert.AreEqual(55055, popRowWroteDataTimeColdstoreInfo2.ColdstorePort);

            DebugLogRowPop popRowWithTimingsABCD1 = new DebugLogRowPop(1, "Popped - 08/03/2016 11:58:16.698 --- (0.030 seconds) " +
                "Q:1 F:83, 6711, 0 T:A 11:58:16.668 B 11:58:16.668 C 11:58:16.698 D 11:58:16.698 ");
            Assert.AreEqual("11:58:16.698", popRowWithTimingsABCD1.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(0, popRowWithTimingsABCD1.TimeElapsedB.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsABCD1.TimeElapsedB.Milliseconds);
            Assert.AreEqual(30, popRowWithTimingsABCD1.TimeElapsedC.Milliseconds);
            Assert.AreEqual(0, popRowWithTimingsABCD1.TimeElapsedD.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsABCD1.TimeElapsedD.Milliseconds);

            DebugLogRowPop popRowWithTimingsABCD2 = new DebugLogRowPop(1, "Popped - 10/03/2016 12:40:04.137 --- (0.200 seconds) " +
                "Q:1 F:1122, 6293, 0 T:A 12:40:03.937 B 12:40:03.937 C 12:40:04.137 D 12:40:04.137 ");
            Assert.AreEqual("12:40:04.137", popRowWithTimingsABCD2.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(0, popRowWithTimingsABCD2.TimeElapsedB.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsABCD2.TimeElapsedB.Milliseconds);
            Assert.AreEqual(200, popRowWithTimingsABCD2.TimeElapsedC.Milliseconds);
            Assert.AreEqual(0, popRowWithTimingsABCD2.TimeElapsedD.Milliseconds);

            DebugLogRowPop popRowWithTimingsACD = new DebugLogRowPop(1, "Popped - 08/03/2016 11:58:18.518 --- (0.010 seconds) Q:1 F:128, 6345, 0 T:A 11:58:18.518 C 11:58:18.518 D 11:58:20.518 ");
            Assert.AreEqual("11:58:18.518", popRowWithTimingsACD.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedB.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedB.Milliseconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedC.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedC.Milliseconds);
            Assert.AreEqual(2, popRowWithTimingsACD.TimeElapsedD.TotalSeconds);
            Assert.AreEqual(0, popRowWithTimingsACD.TimeElapsedD.Milliseconds);

            DebugLogRowPop popRowNullFrame = new DebugLogRowPop(1, "Popped - 08/03/2016 13:50:44.814 --- (0.030 seconds) Q:1 F:Null T:A 13:50:44.814 C 13:50:44.814 D 13:50:44.814 ");
            Assert.AreEqual("13:50:44.814", popRowNullFrame.Timestamp.ToString("HH:mm:ss.fff"));

        }

        [TestMethod]
        public void CSTests()
        {
            DebugLogRowCS csBasicRow1 = new DebugLogRowCS(1, "12:23:01.374 2 157906 bytes:con-STC 0.005--ws-Above 0.565", DateTime.MinValue);
            Assert.AreEqual("12:23:01.374", csBasicRow1.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowCS csBasicRow2 = new DebugLogRowCS(1, "12:32:55.017 40 128702 bytes:con-STC 0.003--ws-Above 0.644 ", DateTime.MinValue);
            Assert.AreEqual("12:32:55.017", csBasicRow2.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowCS csBasicRow3 = new DebugLogRowCS(1, "13:36:04.434 Write max, WT:0.012 WW:0.000 WA:0.007 ", DateTime.MinValue);
            Assert.AreEqual("13:36:04.434", csBasicRow3.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowCS csWriteMax = new DebugLogRowCS(1, "Write max, WT:0.013 WW:0.000 WA:0.007 ", DateTime.MinValue);
        }

        [TestMethod]
        public void FrameTests()
        {
            DebugLogRowFrame frameRow = new DebugLogRowFrame(1, "Record 15:08:44.779 (RV:0.000 ) TOT:0.000 ", DateTime.MinValue);
            Assert.AreEqual("15:08:44.779", frameRow.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameRow2 = new DebugLogRowFrame(2, "Record 14:56:07.972 (C:0.000 O:0.009 RV:0.000 ) TOT:0.009 ", DateTime.MinValue);
            Assert.AreEqual("14:56:07.972", frameRow2.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameRow3 = new DebugLogRowFrame(2, "Record 16:27:36.195 (C:0.021 ) TOT:0.021 ", DateTime.MinValue);
            Assert.AreEqual("16:27:36.195", frameRow3.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameRow4 = new DebugLogRowFrame(2, "Record 11:58:45.219 " +
                "(C:0.000 O:0.008 RV:0.000 ) TOT:0.008 ", DateTime.MinValue);
            Assert.AreEqual("11:58:45.219", frameRow4.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameRow5 = new DebugLogRowFrame(2, "Record 11:58:45.219 " +
                "(C:0.000 O:0.008 MPEG4-AA:0.000 BB:0.000 RV:0.000 ) TOT:0.008 ", DateTime.MinValue);
            Assert.AreEqual("11:58:45.219", frameRow5.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameRow6 = new DebugLogRowFrame(2, "Record 11:58:45.219 " +
                "(C:0.000 O:0.008 MPEG4-AA:0.000 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.000 ) TOT:0.008 ", DateTime.MinValue);
            Assert.AreEqual("11:58:45.219", frameRow6.Timestamp.ToString("HH:mm:ss.fff"));

            // HH can be HH:13:04:15.469 (full time) or HH:0.206 (duration)
            DebugLogRowFrame frameRowHH1 = new DebugLogRowFrame(2, "Record 13:04:15.059 " +
                "(MPEG4-AA:0.206 HH:13:04:15.469 RVE:0.425 ) TOT:0.425 ", DateTime.MinValue);
            Assert.AreEqual("13:04:15.059", frameRowHH1.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(true, frameRowHH1.RVException);

            DebugLogRowFrame frameRowHH2 = new DebugLogRowFrame(2, "Record 13:04:15.059 " +
                "(MPEG4-AA:0.206 HH:0.206 RVE:0.425 ) TOT:0.425 ", DateTime.MinValue);
            Assert.AreEqual("13:04:15.059", frameRowHH2.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(true, frameRowHH2.RVException);

            DebugLogRowFrame frameRowMpeg4_1 = new DebugLogRowFrame(2, "Record 13:05:44.719 " +
                "(C:0.000 O:0.007 MPEG4-AA:0.000 BB:0.000 CR:0.000 RV:0.000 ) TOT:0.007", DateTime.MinValue);
            Assert.AreEqual("13:05:44.719", frameRowMpeg4_1.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(false, frameRowMpeg4_1.RVException);

            DebugLogRowFrame frameRowMpeg4_2 = new DebugLogRowFrame(2, "Record 09:27:39.400 " +
                "RT:0 (MPEG4-AA:0.010 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.010 ) TOT:0.010 ", DateTime.MinValue);
            Assert.AreEqual("09:27:39.400", frameRowMpeg4_2.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameRowWithRTSPErrorCount = new DebugLogRowFrame(2, "Record 09:52:05.767 " +
                "RT:0 (MPEG4-AA:0.000 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.000 ) TOT:0.000 ", DateTime.MinValue);
            Assert.AreEqual("09:52:05.767", frameRowWithRTSPErrorCount.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(0, frameRowWithRTSPErrorCount.RTSPErrorCount);

            DebugLogRowFrame frameRowWithRTSPErrorCount2 = new DebugLogRowFrame(2, "Record 09:52:05.767 " +
                "RT:4 (MPEG4-AA:0.000 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.000 ) TOT:0.000 ", DateTime.MinValue);
            Assert.AreEqual("09:52:05.767", frameRowWithRTSPErrorCount2.Timestamp.ToString("HH:mm:ss.fff"));
            Assert.AreEqual(4, frameRowWithRTSPErrorCount2.RTSPErrorCount);

            DebugLogRowFrame frameRowH264 = new DebugLogRowFrame(2, "Record 10:34:15.236 " +
                "RT:0 (C:0.002 O:0.103 H264-AA:0.000 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.002 ) TOT:0.107 ", DateTime.MinValue);
            Assert.AreEqual("10:34:15.236", frameRowH264.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameRowMpegWriteMax = new DebugLogRowFrame(2, "Record 09:27:39.400 " +
                "RT:0 (MPEG4-Write max, WT:0.010 WW:0.000 WA:0.005 AA:0.010 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.010 ) TOT:0.010 ", DateTime.MinValue);
            Assert.AreEqual("09:27:39.400", frameRowMpegWriteMax.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameWriteMax = new DebugLogRowFrame(1, "Write max, WT:0.013 WW:0.000 WA:0.007 ", DateTime.MinValue);

            DebugLogRowFrame frameWriteMax1 = new DebugLogRowFrame(1, "11:29:50.078 Close max, T:0.009 W: 0.000 A: 0.000 ", DateTime.MinValue);
            Assert.AreEqual("11:29:50.078", frameWriteMax1.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameWriteMax2 = new DebugLogRowFrame(1, "11:29:50.078 Close max, T:0.009 W: 0.000 A: 0.000 ", DateTime.MinValue);
            Assert.AreEqual("11:29:50.078", frameWriteMax2.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameCloseMaxStage1 = new DebugLogRowFrame(1, "Record 21:12:04.628 " +
                "RT:26 (MPEG4-Write max, WT:0.561 WW:0.000 WA:0.556 " +
                "AA:0.561 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.561 ) TOT:0.561", DateTime.MinValue);
            Assert.AreEqual("21:12:04.628", frameCloseMaxStage1.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameCloseMaxStage2 = new DebugLogRowFrame(1, "Record 21:12:04.628 " +
                "RT:26 (MPEG4-Write max, WT:0.561 WW:0.000 WA:0.556 Close max, T:0.561 W:-922337203685.478 A:0.001 " +
                "AA:0.561 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.561 ) TOT:0.561", DateTime.MinValue);
            Assert.AreEqual("21:12:04.628", frameCloseMaxStage2.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameCloseMaxStage3 = new DebugLogRowFrame(1, "Record 21:12:04.628 " +
                "RT:26 (MPEG4-Write max, WT:0.561 WW:0.000 WA:0.556 Close max, T:0.561 W:-922337203685.478 A:0.001 " +
                "CLOSEMAXMINUS:[WriteBufferedData]AA:0.561 BB:0.000 CC:0.000 DD:0.000 EE:0.000 FF:0.000 RV:0.561 ) TOT:0.561", DateTime.MinValue);
            Assert.AreEqual("21:12:04.628", frameCloseMaxStage3.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowFrame frameWriteMax4 = new DebugLogRowFrame(1, "09:42:25.186 Write max, WT:4.218 WW:4.208 WA:0.005 ", DateTime.MinValue);
            Assert.AreEqual("09:42:25.186", frameWriteMax4.Timestamp.ToString("HH:mm:ss.fff"));
        }

        [TestMethod]
        public void AviTests()
        {
            DebugLogRowAvi aviRow1 = new DebugLogRowAvi(1, "Create 12:21:23.578 CR1:0.000 CR2:0.000 CR3:0.008 CR4:0.000 CR5:0.000 CR7:0.000 ", DateTime.MinValue);
            Assert.AreEqual(false, aviRow1.CRException);
            Assert.AreEqual(false, aviRow1.CRError);
            Assert.AreEqual("12:21:23.578", aviRow1.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowAvi aviRow2 = new DebugLogRowAvi(1, "Create 13:48:24.660 CR1:0.000 CR2:0.000 CR3:20.152 CRE:0.000 " +
                "CRX:An I/O error occured on Coldstore. Object reference not set to an instance of an object.", DateTime.MinValue);
            Assert.AreEqual(true, aviRow2.CRException);
            Assert.AreEqual(true, aviRow2.CRError);
            Assert.AreEqual("13:48:24.660", aviRow2.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowAvi aviRow3 = new DebugLogRowAvi(1, "Create 10:34:15.248" +
                " CR1:0.000 CR2:0.000 CR3:0.021 Open max, T:0.022 W:0.000 A:0.013 CR4:0.005 CR5:0.064 CR7:0.000 ", DateTime.MinValue);
            Assert.AreEqual(false, aviRow3.CRException);
            Assert.AreEqual(false, aviRow3.CRError);
            Assert.AreEqual("10:34:15.248", aviRow3.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowAvi aviRowOpenAndClose = new DebugLogRowAvi(1, "Create 18:00:25.750" +
                " CR1:0.000 CR2:0.000 CR3:0.012 Open max, T:0.012 W:0.000 A:0.000 Close max, T:0.012 W:0.478 A:0.001 CR4:0.000 CR5:0.000 CR7:0.000 ", DateTime.MinValue);
            Assert.AreEqual("18:00:25.750", aviRowOpenAndClose.Timestamp.ToString("HH:mm:ss.fff"));

            DebugLogRowAvi aviRowOpenAndNegativeClose = new DebugLogRowAvi(1, "Create 18:00:25.750" +
                " CR1:0.000 CR2:0.000 CR3:0.012 Open max, T:0.012 W:0.000 A:0.000 Close max, T:0.012 W:-922337203685.478 A:0.001 CR4:0.000 CR5:0.000 CR7:0.000 ", DateTime.MinValue);
            Assert.AreEqual("18:00:25.750", aviRowOpenAndNegativeClose.Timestamp.ToString("HH:mm:ss.fff"));

            // Sometimes we flag up strange times, I don't know if want this to be handled in the regex or get an exception os it gets flagged up
            // anyway it looks like this
            //String minusCreateWaitTime = "Create 12:01:42.230 CR1:0.000 CR2:0.001 CR3:0.004 Open max, T:0.005 W:0.000 A:0.000 Close max, T:0.005 W:-922337203685.478 A:0.000 CLOSEMAXMINUS:[CreateAVI1]CR4:0.000 CR5:0.000 CR7:0.000 ";
        }

    }
}
