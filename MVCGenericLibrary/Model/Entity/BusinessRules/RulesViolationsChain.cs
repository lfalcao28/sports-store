// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RulesViolationsChain.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Defines the RulesViolationsChain type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.Model.Entity.BusinessRules
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Linq.Mapping;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class that mantains a sequence of <see cref="RulesViolations"/> and allows the concatenation
    /// of other rule violations <see cref="Concat(RuleViolation[])"/> and <see cref="Concat(System.Collections.Generic.IEnumerable{MVCGenericLibrary.Model.Entity.BusinessRules.RuleViolation})"/>
    /// and the chaining of other rule violations on string properties that must be set (<see cref="StringMembersMustBeSet"/>)
    /// and that exceed it's maximum allowed size (<see cref="StringMembersSizes"/>). 
    /// </summary>
    public class RulesViolationsChain : IEnumerable<RuleViolation>
    {
        private IEnumerable<RuleViolation> _rulesViolations;

        private readonly IModelEntity _instance;

        private IEnumerable<RuleViolation> RulesViolations
        {
            get { return _rulesViolations ?? new RuleViolation[0]; }

            set { _rulesViolations = value; }
        }

        public RulesViolationsChain(IModelEntity instance, IEnumerable<RuleViolation> rulesViolations)
        {
            _rulesViolations = rulesViolations;
            _instance = instance;
        }

        public RulesViolationsChain(IModelEntity instance)
        {
            _instance = instance;
        }

        public IEnumerator<RuleViolation> GetEnumerator()
        {
            return RulesViolations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return RulesViolations.GetEnumerator();
        }

        public RulesViolationsChain StringMembersMustBeSet(params string[] memberNames)
        {
            RulesViolations = RulesViolations.Concat(
                from prop in GetStringProperties(memberNames)
                let value = (String) (prop.GetValue(_instance, null))
                where value == null || value.Trim() == ""
                select new RuleViolation(prop.Name, String.Format("The {0} must be set.", prop.Name)));
            return this;
        }

        public RulesViolationsChain StringMembersSizes(params string[] memberNames)
        {
            IEnumerable<PropertyInfo> props = GetStringProperties(memberNames);
            var propsAttrs = from p in props
                             let attrs = p.GetCustomAttributes(typeof (ColumnAttribute), false)
                             where attrs.Count() == 1
                             select new
                                        {
                                            Property = p,
                                            ColumnAttr = (ColumnAttribute) attrs[0]
                                        };

            IEnumerable<RuleViolation> sizeViolations =
                from pa in propsAttrs
                let value = (String) (pa.Property.GetValue(_instance, null))
                let maxSize = GetMaxSizeForDbType(pa.ColumnAttr)
                where value != null && value.Trim().Length > maxSize
                select
                    new RuleViolation(pa.Property.Name,
                                      String.Format("The {0} must have a maximum of {1} chars.", pa.Property.Name,
                                                    maxSize));

            RulesViolations = RulesViolations.Concat(sizeViolations);
            return this;
        }

        public RulesViolationsChain Concat(params RuleViolation[] rulesViolations)
        {
            if (rulesViolations != null && rulesViolations[0] != null)
                RulesViolations = RulesViolations.Concat(rulesViolations);
            return this;
        }

        public RulesViolationsChain Concat(IEnumerable<RuleViolation> rulesViolations)
        {
            if (rulesViolations != null)
                RulesViolations = RulesViolations.Concat(rulesViolations);
            return this;
        }

        private IEnumerable<PropertyInfo> GetStringProperties(IEnumerable<string> propNames)
        {
            return (from p in
                        _instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty |
                                                          BindingFlags.Instance)
                    from propName in propNames
                    where p.Name == propName && p.PropertyType == typeof (String)
                    select p);
        }

        private static int GetMaxSizeForDbType(ColumnAttribute columnAttr)
        {
            Match m = new Regex(@"Char\((\d+)\)").Match(columnAttr.DbType);
            int maxSize;
            if (!(m.Groups.Count == 2 && int.TryParse((m.Groups[1].Captures[0].Value), out maxSize)))
                maxSize = int.MaxValue;

            return maxSize;
        }
    }
}