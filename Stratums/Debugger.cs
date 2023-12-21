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
        public static Texture2D debugTexture;
        public static Texture2D emptyTexture;

        public static void LoadContent(ContentManager content)
        {
            debugTexture = content.Load<Texture2D>("WhitePixel");
            emptyTexture = content.Load<Texture2D>("Empty");
        }
    }
}
