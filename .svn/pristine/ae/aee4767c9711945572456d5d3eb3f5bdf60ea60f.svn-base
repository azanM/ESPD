<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmRequestView.aspx.cs" Inherits="frmRequestView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td>
                View SPD</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                Tanggal Keberangkatan</td>
            <td>
                <asp:TextBox ID="txtTglBerangkat" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnFind" runat="server" Text="Find" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvViewSPD" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" EnableModelValidation="True" ForeColor="#333333" 
                    GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField HeaderText="No SPD" />
                        <asp:BoundField HeaderText="Tempat Tujuan" />
                        <asp:BoundField HeaderText="Keperluan" />
                        <asp:BoundField HeaderText="Tanggal Berangkat" />
                        <asp:BoundField HeaderText="Tanggal Kembali" />
                        <asp:BoundField HeaderText="Status" />
                        <asp:TemplateField HeaderText="Detail">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbDetail" runat="server">Detail</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Revisi">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbRevisi" runat="server">Revisi</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Close">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbClose" runat="server">Close</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Claim">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbClaim" runat="server">Claim</asp:LinkButton>
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
</asp:Content>

