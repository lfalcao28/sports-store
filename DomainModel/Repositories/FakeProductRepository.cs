namespace SportsStore.DomainModel.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using MVCGenericLibrary.BaseControllersTests;

    public class FakeProductRepository : FakeMemoryRepository<Product, int>, IProductRepository
    {
        // Fake hard-coded list of products
        private static List<Product> _FakeProducts = new List<Product> {
            new Product { ProductID = 1, Name = "Football", Price = 25 },
            new Product { ProductID = 2, Name = "Surf board", Price = 179 },
            new Product { ProductID = 3, Name = "Running shoes", Price = 95 }
            };

        public FakeProductRepository() : base(_FakeProducts, p => p.ProductID)
        {
        }

        public FakeProductRepository(IList<Product> fakeProducts) : base(fakeProducts, p => p.ProductID)
        {
        }

    }
}