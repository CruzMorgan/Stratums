using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratums.Properties.Hitbox;

namespace Stratums.Entities
{
    public struct EntityData
    {
        public Entity HostEntity;

        public Hitbox Hitbox;

        public Vector2 Position;
        
        public Vector2 Velocity;
        
        public SpriteEffects SpriteEffects;
        
        public float Mass;

        public Color Color;

        public bool IsColliding;
    }
}
