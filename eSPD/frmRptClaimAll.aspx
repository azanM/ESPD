<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="frmRptClaimAll.aspx.cs" Inherits="eSPD.frmRptClaimAll" %>
<%@ Register assembly="DevExpress.Web.v14.1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
</asp:ScriptManager>--%>
<script type="text/javascript">
    // <![CDATA[
    var textSeparator = ";";
    function OnListBoxSelectionChanged(listBox, args) {
        if (args.index == 0)
            args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
        UpdateSelectAllItemState();
        UpdateText();
    }
    function UpdateSelectAllItemState() {
        IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
    }
    function IsAllSelected() {
        var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
        return checkListBox.GetSelectedItems().length == selectedDataItemCount;
    }
    function UpdateText() {
        var selectedItems = checkListBox.GetSelectedItems();
        checkComboBox.SetText(GetSelectedItemsText(selectedItems));
    }
    function SynchronizeListBoxValues(dropDown, args) {
        checkListBox.UnselectAll();
        var texts = dropDown.GetText().split(textSeparator);
        var values = GetValuesByTexts(texts);
        checkListBox.SelectValues(values);
        UpdateSelectAllItemState();
        UpdateText(); // for remove non-existing texts
    }
    function GetSelectedItemsText(items) {
        var texts = [];
        for (var i = 0; i < items.length; i++)
            if (items[i].index != 0)
                texts.push(items[i].text);
        return texts.join(textSeparator);
    }
    function GetValuesByTexts(texts) {
        var actualValues = [];
        var item;
        for (var i = 0; i < texts.length; i++) {
            item = checkListBox.FindItemByText(texts[i]);
            if (item != null)
                actualValues.push(item.value);
        }
        return actualValues;
    }
    // ]]>
    </script>    
<asp:Panel runat="server" ID="pnlFilter" >
<%--<dx:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="210px" runat="server" AnimationType="None">
        <DropDownWindowStyle BackColor="#EDEDED" />
        <DropDownWindowTemplate>
            <dx:ASPxListBox Width="100%" ID="listBox" DataSourceID="ldsUser" TextField="namaLengkap" ValueField="namaLengkap"  ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                runat="server">
                <Border BorderStyle="None" />
                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                <Items>
                    <dx:ListEditItem Text="(Select all)" />
                    <dx:ListEditItem Text="Chrome" Value="1" />
                    <dx:ListEditItem Text="Firefox" Value="2" />
                    <dx:ListEditItem Text="IE" Value="3" />
                    <dx:ListEditItem Text="Opera" Value="4" />
                    <dx:ListEditItem Text="Safari" Value="5" />
                </Items>
                <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
            </dx:ASPxListBox>

            <table style="width: 100%">
                <tr>
                    <td style="padding: 4px">
                        <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" style="float: right">
                            <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </DropDownWindowTemplate>
        <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" />
    </dx:ASPxDropDownEdit>
    <asp:LinqDataSource ID="ldsUser" runat="server" 
                        ContextTypeName="eSPD.dsSPDDataContext" EntityTypeName="" 
                        onselecting="LinqClaim_Selecting" OrderBy="namaLengkap" 
                        Select="new (nrp, namaLengkap)" TableName="msKaryawans">
                    </asp:LinqDataSource>--%>
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

<div style="margin-top:100px;" ></div>
<rsweb:ReportViewer ID="ReportViewer1" Height="500px" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="1089px">
        <LocalReport ReportPath="rptClaimAll.rdlc" >
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="dsClaimAll" />               
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Panel>


    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
        TypeName="eSPD.SPDDevDataSetTableAdapters.vReportClaimAllTableAdapter">
    </asp:ObjectDataSource>

    </asp:Content>

    