using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Stratums.Rendering;

namespace Stratums.Entities.EntityPartitioning
{
    public class EntityBatch
    {
        public List<Entity> Entities { get; }
        private MasterPartition _masterPartition;

        public EntityBatch(int partitionWidth, int partitionHeight, int proximityCap, int depthCap)
        {
            Entities = new List<Entity>();
            _masterPartition = new MasterPartition(partitionWidth, partitionHeight, proximityCap, depthCap);
        }

        public EntityBatch(int partitionWidth, int partitionHeight)
        {
            Entities = new List<Entity>();
            _masterPartition = new MasterPartition(partitionWidth, partitionHeight, 10, 5);
        }

        public EntityBatch()
        {
            Entities = new List<Entity>();
            _masterPartition = new MasterPartition(10000, 10000, 10, 5);
        }

        public List<Entity> CalculateAdjecentEntities(Entity entity)
        {
            return _masterPartition.GetEntitiesInPartitionBranch(entity);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Entity entity in Entities)
            {
                _masterPartition.RemoveEntity(entity);
                entity.Update(gameTime, entity.IsColliding(CalculateAdjecentEntities(entity)));
                _masterPartition.AddEntity(entity);
            }
        }

        public void Draw(Vector2 camera)
        {
            foreach (Entity entity in Entities)
            {
                entity.Draw(camera);
            }
        }

        public void DrawPartitions(SpriteBatch spriteBatch)
        {


            spriteBatch.Draw
            (
                data.Texture,
                data.SourceRectangle,
                data.DestinationRectangle,
                data.Color,
                data.Rotation,
                data.Origin,
                SpriteEffects.None,
                0f
            );
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            _masterPartition.AddEntity(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
            _masterPartition.RemoveEntity(entity);
        }
    }
}
