<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="newFormHome.aspx.cs" Inherits="eSPD.newFormHome" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideIt {
            display: none;
        }

        .fixit {
            width: 100%;
        }

        .fright {
            text-align: right;
        }

        .GridPager a {
            display: block;
            height: 15px;
            width: 15px;
            color: #fff;
            font-weight: bold;
            border: 1px solid #e3e3e3;
            text-align: center;
            text-decoration: none;
        }

        .GridPager span {
            display: block;
            height: 15px;
            width: 15px;
            color: #000;
            font-weight: bold;
            border: 1px solid #e3e3e3;
            text-align: center;
            background-color: #E3EAEB;
            text-decoration: none;
        }

        h3 {
            border-bottom: 1px solid #e3e3e3;
            padding-bottom: 5px;
        }

            h3 span {
                float: right;
            }

        .labelMessage {
            padding: 1em;
            background-color: #dcddde;
            border: 1px solid #ddd;
            display: block;
            font-weight: 300;
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
    </style>
    <script type="text/javascript">
        function openDetail(url) {
            window.open(url, "", "width=800, height=600, scrollbars=yes, resizable=yes");
        }


        function ShowLoading() {
            $('.loading').fadeIn();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="loading">Please Wait</div>
    <div id="spdApproval" runat="server">
        <h3>List Approval SPD
        <span>
            <asp:TextBox ID="txtTglBerangkat" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="txtTglBerangkat_CalendarExtender" runat="server"
                TargetControlID="txtTglBerangkat">
            </asp:CalendarExtender>
            <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" />
        </span>
        </h3>

        <asp:UpdatePanel ID="UpApprovalSPD" runat="server">
            <Triggers>
                <ajax:AsyncPostBackTrigger ControlID="btnFind" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(function () {
                        $(function () {
                            $('.loading').fadeOut();
                            var originalDoPostback = __doPostBack;
                            __doPostBack = function (p1, p2) {
                                $('.loading').fadeIn();
                                originalDoPostback(p1, p2);
                            };
                        });
                    });
                </script>

                <p>
                    <asp:Label runat="server" ID="lblMessage" Visible="false" CssClass="labelMessage"></asp:Label>
                </p>
                 <p>
            Filter : &nbsp&nbsp&nbsp&nbsp
            <asp:DropDownList ID="DropDownList1" runat="server" Width="200px"  OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                 <asp:ListItem Text="No SPD" Value="No"></asp:ListItem>
                <asp:ListItem Text="Nama" Value="Nama"></asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
              <asp:TextBox ID="txtcari" runat="server" Visible="false"></asp:TextBox>
            <asp:Button ID="btncari" runat="server" Text="Search"  OnClick="btncari_Click"  />
            &nbsp;&nbsp;
                <div style="border: 1px solid #e3e3e3">
                    <asp:GridView ID="gvViewSPD" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" CssClass="fixit"
                        GridLines="None" AllowPaging="True"
                        OnPageIndexChanging="gvViewSPD_PageIndexChanging"
                        OnRowCommand="gvViewSPD_RowCommand"
                        OnRowCancelingEdit="gvViewSPD_RowCancelingEdit">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                            <asp:BoundField HeaderText="NRP" DataField="nrp" />
                            <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                            <asp:BoundField HeaderText="Tempat Tujuan" DataField="cabangTujuan" />
                            <asp:BoundField HeaderText="Keperluan" DataField="keperluan" />
                            <asp:BoundField HeaderText="Tanggal Berangkat" DataField="tglBerangkat" DataFormatString="{0:M/d/yyyy}" />
                            <asp:BoundField HeaderText="Tanggal Kembali" DataField="tglKembali" DataFormatString="{0:M/d/yyyy}" />
                            <asp:BoundField HeaderText="Status" DataField="status" />
                            <asp:BoundField HeaderText="StatusApproval" DataField="StatusApproval"
                                ItemStyle-CssClass="hideIt"
                                HeaderStyle-CssClass="hideIt"
                                ControlStyle-CssClass="hideIt"
                                FooterStyle-CssClass="hideIt" />
                            <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="60" ItemStyle-Width="200">
                                <ItemTemplate>
                                    <asp:Button ID="btnApprove" runat="server"
                                        CommandName="Approve"
                                        CommandArgument='<%#Eval("noSPD") + ";" +Eval("NrpApproval")+ ";" +Eval("IndexLevel")%>'
                                        OnClientClick="if (!confirm('Are you sure?')) {return false;} else {ShowLoading(); return true;}"
                                        Text="Approve" />
                                    <asp:Button ID="btnReject" runat="server"
                                        CommandName="Reject"
                                        CommandArgument='<%#Eval("noSPD") + ";" +Eval("NrpApproval")+ ";" +Eval("IndexLevel")%>'
                                        OnClientClick="if (!confirm('Are you sure?')) {return false;} else {ShowLoading(); return true;}"
                                        Text="Reject" />
                                    <asp:Button ID="btnDetail" runat="server"
                                        CommandName="Detail"
                                        CommandArgument='<%# Eval("noSPD") %>'
                                        Text="Detail" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" CssClass="GridPager" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="spdtujuan" runat="server">
        <h3>List Approval SPD Tujuan
                  <span>
                      <asp:TextBox ID="txTglBerangkatTujuan" runat="server"></asp:TextBox>
                      <asp:CalendarExtender ID="txTglBerangkatTujuan_CalendarExtender" runat="server"
                          TargetControlID="txTglBerangkatTujuan">
                      </asp:CalendarExtender>
                      <asp:Button ID="btnFindTujuan" runat="server" Text="Find" OnClick="btnFindTujuan_Click" />
                  </span>
        </h3>

        <asp:UpdatePanel ID="UpApprovalTujuan" runat="server">
            <Triggers>
                <ajax:AsyncPostBackTrigger ControlID="btnFindTujuan" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(function () {
                        $(function () {
                            $('.loading').fadeOut();
                            var originalDoPostback = __doPostBack;
                            __doPostBack = function (p1, p2) {
                                $('.loading').fadeIn();
                                originalDoPostback(p1, p2);
                            };
                        });
                    });
                </script>
                <p>
                    <asp:Label runat="server" ID="lblMessageTujuan" Visible="false" CssClass="labelMessage"></asp:Label>
                </p>
                <div style="border: 1px solid #e3e3e3">
                    <asp:GridView ID="gvViewSPDTujuan" runat="server" AutoGenerateColumns="False" CssClass="fixit"
                        CellPadding="4" ForeColor="#333333"
                        GridLines="None" AllowPaging="True"
                        OnPageIndexChanging="gvViewSPDTujuan_PageIndexChanging"
                        OnRowCommand="gvViewSPDTujuan_RowCommand"
                        OnRowDataBound="gvViewSPDTujuan_RowDataBound"
                        OnRowCancelingEdit="gvViewSPDTujuan_RowCancelingEdit">
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
                            <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="60" ItemStyle-Width="200">
                                <ItemTemplate>
                                    <asp:Button ID="btnApprove" runat="server"
                                        CommandName="Approve"
                                        CommandArgument='<%#Eval("noSPD")%>'
                                        OnClientClick="if (!confirm('Are you sure?')) {return false;} else {ShowLoading(); return true;}"
                                        Text="Approve" />
                                    <asp:Button ID="btnReject" runat="server"
                                        CommandName="Reject"
                                        CommandArgument='<%#Eval("noSPD")%>'
                                        OnClientClick="if (!confirm('Are you sure?')) {return false;} else {ShowLoading(); return true;}"
                                        Text="Reject" />
                                    <asp:Button ID="btnDetail" runat="server"
                                        CommandName="Detail"
                                        CommandArgument='<%# Eval("noSPD") %>'
                                        Text="Detail" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" CssClass="GridPager" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="spdGA" runat="server" visible="false">
        <asp:UpdatePanel ID="UpSPDGAFilter" runat="server">
            <ContentTemplate>
                <h3>List SPD GA
            <span>
                <asp:DropDownList ID="ddlParamGA" runat="server" OnSelectedIndexChanged="ddlParamGA_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                    <asp:ListItem Text="Tanggal Keberangkatan" Value="tglBerangkat"></asp:ListItem>
                    <asp:ListItem Text="No SPD" Value="noSPD"></asp:ListItem>
                    <asp:ListItem Text="NRP" Value="nrp"></asp:ListItem>
                    <asp:ListItem Text="Nama" Value="namaLengkap"></asp:ListItem>
                    <asp:ListItem Text="Status" Value="status"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txFilterGA" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="ceFilterGA" runat="server" Enabled="false"
                    TargetControlID="txFilterGA">
                </asp:CalendarExtender>
                <asp:Button ID="btnFindGA" runat="server" Text="Find" OnClick="btnFindGA_Click" /></span>
                </h3>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpSPDGA" runat="server">
            <Triggers>
                <ajax:AsyncPostBackTrigger ControlID="btnFindGA" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(function () {
                        $(function () {
                            $('.loading').fadeOut();
                            var originalDoPostback = __doPostBack;
                            __doPostBack = function (p1, p2) {
                                $('.loading').fadeIn();
                                originalDoPostback(p1, p2);
                            };
                        });
                    });
                </script>
                <p>
                    <asp:Label runat="server" ID="lblMessageGA" Visible="false" CssClass="labelMessage"></asp:Label>
                </p>
                <div style="border: 1px solid #e3e3e3">
                    <asp:GridView ID="gvViewSPDGA" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" CssClass="fixit"
                        GridLines="None" AllowPaging="True"
                        OnPageIndexChanging="gvViewSPDGA_PageIndexChanging"
                        OnRowCommand="gvViewSPDGA_RowCommand"
                        OnRowCancelingEdit="gvViewSPDGA_RowCancelingEdit"
                        OnRowDataBound="gvViewSPDGA_RowDataBound">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                            <asp:BoundField HeaderText="NRP" DataField="nrp" />
                            <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                            <asp:BoundField HeaderText="Tempat Tujuan" DataField="cabangTujuan" />
                            <asp:BoundField HeaderText="Keperluan" DataField="keperluan" />
                            <asp:BoundField HeaderText="Tanggal Berangkat" DataField="tglBerangkat" DataFormatString="{0:M/d/yyyy}" />
                            <asp:BoundField HeaderText="Tanggal Kembali" DataField="tglKembali" DataFormatString="{0:M/d/yyyy}" />
                            <asp:BoundField HeaderText="Status" DataField="status" />
                            <asp:BoundField HeaderText="Tanggal Expired" DataField="tglExpired" DataFormatString="{0:M/d/yyyy}" />
                            <asp:CheckBoxField DataField="isCancel" HeaderText="isCancel" HeaderStyle-CssClass="hideIt" ItemStyle-CssClass="hideIt" ReadOnly="True" SortExpression="isCancel" />
                            <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="60" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Button ID="btnCancel" runat="server"
                                        CommandName="Cancel"
                                        CommandArgument='<%# Eval("noSPD") %>'
                                        OnClientClick="if (!confirm('Are you sure?')) {return false;} else {ShowLoading(); return true;}"
                                        Text="Cancel" />
                                    <asp:Button ID="btnDetail" runat="server"
                                        CommandName="Detail"
                                        CommandArgument='<%# Eval("noSPD") %>'
                                        Text="Detail" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" CssClass="GridPager" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

