<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmRequestPrint.aspx.cs" Inherits="frmRequestPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
       <table style="width:100%;">
        <tr>
            <td>
                View SPD GA</td>
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
                No SPD</td>
            <td>
                <asp:TextBox ID="txtNoSPD" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Nama Lengkap</td>
            <td>
                <asp:TextBox ID="txtNamaLengkap" runat="server"></asp:TextBox>
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
                        <asp:BoundField HeaderText="NRP" />
                        <asp:BoundField HeaderText="Nama Lengkap" />
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
                        <asp:TemplateField HeaderText="Setuju">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbSetuju" runat="server">Setuju</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Revisi">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbRevisi" runat="server">Revisi</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Print">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbRevisi" runat="server">Print</asp:LinkButton>
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
     <asp:Panel ID="Panel4" runat="server">
        <table style="width:100%;">
            <tr>
                <td>
                    Revisi</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    Keterangan Revisi</td>
                <td>
                    <asp:TextBox ID="txtRevisi" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnKirim" runat="server" Text="Kirim Email Revisi" />
                </td>
            </tr>
        </table>
    </asp:Panel>

 
</asp:Content>

