// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Base interface for an entity Repository. This interface includes the basic CRUD operations
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Repository
{
    using System.Linq;

    /// <summary>
    /// Base interface for an entity Repository. This interface includes the basic CRUD operations
    /// </summary>
    /// <typeparam name="TEntity">The TEntity type</typeparam>
    /// <typeparam name="TKey">The type of the TEntity key.</typeparam>
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        ///// <summary>
        ///// Gets the key of the specified <paramref name="entity"/>.
        ///// </summary>
        ///// <param name="entity">The entity.</param>
        ///// <returns>The entity key</returns>
        //TKey GetKey(TEntity entity);

        /// <summary>
        /// Gets an <see cref="IQueryable{T}"/> for all Entities.
        /// </summary>
        /// <returns>An <see cref="IQueryable{TEntity}"/> for all Entities</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Gets an entity given the specified <paramref name="entityId"/>.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        TEntity Get(TKey entityId);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Saves permanently all changes made to the repository.
        /// </summary>
        void Save();
    }
}