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
    public class DebugVisuals : Property
    {
        public override IEnumerable<RenderData> GetRenderData()
        {
            return Array.Empty<RenderData>();
        }

        public override void OnUpdate(GameTime deltaTime, ref EntityData entityData)
        {
            if (entityData.HostEntity.IsColliding())
                entityData.Color = Color.LightGreen;
            else
                entityData.Color = Color.White;
        }
    }
}
