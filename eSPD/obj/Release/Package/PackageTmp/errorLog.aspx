<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="errorLog.aspx.cs" Inherits="eSPD.errorLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        table td {
            vertical-align: top !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gridViewError" runat="server" AutoGenerateColumns="false" EmptyDataText="No files uploaded" Visible="false">
                <Columns>
                    <asp:BoundField DataField="Text" HeaderText="File Name" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick="lnkDownload_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="LinqDataSource1" DataKeyNames="ErrorId">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" />
                    <asp:BoundField DataField="ErrorId" HeaderText="ErrorId" SortExpression="ErrorId" ReadOnly="True" />
                    <asp:BoundField DataField="EmailTo" HeaderText="EmailTo" SortExpression="EmailTo" />
                    <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject" />
                    <asp:BoundField DataField="Body" HeaderText="Body" SortExpression="Body" HtmlEncode="false" />
                    <asp:CheckBoxField DataField="Status" HeaderText="Status" SortExpression="Status" />
                    <asp:BoundField DataField="ErrorMessage" HeaderText="ErrorMessage" SortExpression="ErrorMessage" />
                    <asp:BoundField DataField="ErrorDate" HeaderText="ErrorDate" SortExpression="ErrorDate" DataFormatString="{0:d-MMM-yy HH:mm}" />
                </Columns>
            </asp:GridView>
            <asp:LinqDataSource 
                ID="LinqDataSource1" 
                runat="server" 
                ContextTypeName="eSPD.dsSPDDataContext" 
                EntityTypeName="" OrderBy="ErrorDate desc" 
                TableName="MailErrors" EnableDelete="True">
            </asp:LinqDataSource>
        </div>
        <asp:Button runat="server" ID="btnBreak" Text="Break App, recycle IT (hati2 brayy)" OnClick="btnBreak_Click" />
    </form>
</body>
</html>
