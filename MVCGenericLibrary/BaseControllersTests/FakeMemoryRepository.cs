// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeMemoryRepository.cs" company="CCISEL">
//  Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009 
// </copyright>
// <summary>
//   Implements a &lt;see cref="IRepository{TEntity,TKey}" /&gt; with objects in memory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.BaseControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model.Entity;
    using Model.Helpers;
    using Model.Repository;

    /// <summary>
    /// Implements a <see cref="IRepository{TEntity,TKey}"/> with objects in memory.
    /// </summary>
    /// <typeparam name="TEntity">The TEntity type</typeparam>
    /// <typeparam name="TKey">The type of the TEntity key.</typeparam>
    public class FakeMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IModelEntity {
        /// <summary>
        /// The collection of <typeparam name="TEntity"/>
        /// </summary>
        private readonly Dictionary<TKey, TEntity> _models = new Dictionary<TKey, TEntity>();

        /// <summary>
        /// The function to extract the <typeparam name="TEntity"/> from an <typeparam name="TEntity"/>
        /// </summary>
        private readonly Func<TEntity, TKey> _keySelector;

        /// <summary>
        /// The collection of changes entities
        /// </summary>
        private List<TEntity> _changedEntities = new List<TEntity>();

        /// <summary>
        /// The collection of Inseted entities
        /// </summary>
        private List<TEntity> _insertedEntities = new List<TEntity>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeMemoryRepository{TEntity,TKey}"/> class.
        /// </summary>
        /// <param name="models">
        /// The collection of <typeparamref name="TEntity"/> instances.
        /// </param>
        /// <param name="keySelector">
        /// The id selector function <see cref="_keySelector"/>.
        /// </param>
        protected FakeMemoryRepository(IEnumerable<TEntity> models, Func<TEntity, TKey> keySelector) {
            _keySelector = keySelector;
            if (models == null) {
                return; 
            }

            foreach (TEntity entity in models) {
                _models.Add(keySelector(entity), entity);
            }
        }

        #region private Helper methods

        #endregion private Helper methods

        #region Implementation of IPOSModelRepository

        /// <summary>
        /// Gets the key of the specified <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity key</returns>
        public TKey GetKey(TEntity entity) {
            return _keySelector(entity);
        }

        /// <summary>
        /// Gets an <see cref="IQueryable{T}"/> for all Entities.
        /// </summary>
        /// <returns>An <see cref="IQueryable{TEntity}"/> for all Entities</returns>
        public IQueryable<TEntity> GetAll() {
            _changedEntities.AddRange(_models.Values);
            return _models.Values.AsQueryable();
        }

        /// <summary>
        /// Gets an entity given the specified <paramref name="entityId"/>.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public TEntity Get(TKey entityId) {
            TEntity entity = default(TEntity);
            if (_models.ContainsKey(entityId)) {
                entity = _models[entityId];
                _changedEntities.Add(entity);
            }

            return entity;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public void Update(TEntity entity)
        {
            _changedEntities.Add(entity);
            Save();
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public void Delete(TEntity entity) {
            TKey id = _keySelector(entity);
            _models.Remove(id);

            // If it exists in changdEntities, remove it also.
            TEntity deletedEntity = _changedEntities.SingleOrDefault(e => _keySelector(e).Equals(id));
            if (deletedEntity != default(TEntity)) {
                _changedEntities.Remove(deletedEntity);
            }

            Save();
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Add(TEntity entity) {
            _changedEntities.Add(entity);
            _insertedEntities.Add(entity);
            Save();
        }

        /// <summary>
        /// Saves permanently all changes made to the repository.
        /// </summary>
        public void Save() {
            List<TEntity> changedEntities = _changedEntities;
            _changedEntities = new List<TEntity>();
            _insertedEntities.Clear();
            changedEntities.ForEach(e => e.CheckRulesViolations());
            changedEntities.ForEach(e => _models[_keySelector(e)] = e);
        }
        #endregion
        }
}