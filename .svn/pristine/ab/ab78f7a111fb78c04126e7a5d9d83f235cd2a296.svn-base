<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="frmReportSLA.aspx.cs" Inherits="eSPD.frmReportSLA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p style="height:20px"></p>
    <p style="margin:auto;margin-bottom:10px;text-align:center;">
        Bulan Keberangkatan
        &nbsp;
        <asp:DropDownList ID="ddlBulanKeberangkatan" runat="server">
            <asp:ListItem Text=" - Pilih Bulan -" Value="0"></asp:ListItem>
            <asp:ListItem Text="Januari" Value="1"></asp:ListItem>
            <asp:ListItem Text="Februari" Value="2"></asp:ListItem>
            <asp:ListItem Text="Maret" Value="3"></asp:ListItem>
            <asp:ListItem Text="April" Value="4"></asp:ListItem>
            <asp:ListItem Text="Mei" Value="5"></asp:ListItem>
            <asp:ListItem Text="Juni" Value="6"></asp:ListItem>
            <asp:ListItem Text="Juli" Value="7"></asp:ListItem>
            <asp:ListItem Text="Agustus" Value="8"></asp:ListItem>
            <asp:ListItem Text="September" Value="9"></asp:ListItem>
            <asp:ListItem Text="Oktober" Value="10"></asp:ListItem>
            <asp:ListItem Text="November" Value="11"></asp:ListItem>
            <asp:ListItem Text="Desember" Value="12"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;
        <asp:Button ID="btnFind" runat="server" Text="Find" width="70px" OnClick="btnFind_Click"/>
    </p>

    <asp:GridView ID="grvSLA" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="grvSLA_PageIndexChanging">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField HeaderText="No SPD" DataField="noSPD" />
            <asp:BoundField HeaderText="Nama" DataField="namaLengkap" />
            <asp:BoundField HeaderText="Tempat Tujuan" DataField="tempatTujuan" />
            <asp:BoundField HeaderText="Keperluan" DataField="keperluan" />
            <asp:BoundField HeaderText="Tgl Berangkat" DataField="tglBerangkat" DataFormatString="{0:MM-dd-yyyy}" />
            <asp:BoundField HeaderText="Tgl Kembali" DataField="tglKembali" DataFormatString="{0:MM-dd-yyyy}" />
            <asp:TemplateField HeaderText="Approver 1">
                <ItemTemplate>
                    <asp:label ID="apr1" runat="server" Text='<%#Bind("approverName1") %>'></asp:label> <br />
                    <asp:label ID="apd1" runat="server" Text='<%#Bind("approverDate1") %>'></asp:label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Approver 2">
                <ItemTemplate>
                    <asp:label ID="apr2" runat="server" Text='<%#Bind("approverName2") %>'></asp:label> <br />
                    <asp:label ID="apd2" runat="server" Text='<%#Bind("approverDate2") %>'></asp:label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Approver 3">
                <ItemTemplate>
                    <asp:label ID="apr3" runat="server" Text='<%#Bind("approverName3") %>'></asp:label> <br />
                    <asp:label ID="apd3" runat="server" Text='<%#Bind("approverDate3") %>'></asp:label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Approver 4">
                <ItemTemplate>
                    <asp:label ID="apr4" runat="server" Text='<%#Bind("approverName4") %>'></asp:label> <br />
                    <asp:label ID="apd4" runat="server" Text='<%#Bind("approverDate4") %>'></asp:label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Approver 5">
                <ItemTemplate>
                    <asp:label ID="apr5" runat="server" Text='<%#Bind("approverName5") %>'></asp:label> <br />
                    <asp:label ID="apd5" runat="server" Text='<%#Bind("approverDate5") %>'></asp:label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Approver Tempat Tujuan">
                <ItemTemplate>
                    <asp:label ID="NamaApproverTujuan" runat="server" Text='<%#Bind("NamaApproverTujuan") %>'></asp:label> <br />
                    <asp:label ID="isApprovedDate" runat="server" Text='<%#Bind("isApprovedDate") %>'></asp:label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Total" DataField="totalLeadTime" />
            <asp:BoundField HeaderText="Status" DataField="status" />
        </Columns>
        <EditRowStyle BackColor="#7C6F57" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" />
        <RowStyle BackColor="#E3EAEB" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView>
</asp:Content>
