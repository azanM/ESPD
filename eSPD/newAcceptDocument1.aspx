<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="newAcceptDocument1.aspx.cs" Inherits="eSPD.newAcceptDocument1" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:updatepanel id="UpdatePanel2" runat="server" childrenastriggers="true">
        <ContentTemplate>
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <table style="width: 100%;">
                    <tr>
                        <td width="80">Tanggal Terima</td>
                        <td>
                            <asp:TextBox ID="txtTglTerima" runat="server" Style="margin-left: 0px"></asp:TextBox>
                                <cc1:CalendarExtender ID="Calendar1"  PopupButtonID="imgPopup" runat="server" TargetControlID="txtTglTerima" >
    </cc1:CalendarExtender>
                      <%--        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTglTerima"   >
                            </ajax:CalendarExtender>--%>
                            <asp:RequiredFieldValidator
                                ID="rvTglTerima"
                                runat="server"
                                ControlToValidate="txtTglTerima"
                                ErrorMessage=" Harus diisi "
                                Font-Bold="True"
                                Enabled="False"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </td>

                    </tr>
                    <tr>
                        <td width="80">No.SPD</td>
                        <td>
                            <asp:TextBox ID="txtNoSPD" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                ID="rfNoSPD"
                                runat="server"
                                ControlToValidate="txtNoSPD"
                                ErrorMessage=" Harus diisi "
                                Font-Bold="True"
                                Enabled="False"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="80"></td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox4" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="80"></td>
                        <td>
                            <asp:TextBox ID="TextBox6" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox7" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox8" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="80"></td>
                        <td>
                            <asp:TextBox ID="TextBox9" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox10" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox11" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="80"></td>
                        <td>
                            <asp:TextBox ID="TextBox12" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox13" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox14" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="80"></td>
                        <td>
                            <asp:TextBox ID="TextBox15" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox16" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox17" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="80"></td>
                        <td>
                            <asp:TextBox ID="TextBox18" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox19" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox20" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="80"></td>
                        <td>
                            <asp:TextBox ID="TextBox21" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox22" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox23" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="80"></td>
                        <td>
                            <asp:TextBox ID="TextBox24" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox25" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox26" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="80"></td>
                        <td>
                            <asp:TextBox ID="TextBox27" runat="server" Style="margin-left: 0px"
                                Width="200px" Height="21px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox28" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox29" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;&nbsp;
                <asp:Button ID="btnProsesNew" runat="server" OnClick="btnProsesNew_Click"
                    Text="Submit" />
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnReset" runat="server"
                                Text="Reset" OnClick="btnReset_Click" />

                        </td>
                    </tr>
                    <tr>
                        <asp:Label ID="lblSave" runat="server" Visible="false" Text="Data sudah disave" ForeColor="Blue"></asp:Label>
                    </tr>
                          <tr>
                        <td width="80">For Dev hari sabtu</td>
                        <td>
                            <asp:TextBox ID="sabtu" runat="server" Style="margin-left: 0px"
                                Width="200px" Height="21px"></asp:TextBox>
                               <cc1:CalendarExtender ID="CalendarExtendersabtu"  PopupButtonID="imgPopup" runat="server" TargetControlID="sabtu" >
    </cc1:CalendarExtender>
                       
                       
                    </tr>
                    <tr> <td>hari minggu</td><td>
                            <asp:TextBox ID="minggu" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>  <cc1:CalendarExtender ID="CalendarExtenderMinggu"  PopupButtonID="imgPopup" runat="server" TargetControlID="minggu" >
    </cc1:CalendarExtender>
                        </td></tr>
                    <tr> <td>hari senin</td><td>
                            <asp:TextBox ID="senin" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                          <cc1:CalendarExtender ID="CalendarExtenderSenin"  PopupButtonID="imgPopup" runat="server" TargetControlID="senin" >
    </cc1:CalendarExtender>
                        </td>
                        <td>&nbsp;</td></tr>

                     <tr> <td>hari selasa</td><td>
                            <asp:TextBox ID="selasa" runat="server" Style="margin-left: 0px"
                                Width="200px"></asp:TextBox>
                           <cc1:CalendarExtender ID="CalendarExtenderSelasa"  PopupButtonID="imgPopup" runat="server" TargetControlID="selasa" >
    </cc1:CalendarExtender>
                        </td>
                        <td>&nbsp;</td></tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:updatepanel>
</asp:Content>
