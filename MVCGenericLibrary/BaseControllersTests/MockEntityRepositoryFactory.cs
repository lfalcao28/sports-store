namespace MVCGenericLibrary.BaseControllersTests
{
    using System.Linq;
    using Model.Entity;
    using Model.Repository;
    using Moq;

    public static class MockEntityRepositoryFactory
    {
        public static TIRepository MockEntityRepository<TEntity, TKey, TIRepository>(TKey existingentityId, params TEntity[] entities)
            where TEntity : class, IModelEntity
            where TIRepository : class, IRepository<TEntity, TKey>
        {
            Mock<TIRepository> mocksRepository = new Mock<TIRepository>();
            mocksRepository.Setup(r => r.GetAll()).Returns(entities.AsQueryable());
            mocksRepository.Setup(r => r.Get(existingentityId)).Returns(entities.SingleOrDefault(e => e.Key.ToString() == existingentityId.ToString()));

            //mocksRepository.Setup(r => r.Add(existingentityId)).Returns(entities.SingleOrDefault(e => e.Key.ToString() == existingentityId.ToString()));
            return mocksRepository.Object;
        }
    }
}