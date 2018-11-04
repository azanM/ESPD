<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmGLAccount.aspx.cs" Inherits="eSPD.frmGLAccount" %>
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--kalo ie gak support ini keluar-->
    <div class="bs-callout bs-callout-danger" id="errorbrowser">
        <h4>Browser anda sudah terlalu lawas.
        </h4>
        <p>Performa interface kemungkinan akan kurang maximal, dianjurkan untuk diupdate ke versi baru, terima kasih atas kerja samanya.</p>
    </div>

    <!--hidden field-->
    <asp:HiddenField ID="txNrp" runat="server" />
    <div class="loading">Please Wait</div>

    <h3>
        GL Account
        <%--<span>
            <asp:HyperLink ID="BackTolist" runat="server" NavigateUrl="">Back To List</asp:HyperLink>
        </span>--%>
    </h3>

    <div class="panel panel-default">
        <div class="panel-heading">ADD GL Account</div>
        <div class="panel-body">
            <dl class="dl-horizontal">
                <dt>GL Account Number</dt>
                <dd>
                    <asp:DropDownList ID="ddlGlAccount"
                                        runat="server"
                                        AppendDataBoundItems="true"
                                        AutoPostBack="true">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rvdDdlGlAccount"
                                                runat="server"
                                                ControlToValidate="ddlGlAccount"
                                                ErrorMessage=" Harus dipilih "
                                                Font-Bold="True"
                                                SetFocusOnError="True">
                    </asp:RequiredFieldValidator>
                </dd>
                <dt>GL Account Description</dt>
                <dd>
                    <asp:DropDownList ID="ddlGlAccountDesc"
                                        runat="server"
                                        AppendDataBoundItems="true"
                                        AutoPostBack="true">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rvdDdlGlAccountDesc"
                                                runat="server"
                                                ControlToValidate="ddlGlAccountDesc"
                                                ErrorMessage=" Harus dipilih "
                                                Font-Bold="True"
                                                SetFocusOnError="True">
                    </asp:RequiredFieldValidator>
                </dd>
                <dt>Claim Item</dt>
                <dd>
                    <asp:TextBox ID="txtClaimItem" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                                runat="server"
                                                ControlToValidate="txtClaimItem"
                                                ErrorMessage=" Harus diisi "
                                                Font-Bold="True"
                                                Enabled="false"
                                                SetFocusOnError="True">
                    </asp:RequiredFieldValidator>
                </dd>
                <dt>Status</dt>
                <dd>
                    <asp:DropDownList ID="ddlStatus"
                                        runat="server"
                                        AppendDataBoundItems="true"
                                        AutoPostBack="true">
                        <asp:ListItem Text=" - Select One -" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rvdDdlStatus"
                                                runat="server"
                                                ControlToValidate="ddlStatus"
                                                ErrorMessage=" Harus dipilih "
                                                Font-Bold="True"
                                                SetFocusOnError="True">
                    </asp:RequiredFieldValidator>
                </dd>
            </dl>
        </div>
        <div class="panel-footer">
            <dl class="dl-horizontal">
                <dd>
                    <asp:Button ID="btnSave" runat="server" Width="150px" Height="50px" Text="Save"/>
                    <input type="button" runat="server" id="btnReset" style="width: 150px; height: 50px" value="Reset" onclick="window.location.reload(true)" />
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
        });
    </script>
</asp:Content>
