using Microsoft.Xna.Framework;
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
    public class AirResistance : Property
    {
        private float _decayRate;

        public AirResistance() 
        {
            _decayRate = 0.997f;
        }

        public AirResistance(float decayRate) 
        {
            _decayRate = decayRate;
        }

        public override IEnumerable<RenderData> GetRenderData()
        {
            return Array.Empty<RenderData>();
        }

        public override void OnUpdate(GameTime deltaTime, ref EntityData entityData)
        {
            entityData.Velocity *= _decayRate;
        }
    }
}
