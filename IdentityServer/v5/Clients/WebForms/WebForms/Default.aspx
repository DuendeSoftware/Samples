<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <dl>

    <% 
        var user = User as System.Security.Claims.ClaimsPrincipal;

        foreach (var claim in user.Claims)
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

    </dl>

</asp:Content>