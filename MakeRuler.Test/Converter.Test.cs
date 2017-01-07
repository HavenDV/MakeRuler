using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeRuler.Extensions.Test
{
    [TestClass]
    public class Converter_Test
    {
        public void AreEqualRows(KeyValuePair<int, Row> expected, KeyValuePair<int, Row> actual)
        {
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value.Data.Count, actual.Value.Data.Count);

            var expectedLines = expected.Value.ToLines();
            var actualLines = actual.Value.ToLines();
            Assert.AreEqual(expectedLines.Count, actualLines.Count);
            for (var i = 0; i < actualLines.Count; ++i)
            {
                Assert.AreEqual(expectedLines[i].Start, actualLines[i].Start);
                Assert.AreEqual(expectedLines[i].End, actualLines[i].End);
                Assert.AreEqual(expectedLines[i].Material, actualLines[i].Material);
            }
        }

        public void AreEqualSlices(KeyValuePair<int, Slice> expected, KeyValuePair<int, Slice> actual)
        {
            Assert.AreEqual(expected.Key, actual.Key);

            var expectedRows = expected.Value.Rows.ToList();
            var actualRows = actual.Value.Rows.ToList();
            Assert.AreEqual(expectedRows.Count, actualRows.Count);

            for (var i = 0; i < actualRows.Count; ++i)
            {
                AreEqualRows(expectedRows[i], actualRows[i]);
            }
        }

        public void AreEqualScene(Scene expected, Scene actual)
        {
            var expectedSlices = expected.Slices.ToList();
            var actualSlices = actual.Slices.ToList();
            Assert.AreEqual(expectedSlices.Count, actualSlices.Count);

            for (var i = 0; i < actualSlices.Count; ++i)
            {
                AreEqualSlices(expectedSlices[i], actualSlices[i]);
            }
        }

        [TestMethod]
        public void Converter_ToText_SimpleRow()
        {
            #region TestData
            var row = new Row();
            row.AddLine(new Line(1, 3, 3));
            #endregion

            var expected = @"ROW NR.   2  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    3   3";
            var actual = row.ToText(2, false);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Converter_ToText_EmptyRow()
        {
            #region TestData
            var row = new Row();
            #endregion

            var expected = @"ROW NR.   3  FIRST PIXEL:   1  NUMBER OF AREAS:  0
";
            var actual = row.ToText(3, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Converter_ToText_SimpleIntegerRect()
        {
            #region TestData
            var scene = new Slice();
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
            var scene = new Slice();
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
            var scene = new Slice();
            scene.Add(new Rect(0.8, 0.8, 1.8, 1.8, 3));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  2  LAST ROW:  2
ROW NR.   2  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   3";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Converter_ToText_SimpleIntegerRects()
        {
            #region TestData
            var scene = new Slice();
            scene.Add(new Rect(0, 0, 4, 4, 1));
            scene.Add(new Rect(1, 1, 3, 3, 2));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  4
ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    4   1
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  3
    1   1    3   2    4   1
ROW NR.   3  FIRST PIXEL:   1  NUMBER OF AREAS:  3
    1   1    3   2    4   1
ROW NR.   4  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    4   1";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Converter_ToText_SimpleIntegerCircle()
        {
            #region TestData
            var scene = new Slice();
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
            var scene = new Slice();
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
            var scene = new Slice();
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
            var scene = new Slice();
            scene.Add(new Circle(1.5, 1.5, 0.5, 8));
            #endregion

            var expected = @"SLICE NUMBER:  1  FIRST ROW:  2  LAST ROW:  2
ROW NR.   2  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   8";
            var actual = scene.ToText(1, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Converter_FromText_SimpleRow()
        {
            #region TestData
            var text = @"ROW NR.   2  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    3   3";
            var expected = new KeyValuePair<int, Row>(2, new Row());
            expected.Value.AddLine(new Line(1, 3, 3));
            #endregion

            var actual = Row.FromText(text);
            AreEqualRows(expected, actual);
        }
        
        [TestMethod]
        public void Converter_FromText_EmptyRow()
        {
            #region TestData
            var text = @"ROW NR.   3  FIRST PIXEL:   1  NUMBER OF AREAS:  0
";
            var expected = new KeyValuePair<int, Row>(3, new Row());
            #endregion

            var actual = Row.FromText(text);
            AreEqualRows(expected, actual);
        }

        [TestMethod]
        public void Converter_FromText_SimpleIntegerRect()
        {
            #region TestData
            var text = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  2
ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   2
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   2";

            var expected = new Scene();
            var slice = new Slice();
            slice.Add(new Rect(0, 0, 2, 2, 2));
            expected.AddSlice(1, slice);
            #endregion

            var actual = Conventer.SceneFromText(text);

            AreEqualScene(expected, actual);
        }
        
        [TestMethod]
        public void Converter_FromText_SimpleDecimalRect()
        {
            #region TestData
            var text = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  2
ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   2
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   2";
            var expected = new Scene();
            var slice = new Slice();
            slice.Add(new Rect(0.5, 0.5, 1.5, 1.5, 2));
            expected.AddSlice(1, slice);
            #endregion

            var actual = Conventer.SceneFromText(text);

            AreEqualScene(expected, actual);
        }
        
        [TestMethod]
        public void Converter_FromText_SimpleDecimalRect2()
        {
            #region TestData
            var text = @"SLICE NUMBER:  1  FIRST ROW:  2  LAST ROW:  2
ROW NR.   2  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   3";
            var expected = new Scene();
            var slice = new Slice();
            slice.Add(new Rect(0.8, 0.8, 1.8, 1.8, 3));
            expected.AddSlice(1, slice);
            #endregion

            var actual = Conventer.SceneFromText(text);

            AreEqualScene(expected, actual);
        }

        [TestMethod]
        public void Converter_FromText_SimpleIntegerCircle()
        {
            #region TestData
            var text = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  2
ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   4
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    2   4";
            var expected = new Scene();
            var slice = new Slice();
            slice.Add(new Circle(1, 1, 1, 4));
            expected.AddSlice(1, slice);
            #endregion

            var actual = Conventer.SceneFromText(text);

            AreEqualScene(expected, actual);
        }
        
        [TestMethod]
        public void Converter_FromText_SimpleDecimalCircle()
        {
            #region TestData
            var text = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  3
ROW NR.   1  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   5
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    3   5
ROW NR.   3  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   5";
            var expected = new Scene();
            var slice = new Slice();
            slice.Add(new Circle(1.5, 1.5, 1, 5));
            expected.AddSlice(1, slice);
            #endregion

            var actual = Conventer.SceneFromText(text);

            AreEqualScene(expected, actual);
        }

        [TestMethod]

        public void Converter_FromText_SimpleDecimalCircle2()
        {
            #region TestData
            var text = @"SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:  3
ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    3   6
ROW NR.   2  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    3   6
ROW NR.   3  FIRST PIXEL:   1  NUMBER OF AREAS:  1
    3   6";
            var expected = new Scene();
            var slice = new Slice();
            slice.Add(new Circle(1.5, 1.5, 1.5, 6));
            expected.AddSlice(1, slice);
            #endregion

            var actual = Conventer.SceneFromText(text);

            AreEqualScene(expected, actual);
        }

        [TestMethod]
        public void Converter_FromText_SimpleDecimalCircle3()
        {
            #region TestData
            var text = @"SLICE NUMBER:  1  FIRST ROW:  2  LAST ROW:  2
ROW NR.   2  FIRST PIXEL:   2  NUMBER OF AREAS:  1
    2   8";
            var expected = new Scene();
            var slice = new Slice();
            slice.Add(new Circle(1.5, 1.5, 0.5, 8));
            expected.AddSlice(1, slice);
            #endregion

            var actual = Conventer.SceneFromText(text);

            AreEqualScene(expected, actual);
        }
    }
}
