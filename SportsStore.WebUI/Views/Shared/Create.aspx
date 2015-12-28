<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ModelInstance>" %>
<%@ Import Namespace="MVCGenericLibrary.Controller.PresentationModels"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create a <%=Model.EntityName%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create a <%=Model.EntityName%></h2>
    <% Html.RenderPartial(@"Controls\ModelInstance\ModelInstanceForm"); %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

