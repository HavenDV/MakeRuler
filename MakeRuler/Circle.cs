﻿using System;
using System.Collections.Generic;
using MakeRuler.Extensions;

namespace MakeRuler
{
    public class Circle : Object
    {
        public Point2D Center { get; set; }
        public double Radius { get; set; }
        
        public Circle(Point2D center, double radius, int material) :
            base(material)
        {
            Center = center;
            Radius = radius;
            Min = new Point2D(Center.X - Radius, Center.Y - Radius);
            Max = new Point2D(Center.X + Radius, Center.Y + Radius);
        }

        public Circle(double x, double y, double radius, int material) :
            this(new Point2D(x, y), radius, material)
        {}

        public override SortedDictionary<int, Line> ToLines()
        {
            var lines = new SortedDictionary<int, Line>();
            
            for (int y = Min.minY + 1; y <= Max.maxY; ++y)
            {
                var dy = Center.Y + 0.5 - y;
                if (dy <= Radius)
                {
                    var dx = Math.Sqrt(Radius*Radius - dy*dy);
                    var x1 = Center.X - dx;
                    var x2 = Center.X + dx;
                    lines.Add(y, new Line(x1.RoundMin(), x2.RoundMax(), Material));
                }
            }

            return lines;
        }
    }
}