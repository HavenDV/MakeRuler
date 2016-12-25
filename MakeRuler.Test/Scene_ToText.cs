using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeRuler.Test
{
    [TestClass]
    public class Scene_ToText
    {
        [TestMethod]
        public void SimpleIntegerRect()
        {
            #region TestData
            var scene = new Scene();
            scene.Add(new Rect(0, 0, 2, 2, 2));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  2
ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   2
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   2";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SimpleDecimalRect()
        {
            #region TestData
            var scene = new Scene();
            scene.Add(new Rect(0.5, 0.5, 1.5, 1.5, 2));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  2
ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   2
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   2";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SimpleDecimalRect2()
        {
            #region TestData
            var scene = new Scene();
            scene.Add(new Rect(0.8, 0.8, 1.8, 1.8, 3));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  2  LAST ROW:  2
ROW NR.   2  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   3";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SimpleIntegerCircle()
        {
            #region TestData
            var scene = new Scene();
            scene.Add(new Circle(1, 1, 1, 4));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  2
ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   4
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   4";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SimpleDecimalCircle()
        {
            #region TestData
            var scene = new Scene();
            scene.Add(new Circle(1.5, 1.5, 1, 5));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  3
ROW NR.   1  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   5
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    3   5
ROW NR.   3  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   5";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SimpleDecimalCircle2()
        {
            #region TestData
            var scene = new Scene();
            scene.Add(new Circle(1.5, 1.5, 1.5, 6));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  3
ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    3   6
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    3   6
ROW NR.   3  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    3   6";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void SimpleDecimalCircle3()
        {
            #region TestData
            var scene = new Scene();
            scene.Add(new Circle(1.5, 1.5, 0.5, 8));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  2  LAST ROW:  2
ROW NR.   2  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   8";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }
    }
}
