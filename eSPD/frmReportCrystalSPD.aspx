<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportCrystalSPD.aspx.cs"
    Inherits="eSPD.frmReportCrystalSPD" MasterPageFile="~/MasterPage.master" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<script language="javascript" type="text/javascript">
        var disp_setting = "toolbar=no,location=no,directories=no,menubar=no,";
        var disp_crf_join = "toolbar=no,location=no,directories=no,menubar=no,";
        disp_setting += "scrollbars=yes, width=800,height=500,resizable=yes";
        disp_crf_join += "scrollbars=yes,width=800, height=600, left=100, top=25";

        function popReport(noSPD) {
            window.open('http://trac54/ReportServer/Pages/ReportViewer.aspx?/ReportSPD1/reportSPDPerItem&rs:Command=Render&rc:Parameters=false&noSPD=' + noSPD, "", disp_setting);
            //            window.open('http://trac94/ReportServer/Pages/ReportViewer.aspx?/ReportVehiclePurchasing/reportAcountPayable&rs:Command=Render&rc:Parameters=false&SDATE=' + SDATE + '&EDATE=' + EDATE + '&SINVDATE=' + SINVDATE + '&EINVDATE=' + EINVDATE + '&SCLEARINGDATE=' + SCLEARINGDATE + '&ECLEARINGDATE=' + ECLEARINGDATE + '&OFFICERNAME=' + OFFICERNAME + '&PERIODEPO=' + PERIODEPO + '&ASTRA=' + ASTRA + '&VENDORNAME=' + VENDORNAME + '&BBN=' + BBN + '&POSTATUSID=' + POSTATUSID, "", disp_setting);
        }
    </script>--%>
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    View SPD GA
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Tanggal Keberangkatan
                </td>
                <td>
                    <asp:TextBox ID="txtTglBerangkat" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    No SPD
                </td>
                <td>
                    <asp:TextBox ID="txtNoSPD" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Nama Lengkap
                </td>
                <td>
                    <asp:TextBox ID="txtNamaLengkap" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvViewSPD" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        EnableModelValidation="True" ForeColor="#333333" GridLines="None" OnRowDataBound="gvViewSPD_RowDataBound">
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
                                    <asp:LinkButton ID="lbDetail" runat="server" OnClick="lbDetail_Click">Detail</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SPD">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbViewSPD" runat="server" OnClick="lbViewSPD_Click">View</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--    <asp:TemplateField HeaderText="Claim SPD">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbClaimSPD" runat="server">View</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
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
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlView">
        &nbsp;<CR:CrystalReportSource ID="CrystalReportSource1" runat="server" Report-FileName="~/crSPD.rpt">
        </CR:CrystalReportSource>
        &nbsp;<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ReportSourceID="CrystalReportSource1" />
    </asp:Panel>


</asp:Content>
