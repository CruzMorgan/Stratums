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
        private Dictionary<Point, PartitionBranch> _partitions;

        private int _proximityCap;
        private int _depthCap;

        private Point _dimensions;

        public MasterPartition(int width, int height, int proximityCap, int depthCap) 
        {
            _partitions = new Dictionary<Point, PartitionBranch>();

            _dimensions = new Point(width, height);

            _proximityCap = proximityCap;
            _depthCap = depthCap;
        }

        public void AddEntity(Entity entity)
        {
            IEnumerable<Point> partitionIndexes = CalculatePartitionIndexes(entity);

            foreach (Point partitionIndex in partitionIndexes)
            {
                if (!_partitions.ContainsKey(partitionIndex))
                {
                    CreatePartition(partitionIndex);
                }

                _partitions[partitionIndex].InsertEntity(entity);
            }

        }

        public bool RemoveEntity(Entity entity) 
        {
            IEnumerable<Point> partitionIndexes = CalculatePartitionIndexes(entity);

            bool isEntityFound = false;

            foreach(Point partitionIndex in partitionIndexes)
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
            IEnumerable<Point> partitionIndexes = CalculatePartitionIndexes(entity);

            List<Entity> entities = new List<Entity>();

            foreach (Point partitionIndex in partitionIndexes)
            foreach (Entity subEntity in _partitions[partitionIndex].GetEntitiesInSamePartition(entity))
            {
                entities.Add(subEntity);
            }

            return entities;
        }

        private void CreatePartition(Point partitionIndex)
        {
            Vector2 minRange = (partitionIndex * _dimensions).ToVector2();
            Vector2 maxRange = (partitionIndex * _dimensions + _dimensions).ToVector2();

            PartitionBranch partition = new PartitionBranch(minRange, maxRange, 0, _proximityCap, _depthCap);

            _partitions.Add(partitionIndex, partition);
        }

        private IEnumerable<Point> CalculatePartitionIndexes(Entity entity)
        {
            Point min = CalculatePartitionIndex(entity.GetEntityData().Hitbox.Range.Item1.ToPoint() + entity.GetEntityData().Position.ToPoint());
            Point max = CalculatePartitionIndex(entity.GetEntityData().Hitbox.Range.Item2.ToPoint() + entity.GetEntityData().Position.ToPoint());

            if (max == min)
            {
                yield return min;
            }

            for (int x = min.X; x <= max.X; x++)
            for (int y = min.Y; y <= max.Y; y++)
            {
                yield return new Point(x, y);
            }
        }

        private Point CalculatePartitionIndex(Point pos)
        {
            return pos / _dimensions;
        }

    }
}
