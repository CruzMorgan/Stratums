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
using System.Runtime.CompilerServices;
using Stratums.Properties.Hitbox;
using Stratums.Entities.EntityPartitioning;

namespace Stratums.Entities
{
    public class Entity
    {
        private static int IDCounter = 000001;
        public readonly int ID;

        ContentManager _contentManager;
        SpriteBatch _spriteBatch;
        private readonly List<Property> _properties;

        private EntityData _entityData;

        public EntityData GetEntityData() { return _entityData; }

        public Entity(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            ID = IDCounter;
            IDCounter++;

            _contentManager = contentManager;
            _spriteBatch = spriteBatch;
            _properties = new List<Property>();

            _entityData.HostEntity = this;
            _entityData.SpriteEffects = SpriteEffects.None;
            _entityData.Position = Vector2.Zero;
            _entityData.Velocity = Vector2.Zero;
            _entityData.Hitbox = new CircleHitbox(Vector2.Zero, 10);
            _entityData.Color = Color.White;
            _entityData.IsColliding = false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType()) 
            {
                return false;
            }

            Entity other = (Entity)obj;

            return other.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public void Update(GameTime deltaTime, EntityBatch entityBatch)
        {
            _entityData.IsColliding = IsColliding(entityBatch.CalculateAdjecentEntities(this));

            Vector2 startPos = _entityData.Position;

            foreach (var property in _properties)
            {
                property.OnUpdate(deltaTime, entityBatch, ref _entityData);
            }

            _entityData.Hitbox.OnUpdate(_entityData.Position);
        }

        public void Draw(Vector2 camera)
        {
            foreach(var property in _properties)
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

        public bool IsColliding()
        {
            return _entityData.IsColliding;
        }
        public bool IsColliding(Entity other)
        {
            return this._entityData.Hitbox.IsHitboxColliding(other._entityData.Hitbox);
        }
        public bool IsColliding(List<Entity> others)
        {
            foreach (Entity entity in others)
            {
                if (this != entity && this._entityData.Hitbox.IsHitboxColliding(entity.GetEntityData().Hitbox))
                {
                    return true;
                }
            }

            return false;
        }

        public Entity OverridePosition(float x, float y)
        {
            _entityData.Position = new Vector2(x, y);
            return this;
        }
        public Entity OverridePosition(Vector2 vector)
        {
            _entityData.Position = vector;
            return this;
        }

        public Entity OverrideTranslatePosition(float x, float y)
        {
            _entityData.Position += new Vector2(x, y);
            return this;
        }
        public Entity InfluenceVelocity(float x, float y)
        {
            _entityData.Velocity += new Vector2(x, y);
            return this;
        }

        //EACH ADD{{PROPERTY}} METHOD BELOW
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
        public Entity AddGravity(float gravitationalAcceleration)
        {
            _properties.Add(new Gravity(gravitationalAcceleration));
            return this;
        }
        public Entity AddHitbox(Vector2 localPosition, int radius)
        {
            Hitbox hitbox = new CircleHitbox(localPosition, radius);

            _entityData.Hitbox = hitbox;
            return this;
        }
        public Entity AddHitbox(Vector2 localPosition, int width, int height)
        {
            Hitbox hitbox = new RectangleHitbox(localPosition, width, height);

            _entityData.Hitbox = hitbox;
            return this;
        }
        public Entity AddFriction(float decayRate)
        {
            _properties.Add(new Friction(decayRate));

            return this;
        }
        public Entity AddFriction()
        {
            _properties.Add(new Friction());

            return this;
        }
        public Entity AddAirResistance()
        {
            _properties.Add(new AirResistance());

            return this;
        }
        public Entity AddAirResistance(float decayRate)
        {
            _properties.Add(new AirResistance(decayRate));

            return this;
        }
        public Entity AddRandomMovement()
        {
            _properties.Add(new RandomMovement());

            return this;
        }
        public Entity AddDebugVisuals()
        {
            _properties.Add(new DebugVisuals());

            return this;
        }
    }
}
