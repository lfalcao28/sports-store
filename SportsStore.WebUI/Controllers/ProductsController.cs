// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductsController.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Defines the controller for Product type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SportsStore.WebUI.Controllers
{
    using DomainModel.Entities;
    using DomainModel.Repositories;
    using MVCGenericLibrary.Controller;

    /// <summary>
    /// Controller for <see cref="Product"/>.
    /// </summary>
    public class ProductsController : BaseControllerForIModelEntityMembersEntities<Product, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="repository">
        /// The <see cref="IProductRepository"/> repository.
        /// </param>
        public ProductsController(IProductRepository repository) : base(repository)
        {
            PageSize = 4;
        }
    }
}