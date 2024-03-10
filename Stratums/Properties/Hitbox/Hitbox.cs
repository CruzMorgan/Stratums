using Microsoft.Xna.Framework;
using Stratums.Entities;
using Stratums.HelperMethods;
using Stratums.Properties;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Properties.Hitbox
{
    public abstract class Hitbox : Property
    {
        protected abstract Vector2 LocalPosition { get; }
        public abstract Vector2 GlobalPosition { get; protected set; } 

        //NOTE: find if there is a way to not need the switch statement? Auto converting to correct hitboxes??
        public bool IsHitboxColliding(Entity thisEntity, List<Entity> others)
        {
            foreach(Entity entity in others)
            {
                if (thisEntity != entity && IsHitboxColliding(entity._entityData.Hitboxes))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsHitboxColliding(List<Hitbox> others)
        {
            foreach (Hitbox other in others)
            {
                switch (other)
                {
                    case CircleHitbox:
                        if (IsHitboxColliding((CircleHitbox)other))
                        {
                            return true;
                        }
                        break;

                    case RectangleHitbox:
                        if (IsHitboxColliding((RectangleHitbox)other))
                        {
                            return true;
                        }
                        break;

                    default:
                        Debug.Assert(false, "Invalid or unimplemented type used for hitbox");
                        break;
                }
            }
            return false;
        }

        public bool IsHitboxColliding(Hitbox other)
        {
            switch (other)
            {
                case CircleHitbox:
                    if (IsHitboxColliding((CircleHitbox)other))
                    {
                        return true;
                    }
                    break;

                case RectangleHitbox:
                    if (IsHitboxColliding((RectangleHitbox)other))
                    {
                        return true;
                    }
                    break;

                default:
                    Debug.Assert(false, "Invalid or unimplemented type used for hitbox");
                    break;
            }
            return false;
        }

        public abstract bool IsHitboxColliding(CircleHitbox other);
        public abstract bool IsHitboxColliding(RectangleHitbox other);

        public abstract double CalculateDistanceToEdge(float angle);

        public override void OnUpdate(GameTime deltaTime, EntityBatch entityBatch, ref EntityData entityData)
        {
            GlobalPosition = entityData.Position + LocalPosition;
        }
        public override IEnumerable<RenderData> GetRenderData()
        {
            return Array.Empty<RenderData>();
        }

    }
}
