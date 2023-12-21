using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Rendering
{
    public static class Coordinate
    {
        //Figures out where the global coordinate is on the screen
        public static Vector2 GetRelativeCoordinate(this Vector2 globalCoordinate, Vector2 cameraCoordinate)
        {
            return globalCoordinate - cameraCoordinate;
        }
    }
}
