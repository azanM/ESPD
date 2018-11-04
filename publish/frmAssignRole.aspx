<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmAssignRole.aspx.cs" Inherits="eSPD.frmAssignRole" %>
<%@ Register assembly="DevExpress.Web.v14.1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:100%;">
        <tr>
            <td colspan="3">
                <asp:Label ID="Label1" runat="server" Text="Assign Role" Font-Bold="True" 
                    Font-Size="Large" Visible="False"></asp:Label>
                <asp:Label ID="Label2" runat="server" 
                    style="font-weight: 700; font-size: large" 
                    Text="Anda Tidak Punya Hak Untuk Mengakses Halaman ini" Visible="False"></asp:Label>
                <br />
            </td>
            
        </tr>
        <tr>
            <td width="100" >
                Nama User</td>
            <td align="left">
                <dx:ASPxComboBox ID="cmbxUser" runat="server"
                    DataSourceID="ldsUser" dropDownStyle="DropDown" HorizontalAlign="Left" 
                        IncrementalFilteringMode="StartsWith" TextField="namaLengkap" ValueField="nrp" 
                        Width="270px" EnableIncrementalFiltering="True" 
                    oncallback="cmbxUser_Callback" AutoPostBack="True" 
                    onselectedindexchanged="cmbxUser_SelectedIndexChanged" Visible="False">
                </dx:ASPxComboBox>
                <asp:LinqDataSource ID="ldsUser" runat="server" 
                    ContextTypeName="eSPD.dsSPDDataContext" EntityTypeName="" 
                    Select="new (nrp, namaLengkap)" TableName="msKaryawans">
                </asp:LinqDataSource>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                User Role</td>
            <td align="left" width="400">
                <dx:ASPxComboBox ID="cmbxUserRole" runat="server" Width="150px" 
                    DataSourceID="ldsRole" ropDownStyle="DropDown" HorizontalAlign="Left" 
                        IncrementalFilteringMode="StartsWith" 
                    EnableIncrementalFiltering="True" TextField="namaRole" ValueField="id" 
                    Visible="False">
                </dx:ASPxComboBox>
                <asp:LinqDataSource ID="ldsRole" runat="server" 
                    ContextTypeName="eSPD.dsSPDDataContext" EntityTypeName="" 
                    Select="new (id, namaRole)" TableName="msRoles">
                </asp:LinqDataSource>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnAdd" runat="server" Text="ADD" onclick="btnAdd_Click" 
                    Visible="False" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td colspan="2" align="left">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvRole" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="#333333" GridLines="None" Visible="False">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="nrp" HeaderText="NRP" />
                                <asp:BoundField DataField="nama" HeaderText="Nama Lengkap" />
                                <asp:BoundField DataField="roleid" HeaderText="Role ID" />
                                <asp:BoundField DataField="role" HeaderText="Role" />
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDelete" runat="server" onclick="lbDelete_Click">Delete</asp:LinkButton>
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
                        <asp:AsyncPostBackTrigger ControlID="cmbxUser" 
                            EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>

    </table>
</asp:Content>
