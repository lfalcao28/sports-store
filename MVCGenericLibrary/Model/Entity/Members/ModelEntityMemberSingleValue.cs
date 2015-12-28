// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelEntityMemberSingleValue.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Model entity member that have a sigle value.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Entity.Members
{
    using System;

    /// <summary>
    /// Model entity member that have a sigle value.
    /// </summary>
    public class ModelEntityMemberSingleValue : ModelEntityMember
    {
        /// <summary>
        /// Backing field for <see cref="ValueSelector"/>
        /// </summary>
        private Func<IModelEntity, string> _valueSelector;

        private Func<IModelEntity, string> _visibleValueSelector;

        /// <summary>
        /// Gets or sets the value selector. This selector returns the string representation of member value 
        /// given a <see cref="IModelEntity"/>.
        /// </summary>
        /// <value>The value selector.</value>
        /// <remarks>When this property is set, if <see cref="VisibleValueSelector"/> is null is set with its value.</remarks>
        public Func<IModelEntity, string> ValueSelector {
            get {
                return _valueSelector;
            }

            set
            {
                _valueSelector = value;
                _visibleValueSelector = _visibleValueSelector ?? value;
            }
        }

        /// <summary>
        /// Gets or sets the visible value selector function. This function, given a <see cref="IModelEntity"/> returns
        /// the member value converted to a string.
        /// </summary>
        /// <value>The visible value selector.</value>
        /// <remarks>When <see cref="ValueSelector"/> is set, if this property is null, it is set with its value.
        /// When this property is set, if <see cref="RelatedEntityKeySelector"/> is null is set with its value.
        /// </remarks>
        public Func<IModelEntity, string> VisibleValueSelector
        {
            get {
                return _visibleValueSelector;
            }

            set {
                _visibleValueSelector = value;
                RelatedEntityKeySelector = RelatedEntityKeySelector ?? value;   
            }
        }

        /// <summary>
        /// Gets or sets the related entity key selector, if this member relates to another entity (<see cref="ModelEntityMember.RelatedEntityName"/>.
        /// </summary>
        /// <value>The related entity key selector.</value>
        /// <remarks>When <see cref="VisibleValueSelector"/> is set, if this property is null, it is set with its value.</remarks>
        public Func<IModelEntity, string> RelatedEntityKeySelector { get; set; }
    }
}