using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Entities;
using Stratums.HelperMethods;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Properties
{
    public class Friction : Property
    {
        float _decayRate;

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
            var insertVariableNameHere = (entityData.Velocity * -_decayRate) - entityData.Velocity;

            entityData.Velocity += insertVariableNameHere * (float)deltaTime.ElapsedGameTime.TotalSeconds;
        }
    }
}