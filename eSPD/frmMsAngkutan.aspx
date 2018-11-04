<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmMsAngkutan.aspx.cs" Inherits="eSPD.frmMsAngkutan" %>
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
                                        <asp:HiddenField ID="hdnRoleName" runat="server" /> <%--rolename awal--%>
                                        

                                    <%--</td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtRoleName"
                                        ErrorMessage="*" Font-Bold="True" Font-Size="Small" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                                </tr>--%>
                                <tr>
                                    <td>
                                        Angkutan</td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNrp" runat="server" ></asp:TextBox>
                                        <%--<asp:Label ID="lblCostDesc" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>--%>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Angkutan Tidak boleh kosong"
                                            ValidationGroup="a" ControlToValidate="txtNrp" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    
                                    </td>
                                <td>
                                        <asp:TextBox ID="txtID" runat="server" visible="false"  ></asp:TextBox>
                                     <asp:HiddenField runat="server" ID="hdnRoleID" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Status</td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbUser" runat="server" >
                                            <asp:ListItem Value="Aktif">Aktif</asp:ListItem>
                                            <asp:ListItem Value="Tidak Aktif">Tidak Aktif</asp:ListItem>
                                        </asp:DropDownList>
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
                                    <asp:BoundField DataField="nama" HeaderText="Angkutan" />
                                     <asp:BoundField DataField="status" HeaderText="Status" />
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
