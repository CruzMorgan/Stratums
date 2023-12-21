using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums
{
    internal static class ImageReader
    {
        public static Color GetPixelRGBA(this Texture2D texture, Vector2 specifiedPixel)
        {
            Color[] texturePixelColors = new Color[texture.Width * texture.Height];
            texture.GetData(texturePixelColors);

            return texturePixelColors[(int)(((specifiedPixel.Y - 1) * texture.Width) + (specifiedPixel.X - 1) )];
        }
    }
}
