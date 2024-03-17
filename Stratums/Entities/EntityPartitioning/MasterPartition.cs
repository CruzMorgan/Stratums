using Microsoft.Xna.Framework;
using Stratums.HelperMethods;
using Stratums.Rendering;
using System;
using System.Collections.Generic;
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
            Vector2 partitionIndex = CalculatePartitionIndex(entity);

            if (!_partitions.ContainsKey(partitionIndex))
            {
                CreatePartition(partitionIndex);
            }

            _partitions[partitionIndex].InsertEntity(entity);
        }

        public bool RemoveEntity(Entity entity) 
        {
            Vector2 partitionIndex = CalculatePartitionIndex(entity);

            bool isEntityFound = _partitions[partitionIndex].RemoveEntity(entity);

            if (_partitions[partitionIndex].GetAllEntities().Count == 0)
            {
                _partitions.Remove(partitionIndex);
            }

            return isEntityFound;
        }

        public List<Entity> GetEntitiesInPartitionBranch(Entity entity)
        {
            Vector2 partitionIndex = CalculatePartitionIndex(entity);
            
            return _partitions[partitionIndex].GetEntitiesInSamePartition(entity);
        }

        private void CreatePartition(Vector2 partitionIndex)
        {
            Vector2 minRange = partitionIndex * dimensions;
            Vector2 maxRange = partitionIndex * dimensions + dimensions;

            PartitionBranch partition = new PartitionBranch(minRange, maxRange, 0, _proximityCap, _depthCap);

            _partitions.Add(partitionIndex, partition);
        }

        private Vector2 CalculatePartitionIndex(Entity entity)
        {
            return (entity.GetEntityData().Position / dimensions).Rounded();
        }

        private Vector2 CalculatePartitionIndex(Vector2 pos)
        {
            return (pos / dimensions).Rounded();
        }

    }
}
