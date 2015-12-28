// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexModel.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Defines the IndexModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Controller.PresentationModels
{
    using System.Collections.Generic;
    using Model.Helpers.Collections;

    /// <summary>
    /// The model to pass to the <code>Index</code> view.
    /// </summary>
    public class IndexModel
    {
        private readonly ModelEntityDescriptor _entityDescriptor;

        public IndexModel(ModelEntityDescriptor entityDescriptor, PagedList<ModelInstance> modelInstances)
        {
            _entityDescriptor = entityDescriptor;
            ModelInstances = modelInstances;
        }

        /// <summary>
        /// Gets EntityName that this ModelInstance belongs.
        /// </summary>
        public string EntityName {
            get { return _entityDescriptor.EntityName;  }
        }

        /// <summary>
        /// Gets the instance labels.
        /// </summary>
        /// <value>The instance labels.</value>
        public IList<string> Labels {
            get { return _entityDescriptor.Labels; }
        }

        /// <summary>
        /// Gets the <see cref="ModelInstance"/> collection.
        /// </summary>
        /// <value>The model instances.</value>
        public PagedList<ModelInstance> ModelInstances { get; private set; }
    }
}