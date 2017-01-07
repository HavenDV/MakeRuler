﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeRuler.Extensions.Test
{
    [TestClass]
    public class Line_Test
    {
        [TestMethod]
        public void Line_Positive()
        {
            var line = new Line(1, 2, 3);
            Assert.AreEqual(1, line.Start);
            Assert.AreEqual(2, line.End);
            Assert.AreEqual(3, line.Material);
        }

        [TestMethod]
        public void Line_NegativeMaterial()
        {
            try
            {
                var line = new Line(1, 2, -1);
                Assert.Fail("No exception detected");
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, "Invalid material");
            }
        }

        [TestMethod]
        public void Line_EndLowerStart()
        {
            try
            {
                var line = new Line(2, 1, 1);
                Assert.Fail("No exception detected");
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, "End < Start");
            }
        }
    }
}