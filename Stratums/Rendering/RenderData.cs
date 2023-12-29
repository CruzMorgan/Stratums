using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Rendering
{
    public class RenderData
    {
        public Rectangle SourceRectangle { get; init; } = new Rectangle(0, 0, 100, 100);
        public Texture2D Texture { get; init; } = Debugger.EmptyTexture;
        public Rectangle DestinationRectangle { get; init; } = new Rectangle(0, 0, 100, 100);
        public float Rotation { get; init; } = 0f;
        public Vector2 Origin { get; init; } = Vector2.Zero;
        public Color Color { get; init; } = Color.White;

    }
    
}
