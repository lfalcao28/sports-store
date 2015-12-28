<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ModelInstance>" %>
<%@ Import Namespace="MVCGenericLibrary.View.Helpers"%>
<%@ Import Namespace="MVCGenericLibrary.Controller.PresentationModels"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Model.EntityName%> Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Model.EntityName%> Details</h2>

    <fieldset>
        <legend><%=Model.EntityName%></legend>
        <%
            for (int i = 0; i < Model.Names.Count; ++i)
            { %>
        <p>
            <%= Html.DisplayMemberInDetailsView(Model.Members[i], Model.Labels[i]) %>
        </p>
        <% } %>
    </fieldset>
    <p>

        <%=Html.ActionLink("Edit", "Edit", new { key=Model.Key }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

