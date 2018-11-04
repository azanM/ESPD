<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmFinanceApp.aspx.cs" Inherits="eSPD.frmFinanceApp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel5" runat="server">
        <table style="width:100%;">
            <tr>
                <td class="style1">
                    <strong>List SPD's Approved by Finance</strong></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="right" width="50%">
                    Tanggal Keberangkatan</td>
                <td>
                    <asp:TextBox ID="txtTglSpd" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTglSpd_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtTglSpd">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnListFind" runat="server" Text="Find" 
                        onclick="btnListFind_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="grvList" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" ForeColor="#333333" 
                        GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>                           
                           <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                            <asp:BoundField HeaderText="NRP" DataField="nrp" />
                            <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                            <asp:BoundField HeaderText="Tempat Tujuan" DataField="cabangTujuan" />
                            <asp:BoundField HeaderText="Keperluan" DataField="keperluan" />
                            <asp:BoundField HeaderText="Tanggal Berangkat" DataField="tglBerangkat" />
                            <asp:BoundField HeaderText="Tanggal Kembali" DataField="tglKembali" />
                            <asp:BoundField HeaderText="Status" DataField="status" />
                            <asp:TemplateField HeaderText="Detail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbListRevisi" runat="server" onclick="lbListRevisi_Click">Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
