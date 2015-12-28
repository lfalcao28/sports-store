// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerEnhancer.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Defines the ModeStateEnhancer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Controller.Helpers
{
    using System.Web.Mvc;
    using Model.Entity;
    using Model.Entity.BusinessRules;

    public static class ModeStateEnhancer
    {
        public static void AddRulesViolations(this ModelStateDictionary modelState, IModelEntity modelEntity)
        {
            foreach (RuleViolation issue in modelEntity.GetRuleViolations())
            {
                modelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
            }
        }
    }
}