<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="frmClaimApproval.aspx.cs" Inherits="eSPD.frmClaimApproval" %>




<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width:50%;
        }
    </style>

    <script language="javascript" type="text/javascript">
        var disp_setting = "toolbar=no,location=no,directories=no,menubar=no,";
        var disp_crf_join = "toolbar=no,location=no,directories=no,menubar=no,";
        disp_setting += "scrollbars=yes, width=800,height=500,resizable=yes";
        disp_crf_join += "scrollbars=yes,width=800, height=600, left=100, top=25";


        function popReport(noSPD) {

            window.open('http://trac54/Reports/Pages/Report.aspx?ItemPath=%2fReportSPD%2freportClaimPerItem&rs:Command=Render&rc:Parameters=false&noSPD=' + noSPD, "", disp_setting);
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<asp:Image ID="Image1" runat="server" Height="150px" ImageAlign="Middle" 
        ImageUrl="~/Style/claim.png" />--%>

    <img src="Style/claim.png" alt="Flow Claim" height="150px" />
    <asp:Panel ID="pnlAtasan" runat="server">
        <table style="width:100%;">
            <tr>
                <td align="right" class="style1">
                    <strong>Approval Claim SPD&nbsp; Oleh Atasan</strong></td>
                <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td align="right" width="50%">
                Tanggal Keberangkatan</td>
                <td>
                    <asp:TextBox ID="txtTglBerangkat" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTglBerangkat_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtTglBerangkat">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                &nbsp;</td>
                <td>
                    <asp:Button ID="btnFind" runat="server" Text="Find" onclick="btnFind_Click" Height="26px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvViewClaim" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" 
                    GridLines="None" onrowdatabound="gvViewClaim_RowDataBound" AllowPaging="True" 
                        onpageindexchanging="gvViewClaim_PageIndexChanging">
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
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Literal ID="ltrlStatus" runat="server" Text='<%# Eval("Status") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetail" runat="server" onclick="lbDetail_Click">Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Setuju">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbSetuju" runat="server" onclick="lbSetuju_Click">Setuju</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tolak">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbTolak" runat="server" onclick="lbTolak_Click">Tolak</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Revisi">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbRevisi" runat="server" onclick="lbRevisi_Click">Revisi</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                <asp:Label ID="lblStat" runat="server"></asp:Label></td>
                <td>
                &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlGA" runat="server">
        <table style="width:100%;">
            <tr>
                <td align="right" class="style1">
                    <strong>Approval Claim SPD Oleh GA</strong></td>
                <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td align="right" width="50%">
                    Filter : &nbsp&nbsp&nbsp&nbsp
                    <asp:DropDownList ID="DropDownList2" runat="server" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                        <asp:ListItem Text="Tanggal Keberangkatan" Value="tglBerangkat"></asp:ListItem>
                        <asp:ListItem Text="No SPD" Value="noSPD"></asp:ListItem>
                        <asp:ListItem Text="NRP" Value="nrp"></asp:ListItem>
                        <asp:ListItem Text="Nama" Value="namaLengkap"></asp:ListItem>
                    </asp:DropDownList>
                <%--Tanggal Keberangkatan--%></td>
                <td>
                    <asp:TextBox ID="txtTglBerangkatGA" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTglBerangkatGA_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtTglBerangkatGA">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                &nbsp;</td>
                <td>
                    <asp:Button ID="btnFindGA" runat="server" Text="Find" 
                        onclick="btnFindGA_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvViewClaimGA" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" 
                    GridLines="None" onrowdatabound="gvViewClaimGA_RowDataBound" 
                        AllowPaging="True" onpageindexchanging="gvViewClaimGA_PageIndexChanging">
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
                                    <asp:LinkButton ID="lbDetailGA" runat="server" onclick="lbDetailGA_Click">Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Setuju">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbSetujuGA" runat="server" onclick="lbSetujuGA_Click">Setuju</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tolak">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbTolakGA" runat="server" onclick="lbTolakGA_Click">Tolak</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Revisi">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbRevisiGA" runat="server" onclick="lbRevisiGA_Click">Revisi</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                              <asp:TemplateField HeaderText="Cancel">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbListGACancel" runat="server" onclick="lbListGACancel_Click">Cancel</asp:LinkButton>
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
                </td>
            </tr>
            <tr>
                <td>
                <asp:Label ID="lblStat2" runat="server"></asp:Label></td>
                <td>
                &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlFinance" runat="server">
        <table style="width:100%;">
            <tr>
                <td align="right" class="style1">
                    <strong>View Claim SPD Oleh Finance</strong></td>
                <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td align="right" width="50%">
                Tanggal Keberangkatan</td>
                <td>
                    <asp:TextBox ID="txtTglBerangkatFinance" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTglBerangkatFinance_CalendarExtender" 
                        runat="server" Enabled="True" TargetControlID="txtTglBerangkatFinance">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                &nbsp;</td>
                <td>
                    <asp:Button ID="btnFindFinance" runat="server" Text="Find" 
                        onclick="btnFindFinance_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvViewClaimFinance" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" 
                    GridLines="None" onrowdatabound="gvViewClaimFinance_RowDataBound" 
                        AllowPaging="True" onpageindexchanging="gvViewClaimFinance_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                            <asp:BoundField HeaderText="NRP" DataField="nrp" />
                            <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                            <asp:BoundField HeaderText="Tujuan" DataField="cabangTujuan" />                            
                            <asp:BoundField HeaderText="Total" datafield="Total"/>
                            <asp:BoundField HeaderText="Status" DataField="status" />
                            <asp:TemplateField Visible="false" >
                                <ItemTemplate>
                                    <asp:Literal ID="ltrlStatus" runat="server" Text='<%# Eval("Status") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailFinance" runat="server" 
                                        onclick="lbDetailFinance_Click">Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Setuju">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbSetujuFinance" runat="server" onclick="lbSetujuFinance_Click">Setuju</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField HeaderText="Tolak">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbTolakFinance" runat="server" onclick="lbTolakFinance_Click">Tolak</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <%--<asp:TemplateField HeaderText="Revisi">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbRevisiFinance" runat="server" onclick="lbRevisiFinance_Click">Revisi</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                <asp:Label ID="lblStat3" runat="server"></asp:Label></td>
                <td>
                &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlKasir" runat="server">
        <table style="width:100%;">
            <tr>
                <td align="right" class="style1">
                    <strong>View Claim SPD Oleh Kasir</strong></td>
                <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td align="right" width="50%">
                Tanggal Keberangkatan</td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="TextBox1">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                &nbsp;</td>
                <td>
                    <asp:Button ID="btnFindKasir" runat="server" Text="Find" 
                        onclick="btnFindKasir_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvViewKasir" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" 
                    GridLines="None" AllowPaging="True" 
                        onpageindexchanging="gvViewKasir_PageIndexChanging">
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
                            <asp:TemplateField HeaderText="Detail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailFKasir" runat="server" onclick="lbDetailFKasir_Click" 
                                       >Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Acknowledge">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbCloseKasir" runat="server" onclick="lbCloseKasir_Click">Setuju</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>   --%>                             
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" />
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
     <asp:Panel ID="pnlClaimPerorangan" runat="server">
        <table style="width:100%;">
            <tr>
                <td align="right" class="style1">
                    <strong>List SPD</strong></td>
                <td>
                    <asp:Label ID="lblUserName" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblNRP" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" dir="rtl" width="50%">
                    Tanggal Keberangkatan</td>
                <td>
                    <asp:TextBox ID="txtTglSpd" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTglSpd_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtTglSpd">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnListFind" runat="server" onclick="btnListFind_Click" 
                        Text="Find" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="grvList" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" ForeColor="#333333" 
                        GridLines="None" AllowPaging="True" 
                        onpageindexchanging="grvList_PageIndexChanging">
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
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbListRevisi" runat="server" onclick="lbListRevisi_Click">Buat Claim</asp:LinkButton>
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
                </td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlSPDPerorangan" runat="server">
        <table style="width:100%;">
            <tr>
                <td align="right" class="style1">
                    <strong>View Claim SPD Perseorangan</strong></td>
                <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td align="right" width="50%">
                Tanggal Keberangkatan</td>
                <td>
                    <asp:TextBox ID="txtTglBerangkatPribadi" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTglBerangkatPribadi_CalendarExtender" 
                        runat="server" Enabled="True" TargetControlID="txtTglBerangkatPribadi">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                &nbsp;</td>
                <td>
                    <asp:Button ID="btnFindPribadi" runat="server" Text="Find" onclick="btnFindPribadi_Click" 
                         />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                  
                    <asp:GridView ID="gvViewClaimPribadi2" runat="server" AllowPaging="True" 
                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                        GridLines="None" onpageindexchanging="gvViewClaimPribadi2_PageIndexChanging">
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
                             <asp:TemplateField HeaderText="Detail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailribadi" runat="server" onclick="lbDetailPribadi_Click">Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField> 
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
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
   
    <asp:Panel ID="Panel5" runat="server" Visible="False">
        <table style="width:100%;">
            <tr>
                <td align="right" class="style1">
                    No SPD</td>
                <td>
                    <asp:Label ID="lblNoSPD" runat="server"></asp:Label>
                    <asp:Label ID="lblStatus" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" class="style1">
                    <asp:Label ID="lblRevisi" runat="server" Text="Keterangan Revisi"></asp:Label>
                    <asp:Label ID="lblTolak" runat="server" Text="Keterangan Tolak" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRevisi" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnSimpan" runat="server" Text="Simpan" 
                        onclick="btnSimpan_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
</asp:Content>
