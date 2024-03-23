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
        public override Tuple<Vector2, Vector2> Range 
        { 
            get; 
            init; 
        }

        public CircleHitbox(Vector2 localPosition, float radius)
        {
            Range = new Tuple<Vector2, Vector2>(_localPosition.AddToEach(-Radius), _localPosition.AddToEach(Radius));

            Radius = radius;
            _localPosition = localPosition;
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
