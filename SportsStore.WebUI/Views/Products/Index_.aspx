<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IndexModel>" %>

<%@ Import Namespace="MVCGenericLibrary.Controller.PresentationModels"%>
<%@ Import Namespace="MVCGenericLibrary.View.Helpers"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Listing
  <%= Model.EntityName %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <h2>
    Listing1
    <%=Model.EntityName%></h2>
  <% if (Model.ModelInstances.FirstOrDefault() == null)
     { %>
  <div class="error">
    Page contains no data</div>
  <% }
     else
     { %>
  <% foreach (var product in Model.ModelInstances)
     { %>
  <div class="item">
    <h3>
      <%= Html.DisplayMember(product.Members["Name"], "") %></h3>
    <%= Html.DisplayMember(product.Members["Description"], "") %>
    <h4>
      <%= Html.DisplayMember(product.Members["Price"], "") %></h4>
  </div>
  <% } %>
  <%-- <table>
    <tr>
      <th>
      </th>
      <% foreach (var prop in Model.Labels)
         {%>
      <th>
        <%= prop %>
      </th>
      <% } %>
    </tr>
    <% foreach (var modelInstance in Model.ModelInstances)
       { %>
    <tr>
      <th>
        <%= Html.ActionLink("Details", "Details", new { key = modelInstance.Key })%>
        |
        <%= Html.ActionLink("Edit", "Edit", new { key = modelInstance.Key })%>
        |
        <%= Html.ActionLink("Delete", "Delete", new { key = modelInstance.Key })%>
      </th>
      <% for (int i = 0; i < modelInstance.Members.Count; ++i)
         {%>
      <td>
        <%= Html.DisplayMemberInIndexView(modelInstance.Members[i]) %>
      </td>
      <% } %>
    </tr>
    <% } %>
  </table>--%>
  <% } %>
  <%--<%= Html.PageLinksPrevNext(Model.ModelInstances.PageIndex + 1, Model.ModelInstances.TotalPages, i => Url.RouteUrl("PagedIndexData", new { page = i }), "prev", "next")%>--%>
  <%= Html.PageLinksNumbered(Model.ModelInstances.PageIndex + 1, Model.ModelInstances.TotalPages, i => Url.RouteUrl("PagedIndexData", new { page = i }))%>
</asp:Content>
