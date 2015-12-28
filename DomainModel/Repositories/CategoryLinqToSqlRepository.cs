// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryLinqToSqlRepository.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Class that implements the ICategoryRepository for LinqToSqlRepository{TEntity,TKey,TDataContextImpl} with
//   TEntity = Category, TKey = int and TDataContextImpl = DataContext.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SportsStore.DomainModel.Repositories
{
    using System.Data.Linq;
    using Entities;
    using MVCGenericLibrary.Model.Repository;

    /// <summary>
    /// Class that implements the <see cref="ICategoryRepository"/> for <see cref="LinqToSqlRepository{TEntity,TKey}"/> with
    /// TEntity = Category, TKey = int and TDataContextImpl = DownloadServerDBDataContext.
    /// </summary>
    public class CategoryLinqToSqlRepository : LinqToSqlRepository<Category, int>, ICategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryLinqToSqlRepository"/> class, 
        /// giving the base class constructor the expression to get the Category Key
        /// </summary>
        /// <param name="ctx">
        /// The <see cref="DataContext"/>.
        /// </param>
        public CategoryLinqToSqlRepository(DataContext ctx) : base(category => category.CategoryID, ctx)
        {
        }
    }
}