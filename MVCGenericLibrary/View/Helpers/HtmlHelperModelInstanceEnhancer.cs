// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlHelperModelInstanceEnhancer.cs" company="CCISEL">
//   Centro de Cálculo do Instituto Superior de Engenharia de Lisboa - CCISEL  2009
// </copyright>
// <summary>
//   Extension methods to &lt;see cref="HtmlHelper{TModel}" /&gt; to fill data with &lt;see cref="ModelInstance" /&gt;.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MVCGenericLibrary.View.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Controller.PresentationModels;
    using Model.Entity.Members;

    /// <summary>
    /// Extension methods to <see cref="HtmlHelper{TModel}"/> to fill data with <see cref="ModelInstance"/>.
    /// </summary>
    public static class HtmlHelperModelInstanceEnhancer
    {

        private static void AppendLabel(string label, StringBuilder returnStr)
        {
            if (String.IsNullOrEmpty(label) == false)
            {
                returnStr.AppendFormat("<label for='{0}'>{0}</label>: ", label);
            }
        }

        public static string DisplayMemberInIndexView(this HtmlHelper helper, ModelInstance.Member member)
        {
            if((member.Display & MemberDisplay.Index) == 0) {
                return string.Empty;
            }
            return helper.DisplayMember(member, string.Empty);
        }

        public static string DisplayMemberInDetailsView(this HtmlHelper helper, ModelInstance.Member member, string label)
        {
            if ((member.Display & MemberDisplay.Details) == 0)
            {
                return string.Empty;
            }

            return helper.DisplayMember(member, label);
        }

        public static string DisplayMemberInEditView(this HtmlHelper helper, ModelInstance.Member member, string label)
        {
            if ((member.Display & MemberDisplay.Edit) == 0)
            {
                return string.Empty;
            }

            if ((member.Display & MemberDisplay.Readonly) != 0)
            {
                return helper.DisplayMember(member, label);
            }
            return helper.DisplayMemberEditable(member, label);
        }

        public static string DisplayMemberInCreateView(this HtmlHelper helper, ModelInstance.Member member, string label)
        {
            if ((member.Display & MemberDisplay.Create) == 0)
            {
                return string.Empty;
            }

            if ((member.Display & MemberDisplay.Readonly) != 0)
            {
                return helper.DisplayMember(member, label);
            }

            return helper.DisplayMemberEditable(member, label);
        }

        public static string DisplayMemberEditable(this HtmlHelper helper, ModelInstance.Member member, string label)
        {
            if (member is ModelInstance.MemberSingleValue)
            {
                return helper.DisplayMemberEditableSigleValue((ModelInstance.MemberSingleValue)member, label);
            }

            if (member is ModelInstance.MemberMultipleValues)
            {
                return helper.DisplayMemberEditableMultipleValues((ModelInstance.MemberMultipleValues)member, label);
            }

            throw new ArgumentException("Invalid Argument", "member");
        }

        public static string DisplayMemberEditableSigleValue(this HtmlHelper helper, ModelInstance.MemberSingleValue member, string label)
        {
            StringBuilder returnStr = new StringBuilder();
            AppendLabel(label, returnStr);

            if (member.AllowedValues.Count == 0)
            {
                return returnStr.Append(helper.TextBox(member.Name, member.Value)).ToString();
            }

            IEnumerable<SelectListItem> listItens = member.AllowedValues.Select((v, i) => new SelectListItem()
                                                                                              {
                                                                                                  Text = member.AllowedVisibleValues[i],
                                                                                                  Value = v,
                                                                                                  Selected = v == member.Value
                                                                                              });
            return returnStr.Append(helper.DropDownList(member.Name, listItens)).ToString();

        }

        public static string DisplayMemberEditableMultipleValues(this HtmlHelper helper, ModelInstance.MemberMultipleValues member, string label)
        {
            StringBuilder returnStr = new StringBuilder();
            AppendLabel(label, returnStr);
            IEnumerable<SelectListItem> listItens = member.AllowedValues.Select((v, i) => new SelectListItem()
                                                                                              {
                                                                                                  Text = member.AllowedVisibleValues[i],
                                                                                                  Value = v,
                                                                                                  Selected = member.Values.Contains(v)
                                                                                              });
            return returnStr.Append(helper.ListBox(member.Name, listItens, new { multiple = "multiple" })).ToString();
        }

        public static string DisplayMember(this HtmlHelper helper, ModelInstance.Member member, string label) {
            if(member is ModelInstance.MemberSingleValue) {
                return helper.DisplayMemberSingleValue((ModelInstance.MemberSingleValue) member, label);
            }

            if (member is ModelInstance.MemberMultipleValues) {
                return helper.DisplayMemberMultipleValues((ModelInstance.MemberMultipleValues)member, label);
            }

            throw new ArgumentException("Invalid Argument", "member");
        }

        public static string DisplayMemberSingleValue(this HtmlHelper helper, ModelInstance.MemberSingleValue member, string label)
        {
            StringBuilder returnStr = new StringBuilder();
            AppendLabel(label, returnStr);

            if (String.IsNullOrEmpty(member.RalatedEntityName))
            {
                returnStr.Append(helper.Encode(member.VisibleValue));
            }
            else {
                if (String.IsNullOrEmpty(member.VisibleValue) == false) {
                    returnStr.Append(helper.ActionLink(member.VisibleValue, "Details", member.RalatedEntityName, new { id = member.RelatedEntityKey }, null));
                }
            }

            return returnStr.ToString();
        }

        public static string DisplayMemberMultipleValues(this HtmlHelper helper, ModelInstance.MemberMultipleValues member, string label) {
            StringBuilder returnStr = new StringBuilder();
            AppendLabel(label, returnStr);

            String separator = " | ";
            for (int i = 0; i < member.VisibleValues.Count; i++) {
                if (String.IsNullOrEmpty(member.RalatedEntityName)) {
                    returnStr.Append(helper.Encode(member.VisibleValues[i]));
                }
                else {
                    returnStr.Append(helper.ActionLink(
                                         member.VisibleValues[i], 
                                         "Details", 
                                         member.RalatedEntityName,
                                         new { id = member.RelatedEntitiesKeys[i] }, 
                                         null));
                }

                returnStr.Append(separator);
            }

            return returnStr.Length == 0 ? String.Empty : returnStr.Remove(returnStr.Length - separator.Length, separator.Length).ToString();
        }
    }
}