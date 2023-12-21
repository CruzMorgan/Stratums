using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Entities;
using Stratums.HelperMethods;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Properties
{
    public class Collider : Property
    {
        private List<Entity> _entities;

        public override IEnumerable<RenderData> GetRenderData()
        {
            return Array.Empty<RenderData>();
        }

        public override void OnUpdate(GameTime deltaTime, EntityBatch entityBatch, ref EntityData entityData)
        {
            _entities = entityBatch.GetComparableEntities(entityData.HostEntity);

            AdjustPosition(ref entityData);
        }

        private void AdjustPosition(ref EntityData entityData)
        {

            if (entityData.HostEntity.IsEntityColliding(_entities, out var intersectionPoint))
            {
                Vector2 moveTo = MoveTo(entityData.Position, intersectionPoint.Value);

                entityData.Position += moveTo;
                entityData.Velocity = Vector2.Zero;

                AdjustPosition(ref entityData);
            }
        }

        private Vector2 MoveTo(Vector2 current, Vector2 other)
        {
            return (current - other).Normalized();
            
        }
    }
}
