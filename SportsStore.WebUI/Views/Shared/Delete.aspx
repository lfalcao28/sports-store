<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ModelInstance>" %>
<%@ Import Namespace="MVCGenericLibrary.Controller.PresentationModels"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Delete confirmation
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <h2>Deleting <%=Model.EntityName%></h2>
  <div>
    <p>Please confirm you want to delete the <%= Model.EntityName %> with Id:
    <i> <%= Html.Encode(Model.Key) %>? </i> </p>
  </div>
  <% using (Html.BeginForm()) { %>
    <input type="submit" value="Delete" />
  <% } %>
  <p>
    <%=Html.ActionLink("Back to List", "Index") %>
  </p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
