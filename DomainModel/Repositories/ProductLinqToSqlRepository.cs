// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductLinqToSqlRepository.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Class that implements the IProductRepository for LinqToSqlRepository{TEntity,TKey,TDataContextImpl} with
//   TEntity = Product, TKey = int and TDataContextImpl = DownloadServerDBDataContext.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SportsStore.DomainModel.Repositories
{
    using System.Data.Linq;
    using Entities;
    using MVCGenericLibrary.Model.Repository;

    /// <summary>
    /// Class that implements the <see cref="IProductRepository"/> for <see cref="LinqToSqlRepository{TEntity,TKey}"/> with
    /// TEntity = Product, TKey = int and TDataContextImpl = DownloadServerDBDataContext.
    /// </summary>
    public class ProductLinqToSqlRepository : LinqToSqlRepository<Product, int>, IProductRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductLinqToSqlRepository"/> class, 
        /// giving the base class constructor the expression to get the Product Key
        /// </summary>
        /// <param name="ctx">
        /// The <see cref="DataContext"/>.
        /// </param>
        public ProductLinqToSqlRepository(DataContext ctx) : base(product => product.ProductID, ctx)
        {
        }
    }
}