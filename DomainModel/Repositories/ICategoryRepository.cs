// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategoryRepository.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Defines the IRepository interface for Category.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SportsStore.DomainModel.Repositories
{
    using Entities;
    using MVCGenericLibrary.Model.Repository;

    /// <summary>
    /// The <see cref="IRepository{TEntity,TKey}"/> interface for <see cref="Category"/>
    /// </summary>
    public interface ICategoryRepository : IRepository<Category, int>
    {
    }
}