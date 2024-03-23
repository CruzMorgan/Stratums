using Microsoft.Xna.Framework;
using Stratums.HelperMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Properties.Hitbox
{
    internal static class TestCollisionCases
    {
        internal static bool CircleAndCircle(CircleHitbox circle1, CircleHitbox circle2)
        {
            return circle1._globalPosition.Dist(circle2._globalPosition) <= circle1.Radius + circle2.Radius;
        }

        internal static bool CircleAndRectangle(CircleHitbox circle, RectangleHitbox rectangle)
        {
            if (rectangle.IsPointWithinHitbox(circle._globalPosition))
            {
                return true;
            }

            Vector2 minRange = new Vector2(rectangle._globalPosition.X - rectangle.Width / 2, rectangle._globalPosition.Y - rectangle.Height / 2);
            Vector2 maxRange = new Vector2(rectangle._globalPosition.X + rectangle.Width / 2, rectangle._globalPosition.Y + rectangle.Height / 2);

            Vector2 closestPointToCircle = circle._globalPosition.Clamp(minRange, maxRange);

            return circle._globalPosition.Dist(closestPointToCircle) <= circle.Radius;
        }

        internal static bool RectangleAndRectangle(RectangleHitbox rectangle1, RectangleHitbox rectangle2)
        {
            float[] verticalSides = new float[] 
            { 
                rectangle1.getGlobalPositionSides()[0], 
                rectangle1.getGlobalPositionSides()[2], 
                rectangle2.getGlobalPositionSides()[0], 
                rectangle2.getGlobalPositionSides()[2] 
            };
            float[] horizontalSides = new float[]
            {
                rectangle1.getGlobalPositionSides()[1],
                rectangle1.getGlobalPositionSides()[3],
                rectangle2.getGlobalPositionSides()[1],
                rectangle2.getGlobalPositionSides()[3]

            };

            Vector2 min = new Vector2(horizontalSides.Min(), verticalSides.Min());
            Vector2 max = new Vector2(horizontalSides.Max(), verticalSides.Max());

            bool isCollidingHorizontally = (rectangle1.Width + rectangle2.Width) >= (max.X - min.X);
            bool isCollidingVertically = (rectangle1.Height + rectangle2.Height) >= (max.Y - min.Y);

            return isCollidingHorizontally && isCollidingVertically;
        }
    }
}
