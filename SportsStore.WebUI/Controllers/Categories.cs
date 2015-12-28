// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoriesController.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Defines the controller for Categorie type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SportsStore.WebUI.Controllers
{
    using DomainModel.Entities;
    using DomainModel.Repositories;
    using MVCGenericLibrary.Controller;

    /// <summary>
    /// Controller for <see cref="Category"/>.
    /// </summary>
    public class CategoriesController : BaseControllerForIModelEntityMembersEntities<Category, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="repository">
        /// The <see cref="ICategoryRepository"/> repository.
        /// </param>
        public CategoriesController(ICategoryRepository repository) : base(repository)
        {
        }
    }
}