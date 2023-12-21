using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Entities;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratums.HelperMethods;

namespace Stratums.Properties
{
    public class Gravity : Property
    {
        private const float GravitationalAcceleration = 9.8f;
        private const float PixelsPerMeter = 100f;

        public override IEnumerable<RenderData> GetRenderData()
        {
            return Array.Empty<RenderData>();
        }

        public override void OnUpdate(GameTime deltaTime, EntityBatch entityBatch, ref EntityData entityData)
        {
            entityData.Velocity.Y -= GravitationalAcceleration * (float)deltaTime.ElapsedGameTime.TotalSeconds * PixelsPerMeter;
        }
    }
}
