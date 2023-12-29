using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Stratums
{
    public static class Debugger
    {
        public static Texture2D DebugTexture { get; private set; }
        public static Texture2D EmptyTexture { get; private set; }
        public static SpriteFont Font { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            DebugTexture = content.Load<Texture2D>("WhitePixel");
            EmptyTexture = content.Load<Texture2D>("Empty");
            Font = content.Load<SpriteFont>("Arial");
        }
    }
}
