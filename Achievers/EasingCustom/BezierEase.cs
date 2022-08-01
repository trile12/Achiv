using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

namespace Achievers.EasingCustom
{
    public class BezierEase : EasingFunctionBase
    {
        private readonly ICollection<Point> _controlPoints = new List<Point>
            {
                    new Point(0.00,  0.00),
                    new Point(0.01,  0.00),
                    new Point(0.40,  0.00),
                    new Point(0.70,  0.00),
                    new Point(0.85,  1.00),
                    new Point(0.90,  1.00),
                    new Point(1.00,  1.00)
            };

        protected override Freezable CreateInstanceCore()
        {
            return new BezierEase();
        }

        protected override double EaseInCore(double normalizedTime)
        {
            return GetPointIter(_controlPoints, normalizedTime).X;
        }

        private static Point GetPointIter(ICollection<Point> points, double posX)
        {
            if (points.Count == 1)
            {
                return points.ElementAt(0);
            }

            var result = new List<Point>();

            for (var i = 1; i < points.Count; i++)
            {
                var point1 = points.ElementAt(i - 1);
                var point2 = points.ElementAt(i);
                result.Add(GetPointOnLine(point1, point2, posX));
            }

            return GetPointIter(result, posX);
        }

        private static Point GetPointOnLine(Point point1, Point point2, double posX)
        {
            var x1 = point1.X;
            var x2 = point2.X;
            var y1 = point1.Y;
            var y2 = point2.Y;

            var x = x1 + ((x2 - x1) * posX);
            var y = y1 + ((y2 - y1) * posX);

            return CreateAdjustedPoint(x, y);
        }

        private static Point CreateAdjustedPoint(double x, double y)
        {
            return new Point(x, y);
        }
    }
}