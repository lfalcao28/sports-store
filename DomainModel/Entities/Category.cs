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

    public partial class Category : IModelEntity, IModelEntityMembers<Category> {
        #region IModelEntityMembers<Category> Members
        private static readonly ModelEntityMember[] _AllowedUpdatableModelMembers =
            new ModelEntityMember[] {
                new ModelEntityMemberSingleValue {
                                                     Name = "CategoryID", 
                                                     ValueSelector = category => ((Category)category).CategoryID.ToString(),
                                                     Display = MemberDisplay.Readonly | MemberDisplay.Details
                                                 },
                new ModelEntityMemberSingleValue {
                                                     Name = "CategoryName", ValueSelector = category => ((Category)category).CategoryName
                                                 }, 
                //new ModelEntityMemberSingleValue {
                //                                     Name = "CategoryID", Label = "Category", 
                //                                     ValueSelector = category => ((Category)category).CategoryID.ToString(),
                //                                     VisibleValueSelector = category => ((Category)category).Category.CategoryName,
                //                                     AllowedValuesSelector = () => ObjectFactory.GetInstance<ICategoryRepository>().GetAll().Select(c => c.CategoryID.ToString()).ToList(), 
                //                                     AllowedVisibleValuesSelector = () => ObjectFactory.GetInstance<ICategoryRepository>().GetAll().Select(c => c.CategoryName).ToList()

                //                                 }
                                        //new ModelEntityMemberMultipleValues {
                                        //    Name = "SoftwareVersions", 
                                        //    ValuesSelector = category => ((Category) category).SoftwareVersions.Select(sv => sv.SoftwareVersionDBId.ToString()).ToList(),
                                        //    VisibleValuesSelector = category => ((Category) category).SoftwareVersions.Select(sv => sv.Version.ToString()).ToList(),
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
            IEnumerable <RuleViolation> violations = this.RulesViolations().StringMembersMustBeSet("CategoryName").
                StringMembersSizes("CategoryName");
            return violations;
        }

        public string Key
        {
            get { return CategoryID == 0 ? String.Empty : CategoryID.ToString(); }
        }

        #endregion
    }
}