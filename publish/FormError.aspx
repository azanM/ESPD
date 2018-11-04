<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormError.aspx.cs" Inherits="eSPD.FormError" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
           .bs-callout {
            -moz-border-bottom-colors: none;
            -moz-border-left-colors: none;
            -moz-border-right-colors: none;
            -moz-border-top-colors: none;
            border-color: #ebebeb;
            border-image: none;
            border-radius: 3px;
            border-style: solid;
            border-width: 1px 1px 1px 10px;
            margin: 20px 0;
            padding: 20px;
            background-color: #fdfdfd;
            font-family:'Comic Sans MS';
            
        }

           .bs-callout-danger {
    border-left-color: #ce4844;
    color:#ce4844;
}

            .bs-callout h4 {
                margin-bottom: 5px;
                margin-top: 0;
                padding-bottom: 1em;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="bs-callout bs-callout-danger">
            <h4>
                
         <asp:Label ID="lblMessageError" runat="server" Text=""></asp:Label>
            </h4>
        </div>
    </form>
</body>
</html>
