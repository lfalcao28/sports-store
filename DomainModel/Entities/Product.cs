namespace SportsStore.DomainModel.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MVCGenericLibrary.Model.Entity;
    using MVCGenericLibrary.Model.Entity.BusinessRules;
    using MVCGenericLibrary.Model.Entity.Members;
    using MVCGenericLibrary.Model.Helpers;
    using Repositories;
    using StructureMap;

    public partial class Product : IModelEntity, IModelEntityMembers<Product> {
        #region IModelEntityMembers<Product> Members
        private static readonly ModelEntityMember[] _AllowedUpdatableModelMembers =
            new ModelEntityMember[] {
                new ModelEntityMemberSingleValue {
                                                     Name = "ProductID", 
                                                     ValueSelector = product => ((Product)product).ProductID.ToString(),
                                                     Display = MemberDisplay.Details
                                                 },
                new ModelEntityMemberSingleValue {
                                                     Name = "Name", ValueSelector = product => ((Product)product).Name
                                                 }, 
                new ModelEntityMemberSingleValue {
                                                     Name = "Description", ValueSelector = product => ((Product)product).Description 
                                                 },
                new ModelEntityMemberSingleValue {
                                                     Name = "Price", ValueSelector = product => ((Product)product).Price.ToString("c")
                                                 },
                new ModelEntityMemberSingleValue {
                                                     Name = "CategoryID", Label = "Category", 
                                                     ValueSelector = product => ((Product)product).CategoryID.ToString(),
                                                     VisibleValueSelector = product => ((Product)product).Category.CategoryName,
                                                     AllowedValuesSelector = () => ObjectFactory.GetInstance<ICategoryRepository>().GetAll().Select(c => c.CategoryID.ToString()).ToList(), 
                                                     AllowedVisibleValuesSelector = () => ObjectFactory.GetInstance<ICategoryRepository>().GetAll().Select(c => c.CategoryName).ToList()

                                                 }
                                        //new ModelEntityMemberMultipleValues {
                                        //    Name = "SoftwareVersions", 
                                        //    ValuesSelector = product => ((Product) product).SoftwareVersions.Select(sv => sv.SoftwareVersionDBId.ToString()).ToList(),
                                        //    VisibleValuesSelector = product => ((Product) product).SoftwareVersions.Select(sv => sv.Version.ToString()).ToList(),
                                        //    AllowedValuesSelector = () => DownloadServerDBDataContext.GetInstance().SoftwareVersions.Select(sv => sv.SoftwareVersionDBId.ToString()).ToList(),
                                        //    AllowedVisibleValuesSelector = () => DownloadServerDBDataContext.GetInstance().SoftwareVersions.Select(sv => sv.Version.ToString()).ToList(),
                                        //    RelatedEntityName = "SoftwareVersions"
                                        //}
                                    };

        /// <summary>
        /// Member names that are allowed to be updated.
        /// </summary>
        private static readonly String[] _AllowedUpdatableModelMemberNames = _AllowedUpdatableModelMembers.Names();

        public IList<ModelEntityMember> Members
        {
            get { return _AllowedUpdatableModelMembers; }
        }

        #endregion

        #region IModelEntity Members

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            IEnumerable<RuleViolation> violations = this.RulesViolations().StringMembersMustBeSet("Name", "Description").
                StringMembersSizes(_AllowedUpdatableModelMemberNames);
            return violations;
        }

        public string Key
        {
            get { return ProductID == 0 ? String.Empty : ProductID.ToString(); }
        }

        #endregion
    }
}