using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Entities;
using Stratums.Entities.EntityPartitioning;
using Stratums.Rendering;

namespace Stratums.Properties
{
    public abstract class Property
    {
        public abstract void OnUpdate(GameTime deltaTime, EntityBatch entityBatch, ref EntityData entityData);
        public abstract IEnumerable<RenderData> GetRenderData();

    }
}
