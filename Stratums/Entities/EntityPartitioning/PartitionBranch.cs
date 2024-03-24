using Stratums.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Globalization;

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
                int? subBranchIndex = CalculateSubBranchIndex(entity);

                if (subBranchIndex != null)
                {
                    _subBranches[subBranchIndex.Value].InsertEntity(entity);
                }
                else
                {
                    _entities.Add(entity);
                }

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
                int? subBranchIndex = CalculateSubBranchIndex(entity);

                if (subBranchIndex != null)
                {
                    isEntityFound = _subBranches[subBranchIndex.Value].RemoveEntity(entity);
                }
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

        public List<Entity> GetEntitiesInProximity(Entity entity)
        {
            if (_subBranches == null)
            {
                return _entities;
            }
            
            List<Entity> entitiesInProximity = new List<Entity>();

            int? subBranchIndex = CalculateSubBranchIndex(entity);

            if (subBranchIndex != null)
            {
                entitiesInProximity = _subBranches[subBranchIndex.Value].GetEntitiesInProximity(entity);
            }

            foreach (Entity subEntity in _entities)
            {
                entitiesInProximity.Add(subEntity);
            }

            return entitiesInProximity;
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

            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                int? subBranchIndex = CalculateSubBranchIndex(_entities[i]);

                if (subBranchIndex != null)
                {
                    _subBranches[subBranchIndex.Value].InsertEntity(_entities[i]);
                    _entities.Remove(_entities[i]);
                }
            }
        }

        private void DissolveSubPartitions()
        {
            _entities = GetAllEntities();
            _subBranches = null;
        }

        /// <param name="entity"></param>
        /// <returns>Index of the sub branch the entity should go in, or null if it should go in multiple.</returns>
        private int? CalculateSubBranchIndex(Entity entity)
        {
            int min = (entity.GetEntityData().Hitbox.Range.Item1 + entity.GetEntityData().Position).QuadrantFromOrigin(_center);
            int max = (entity.GetEntityData().Hitbox.Range.Item2 + entity.GetEntityData().Position).QuadrantFromOrigin(_center);


            if (min == max)
            {
                return min - 1;
            }

            return null;
        }

    }
}

