// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelEntityDescriptor.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Model instance to pass to the Generic Index View
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Controller.PresentationModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model.Entity.Members;

    /// <summary>
    /// Instances of this class descrie a Model entity. 
    /// </summary>
    public class ModelEntityDescriptor 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelEntityDescriptor"/> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="members">The members.</param>
        public ModelEntityDescriptor(String entityName, IList<ModelEntityMember> members)
        {
            EntityName = entityName;
            Labels = members.Select(mem => mem.Label).ToList();
            Names = members.Select(mem => mem.Name).ToList();
            MembersSingleValue = members.OfType<ModelEntityMemberSingleValue>().Select(mem => mem).ToList();
            MembersMultipleValues = members.OfType<ModelEntityMemberMultipleValues>().Select(mem => mem).ToList();
        }

        /// <summary>
        /// Gets the Model entity name that this ModelInstance belongs.
        /// </summary>
        public string EntityName { get; private set; }

        /// <summary>
        /// Gets the <see cref="ModelInstance"/> properties/fields Labels. 
        /// By default, Labels have the same name that the Names. When there are relations
        /// between entities, the Labels are the text that the user sees and the Names 
        /// correnpond to the fields/properties names.
        /// </summary>
        /// <value>The <see cref="ModelInstance"/> properties/fields names.</value>
        public IList<string> Labels { get; private set; }

        /// <summary>
        /// Gets the <see cref="ModelInstance"/> properties/fields names. 
        /// </summary>
        /// <value>The <see cref="ModelInstance"/> properties/fields names.</value>
        public IList<string> Names { get; private set; }

        /// <summary>
        /// Gets the list of <see cref="ModelInstance"/> Members with single value. 
        /// </summary>
        /// <value>The list of <see cref="ModelInstance"/> members with single value.</value>
        public IList<ModelEntityMemberSingleValue> MembersSingleValue { get; private set; }

        /// <summary>
        /// Gets the list of <see cref="ModelInstance"/> Members with multiple values. 
        /// </summary>
        /// <value>The list of <see cref="ModelInstance"/> members with multiple values.</value>
        public IList<ModelEntityMemberMultipleValues> MembersMultipleValues { get; private set; }
    }
}