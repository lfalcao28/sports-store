// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelEntityMember.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Defines the base class for Model entities members
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Entity.Members
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the base class for Model entities members
    /// </summary>
    public abstract class ModelEntityMember
    {
        /// <summary>
        /// Backing field for <see cref="AllowedValuesSelector"/>
        /// </summary>
        private Func<IList<string>> _allowedValuesSelector;

        /// <summary>
        /// Backing field for <see cref="AllowedVisibleValuesSelector"/>
        /// </summary>
        private Func<IList<string>> _allowedVisibleValuesSelector;

        /// <summary>
        /// Backing field for <see cref="Name"/> property.
        /// </summary>
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelEntityMember"/> class.
        /// </summary>
        protected ModelEntityMember()
        {
            Display = MemberDisplay.Details | MemberDisplay.Index | MemberDisplay.Edit | MemberDisplay.Create;
        }

        /// <summary>
        /// Gets or sets the member name.
        /// </summary>
        /// <value>The mamber name.</value>
        /// <remarks>If <see cref="Label"/> is null or empty when this property is set, it is also set with this value.</remarks>
        public String Name {
            get {
                return _name;
            }

            set { 
                _name = value; 
                if(String.IsNullOrEmpty(Label)) {
                    Label = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the member label. The label is the visible value that
        /// identifies the member in the UI.
        /// </summary>
        /// <value>The mamber label.</value>
        public String Label { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Func{TResult}"/> selector to obtain the string list
        /// of the member allowed values.
        /// </summary>
        /// <value>The allowed values selector.</value>
        public Func<IList<string>> AllowedValuesSelector
        {
            get
            {
                return _allowedValuesSelector ?? ( () => new string[0]);
            }

            set
            {
                _allowedValuesSelector = value;
                if (_allowedVisibleValuesSelector == null) {
                    _allowedVisibleValuesSelector = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Func{TResult}"/> selector to obtain the 
        /// string list of the member allowed visible (in the UI) values.
        /// </summary>
        /// <value>The allowed visible values selector.</value>
        public Func<IList<string>> AllowedVisibleValuesSelector
        {
            get
            {
                return _allowedVisibleValuesSelector ?? (() => new string[0]);
            }

            set {
                _allowedVisibleValuesSelector = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the ralated entity, or null when this member does not relate to any other entity.
        /// This property is <code>null</code> by default.
        /// </summary>
        /// <value>The name of the ralated entity.</value>
        public String RelatedEntityName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MemberDisplay"/> of the member.
        /// </summary>
        /// <value>The display.</value>
        /// <remarks>The default value is: <see cref="MemberDisplay.Index"/>, <see cref="MemberDisplay.Edit"/>, 
        /// <see cref="MemberDisplay"/>
        /// </remarks>
        public MemberDisplay Display { get; set; }
    }
}