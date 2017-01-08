using System;
using System.Drawing;

namespace MakeRuler.Extensions
{
    public static class BitmapExtensions
    {
        public static Bitmap ToPerspective(this Bitmap bitmap)
        {
            PointF[] destinationPoints = {
                new PointF(0.0F, 0.0F),
                new PointF(bitmap.Width, 0.0F),
                new PointF(0.5F * bitmap.Height, 0.5F * bitmap.Height)
            };

            var newBitmap = new Bitmap(bitmap.Width + bitmap.Height / 2, bitmap.Height);
            var graphics = Graphics.FromImage(newBitmap);
            graphics.DrawImage(bitmap, destinationPoints);

            return newBitmap;
        }

        public static Bitmap WithBorder(this Bitmap bitmap, Color color)
        {
            int w = bitmap.Width;
            int h = bitmap.Height;
            Point lastPoint = new Point();
            Color lastColor = new Color();
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (pixel.A != Color.Transparent.A &&
                        lastColor.A == Color.Transparent.A)
                    {
                        bitmap.SetPixel(lastPoint.X, lastPoint.Y, color);
                    }
                    lastPoint = new Point(x, y);
                    lastColor = bitmap.GetPixel(x, y);
                }
            }

            for (int y = h - 1; y > 0; y--)
            {
                for (int x = w - 1; x > 0; x--)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (pixel.A != Color.Transparent.A &&
                        lastColor.A == Color.Transparent.A)
                    {
                        bitmap.SetPixel(lastPoint.X, lastPoint.Y, color);
                    }
                    lastPoint = new Point(x, y);
                    lastColor = bitmap.GetPixel(x, y);
                }
            }

            for (int x = w - 1; x > 0; x--)
            {
                for (int y = 0; y < h; ++y)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (pixel.A != Color.Transparent.A &&
                        lastColor.A == Color.Transparent.A)
                    {
                        bitmap.SetPixel(lastPoint.X, lastPoint.Y, color);
                    }
                    lastPoint = new Point(x, y);
                    lastColor = bitmap.GetPixel(x, y);
                }
            }

            for (int x = w - 1; x > 0; x--)
            {
                for (int y = h - 1; y > 0; y--)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (pixel.A != Color.Transparent.A &&
                        lastColor.A == Color.Transparent.A)
                    {
                        bitmap.SetPixel(lastPoint.X, lastPoint.Y, color);
                    }
                    lastPoint = new Point(x, y);
                    lastColor = bitmap.GetPixel(x, y);
                }
            }

            return bitmap;
        }
    }
}
