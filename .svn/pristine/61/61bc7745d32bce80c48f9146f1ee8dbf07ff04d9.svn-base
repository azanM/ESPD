<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="frmRptSPD.aspx.cs" Inherits="eSPD.frmRptSPD" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%--<asp:ScriptManager runat="server" ID="scrptmngr"></asp:ScriptManager>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Panel runat="server" ID="pnlfilter">
<table>
<tr>
<td>No SPD</td>
<td>:</td>
<td><asp:TextBox runat="server" ID="tbNoSPD" /></td>
<td colspan="3" align="right" ><asp:Button runat="server" ID="btnView" Text="View Report" onclick="btnView_Click" /></td>
</tr>
</table>
</asp:Panel>

<div style="margin-top:20px"></div>
<asp:Panel runat="server" ID="pnlView">
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" HasToggleGroupTreeButton="false" GroupTreeStyle-ShowLines="false"
        AutoDataBind="true" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" ReportSourceID="CrystalReportSource1"  />
    <CR:CrystalReportSource ID="CrystalReportSource1" Report-FileName="~/crSPD.rpt" runat="server"  >
    </CR:CrystalReportSource>
</asp:Panel>
</asp:Content>
