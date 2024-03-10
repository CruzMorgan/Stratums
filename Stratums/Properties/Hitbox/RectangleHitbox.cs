using Microsoft.Xna.Framework;
using Stratums.HelperMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Properties.Hitbox
{
    public class RectangleHitbox : Hitbox
    {
        public override Vector2 GlobalPosition { get; protected set; }
        protected override Vector2 LocalPosition { get; }
        public float Width { get; }
        public float Height { get; }

        private float _lateralRadius;
        private float _longitudinalRadius;

        private float[] _sides;

        public RectangleHitbox(Vector2 localPosition, float width, float height)
        {
            LocalPosition = localPosition;
            Width = width;
            Height = height;

            _lateralRadius = Width / 2f;
            _longitudinalRadius = Height / 2f;

            _sides = new float[] 
            { 
                localPosition.X - _lateralRadius, 
                localPosition.Y + _longitudinalRadius, 
                localPosition.X + _lateralRadius, 
                localPosition.Y - _longitudinalRadius
            };
        }

        /// <summary>
        /// Order: left, top, right, bottom
        /// </summary>
        /// <returns></returns>
        public float[] getGlobalPositionSides()
        {
            return new float[]
            {
                _sides[0] + GlobalPosition.X,
                _sides[1] + GlobalPosition.Y,
                _sides[2] + GlobalPosition.X,
                _sides[3] + GlobalPosition.Y
            };
        }

        public override double CalculateDistanceToEdge(float angle)
        {
            angle %= 360f;

            float lateralRangeAngle = 2f * ((float)Math.Atan(_longitudinalRadius / _lateralRadius) * (180f / (float)Math.PI));

            float[] calculateDistance =
            {
                Math.Abs(_lateralRadius/ (float) Math.Cos(angle)),
                Math.Abs(_longitudinalRadius / (float) Math.Cos(angle + 90f))
            };
            float calculateDistanceIndex = 1f / lateralRangeAngle * (angle + 0.5f * lateralRangeAngle) % (180f / lateralRangeAngle) - 1f;
            calculateDistanceIndex = calculateDistanceIndex == 0f ? 0f : 0.5f * (calculateDistanceIndex / Math.Abs(calculateDistanceIndex)) + 0.5f;

            return calculateDistance[(int)calculateDistanceIndex];
        }

        public override bool IsHitboxColliding(CircleHitbox other)
        {
            return TestCollisionCases.CircleAndRectangle(other, this);
        }

        public override bool IsHitboxColliding(RectangleHitbox other)
        {
            return TestCollisionCases.RectangleAndRectangle(this, other);
        }

        public bool IsPointWithinHitbox(Vector2 point)
        {
            return 
                point.X <= GlobalPosition.X + _lateralRadius &&
                point.X >= GlobalPosition.X - _lateralRadius &&
                point.Y <= GlobalPosition.Y + _longitudinalRadius &&
                point.Y >= GlobalPosition.Y - _longitudinalRadius;
        }

    }
}
