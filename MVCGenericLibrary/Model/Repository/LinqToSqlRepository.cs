// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinqToSqlRepository.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   LinqToSql implementation of &lt;see cref="IRepository{TEntity,TKey}" /&gt;
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MVCGenericLibrary.Model.Repository
{
    using System;
    using System.Data.Linq;
    using System.Linq;
    using System.Linq.Expressions;
    using Entity;
    using Helpers;

    /// <summary>
    /// LinqToSql implementation of <see cref="IRepository{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">The TEntity type</typeparam>
    /// <typeparam name="TKey">The type of the TEntity key.</typeparam>
    public class LinqToSqlRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IModelEntity
    {
        /// <summary>
        /// The <see cref="DataContext"/> derived type instance
        /// </summary>
        private readonly DataContext _dataContext;

        /// <summary>
        /// The <see cref="Table{TEntity}"/> correnponding to the entity represented by the Repository.
        /// </summary>
        private readonly Table<TEntity> _table;

        /// <summary>
        /// The Key selector expression for instances of <typeparamref name="TEntity"/>. 
        /// This  exression projects an instance of <typeparam name="TEntity"/> in <typeparam name="TKey"/>
        /// </summary>
        private readonly Expression<Func<TEntity, TKey>> _keySelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinqToSqlRepository{TEntity,TKey}"/> class. 
        /// </summary>
        /// <param name="keySelector">
        /// The key selector.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        protected LinqToSqlRepository(Expression<Func<TEntity, TKey>> keySelector, DataContext dataContext)
        {
            // _dataContext.Log = Console.Out;
            _dataContext = dataContext;
            _table = _dataContext.GetTable<TEntity>();
            _keySelector = keySelector;
            CheckRulesViolationsOnEntitiesWhenUpdateddAndCreated = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the rules violations should be verified on entities 
        /// when being edited or created.
        /// </summary>
        /// <value>
        /// <c>true</c> if whether the rules violations should be verified on entities 
        /// when being edited or created; otherwise, <c>false</c>.
        /// </value>
        protected bool CheckRulesViolationsOnEntitiesWhenUpdateddAndCreated { get; set; }

        #region Implementation of IRepository<TEntity,string>

        /// <summary>
        /// Gets the key of the specified <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity key</returns>
        public TKey GetKey(TEntity entity)
        {
            return _keySelector.Compile()(entity);
        }

        /// <summary>
        /// Gets an <see cref="IQueryable{T}"/> for all Entities.
        /// </summary>
        /// <returns>An <see cref="IQueryable{TEntity}"/> for all Entities</returns>
        public IQueryable<TEntity> GetAll()
        {
            return _table;
        }

        /// <summary>
        /// Gets an entity given the specified <paramref name="entityId"/>.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public TEntity Get(TKey entityId)
        {
            ParameterExpression prm = Expression.Parameter(typeof(TEntity), "x");
            return _table.SingleOrDefault(
                Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(
                        Expression.Invoke(_keySelector, prm),
                        Expression.Constant(entityId)),
                    prm));
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Add(TEntity entity)
        {
            if (CheckRulesViolationsOnEntitiesWhenUpdateddAndCreated) {
                entity.CheckRulesViolations();
            }
            _table.InsertOnSubmit(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public void Update(TEntity entity)
        {
            if (CheckRulesViolationsOnEntitiesWhenUpdateddAndCreated) {
                entity.CheckRulesViolations();
            }
            ////_dataContext.SubmitChanges();
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public void Delete(TEntity entity)
        {
            _table.DeleteOnSubmit(entity);
        }

        /// <summary>
        /// Saves permanently all changes made to the repository.
        /// </summary>
        public void Save()
        {
            _dataContext.SubmitChanges();
        }

        #endregion
    }
}

