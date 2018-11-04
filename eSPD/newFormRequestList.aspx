<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="newFormRequestList.aspx.cs" Inherits="eSPD.newFormRequestList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .hideIt {
            display: none;
        }

        .fixit {
            width: 100%;
        }
        f
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


        .labelMessage {
            padding: 1em;
            background-color: #dcddde;
            border: 1px solid #ddd;
            display: block;
            font-weight: 300;
        }

        h3 {
            border-bottom: 1px solid #e3e3e3;
            padding-bottom: 5px;
        }

            h3 span {
                float: right;
            }
    </style>
    <script type="text/javascript">
        function openDetail(url) {
            //url = location.protocol + '/' + location.host + '/' + url;
            window.open(url, "", "width=800, height=600, scrollbars=yes, resizable=yes");
        }

        function areYouSure() {
            return confirm("Are you sure?");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="nrp" runat="server" />
    <h3>List SPD (Personal) <span>
        <asp:HyperLink ID="CreateNew" runat="server" NavigateUrl="~/newFormRequestInput.aspx">Create New</asp:HyperLink></span>
    </h3>
       Filter : &nbsp&nbsp&nbsp&nbsp
            <asp:DropDownList ID="DropDownList2" runat="server" Width="200px"  OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" AutoPostBack="true">
               <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                 <asp:ListItem Text="No SPD" Value="No"></asp:ListItem>
                <asp:ListItem Text="Nama" Value="Nama"></asp:ListItem>
                <asp:ListItem Text="Status" Value="Status"></asp:ListItem>
                <asp:ListItem Text="Tanggal Berangkat" Value="TglBerangkat" ></asp:ListItem>
            </asp:DropDownList>
             <asp:TextBox ID="txtcari1" runat="server" Visible="false"></asp:TextBox>
            <asp:DropDownList ID="ddlstatus1" runat="server" Visible="false">
                  <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
           <asp:TextBox
                        ID="txtTglBerangkatAwal1" runat="server"
                        AutoPostBack="True"
                       
                        Width="200" Visible="false"></asp:TextBox>
          <ajax:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtTglBerangkatAwal1">
           </ajax:CalendarExtender>
           &nbsp;&nbsp;&nbsp;&nbsp;
           <asp:TextBox
             ID="txtTglBerangkatAkhir1" runat="server"
             AutoPostBack="True"
             
             Width="200" Visible="false"></asp:TextBox>
          <ajax:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtTglBerangkatAkhir1" >
           </ajax:CalendarExtender>
            <asp:Button ID="btncari1" runat="server" Text="Search" OnClick="btncari1_Click" />
            &nbsp;&nbsp;
    <div style="border: 1px solid #e3e3e3">
        <asp:Label runat="server" ID="lblMessage" Visible="false" CssClass="labelMessage"></asp:Label>
        <asp:GridView ID="gvList"
            runat="server"
            AutoGenerateColumns="False"
            OnPageIndexChanging="gvList_PageIndexChanging"
            CellPadding="4"
            ForeColor="#333333"
            GridLines="None"
            OnRowCommand="gvList_RowCommand"
            OnRowDataBound="gvList_RowDataBound"
            OnRowCancelingEdit="gvList_RowCancelingEdit"
            OnSorting="gvList_Sorting"
            CssClass="fixit"
            AllowPaging="True">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="noSPD" HeaderText="No SPD" ReadOnly="True" SortExpression="noSPD" />
                <asp:BoundField DataField="namaLengkap" HeaderText="Nama" ReadOnly="True" SortExpression="namaLengkap" />
                <asp:BoundField DataField="Tujuan" HeaderText="Tujuan" ReadOnly="True" SortExpression="Tujuan" />
                <asp:BoundField DataField="Keperluan" HeaderText="Keperluan" ReadOnly="True" SortExpression="ketKeperluan" />
                <asp:BoundField DataField="tglBerangkat" HeaderText="Berangkat" ReadOnly="True" SortExpression="tglBerangkat" DataFormatString="{0:dd MMMM yyyy}" HtmlEncode="false" />
                <asp:BoundField DataField="status" HeaderText="Status" ReadOnly="True" SortExpression="status" />
                   <asp:BoundField DataField="tglExpired" HeaderText="Tgl Expired" ReadOnly="True" SortExpression="tglExpired" DataFormatString="{0:dd MMMM yyyy}" HtmlEncode="false" />
            <asp:CheckBoxField DataField="isCancel" HeaderText="isCancel" HeaderStyle-CssClass="hideIt" ItemStyle-CssClass="hideIt" ReadOnly="True" SortExpression="isCancel" />
                <asp:CheckBoxField DataField="isApproved" HeaderText="isApproved" HeaderStyle-CssClass="hideIt" ItemStyle-CssClass="hideIt" ReadOnly="True" SortExpression="isCancel" />
                <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="60" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Button ID="btnCancel" runat="server"
                            CommandName="Cancel"
                            CommandArgument='<%# Eval("noSPD") %>'
                            OnClientClick="if (!confirm('Are you sure?')) return false;"
                            Text="Cancel" />
                        <asp:Button ID="btnDetail" runat="server"
                            CommandName="Detail"
                            CommandArgument='<%# Eval("noSPD") %>'
                            Text="Detail" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" CssClass="GridPager" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
        <asp:LinqDataSource
            ID="LinqListSPD"
            runat="server"
            ContextTypeName="eSPD.dsSPDDataContext"
            EntityTypeName=""
            Select="new (noSPD, namaLengkap, Tujuan, ketKeperluan, tglBerangkat, status, isCancel, isApproved)" TableName="trSPDs" Where="nrp == @nrp" OrderBy="noSPD desc, tglBerangkat desc">
            <WhereParameters>
                <asp:ControlParameter ControlID="nrp" Name="nrp" PropertyName="Value" Type="String" />
            </WhereParameters>
        </asp:LinqDataSource>
    </div>

    <div id="spdDirector" runat="server" visible="false">
        <h3>List SPD (Director)
             <p>
            Filter : &nbsp&nbsp&nbsp&nbsp
            <asp:DropDownList ID="DropDownList1" runat="server" Width="200px"  OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
                 <asp:ListItem Text="No SPD" Value="No"></asp:ListItem>
                <asp:ListItem Text="Nama" Value="Nama"></asp:ListItem>
                <asp:ListItem Text="Status" Value="Status"></asp:ListItem>
                <asp:ListItem Text="Tanggal Berangkat" Value="TglBerangkat" ></asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtcari" runat="server" Visible="false"></asp:TextBox>
            <asp:DropDownList ID="ddlstatus" runat="server" Visible="false">
                  <asp:ListItem Selected="True" Text="--Select--" Value="--Select--"></asp:ListItem>
            </asp:DropDownList>
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
          <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTglBerangkatAkhir" >
           </ajax:CalendarExtender>
            <asp:Button ID="btncari" runat="server" Text="Search" OnClick="btncari_Click" />
            &nbsp;&nbsp;
        </p>
           <%-- <span>
                <asp:TextBox ID="txtTglBerangkat" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="txtTglBerangkat_CalendarExtender" runat="server"
                    TargetControlID="txtTglBerangkat">
                </asp:CalendarExtender>
                <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" />
            </span>--%>
        </h3>
        <div style="border: 1px solid #e3e3e3">
            <asp:GridView ID="gvDirect"
                runat="server"
                Visible="false"
                AutoGenerateColumns="False"
                CellPadding="4"
                ForeColor="#333333"
                GridLines="None"
                AllowSorting="True"
                OnSorting="gvDirect_Sorting"
              
                OnRowCommand="gvDirect_RowCommand"
                OnRowDataBound="gvDirect_RowDataBound"
                OnPageIndexChanging="gvDirect_PageIndexChanging"
                OnRowCancelingEdit="gvDirect_RowCancelingEdit"
                CssClass="fixit"
                AllowPaging="True"
               PagerSettings-Mode="NumericFirstLast"
                >
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="noSPD" HeaderText="No SPD" ReadOnly="True" SortExpression="noSPD" />
                    <asp:BoundField DataField="namaLengkap" HeaderText="Nama" ReadOnly="True" SortExpression="namaLengkap" />
                    <asp:BoundField DataField="tujuan" HeaderText="Tujuan" ReadOnly="True" SortExpression="Tujuan" />
                    <asp:BoundField DataField="keperluan" HeaderText="Keperluan" ReadOnly="True" SortExpression="ketKeperluan" />
                    <asp:BoundField DataField="tglBerangkat" HeaderText="Berangkat" ReadOnly="True" SortExpression="tglBerangkat" DataFormatString="{0:dd MMMM yyyy}" HtmlEncode="false" />
                    <asp:BoundField DataField="status" HeaderText="Status" ReadOnly="True" SortExpression="status" />
                    <asp:BoundField DataField="tglExpired" HeaderText="Tgl Expired" ReadOnly="True" SortExpression="tglExpired" DataFormatString="{0:dd MMMM yyyy}" HtmlEncode="false" />
                  
                    <asp:CheckBoxField DataField="isCancel" HeaderText="isCancel" HeaderStyle-CssClass="hideIt" ItemStyle-CssClass="hideIt" ReadOnly="True" SortExpression="isCancel" />
                    <asp:TemplateField ItemStyle-CssClass="fright" ControlStyle-Width="60" ItemStyle-Width="150">
                        <ItemTemplate>
                            <asp:Button ID="btnCancel" runat="server"
                                CommandName="Cancel"
                                CommandArgument='<%# Eval("noSPD") %>'
                                OnClientClick="if (!confirm('Are you sure?')) return false;"
                                Text="Cancel" />
                            <asp:Button ID="btnDetail" runat="server"
                                CommandName="Detail"
                                CommandArgument='<%# Eval("noSPD") %>'
                                Text="Detail" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" CssClass="GridPager" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>
        </div>
    </div>

</asp:Content>
