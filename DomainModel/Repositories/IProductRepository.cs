// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductRepository.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Defines the IRepository interface for Product.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SportsStore.DomainModel.Repositories
{
    using Entities;
    using MVCGenericLibrary.Model.Repository;

    /// <summary>
    /// The <see cref="IRepository{TEntity,TKey}"/> interface for <see cref="Product"/>
    /// </summary>
    public interface IProductRepository : IRepository<Product, int>
    {
        // IProductRepository specific members should go here
    }
}