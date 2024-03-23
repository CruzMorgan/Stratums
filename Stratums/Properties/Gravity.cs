using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Entities;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratums.Entities.EntityPartitioning;

namespace Stratums.Properties
{
    public class Gravity : Property
    {
        private const float PixelsPerMeter = 100f;

        private float _gravitationalAcceleration;

        public Gravity()
        {
            _gravitationalAcceleration = 9.8f;
        }

        public Gravity(float gravitationalAcceleration)
        {
            _gravitationalAcceleration = gravitationalAcceleration;
        }

        public override IEnumerable<RenderData> GetRenderData()
        {
            return Array.Empty<RenderData>();
        }

        public override void OnUpdate(GameTime deltaTime, EntityBatch entityBatch, ref EntityData entityData)
        {
            entityData.Velocity.Y -= _gravitationalAcceleration * (float)deltaTime.ElapsedGameTime.TotalSeconds * PixelsPerMeter;
        }
    }
}
