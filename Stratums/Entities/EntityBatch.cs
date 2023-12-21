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
        private Dictionary<Entity, List<Vector2>> _entities;
        private Dictionary<Vector2, List<Entity>> _partitions;
        private int _partitionSize;

        public List<Entity> GetEntities() { return _entities.Keys.ToList(); }
        public Dictionary<Entity, List<Vector2>> GetEntitiesWithPartitions() { return _entities; }
        public List<Vector2> GetPartitionsInEntity(Entity entity) { return _entities[entity]; }
        public EntityBatch()
        {
            _entities = new Dictionary<Entity, List<Vector2>>();
            _partitions = new Dictionary<Vector2, List<Entity>>();
            _partitionSize = 100;
        }
        public EntityBatch(int partitionSize)
        {
            _entities = new Dictionary<Entity, List<Vector2>>();
            _partitions = new Dictionary<Vector2, List<Entity>>();
            _partitionSize = partitionSize;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Entity entity in _entities.Keys)
            {
                entity.Update(gameTime, this);
                UpdateEntityPartitions(entity);

            }

        }

        public void Draw(Vector2 camera)
        {
            foreach (Entity entity in _entities.Keys)
            {
                entity.Draw(camera);
            }
        }

        /// <summary> Declare "AddEntity()" in LoadContent() </summary>
        public void AddEntity(Entity entity)
        {
            var partitionsInEntity = new Tuple<Entity, List<Vector2>>(entity, entity.CalculatePartitionIndexes(_partitionSize));
            _entities.Add(partitionsInEntity.Item1, partitionsInEntity.Item2);
            //Debug.Assert(false);

            foreach (var partitionIndex in partitionsInEntity.Item2)
            {
                switch (_partitions.ContainsKey(partitionIndex))
                {
                    case true:

                        _partitions[partitionIndex].AddWithoutRepeats(entity);
                        break;

                    case false:

                        _partitions.Add(partitionIndex, new List<Entity>());
                        _partitions[partitionIndex].Add(entity);
                        break;
                }
            }
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);

            foreach (var partitionIndex in entity.CalculatePartitionIndexes(_partitionSize))
            {
                _partitions[partitionIndex].Remove(entity);

                if (_partitions[partitionIndex].Count == 0)
                {
                    _partitions.Remove(partitionIndex);
                }

            }

        }

        public void UpdateEntityPartitions(Entity entity)
        {
            var partitionIndexes = entity.CalculatePartitionIndexes(_partitionSize);

            foreach (var partitionIndex in _entities[entity])
            {
                _partitions[partitionIndex].Remove(entity);

                if (_partitions[partitionIndex].Count == 0)
                {
                    _partitions.Remove(partitionIndex);
                }
            }

            _entities[entity] = partitionIndexes;

            foreach (var partitionIndex in partitionIndexes)
            {
                switch (_partitions.ContainsKey(partitionIndex))
                {
                    case true:

                        _partitions[partitionIndex].AddWithoutRepeats(entity);
                        break;

                    case false:

                        _partitions.Add(partitionIndex, new List<Entity>());
                        _partitions[partitionIndex].Add(entity);
                        break;
                }
            }
        }

        public void TranslateEntityPartitions(this Entity entity, Vector2 translation)
        {
            for (int i = 0; i < _entities[entity].Count; i++)
            {
                _entities[entity][i] += new Vector2(translation.X, translation.Y);
            }
        }

        public List<Entity> GetComparableEntities(Entity entity)
        {
            var partitions = _entities[entity];
            var entities = new List<Entity>();

            foreach (var partition in partitions)
            {
                entities.AddWithoutRepeats(_partitions[partition]);
            }

            return entities;
        }
    }
}
