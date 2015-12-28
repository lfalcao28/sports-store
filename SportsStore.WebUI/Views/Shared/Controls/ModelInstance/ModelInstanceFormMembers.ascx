<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ModelInstance>" %>
<%@ Import Namespace="MVCGenericLibrary.View.Helpers"%>
<%@ Import Namespace="MVCGenericLibrary.Controller.PresentationModels"%>
<%
  for (int i = 0; i < Model.Names.Count; ++i)
  { %>
<p>
  <% if (Model.CurrentViewMode == ModelInstance.ViewMode.Edit) { %>
        <%= Html.DisplayMemberInEditView(Model.Members[i], Model.Labels[i])%>
  <% }
     else { %>
        <%= Html.DisplayMemberInCreateView(Model.Members[i], Model.Labels[i])%>
  <% } %>
  <%= Html.ValidationMessage(Model.Members[i].Name, "*")%>
</p>
<% } %>
