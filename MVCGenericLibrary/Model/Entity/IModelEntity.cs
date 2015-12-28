// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModelEntity.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Interface that all Business entities must implement to get the business rules violations
//   and the updatable properties names.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BusinessRules;

    /// <summary>
    /// Interface that all Business entities must implement to get the business rules violations
    /// and the updatable properties names.
    /// </summary>
    public interface IModelEntity
    {
        /// <summary>
        /// Gets the string representation of the entity key.
        /// </summary>
        /// <returns>The entity key</returns>
        String Key { get; }

        /// <summary>
        /// Gets the rule violations.
        /// </summary>
        /// <returns></returns>
        IEnumerable<RuleViolation> GetRuleViolations();
    }
}