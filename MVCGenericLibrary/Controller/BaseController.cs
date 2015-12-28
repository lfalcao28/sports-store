// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Base class for all controller with basic CRUD needs. It includes generic actions for list,
//   view details, edit, create and delete operations for a Model entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Helpers;
    using Model.Entity;
    using Model.Helpers.Collections;
    using Model.Repository;
    using PresentationModels;

    /// <summary>
    /// Base class for all controller with basic CRUD needs. It includes generic actions for list,
    /// view details, edit, create and delete operations for a Model entity.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the TEntity.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the <typeparamref name="TEntity"/> key.
    /// </typeparam>
    /// <typeparam name="TModelEntityMembers">
    /// The type that implements <see cref="IModelEntityMembers{TEntity}"/>.
    /// </typeparam>
    public abstract class BaseController<TEntity, TKey, TModelEntityMembers> : Controller
        where TEntity : class, IModelEntity, new()
        where TModelEntityMembers : class, IModelEntityMembers<TEntity>, new()
    {
        #region Private Fields

        /// <summary>
        /// The <typeparam name="TEntity"/> repository.
        /// </summary>
        private readonly IRepository<TEntity, TKey> _repository;

        /// <summary>
        /// The names of the <typeparam name="TEntity"/> allowed properties updatable by <see cref="Controller.UpdateModel{TModel}(TModel)"/>.
        /// </summary>
        private static readonly string[] _AllowedUpdateModelMembers;

        /// <summary>
        /// The <typeparam name="TEntity"/> <see cref="ModelEntityDescriptor"/>.
        /// </summary>
        private static readonly ModelEntityDescriptor _Modeldescriptor;

        /// <summary>
        /// Indicates weather the <typeparam name="TKey" /> is a value type.
        /// This field is initiated in controller type constructor and is used only
        /// in <see cref="GetModelInstance"/> to convert the key value to a string.
        /// </summary>
        /// private static readonly bool _KeyIsValueType;

        /// <summary>
        /// Backing field for <see cref="PageSize"/> property.
        /// </summary>
        private int _pageSize = 5;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="BaseController{TEntity,TKey,TModelEntityMembers}"/> class.
        /// This constructor initializes the <see cref="_Modeldescriptor"/> and <see cref="_AllowedUpdateModelMembers"/> members.
        /// </summary>
        static BaseController() 
        {
            TModelEntityMembers modelEntity = new TModelEntityMembers();
            _Modeldescriptor = new ModelEntityDescriptor(modelEntity.GetType().Name, modelEntity.Members);
            _AllowedUpdateModelMembers = _Modeldescriptor.Names.ToArray();
            // _KeyIsValueType = typeof (TKey).IsValueType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{TEntity,TKey, TModelEntityMembers}"/> class.
        /// </summary>
        /// <param name="repository">
        /// The <typeparamref name="TEntity"/> repository.
        /// </param>
        protected BaseController(IRepository<TEntity, TKey> repository) {
            _repository = repository;
        }

        #endregion Constructors

        #region Protected Properties

        /// <summary>
        /// Gets or sets the page size listing entities.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        /// <summary>
        /// Gets the array of <see cref="Controller.UpdateModel{TModel}(TModel)"/> updatable properties.
        /// </summary>
        /// <returns>
        /// The array of <see cref="Controller.UpdateModel{TModel}(TModel)"/> updatable properties.
        /// </returns>
        protected static string[] AllowedUpdateModelMembers
        {
            get { return _AllowedUpdateModelMembers; }
        }

        /// <summary>
        /// Gets the modeldescriptor associated with <typeparam name="TEntity"/>.
        /// </summary>
        /// <value>The modeldescriptor.</value>
        protected static ModelEntityDescriptor Modeldescriptor
        {
            get { return _Modeldescriptor; }
        }

        /// <summary>
        /// Gets the repository associated with <typeparam name="TEntity"/>.
        /// </summary>
        /// <value>The repository.</value>
        protected IRepository<TEntity, TKey> Repository
        {
            get { return _repository; }
        }

        #endregion Protected Properties

        #region Public interface

        #region Listing TEntity

        /// <summary>
        /// GET: /TEntity/ 
        /// Lists all <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="page">
        /// The page to return (1 based). If null shows the first page.
        /// </param>
        /// <returns>
        /// The Index <see cref="ActionResult"/>
        /// </returns>
        public virtual ActionResult Index(int? page)
        {
            return ViewIndex(page);
        }

        #endregion Listing TEntity

        #region Show TEntity Details

        /// <summary>
        /// GET: /TEntity/Details/5
        /// Returns a view that displays the entity details.
        /// </summary>
        /// <param name="key">
        /// The entity key.
        /// </param>
        /// <returns>
        /// The Details <see cref="ActionResult"/>
        /// </returns>
        public virtual ActionResult Details(TKey key) {
            TEntity entity = _repository.Get(key);
            if (entity == null) {
                return ViewEntityNotFound(key);
            }

            return ViewDetails(entity);
        }

        #endregion Show Entyty Details

        #region Editing TEntity

        /// <summary>
        /// GET: /TEntity/Edit/5
        /// Returns a view to edit the entity details.
        /// </summary>
        /// <param name="key">
        /// The entity key.
        /// </param>
        /// <returns>
        /// The Edit <see cref="ActionResult"/>
        /// </returns>
        public virtual ActionResult Edit(TKey key) {
            TEntity entity = _repository.Get(key);
            if (entity == null) {
                return ViewEntityNotFound(key);
            }

            return ViewEditGet(entity);
        }

        /// <summary>
        /// POST: /TEntity/Edit/5
        /// Returns a view after editing the entity details.
        /// </summary>
        /// <param name="key">
        /// The entity key.
        /// </param>
        /// <param name="collection">
        /// The Post <see cref="FormCollection"/>. 
        /// </param>
        /// <returns>
        /// The Post Edit <see cref="ActionResult"/>
        /// </returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Edit(TKey key, FormCollection collection) {
            TEntity entity = _repository.Get(key);
            if (entity == null) {
                return ViewEntityNotFound(key);
            }

            try {
                UpdateModelEntity(entity);
                OnBeforeEdit(entity);
                _repository.Update(entity);
                _repository.Save();
                return ViewEditPost(entity);
            } catch (Exception e) {
                ModelState.AddRulesViolations(entity);
                return ViewEditPostError(entity, e);
            }
        }

        #endregion Creating TEntity

        #region Creating TEntity

        /// <summary>
        /// GET: /TEntity/Create
        /// Returns a View to create an <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>
        /// The Create <see cref="ActionResult"/>
        /// </returns>
        public virtual ActionResult Create() {
            TEntity entity = new TEntity();
            return ViewCreateGet(entity);
        }

        /// <summary>
        /// POST: /TEntity/Create
        /// Returns a View after creating a <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="collection">
        /// The Post <see cref="FormCollection"/>.
        /// </param>
        /// <returns>
        /// The Post Create <see cref="ActionResult"/>
        /// </returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Create(FormCollection collection) {
            TEntity entity = new TEntity();
            try {
                UpdateModelEntity(entity);
                OnBeforeCreate(entity);
                _repository.Add(entity);
                _repository.Save();
                return ViewCreatePost(entity);
            } catch (Exception e) {
                ModelState.AddRulesViolations(entity);
                return ViewCreatePostError(entity, e);
            }
        }

        #endregion Creating TEntity

        #region Deleting TEntity

        /// <summary>
        /// GET: /TEntity/Delete/1
        /// Returns a View to delete an <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="key">The <typeparamref name="TEntity"/> unique identifier (key). </param>
        /// <returns>
        /// The Delete <see cref="ActionResult"/>
        /// </returns>
        public virtual ActionResult Delete(TKey key) 
        {
            TEntity entity = _repository.Get(key);
            if (entity == null) 
            {
                return ViewEntityNotFound(key);
            }

            return ViewDeleteGet(entity);
        }

        /// <summary>
        /// POST: /TEntity/Create
        /// Returns a View after deleting a <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="key">The <typeparamref name="TEntity"/> unique identifier (key).</param>
        /// <param name="collection">The Post <see cref="FormCollection"/>.</param>
        /// <returns>
        /// The Post Delete <see cref="ActionResult"/>
        /// </returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Delete(TKey key, FormCollection collection) 
        {
            TEntity entity = _repository.Get(key);
            if (entity == null) {
                return ViewEntityNotFound(key);
            }

            try {
                OnBeforeDelete(entity);
                _repository.Delete(entity);
                _repository.Save();
                return ViewDeletePost(entity);
            } catch (Exception e) {
                ModelState.AddRulesViolations(entity);
                return ViewDeletePostError(entity, e);
            }
        }

        #endregion Deleting TEntity

        #endregion Public interface

        #region Protected interface

        #region Protected action hooks methods (virtual)

        /// <summary>
        /// Hook method to update model entity. This methos call by default <see cref="Controller.UpdateModel{TModel}(TModel,string,string[])"/> 
        /// passing the <paramref name="entity"/> and the <see cref="AllowedUpdateModelMembers"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void UpdateModelEntity(TEntity entity)
        {
            UpdateModel(entity, AllowedUpdateModelMembers);
        }

        /// <summary>
        /// Hook method called on <see cref="Edit(TKey,System.Web.Mvc.FormCollection)"/> Post, 
        /// before sumbiting the updated <typeparamref name="TEntity"/> to the <see cref="_repository"/>
        /// </summary>
        /// <param name="entity">
        /// The entity being edited.
        /// </param>
        protected virtual void OnBeforeEdit(TEntity entity)
        {
        }

        /// <summary>
        /// Hook method called on <see cref="Create(System.Web.Mvc.FormCollection)"/> Post, 
        /// before sumbiting the created <typeparamref name="TEntity"/> to the <see cref="_repository"/>
        /// </summary>
        /// <param name="entity">
        /// The entity being created.
        /// </param>
        protected virtual void OnBeforeCreate(TEntity entity)
        {
        }

        /// <summary>
        /// Hook method called on <see cref="Delete(TKey,System.Web.Mvc.FormCollection)"/> Post, 
        /// before sumbiting the deleted <typeparamref name="TEntity"/> to the <see cref="_repository"/>
        /// </summary>
        /// <param name="entity">
        /// The entity being deleted.
        /// </param>
        protected virtual void OnBeforeDelete(TEntity entity)
        {
        }

        /// <summary>
        /// This is a hook method called to render the view when entity is not found. 
        /// By default renders <code>EntityNotFound</code> view.
        /// </summary>
        /// <param name="key">
        /// The entity key not found.
        /// </param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewEntityNotFound(TKey key)
        {
            return View("EntityNotFound", key.ToString() as object);
        }

        /// <summary>
        /// This is a hook method called to render the view that lists a page of entities. 
        /// By default renders <code>Index</code> view.
        /// </summary>
        /// <param name="page">
        /// The current page number received in the request (one based).
        /// </param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewIndex(int? page)
        {
            IndexModel indexModel = TempData["indexModelPage"] as IndexModel;

            if (indexModel == null)
            {
                indexModel = GetIndexModel(page == null ? 0 : page.Value - 1);

                // See if the requested page corresponds to the indexModel page
                if (page != null && indexModel.ModelInstances.PageIndex != page.Value - 1)
                {
                    // If not redirect so that a correct Url appears on the browser
                    TempData["indexModelPage"] = indexModel;
                    return RedirectToAction("Index", new { page = indexModel.ModelInstances.PageIndex + 1 });
                }
            }

            return View("Index", indexModel);
        }

        /// <summary>
        /// This is a hook method called to render the view that shows the entity details. 
        /// By default renders <code>Details</code> view.
        /// </summary>
        /// <param name="entity">
        /// The <typeparamref name="TEntity"/> to show the details.
        /// </param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewDetails(TEntity entity)
        {
            return View("Details", GetModelInstance(entity, ModelInstance.ViewMode.Details));
        }

        /// <summary>
        /// This is a hook method called to render the view to edit an <typeparamref name="TEntity"/>. 
        /// By default renders <code>Edit</code> view.
        /// </summary>
        /// <param name="entity">
        /// The <typeparamref name="TEntity"/> to edit.
        /// </param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewEditGet(TEntity entity)
        {
            return View("Edit", GetModelInstance(entity, ModelInstance.ViewMode.Edit));
        }

        /// <summary>
        /// This is a hook method called to render the view after editing an <typeparamref name="TEntity"/>. 
        /// By default renders <code>Details</code> view.
        /// </summary>
        /// <param name="entity">
        /// The <typeparamref name="TEntity"/> edited.
        /// </param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewEditPost(TEntity entity)
        {
            return RedirectToAction("Details", new { key = entity.Key });
        }

        /// <summary>
        /// This is a hook method called to render the view after editing an <typeparamref name="TEntity"/> with errors.
        /// By default renders <code>Edit</code> view.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> edited.</param>
        /// <param name="exception">The exception that prevented editing the entity.</param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewEditPostError(TEntity entity, Exception exception)
        {
            ModelInstance mi = GetModelInstance(entity, ModelInstance.ViewMode.Edit);
            #if DEBUG
            ModelState.AddModelError(string.Empty, String.Format("Error editing {0}. {1}", mi.EntityName, exception));
            #else
            ModelState.AddModelError(string.Empty, String.Format("Error editing {0}.", mi.EntityName));
            
            #endif
            return View("Edit", mi);
        }

        /// <summary>
        /// This is a hook method called to render the view to create an <typeparamref name="TEntity"/>. 
        /// By default renders <code>Create</code> view.
        /// </summary>
        /// <param name="entity">
        /// The <typeparamref name="TEntity"/> to create.
        /// </param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewCreateGet(TEntity entity)
        {
            return View("Create", GetModelInstance(entity, ModelInstance.ViewMode.Create));
        }

        /// <summary>
        /// This is a hook method called to render the view after creating an <typeparamref name="TEntity"/>. 
        /// By default renders <code>Details</code> view.
        /// </summary>
        /// <param name="entity">
        /// The <typeparamref name="TEntity"/> created.
        /// </param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewCreatePost(TEntity entity)
        {
            return RedirectToAction("Details", new { key = entity.Key });
        }

        /// <summary>
        /// This is a hook method called to render the view after creating an <typeparamref name="TEntity"/> with errors.
        /// By default renders <code>Create</code> view.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> created.</param>
        /// <param name="exception">The exception that prevented creating the entity.</param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewCreatePostError(TEntity entity, Exception exception)
        {
            ModelInstance mi = GetModelInstance(entity, ModelInstance.ViewMode.Create);
            #if DEBUG
            ModelState.AddModelError(string.Empty, String.Format("Error creating {0}. {1}", mi.EntityName, exception));
            #else
            ModelState.AddModelError(string.Empty, String.Format("Error creating {0}.", mi.EntityName));
            #endif
            return View("Create", mi);
        }

        /// <summary>
        /// This is a hook method called to render the view to delete a <typeparamref name="TEntity"/>. 
        /// By default renders <code>Delete</code> view.
        /// </summary>
        /// <param name="entity">
        /// The <typeparamref name="TEntity"/> to edit.
        /// </param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewDeleteGet(TEntity entity)
        {
            return View("Delete", GetModelInstance(entity, ModelInstance.ViewMode.Delete));
        }

        /// <summary>
        /// This is a hook method called to render the view after deleting a <typeparamref name="TEntity"/>. 
        /// By default renders <code>Deleted</code> view.
        /// </summary>
        /// <param name="entity">
        /// The deleted <typeparamref name="TEntity"/>.
        /// </param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewDeletePost(TEntity entity)
        {
            return View("Deleted", GetModelInstance(entity, ModelInstance.ViewMode.Delete));
        }

        /// <summary>
        /// This is a hook method called to render the view after deleting a <typeparamref name="TEntity"/> with errors.
        /// By default renders <code>DeleteError</code> view.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> that was tried to delete.</param>
        /// <param name="exception">The exception that prevented deleting the entity.</param>
        /// <returns>
        /// Return a <see cref="ViewResult"/> that renders a view to the response.
        /// </returns>
        protected virtual ActionResult ViewDeletePostError(TEntity entity, Exception exception)
        {
            return View("NotDeleted", GetModelInstance(entity, ModelInstance.ViewMode.Delete));
        }

        #endregion Protected action hooks methods (virtual)

        /// <summary>
        /// Gets the <see cref="ModelInstance"/> for the given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="viewMode">The view the ModelInstance is going to be passed.</param>
        /// <returns>
        /// The <see cref="ModelInstance"/> for the given <paramref name="entity"/>.
        /// </returns>
        protected ModelInstance GetModelInstance(TEntity entity, ModelInstance.ViewMode viewMode) {
            // TKey key = entity.Key;
            // string strKey = (_KeyIsValueType || !Equals(key, default(TKey))) ? key.ToString() : string.Empty;
            string strKey = entity.Key;
            ModelInstance mi = new ModelInstance(_Modeldescriptor, entity, viewMode);
            return mi;
        }

        /// <summary>
        /// Returns the <see cref="IndexModel"/> instance for the <paramref name="pageIndex"/> page, 
        /// to pass to the <code>Index</code> View.
        /// </summary>
        /// <param name="pageIndex">
        /// The page index (zero based).
        /// </param>
        /// <returns>
        /// The <see cref="IndexModel"/> instance to pass to the <code>Index</code> View.
        /// </returns>
        /// <remarks>If <see cref="pageIndex"/> is less than zero, the first page is returned. 
        /// If <see cref="pageIndex"/> is greater than the maximum number of pages for the available data, the last
        /// page is returned.
        /// </remarks>
        protected IndexModel GetIndexModel(int pageIndex) {
            IQueryable<TEntity> allEntities = _repository.GetAll();

            // Check if the supplied page index is < 0 or > maxPages and correct it correspondingly
            if(pageIndex < 0) {
                pageIndex = 0;
            }
            else {
                int maxPages = (int)Math.Ceiling(allEntities.Count() / (double)PageSize);

                // Math.Abs us used for the case where there is no data, maxPages-1 would be -1, and consequently < 0
                pageIndex = Math.Min(pageIndex, Math.Abs(maxPages - 1));
            }

            PagedList<TEntity> list = allEntities.ToPagedList(pageIndex, PageSize);

            PagedList<ModelInstance> modelInstances = new PagedList<ModelInstance>(list.Select(entity => GetModelInstance(entity, ModelInstance.ViewMode.Index)), list.TotalCount, list.PageIndex, list.PageSize);
            return new IndexModel(_Modeldescriptor, modelInstances);
        }

        #endregion Protected interface

        #region Private Methods and Properties

        #endregion Private Methods and Properties
    }
}