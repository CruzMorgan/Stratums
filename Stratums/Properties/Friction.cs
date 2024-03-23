using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Entities;
using Stratums.Entities.EntityPartitioning;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Properties
{
    public class Friction : Property
    {
        float _decayRate;

        public Friction()
        {
            _decayRate = 0.9f;
        }

        public Friction(float decayRate) 
        {
            _decayRate = decayRate;
        }

        public override IEnumerable<RenderData> GetRenderData()
        {
            return Array.Empty<RenderData>();
        }
        
        public override void OnUpdate(GameTime deltaTime, EntityBatch entityBatch, ref EntityData entityData)
        {
            if (entityData.HostEntity.IsColliding())
            {
                entityData.Velocity *= _decayRate;
            }
        }
    }
}