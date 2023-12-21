using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Rendering;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Stratums.HelperMethods;
using Stratums.Entities;

namespace Stratums.Properties
{
    public class Hitbox : Property
    {
        private List<Vector2> _vertices;

        private float _radius;
        private int _radiusIndex;
        private Vector2 _centerOfHitbox;

        private RenderData[] _renderData;
        private Color _color;

        private bool _debugMode;

        public List<Vector2> GetVertices() { return _vertices; }
        public float GetRadius() { return _radius; }
        public Vector2 GetCenter() { return _centerOfHitbox; }

        public Hitbox(List<Vector2> vertices, bool debugMode)
        {
            _vertices = vertices;

            _centerOfHitbox = MidOfHitbox();

            _debugMode = debugMode;

            // determines the radius length
            float testedDist;

            for (int i = 0; i < _vertices.Count; i++)
            {
                testedDist = (float)_centerOfHitbox.GetDistBetwCoords(_vertices[i]);

                if (_radius < testedDist)
                {
                    _radius = testedDist;
                    _radiusIndex = i;
                }
            }

            // only attempts render data if debug mode is on
            if (_debugMode)
            {
                _color = Color.Yellow;
                _renderData = GetHitboxRenderData();
            }
            else
            {
                _renderData = Array.Empty<RenderData>();
            }
        }

        public override IEnumerable<RenderData> GetRenderData()
        {
            return _renderData;
        }

        public override void OnUpdate(GameTime deltaTime, EntityBatch entityBatch, ref EntityData entityData)
        {
            if (_debugMode)
            {
                //Debug.Assert(_parentEntity.TestForCollision(entities));
                if (entityData.HostEntity.IsEntityColliding(entityBatch.GetComparableEntities(entityData.HostEntity)))
                {
                    _color = Color.Red;
                    _renderData = GetHitboxRenderData();
                }
                else if (_color != Color.Yellow)
                {
                    _color = Color.Yellow;
                    _renderData = GetHitboxRenderData();
                }
            }
        }

        /// <returns>An array of RenderData types used as the return value for GetRenderData()</returns>
        private RenderData[] GetHitboxRenderData()
        {
            RenderData[] fullhitbox = new RenderData[_vertices.Count + 1];

            for (int i = 0; i < _vertices.Count; i++)
            {
                var nextVertex = (i + 1) % _vertices.Count;
                var distBetwCoords = (int)_vertices[i].GetDistBetwCoords(_vertices[nextVertex]);

                //Gets the render data for each line in the polygon
                fullhitbox[i] = new RenderData()
                {
                    Texture = Debugger.debugTexture,

                    SourceRectangle = new Rectangle(0, 0, Debugger.debugTexture.Width, Debugger.debugTexture.Height),

                    DestinationRectangle = new Rectangle((int)_vertices[i].X, -(int)_vertices[i].Y, Debugger.debugTexture.Width, distBetwCoords),

                    Rotation = _vertices[i].GetAngleFromCoord(_vertices[nextVertex]) + (float)Math.PI / 2f,

                    Origin = Vector2.Zero,

                    Color = _color,
                };
            }

            //Gets the render data for the radius line
            fullhitbox[_vertices.Count] = new RenderData()
            {
                Texture = Debugger.debugTexture,

                SourceRectangle = new Rectangle(0, 0, Debugger.debugTexture.Width, Debugger.debugTexture.Height),

                DestinationRectangle = new Rectangle((int)_centerOfHitbox.X, -(int)_centerOfHitbox.Y, Debugger.debugTexture.Width, (int)_centerOfHitbox.GetDistBetwCoords(_vertices[_radiusIndex])),

                Rotation = _centerOfHitbox.GetAngleFromCoord(_vertices[_radiusIndex]) + (float)Math.PI / 2f,

                Origin = Vector2.Zero,

                Color = Color.Yellow
            };

            return fullhitbox;
        }


        /// <returns>The average Vector2 between the two furthest Vector2 types of a list.</returns>
        private Vector2 MidOfHitbox()
        {
            var greatestValue = Vector2.Zero;
            var smallestValue = Vector2.Zero;

            foreach (var v in _vertices)
            {
                if (v.X > greatestValue.X) { greatestValue.X = v.X; }
                if (v.Y > greatestValue.Y) { greatestValue.Y = v.Y; }

                if (v.X < smallestValue.X) { smallestValue.X = v.X; }
                if (v.Y < smallestValue.Y) { smallestValue.Y = v.Y; }
            }

            var x = (greatestValue.X + smallestValue.X) / 2;
            var y = (greatestValue.Y + smallestValue.Y) / 2;

            return new Vector2(x, y);
        }
    }
}
