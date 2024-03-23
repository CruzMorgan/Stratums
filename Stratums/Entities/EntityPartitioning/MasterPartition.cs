using Microsoft.Xna.Framework;
using Stratums.HelperMethods;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratums.Entities.EntityPartitioning
{
    internal class MasterPartition
    {
        private Dictionary<Vector2, PartitionBranch> _partitions;

        private int _proximityCap;
        private int _depthCap;

        private Vector2 dimensions;

        public MasterPartition(int width, int height, int proximityCap, int depthCap) 
        {
            _partitions = new Dictionary<Vector2, PartitionBranch>();

            dimensions = new Vector2(width, height);

            _proximityCap = proximityCap;
            _depthCap = depthCap;
        }

        public void AddEntity(Entity entity)
        {
            IEnumerable<Vector2> partitionIndexes = CalculatePartitionIndexes(entity);

            if (partitionIndexes.Count() == 1)
            {
                Vector2 partitionIndex = partitionIndexes.First();

                if (!_partitions.ContainsKey(partitionIndex))
                {
                    CreatePartition(partitionIndex);
                }

                _partitions[partitionIndex].InsertEntity(entity);
            }
            else
            {
                foreach (Vector2 partitionIndex in partitionIndexes)
                {
                    if (!_partitions.ContainsKey(partitionIndex))
                    {
                        CreatePartition(partitionIndex);
                    }

                    _partitions[partitionIndex].InsertEntity(entity);
                }

            }
        }

        public bool RemoveEntity(Entity entity) 
        {
            IEnumerable<Vector2> partitionIndexes = CalculatePartitionIndexes(entity);

            bool isEntityFound = false;

            if (partitionIndexes.Count() == 1)
            {
                Vector2 partitionIndex = partitionIndexes.First();

                isEntityFound = _partitions[partitionIndex].RemoveEntity(entity);

                if (_partitions[partitionIndex].GetAllEntities().Count == 0)
                {
                    _partitions.Remove(partitionIndex);
                }

                return isEntityFound;
            }

            foreach(Vector2 partitionIndex in partitionIndexes)
            {
                if (!_partitions[partitionIndex].RemoveEntity(entity))
                {
                    isEntityFound = true;
                }

                if (_partitions[partitionIndex].GetAllEntities().Count == 0)
                {
                    _partitions.Remove(partitionIndex);
                }
            }

            return isEntityFound;
        }

        public List<Entity> GetEntitiesInPartitionBranch(Entity entity)
        {
            IEnumerable<Vector2> partitionIndexes = CalculatePartitionIndexes(entity);

            if (partitionIndexes.Count() == 1)
            {
                return _partitions[partitionIndexes.First()].GetEntitiesInSamePartition(entity);
            }

            List<Entity> entities = new List<Entity>();

            foreach (Vector2 partitionIndex in partitionIndexes)
            foreach (Entity subEntity in _partitions[partitionIndex].GetEntitiesInSamePartition(entity))
            {
                entities.Add(subEntity);
            }

            return entities;
        }

        private void CreatePartition(Vector2 partitionIndex)
        {
            Vector2 minRange = partitionIndex * dimensions;
            Vector2 maxRange = partitionIndex * dimensions + dimensions;

            PartitionBranch partition = new PartitionBranch(minRange, maxRange, 0, _proximityCap, _depthCap);

            _partitions.Add(partitionIndex, partition);
        }

        private IEnumerable<Vector2> CalculatePartitionIndexes(Entity entity)
        {
            Vector2 min = CalculatePartitionIndex(entity.GetEntityData().Hitbox.Range.Item1 + entity.GetEntityData().Position);
            Vector2 max = CalculatePartitionIndex(entity.GetEntityData().Hitbox.Range.Item2 + entity.GetEntityData().Position);

            if (max - min != Vector2.Zero)
            {
                int width = (int)(max.X - min.X + 1);
                int height = (int)(max.Y - min.Y + 1);

                for (int i = 0; i < width * height; i++)
                {
                    yield return new Vector2(i % width + min.X, i / height + min.Y);
                }
            }
            else
            {
                yield return min;
            }
        }

        private Vector2 CalculatePartitionIndex(Vector2 pos)
        {
            return (pos / dimensions).Rounded();
        }

    }
}
