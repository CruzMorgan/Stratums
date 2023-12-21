using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Stratums.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Entities
{
    public struct EntityData
    {
        public Entity HostEntity;

        public List<Hitbox> Hitboxes;

        public Vector2 Position;
        
        public Vector2 Velocity;
        
        public SpriteEffects SpriteEffects;
        
        public float Mass;
    }
}
