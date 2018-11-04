<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="frmMsRole.aspx.cs" Inherits="eSPD.frmMsRole" %>

<%--
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
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
                                        <asp:HiddenField ID="hdnRoleName" runat="server" /> <%--rolename awal--%>
                                        <asp:HiddenField ID="hdnRoleID" runat="server" /> <%--roleid awal--%>

                                    <%--</td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtRoleName"
                                        ErrorMessage="*" Font-Bold="True" Font-Size="Small" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                                </tr>--%>
                                <tr>
                                    <td>
                                        NRP
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNrp" runat="server" AutoPostBack="true" OnTextChanged="txtNrp_TextChanged"></asp:TextBox>
                                        <%--<asp:Label ID="lblCostDesc" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>--%>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="NRP Tidak boleh kosong"
                                            ValidationGroup="a" ControlToValidate="txtNrp" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:HiddenField runat="server" ID="hfCostId" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Nama Lengkap
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <%--<asp:TextBox ID="txtCostCenter" runat="server" Width="300px" AutoPostBack="true"
                                            Enabled="False"></asp:TextBox>--%>
                                        <asp:DropDownList ID="cmbUser" runat="server" AutoPostBack="True" DataTextField="namaLengkap"
                                            DataValueField="Nrp" OnSelectedIndexChanged="cmbUser_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <%--<asp:LinqDataSource ID="dsKaryawan" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                                            EntityTypeName="" Select="new (nrp as nrp, namaLengkap as namaLengkap)" TableName="msKaryawan">
                                        </asp:LinqDataSource>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Posisi
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPosisi" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Posisi Tidak boleh kosong"
                                            ValidationGroup="a" ControlToValidate="txtPosisi" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                    </td>
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
            <td>
            </td>
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
                                    <asp:BoundField DataField="namaRole" HeaderText="Role" />
                                    <%--<asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click">Edit</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
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
