<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmRequestInput.aspx.cs" Inherits="eSPD.frmRequestInput" %>

<%--<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallback" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="DevExpress.Web.v14.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2 {
            height: 26px;
        }
        .style9 {
            width: 150px;
        }

        .hideIt {
            display: none !important;
        }
		table.gridtable {
			font-family: verdana,arial,sans-serif;
			font-size:11px;
			color:#333333;
			border-width: 1px;
			border-color: #666666;
			border-collapse: collapse;
		}
		table.gridtable th {
			border-width: 1px;
			padding: 8px;
			border-style: solid;
			border-color: #666666;
			background-color: #dedede;
		}
		table.gridtable td {
			border-width: 1px;
			padding: 8px;
			border-style: solid;
			border-color: #666666;
			background-color: #ffffff;
		}
	</style>

    <!-- This goes in the document HEAD so IE7 and IE8 don't cry -->
    <!--[if lt IE 9]>
	<style type="text/css">
		table.gradienttable th {
			filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#d5e3e4', endColorstr='#b3c8cc',GradientType=0 );
			position: relative;
			z-index: -1;
		}
		table.gradienttable td {
			filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ebecda', endColorstr='#ceceb7',GradientType=0 );
			position: relative;
			z-index: -1;
		}
	</style>
	<![endif]-->

    <!-- CSS goes in the document HEAD or added to your external stylesheet -->

    <!-- TIP: Generate your own CSS Gradients using this tool: http://www.colorzilla.com/gradient-editor/ -->

    <script type="text/javascript">
        $(function () {
            $("#<%=btnUpdate.ClientID%>").click();
            $("#<%=rdDalamNegeri.ClientID%>").on("change", function () {
                $("#<%=btnUpdate.ClientID%>").click();
            });

            $("#<%=rdbHO.ClientID%>").on("change", function () {
                $("#<%=btnUpdate.ClientID%>").click();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="nrpAtasanFirst" runat="server" />
    <asp:HiddenField ID="emailAtasanFirst" runat="server" />
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;" cellspacing="3px" celpadding="1px" border="0" class="mastertable">
            <tr>
                <td colspan="2" class="style3">
                    <strong><font size="4">Permintaan SPD</font></strong>
                </td>
                <td></td>
            </tr>
            <tr>
                <td align="center" colspan="3" class="style3" style="border-style: dashed; border-color: blue">
                    <strong><font size="3" color="blue">! Pilih dan lengkapi data dibawah ini secara berurutan</font></strong><br />
                    <strong><font size="3" color="blue">! Karyawan SERA HO memilih SERA Head Office, selain itu memilih Sub Business Unit</font></strong>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td class="style9">
                    Asal
                </td>
                <td colspan="2">
                    <script language="javascript" type="text/javascript" >
                        function CustomValidator1_ClientValidate(source, args) {
                            if (document.getElementById("<%= rdbHO.ClientID %>").checked || document.getElementById("<%= rdbCbg.ClientID %>").checked) {
                                args.IsValid = true;
                            }
                            else {
                                args.IsValid = false;
                                document.getElementById("<%= rdbHO.ClientID %>").focus();
                            }
                        }
                    </script>
                    <asp:RadioButton ID="rdbHO" runat="server" AutoPostBack="true" Text="SERA Head Office" GroupName="Asal" OnCheckedChanged="rdbHO_CheckedChanged" />
                    <asp:RadioButton ID="rdbCbg" runat="server" AutoPostBack="true" Text="Sub Business Unit" GroupName="Asal" OnCheckedChanged="rdbCbg_CheckedChanged" />
                    <%--<asp:HiddenField ID="hdfHOCabang" runat="server" />--%>
                    &nbsp;&nbsp;&nbsp;
                    <asp:CustomValidator id="CustomValidator1" runat="server" ValidationGroup="a" Display="Dynamic" ErrorMessage="Pilih SERA Head Office untuk Karyawan HO, atau pilih Sub Business Unit untuk Karyawan Cabang" ClientValidationFunction="CustomValidator1_ClientValidate"><label style="color:red">*</label><strong>Harus dipilih sesuai karyawan berasal</strong></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td class="style9">Nomor SPD
                </td>
                <td>
                    <asp:TextBox ID="txtNoSPD" runat="server" AutoPostBack="True" OnTextChanged="txtNoSPD_TextChanged"
                        ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td class="style9">NRP
                </td>
                <td class="style15" rowspan="2">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="style2" colspan="2">
                                        <asp:TextBox ID="txtNrp" runat="server" Enabled="false"></asp:TextBox>
                                        <asp:CheckBox ID="cmbDireksi" runat="server" Text="Direksi" AutoPostBack="true" OnCheckedChanged="cmbDireksi_CheckedChanged" CssClass="ruleTrigger" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtNamaLengkap" runat="server" colspan="2" Width="250px" Enabled="false"></asp:TextBox>
                                        <asp:DropDownList ID="ddlDireksi" runat="server" DataSourceID="IdsSec" Visible="false"
                                            DataTextField="namaLengkap" DataValueField="nrp" Width="210px"
                                            OnSelectedIndexChanged="ddlDireksi_SelectedIndexChanged"
                                            AutoPostBack="True" OnDataBound="ddlDireksi_DataBound">
                                        </asp:DropDownList>
                                        <div style="display: inline;">
                                            &nbsp;<asp:Label ID="ValDir" runat="server" ForeColor="#CC0000" Text="*" Visible="false"></asp:Label>
                                            <asp:RequiredFieldValidator ID="ValidateDir" runat="server" Enabled="false" ControlToValidate="ddlDireksi"
                                                ErrorMessage="Harus Diisi" Font-Bold="True" Font-Size="Small" ForeColor="Black"
                                                SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbDireksi" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDireksi" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDireksi" EventName="DataBound" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="style9">Nama Lengkap
                </td>
            </tr>
            <tr>
                <td class="style9">
                    <asp:Label ID="lblEmail" runat="server" Text="Email" Visible="False"></asp:Label>
                </td>
                <td width="auto">
                    <asp:TextBox ID="txtEmail" runat="server" Visible="False" Width="250px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Harus Diisi" Font-Bold="True" Font-Size="Small" SetFocusOnError="True"
                        Visible="False" ValidationGroup="a"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style9">Jabatan
                </td>
                <td class="style13" rowspan="4">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
	                     <ContentTemplate>
                             <asp:TextBox ID="txtJabatan" runat="server" Enabled="False"></asp:TextBox> <br />
                             <asp:TextBox ID="txtComCode" runat="server" Enabled="False"></asp:TextBox> <br />
                             <asp:TextBox ID="txtPA" runat="server" Enabled="False"></asp:TextBox> <br />
                             <asp:TextBox ID="txtPsa" runat="server" Enabled="False"></asp:TextBox> <br />
	                     </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlDireksi" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbDireksi" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>                    
                </td>
            </tr>
            <tr>
                <td class="style9">Company Code
                </td>
                <%--<td class="style13">
                </td>--%>
            </tr>
            <tr>
                <td class="style9">Personel Area
                </td>
                <%--<td class="style13">
                </td>--%>
            </tr>
            <tr>
                <td class="style9">Personel Sub Area
                </td>
                <%--<td class="style13">
                </td>--%>
            </tr>
            <tr>
                <td class="style9">No. Ponsel
                </td>
                <td align="left">
                    <asp:TextBox ID="txtNoHp" runat="server" Width="206px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="txtNoHp_FilteredTextBoxExtender" runat="server"
                        Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtNoHp">
                    </ajax:FilteredTextBoxExtender>
                    <asp:Label ID="Label14" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNoHp"
                        ErrorMessage="Harus Diisi" Font-Bold="True" Font-Size="Small" ForeColor="Black"
                        SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style9">Golongan
                </td>
                <td class="style13">
                    <asp:CheckBox ID="chkUbah" runat="server" AutoPostBack="True" OnCheckedChanged="chkUbah_CheckedChanged"
                        Text="Ubah Data" Visible="false" />
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:TextBox ID="txtGolongan" runat="server" Enabled="False" OnTextChanged="txtGolongan_TextChanged"
                                Visible="False" Width="20px">I</asp:TextBox>
                            <asp:DropDownList ID="cmbGolongan" runat="server" OnSelectedIndexChanged="cmbGolongan_SelectedIndexChanged"
                                AutoPostBack="True" AppendDataBoundItems="True" DataSourceID="LinqGol" DataTextField="Golongan" DataValueField="Golongan" Width="200px">
                                <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinqDataSource 
                                ID="LinqGol" runat="server" 
                                ContextTypeName="eSPD.dsSPDDataContext" 
                                EntityTypeName="" 
                                GroupBy="Golongan" Select="new (key as Golongan, it as ApprovalRules)" 
                                TableName="ApprovalRules">
                            </asp:LinqDataSource>
                            <asp:Label ID="Label6" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="cmbGolongan"
                                ErrorMessage="Harus Diisi" Font-Bold="True" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkUbah" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>

            <tr>
                <td class="style9">Posisi
                </td>
                <td class="style13">
                    <asp:DropDownList ID="ddlPosisi"
                        runat="server"
                        OnSelectedIndexChanged="ddlPosisi_SelectedIndexChanged"
                        AutoPostBack="true"
                        Width="200px">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                        <asp:ListItem Text="BM" Value="BM"></asp:ListItem>
                        <asp:ListItem Text="CFAT Div Head" Value="CFAT Div Head"></asp:ListItem>
                        <asp:ListItem Text="DIC" Value="DIC"></asp:ListItem>
                        <asp:ListItem Text="Direksi" Value="Direksi"></asp:ListItem>
                        <asp:ListItem Text="Finance Director" Value="Finance Director"></asp:ListItem>
                        <asp:ListItem Text="GM/OM/RM" Value="GM/OM/RM"></asp:ListItem>
                        <asp:ListItem Text="Kadept Sera HO/SBU HO" Value="Kadept"></asp:ListItem>
                        <asp:ListItem Text="Kadiv/OM" Value="Kadiv/OM"></asp:ListItem>
                        <asp:ListItem Text="Presiden Director" Value="Presiden Director"></asp:ListItem>
                        <asp:ListItem Text="Staff" Value="Staff"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label5" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlPosisi"
                        ErrorMessage="Harus Diisi" Font-Bold="True" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                </td>
            </tr>

            <tr>
                <td class="style9">Plafon
                </td>
                <td class="style2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlPlafon" runat="server" Visible="true" Width="393px" Height="89px">
                                <table>
                                    <tr>
                                        <td class="style2">Biaya Makan
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMakan" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Uang Saku
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUangSaku" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Transportasi
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransportasi" runat="server" Enabled="false" Width="291px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rdDalamNegeri" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="rbLuarNegeri" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlWilayah" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbGolongan" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="style9" rowspan="4">Tempat Tujuan
                </td>
                <td class="style13">
                    <asp:RadioButton ID="rdDalamNegeri" runat="server" GroupName="negeri" Text="Dalam Negeri"
                        AutoPostBack="True" OnCheckedChanged="rdDalamNegeri_CheckedChanged" CssClass="ruleTrigger" />
                    <asp:RadioButton ID="rbLuarNegeri" runat="server" GroupName="negeri" Text="Luar Negeri"
                        AutoPostBack="True" OnCheckedChanged="rbLuarNegeri_CheckedChanged" />
                </td>
            </tr>
            <tr>
                <td class="style11">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlWilayah" runat="server" AutoPostBack="True" DataSourceID="LinqDSLuarNegeri"
                                DataTextField="wilayah" DataValueField="wilayah" Enabled="False" OnSelectedIndexChanged="ddlWilayah_SelectedIndexChanged"
                                Width="230px">
                            </asp:DropDownList>
                            <asp:LinqDataSource ID="LinqDSLuarNegeri" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                                EntityTypeName="" GroupBy="wilayah" Select="new (key as wilayah, it as msGolonganPlafons)"
                                TableName="msGolonganPlafons" Where="jenisSPD == @jenisSPD">
                                <WhereParameters>
                                    <asp:Parameter DefaultValue="Luar Negeri" Name="jenisSPD" Type="String" />
                                </WhereParameters>
                            </asp:LinqDataSource>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rdDalamNegeri" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="rbLuarNegeri" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="cmbCompanyCode" runat="server" DataSourceID="LinqDSComCode"
                        DataTextField="companyCode" DataValueField="coCd" Height="20px" OnSelectedIndexChanged="cmbCompanyCode_SelectedIndexChanged"
                        Width="185px" AutoPostBack="True" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:Label ID="L1" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                    <asp:RequiredFieldValidator ID="Validate1" runat="server" ControlToValidate="cmbCompanyCode"
                        ErrorMessage="Harus Diisi" Font-Bold="True" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbTempatTujuan" runat="server" AutoPostBack="True" DataSourceID="LinqDSCabang"
                                DataTextField="leasingRental" DataValueField="kodePA" Height="20px" OnSelectedIndexChanged="cmbTempatTujuan_SelectedIndexChanged"
                                Width="185px" OnTextChanged="cmbTempatTujuan_TextChanged" OnDataBound="cmbTempatTujuan_DataBound">
                            </asp:DropDownList>
                            <asp:Label ID="L2" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                            <asp:RequiredFieldValidator ID="Validate2" runat="server" ControlToValidate="cmbTempatTujuan"
                                ErrorMessage="Harus Diisi" Font-Bold="True" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="TujuanHid" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbCompanyCode" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbSubArea" runat="server" DataSourceID="LinqDSSubCabang" DataTextField="pSubArea"
                                DataValueField="kodePSubArea" Height="20px" Width="185px" OnDataBound="cmbSubArea_DataBound"
                                AutoPostBack="True" OnSelectedIndexChanged="cmbSubArea_SelectedIndexChanged"
                                OnTextChanged="cmbSubArea_TextChanged">
                            </asp:DropDownList>
                            <asp:Label ID="L4" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                            <asp:RequiredFieldValidator ID="Validate4" runat="server" ControlToValidate="cmbSubArea"
                                ErrorMessage="Harus Diisi" Font-Bold="True" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="SaHid" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbTempatTujuan" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbCompanyCode" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTempatTujuan" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:LinqDataSource ID="LinqDSComCode" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" GroupBy="coCd" OrderGroupsBy="key" Select="new (key as coCd, it as msKaryawans, Min(companyCode) as companyCode)"
                        TableName="msKaryawans">
                    </asp:LinqDataSource>
                    <asp:LinqDataSource ID="LinqDSCabang" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" GroupBy="kodePA" OrderGroupsBy="key" Select="new (key as kodePA, it as v_karyawanTrac, Min(leasingRental) as leasingRental)"
                        TableName="v_karyawanTrac" Where="coCd == @coCd" OnSelecting="LinqDSCabang_Selecting">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="cmbCompanyCode" Name="coCd" PropertyName="SelectedValue"
                                Type="String" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                    <asp:LinqDataSource ID="LinqDSSubCabang" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" GroupBy="kodePSubArea" OrderGroupsBy="key" Select="new (key as kodePSubArea, it as v_karyawanTracs, Max(pSubArea) as pSubArea)"
                        TableName="v_karyawanTracs" Where="kodePA == @kodePA" OnSelecting="LinqDSSubCabang_Selecting">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="cmbTempatTujuan" Name="kodePA" PropertyName="SelectedValue"
                                Type="String" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                </td>
            </tr>
            <tr>
                <td class="style13">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblTmpLain" Text="Lain-lain" Visible="false" />
                            <asp:TextBox ID="txtTempatTujuanLain" runat="server" Visible="false" AutoPostBack="true"></asp:TextBox>
                            <asp:Label ID="L5" runat="server" ForeColor="#CC0000" Text="*" Visible="false"></asp:Label>
                            <asp:RequiredFieldValidator ID="Validate5" runat="server" ControlToValidate="txtTempatTujuanLain"
                                ErrorMessage="Harus diisi" Font-Bold="True" Font-Size="Small" SetFocusOnError="True"
                                ValidationGroup="a"></asp:RequiredFieldValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbCompanyCode" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <%--     <tr>
                <td align="left" style="height:140px">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlLabelDynamic" runat="server" Visible="true" Height="110px">
                                <asp:PlaceHolder ID="phLabelDynamic" runat="server"></asp:PlaceHolder>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbGolongan" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td align="left" width="400" style="height:140px">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server" Visible="true" Height="110px">
                                <asp:PlaceHolder ID="phDdlDynamic" runat="server"></asp:PlaceHolder>
                                            <asp:LinqDataSource ID="ldsApproval1" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                                                EntityTypeName="" OnSelecting="ldsApproval1_Selecting" OrderBy="namaLengkap"
                                                Select="new (nrp, namaLengkap)" TableName="v_atasans">
                                            </asp:LinqDataSource>
                                            <%--<dx:ASPxComboBox ID="cmbxApproval1" runat="server" DataSourceID="ldsApproval1" DropDownStyle="DropDown"
                                                HorizontalAlign="Left" IncrementalFilteringMode="StartsWith" TextField="namaLengkap"
                                                ValueField="nrp" Width="270px" EnableIncrementalFiltering="True" >
                                            </dx:ASPxComboBox>--%%%>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbGolongan" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>                                
                </td>
            </tr>--%>
            <tr>
                <td class="style9">Keperluan / Tugas
                </td>
                <td class="style13">
                    <asp:DropDownList ID="cmbKeperluan" runat="server" DataSourceID="LinqDSKeperluan"
                        DataTextField="keperluan" DataValueField="id" OnSelectedIndexChanged="cmbKeperluan_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Label ID="Label9" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                    &nbsp;&nbsp;<asp:LinqDataSource ID="LinqDSKeperluan" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" Select="new (id, keperluan)" TableName="msKeperluans">
                    </asp:LinqDataSource>
                    <asp:LinqDataSource ID="IdsSec" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" OnSelecting="IdsSec_Selecting" OrderBy="namaLengkap" Select="new (nrp, namaLengkap)"
                        TableName="msKaryawans">
                    </asp:LinqDataSource>
                </td>
            </tr>
            <tr>
                <td>Keterangan Keperluan
                </td>
                <td>
                    <asp:TextBox ID="txtKetKeperluan" runat="server" MaxLength="500"></asp:TextBox>
                    <asp:Label ID="L12" runat="server" ForeColor="#CC0000" Text="*" Visible="False"></asp:Label>
                    <asp:RequiredFieldValidator ID="Validate12" runat="server" ControlToValidate="txtKetKeperluan"
                        ErrorMessage="Harus Diisi" Font-Bold="True" Font-Size="Small" ValidationGroup="a"
                        Enabled="False"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="display:none !important">
                <td class="style9">Tiket
                </td>
                <td class="style13">
                    <asp:RadioButton ID="rbDicarikan" runat="server" Checked="True" Text="Dicarikan"
                        GroupName="Tiket" />
                    <br />
                    <asp:RadioButton ID="rbSendiri" runat="server" Text="Sendiri" GroupName="Tiket" />
                </td>
            </tr>
            <tr>
                <td class="style9">Hotel
                </td>
                <td class="style13">
                    <asp:CheckBox ID="cbHotel" runat="server" Checked="true" />
                </td>
            </tr>

            <tr>
                <td>Tanggal Berangkat
                </td>
                <td>
                    <asp:TextBox ID="txtTglBerangkat" runat="server" OnTextChanged="txtTglBerangkat_TextChanged"
                        AutoPostBack="True"></asp:TextBox>
                    <ajax:CalendarExtender ID="txtTglBerangkat_CalendarExtender" runat="server" TargetControlID="txtTglBerangkat">
                    </ajax:CalendarExtender>
                    <asp:Label ID="Label1" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTglBerangkat"
                        ErrorMessage="Harus Diisi" ValidationGroup="a" Font-Bold="True" Font-Size="Small"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    &nbsp; Jam
                    <asp:DropDownList ID="cmbJamBerangkat0" runat="server">
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;Menit
                    <asp:DropDownList ID="cmbMenitBerangkat0" runat="server">
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>26</asp:ListItem>
                        <asp:ListItem>27</asp:ListItem>
                        <asp:ListItem>28</asp:ListItem>
                        <asp:ListItem>29</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>31</asp:ListItem>
                        <asp:ListItem>32</asp:ListItem>
                        <asp:ListItem>33</asp:ListItem>
                        <asp:ListItem>34</asp:ListItem>
                        <asp:ListItem>35</asp:ListItem>
                        <asp:ListItem>36</asp:ListItem>
                        <asp:ListItem>37</asp:ListItem>
                        <asp:ListItem>38</asp:ListItem>
                        <asp:ListItem>39</asp:ListItem>
                        <asp:ListItem>40</asp:ListItem>
                        <asp:ListItem>41</asp:ListItem>
                        <asp:ListItem>42</asp:ListItem>
                        <asp:ListItem>43</asp:ListItem>
                        <asp:ListItem>44</asp:ListItem>
                        <asp:ListItem>45</asp:ListItem>
                        <asp:ListItem>46</asp:ListItem>
                        <asp:ListItem>47</asp:ListItem>
                        <asp:ListItem>48</asp:ListItem>
                        <asp:ListItem>49</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>51</asp:ListItem>
                        <asp:ListItem>52</asp:ListItem>
                        <asp:ListItem>53</asp:ListItem>
                        <asp:ListItem>54</asp:ListItem>
                        <asp:ListItem>55</asp:ListItem>
                        <asp:ListItem>56</asp:ListItem>
                        <asp:ListItem>57</asp:ListItem>
                        <asp:ListItem>58</asp:ListItem>
                        <asp:ListItem>59</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style9">
                    Tanggal Kembali
                    <%--<asp:Label ID="Label4" runat="server" Text="Tanggal Kembali" Width="162px"></asp:Label>--%>
                </td>
                <td>
                    <asp:TextBox ID="txtTglKembali" runat="server" AutoPostBack="True" OnTextChanged="txtTglKembali_TextChanged"></asp:TextBox>
                    <ajax:CalendarExtender ID="txtTglKembali_CalendarExtender" runat="server" TargetControlID="txtTglKembali">
                    </ajax:CalendarExtender>
                    <asp:Label ID="Label2" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTglKembali"
                        ErrorMessage="Harus Diisi" Font-Bold="True" Font-Size="Small" SetFocusOnError="True"
                        ValidationGroup="a"></asp:RequiredFieldValidator>
                    &nbsp; Jam
                    <asp:DropDownList ID="cmbJamKembali" runat="server">
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;Menit
                    <asp:DropDownList ID="cmbMenitKembali" runat="server">
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>26</asp:ListItem>
                        <asp:ListItem>27</asp:ListItem>
                        <asp:ListItem>28</asp:ListItem>
                        <asp:ListItem>29</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>31</asp:ListItem>
                        <asp:ListItem>32</asp:ListItem>
                        <asp:ListItem>33</asp:ListItem>
                        <asp:ListItem>34</asp:ListItem>
                        <asp:ListItem>35</asp:ListItem>
                        <asp:ListItem>36</asp:ListItem>
                        <asp:ListItem>37</asp:ListItem>
                        <asp:ListItem>38</asp:ListItem>
                        <asp:ListItem>39</asp:ListItem>
                        <asp:ListItem>40</asp:ListItem>
                        <asp:ListItem>41</asp:ListItem>
                        <asp:ListItem>42</asp:ListItem>
                        <asp:ListItem>43</asp:ListItem>
                        <asp:ListItem>44</asp:ListItem>
                        <asp:ListItem>45</asp:ListItem>
                        <asp:ListItem>46</asp:ListItem>
                        <asp:ListItem>47</asp:ListItem>
                        <asp:ListItem>48</asp:ListItem>
                        <asp:ListItem>49</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>51</asp:ListItem>
                        <asp:ListItem>52</asp:ListItem>
                        <asp:ListItem>53</asp:ListItem>
                        <asp:ListItem>54</asp:ListItem>
                        <asp:ListItem>55</asp:ListItem>
                        <asp:ListItem>56</asp:ListItem>
                        <asp:ListItem>57</asp:ListItem>
                        <asp:ListItem>58</asp:ListItem>
                        <asp:ListItem>59</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style9">Angkutan
                </td>
                <td class="style13">
                    <asp:DropDownList ID="cmbAngkutan" runat="server" DataSourceID="LinqDSAngkutan" DataTextField="nama"
                        DataValueField="id" AutoPostBack="True" OnSelectedIndexChanged="cmbAngkutan_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:LinqDataSource ID="LinqDSAngkutan" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" Select="new (id, nama)" TableName="msAngkutans">
                    </asp:LinqDataSource>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblAngkutanLain" runat="server" Visible="false" Text="Lain-lain"></asp:Label>
                            &nbsp;
                            <asp:TextBox ID="txtAngkutanLain" runat="server" Visible="false"></asp:TextBox>
                            <asp:Label ID="ValLain" runat="server" Visible="false" ForeColor="#CC0000" Text="*"></asp:Label>
                            <asp:RequiredFieldValidator ID="ValidateLain" runat="server" ControlToValidate="txtAngkutanLain"
                                ErrorMessage="Harus Diisi" Font-Bold="True" Enabled="false" Font-Size="Small"
                                SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbAngkutan" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <%--Lain-lain&nbsp;<asp:TextBox ID="txtAngkutanLain" runat="server"></asp:TextBox>
                    <asp:Label ID="ValLain" runat="server" Visible="false" ForeColor="#CC0000" Text="*"></asp:Label>
                    <asp:RequiredFieldValidator ID="ValidateLain" runat="server" ControlToValidate="txtAngkutanLain"
                        ErrorMessage="Harus Diisi" Font-Bold="True" Enabled="false" Font-Size="Small"
                        SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr style="display:none !important">
                <td class="style9">Penginapan
                </td>
                <td class="style13">
                    <asp:RadioButton ID="rbDisediakan" runat="server" Checked="True" Text="Disediakan"
                        OnCheckedChanged="rbDisediakan_CheckedChanged" GroupName="Penginapan" />
                    <br />
                </td>
            </tr>
            <tr style="display:none !important">
                <td class="style9">&nbsp;
                </td>
                <td class="style13">
                    <asp:RadioButton ID="rbTidakDisediakan" runat="server" Text="Tidak Disediakan" OnCheckedChanged="rbTidakDisediakan_CheckedChanged"
                        GroupName="Penginapan" />
                </td>
            </tr>
            <tr>
                <td class="style9">Uang Muka
                </td>
                <td class="style13">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUangMuka" runat="server" Enabled="False" Width="191px"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="txtUangMuka_FilteredTextBoxExtender" runat="server"
                                FilterType="Numbers" TargetControlID="txtUangMuka" ValidChars="1234567890">
                            </ajax:FilteredTextBoxExtender>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtTglKembali" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtTglBerangkat" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="style9">Cost Center Pembebanan
                </td>
                <td align="left">
                    <table>
                        <tr>
                            <td>
                                <asp:DropDownList
                                    ID="txtCostCenter"
                                    runat="server"
                                    DataTextField="costDesc"
                                    DataValueField="costDesc"
                                    DataSourceID="dsCost"
                                    AppendDataBoundItems="true" Width="400px">
                                    <asp:ListItem Value="" Text="- Select one -"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:LinqDataSource ID="dsCost" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                                    EntityTypeName="" Select="new (costId, costDesc)" TableName="msCosts">
                                </asp:LinqDataSource>
                                <asp:Label ID="Label8" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtCostCenter"
                                            ErrorMessage="Harus Diisi" Font-Bold="True" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                            </td>
                            <td><%---Nama Departemen &nbsp;
                                <asp:Label ID="Label11" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCostCenter"
                                    ErrorMessage="Harus Diisi" Font-Bold="True" Font-Size="Small" SetFocusOnError="True"
                                    ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="style9">Keterangan
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtKeterangan" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revTxtKeterangan" runat="server"             
                       ErrorMessage="* Maksimal 500 karakter"     
                       ValidationGroup="a"       
                       ValidationExpression="^([\S\s]{0,500})$"             
                       ControlToValidate="txtKeterangan"            
                       Display="Dynamic"><label style="color:red">* </label><strong>Maksimal 500 karakter</strong></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr style="display:none !important;">
                <td class="style9">Revisi
                </td>
                <td valign="top" class="style13">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="LinqDSRevisi"
                        HorizontalAlign="Left" Width="174px">
                        <Columns>
                            <asp:BoundField DataField="nrpRevisi" HeaderText="NRP" ReadOnly="True" SortExpression="nrpRevisi" />
                            <asp:BoundField DataField="keteranganRevisi" HeaderText="Alasan Revisi" ReadOnly="True"
                                SortExpression="keteranganRevisi" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td valign="top">
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="LinqDaSTolak"
                        HorizontalAlign="Left" Width="218px">
                        <Columns>
                            <asp:BoundField DataField="nrpTolak" HeaderText="NRP" ReadOnly="True" SortExpression="nrpTolak" />
                            <asp:BoundField DataField="keteranganTolak" HeaderText="Alasan Ditolak" ReadOnly="True"
                                SortExpression="keteranganTolak" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="style9">&nbsp;
                </td>
                <td class="style13">
                    <asp:LinqDataSource ID="LinqDSRevisi" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" Select="new (nrpRevisi, keteranganRevisi)" TableName="trRevisis"
                        Where="noSPD == @noSPD &&( status == @status || status == @status1)" OrderBy="dibuatTanggal desc">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="txtNoSPD" Name="noSPD" PropertyName="Text" Type="String" />
                            <asp:Parameter DefaultValue="8" Name="status" Type="Int32" />
                            <asp:Parameter DefaultValue="9" Name="status1" Type="Int32" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                </td>
                <td class="style9">
                    <asp:LinqDataSource ID="LinqDaSTolak" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" Select="new (nrpTolak, keteranganTolak)" TableName="trTolaks"
                        Where="noSPD == @noSPD &&( status == @status || status == @status1)">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="txtNoSPD" Name="noSPD" PropertyName="Text" Type="String" />
                            <asp:Parameter DefaultValue="12" Name="status" Type="Int32" />
                            <asp:Parameter DefaultValue="13" Name="status1" Type="Int32" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                </td>
            </tr>

            <tr>
                <td>Atasan
                </td>
                <td align="left" width="400">
                    <table width="400">
                        <tr>
                            <td width="270">
                                <%--<dx:ASPxComboBox ID="cmbxAtasan" runat="server" DataSourceID="ldAppAtasan" DropDownStyle="DropDown"
                                    HorizontalAlign="Left" IncrementalFilteringMode="StartsWith" TextField="namaLengkap"
                                    ValueField="nrp" Width="270px" EnableIncrementalFiltering="True" OnDataBound="cmbxAtasan_DataBound">
                                </dx:ASPxComboBox>--%>
                                <%--ixan --%>

                                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" CssClass="hideIt" />
                                <asp:UpdatePanel ID="UpdateForm" runat="server">
                                    <ContentTemplate>
                                        <div class="updateApproval">

                                            <asp:GridView ID="gvApproval" runat="server" AutoGenerateColumns="False" CssClass="gridtable">

                                                <Columns>
                                                    <%--<asp:TemplateField HeaderText="LN / DN">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl1" Text='<%# Eval("Tipe") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="HO / Cabang">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl2" Text='<%# Eval("TipeDetail") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Rule">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl3" Text='<%# Eval("RuleID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desk Posisi">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl4" Text='<%# Eval("Posisi") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Gol">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl5" Text='<%# Eval("Golongan") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desk">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbl6" Text='<%# Eval("Deskripsi") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Index">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblIndexLevel" Text='<%# Eval("IndexLevel") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Posisi" ItemStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblDeskripsi" Text='<%# Eval("Deskripsi") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Atasan">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txNrpApproval" runat="server" Text='<%# Eval("NrpApproval") %>' CssClass="ddlAtasan"></asp:TextBox>
                                                            <asp:TextBox ID="txNamaApproval" runat="server" Text='<%# Eval("Nama") %>' CssClass="hideIt"></asp:TextBox>
                                                            <asp:TextBox ID="txDesc" runat="server" Text='<%# Eval("Deskripsi") %>' CssClass="hideIt"></asp:TextBox>
                                                            <asp:TextBox ID="txIndexLevel" runat="server" Text='<%# Eval("IndexLevel") %>' CssClass="hideIt"></asp:TextBox>
                                                            <asp:TextBox ID="txEmail" runat="server" Text='<%# Eval("Email") %>' CssClass="hideIt"></asp:TextBox>
                                                            <asp:TextBox ID="txRuleID" runat="server" Text='<%# Eval("RuleID") %>' CssClass="hideIt"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                        </div>
                                        <script type="text/javascript">
                                            Sys.Application.add_load(function () {
                                                $(function () {
                                                    $("#<%=cmbxTujuan.ClientID%>,#<%=txtCostCenter.ClientID%>").select2({
                                                        placeholder: "Select one",
                                                        allowClear: true
                                                    });

                                                    $('.ddlAtasan').each(function () {
                                                        $(this).select2({
                                                            placeholder: "Pilih atasan ",
                                                            minimumInputLength: 1,
                                                            triggerChange: true,
                                                            allowClear: true,
                                                            ajax: {
                                                                url: '<%= ResolveUrl("~/frmRequestInput.aspx/GetAtasan") %>',
                                                                type: 'POST',
                                                                params: {
                                                                    contentType: 'application/json; charset=utf-8'
                                                                },
                                                                dataType: 'json',
                                                                cache: true,
                                                                data: function (bond, page) {
                                                                    return JSON.stringify({
                                                                        searchText: bond,
                                                                        additionalFilter: $("#<%=cmbDireksi.ClientID%>").is(':checked'),
                                                                        page: page,
                                                                        posisi: $(this).next().next().val()
                                                                    });
                                                                },
                                                                results: function (bond, page) {
                                                                    return {
                                                                        results: bond.d,
                                                                        more: (bond.d && bond.d.length == 10 ? true : false)
                                                                    }
                                                                }
                                                            },
                                                            initSelection: function (element, callback) {
                                                                callback({
                                                                    id: element.val(),
                                                                    text: element.next().val()
                                                                });
                                                            }
                                                        });
                                                    });

                                                });
                                            });

                                        </script>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="rdDalamNegeri" EventName="CheckedChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="rbLuarNegeri" EventName="CheckedChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDireksi" EventName="CheckedChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbGolongan" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlPosisi" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="rdbHO" EventName="CheckedChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="rdbCbg" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td width="120" align="left">
                                <%--<asp:Label ID="Label5" runat="server" ForeColor="#CC0000" Text="*"></asp:Label><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator10" runat="server" ControlToValidate="cmbxAtasan" ErrorMessage="Harus Diisi"
                                    Font-Bold="True" Font-Size="Small" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td class="style9">
                    Approval Tujuan
                    <%--<asp:Label ID="Label3" runat="server" Text="Approval Tujuan" Width="162px"></asp:Label>--%>
                </td>
                <td align="left" width="400">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <table width="400">
                                <tr>
                                    <td width="270">


                                        <asp:DropDownList
                                            ID="cmbxTujuan"
                                            runat="server"
                                            DataSourceID="ldAppTujuan"
                                            DataValueField="nrp"
                                            DataTextField="namaLengkap"
                                            AppendDataBoundItems="true" Width="300px">
                                            <asp:ListItem Value="" Text="- Select one -"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="Label7" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="cmbxTujuan"
                                            ErrorMessage="Harus Diisi" Font-Bold="True" SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                                    </td>
                                    <td width="120" align="left">
                                <%--        <asp:Label ID="Label10" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="cmbxTujuan"
                                            ErrorMessage=" Harus Diisi" Font-Bold="True" Font-Size="Small" SetFocusOnError="True"
                                            ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbTempatTujuan" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbCompanyCode" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbSubArea" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTempatTujuan" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:LinqDataSource ID="LinqDSNRPTujuan" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" OnSelecting="LinqDSNRPTujuan_Selecting" OrderBy="namaLengkap"
                        Select="new (nrp, namaLengkap)" TableName="msKaryawans">
                    </asp:LinqDataSource>
                    <asp:LinqDataSource ID="ldsUser" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" OnSelecting="ldsUser_Selecting1" OrderBy="namaLengkap" Select="new (nrp, namaLengkap)"
                        TableName="msKaryawans">
                    </asp:LinqDataSource>
                    <asp:LinqDataSource ID="ldAppAtasan" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" OnSelecting="ldAppAtasan_Selecting1" OrderBy="namaLengkap"
                        Select="new (nrp, namaLengkap)" TableName="v_atasans">
                    </asp:LinqDataSource>
                    <asp:LinqDataSource ID="ldAppTujuan" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" OrderBy="namaLengkap"
                        Select="new (nrp, namaLengkap)" TableName="v_atasan_tujuans">
                    </asp:LinqDataSource>
                    <%--<asp:LinqDataSource ID="ldAppTujuan" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" OnSelecting="ldAppTujuan_Selecting1" OrderBy="namaLengkap"
                        Select="new (nrp, namaLengkap)" TableName="v_atasans" Where="kodePSubArea == @kodePSubArea">
                        <WhereParameters>
                            <asp:ControlParameter Name="kodePSubArea" DefaultValue="0" ControlID="cmbSubArea"
                                Type="String" />
                        </WhereParameters>
                    </asp:LinqDataSource>--%>
                    <%--<asp:LinqDataSource ID="ldAppTujuan" runat="server" 
                         >                  --%>
                    <%--<asp:LinqDataSource 
                            ContextTypeName="eSPD.dsSPDDataContext" 
                            TableName="msKaryawans" 
                            Where="kodePSubArea == @kodePSubArea"
                            ID="ldAppTujuan" 
                            runat="server">
                          <WhereParameters>
                            <asp:ControlParameter 
                              Name="kodePSubArea" 
                              DefaultValue="0" 
                              ControlID="cmbSubArea" 
                              Type="String" />
                          </WhereParameters>
                        </asp:LinqDataSource>   --%>
                </td>
            </tr>
            <tr>
                <td class="style9">&nbsp;
                </td>
                <td colspan="2">
                    <asp:Label ID="Label12" runat="server" ForeColor="#CC0000" Text="*"></asp:Label>
                    &nbsp;<strong>Kolom Wajib Diisi</strong>
                </td>
            </tr>
            <tr>
                <td class="style9">
                    <asp:HiddenField ID="FLDgamode" runat="server" />
                </td>
                <td colspan="2" rowspan="1">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" ValidationGroup="a" />
                    &nbsp;<asp:Button ID="btnSubmit0" runat="server" Enabled="False" OnClick="btnSubmit0_Click"
                        Text="Submit" ValidationGroup="a" Style="height: 26px" />
                    &nbsp;<asp:Button ID="btnReset0" runat="server" OnClick="btnReset0_Click" Text="Reset" />
                    &nbsp;&nbsp;<asp:Label ID="lblStat" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

