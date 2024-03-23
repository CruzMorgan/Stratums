using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Threading.Tasks;
using Stratums.Properties;
using Stratums.HelperMethods;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Net.Http.Headers;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Stratums.HelperMethods
{
    public static class VectorExtension
    {

        public static double ToAngle(this Vector2 thisCoordinate, Vector2 otherCoordinate)
        {
            var relativeCoordinate = otherCoordinate - thisCoordinate;

            var angle = Math.Atan2(relativeCoordinate.Y, relativeCoordinate.X) * (180 / Math.PI);

            return angle < 0 ? 360 + angle : angle;
        }
        
        public static float GetAngleFromVector(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static float Atan2(this Vector2 current)
        {
            return (float)Math.Atan2(current.Y, current.X);
        }

        public static double Dist(this Vector2 current, Vector2 compared) 
        {
            return Math.Sqrt( Math.Pow(current.X - compared.X, 2) + Math.Pow(current.Y - compared.Y, 2) );
        }

        public static Vector2 Rounded(this Vector2 current)
        {
            return new Vector2((float)Math.Round(current.X), (float)Math.Round(current.Y));
        }

        public static bool IsPointWithinRange(this Vector2 tested, Vector2 range1, Vector2 range2)
        {
            float minX;
            float minY;
            float maxX;
            float maxY;

            //Finds which X value is greater and which is smaller
            if (range1.X > range2.X)
            {
                maxX = range1.X;
                minX = range2.X;
            } 
            else 
            {
                maxX = range2.X;
                minX = range1.X;
            }

            //Finds which Y value is greater and which is smaller
            if (range1.Y > range2.Y)
            {
                maxY = range1.Y;
                minY = range2.Y;
            }
            else
            {
                maxY = range2.Y;
                minY = range1.Y;
            }

            return minX <= tested.X && tested.X <= maxX && minY <= tested.Y && tested.Y <= maxY;
        }

        public static Vector2 Reverse(this Vector2 current)
        {
            return current * -1;
        }

        public static Vector2 Normalized(this Vector2 current)
        {
            float num = 1f / MathF.Sqrt(current.X * current.X + current.Y * current.Y);

            return current * num;
        }

        /// <summary>
        /// Slow Calculation; avoid using in a spot that needs to be efficient
        /// </summary>
        /// <param name="points"></param>
        /// <returns>Two Vectors representing the range of all vectors</returns>
        public static Tuple<Vector2, Vector2> RangeOfValues(this List<Vector2> points)
        {
            float minX = 0;
            float minY = 0;
            float maxX = 0;
            float maxY = 0;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X > minX) { minX = points[i].X; }
                if (points[i].X < maxX) { maxX = points[i].X; }
                if (points[i].Y > minY) { minY = points[i].Y; }
                if (points[i].Y < maxY) { maxY = points[i].Y; }
            }

            return new Tuple<Vector2, Vector2> ( new Vector2(minX, minY), new Vector2(maxX, maxY) );
        }

        public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
        {
            return new Vector2(Math.Clamp(value.X, min.X, max.X), Math.Clamp(value.Y, min.Y, max.Y));
        }

        public static Vector2 InvertY(this Vector2 value)
        {
            return new Vector2(value.X, -value.Y);
        }

        public static Vector2 AddToEach(this Vector2 value1, float value2)
        {
            return new Vector2(value1.X + value2, value1.Y + value2);
        }
        public static Vector2 AddToEach(this Vector2 value1, float x, float y)
        {
            return new Vector2(value1.X + x, value1.Y + y);
        }

        public static byte QuadrantFromOrigin(this Vector2 value, Vector2 origin)
        {
            value -= origin;

            if (value.X == 0)
            {
                value.X = 1;
            }
            if (value.Y == 0)
            {
                value.Y = 1;
            }

            value /= new Vector2(Math.Abs(value.X), Math.Abs(value.Y));


            int toInt = (int)value.X * 10 + (int)value.Y;

            switch (toInt)
            {
                case 11:
                    return 1;

                case -9:
                    return 2;

                case -11:
                    return 3;

                case 9:
                    return 4;

                default:
                    Debug.Assert(false, "Should be unreachable");
                    return 0;
            }
        }

    }
}
