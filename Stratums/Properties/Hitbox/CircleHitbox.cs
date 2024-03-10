using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratums.HelperMethods;
using System.Diagnostics;

namespace Stratums.Properties.Hitbox
{
    public class CircleHitbox : Hitbox
    {
        public float Radius { get; }

        protected override Vector2 LocalPosition { get; }

        public override Vector2 GlobalPosition { get; protected set; }

        public CircleHitbox(Vector2 localPosition, float radius)
        {
            Radius = radius;
            LocalPosition = localPosition;
        }

        public override double CalculateDistanceToEdge(float angle)
        {
            return Radius;
        }

        public override bool IsHitboxColliding(CircleHitbox other)
        {
            return TestCollisionCases.CircleAndCircle(this, other);
        }

        public override bool IsHitboxColliding(RectangleHitbox other)
        {
            return TestCollisionCases.CircleAndRectangle(this, other);
        }
    }
}
