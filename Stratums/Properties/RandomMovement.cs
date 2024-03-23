using Microsoft.Xna.Framework;
using Stratums.Entities;
using Stratums.Entities.EntityPartitioning;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Properties
{
    public class RandomMovement : Property
    {
        Vector2 _randomVector;

        public RandomMovement() 
        {
            var rand = new Random();

            _randomVector = new Vector2((float)rand.NextDouble(), (float)rand.NextDouble()) * 5f;
        }


        public override IEnumerable<RenderData> GetRenderData()
        {
            return Enumerable.Empty<RenderData>();
        }

        public override void OnUpdate(GameTime deltaTime, ref EntityData entityData)
        {
            entityData.Velocity += _randomVector;
        }
    }
}
