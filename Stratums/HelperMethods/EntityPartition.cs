using Microsoft.Xna.Framework;
using Stratums.Entities;
using Stratums.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.HelperMethods
{
    public static class EntityPartition
    {
        /// <param name="entity"></param>
        /// <returns>List of Indexes for Partitions</returns>
        public static List<Vector2> CalculatePartitionIndexes(this Entity entity, int partitionSize)
        {
            var entityPartitionIndexes = new List<Vector2>();

            foreach (var hitbox in entity.GetHitboxes())
            {
                var hitboxPartitionIndexes = hitbox.CalculatePartitionIndexes(partitionSize, entity.GetPosition());
                //Debug.Assert(hitboxPartitionIndexes.Count != 1);

                float minX = 0;
                float minY = 0;
                float maxX = 0;
                float maxY = 0;

                for (int i = 0; i < hitboxPartitionIndexes.Count; i++)
                {
                    if (hitboxPartitionIndexes[i].X < minX) { minX = hitboxPartitionIndexes[i].X; }
                    if (hitboxPartitionIndexes[i].X > maxX) { maxX = hitboxPartitionIndexes[i].X; }
                    if (hitboxPartitionIndexes[i].Y < minY) { minY = hitboxPartitionIndexes[i].Y; }
                    if (hitboxPartitionIndexes[i].Y > maxY) { maxY = hitboxPartitionIndexes[i].Y; }
                }

                for (int x = (int)minX; x <= (int)maxX; x++)
                    for (int y = (int)minY; y <= (int)maxY; y++)
                    {
                        entityPartitionIndexes.Add(new Vector2(x, y));
                    }
            }
            return entityPartitionIndexes;
        }

        public static List<Vector2> CalculatePartitionIndexes(this Hitbox hitbox, int partitionSize, Vector2 position)
        {
            var partitionIndexes = new List<Vector2>();

            foreach (var coordinate in hitbox.GetVertices())
            {
                var partitionIndex = (coordinate + position).CalculatePartitionIndexes(partitionSize);
                partitionIndexes.AddWithoutRepeats(partitionIndex);
            }

            Debug.Assert(partitionIndexes.Count != 0);
            return partitionIndexes;
        }

        public static Vector2 CalculatePartitionIndexes(this Vector2 coordinate, int partitionSize)
        {
            return (coordinate / partitionSize).Rounded();
        }

    }
}
