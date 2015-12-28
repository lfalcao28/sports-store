namespace MVCGenericLibrary.Model.Entity
{
    using System;
    using System.Collections.Generic;
    using BusinessRules;
    using Members;

    /// <summary>
    /// Implements <see cref="IModelEntityMembers{TEntity}"/> for a <typeparamref name="TEntity"/> using Reflection,
    /// including all public properties and fields as visible and editable as single value members <see cref="ModelEntityMemberSingleValue"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class ThirdPartyModelEntityMembers : IModelEntity
    {
        private IList<ModelEntityMember> _entityMembers = new List<ModelEntityMember>();
        public ThirdPartyModelEntityMembers()
        {
            // TODO
        }

        /// <summary>
        /// Gets the list of <see cref="ModelEntityMember"/> that represent the visible and updatable members for 
        /// <typeparam name="TEntity"/>.
        /// </summary>
        public IList<ModelEntityMember> Members
        {
            get { throw new NotImplementedException(); }
        }

        #region IModelEntity Members

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            throw new NotImplementedException();
        }

        public string Key
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}