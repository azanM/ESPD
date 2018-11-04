<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmMsPlafonGolongan.aspx.cs" Inherits="eSPD.frmMsPlafonGolongan" %>

<%--<%@ Register Assembly="DevExpress.Web.v14.2" Namespace="DevExpress.Web"
    TagPrefix="dx" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:Panel ID="pnlForm" runat="server" Visible="false">
                            <table width="auto" align="left">
                                <%--<tr>
                                    <td>
                                        Role
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td width="600px">
                                        <asp:TextBox ID="txtRoleName" runat="server" Enabled="false"></asp:TextBox>--%>
                                <asp:HiddenField ID="hdnRoleName" runat="server" />
                                <%--rolename awal--%>
                                <asp:HiddenField ID="hdnRoleID" runat="server" />
                                <tr>
                                    <td>Jenis SPD</td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlJenisSPD" runat="server">
                                            <asp:ListItem Value="Dalam Negeri">Dalam Negeri</asp:ListItem>
                                            <asp:ListItem Value="Luar Negeri">Luar Negeri</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Wilayah</td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlWilayah" runat="server">
                                            <asp:ListItem Value="Seluruh Indonesia">Seluruh Indonesia</asp:ListItem>
                                            <asp:ListItem Value="Seluruh Dunia">Seluruh Dunia</asp:ListItem>
                                            <asp:ListItem Value="Asia(kecuali Jepang, Taiwan, Korea)">Asia(kecuali Jepang, Taiwan, Korea)</asp:ListItem>
                                            <asp:ListItem Value="Eropa">Eropa</asp:ListItem>
                                            <asp:ListItem Value="Jepang, Taiwan, Korea, USA, Afrika, Australia">Jepang, Taiwan, Korea, USA, Afrika, Australia</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Golongan</td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGolongan" runat="server">
                                            <asp:ListItem Value="I">I</asp:ListItem>
                                            <asp:ListItem Value="II">II</asp:ListItem>
                                            <asp:ListItem Value="III">III</asp:ListItem>
                                            <asp:ListItem Value="C0">C0</asp:ListItem>
                                            <asp:ListItem Value="C1">C1</asp:ListItem>
                                            <asp:ListItem Value="C2">C2</asp:ListItem>
                                            <asp:ListItem Value="C3">C3</asp:ListItem>
                                            <asp:ListItem Value="A0">A0</asp:ListItem>
                                            <asp:ListItem Value="A1">A1</asp:ListItem>
                                            <asp:ListItem Value="S0">S0</asp:ListItem>
                                            <asp:ListItem Value="S1">S1</asp:ListItem>
                                            <asp:ListItem Value="S2">S2</asp:ListItem>
                                            <asp:ListItem Value="IV">IV</asp:ListItem>
                                            <asp:ListItem Value="A2">A2</asp:ListItem>
                                            <asp:ListItem Value="S3">S3</asp:ListItem>
                                            <asp:ListItem Value="V">V</asp:ListItem>
                                            <asp:ListItem Value="VI">VI</asp:ListItem>
                                            <asp:ListItem Value="VII">VII</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>ID Plafon</td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPlafon" runat="server" DataSourceID="LinqDSPlafon"
                                            DataTextField="plafon" DataValueField="id" Width="230px">
                                        </asp:DropDownList>
                                         <asp:LinqDataSource ID="LinqDSPlafon" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                                EntityTypeName="" 
                                TableName="msPlafons" Where="status == @jenisSPD">
                                <WhereParameters>
                                    <asp:Parameter DefaultValue="aktif" Name="jenisSPD" Type="String" />
                                </WhereParameters>
                            </asp:LinqDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Harga</td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHarga" runat="server" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Harga Tidak boleh kosong"
                                            ValidationGroup="a" ControlToValidate="txtHarga"></asp:RequiredFieldValidator>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Custom, Numbers" TargetControlID="txtHarga">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Deskripsi</td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNrp" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hfCostId" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Status</td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbUser" runat="server">
                                            <asp:ListItem Value="AKTIF">Aktif</asp:ListItem>
                                            <asp:ListItem Value="TIDAK AKTIF">Tidak Aktif</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right"></td>
                                    <td align="left">
                                        <asp:Button ID="btnSimpan" runat="server" Text="Simpan" OnClick="btnSimpan_Click"
                                            ValidationGroup="a" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="notif" runat="server" Text="" Visible="true" ViewStateMode="Disabled"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSimpan" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <%--<asp:Button ID="btnTambah" runat="server" Text="Tambah" OnClick="btnTambah_Click" />
                        <asp:HiddenField ID="hfmode" runat="server" />--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td></td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hfmode" runat="server" />
                        <asp:Panel ID="pnlGrid" runat="server" Visible="true">
                            Search :
                            <asp:TextBox ID="txtFilterName" runat="server" Width="150px"></asp:TextBox>
                            &nbsp;
                            <asp:Button ID="btnFilter" runat="server" Text="Search" OnClick="btnFilter_Click" />
                            &nbsp;
                            <asp:Button ID="btnTambah" runat="server" Text="Add New" OnClick="btnTambah_Click" />
                            <br />
                            <asp:GridView ID="gvRole" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                ForeColor="#333333" GridLines="None" AllowPaging="true" OnPageIndexChanging="gvRole_PageIndexChanging">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Visible="false" ID="id" Text='<%# Eval("id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="jenisSPD" HeaderText="Jenis SPD" />
                                    <asp:BoundField DataField="wilayah" HeaderText="Wilayah" />
                                    <asp:BoundField DataField="golongan" HeaderText="Golongan" />
                                    <asp:BoundField DataField="plafon" HeaderText="ID Plafon" />
                                    <asp:BoundField DataField="harga" HeaderText="Harga" />
                                    <asp:BoundField DataField="deskripsi" HeaderText="Deskripsi" />
                                    <asp:BoundField DataField="status" HeaderText="Status" />
                                      <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Visible="false" ID="idPlafon" Text='<%# Eval("idPlafon") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click">Edit</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BackColor="#7C6F57" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#E3EAEB" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSimpan" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
