using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeRuler.Extensions.Test
{
    [TestClass]
    public class Converter
    {
        [TestMethod]
        public void Converter_ToText_SimpleRow()
        {
            var row = new Row();
            row.AddLine(new Line(1, 3, 3));

            var expected = @"ROW NR.   2  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    3   3";
            var actual = row.ToText(2, false);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Converter_ToText_EmptyRow()
        {
            var row = new Row();

            var expected = @"ROW NR.   3  FIRST PIXEL:   1  NUMBER OF AREAS:  0
";
            var actual = row.ToText(3, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Converter_ToText_SimpleIntegerRect()
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
        public void Converter_ToText_SimpleDecimalRect()
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
        public void Converter_ToText_SimpleDecimalRect2()
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
        public void Converter_ToText_SimpleIntegerCircle()
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
        public void Converter_ToText_SimpleDecimalCircle()
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

        public void Converter_ToText_SimpleDecimalCircle2()
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
        public void Converter_ToText_SimpleDecimalCircle3()
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
