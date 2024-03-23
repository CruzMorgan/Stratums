using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Entities;
using Stratums.Entities.EntityPartitioning;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Stratums.Properties
{
    internal class Inertia : Property
    {
        public override IEnumerable<RenderData> GetRenderData()
        {
            return Array.Empty<RenderData>();
        }

        public override void OnUpdate(GameTime deltaTime, ref EntityData entityData)
        {
            entityData.Position += new Vector2(entityData.Velocity.X, entityData.Velocity.Y) * (float)deltaTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
