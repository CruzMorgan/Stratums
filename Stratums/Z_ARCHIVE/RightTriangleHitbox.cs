using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace Stratums.Properties.Hitbox
{
    public class RightTriangleHitbox : Hitbox
    {
        public override Vector2 GlobalPosition { get; protected set; }
        protected override Vector2 LocalPosition { get; }

        public float Width { get; } 
        public float Height { get; }
        public float Hypotenuse { get; }

        private float _centerToHorizontalLineDistance;
        private Tuple<Vector2, Vector2, Vector2> _vertices;

        /// <summary>
        /// "hypotenuseSide" represents which side of the right triangle is the hypotenuse: 0 = top-right; 1 = top-left; 2 = bottom-left; 3 = bottom-right.
        /// </summary>
        /// <param name="localPosition"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hypotenuseSide">Represents which side of the right triangle is the hypotenuse: 0 = top-right; 1 = top-left; 2 = bottom-left; 3 = bottom-right;</param>
        public RightTriangleHitbox(Vector2 localPosition, float width, float height, byte hypotenuseSide)
        {
            LocalPosition = localPosition;

            Width = width;
            Height = height;
            Hypotenuse = (float)Math.Sqrt(width * width + height * height);

            var topLeft = new Vector2(LocalPosition.X - width / 3f, LocalPosition.Y + (height / 3f) * 2f);
            var topRight = new Vector2(LocalPosition.X + (width / 3f) * 2f, LocalPosition.Y + (height / 3f) * 2f);
            var bottomRight = new Vector2(LocalPosition.X + (width / 3f) * 2f, LocalPosition.Y - height / 3f);
            var bottomLeft = new Vector2(LocalPosition.X - width / 3f, LocalPosition.Y - height / 3f);

            switch (hypotenuseSide)
            {
                case 0:
                    _vertices = new Tuple<Vector2, Vector2, Vector2>(topLeft, bottomRight, bottomLeft);
                    break;

                case 1:
                    _vertices = new Tuple<Vector2, Vector2, Vector2>(bottomRight, topRight, bottomLeft);
                    break;

                case 2:
                    _vertices = new Tuple<Vector2, Vector2, Vector2>(topLeft, bottomRight, topRight);
                    break;

                case 3:
                    _vertices = new Tuple<Vector2, Vector2, Vector2>(bottomLeft, topRight, topLeft);
                    break;

                default:
                    Debug.Assert(false, "hypotenuseSide should only be 0, 1, 2, or 3");
                    break;
            }

        }

        public override double CalculateDistanceToEdge(float angle)
        {
            throw new NotImplementedException();
        }

        public override bool IsHitboxColliding(CircleHitbox other)
        {
            return TestCollisionCases.CircleAndRightTriangle(other, this);
        }

        public override bool IsHitboxColliding(RectangleHitbox other)
        {
            return TestCollisionCases.RectangleAndRightTriangle(other, this);
        }

        public override bool IsHitboxColliding(RightTriangleHitbox other)
        {
            return TestCollisionCases.RightTriangleAndRightTriangle(this, other);
        }
    }
}
*/