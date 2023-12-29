using Microsoft.Xna.Framework;
using Stratums.Entities;
using Stratums.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Stratums.HelperMethods
{
    public static class TestForEntityCollisions
    {
        public static bool IsEntityColliding(this Entity current, List<Entity> entityList)
        {
            foreach (Entity other in entityList)
            {
                if (current != other && IsEntityColliding(current, other))
                {
                    return true;
                }
            }

            return false;
        }

        public static List<EntityCollisionData> CalculateEntityCollisions(this Entity current, List<Entity> others)
        {
            var collisionDatas = new List<EntityCollisionData>();

            foreach (Entity other in others)
            {
                var collisionData = current.CalculateEntityCollisions(other);
                if (collisionData.IsThereCollisionData())
                {
                    collisionDatas.Add(collisionData);
                }
            }

            return collisionDatas;
        }

        public static bool IsEntityColliding(this Entity current, Entity other)
        {
            EntityCollisionData collisionData = new EntityCollisionData();
            foreach (Hitbox currentHitbox in current.GetHitboxes())
            foreach (Hitbox otherHitbox in other.GetHitboxes())
            {
                if (IsHitboxColliding(currentHitbox, otherHitbox, current.GetPosition(), other.GetPosition(), ref collisionData))
                {
                    return true;
                }
            }

            return false;
        }

        public static EntityCollisionData CalculateEntityCollisions(this Entity current, Entity other)
        {
            var collisionData = new EntityCollisionData();

            foreach (Hitbox currentHitbox in current.GetHitboxes())
            foreach (Hitbox otherHitbox in other.GetHitboxes())
            {
                IsHitboxColliding(currentHitbox, otherHitbox, current.GetPosition(), other.GetPosition(), ref collisionData);
            }

            return collisionData;
        }

        private static bool IsHitboxColliding(Hitbox current, Hitbox other, Vector2 currentPos, Vector2 otherPos, ref EntityCollisionData collisionData)
        {
            return RadiiColliding(current, other, currentPos, otherPos) && VerticesColliding(current.GetVertices(), other.GetVertices(), currentPos, otherPos, ref collisionData);
        }

        private static bool RadiiColliding(Hitbox current, Hitbox other, Vector2 currentPos, Vector2 otherPos)
        {
            var sumOfRadiusLengths = current.GetRadius() + other.GetRadius();
            var distanceBetweenCenters = VectorExtension.GetDistBetwCoords(current.GetCenter() + currentPos, other.GetCenter() + otherPos);

            return sumOfRadiusLengths >= distanceBetweenCenters;
        }

        private static bool VerticesColliding(List<Vector2> current, List<Vector2> other, Vector2 currentPos, Vector2 otherPos, ref EntityCollisionData collisionData)
        {
            for (int i = 0; i < current.Count; i++)
            for (int j = 0; j < other.Count; j++)
            {
                var nextInI = current[(i + 1) % current.Count];
                var nextInJ = other[(j + 1) % other.Count];

                if (LinesColliding(current[i] + currentPos, nextInI + currentPos, other[j] + otherPos, nextInJ + otherPos, ref collisionData))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool LinesColliding(Vector2 current1, Vector2 current2, Vector2 other1, Vector2 other2, ref EntityCollisionData collisionData)
        {
            //  If both lines' slopes are defined
            if (current2.X - current1.X != 0 && other1.X - other2.X != 0)
            {
                return SlopesColliding(current1, current2, other1, other2, ref collisionData);
            }

            //  If current slope is defined and other slope is undefined
            else if (current2.X - current1.X == 0 && other1.X - other2.X != 0)
            {
                return SlopeAndUndefinedColliding(current1, current2, other1, other2, ref collisionData);
            }

            //  If current slope is undefined and other slope is defined
            else if (current2.X - current1.X != 0 && other1.X - other2.X == 0)
            {
                return SlopeAndUndefinedColliding(other1, other2, current1, current2, ref collisionData);
            }

            //  If both lines' slopes are undefined
            else if (current2.X - current1.X == 0 && other1.X - other2.X == 0)
            {
                return UndefinedColliding(other1, other2, current1, current2, ref collisionData);
            }

            Debug.Assert(false, "This section should not be reached!");

            return false;
        }

        private static bool SlopesColliding(Vector2 current1, Vector2 current2, Vector2 other1, Vector2 other2, ref EntityCollisionData collisionData)
        {
            var currentSlope = (current2.Y - current1.Y) / (current2.X - current1.X);
            var otherSlope = (other2.Y - other1.Y) / (other2.X - other1.X);

            var currentIntercept = -(currentSlope * current1.X) + current1.Y;
            var otherIntercept = -(otherSlope * other1.X) + other1.Y;

            var currentSmallestValue = FloatExtension.SmallestValue(current1.X, current2.X);
            var currentGreatestValue = FloatExtension.GreatestValue(current1.X, current2.X);
            var otherSmallestValue = FloatExtension.SmallestValue(other1.X, other2.X);
            var otherGreatestValue = FloatExtension.GreatestValue(other1.X, other2.X);

            //Backup for if the lines are the same (dividing by zero)
            if (-currentSlope + otherSlope == 0 && currentIntercept == otherIntercept)
            {
                var totalDistance = (currentGreatestValue - currentSmallestValue) + (otherGreatestValue - otherSmallestValue);
                var rangeOfPoints = currentGreatestValue.GreatestValue(otherGreatestValue) - currentSmallestValue.SmallestValue(otherSmallestValue);

                if (totalDistance >= rangeOfPoints) 
                {
                    Vector2 intersection1;
                    Vector2 intersection2;

                    if (currentGreatestValue >= otherGreatestValue)
                    {
                        intersection1.X = currentGreatestValue - otherGreatestValue;
                    }
                    else
                    {
                        intersection1.X = otherGreatestValue - currentGreatestValue;
                    }

                    if (currentSmallestValue >= otherSmallestValue)
                    {
                        intersection2.X = currentSmallestValue - otherSmallestValue;
                    }
                    else
                    {
                        intersection2.X = otherSmallestValue - currentSmallestValue;
                    }

                    intersection1.Y = currentSlope * intersection1.X + currentIntercept;
                    intersection2.Y = currentSlope * intersection2.X + currentIntercept;

                    collisionData.OverlappingLines.Add(new Tuple<Vector2, Vector2>(intersection1, intersection2));
                    return true;
                }

                return false;
            }

            var xIntersection = (currentIntercept - otherIntercept) / (-currentSlope + otherSlope);
            var yIntersection = currentSlope * xIntersection + currentIntercept;

            //Makes sure the intersection point is within the edge x values of both lines
            if (currentSmallestValue <= xIntersection && otherSmallestValue <= xIntersection && currentGreatestValue >= xIntersection && otherGreatestValue >= xIntersection)
            {
                collisionData.IntersectionPoints.Add(new Vector2(xIntersection, yIntersection));
                return true;
            }

            return false;
        }

        private static bool SlopeAndUndefinedColliding(Vector2 undefined1, Vector2 undefined2, Vector2 point1, Vector2 point2, ref EntityCollisionData collisionData)
        {
            var slope = (point2.Y - point1.Y) / (point2.X - point1.X);
            var intercept = -(slope * point1.X) + point1.Y;
            var yIntersection = slope * undefined1.X + intercept;

            var undefinedSmallestYValue = FloatExtension.SmallestValue(undefined1.Y, undefined2.Y);
            var undefinedGreatestYValue = FloatExtension.GreatestValue(undefined1.Y, undefined2.Y);
            var slopeSmallestXValue = FloatExtension.SmallestValue(point1.X, point2.X);
            var slopeGreatestXValue = FloatExtension.GreatestValue(point1.X, point2.X);

            bool isYIntersectionInRange = undefinedSmallestYValue <= yIntersection && undefinedGreatestYValue >= yIntersection;
            bool isXIntersectionInRange = slopeSmallestXValue <= undefined1.X && slopeGreatestXValue >= undefined1.X;

            if (isYIntersectionInRange && isXIntersectionInRange)
            {
                collisionData.IntersectionPoints.Add(new Vector2(undefined1.X, yIntersection));
                return true;
            }

            return false;
        }

        private static bool UndefinedColliding(Vector2 current1, Vector2 current2, Vector2 other1, Vector2 other2, ref EntityCollisionData collisionData)
        {
            //NOTE: Find optimizations for these calculations later!
            var currentSmallestYValue = FloatExtension.SmallestValue(current1.Y, current2.Y);
            var currentGreatestYValue = FloatExtension.GreatestValue(current1.Y, current2.Y);
            var otherSmallestYValue = FloatExtension.SmallestValue(other1.Y, other2.Y);
            var otherGreatestYValue = FloatExtension.GreatestValue(other1.Y, other2.Y);

            var totalDistance = (currentGreatestYValue - currentSmallestYValue) + (otherGreatestYValue - otherSmallestYValue);
            var rangeOfPoints = currentGreatestYValue.GreatestValue(otherGreatestYValue) - currentSmallestYValue.SmallestValue(otherSmallestYValue);

            //Debug.Assert(!(current1.X == other1.X && totalDistance >= rangeOfPoints));

            float topOfIntersection;
            float bottomOfIntersection;

            if (currentGreatestYValue >= otherGreatestYValue)
            {
                topOfIntersection = otherGreatestYValue;
            }
            else
            {
                topOfIntersection = currentGreatestYValue;
            }

            if (currentSmallestYValue >= otherSmallestYValue) 
            {
                bottomOfIntersection = currentSmallestYValue;
            }
            else
            {
                bottomOfIntersection = otherSmallestYValue;
            }

            if (current1.X == other1.X && totalDistance >= rangeOfPoints)
            {
                collisionData.OverlappingLines.Add(new Tuple<Vector2, Vector2>(new Vector2(current1.X, topOfIntersection), new Vector2(current1.X, bottomOfIntersection)));
                return true;
            }
            return false;
        }
    }
}
