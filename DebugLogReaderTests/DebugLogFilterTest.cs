﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DebugLogReader;
using System.Collections.Generic;

namespace DebugLogReaderTests
{
    [TestClass]
    public class DebugLogFilterTest
    {
        [TestMethod]
        public void NoFilterTest()
        {
            // Create mock file wrapper for all tests
            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(x => x.LoadFromFile("")).Returns(new String[] { });

            // Create debugLog with mock file wrapper and no filters
            DebugLog debugLog = new DebugLog(mockFileWrapper.Object, 1, null);

            debugLog.Load("");
        }

        [TestMethod]
        public void CameraFilterTest()
        {
            // Create mock file wrapper for all tests
            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(x => x.LoadFromFile("test")).Returns(new String[] {
            "Pushed - 09/03/2016 16:20:16.843 ---  (0.060 seconds) Q:0 F:8, 6174, 0",
            "Pushed - 09/03/2016 16:20:16.853 ---  (0.010 seconds) Q:1 F:9, 6441, 0",
            "Pushed - 09/03/2016 16:20:16.893 ---  (0.040 seconds) Q:2 F:10, 6190, 0",
            "Pushed - 09/03/2016 16:20:17.193 ---  (0.040 seconds) Q:3 F:11, 6190, 0",
            "Pushed - 09/03/2016 16:20:17.393 ---  (0.040 seconds) Q:4 F:12, 6190, 0"
            });

            // QueueCount equal to 1
            List<DebugLogFilter> filters = new List<DebugLogFilter>();
            filters.Add(new DebugLogFilter("QueueCount", eFilterComparision.EqualTo, 1));
            // Create debugLog with mock file wrapper and test filters
            DebugLog debugLog1 = new PushDebugLog(mockFileWrapper.Object, 1, filters);

            debugLog1.Load("test");
            Assert.AreEqual(1, debugLog1.Count);
            Assert.AreEqual(new DateTime(2016, 3, 9, 16, 20, 16).AddMilliseconds(853), debugLog1.GetStartTime());

            // Start time less than
            filters = new List<DebugLogFilter>();
            filters.Add(new DebugLogFilter("Timestamp", eFilterComparision.LessThan, new DateTime(2016, 3, 9, 16, 20, 16).AddMilliseconds(860)));
            // Create debugLog with mock file wrapper and test filters
            DebugLog debugLog2 = new PushDebugLog(mockFileWrapper.Object, 1, filters);

            debugLog2.Load("test");
            Assert.AreEqual(2, debugLog2.Count);
            Assert.AreEqual(new DateTime(2016, 3, 9, 16, 20, 16).AddMilliseconds(843), debugLog2.GetStartTime());
            Assert.AreEqual(new DateTime(2016, 3, 9, 16, 20, 16).AddMilliseconds(853), debugLog2.GetEndime());
            
            // Start time less than and greater than
            filters = new List<DebugLogFilter>();
            filters.Add(new DebugLogFilter("Timestamp", eFilterComparision.GreaterThan, new DateTime(2016, 3, 9, 16, 20, 16).AddMilliseconds(850)));
            filters.Add(new DebugLogFilter("Timestamp", eFilterComparision.LessThan, new DateTime(2016, 3, 9, 16, 20, 17).AddMilliseconds(200)));
            // Create debugLog with mock file wrapper and test filters
            DebugLog debugLog3 = new PushDebugLog(mockFileWrapper.Object, 1, filters);

            debugLog3.Load("test");
            Assert.AreEqual(3, debugLog3.Count);
            Assert.AreEqual(new DateTime(2016, 3, 9, 16, 20, 16).AddMilliseconds(853), debugLog3.GetStartTime());
            Assert.AreEqual(new DateTime(2016, 3, 9, 16, 20, 17).AddMilliseconds(193), debugLog3.GetEndime());

            // Invalid filter
            filters = new List<DebugLogFilter>();
            filters.Add(new DebugLogFilter("Bananas", eFilterComparision.GreaterThan, 2));
            // Create debugLog with mock file wrapper and test filters
            DebugLog debugLog4 = new PushDebugLog(mockFileWrapper.Object, 1, filters);

            debugLog4.Load("test");
            Assert.AreEqual(0, debugLog4.Count);
        }

        [TestMethod]
        public void NewCameraFilterTest()
        {
            // Create mock file wrapper for all tests
            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(x => x.LoadFromFile("test")).Returns(new String[] {
            "Pushed - 09/03/2016 16:20:16.843 ---  (0.060 seconds) Q:0 F:8, 6174, 0",
            "Pushed - 09/03/2016 16:20:16.853 ---  (0.010 seconds) Q:1 F:9, 6441, 0",
            "Pushed - 09/03/2016 16:20:16.893 ---  (0.040 seconds) Q:2 F:10, 6190, 0"
            });

            // QueueCount equal to 1
            List<DebugLogFilter> filters = new List<DebugLogFilter>();
            filters.Add(new DebugLogFilter("QueueCount", eFilterComparision.EqualTo, 1));
            // Create debugLog with mock file wrapper and test filters
            DebugLog debugLog1 = new PushDebugLog(mockFileWrapper.Object, 1, filters);

            debugLog1.Load("test");
            Assert.AreEqual(1, debugLog1.Count);
            Assert.AreEqual(new DateTime(2016, 3, 9, 16, 20, 16).AddMilliseconds(853), debugLog1.GetStartTime());
        }
    }
}
