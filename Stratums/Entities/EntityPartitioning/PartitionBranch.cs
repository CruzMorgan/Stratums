using Stratums.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Stratums.Entities.EntityPartitioning
{
    internal class PartitionBranch
    {
        private static int _proximityCap = 10;
        private static int _depthCap = 5;

        private readonly int _depth;

        private readonly Vector2 _minRange;
        private readonly Vector2 _maxRange;
        private readonly Vector2 _center;

        private PartitionBranch[] _subBranches;
        private List<Entity> _entities;

        public PartitionBranch(Vector2 minRange, Vector2 maxRange, int depth, int proximityCap, int depthCap)
        {
            _depth = depth;

            _minRange = minRange;
            _maxRange = maxRange;
            _center = (_maxRange - _minRange) / 2 + _minRange;

            _subBranches = null;
            _entities = new List<Entity>();

            _proximityCap = proximityCap;
            _depthCap = depthCap;
        }

        public PartitionBranch(Vector2 minRange, Vector2 maxRange, int depth)
        {
            _depth = depth;

            _minRange = minRange;
            _maxRange = maxRange;
            _center = (_maxRange - _minRange) / 2 + _minRange;

            _subBranches = null;
            _entities = new List<Entity>();

        }

        public void InsertEntity(Entity entity)
        {
            if (_subBranches != null)
            {
                int subBranchIndex = CalculateSubBranchIndex(entity);

                _subBranches[subBranchIndex].InsertEntity(entity);

            }
            else if (_entities.Count > _proximityCap && _depth < _depthCap)
            {
                DividePartition();
            }
            else
            {
                _entities.Add(entity);
            }
        }

        /// <summary>
        /// Removes entity ID from the entity partition and its sub partitions.
        /// </summary>
        /// <param name="entity">Desired entity ID to remove.</param>
        /// <returns>False if no matching entity ID is found; true otherwise.</returns>
        public bool RemoveEntity(Entity entity)
        {
            bool isEntityFound = _entities.Remove(entity);

            if (!isEntityFound && _subBranches != null)
            {
                int subBranchIndex = CalculateSubBranchIndex(entity);

                isEntityFound = _subBranches[subBranchIndex].RemoveEntity(entity);
            }

            if (_subBranches != null && _entities.Count <= _proximityCap)
            {
                DissolveSubPartitions();
            }

            return isEntityFound;
        }

        public List<Entity> GetAllEntities()
        {
            List<Entity> allEntities = _entities;

            if (_subBranches != null)
            {
                foreach (PartitionBranch subBranch in _subBranches)
                foreach (Entity subEntity in subBranch.GetAllEntities())
                {
                    allEntities.Add(subEntity);
                }
            }

            return allEntities;
        }

        public List<Entity> GetEntitiesInSamePartition(Entity entity)
        {
            if (_subBranches != null)
            {
                int subBranchIndex = CalculateSubBranchIndex(entity);

                return _subBranches[subBranchIndex].GetEntitiesInSamePartition(entity);
            }

            return _entities;
        }

        private void DividePartition()
        {
            //Ordering: Quadrant I, II, III, IV
            _subBranches = new PartitionBranch[]
            {
                new PartitionBranch(_center, _maxRange, _depth + 1),
                new PartitionBranch(new Vector2(_minRange.X, _center.Y), new Vector2(_center.X, _maxRange.Y), _depth + 1),
                new PartitionBranch(_minRange, _center, _depth + 1),
                new PartitionBranch(new Vector2(_center.X, _minRange.Y), new Vector2(_maxRange.X, _center.Y), _depth + 1)
            };

            foreach (Entity subEntity in _entities)
            {
                int subBranchIndex = CalculateSubBranchIndex(subEntity);

                _subBranches[subBranchIndex].InsertEntity(subEntity);
            }

            _entities.Clear();
        }

        private void DissolveSubPartitions()
        {
            _entities = GetAllEntities();
            _subBranches = null;
        }

        private int CalculateSubBranchIndex(Entity entity)
        {
            Vector2 entityPosition = entity.GetEntityData().Position;

            int subBranchIndex = entityPosition.QuadrantFromOrigin(_center) - 1;

            return subBranchIndex;

        }

    }
}

