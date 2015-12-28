// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseControllerForIModelEntityMembersEntities.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Base controller for &lt;typeparamref name="TEntity" /&gt; that implement the &lt;see cref="IModelEntityMembers{TEntity}" /&gt;.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Controller
{
    using Model.Entity;
    using Model.Repository;

    /// <summary>
    /// Base controller for <typeparamref name="TEntity"/> that implement the <see cref="IModelEntityMembers{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public abstract class BaseControllerForIModelEntityMembersEntities<TEntity, TKey> : BaseController<TEntity, TKey, TEntity>
        where TEntity : class, IModelEntity, IModelEntityMembers<TEntity>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseControllerForIModelEntityMembersEntities&lt;TEntity, TKey&gt;"/> class.
        /// </summary>
        /// <param name="repository">The <typeparamref name="TEntity"/> repository.</param>
        protected BaseControllerForIModelEntityMembersEntities(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }
}