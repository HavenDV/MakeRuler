using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeRuler.Extensions.Test
{
    [TestFixture]
    public class Row_Test
    {
        static public void AreEqualLines(Line expected, Line actual)
        {
            var message = $@"
Expected line: Line({expected.Start}, {expected.End}, {expected.Material})
Actual line: Line({actual.Start}, {actual.End}, {actual.Material})
";
            Assert.AreEqual(expected.Start, actual.Start, message + "Lines start not equals");
            Assert.AreEqual(expected.End, actual.End, message + "Lines end not equals");
            Assert.AreEqual(expected.Material, actual.Material, message + "Lines material not equals");
        }

        static public void PrintRowData(Row row)
        {
            foreach (var data in row.Data)
            {
                Console.Write(data.Key);
            }
            Console.Write(Environment.NewLine);
            foreach (var data in row.Data)
            {
                Console.Write(data.Value);
            }
            Console.Write(Environment.NewLine);
            foreach (var line in row.ToLines())
            {
                Console.Write($"Line({line.Start}, {line.End}, {line.Material}) ");
            }
            Console.Write(Environment.NewLine);
            Console.WriteLine(row.ToText(1));
        }

        [Test]
        public void Row_SimpleLine()
        {
            var row = new Row();
            row.AddLine(new Line(0, 3, 1));

            var lines = row.ToLines();
            Assert.AreEqual(1, lines.Count, "Lines count not equal");
            AreEqualLines(new Line(0, 3, 1), lines[0]);
            Assert.AreEqual(3, row.Width, "Row width not equal");
            Assert.AreEqual(1, row.Center, "Row center not equal");
            Assert.IsFalse(row.IsEmpty, "Row is empty");
            Assert.AreEqual(1, row.GetValue(0), "Row(0) not equal");
            Assert.AreEqual(1, row.GetValue(1), "Row(1) not equal");
            Assert.AreEqual(1, row.GetValue(2), "Row(2) not equal");
            Assert.AreEqual(Constants.AirMaterial, row.GetValue(3), "Row(3) not equal");
        }

        [Test]
        public void Row_TwoLinesIntersect()
        {
            var row = new Row();
            row.AddLine(new Line(0, 3, 1));
            row.AddLine(new Line(2, 4, 2));
            PrintRowData(row);

            var lines = row.ToLines();
            Assert.AreEqual(2, lines.Count, "Lines count not equal");
            AreEqualLines(new Line(0, 2, 1), lines[0]);
            AreEqualLines(new Line(2, 4, 2), lines[1]);
            Assert.AreEqual(4, row.Width, "Row width not equal");
            Assert.AreEqual(2, row.Center, "Row center not equal");
            Assert.IsFalse(row.IsEmpty, "Row is empty");
            Assert.AreEqual(1, row.GetValue(0), "Row(0) not equal");
            Assert.AreEqual(1, row.GetValue(1), "Row(1) not equal");
            Assert.AreEqual(2, row.GetValue(2), "Row(2) not equal");
            Assert.AreEqual(2, row.GetValue(3), "Row(3) not equal");
            Assert.AreEqual(Constants.AirMaterial, row.GetValue(4), "Row(4) not equal");
        }

        [Test]
        public void Row_TwoLinesIntersectInvert()
        {
            var row = new Row();
            row.AddLine(new Line(2, 4, 2));
            row.AddLine(new Line(0, 3, 1));
            PrintRowData(row);

            var lines = row.ToLines();
            Assert.AreEqual(2, lines.Count);
            AreEqualLines(new Line(0, 3, 1), lines[0]);
            AreEqualLines(new Line(3, 4, 2), lines[1]);
            Assert.AreEqual(4, row.Width, "Row width not equal");
            Assert.AreEqual(2, row.Center, "Row center not equal");
            Assert.IsFalse(row.IsEmpty, "Row is empty");
            Assert.AreEqual(1, row.GetValue(0), "Row(0) not equal");
            Assert.AreEqual(1, row.GetValue(1), "Row(1) not equal");
            Assert.AreEqual(1, row.GetValue(2), "Row(2) not equal");
            Assert.AreEqual(2, row.GetValue(3), "Row(3) not equal");
            Assert.AreEqual(Constants.AirMaterial, row.GetValue(4), "Row(4) not equal");
        }

    }
}
