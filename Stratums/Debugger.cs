using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Stratums.HelperMethods;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
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

        public static RenderData DrawLine(Vector2 point1, Vector2 point2)
        {
            var distBetwCoords = (int)point1.Dist(point2);

            RenderData data = new RenderData()
            {
                Texture = Debugger.DebugTexture,

                SourceRectangle = new Rectangle(0, 0, Debugger.DebugTexture.Width, Debugger.DebugTexture.Height),

                DestinationRectangle = new Rectangle((int)point1.X, -(int)point1.Y, Debugger.DebugTexture.Width, distBetwCoords),

                Color = Color.White,

                Rotation = (float)point1.ToAngle(point2) + (float)Math.PI / 2f,

                Origin = Vector2.Zero,
            };

            return data;
        }

        public static IEnumerable<RenderData> DrawRectangle(Vector2 min, Vector2 max)
        {
            Vector2[] corners = new Vector2[4];

            corners[0] = new Vector2(min.X, max.Y);
            corners[1] = max;
            corners[2] = min;
            corners[3] = new Vector2(max.X, min.Y);

            for (int i = 0; i < 4; i++)
            {
                yield return Debugger.DrawLine(corners[i], corners[(i + 1) % 4]);
            }
        }
    }
}
