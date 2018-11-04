<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="newClaimList.aspx.cs" Inherits="eSPD.newClaimList" %>

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
    <br />
    <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>

    <!--Create Claim-->
    <div id="ListSPDClaim" runat="server">
        <h3>List SPD To Claim
        </h3>
        <p>
            Filter : &nbsp&nbsp&nbsp&nbsp
            <asp:DropDownList ID="DropDownList1" runat="server" Width="200px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                <asp:ListItem Text="No SPD" Value="No"></asp:ListItem>
                <asp:ListItem Text="Nama" Value="Nama"></asp:ListItem>
                <asp:ListItem Text="Tanggal Berangkat" Value="TglBerangkat"></asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
              <asp:TextBox ID="txtcari" runat="server" Visible="false"></asp:TextBox>

            &nbsp;&nbsp;&nbsp;&nbsp;
           <asp:TextBox
               ID="txtTglBerangkatAwal" runat="server"
               AutoPostBack="True"
               Width="200" Visible="false"></asp:TextBox>
            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTglBerangkatAwal">
            </ajax:CalendarExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;
           <asp:TextBox
               ID="txtTglBerangkatAkhir" runat="server"
               AutoPostBack="True"
               Width="200" Visible="false"></asp:TextBox>
            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTglBerangkatAkhir">
            </ajax:CalendarExtender>
            <asp:Button ID="btncari" runat="server" Text="Search" OnClick="btncari_Click" />
            &nbsp;&nbsp;
        <div style="border: 1px solid #e3e3e3">
            <asp:GridView ID="gvClaimSPD" runat="server" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" CssClass="fixit"
                GridLines="None" AllowPaging="True"
                OnPageIndexChanging="gvClaimSPD_PageIndexChanging"
                OnRowCommand="gvClaimSPD_RowCommand" 
                OnRowDataBound="gvClaimSPD_RowDataBound" >
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
                    <asp:BoundField DataField="tglExpired" HeaderText="Tgl Expired" DataFormatString="{0:M/d/yyyy}" />

                    <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="100" ItemStyle-Width="105">
                        <ItemTemplate>
                            <asp:Button ID="btnCreate" runat="server"
                                CommandName="Create"
                                CommandArgument='<%# Eval("noSPD") %>'
                                Text="Buat Claim" />
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
    </div>

    <!--Approval Claim-->
    <div id="ClaimAtasan" runat="server">
        <h3>List Approval Atasan</h3>
        <div style="border: 1px solid #e3e3e3">
            <asp:GridView ID="gvAtasan" runat="server" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" CssClass="fixit"
                AllowPaging="True"
                GridLines="None"
                OnRowCommand="gvAtasan_RowCommand"
                OnPageIndexChanging="gvAtasan_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                    <asp:BoundField HeaderText="NRP" DataField="nrp" />
                    <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                    <asp:BoundField HeaderText="Tujuan" DataField="cabangTujuan" />
                    <asp:BoundField HeaderText="Berangkat" DataField="tglBerangkat" />
                    <asp:BoundField HeaderText="Kembali" DataField="tglKembali" />
                    <asp:BoundField HeaderText="Total" DataField="Total" />
                    <asp:BoundField HeaderText="Status" DataField="status" />
                    <asp:BoundField DataField="tglExpired" HeaderText="Tgl Expired" DataFormatString="{0:M/d/yyyy}" />

                    <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="80" ItemStyle-Width="250">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server"
                                CommandName="Approve"
                                CommandArgument='<%# Eval("noSPD") %>'
                                OnClientClick="if (!confirm('Are you sure?')) return false;"
                                Text="Approve" />
                            <asp:Button ID="btnReject" runat="server"
                                CommandName="Reject"
                                CommandArgument='<%# Eval("noSPD") %>'
                                OnClientClick="if (!confirm('Are you sure?')) return false;"
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
    </div>
    <div id="ClaimGA" runat="server">
        <h3>List Approval GA
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
        <div style="border: 1px solid #e3e3e3">
            <asp:GridView ID="gvGA" runat="server" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" CssClass="fixit"
                GridLines="None" AllowPaging="True"
                OnRowDataBound="gvGA_RowDataBound"
                OnRowCancelingEdit="gvGA_RowCancelingEdit"
                OnRowCommand="gvGA_RowCommand"
                OnPageIndexChanging="gvGA_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                    <asp:BoundField HeaderText="NRP" DataField="nrp" />
                    <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                    <asp:BoundField HeaderText="Tujuan" DataField="cabangTujuan" />
                    <asp:BoundField HeaderText="Berangkat" DataField="tglBerangkat" DataFormatString="{0:M/d/yyyy}" />
                    <asp:BoundField HeaderText="Kembali" DataField="tglKembali" DataFormatString="{0:M/d/yyyy}" />
                    <asp:BoundField HeaderText="Total" DataField="Total" />
                    <asp:BoundField HeaderText="Status" DataField="status" />
                     <asp:BoundField HeaderText="Tanggal Expired" DataField="tglExpired" DataFormatString="{0:M/d/yyyy}"/>
                   <asp:BoundField HeaderText="Tanggal Terima" DataField="tglTerima" DataFormatString="{0:M/d/yyyy}"/>
                  
                    <asp:CheckBoxField DataField="isApprovedGA" HeaderText="isApprovedGA" HeaderStyle-CssClass="hideIt" ItemStyle-CssClass="hideIt" ReadOnly="True" SortExpression="isApprovedGA" />
                    <asp:CheckBoxField DataField="isApprovedAtasan" HeaderText="isApprovedAtasan" HeaderStyle-CssClass="hideIt" ItemStyle-CssClass="hideIt" ReadOnly="True" SortExpression="isApprovedAtasan" />
                     <%--  <asp:HyperLinkField  runat="server" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true" DataNavigateUrlFields="hlDownloadTemplateSJ"
                     HeaderText="Download BoardingPass" Text="Download" />
                      <asp:HyperLinkField  runat="server" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true" DataNavigateUrlFields="hlDownloadTemplateSJ1"
                     HeaderText="Download BoardingPass1" Text="Download" />--%>
                  <asp:TemplateField HeaderText="BoardingPass">  
                                    <ItemTemplate>  
                                        <asp:LinkButton ID="lnkBoardingPass" runat="server" CausesValidation="False" CommandArgument='<%# Eval("urlBoardingPass") %>'  
                                            CommandName="DownloadBoardingPass" Text="Download" />  
                                    </ItemTemplate>  
                                </asp:TemplateField>
                       <asp:TemplateField HeaderText="BoardingPass1">  
                                    <ItemTemplate>  
                                        <asp:LinkButton ID="lnkBoardingPass1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("urlBoardingPass1") %>'  
                                            CommandName="DownloadBoardingPass1" Text="Download" />  
                                    </ItemTemplate>  
                                </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="60" ItemStyle-Width="130">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server"
                                CommandName="Approve"
                                CommandArgument='<%# Eval("noSPD") %>'
                                OnClientClick="if (!confirm('Are you sure?')) return false;"
                                Text="Approve" />
                            <asp:Button ID="btnReject" runat="server"
                                CommandName="Reject"
                                CommandArgument='<%# Eval("noSPD") %>'
                                OnClientClick="if (!confirm('Are you sure?')) return false;"
                                Text="Reject" />
                            <asp:Button ID="btnCancel" runat="server"
                                CommandName="Cancel"
                                CommandArgument='<%# Eval("noSPD") %>'
                                OnClientClick="if (!confirm('Are you sure?')) return false;"
                                Text="Cancel" />
                            <asp:Button ID="btnDetail" runat="server"
                                CommandName="Detail"
                                CommandArgument='<%# Eval("noSPD") %>'
                                Text="Detail" />
                            <asp:Button ID="btnEdit" runat="server"
                                CommandName="Edit"
                                CommandArgument='<%# Eval("noSPD") %>'
                                Text="Edit" />
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
    </div>
    <div id="ClaimFinance" runat="server">
        <h3>List Approval Finance
            <span>
                <asp:TextBox ID="txFilterFinance" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="ceFilterFinance" runat="server"
                    TargetControlID="txFilterFinance">
                </asp:CalendarExtender>
                <asp:Button ID="btnFindFinance" runat="server" Text="Find" OnClick="btnFindFinance_Click" />
            </span>
        </h3>

        <div style="border: 1px solid #e3e3e3">
            <asp:GridView ID="gvFinance" runat="server" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" CssClass="fixit"
                GridLines="None" AllowPaging="True"
                OnRowCommand="gvFinance_RowCommand"
                OnPageIndexChanging="gvFinance_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                    <asp:BoundField HeaderText="NRP" DataField="nrp" />
                    <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                    <asp:BoundField HeaderText="Tujuan" DataField="cabangTujuan" />
                    <asp:BoundField HeaderText="Berangkat" DataField="tglBerangkat" DataFormatString="{0:M/d/yyyy}" />
                    <asp:BoundField HeaderText="Kembali" DataField="tglKembali" DataFormatString="{0:M/d/yyyy}" />
                    <asp:BoundField HeaderText="Total" DataField="Total" />
                    <asp:BoundField HeaderText="Status" DataField="status" />
                    <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="80" ItemStyle-Width="250">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server"
                                CommandName="Approve"
                                CommandArgument='<%# Eval("noSPD") %>'
                                OnClientClick="if (!confirm('Are you sure?')) return false;"
                                Text="Approve" />
                            <asp:Button ID="btnReject" runat="server"
                                CommandName="Reject"
                                CommandArgument='<%# Eval("noSPD") %>'
                                OnClientClick="if (!confirm('Are you sure?')) return false;"
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
    </div>

    <!--List SPD Personal View-->
    <div id="ClaimPersonal" runat="server">
        <h3>List Claim
           <%-- <span>
                <asp:TextBox ID="txFilterPersonal" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="ceFilterPersonel" runat="server"
                    TargetControlID="txFilterPersonal">
                </asp:CalendarExtender>
                <asp:Button ID="btnFindPersonal" runat="server" Text="Find" OnClick="btnFindPersonal_Click" />
            </span>--%>
        </h3>

        <p>
            Filter : &nbsp&nbsp&nbsp&nbsp
            <asp:DropDownList ID="DropDownList2" runat="server" Width="200px" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                <asp:ListItem Text="No SPD" Value="No"></asp:ListItem>
                <asp:ListItem Text="Nama" Value="Nama"></asp:ListItem>
                <asp:ListItem Text="Tanggal Berangkat" Value="TglBerangkat"></asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
              <asp:TextBox ID="txtcari2" runat="server" Visible="false"></asp:TextBox>

            &nbsp;&nbsp;&nbsp;&nbsp;
           <asp:TextBox
               ID="txtTglBerangkatAwal2" runat="server"
               AutoPostBack="True"
               Width="200" Visible="false"></asp:TextBox>
            <ajax:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtTglBerangkatAwal2">
            </ajax:CalendarExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;
           <asp:TextBox
               ID="txtTglBerangkatAkhir2" runat="server"
               AutoPostBack="True"
               Width="200" Visible="false"></asp:TextBox>
            <ajax:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtTglBerangkatAkhir2">
            </ajax:CalendarExtender>
            <asp:Button ID="btncari2" runat="server" Text="Search" OnClick="btncari2_Click" />
            &nbsp;&nbsp;
        <div style="border: 1px solid #e3e3e3">

            <asp:Label ID="lblMessagePersonal" runat="server" CssClass="labelMessage" Visible="false"></asp:Label>
            <asp:GridView ID="gvPersonal" runat="server" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" CssClass="fixit"
                GridLines="None" AllowPaging="True"
                OnPageIndexChanging="gvPersonal_PageIndexChanging"
                OnRowDataBound="gvPersonal_RowDataBound"
                OnRowCommand="gvPersonal_RowCommand">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                    <asp:BoundField HeaderText="NRP" DataField="nrp" />
                    <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                    <asp:BoundField HeaderText="Tempat Tujuan" DataField="cabangTujuan" />
                    <asp:BoundField HeaderText="Keperluan" DataField="keperluan" />
                    <asp:BoundField HeaderText="Tanggal Berangkat" DataField="tglBerangkat" DataFormatString="{0:M/d/yyyy}" />
                    <asp:BoundField HeaderText="Tanggal Kembali" DataField="tglKembali" DataFormatString="{0:M/d/yyyy}" />
                    <asp:BoundField HeaderText="Status SPD" DataField="status" />
                    <asp:BoundField HeaderText="Status Claim" DataField="statusClaim" />
                    <asp:BoundField DataField="isApprovedFinance" HeaderText="isApprovedFinance" HeaderStyle-CssClass="hideIt" ItemStyle-CssClass="hideIt" ReadOnly="True" />
                    <asp:BoundField HeaderText="Tanggal Expired" DataField="tglExpired" DataFormatString="{0:M/d/yyyy}" />
                   
                    <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="100" ItemStyle-Width="105">
                        <ItemTemplate>
                            <asp:Button ID="btnDetail" runat="server"
                                CommandName="Detail"
                                CommandArgument='<%# Eval("noSPD") %>'
                                Text="Detail" />
                            <asp:Button ID="btnEdit" runat="server"
                                CommandName="Edit"
                                CommandArgument='<%# Eval("noSPD") %>'
                                Text="Edit" />
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
    </div>


      <!--List SPD Personal View-->
    <div id="ClaimExpired" runat="server">
        <h3>List SPD Expired
        </h3>

           
        <div style="border: 1px solid #e3e3e3">
            <asp:Label ID="lblMessageExpired" runat="server" CssClass="labelMessage" Visible="false"></asp:Label>

            <asp:GridView ID="gvExpired" runat="server" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" CssClass="fixit"
                GridLines="None" AllowPaging="True"
                OnPageIndexChanging="gvExpired_PageIndexChanging"
                OnRowCommand="gvExpired_RowCommand">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                    <asp:BoundField HeaderText="NRP" DataField="nrp" />
                    <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
                    <asp:BoundField HeaderText="Tempat Tujuan" DataField="cabangTujuan" />
                    <asp:BoundField HeaderText="Keperluan" DataField="keperluan" />
                    <asp:BoundField HeaderText="Tanggal Berangkat" DataField="tglBerangkat" DataFormatString="{0:M/d/yyyy}" />
                    <asp:BoundField HeaderText="Tanggal Kembali" DataField="tglKembali" DataFormatString="{0:M/d/yyyy}" />
                     <asp:BoundField HeaderText="Tanggal Expired" DataField="tglExpired" DataFormatString="{0:M/d/yyyy}" />
                   
                    <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="100" ItemStyle-Width="105">
                        <ItemTemplate>
                            <asp:Button ID="Button2" runat="server"
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
    </div>


</asp:Content>
