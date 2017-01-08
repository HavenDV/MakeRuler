using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeRuler.Extensions.Test
{
    [TestClass]
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

        [TestMethod]
        public void Row_SimpleLine()
        {
            var row = new Row();
            row.AddLine(new Line(0, 3, 1));

            var lines = row.ToLines();
            Assert.AreEqual(1, lines.Count, "Lines count not equal");
            AreEqualLines(new Line(0, 3, 1), lines[0]);
            Assert.AreEqual(3, row.Width, "Row width not equal");
        }

        [TestMethod]
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
        }

        [TestMethod]
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
        }

    }
}
