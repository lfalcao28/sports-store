namespace MVCGenericLibrary.BaseControllersTests
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Controller;
    using Controller.PresentationModels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model.Entity;
    using Model.Helpers;
    using Model.Repository;
    using NUnit.Framework;
    using StructureMap;
    using Assert=NUnit.Framework.Assert;

    /// <summary>
    /// Generic base class for all <see cref="Controller"/> tests.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The entity key type.
    /// </typeparam>
    /// <typeparam name="TRepository">
    /// The <see cref="IRepository{TEntity,TKey}"/> implementation.
    /// </typeparam>
    /// <typeparam name="TController">
    /// The <see cref="BaseController{TEntity,TKey,TModelEntityMembers}"/> derived class.
    /// </typeparam>
    [TestClass]
    [TestFixture]
    public class BaseControllerTest<TEntity, TKey, TRepository, TController>
        where TEntity : class, IModelEntity, IModelEntityMembers<TEntity>, new()
        where TRepository : IRepository<TEntity, TKey>
        where TController : BaseController<TEntity, TKey, TEntity>
    {
        #region Test Constants

        #endregion Test Constants

        #region Test Fields

        protected TRepository _repository;
        protected TController _controller;
        protected TKey _repositoryExistingId;
        protected TKey _repositoryNonExistingId;
        protected TEntity _validEntity;
        private readonly TEntity _invalidEntity;

        #endregion Test Fields

        public BaseControllerTest(TKey repositoryExistingId, TKey repositoryNonExistingId, TEntity validEntity, TEntity invalidEntity)
        {
            _repositoryExistingId = repositoryExistingId;
            _repositoryNonExistingId = repositoryNonExistingId;
            _validEntity = validEntity;
            _invalidEntity = invalidEntity;

            _repository = ObjectFactory.GetInstance<TRepository>();
            _controller = ObjectFactory.GetInstance<TController>();
        }

        #region private Utility methods

        private static void CheckActionReturnsAViewResultAndHasAModelOfType(ViewResult result, String viewName, Type modelType)
        {
            Assert.IsNotNull(result, "View Expected");
            Assert.AreEqual(viewName, result.ViewName);
            Assert.AreSame(modelType, result.ViewData.Model.GetType(), "Model Type is not the expected");
        }

        protected void FillControllerValueProviderWithEntityToInsertKeyNamesAndValues(TEntity entity)
        {
            String[] updatableProperties = entity.Members.Names();

            Type t = typeof (TEntity);
            foreach (String propName in updatableProperties)
            {
                Object value = t.GetProperty(propName, BindingFlags.Public | BindingFlags.GetProperty |
                                                       BindingFlags.Instance).GetValue(entity, null);
                _controller.ValueProvider.Add(propName, new ValueProviderResult(value, value == null ? "" : value.ToString(),
                                                                      CultureInfo.InvariantCulture));
            }
        }

        protected void ClearControllerValueProvider()
        {
            _controller.ValueProvider.Clear();
        }

        #endregion private Utility methods

        #region Initialize and Cleanup methods
        
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void MyTestInitialize()
        {
        }

        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        [TearDown]
        public void MyTestCleanup()
        {
            ClearControllerValueProvider();
            _repository.Delete(_validEntity);
            _controller.TempData.Clear();
            //_repository.Save();
        }

        #endregion Initialize and Cleanup methods

        #region Test Methods

        [TestMethod]
        [Test]
        public void IndexActionShouldReturnIndexViewWithModelOfTypeIndexModel()
        {
            // Arrange

            // Act
            ViewResult result = _controller.Index(1) as ViewResult;

            // Assert
            CheckActionReturnsAViewResultAndHasAModelOfType(result, "Index", typeof (IndexModel));
            Assert.IsTrue(((IndexModel) result.ViewData.Model).ModelInstances.Count > 0);
        }

        [TestMethod]
        [Test]
        public void IndexActionShouldPresentTheCorrectPage()
        {
            // Arrange
            _controller.PageSize = 2;
            int total = _repository.GetAll().Count();
            int lastPage = (total / _controller.PageSize) + (total % _controller.PageSize == 0 ? 0 : 1);

            // Act
            ViewResult result = _controller.Index(lastPage) as ViewResult;

            // Assert
            IndexModel model = (IndexModel) result.ViewData.Model;
            Assert.AreEqual(_controller.PageSize, model.ModelInstances.PageSize);
            Assert.AreEqual(lastPage, model.ModelInstances.PageIndex + 1);
            Assert.AreEqual(lastPage, model.ModelInstances.TotalPages);
        }

        [TestMethod]
        [Test]
        public void IndexActionShouldRedirectToTheFirstPageWhenPageNumberBellowOne()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = _controller.Index(0) as RedirectToRouteResult;
            _controller.TempData.Clear();
            RedirectToRouteResult result1 = _controller.Index(-100) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(1, result.RouteValues["page"]);
            Assert.AreEqual("Index", result1.RouteValues["action"]);
            Assert.AreEqual(1, result1.RouteValues["page"]);
        }

        [TestMethod]
        [Test]
        public void IndexActionShouldRedirectToTheLastPageWhenPageNumberIsGreaterThanLastPage()
        {
            // Arrange
            _controller.PageSize = 2;
            int total = _repository.GetAll().Count();
            int lastPage = (int)Math.Ceiling(total / (double)_controller.PageSize);

            // Act
            RedirectToRouteResult result = _controller.Index(1000000) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(lastPage, result.RouteValues["page"]);
        }

        [TestMethod]
        [Test]
        public void DetailsActionShouldReturnDetailsViewWithValidId()
        {
            // Arrange

            // Act
            var result = _controller.Details(_repositoryExistingId) as ViewResult;
            // Assert
            CheckActionReturnsAViewResultAndHasAModelOfType(result, "Details", typeof(ModelInstance));
        }

        [TestMethod]
        [Test]
        public void DetailsActionShouldReturnEntityNotFoundViewWithInvalidId()
        {
            // Arrange

            // Act
            ViewResult result = _controller.Details(_repositoryNonExistingId) as ViewResult;
            // Assert
            CheckActionReturnsAViewResultAndHasAModelOfType(result, "EntityNotFound", typeof (String));
            Assert.AreEqual(_repositoryNonExistingId.ToString(), result.ViewData.Model);
        }

        [TestMethod]
        [Test]
        public void EditGetActionShouldReturnEditViewWithValidId()
        {
            // Act
            var result = _controller.Edit(_repositoryExistingId) as ViewResult;
            // Assert
            CheckActionReturnsAViewResultAndHasAModelOfType(result, "Edit", typeof (ModelInstance));
        }

        [TestMethod]
        [Test]
        public void EditGetActionShouldReturnEntityNotFoundViewWithInvalidId()
        {
            // Act
            var result = _controller.Edit(_repositoryNonExistingId) as ViewResult;

            // Assert
            CheckActionReturnsAViewResultAndHasAModelOfType(result, "EntityNotFound", typeof (String));
            Assert.AreEqual(_repositoryNonExistingId.ToString(), result.ViewData.Model);
        }

        [TestMethod]
        [Test]
        public void EditPostActionShouldReturnRedirectToRouteResultForDetailsWithValidId()
        {
            // Act
            var result = _controller.Edit(_repositoryExistingId, null) as RedirectToRouteResult;
            // Assert
            Assert.IsNotNull(result, "RedirectToRouteResult expected");
            Assert.AreEqual(2, result.RouteValues.Keys.Count);
            Assert.AreEqual(_repositoryExistingId.ToString(), result.RouteValues["id"]);
            Assert.AreEqual("Details", result.RouteValues["action"]);
        }

        [TestMethod]
        [Test]
        public void EditPostActionShouldReturnEntityNotFoundViewWithInvalidId()
        {
            // Act
            var result = _controller.Edit(_repositoryNonExistingId, null) as ViewResult;

            // Assert
            CheckActionReturnsAViewResultAndHasAModelOfType(result, "EntityNotFound", typeof (String));
            Assert.AreEqual(_repositoryNonExistingId.ToString(), result.ViewData.Model);
        }

        [TestMethod]
        [Test]
        public void EditPostActionShouldReturnEditViewAndModelStateWithErrorsWithInvalidEntityData() {
            // Prepare
            _repository.Add(_validEntity);
            //_repository.Save();
            FillControllerValueProviderWithEntityToInsertKeyNamesAndValues(_invalidEntity);

            // Act
            var result = _controller.Edit((TKey)Convert.ChangeType(_invalidEntity.Key, typeof(TKey)), null) as ViewResult;

            // Assert
            CheckActionReturnsAViewResultAndHasAModelOfType(result, "Edit", typeof (ModelInstance));

            //Assert.AreEqual(_repositoryNonExistingId.ToString(), result.ViewData.Model);
        }

        [TestMethod]
        [Test]
        public void CreateGetActionShouldReturnCreateViewWithDefaultId()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            CheckActionReturnsAViewResultAndHasAModelOfType(result, "Create", typeof (ModelInstance));
            Assert.AreEqual(String.Empty, ((ModelInstance) result.ViewData.Model).Key);
        }

        [TestMethod]
        [Test]
        public void CreatePostActionShouldReturnRedirectToRouteResultForDetailsWithTheNewId()
        {
            // Prepare
            FillControllerValueProviderWithEntityToInsertKeyNamesAndValues(_validEntity);

            // Act
            var result = _controller.Create(null) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "RedirectToRouteResult expected");
            Assert.AreEqual(2, result.RouteValues.Keys.Count);
            //Assert.AreEqual(_repositoryExistingId, result.RouteValues["id"]);
            Assert.AreEqual("Details", result.RouteValues["action"]);
        }

        [TestMethod]
        [Test]
        public void CreatePostActionShouldInsertTheNewEntityInTheRepository()
        {
            // Prepare
            FillControllerValueProviderWithEntityToInsertKeyNamesAndValues(_validEntity);

            // Act
            var result = _controller.Create(null) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "RedirectToRouteResult expected");
            Assert.AreEqual(2, result.RouteValues.Keys.Count);
            //Assert.AreEqual(_repositoryExistingId, result.RouteValues["id"]);
            Assert.AreEqual("Details", result.RouteValues["action"]);
        }

        [TestMethod]
        [Test]
        public void CreatePostActionShouldReturnCreateViewAndModelStateWithErrorsForInvalidEntityData()
        {
            // Prepare
            FillControllerValueProviderWithEntityToInsertKeyNamesAndValues(_invalidEntity);

            // Act
            var result = _controller.Create(null) as ViewResult;

            // Assert
            CheckActionReturnsAViewResultAndHasAModelOfType(result, "Create", typeof (ModelInstance));
            Assert.Greater(_controller.ModelState.Count, 0, "ModelState error should exist");
        }

        #endregion Test Methods
    }
}