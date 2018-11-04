<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ResendNotification.aspx.cs" Inherits="eSPD.ResendNotification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:100%;">
        <tr>
            <td width="80" >
                No.SPD</td>
            <td>
                <asp:TextBox ID="txtNoSPD" runat="server" style="margin-left: 0px" 
                    Width="200px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnProses" runat="server" onclick="btnProses_Click" 
                    Text="Proses Old Version" Enabled="false"/>
                &nbsp;&nbsp;
                <asp:Button ID="btnProsesNew" runat="server" onclick="btnProsesNew_Click" 
                    Text="Proses New Version" />
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnResendAll" runat="server"  OnClientClick="return confirm('Are you sure want to Resend all notification ?')" Enabled="false"
                    Text="Resend All Old Version (Danger, Becarefull)" OnClick="btnResendAll_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnResendAllNew" runat="server"  OnClientClick="return confirm('Are you sure want to Resend all notification ?')" Enabled="false"
                    Text="Resend All New Version (Danger, Becarefull)" OnClick="btnResendAllNew_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
