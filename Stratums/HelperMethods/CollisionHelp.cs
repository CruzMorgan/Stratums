using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Entities;
using Stratums.Properties;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.HelperMethods
{
    public static class CollisionHelp
    {
        /// <summary>
        /// Uses colliding entities and takes into account their mass, velocity, and intersection points to determine the position that the entity needs to be in to satisfy the collision occuring.
        /// </summary>
        /// <param name="collidingEntities">Entities that the current entity is currently colliding with.</param>
        /// <returns>A Vector that represents the position change needed to satisfy the collisions.</returns>
        public static Vector2 FindNewPosition(List<Entity> collidingEntities)
        {
            throw new NotImplementedException();
        }
    }
}
