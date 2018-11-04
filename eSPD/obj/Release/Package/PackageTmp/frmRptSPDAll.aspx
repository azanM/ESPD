<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="frmRptSPDAll.aspx.cs" Inherits="eSPD.frmRptSPDAll" %>
<%@ Register assembly="DevExpress.Web.v14.1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    
  
  <asp:Panel runat="server" ID="pnlFilter" >
<table style="text-align:left;" >
<tr>
<td>Nama Lengkap</td><td>:</td>
<td>
 <asp:DropDownCheckBoxes ID="checkBoxes1" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="checkBoxes_SelcetedIndexChanged" >
    <Style DropDownBoxBoxWidth="150px" />
    <Texts SelectBoxCaption="-Pilih Nama-" />
    <%--<Items>
        <asp:ListItem Text="Sunday" Value="7" />
        <asp:ListItem Text="Monday" Value="1" />
        <asp:ListItem Text="Tuesday" Value="2" />
        <asp:ListItem Text="Wednesday" Value="3" />
        <asp:ListItem Text="Thrusday" Value="4" />
        <asp:ListItem Text="Friday" Value="5" />
        <asp:ListItem Text="Saturday" Value="6" />
    </Items>--%>
</asp:DropDownCheckBoxes>
</td><td>
    <asp:Button runat="server" ID="btnView" Text="View" onclick="btnView_Click" 
        Width="75px" /></td>
</tr>
</table>
</asp:Panel>
<asp:Panel runat="server" ID="pnlView">

<div style="margin-top:100px;" >
    <rsweb:ReportViewer ID="ReportViewer1" Height="500px" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="1089px">
        <LocalReport ReportPath="rptSPDAll.rdlc" >
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="dsSPDAll" />               
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
        SelectMethod="GetData" 
        TypeName="eSPD.dsSPDALLNewTableAdapters.vRptSPDAllTableAdapter" 
        OldValuesParameterFormatString="original_{0}">
    </asp:ObjectDataSource>
</asp:Panel> 
</asp:Content>