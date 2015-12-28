// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelInstance.cs" company="CCISEL">
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
    using Model.Entity;
    using Model.Entity.Members;

    /// <summary>
    /// Model instance to pass to the Generic Index View
    /// </summary>
    public class ModelInstance
    {
        /// <summary>
        /// The <see cref="ModelEntityDescriptor"/> for the current <see cref="ModelInstance"/>.
        /// </summary>
        private readonly ModelEntityDescriptor _modelEntityDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelInstance"/> class.
        /// </summary>
        /// <param name="entityDescriptor">
        /// The <see cref="ModelEntityDescriptor"/> descriptor.
        /// </param>
        /// <param name="instance">
        /// The <see cref="IModelEntity"/> instance.
        /// </param>
        /// <param name="currentViewMode">
        /// The current View Mode.
        /// </param>
        public ModelInstance(ModelEntityDescriptor entityDescriptor, IModelEntity instance, ViewMode currentViewMode)
        {
            _modelEntityDescriptor = entityDescriptor;
            DomainModelInstance = instance;
            CurrentViewMode = currentViewMode;
            Key = instance.Key.Trim();
            MembersSingleValue = new MemberList<MemberSingleValue>(entityDescriptor.MembersSingleValue.Select(mmd => new MemberSingleValue(mmd, instance)));
            MembersMultipleValues = new MemberList<MemberMultipleValues>(entityDescriptor.MembersMultipleValues.Select(mmd => new MemberMultipleValues(mmd, instance)));
            Members = new MemberList<Member>(MembersSingleValue.Cast<Member>().Concat(MembersMultipleValues.Cast<Member>()));
            
        }

        public IModelEntity DomainModelInstance { get; private set; }

        /// <summary>
        /// Gets the EntityName that this ModelInstance belongs.
        /// </summary>
        public string EntityName
        {
            get { return _modelEntityDescriptor.EntityName; }
        }

        /// <summary>
        /// Gets the <see cref="ModelInstance"/> properties/fields Labels.
        /// By default, Labels have the same name that the Names. When there are relations
        /// between entities, the Labels are the text that the user sees and the Names
        /// correnpond to the fields/properties names.
        /// </summary>
        /// <value>
        /// The <see cref="ModelInstance"/> properties/fields names.
        /// </value>
        public IList<string> Labels
        {
            get { return _modelEntityDescriptor.Labels; }
        }

        /// <summary>
        /// Gets the <see cref="ModelInstance"/> properties/fields names. 
        /// </summary>
        /// <value>The <see cref="ModelInstance"/> properties/fields names.</value>
        public IList<string> Names
        {
            get { return _modelEntityDescriptor.Names; }
        }

        /// <summary>
        /// Gets the list of members with a single value <see cref="MemberSingleValue"/>. 
        /// </summary>
        /// <value>The list of members with a single value <see cref="MemberSingleValue"/>.</value>
        public MemberList<MemberSingleValue> MembersSingleValue { get; private set; }

        /// <summary>
        /// Gets the list of members with multiple values (<see cref="MembersMultipleValues"/>). 
        /// </summary>
        /// <value>The the list of members with multiple values (<see cref="MembersMultipleValues"/>).</value>
        public MemberList<MemberMultipleValues> MembersMultipleValues { get; private set; }

        /// <summary>
        /// Gets the list of all members (<see cref="Member"/> (with single and multiple values). 
        /// </summary>
        /// <value>The <see cref="ModelInstance"/> members.</value>
        public MemberList<Member> Members { get; private set; }

        /// <summary>
        /// Gets the model instance Key.
        /// </summary>
        /// <value>The model instance Key.</value>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the current view mode.
        /// </summary>
        /// <value>The current view mode.</value>
        public ViewMode CurrentViewMode { get; private set; }

        /// <summary>
        /// This enum constants define the <see cref="ModelInstance"/> view mode. The view mode defines
        /// the kind of view that a model instance is going to be passed to. 
        /// </summary>
        public enum ViewMode {
            /// <summary>
            /// Index view mode
            /// </summary>
            Index,

            /// <summary>
            /// Details view mode
            /// </summary>
            Details,

            /// <summary>
            /// Edit view mode
            /// </summary>
            Edit,

            /// <summary>
            /// Create view mode
            /// </summary>
            Create,

            /// <summary>
            /// Delete view mode
            /// </summary>
            Delete
        }

        /// <summary>
        /// This class defines a model instance Member. 
        /// A member has a <see cref="Name"/> and a <see cref="Label"/>.
        /// </summary>
        public abstract class Member
        {
            /// <summary>
            /// Backing field of <see cref="AllowedValues"/>.
            /// </summary>
            private IList<string> _allowedValues;

            /// <summary>
            /// Backing field of <see cref="AllowedVisibleValues"/>.
            /// </summary>
            private IList<string> _allowedVisibleValues;

            /// <summary>
            /// Initializes a new instance of the <see cref="Member"/> class.
            /// </summary>
            /// <param name="instance">The <see cref="IModelEntity"/> instance to get the values.</param>
            protected Member(IModelEntity instance)
            {
                Instance = instance;
            }

            /// <summary>
            /// Gets the <see cref="ModelInstance"/> member name.
            /// </summary>
            /// <value>The member name.</value>
            public string Name
            {
                get { return EntityMember.Name; }
            }

            /// <summary>
            /// Gets the member Label (visible to the user).
            /// By default the <see cref="Label"/> has the same value as <see cref="Name"/>. 
            /// When there are relations between entities, the Label is the text that the user sees and the Name
            /// corresponds to the member name.
            /// </summary>
            /// <value>The member name.</value>
            public string Label
            {
                get { return EntityMember.Label; }
            }

            /// <summary>
            /// Gets the member possible AllowedValues.
            /// </summary>
            /// <remarks>
            /// This values are used in edit/create views to fill the combobox values.
            /// </remarks>
            public IList<string> AllowedValues
            {
                get
                {
                    return _allowedValues ?? (_allowedValues = EntityMember.AllowedValuesSelector());
                }
            }

            /// <summary>
            /// Gets the member visible (to the User) values.
            /// </summary>
            /// <value>The visible values.</value>
            /// <remarks>
            /// This values are used in edit/create views to fill the combobox visible values.
            /// </remarks>
            public IList<string> AllowedVisibleValues
            {
                get { return _allowedVisibleValues ?? (_allowedVisibleValues = EntityMember.AllowedVisibleValuesSelector()); }
            }

            /// <summary>
            /// Gets or sets the name of the ralated entity, or null when this member does not relate 
            /// to any other entity.
            /// This property is <code>null</code> by default and retuns <see cref="EntityMember.RelatedEntityName"/>
            /// </summary>
            /// <value>The name of the ralated entity.</value>
            public String RalatedEntityName { get { return EntityMember.RelatedEntityName; } }

            protected abstract ModelEntityMember EntityMember { get; }

            /// <summary>
            /// Gets the <see cref="MemberDisplay"/> of the member.
            /// </summary>
            /// <value>The display.</value>
            /// <remarks>The default value is: <see cref="MemberDisplay.Index"/>, <see cref="MemberDisplay.Edit"/>, 
            /// <see cref="MemberDisplay.Create"/>
            /// </remarks>
            public MemberDisplay Display
            {
                get { return EntityMember.Display; }
            }

            protected IModelEntity Instance { get; private set; }
        }

        /// <summary>
        /// Defines a <see cref="ModelInstance"/> member with a single value
        /// </summary>
        public class MemberSingleValue : Member
        {
            /// <summary>
            /// Backing field for <see cref="EntityMemberDescriptor"/>
            /// </summary>
            private readonly ModelEntityMemberSingleValue _entityMemberDescriptor;

            public MemberSingleValue(ModelEntityMemberSingleValue entityMemberDescriptor, IModelEntity instance)
                : base(instance)
            {
                _entityMemberDescriptor = entityMemberDescriptor;
            }

            /// <summary>
            /// Gets the Memeber Value.
            /// </summary>
            /// <value>The member value.</value>
            public string Value
            {
                get { return _entityMemberDescriptor.ValueSelector(Instance); }
            }

            /// <summary>
            /// Gets the Memeber visible (to user) Value.
            /// </summary>
            /// <value>The member value.</value>
            public string VisibleValue
            {
                get { return _entityMemberDescriptor.VisibleValueSelector(Instance); }
            }

            public string RelatedEntityKey
            {
                get { return _entityMemberDescriptor.RelatedEntityKeySelector(Instance); }
            }

            /// <summary>
            /// Gets the member descriptor for the current model entity member.
            /// </summary>
            /// <value>The member descriptors.</value>
            protected override ModelEntityMember EntityMember
            {
                get { return _entityMemberDescriptor; }
            }


        }

        public class MemberMultipleValues : Member
        {
            private ModelEntityMemberMultipleValues _entityMemberDescriptor;

            public MemberMultipleValues(ModelEntityMemberMultipleValues entityMemberDescriptor, IModelEntity instance)
                : base(instance)
            {
                _entityMemberDescriptor = entityMemberDescriptor;
            }

            /// <summary>
            /// Gets the Memeber Values.
            /// </summary>
            /// <value>The member values.</value>
            public IList<string> Values
            {
                get { return _entityMemberDescriptor.ValuesSelector(Instance); }
            }

            /// <summary>
            /// Gets the Memeber visible (to user) Values.
            /// </summary>
            /// <value>The member visible values.</value>
            public IList<string> VisibleValues
            {
                get { return _entityMemberDescriptor.VisibleValuesSelector(Instance); }
            }

            public IList<string> RelatedEntitiesKeys
            {
                get { return _entityMemberDescriptor.RelatedEntitiesKeysSelector(Instance); }
            }

            protected override ModelEntityMember EntityMember
            {
                get { return _entityMemberDescriptor; }
            }
        }

        public class MemberList<TMember> : List<TMember> where TMember : Member
        {
            public MemberList(IEnumerable<TMember> collection) : base(collection) { }
            public TMember this[String memberName]
            {
                get { return this.SingleOrDefault(m => m.Name == memberName); }
            }
        }
    }
}