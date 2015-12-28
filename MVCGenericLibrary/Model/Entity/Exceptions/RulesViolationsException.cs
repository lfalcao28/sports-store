// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RulesViolationsException.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Exception thrown when business rules exceptions exist on a model entity. Each rule violation is
//   indicated by a RuleViolation instance included in the RulesViolations collection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MVCGenericLibrary.Model.Entity.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;
    using BusinessRules;

    /// <summary>
    /// Exception thrown when business rules exceptions exist on a model entity. Each rule violation is 
    /// indicated by a <see cref="RuleViolation"/> instance included in the <see cref="RulesViolations"/>
    /// collection.
    /// </summary>
    [Serializable]
    public class RulesViolationsException : Exception
    {
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp

        /// <summary>
        /// Initializes a new instance of the <see cref="RulesViolationsException"/> class.
        /// </summary>
        public RulesViolationsException()
        {
        }

        public RulesViolationsException(string message, IEnumerable<RuleViolation> rulesViolations) : base(message)
        {
            RulesViolations = rulesViolations;
        }

        public RulesViolationsException(string message, IEnumerable<RuleViolation> rulesViolations, Exception inner)
            : base(message, inner)
        {
            RulesViolations = rulesViolations;
        }

        protected RulesViolationsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Gets collection of <see cref="RuleViolation"/> that caused the current exception.
        /// </summary>
        public IEnumerable<RuleViolation> RulesViolations { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (RuleViolation violation in RulesViolations) {
                sb.AppendLine(violation.ToString());
            }

            return sb.ToString();
        }
    }
}