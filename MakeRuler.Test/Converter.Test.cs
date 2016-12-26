using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeRuler.Extensions.Test
{
    [TestClass]
    public class Converter_Test
    {
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

        public void AreEqualLayers(KeyValuePair<int, Scene> expected, KeyValuePair<int, Scene> actual)
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

        public void AreEqualScene(SortedDictionary<int, Scene> expected, SortedDictionary<int, Scene> actual)
        {
            var expectedLayers = expected.ToList();
            var actualLayers = actual.ToList();
            Assert.AreEqual(expectedLayers.Count, actualLayers.Count);

            for (var i = 0; i < actualLayers.Count; ++i)
            {
                AreEqualLayers(expectedLayers[i], actualLayers[i]);
            }
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

            var expected = new SortedDictionary<int, Scene>();
            var layer = new Scene();
            layer.Add(new Rect(0, 0, 2, 2, 2));
            expected.Add(1, layer);
            #endregion

            var actual = Conventer.SceneFromText(text);

            AreEqualScene(expected, actual);
        }
        /*
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
        }*/
    }
}
