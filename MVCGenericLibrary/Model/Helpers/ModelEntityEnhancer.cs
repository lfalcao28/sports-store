// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelEntityEnhancer.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Sttaic class with extension methods for IModelEntity interface;
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Helpers
{
    using Entity;
    using Entity.BusinessRules;
    using Entity.Exceptions;

    /// <summary>
    /// Sttaic class with extension methods for <see cref="IModelEntity"/> interface.
    /// </summary>
    public static class ModelEntityEnhancer
    {
        /// <summary>
        /// Determines wheather the <see cref="IModelEntity"/> instance is valid (without business rules
        /// violations).
        /// </summary>
        /// <param name="modelEntity">The <see cref="IModelEntity"/> that this method extends.</param>
        /// <returns>
        /// <c>true</c>if <paramref name="modelEntity"/> instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValid(this IModelEntity modelEntity)
        {
            return modelEntity.GetRuleViolations().GetEnumerator().MoveNext() == false;
        }

        /// <summary>
        /// Estension method that crestes and returns a new <see cref="IModelEntity"/> for the
        /// given <paramref name="modelEntity"/>.
        /// </summary>
        /// <param name="modelEntity">The <see cref="RulesViolationsChain"/> that this method extends.</param>
        /// <returns>
        /// A new <see cref="RulesViolationsChain"/> for the given <paramref name="modelEntity"/>
        /// </returns>
        public static RulesViolationsChain RulesViolations(this IModelEntity modelEntity)
        {
            return new RulesViolationsChain(modelEntity);
        }

        /// <summary>
        /// Cheks the Rules Violations on the given <see cref="IModelEntity"/>. If rules violations exist,
        /// a <see cref="RulesViolationsException"/> is thrown.
        /// </summary>
        /// <param name="modelEntity">The model entity.</param>
        /// <exception cref="RulesViolationsException">
        /// Thrown if rules violations exist.
        /// </exception>
        public static void CheckRulesViolations(this IModelEntity modelEntity)
        {
            if (!modelEntity.IsValid())
            {
                throw new RulesViolationsException("Rule violations prevent saving", modelEntity.GetRuleViolations());
            }
        }
    }
}