using System;
using System.Drawing;

namespace MakeRuler.Extensions
{
    public static class BitmapExtensions
    {
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
