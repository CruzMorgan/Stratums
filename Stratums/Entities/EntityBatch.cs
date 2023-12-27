using Microsoft.Xna.Framework;
using Stratums.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratums.HelperMethods;
using System.Dynamic;
using System.Diagnostics;

namespace Stratums.HelperMethods
{
    public class EntityBatch
    {
        public List<Entity> Entities { get; }
        public List<Entity> CollidableEntities { get; }

        public EntityBatch()
        {
            Entities = new List<Entity>();
            CollidableEntities = new List<Entity>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Entity entity in Entities)
            {
                entity.Update(gameTime, this);
            }

            foreach (Entity entity in CollidableEntities)
            {
                entity.PositionUpdate(gameTime, this);
            }
        }

        public void Draw(Vector2 camera)
        {
            foreach (Entity entity in Entities)
            {
                entity.Draw(camera);
            }
        }

        /// <summary> Declare "AddEntity()" in LoadContent() </summary>
        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);

            if (entity.IsCollidable)
            {
                CollidableEntities.Add(entity);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
            CollidableEntities.Remove(entity);
        }
    }
}
