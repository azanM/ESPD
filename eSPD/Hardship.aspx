<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Hardship.aspx.cs" Inherits="eSPD.Hardship" %>

<%@ Register assembly="DevExpress.Web.v14.1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:Panel ID="inputForm" runat="server">
                            <table width="auto" align="left">
                                <tr>
                                    <td>Provinsi</td>
                                    <td>:</td>
                                    <td width="270">
                                        <asp:DropDownList ID="provinsiList" runat="server" AutoPostBack="True"
                                            DataTextField="Propinsi" DataValueField="Id" OnSelectedIndexChanged="Provinsi_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="provinsiList"
                                        ErrorMessage="*" Font-Bold="True" Font-Size="Small" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                                    <asp:HiddenField runat="server" ID="HFmasterHardshipId" />
                                </tr>
                                <tr>
                                    <td>Lokasi</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtLokasi" runat="server" Width="300px" AutoPostBack="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtLokasi" runat="server" ErrorMessage="Lokasi Tidak Boleh Kosong" ValidationGroup="a"
                                             ControlToValidate="txtLokasi" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Golongan</td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="golonganList" runat="server" AutoPostBack="true">
                                            <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                                            <asp:ListItem Text="I" Value="I"></asp:ListItem>
                                            <asp:ListItem Text="II" Value="II"></asp:ListItem>
                                            <asp:ListItem Text="III" Value="III"></asp:ListItem>
                                            <asp:ListItem Text="C0" Value="C0"></asp:ListItem>
                                            <asp:ListItem Text="C1" Value="C1"></asp:ListItem>
                                            <asp:ListItem Text="C2" Value="C2"></asp:ListItem>
                                            <asp:ListItem Text="C3" Value="C3"></asp:ListItem>
                                            <asp:ListItem Text="A0" Value="A0"></asp:ListItem>
                                            <asp:ListItem Text="A1" Value="A1"></asp:ListItem>
                                            <asp:ListItem Text="S0" Value="S0"></asp:ListItem>
                                            <asp:ListItem Text="S1" Value="S1"></asp:ListItem>
                                            <asp:ListItem Text="S2" Value="S2"></asp:ListItem>
                                            <asp:ListItem Text="IV" Value="IV"></asp:ListItem>
                                            <asp:ListItem Text="A2" Value="A2"></asp:ListItem>
                                            <asp:ListItem Text="S3" Value="S3"></asp:ListItem>
                                            <asp:ListItem Text="V" Value="V"></asp:ListItem>
                                            <asp:ListItem Text="VI" Value="VI"></asp:ListItem>
                                            <asp:ListItem Text="VII" Value="VII"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Harga</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtHarga" runat="server"></asp:TextBox> 
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Harga Tidak boleh kosong"
                                        ValidationGroup="a" ControlToValidate="txtHarga"></asp:RequiredFieldValidator> 
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Custom, Numbers" TargetControlID="txtHarga">
                                        </asp:FilteredTextBoxExtender>                                      
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btnSimpan" runat="server" Text="Simpan" OnClick="btnSimpan_Click"
                                            ValidationGroup="a" />
                                        &nbsp;
                                        <asp:Button ID="btnBatal" runat="server" Text="Batal" OnClick="btnBatal_Click" />
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
                        <asp:AsyncPostBackTrigger ControlID="btnBatal" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSimpan" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:Button ID="btnTambah" runat="server" Text="Tambah" OnClick="btnTambah_Click" />
                        <asp:HiddenField ID="hfmode" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMasterHardship" runat="server" AutoGenerateColumns="False" CellPadding="5"
                            ForeColor="#333333" GridLines="None" AllowPaging="true" OnPageIndexChanging="gvMasterHardship_PageIndexChanging">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Visible="false" ID="id" Text='<%# Eval("id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Propinsi" HeaderText="Provinsi" />
                                <asp:BoundField DataField="Lokasi" HeaderText="Lokasi" />
                                <asp:BoundField DataField="Golongan" HeaderText="Golongan"/>
                                <asp:BoundField DataField="Harga" HeaderText="Harga" />
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click">Edit</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDelete" runat="server" OnClick="lbDelete_Click">Delete</asp:LinkButton>
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
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSimpan" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
