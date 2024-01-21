<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReadXml.aspx.cs" Inherits="ReadingWritingXML.ReadXml" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="ErrorLabel" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>

    <br />

    <h2>Students:</h2>

    <asp:GridView ID="GridView1" runat="server" CssClass="table" EmptyDataText="No data"></asp:GridView>

</asp:Content>
