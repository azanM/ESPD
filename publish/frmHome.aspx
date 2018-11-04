<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" CodeBehind="frmHome.aspx.cs" Inherits="eSPD.frmHome" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .hideIt {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <asp:TextBox ID="txtLogin" runat="server" OnTextChanged="txtLogin_TextChanged" Visible="false"></asp:TextBox>
   
</asp:Content>