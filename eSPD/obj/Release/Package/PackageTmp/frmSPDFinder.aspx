<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmSPDFinder.aspx.cs" Inherits="eSPD.frmSPDFinder" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
       
            <table id="filter_container">
                <tr>
                    <td>Tipe</td>
                    <td>
                        <asp:DropDownList ID="ddlTipe" runat="server">
                            <asp:ListItem Value="spd">SPD</asp:ListItem>
                            <asp:ListItem Value="claim">Claim</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Nama</td>
                    <td>
                        <asp:TextBox ID="txtNama" runat="server"></asp:TextBox>
                    </td>
                    <td>Tanggal Berangkat</td>
                    <td>
                        <asp:TextBox ID="txtTglBerangkat" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="txtTglBerangkat_CalendarExtender" runat="server" 
                            Enabled="True" TargetControlID="txtTglBerangkat">
                        </asp:CalendarExtender>
                    </td>
                    <td>Status</td>
                    <td><asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="LinqDataSource1" 
                            DataTextField="Status" DataValueField="Status" 
                            ondatabound="ddlStatus_DataBound">

                        <asp:ListItem>All</asp:ListItem>

                        </asp:DropDownList>
                    </td>
                    <td>
                        
                        <asp:Button ID="btnFind" runat="server" Text="Cari" onclick="btnFind_Click" />
                        
                    </td>
                </tr>
            </table>            
            <br />
            <asp:Panel ID="PanelSPD" runat="server">
                <strong>List SPD</strong>
                <br />
                <asp:Label ID="lblStat" runat="server"></asp:Label>
                <asp:GridView ID="grSPD" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" ForeColor="#333333" 
                        GridLines="None"  AllowPaging="True" 
                    onpageindexchanging="grSPD_PageIndexChanging" >
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
                                    <asp:LinkButton ID="DetailSPD" OnClick="DetailSPD_Click" runat="server" >Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                            <asp:TemplateField HeaderText="Cancel">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbListSPDCancel" runat="server" onclick="lbListSPDCancel_Click">Cancel</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>   
                                                
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
            </asp:Panel>

            <asp:Panel ID="PanelClaim" runat="server">
                <strong>List Claim</strong>
                <br />
                <asp:Label ID="lblStat2" runat="server"></asp:Label>
                <asp:GridView ID="grClaim" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" 
                    GridLines="None"
                        AllowPaging="True" onpageindexchanging="grClaim_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                            <asp:BoundField HeaderText="NRP" DataField="nrp" />
                            <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                            <asp:BoundField HeaderText="Tujuan" DataField="cabangTujuan" />
                            <asp:BoundField HeaderText="Berangkat" DataField="tglBerangkat" />
                            <asp:BoundField HeaderText="Kembali" DataField="tglKembali" />                            
                            <asp:BoundField HeaderText="Total" datafield="Total"/>
                            <asp:BoundField HeaderText="Status" DataField="status" />
                            <asp:TemplateField Visible="false" >
                                <ItemTemplate>
                                    <asp:Literal ID="ltrlStatus" runat="server" Text='<%# Eval("Status") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="Detailga" OnClick="DetailGA_Click" runat="server" >Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                            <asp:TemplateField HeaderText="Cancel">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbListClaimCancel" runat="server" onclick="lbListClaimCancel_Click">Cancel</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>   
                      
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
            </asp:Panel>
            
            
            <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
                ContextTypeName="eSPD.dsSPDDataContext" EntityTypeName="" 
                onselecting="LinqDataSource1_Selecting" TableName="msStatus">
            </asp:LinqDataSource>
      
   
</asp:Content>
