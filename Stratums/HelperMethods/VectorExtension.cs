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

namespace Stratums.HelperMethods
{
    public static class VectorExtension
    {
        public static float GetAngleFromCoord(this Vector2 startCoord, Vector2 destinationCoord)
        {
            var relativeCoordinate = startCoord - destinationCoord;

            return (float)Math.Atan2(-relativeCoordinate.Y, relativeCoordinate.X);
        }
        
        public static float GetAngleFromVector(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static float Atan2(this Vector2 current)
        {
            return (float)Math.Atan2(current.Y, current.X);
        }

        public static double GetDistBetwCoords(this Vector2 current, Vector2 compared) 
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
    }
}
