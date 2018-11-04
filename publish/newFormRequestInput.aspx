<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="newFormRequestInput.aspx.cs" Inherits="eSPD.newFormRequestInput" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        h3 {
            border-bottom: 1px solid #e3e3e3;
            padding-bottom: 5px;
        }

            h3 span {
                float: right;
            }

        .bs-callout {
            -moz-border-bottom-colors: none;
            -moz-border-left-colors: none;
            -moz-border-right-colors: none;
            -moz-border-top-colors: none;
            border-color: #eee;
            border-image: none;
            border-radius: 3px;
            border-style: solid;
            border-width: 1px 1px 1px 5px;
            margin: 20px 0;
            padding: 20px;
        }

        .bs-callout-danger {
            border-left-color: #ce4844;
        }

        .bs-callout-info {
            border-left-color: #1b809e;
        }

        .bs-callout h4 {
            margin-bottom: 5px;
            margin-top: 0;
        }


        .bs-callout-danger h4 {
            color: #ce4844;
        }

        .bs-callout-info h4 {
            color: #1b809e;
        }

        .panel {
            background-color: #fff;
            border: 1px solid transparent;
            border-radius: 4px;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.05);
            margin-bottom: 20px;
        }

        .panel-default {
            border-color: #ddd;
        }

            .panel-default > .panel-heading {
                background-color: #f5f5f5;
                border-color: #ddd;
                color: #333;
            }

        .panel-heading {
            border-bottom: 1px solid transparent;
            border-top-left-radius: 3px;
            border-top-right-radius: 3px;
            padding: 10px 15px;
        }

        .panel-body {
            padding: 15px;
        }

        .panel-footer {
            background-color: #f5f5f5;
            border-bottom-left-radius: 3px;
            border-bottom-right-radius: 3px;
            border-top: 1px solid #ddd;
            padding: 10px 15px;
        }

        dl {
            margin-bottom: 20px;
            margin-top: 0;
        }

        .dl-horizontal dt {
            clear: left;
            float: left;
            overflow: hidden;
            text-align: right;
            text-overflow: ellipsis;
            white-space: nowrap;
            width: 160px;
        }

        dt {
            font-weight: 700;
        }

        dd, dt {
            line-height: 1.42857;
            margin: 5px;
        }

        .dl-horizontal dd {
            margin-left: 180px;
        }

        dd {
            margin-left: 0;
        }

        * {
            box-sizing: border-box;
        }

        input[type=text]:not(.select2-input), select {
            width: 380px;
            height: 30px;
            padding: 2px;
        }

            select.time {
                width: 50px;
            }

        textarea {
            width: 380px;
            padding: 2px;
        }

        [readonly="readonly"], [disabled="disabled"] {
            background-color: #DDD;
            border: 0px;
        }

        .hideIt {
            display: none !important;
        }

        table.gridtable {
            font-family: verdana,arial,sans-serif;
            font-size: 11px;
            color: #333333;
            border-width: 1px;
            border-color: #DDD;
            border-collapse: collapse;
        }

            table.gridtable th {
                border-width: 1px;
                padding: 8px;
                border-style: solid;
                border-color: #DDD;
                background-color: #f5f5f5;
            }

            table.gridtable td {
                border-width: 1px;
                padding: 8px;
                border-style: solid;
                border-color: #DDD;
                background-color: #ffffff;
            }

        .loading {
            background-color: white;
            bottom: 0;
            color: black;
            display: none;
            font-size: 4em;
            left: 0;
            opacity: 0.5;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=50)";
            /* IE 5-7 */
            filter: alpha(opacity=50);
            /* Netscape */
            -moz-opacity: 0.5;
            /* Safari 1.x */
            -khtml-opacity: 0.5;
            padding: 6em;
            position: fixed;
            right: 0;
            top: 0;
            z-index: 10000;
            float: right;
            text-align: right;
        }

        #errorbrowser {
            display: none;
        }
    </style>

    <!-- This goes in the document HEAD so IE7 and IE8 don't cry -->
    <!--[if lt IE 9]>
	<style type="text/css">
	  input[type=text], select, textarea {
            width: 380px;
            height: 30px;
            padding: 2px;
        }

        input[type=text].select2-input
        {
            width:100%;
        }

        #errorbrowser
        {
            display:block;
            position:fixed;
            bottom:100px;
            right:50px;
            background-color:white;
            z-index:1000;
            width:20%;
        }
	</style>
	<![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="UpdatePanelContent" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>


    <!--kalo ie gak support ini keluar-->
    <div class="bs-callout bs-callout-danger" id="errorbrowser">
        <h4>Browser anda sudah terlalu lawas.
        </h4>
        <p>Performa interface kemungkinan akan kurang maximal, dianjurkan untuk diupdate ke versi baru, terima kasih atas kerja samanya.</p>
    </div>

    <!--hidden field-->
    <asp:HiddenField ID="txNrp" runat="server" />
    <div class="loading">Please Wait</div>
    <h3>Permintaan SPD New <span>
        <asp:HyperLink ID="BackTolist" runat="server" NavigateUrl="~/newFormRequestList.aspx">Back To List</asp:HyperLink></span></h3>

    <div class="panel panel-default">
        <div class="panel-heading">Informasi Requester</div>
        <div class="panel-body">
            <dl class="dl-horizontal">
                <asp:Panel ID="pnlSekertaris" Visible="false" runat="server">
                    <dt>Direksi / Sendiri</dt>
                    <dd>
                        <asp:DropDownList
                            ID="ddlSelfOrDirect"
                            runat="server"
                            AppendDataBoundItems="true"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSelfOrDirect_SelectedIndexChanged">
                            <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                            <asp:ListItem Text="Untuk Direksi" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Untuk Sendiri" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="rvSelfOrDirect"
                            runat="server"
                            ControlToValidate="ddlSelfOrDirect"
                            ErrorMessage=" Harus dipilih "
                            Font-Bold="True"
                            Enabled="false"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </dd>
                    <dd runat="server" visible="false" id="ddDireksi">
                        <asp:DropDownList
                            ID="ddlDireksi"
                            runat="server"
                            AutoPostBack="true"
                            Visible="false" OnSelectedIndexChanged="ddlDireksi_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="rvDireksi"
                            runat="server"
                            ControlToValidate="ddlDireksi"
                            ErrorMessage=" Harus dipilih "
                            Font-Bold="True"
                            Enabled="false"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </dd>
                </asp:Panel>
                <dt>Asal
                </dt>
                <dd>
                    <asp:DropDownList
                        ID="ddlAsal"
                        runat="server"
                        AppendDataBoundItems="true"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlAsal_SelectedIndexChanged">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                        <asp:ListItem Value="HO">SERA Head Office</asp:ListItem>
                        <asp:ListItem Value="Cabang">Sub Business Unit</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rvddlAsal"
                        runat="server"
                        ControlToValidate="ddlAsal"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dd style="margin-bottom: 20px;">
                    <p><i>Sera HO untuk karyawan HO & Sub Business Unit untuk karyawan cabang.</i></p>
                </dd>
                <dt>No SPD</dt>
                <dd>
                    <asp:TextBox ID="txNoSPD" runat="server" ReadOnly="true"></asp:TextBox></dd>
                <dt>Nama Lengkap</dt>
                <dd>
                    <asp:TextBox ID="txNamaLengkap" runat="server" ReadOnly="true" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rvNamaLengkap"
                        runat="server"
                        ControlToValidate="txNamaLengkap"
                        ErrorMessage=" Harus diisi "
                        Font-Bold="True"
                        Enabled="false"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dt>Email</dt>
                <dd>
                    <asp:TextBox ID="txEmail" runat="server" ReadOnly="true" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rvEmail"
                        runat="server"
                        ControlToValidate="txEmail"
                        ErrorMessage=" Harus diisi "
                        Font-Bold="True"
                        Enabled="false"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dt>Jabatan</dt>
                <dd>
                    <asp:TextBox ID="txJabatan" runat="server" ReadOnly="true"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rvJabatan"
                        runat="server"
                        ControlToValidate="txJabatan"
                        ErrorMessage=" Harus diisi "
                        Font-Bold="True"
                        Enabled="false"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dt>Company Code</dt>
                <dd>
                    <asp:TextBox ID="txCompanyCode" runat="server" ReadOnly="true"></asp:TextBox></dd>
                <dt>Personel Area</dt>
                <dd>
                    <asp:TextBox ID="txPersonalArea" runat="server" ReadOnly="true"></asp:TextBox></dd>
                <dt>Personel Sub Area</dt>
                <dd>
                    <asp:TextBox ID="txPersonalSubArea" runat="server" ReadOnly="true"></asp:TextBox></dd>
                <dt>No Ponsel</dt>
                <dd>
                    <asp:TextBox ID="txNoPonsel" runat="server" MaxLength="15"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="ftNoPonsel" runat="server"
                        TargetControlID="txNoPonsel"
                        FilterType="Numbers" />
                    <asp:RegularExpressionValidator ID="ReNoPonsel"
                        ControlToValidate="txNoPonsel"
                        ValidationExpression="^\d+"
                        Display="Dynamic"
                        Font-Bold="true"
                        runat="server"><b>No ponsel hanya diperbolehkan nomor.</b></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator
                        ID="rvNoPonsel"
                        runat="server"
                        ControlToValidate="txNoPonsel"
                        ErrorMessage=" Harus diisi "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dt>Golongan</dt>
                <dd>
                    <asp:DropDownList
                        ID="ddlGolongan"
                        runat="server"
                        AutoPostBack="True"
                        AppendDataBoundItems="True"
                        DataSourceID="LinqGol"
                        DataTextField="Golongan"
                        DataValueField="Golongan" OnSelectedIndexChanged="ddlGolongan_SelectedIndexChanged">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:LinqDataSource
                        ID="LinqGol" runat="server"
                        ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName=""
                        GroupBy="Golongan" Select="new (key as Golongan, it as ApprovalRules)"
                        TableName="ApprovalRules">
                    </asp:LinqDataSource>
                    <asp:RequiredFieldValidator
                        ID="rvGolongan"
                        runat="server"
                        ControlToValidate="ddlGolongan"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>

                <dt>Posisi sebagai</dt>
                <dd>
                    <asp:DropDownList ID="ddlPosisi"
                        runat="server"
                        OnSelectedIndexChanged="ddlPosisi_SelectedIndexChanged"
                        AutoPostBack="true">
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
                        <asp:ListItem Text="Chief" Value="Chief"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rvPosisi"
                        runat="server"
                        ControlToValidate="ddlPosisi"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True">
                    </asp:RequiredFieldValidator>
                </dd>
            </dl>
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">Tujuan</div>
        <div class="panel-body">
            <dl class="dl-horizontal">
                <dt>Luar / Dalam Negri</dt>
                <dd>
                    <asp:DropDownList
                        ID="ddlTujuan"
                        runat="server"
                        AppendDataBoundItems="true"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlTujuan_SelectedIndexChanged">
                        <asp:ListItem Value=""> - Select One - </asp:ListItem>
                        <asp:ListItem Value="DN">Dalam Negeri</asp:ListItem>
                        <asp:ListItem Value="LN">Luar Negeri</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rvTujuan"
                        runat="server"
                        ControlToValidate="ddlTujuan"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dd>
                    <asp:DropDownList
                        ID="ddlWilayahTujuan"
                        runat="server"
                        AutoPostBack="True"
                        DataSourceID="LinqLuarNegeri"
                        DataTextField="wilayah"
                        DataValueField="wilayah" OnSelectedIndexChanged="ddlWilayahTujuan_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:LinqDataSource
                        ID="LinqLuarNegeri"
                        runat="server"
                        ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName=""
                        GroupBy="wilayah"
                        Select="new (key as wilayah, it as msGolonganPlafons)"
                        TableName="msGolonganPlafons"
                        Where="jenisSPD == @jenisSPD">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="ddlTujuan" Name="jenisSPD" PropertyName="SelectedItem.Text" Type="String" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                    <asp:RequiredFieldValidator
                        ID="rvWilayahTujuan"
                        runat="server"
                        ControlToValidate="ddlWilayahTujuan"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dt>Tempat tujuan</dt>
                <dd>
                    <asp:DropDownList
                        ID="ddlCompanyTujuan"
                        runat="server"
                        AppendDataBoundItems="true"
                        DataTextField="companyCode"
                        DataValueField="coCd"
                        DataSourceID="LinqComCode"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCompanyTujuan_SelectedIndexChanged">
                        <asp:ListItem Value=""> - Select One - </asp:ListItem>
                        <asp:ListItem Value="0">LAIN - LAIN</asp:ListItem>
                    </asp:DropDownList>
                    <asp:LinqDataSource
                        ID="LinqComCode"
                        runat="server"
                        ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName=""
                        GroupBy="coCd"
                        OrderGroupsBy="key"
                        Select="new (key as coCd, it as msKaryawans, Min(companyCode) as companyCode)"
                        TableName="msKaryawans">
                    </asp:LinqDataSource>
                    <asp:RequiredFieldValidator
                        ID="rvCompanyTujuan"
                        runat="server"
                        ControlToValidate="ddlCompanyTujuan"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dd>
                    <asp:DropDownList
                        ID="ddlPersonalAreaTujuan"
                        runat="server"
                        DataTextField="leasingRental"
                        DataValueField="kodePA"
                        DataSourceID="LinqCabang"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlPersonalAreaTujuan_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:LinqDataSource
                        ID="LinqCabang"
                        runat="server"
                        ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName=""
                        GroupBy="kodePA"
                        OrderGroupsBy="key"
                        Select="new (key as kodePA, it as v_karyawanTracs, Min(leasingRental) as leasingRental)"
                        TableName="v_karyawanTracs" Where="coCd == @coCd">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="ddlCompanyTujuan" Name="coCd" PropertyName="SelectedValue" Type="String" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                    <asp:RequiredFieldValidator
                        ID="rvPersonalAreaTujuan"
                        runat="server"
                        ControlToValidate="ddlPersonalAreaTujuan"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dd>
                    <asp:DropDownList
                        ID="ddlPSubAreaTujuan"
                        runat="server"
                        DataSourceID="LinqSubCabang"
                        DataTextField="pSubArea"
                        DataValueField="kodePSubArea">
                    </asp:DropDownList>
                    <asp:LinqDataSource
                        ID="LinqSubCabang"
                        runat="server"
                        ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName=""
                        GroupBy="kodePSubArea"
                        OrderGroupsBy="key"
                        Select="new (key as kodePSubArea, it as v_karyawanTracs, Max(pSubArea) as pSubArea)"
                        TableName="v_karyawanTracs" Where="kodePA == @kodePA">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="ddlPersonalAreaTujuan" Name="kodePA" PropertyName="SelectedValue" Type="String" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                    <asp:RequiredFieldValidator
                        ID="rvPSubAreaTujuan"
                        runat="server"
                        ControlToValidate="ddlPSubAreaTujuan"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dt>Tempat tujuan lain</dt>
                <dd>
                    <asp:TextBox ID="txTempatTujuanLain" runat="server" Enabled="false" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rvTempatTujuanLain"
                        runat="server"
                        ControlToValidate="txTempatTujuanLain"
                        ErrorMessage=" Harus diisi "
                        Font-Bold="True"
                        Enabled="False"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>

                <dt>Keperluan / Tugas</dt>
                <dd>
                    <asp:DropDownList
                        ID="ddlKeperluan"
                        runat="server"
                        DataSourceID="LinqKeperluan"
                        DataTextField="keperluan"
                        DataValueField="id"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:LinqDataSource
                        ID="LinqKeperluan"
                        runat="server"
                        ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName=""
                        Select="new (id, keperluan)" TableName="msKeperluans">
                    </asp:LinqDataSource>
                    <asp:RequiredFieldValidator
                        ID="rvKeperluan"
                        runat="server"
                        ControlToValidate="ddlKeperluan"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dt>Keterangan keperluan</dt>
                <dd>
                    <asp:TextBox ID="txKetKeperluan" runat="server" Rows="3" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rvKetKeperluan"
                        runat="server"
                        ControlToValidate="txKetKeperluan"
                        ErrorMessage=" Harus diisi "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revTxtKeterangan" runat="server"
                        ErrorMessage="* Maksimal 500 karakter"
                        ValidationExpression="^([\S\s]{0,500})$"
                        ControlToValidate="txKetKeperluan"
                        Display="Dynamic"><strong>Maksimal 500 karakter</strong></asp:RegularExpressionValidator>
                </dd>

                <dt>Tanggal berangkat</dt>
                <dd>
                    <asp:TextBox
                        ID="txtTglBerangkat" runat="server"
                        AutoPostBack="True"
                        OnTextChanged="txtTglBerangkat_TextChanged"
                        Width="200"></asp:TextBox>
                    <ajax:CalendarExtender ID="txtTglBerangkat_CalendarExtender" runat="server" TargetControlID="txtTglBerangkat">
                    </ajax:CalendarExtender>
                    &nbsp; Jam
                    <asp:DropDownList ID="ddlJamBerangkat" runat="server" CssClass="time">
                        <asp:ListItem Value="">-</asp:ListItem>
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
                    <asp:DropDownList ID="ddlMenitBerangkat" runat="server" CssClass="time">
                        <asp:ListItem Value="00">00</asp:ListItem>
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

                </dd>

                <dt>Tanggal kembali</dt>
                <dd>
                    <asp:TextBox ID="txtTglKembali" runat="server"
                        AutoPostBack="True" OnTextChanged="txtTglKembali_TextChanged" Width="200"></asp:TextBox>
                    <ajax:CalendarExtender ID="txtTglKembali_CalendarExtender" runat="server" TargetControlID="txtTglKembali">
                    </ajax:CalendarExtender>
                    &nbsp; Jam
                    <asp:DropDownList ID="ddlJamKembali" runat="server" CssClass="time">
                        <asp:ListItem Value="">-</asp:ListItem>
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
                    <asp:DropDownList ID="ddlMenitKembali" runat="server" CssClass="time">
                        <asp:ListItem Value="00">00</asp:ListItem>
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
                </dd>
                <dd>
                    <ul>
                        <asp:RequiredFieldValidator
                            ID="rvTglBerangkat"
                            runat="server"
                            ControlToValidate="txtTglBerangkat"
                            ErrorMessage="<li>Tanggal berangkat harus dipilih</li>"
                            Font-Bold="True"
                            Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator
                            ID="rvJamBerangkat"
                            runat="server"
                            ControlToValidate="ddlJamBerangkat"
                            ErrorMessage="<li>Jam berangkat harus dipilih</li>"
                            Font-Bold="True"
                            Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator
                            ID="rvMenitBrangkat"
                            runat="server"
                            ControlToValidate="ddlMenitBerangkat"
                            ErrorMessage="<li>Menit berangkat harus dipilih</li>"
                            Font-Bold="True"
                            Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>

                        <asp:RequiredFieldValidator
                            ID="rvTglKembali"
                            runat="server"
                            ControlToValidate="txtTglKembali"
                            ErrorMessage="<li>Tanggal kembali harus dipilih</li>"
                            Font-Bold="True"
                            Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>

                        <asp:RequiredFieldValidator
                            ID="rvJamKembali"
                            runat="server"
                            ControlToValidate="ddlJamKembali"
                            ErrorMessage="<li>Jam kembali dipilih</li>"
                            Font-Bold="True"
                            Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>


                        <asp:RequiredFieldValidator
                            ID="rvMenitKembali"
                            runat="server"
                            ControlToValidate="ddlMenitKembali"
                            ErrorMessage="<li>Menit kembali dipilih</li>"
                            Font-Bold="True"
                            Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>


                        <asp:Label ID="lblDateLessThan" runat="server" Visible="false"> <li>     
                                <b>Tangal berangkat harus lebih kecil atau sama dengan tanggal kembali.</b>  </li></asp:Label>


                        <li>
                            <asp:Label ID="lblDateError" runat="server"> <i>Waktu berangkat harus lebih kecil nilainya, dari pada waktu kembali.</i> </asp:Label>
                        </li>
                    </ul>

                </dd>
                 <dt>Tanggal Expired</dt>
                <dd>
                    <asp:TextBox ID="txtTglExp" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>
                </dd>
                <dt>Alasan menginap</dt>
                <dd>
                    <asp:TextBox runat="server" ID="txAlasan" TextMode="MultiLine" Rows="3" Enabled="false" MaxLength="500"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rvAlasan"
                        runat="server"
                        ControlToValidate="txAlasan"
                        ErrorMessage=" Harus diisi "
                        Font-Bold="True"
                        SetFocusOnError="True"
                        Enabled="false"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="reAlasan" runat="server"
                        ErrorMessage="* Maksimal 500 karakter"
                        ValidationExpression="^([\S\s]{0,500})$"
                        ControlToValidate="txAlasan"
                        Display="Dynamic"
                        Enabled="false"><strong>Maksimal 500 karakter</strong></asp:RegularExpressionValidator>
                </dd>
            </dl>
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">Informasi lain - lain</div>
        <div class="panel-body">
            <dl class="dl-horizontal">
                <dt>Cost center pembebanan</dt>
                <dd>
                    <asp:DropDownList
                        ID="ddlCostCenter"
                        runat="server"
                        DataSourceID="LinqCostCenter"
                        DataTextField="costDesc"
                        DataValueField="costDesc"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:LinqDataSource
                        ID="LinqCostCenter"
                        runat="server"
                        ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName=""
                        Select="new (costId, costDesc)" TableName="msCosts">
                    </asp:LinqDataSource>
                    <asp:RequiredFieldValidator
                        ID="rvCostCenter"
                        runat="server"
                        ControlToValidate="ddlCostCenter"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dt>Keterangan</dt>
                <dd>
                    <asp:TextBox ID="txKeterangan" MaxLength="500" Rows="3" TextMode="MultiLine" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="rgKeterangan" runat="server"
                        ErrorMessage="* Maksimal 500 karakter"
                        ValidationGroup="a"
                        ValidationExpression="^([\S\s]{0,500})$"
                        ControlToValidate="txKeterangan"
                        Display="Dynamic"><strong>Maksimal 500 karakter</strong></asp:RegularExpressionValidator>
                </dd>
            </dl>
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            Informasi transportasi & akomodasi
        </div>
        <div class="panel-body">
            <dl class="dl-horizontal">
                <dt>Angkutan</dt>
                <dd>
                    <asp:DropDownList
                        ID="ddlAngkutan"
                        runat="server"
                        DataSourceID="LinqAngkutan"
                        DataTextField="nama"
                        DataValueField="id"
                        AutoPostBack="true"
                        AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAngkutan_SelectedIndexChanged">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:LinqDataSource ID="LinqAngkutan" runat="server" ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName="" Select="new (id, nama)" TableName="msAngkutans">
                    </asp:LinqDataSource>
                    <asp:RequiredFieldValidator
                        ID="rvAngktan"
                        runat="server"
                        ControlToValidate="ddlAngkutan"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
                <dt>Angkutan lain - lain</dt>
                <dd>
                    <asp:TextBox ID="txAngkutanLain" runat="server" MaxLength="500" 
                        AutoPostBack="True"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rvAngkutanLain"
                        runat="server"
                        ControlToValidate="txAngkutanLain"
                        ErrorMessage=" Harus diisi "
                        Enabled="false"
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rgAngkutanLain" runat="server"
                        ErrorMessage="* Maksimal 50 karakter"
                        ValidationGroup="a"
                        ValidationExpression="^([\S\s]{0,50})$"
                        ControlToValidate="txAngkutanLain"
                        Display="Dynamic"><strong>Maksimal 50 karakter</strong></asp:RegularExpressionValidator>
                </dd>
                <dt>Hotel</dt>
                <dd>
                    <%--  <asp:CheckBox ID="cbIsHotel" Checked="true" runat="server" CssClass="hideIt" />--%>
                    <asp:DropDownList ID="ddlPenginapan" runat="server" Enabled="false">
                        <asp:ListItem>Tidak disediakan</asp:ListItem>
                        <asp:ListItem>Disediakan</asp:ListItem>
                        <asp:ListItem>Menginap di saudara</asp:ListItem>
                    </asp:DropDownList>
                </dd>
                <dt>Uang muka</dt>
                <dd>
                    <asp:TextBox ID="txUangMuka" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>
                    &nbsp;
                            <asp:RegularExpressionValidator ID="reUangMuka"
                                ControlToValidate="txUangMuka"
                                ValidationExpression="^\d+"
                                Display="Dynamic"
                                Font-Bold="true"
                                runat="server"><b>Uang hanya diperbolehkan nomor.</b></asp:RegularExpressionValidator>
                    <cc1:FilteredTextBoxExtender ID="ftUangMuka" runat="server"
                        TargetControlID="txUangMuka"
                        FilterType="Numbers" />
                </dd>
                <dt>Biaya makan</dt>
                <dd>
                    <asp:TextBox ID="txBiayaMakan" runat="server" ReadOnly="true"></asp:TextBox>
                </dd>
                <dt>Uang saku</dt>
                <dd>
                    <asp:TextBox ID="txUangSaku" runat="server" ReadOnly="true"></asp:TextBox>
                </dd>
                <dt>Transportasi</dt>
                <dd>
                    <asp:TextBox ID="txTransportasi" runat="server" ReadOnly="true" Text="-"></asp:TextBox>
                </dd>
            </dl>
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            Approval
        </div>
        <div class="panel-body">
            <dl class="dl-horizontal">
                <dt>Approval atasan</dt>
                <dd>
                    <asp:Panel ID="pnlRequiredAppval" runat="server" Visible="true" CssClass="pnlInfo">
                        <div class="bs-callout bs-callout-danger">
                            <h4>Approval ini harus ada dan diisi, Silahkan isi form dibawah ini</h4>
                            <ul>
                                <li>Asal</li>
                                <li>Golongan</li>
                                <li>Posisi</li>
                                <li>Tujuan Luar / Dalam Negri</li>
                            </ul>
                            <p>
                                <i><b>note : Matrix approval yang Anda pilih tidak sesuai dengan SOP skenario approval E-SPD.</b></i>
                            </p>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlApproval" runat="server" Visible="false">
                        <asp:GridView ID="gvApproval" runat="server" AutoGenerateColumns="False" CssClass="gridtable">
                            <Columns>
                                <asp:TemplateField HeaderText="Posisi" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDeskripsi" Text='<%# Eval("Deskripsi") %>'></asp:Label>
                                        <asp:RequiredFieldValidator
                                            ID="rvNrpApproval"
                                            runat="server"
                                            ControlToValidate="txNrpApproval"
                                            ErrorMessage=" Harus dipilih "
                                            Font-Bold="True"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
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
                    </asp:Panel>
                </dd>
                <dt>Approval tujuan</dt>
                <dd>
                    <asp:DropDownList
                        ID="ddlApprovalTujuan"
                        runat="server"
                        DataSourceID="LinqApprovalTujuan"
                        DataValueField="nrp"
                        DataTextField="namaLengkap"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:LinqDataSource
                        ID="LinqApprovalTujuan"
                        runat="server"
                        ContextTypeName="eSPD.dsSPDDataContext"
                        EntityTypeName=""
                        OrderBy="namaLengkap"
                        Select="new (nrp, namaLengkap)"
                        TableName="v_atasan_tujuans" Where="nrp != @nrp">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="txNrp" Name="nrp" PropertyName="Value" Type="String" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                    <asp:RequiredFieldValidator
                        ID="rvApprovalTujuan"
                        runat="server"
                        ControlToValidate="ddlApprovalTujuan"
                        ErrorMessage=" Harus dipilih "
                        Font-Bold="True"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </dd>
            </dl>
        </div>
    </div>

    <div class="panel panel-default">
        <asp:Panel ID="pnlSuccess" class="panel-body" Visible="false" runat="server">
            <div class="panel-body">
                <div class="bs-callout bs-callout-info">
                    <h4>
                        <asp:Label ID="lblSuccess" runat="server" Text="">
                        </asp:Label>
                    </h4>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlError" class="panel-body" Visible="false" runat="server">
            <div class="panel-body">
                <div class="bs-callout bs-callout-danger">
                    <h4>SPD Gagal disave
                    </h4>
                    <ul>
                        <asp:ListView ID="errorMessage" runat="server">
                            <ItemTemplate>
                                <li><%#Container.DataItem %></li>
                            </ItemTemplate>
                        </asp:ListView>
                    </ul>

                </div>
            </div>
        </asp:Panel>

        <div class="panel-footer">
            <dl class="dl-horizontal">
                <dt></dt>
                <dd>
                    <input type="button" runat="server" id="btnReset" style="width: 150px; height: 50px" value="Reset" onclick="window.location.reload(true)" />
                    <asp:Button ID="btnSave" Width="150px" Height="50px" runat="server" Text="Save" OnClientClick="" OnClick="btnSave_Click" />
                    <asp:Button ID="btnSubmit" Width="150px" Height="50px" runat="server" Enabled="false" Text="Submit" OnClientClick="confirm('Are you sure?');" OnClick="btnSubmit_Click" />
                </dd>
            </dl>
        </div>
    </div>

    <script type="text/javascript">
        function disableScrolling() {
            var x = window.scrollX;
            var y = window.scrollY;
            window.onscroll = function () { window.scrollTo(x, y); };
        }

        function enableScrolling() {
            window.onscroll = function () { };
        }

        $(function () {
            enableScrolling();
            // setiap action post back keluarin loading
            var originalDoPostback = __doPostBack;
            __doPostBack = function (p1, p2) {
                $('.loading').fadeIn();
                disableScrolling();
                originalDoPostback(p1, p2);
            };

            // setiap action post back keluarin loading
            $('form').on('submit', function () {
                if (Page_IsValid) {
                    // do something
                    $('.loading').show();
                }
            });

            // smua dropdownlist (select) kecuali dropdown menit / detik 
            $("select:not(.time)").select2({
                placeholder: "Select one",
                allowClear: true,
                width: '380px'
            });


            // event dynamic buat matrix approval

            $('.ddlAtasan').each(function () {
                $(this).select2({
                    width: 'resolve',
                    placeholder: "Pilih atasan ",
                    minimumInputLength: 1,
                    triggerChange: true,
                    allowClear: true,
                    ajax: {
                        url: '<%= ResolveUrl("~/newFormRequestInput.aspx/GetAtasan") %>',
                        type: 'POST',
                        params: {
                            contentType: 'application/json; charset=utf-8'
                        },
                        dataType: 'json',
                        cache: true,
                        data: function (bond, page) {
                            return JSON.stringify({
                                searchText: bond,
                                additionalFilter: false,
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
    </script>
</asp:Content>
