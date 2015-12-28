<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ModelInstance>" %>
<%@ Import Namespace="MVCGenericLibrary.Controller.PresentationModels"%>
    <p>
        <input type="submit" value="Save" />
    </p>
</fieldset>
<% Html.EndForm(); %>
<div>
    <%=Html.ActionLink("Back to List", "Index") %>
</div>
