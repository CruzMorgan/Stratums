using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Properties;
using Stratums.Rendering;
using Stratums;
using Stratums.HelperMethods;
using System.Runtime.CompilerServices;

namespace Stratums.Entities
{
    public class Entity
    {
        ContentManager _contentManager;
        SpriteBatch _spriteBatch;
        private readonly List<Property> _properties;

        private EntityData _entityData;

        public List<Hitbox> GetHitboxes() { return _entityData.Hitboxes; }
        public Vector2 GetPosition() { return _entityData.Position; }

        public Entity(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;
            _properties = new List<Property>();

            _entityData.HostEntity = this;
            _entityData.SpriteEffects = SpriteEffects.None;
            _entityData.Position = new Vector2(0, 0);
            _entityData.Velocity = Vector2.Zero;
            _entityData.Hitboxes = new List<Hitbox>();
        }

        public void Update(GameTime deltaTime, EntityBatch entityBatch)
        {
            foreach (var property in _properties)
            {
                property.OnUpdate(deltaTime, entityBatch, ref _entityData);
            }
        }

        public void Draw(Vector2 camera)
        {
            foreach(var property in _properties)
            {
                foreach(var renderData in property.GetRenderData())
                {
                    var renderPosition = new Vector2
                    (
                        (float)Math.Round(_entityData.Position.X + renderData.DestinationRectangle.X), 
                        (float)Math.Round(_entityData.Position.Y + -renderData.DestinationRectangle.Y)
                    ).GetRelativeCoordinate(new Vector2(camera.X, -camera.Y));

                    _spriteBatch.Draw
                    (
                        renderData.Texture,
                        new Rectangle((int)renderPosition.X, -(int)renderPosition.Y, renderData.DestinationRectangle.Width, renderData.DestinationRectangle.Height),
                        renderData.SourceRectangle,
                        renderData.Color,
                        renderData.Rotation,
                        Vector2.Round(renderData.Origin),
                        _entityData.SpriteEffects,
                        0
                    );

                }
            }

        }

        public Entity OverridePosition(float x, float y)
        {
            _entityData.Position = new Vector2(x, y);
            return this;
        }
        public Entity OverrideVelocity(float x, float y)
        {
            _entityData.Position += new Vector2(x, y);
            return this;
        }

        //EACH ADDPROPERTY METHOD BELOW

        public Entity AddInertia()
        {
            _properties.Add(new Inertia());
            return this;
        }
        public Entity AddAnimation(string textureName)
        {
            _properties.Add(new Animation(_contentManager, textureName));
            return this;
        }
        public Entity AddGravity()
        {
            _properties.Add(new Gravity());
            return this;
        }
        public Entity AddHitbox(List<Vector2> vertices, bool debugMode)
        {
            Hitbox hitbox = new Hitbox(vertices, debugMode);

            _entityData.Hitboxes.Add(hitbox);
            _properties.Add(hitbox);
            return this;
        }
        public Entity AddHitbox(int width, int height, bool debugMode)
        {
            width /= 2;
            height /= 2;
            List<Vector2> vertices = new List<Vector2> { new Vector2(width, height), new Vector2(-width, height), new Vector2(-width, -height), new Vector2(width, -height) };

            AddHitbox(vertices, debugMode);

            return this;
        }
        public Entity AddCollider()
        {
            _properties.Add(new Collider());

            return this;
        }
        public Entity AddFriction(float decayRate)
        {
            _properties.Add(new Friction(decayRate));

            return this;
        }
    }
}
