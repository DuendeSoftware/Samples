<%@ Page Title="Call API" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CallApi.aspx.cs" Inherits="WebForms.CallApi" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<h1>API Response:</h1>
<pre><%: Payload %></pre>

</asp:Content>