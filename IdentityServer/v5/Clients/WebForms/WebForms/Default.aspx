<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebForms._Default" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <dl>

    <% 
        // hack
        var result = Context.GetOwinContext().Authentication.AuthenticateAsync("cookies").Result;


        foreach (var claim in result.Identity.Claims)
        {
    %>
   
            <dt>
                <%: claim.Type %>
            </dt>

            <dd>
                <%: claim.Value %>
            </dd>
        
    <%
        }
    %>

    <%
        foreach (var item in result.Properties.Dictionary)
        {
    %>
            <dt><%:item.Key %></dt>
            <dd><%:item.Value %></dd>
    <%
        }
    %>
    </dl>

</asp:Content>