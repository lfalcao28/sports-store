// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelEntityMemberMultipleValues.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Model entity member that has multiple values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Entity.Members
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Model entity member that has multiple values.
    /// </summary>
    public class ModelEntityMemberMultipleValues : ModelEntityMember {
        /// <summary>
        /// Backing field for <see cref="ValuesSelector"/>
        /// </summary>
        private Func<IModelEntity, IList<string>> _valuesSelector;

        /// <summary>
        /// Backing field for <see cref="VisibleValuesSelector"/>
        /// </summary>
        private Func<IModelEntity, IList<string>> _visibleValuesSelector;

        /// <summary>
        /// Gets or sets the values selector. This selector returns the <see cref="IList{T}"/> of strings 
        /// representation of member values given a <see cref="IModelEntity"/>.
        /// </summary>
        public Func<IModelEntity, IList<string>> ValuesSelector {
            get {
                return _valuesSelector;
            }

            set {
                _valuesSelector = value;
                if (VisibleValuesSelector == null) {
                    VisibleValuesSelector = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the visible values selector function. This function, given a <see cref="IModelEntity"/> returns
        /// the member values converted to a <see cref="IList{T}"/> of strings.
        /// </summary>
        public Func<IModelEntity, IList<string>> VisibleValuesSelector { 
            get
            {
                return _visibleValuesSelector;
            }

            set
            {
                _visibleValuesSelector = value;
                RelatedEntitiesKeysSelector = RelatedEntitiesKeysSelector ?? value;
            }
        }

        /// <summary>
        /// Gets or sets the related entities keys selector, if this member relates to another entity 
        /// (<see cref="ModelEntityMember.RelatedEntityName"/>.
        /// </summary>
        /// <value>The related entities keys selector.</value>
        /// <remarks>When <see cref="VisibleValuesSelector"/> is set, if this property is null, it is set with its value.</remarks>
        public Func<IModelEntity, IList<string>> RelatedEntitiesKeysSelector { get; set; }
    }
}