<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ModelInstance>" %>
<%@ Import Namespace="MVCGenericLibrary.Controller.PresentationModels"%>
<% Html.RenderPartial(@"Controls\ModelInstance\ModelInstanceFormPrefix"); %>
<% Html.RenderPartial(@"Controls\ModelInstance\ModelInstanceFormMembers"); %>
<% Html.RenderPartial(@"Controls\ModelInstance\ModelInstanceFormSuffix"); %>