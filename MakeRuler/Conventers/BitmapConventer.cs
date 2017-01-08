using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MakeRuler.Extensions;
using System.Drawing;

namespace MakeRuler
{
    public static class BitmapConventer
    {
        public static Color GetColor(int material)
        {
            switch (material)
            {
                case 1:
                    return Color.DarkGray;
                case 2:
                    return Color.Blue;
                case 3:
                    return Color.Red;
                case 4:
                    return Color.Green;
                case 5:
                    return Color.Yellow;
                case 6:
                    return Color.Pink;
                case 7:
                    return Color.MediumSeaGreen;
                case 8:
                    return Color.Aqua;
            }
            return Color.Transparent;
        }

        public static Bitmap ToBitmap(Row row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            var lines = row.ToLines();
            if (lines.Count == 0)
            {
                return new Bitmap(1, 1);
            }

            var firstPixel = lines.First().Start;
            var bitmap = new Bitmap(lines.Last().End + firstPixel, 1);
            var g = Graphics.FromImage(bitmap);
            foreach (var line in lines)
            {
                var pen = new Pen(GetColor(line.Material), 1);
                g.DrawLine(pen, line.Start, 0, line.End, 0);
            }

            return bitmap;
        }

        public static Bitmap ToBitmap(Slice slice)
        {
            // + 3 For edges on bottom and right
            var bitmap = new Bitmap(slice.Width + 3, slice.Height + 3);
            var g = Graphics.FromImage(bitmap);
            foreach (var row in slice.Rows)
            {
                g.DrawImage(ToBitmap(row.Value), new Point(0, row.Key));
            }

            return bitmap;
        }


        public static Bitmap ToFrontBitmap(Scene scene)
        {
            var bitmap = new Bitmap(scene.Width, scene.Depth);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var slice in scene.Slices)
            {
                var centerRow = slice.Value.CenterRow;
                graphics.DrawImage(BitmapConventer.ToBitmap(centerRow), new Point(0, (int)(scene.Depth - scene.Depth * (slice.Key - 0.5) / scene.Slices.Count)));
            }

            return bitmap;
        }

        public static Bitmap ToSideBitmap(Scene scene)
        {
            var bitmap = new Bitmap(scene.Height, scene.Depth);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var slice in scene.Slices)
            {
                var centerColumn = slice.Value.CenterColumn;
                graphics.DrawImage(BitmapConventer.ToBitmap(centerColumn), new Point(0, (int)(scene.Depth - scene.Depth * (slice.Key - 0.5) / scene.Slices.Count)));
            }

            return bitmap;
        }
    }
}
