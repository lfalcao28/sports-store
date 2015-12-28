// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleViolation.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Class that represents a business rule violation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Entity.BusinessRules
{
    /// <summary>
    /// Class that represents a business rule violation.
    /// </summary>
    public class RuleViolation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleViolation"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property where the violation ocurrs.</param>
        /// <param name="errorMessage">The rule violation error message.</param>
        public RuleViolation(string propertyName, string errorMessage)
        {
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleViolation"/> class.
        /// </summary>
        /// <param name="errorMessage">The rule violation error message.</param>
        public RuleViolation(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the error message, describing the violation.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Gets the name of the property where the violation ocurrs.
        /// </summary>
        /// <value>The name of the property where the violation ocurrs.</value>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("RuleViolation on property {0}: {1}", PropertyName, ErrorMessage);
        }
    }
}