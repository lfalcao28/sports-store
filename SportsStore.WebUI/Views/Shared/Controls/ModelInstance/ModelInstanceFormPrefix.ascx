<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ModelInstance>" %>
<%@ Import Namespace="MVCGenericLibrary.Controller.PresentationModels"%>
<%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
<form action="<%= Html.ViewContext.HttpContext.Request.RawUrl%>" method="post" enctype="multipart/form-data">
<fieldset>
    <legend>
        <%=Model.EntityName%></legend>
