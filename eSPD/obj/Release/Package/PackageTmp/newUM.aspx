<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="newUM.aspx.cs" Inherits="eSPD.newUM" %>

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



    <!--Approval Claim-->
    <div id="finance" runat="server">
        <h3>List UM Finance</h3>
        <p>
            Filter : &nbsp&nbsp&nbsp&nbsp
            <asp:DropDownList ID="DropDownList1" runat="server" Width="200px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                <asp:ListItem Text="No SPD" Value="No"></asp:ListItem>
                <asp:ListItem Text="Nama" Value="Nama"></asp:ListItem>
                <asp:ListItem Text="Status" Value="Status"></asp:ListItem>
                <asp:ListItem Text="Tanggal Penyelesaian" Value="TglPenyelesaian"></asp:ListItem>
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
            <asp:DropDownList ID="ddlStatusFinance" runat="server" Width="200px" Visible="false" >
                         <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>

                <asp:ListItem Text="pending" Value="pending"></asp:ListItem>
                <asp:ListItem Text="approve" Value="approve"></asp:ListItem>
            </asp:DropDownList>

            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTglBerangkatAkhir">
            </ajax:CalendarExtender>
            <asp:Button ID="btncari" runat="server" Text="Search" OnClick="btncari_Click" />
            <asp:Button ID="btnExportFinance" runat="server" Text="Export To Excel" Height="26px" OnClick="btnExportFinance_Click" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />

            &nbsp;&nbsp;
            <asp:GridView ID="gvFinance" runat="server" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" CssClass="fixit"
                AllowPaging="True"
                GridLines="None"
                OnRowCommand="gvFinance_RowCommand"
                OnRowDataBound="gvFinance_RowDataBound"
                OnPageIndexChanging="gvFinance_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                    <asp:BoundField HeaderText="NRP" DataField="nrp" />
                    <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
<asp:BoundField HeaderText="Uang Muka" DataField="uangMuka" />
                    <asp:BoundField HeaderText="Status" DataField="status" />
                    <asp:BoundField DataField="tglPenyelesaian" HeaderText="Tgl Penyelesaian" DataFormatString="{0:M/d/yyyy}" />

                    <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="80" ItemStyle-Width="250">
                        <ItemTemplate>
                            <asp:Button ID="lbApprove" runat="server"
                                CommandName="Approve"
                                CommandArgument='<%# Eval("noSPD") %>'
                                OnClientClick="if (!confirm('Are you sure?')) return false;"
                                Text="approve" />
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

    <div id="ClaimGA" runat="server">
        <h3>List UM GA</h3>
        <p>
            Filter : &nbsp&nbsp&nbsp&nbsp
            <asp:DropDownList ID="DropDownList2" runat="server" Width="200px" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                <asp:ListItem Text="No SPD" Value="No"></asp:ListItem>
                <asp:ListItem Text="Nama" Value="Nama"></asp:ListItem>
                <asp:ListItem Text="Status" Value="Status"></asp:ListItem>
                <asp:ListItem Text="Tanggal Penyelesaian" Value="TglPenyelesaian"> </asp:ListItem>
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
            <asp:DropDownList ID="ddlStatusGA" runat="server" Width="200px" Visible="false">
                         <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>

                <asp:ListItem Text="pending" Value="pending"></asp:ListItem>
                <asp:ListItem Text="approve" Value="approve"></asp:ListItem>
            </asp:DropDownList>
            <ajax:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtTglBerangkatAkhir2">
            </ajax:CalendarExtender>
            <asp:Button ID="btncari2" runat="server" Text="Search" OnClick="btncari2_Click" Height="26px" />
            <asp:Button ID="btnExportGA" runat="server" Text="Export To Excel" Height="26px" OnClick="btnExportGA_Click" />
            <asp:Button ID="btnResetGA" runat="server" Text="Reset" OnClick="btnResetGA_Click" />
            &nbsp;&nbsp;
        <div style="border: 1px solid #e3e3e3">
            <asp:GridView ID="gvGA" runat="server" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" CssClass="fixit"
                AllowPaging="True"
                GridLines="None"
                OnPageIndexChanging="gvGA_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
                    <asp:BoundField HeaderText="NRP" DataField="nrp" />
                    <asp:BoundField HeaderText="Nama Lengkap" DataField="namaLengkap" />
<asp:BoundField HeaderText="Uang Muka" DataField="uangMuka" />
                    <asp:BoundField HeaderText="Status" DataField="status" />
                    <asp:BoundField DataField="tglPenyelesaian" HeaderText="Tgl Penyelesaian" DataFormatString="{0:M/d/yyyy}" />


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

    <div id="Div1" runat="server" visible="false">
    </div>
</asp:Content>
