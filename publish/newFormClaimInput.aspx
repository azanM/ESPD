<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="newFormClaimInput.aspx.cs" Inherits="eSPD.newFormClaimInput" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
            padding-bottom: 1em;
        }

        .bs-callout-danger {
            border-left-color: #ce4844;
        }

        .bs-callout-info {
            border-left-color: #1b809e;
        }

        .bs-callout h4 {
            border-bottom: 1px solid #f5f5f5;
            margin-bottom: 10px;
            margin-top: 0;
            padding-bottom: 5px;
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
            line-height: 2;
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

        .output {
            width: 100px;
            height: 30px;
            padding: 2px;
            background-color: #ddd;
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

            table.gridtable tr.Sub {
                font-weight: bold;
            }

                table.gridtable tr.Sub td {
                    background-color: #f5f5f5;
                }

            table.gridtable td:last-child {
                text-align: right;
            }

            table.gridtable input[type="text"] {
                text-align: right;
                max-width: 100px;
            }

            table.gridtable td:first-child {
                min-width: 300px;
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

        .labelMessage {
            padding: 1em;
            background-color: #dcddde;
            border: 1px solid #ddd;
            display: block;
            font-weight: 300;
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
    <script type="text/javascript">
        function openDetail(url) {
            //url = location.protocol + '/' + location.host + '/' + url;
            window.open(url, "", "width=800, height=600, scrollbars=yes, resizable=yes");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3>Input Claim<span>
        <asp:HyperLink ID="BackTolist" runat="server" NavigateUrl="~/newClaimList.aspx">Back To List</asp:HyperLink></span>
    </h3>

    <div id="DetailSPD" runat="server">
        <div class="panel panel-default">
            <div class="panel-heading">Detail SPD</div>
            <div class="panel-body">
                <div class="bs-callout bs-callout-info">
                    <h4>Informasi SPD
                    </h4>

                    <dl class="dl-horizontal">
                        <dt>SPD:</dt>
                        <dd>&nbsp;<asp:Label ID="lblnoSPD" runat="server" />
                        </dd>
                        <dt>Nama:</dt>
                        <dd>&nbsp;<asp:Label ID="lblnamaLengkap" runat="server" />
                        </dd>
                        <dt>Golongan:</dt>
                        <dd>&nbsp;<asp:Label ID="lblGolongan" runat="server" />
                        </dd>
                        <dt>Jabatan:</dt>
                        <dd>&nbsp;<asp:Label ID="lblJabatan" runat="server" />
                        </dd>
                        <dt>Berangkat:</dt>
                        <dd>&nbsp;<asp:Label ID="lblTglBerangkat" runat="server" />
                            &nbsp;<asp:Label ID="lblJamberangkat" runat="server" />:<asp:Label ID="lblMenitBerangkat" runat="server" />
                        </dd>
                        <dt>Kembali:</dt>
                        <dd>&nbsp;<asp:Label ID="lblTglKembali" runat="server" />
                            &nbsp;<asp:Label ID="lblJamKembali" runat="server" />:<asp:Label ID="lblMenitKembali" runat="server" />
                        </dd>
                        <dt>Keperluan:</dt>
                        <dd>&nbsp;<asp:Label ID="lblKeperluan" runat="server" />
                        </dd>
                        <dt>ket Keperluan:</dt>
                        <dd>&nbsp;<asp:Label ID="lblKetKeperluan" runat="server" />
                        </dd>
                        <dt>Ket Lain:</dt>
                        <dd>&nbsp;<asp:Label ID="lblKeperluanLain" runat="server" />
                        </dd>
                        <dt>Total Hari:</dt>
                        <dd>&nbsp;<asp:Label ID="lblJumlahHari" runat="server" /></dd>
                        <dd>
                            <asp:Button ID="btnDetail" runat="server" Text="Detail" OnClick="btnDetail_Click" /></dd>
                    </dl>
                    <asp:CheckBox ID="isLuarNegeri" runat="server" Visible="false" />
                    <asp:HiddenField ID="hiddenSPD" runat="server" />
                    <asp:HiddenField ID="hiddenNrpAtasan" runat="server" />
                    <asp:HiddenField ID="hiddenUangMuka" runat="server" />
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanelContent" runat="server">
            <ContentTemplate>
                <!--kalo ie gak support ini keluar-->
                <div class="bs-callout bs-callout-danger" id="errorbrowser">
                    <h4>Browser anda sudah terlalu lawas.
                    </h4>
                    <p>Performa interface kemungkinan akan kurang maximal, dianjurkan untuk diupdate ke versi baru, terima kasih atas kerja samanya.</p>
                </div>

                <!--hidden field-->
                <asp:HiddenField ID="txNrp" runat="server" />
                <div class="loading">Please Wait</div>
                <div class="panel panel-default">
                    <div class="panel-heading">Input Claim</div>
                    <div class="panel-body">
                        <table class="gridtable">
                            <thead>
                                <tr>
                                    <th>Keterangan</th>
                                    <th>Amount</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr id="kurs" class="sub" runat="server">
                                    <td>Kurs</td>
                                    <td>
                                        <asp:TextBox ID="txtKurs" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">1</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fetxtKurs" runat="server"
                                            Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtKurs">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Uang Makan&nbsp;(
                                        <asp:Label ID="lblUangMakan" runat="server"></asp:Label>
                                        /Hari X
                                        <asp:Label ID="lblHariUangMakan" runat="server"></asp:Label>)
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUangMakan" runat="server" Text="0" AutoPostBack="True" OnTextChanged="GeneralFunction" ReadOnly="true"></asp:TextBox>
                                        <asp:TextBox ID="UangMakan" ReadOnly="True" runat="server" Text="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Uang Saku
                                        &nbsp;(
                                        <asp:Label ID="lblUangSaku" runat="server"></asp:Label>
                                        /Hari X 
                                        <asp:Label ID="lblHariUangSaku" runat="server"></asp:Label>)
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUangSaku" runat="server" AutoPostBack="True" Text="0" OnTextChanged="GeneralFunction" ReadOnly="true"></asp:TextBox>
                                        <asp:TextBox ID="UangSaku" ReadOnly="True" runat="server" Text="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tiket</td>
                                    <td>

                                        <asp:CheckBox ID="cbTiket" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtTiket" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fetxtTiket" runat="server"
                                            Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtTiket">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="Tiket" runat="server" ReadOnly="True" Text="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Penginapan
                                    </td>
                                    <td>

                                        <asp:CheckBox ID="cbHotel" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtHotel" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fetxtHotel" runat="server"
                                            Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtHotel">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="Hotel" runat="server" ReadOnly="True" Text="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Laundry
                                    </td>
                                    <td>

                                        <asp:CheckBox ID="cbLaundry" runat="server" Visible="false" />
                                        <asp:HiddenField ID="laundryActual" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtLaundry" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">0</asp:TextBox>
                                        <asp:HiddenField ID="LaundryMaxHidden" runat="server" />
                                        <asp:FilteredTextBoxExtender ID="fetxtLaundry" runat="server"
                                            Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtLaundry">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="Laundry" runat="server" ReadOnly="True" Text="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Komunikasi
                                    </td>
                                    <td>

                                        <asp:CheckBox ID="cbKomunikasi" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtKomunikasi" runat="server" OnTextChanged="GeneralFunction"
                                            AutoPostBack="True">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fetxtKomunikasi" runat="server"
                                            Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtKomunikasi">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="Komunikasi" runat="server" ReadOnly="True" Text="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Air Port Tax
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbAirPortTax" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtAirPortTax" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fetxtAirPortTax" runat="server"
                                            Enabled="True" ValidChars="1234567890" FilterType="Custom" TargetControlID="txtAirPortTax">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="AirPortTax" runat="server" ReadOnly="True" Text="0"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>BBM
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbBBM" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtBBM" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fetxtBBM" runat="server" Enabled="True"
                                            ValidChars="1234567890" FilterType="Custom" TargetControlID="txtBBM">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="BBM" runat="server" ReadOnly="True" Text="0"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Tol
                                    </td>
                                    <td class="dolar" runat="server">
                                        <asp:CheckBox ID="cbTol" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtTol" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fetxtTol" runat="server" Enabled="True"
                                            ValidChars="1234567890" FilterType="Custom" TargetControlID="txtTol">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="Tol" runat="server" ReadOnly="True" Text="0"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Taxi
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbTaxi" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtTaxi" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fetxtTaxi" runat="server"
                                            ValidChars="1234567890" FilterType="Custom" Enabled="True" TargetControlID="txtTaxi">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="Taxi" runat="server" ReadOnly="True" Text="0"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Parkir
                                    </td>
                                    <td class="dolar" runat="server">
                                        <asp:CheckBox ID="cbParkir" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtParkir" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fetxtParkir" runat="server"
                                            ValidChars="1234567890" FilterType="Custom" Enabled="True" TargetControlID="txtParkir">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="Parkir" runat="server" ReadOnly="True" Text="0"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Biaya Lain-lain
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtBiayaLainlain" runat="server" AutoPostBack="True" OnTextChanged="GeneralFunction">0</asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="feTxtBiayaLainlain" runat="server"
                                            ValidChars="1234567890" FilterType="Custom" Enabled="True" TargetControlID="txtBiayaLainlain">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:TextBox ID="BiayaLainlain" runat="server" ReadOnly="True" Text="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="Sub">
                                    <td>Total
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtTotal" runat="server" ReadOnly="true">0</asp:TextBox>
                                        <asp:TextBox ID="Total" runat="server" ReadOnly="True" Text="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Uang Muka</td>
                                    <td>
                                        <asp:TextBox ID="txtUangMuka" AutoPostBack="True" OnTextChanged="GeneralFunction" runat="server" ReadOnly="true">0</asp:TextBox>
                                        <asp:TextBox ID="UangMuka" runat="server" ReadOnly="True" Text="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="Sub">
                                    <td>Penyelesaian
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPenyelesaian" runat="server" ReadOnly="true">0</asp:TextBox>
                                        <asp:TextBox ID="Penyelesaian" runat="server" ReadOnly="True" Text="0"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td>Keterangan biaya lain lain</td>
                                    <td>
                                        <asp:TextBox ID="txtKeteranganBiayaLainlain" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="retxtKeteranganBiayaLainlain" runat="server"
                                            ErrorMessage="Maksimal 500 karakter"
                                            ValidationExpression="^([\S\s]{0,500})$"
                                            ControlToValidate="txtKeteranganBiayaLainlain"
                                            Display="Dynamic"
                                            Enabled="false"><strong>Maksimal 500 karakter</strong></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Upload Boarding Pass</td>
                                    <td>

                                        <asp:UpdatePanel runat="server" ID="updatepanel1">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSave" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSubmit" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:FileUpload ID="fuDoc" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </td>

                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                         <asp:UpdatePanel runat="server" ID="updatepanel2">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSave" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSubmit" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:FileUpload ID="fuDoc1" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
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
                                <h4>Claim Gagal disave
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
                                <asp:Button ID="btnSave" Width="150px" Height="50px" runat="server" Text="Save" OnClientClick="confirm('Are your sure?');" OnClick="btnSave_Click" />
                                <asp:Button ID="btnSubmit" Width="150px" Height="50px" runat="server" Enabled="false" Text="Submit" OnClientClick="confirm('Are you sure?');" OnClick="btnSubmit_Click" />
                            </dd>
                        </dl>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:Label ID="lblEmpty" runat="server" CssClass="lblMessage" Visible="false"></asp:Label>
</asp:Content>
