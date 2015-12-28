// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductsControllerTests.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Test Fixture for &lt;see cref="ProductsController" /&gt;.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SportsStore.Tests
{
    using DomainModel.Entities;
    using DomainModel.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MVCGenericLibrary.BaseControllersTests;
    using NUnit.Framework;
    using StructureMap;
    using StructureMap.Attributes;
    using WebUI.Controllers;

    /// <summary>
    /// Test Fixture for <see cref="ProductsController"/>.
    /// </summary>
    //[TestClass]
    [TestFixture]
    public class ProductsControllerTests :
        BaseControllerTest<Product, int, IProductRepository, ProductsController>
    {
        #region Test Constants
        private const int RepositoryExistingId = 1;
        private const int RepositoryNonExistingId = 4;
        #endregion Test Constants

        #region Test Fields


        #endregion Test Fields

        #region Constructors

        static ProductsControllerTests()
        {
            ObjectFactory.Initialize(
                x =>
                {
                    //x.ForRequestedType<IProductRepository>().CacheBy(InstanceScope.Singleton).TheDefaultIsConcreteType<FakeProductRepository>();
                    x.ForRequestedType<IProductRepository>().CacheBy(InstanceScope.Singleton).TheDefault.IsThis(new FakeProductRepository());
                    x.ForRequestedType<ProductsController>().CacheBy(InstanceScope.Singleton).TheDefault.Is.OfConcreteType<ProductsController>();
                });

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsControllerTests"/> class. 
        /// </summary>
        public ProductsControllerTests() :
            base(RepositoryExistingId, RepositoryNonExistingId, new Product { ProductID = 5, Name = "Product1" }, new Product { ProductID = 2 })
        {
        }

        #endregion Constructors

        #region private Utility methods

        #endregion private Utility methods
    }
}