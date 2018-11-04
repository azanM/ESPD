<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="msCostCenter.aspx.cs" Inherits="eSPD.msCostCenter" %>

<%@ Register assembly="DevExpress.Web.v14.1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" >
 <tr>
    <td>    
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
      <ContentTemplate>    
        <asp:Panel ID="pnlForm" runat="server">
            <table width="auto" align="left" >                              
                <tr>
                    <td>Company Code</td>
                    <td>:</td>
                    <td width="270">
                        <%--<dx:ASPxComboBox ID="cmbCompanyName" runat="server" 
                        DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith"
                        DataSourceID="ldCompanyName" TextField="companyCode" ValueField="coCd"
                        Width="270px" HorizontalAlign="Left" EnableIncrementalFiltering="True" AutoPostBack="true" >                                                     
                        </dx:ASPxComboBox>--%>
                        <asp:DropDownList ID="cmbCompanyName" runat="server" AutoPostBack="True" 
                                DataSourceID="ldCompanyName" DataTextField="companyCode" 
                                DataValueField="coCd" >                                
                        </asp:DropDownList>                       
                    </td>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                        ControlToValidate="cmbCompanyName" ErrorMessage=" Harus Diisi" Font-Bold="True" 
                        Font-Size="Small" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                        <%--<asp:LinqDataSource ID="ldCompanyName" runat="server" 
                        ContextTypeName="eSPD.dsSPDDataContext" EntityTypeName="" 
                        onselecting="cmbCompanyName_Selecting1" OrderBy="companyCode" 
                        Select="new (coCd, companyCode)" TableName="msKaryawans">--%>
                        <asp:LinqDataSource ID="ldCompanyName" runat="server" 
                        ContextTypeName="eSPD.dsSPDDataContext" EntityTypeName="" GroupBy="coCd" 
                        OrderGroupsBy="key" 
                        Select="new (key as coCd, it as msKaryawans, Min(companyCode) as companyCode)" 
                        TableName="msKaryawans">
                    </asp:LinqDataSource>
                    </asp:LinqDataSource>
                </tr>
                 <tr runat="server" id="trCostCenter" visible="false" >
                    <td>Cost Center</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="txtCostCenter" runat="server" Width="300px"  ></asp:TextBox>
                         <asp:Label ID="lblCostCenter" runat="server" ForeColor="#CC0000" Text="*"  ></asp:Label>
                        <asp:RequiredFieldValidator ID="valCostCenter" runat="server" 
                            ErrorMessage="Kode PA Tidak boleh kosong" ValidationGroup="a" 
                            ControlToValidate="txtCostCenter" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Cost Description</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="txtCostDesc" runat="server" Width="300px" ></asp:TextBox>
                         <asp:Label ID="lblCostDesc" runat="server" ForeColor="#CC0000" Text="*"  ></asp:Label>
                        <asp:RequiredFieldValidator ID="valCostDesc" runat="server" 
                            ErrorMessage="Kode PA Tidak boleh kosong" ValidationGroup="a" 
                            ControlToValidate="txtCostDesc" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:HiddenField runat="server" ID="hfCostId" />
                    </td>
                </tr>               
                <tr>
                    <td colspan="2" align="right" >
                                          
                    </td>
                    <td align="left" >
                      <asp:Button ID="btnSimpan" runat="server" Text="Simpan" 
                            onclick="btnSimpan_Click" ValidationGroup="a" /> &nbsp;   
                        <asp:Button ID="btnBatal" runat="server" Text="Batal" 
                            onclick="btnBatal_Click" />
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
    <asp:Button ID="btnTambah" runat="server" Text="Tambah" 
            onclick="btnTambah_Click" />
       </ContentTemplate>
        <%--<Triggers>                        
            <asp:AsyncPostBackTrigger ControlID="gvCostCenter" EventName="Click" />
        </Triggers>--%>
      </asp:UpdatePanel>
     </td>     
    <td> <asp:Label ID="notif" runat="server" Text="" Visible="false" ></asp:Label><asp:HiddenField ID="hfmode" runat="server" /></td>
 </tr>
 <tr>
    <td align="left" >        
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvCostCenter" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="true" 
                            onpageindexchanging="gvCostCenter_PageIndexChanging" >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                            <asp:TemplateField>
                            <ItemTemplate>
                             <asp:Label runat="server" Visible="false" id="lblIdCost" Text='<%# Eval("costId") %>' />
                            </ItemTemplate>
                            </asp:TemplateField>
                                <%--<asp:BoundField DataField="CompanyCode" HeaderText="Company Code" />--%>
                                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                <%--<asp:BoundField DataField="CostCenter" HeaderText="Cost Center" />--%>
                                <asp:BoundField DataField="CostDesc" HeaderText="Cost Desc" />                                                         
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click" >Edit</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDelete" runat="server" OnClick="lbDelete_Click" >Delete</asp:LinkButton>
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