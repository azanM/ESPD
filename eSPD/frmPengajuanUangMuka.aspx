<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmPengajuanUangMuka.aspx.cs" Inherits="eSPD.frmPengajuanUangMuka" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel5" runat="server">
        <table style="width: 100%;">
            <tr>
                <td class="style1">
                    <strong>List Pengajuan Uang Muka</strong></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>No. SPD</td>
                <td>
                    <asp:TextBox ID="txtNoSpd" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>NRP</td>
                <td>
                    <asp:TextBox ID="txtNrp" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Nama</td>
                <td>
                    <asp:TextBox ID="txtNama" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Status</td>
                <td>

                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem>approve</asp:ListItem>
                        <asp:ListItem>pending</asp:ListItem>
                    </asp:DropDownList>

                </td>
            </tr>
            <tr>
                <td>Tanggal Penyelesaian</td>
                <td>
                    <asp:TextBox ID="txtTglSpd" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTglSpd_CalendarExtender" runat="server"
                        Enabled="True" TargetControlID="txtTglSpd">
                    </asp:CalendarExtender>
                    sampai dengan
                <asp:TextBox ID="txtTglSpdEnd" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                        Enabled="True" TargetControlID="txtTglSpdEnd">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btnListFind" runat="server" Text="Find"
                        OnClick="btnListFind_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                    <asp:Button ID="btnExportToExcel" runat="server" Text="Export To Excel" OnClick="btnExportToExcel_Click" />

                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="grvList" runat="server"
                         AutoGenerateColumns="False" OnRowDataBound="gvList_RowDataBound"
                        OnRowCommand="gvList_RowCommand" AllowPaging="True"
                        OnPageIndexChanging="gvList_PageIndexChanging"
                        CellPadding="4" ForeColor="#333333"
                        GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                            <asp:BoundField HeaderText="NRP" DataField="nrp" />
                            <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                            <asp:BoundField HeaderText="Uang Muka" DataField="UangMuka" />
                            <asp:BoundField HeaderText="Tanggal Penyelesaian" DataField="tglPenyelesaian" />
                            <asp:BoundField HeaderText="Status Uang Muka" DataField="status" />
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Button ID="lbApprove" runat="server" 
                                        CommandName="Approve" CommandArgument='<%# Eval("noSPD") %>'
                                        OnClientClick="if (!confirm('Are you sure?')) return false;"
                                        Text="Approve" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" CssClass="GridPager" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
