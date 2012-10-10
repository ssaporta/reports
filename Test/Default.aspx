<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Test.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        URL: <asp:TextBox ID="tbUrl" runat="server" Text="http://localhost:59404/"></asp:TextBox><br />
        Territories: <asp:TextBox ID="tbTerritories" runat="server"></asp:TextBox><br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" />
        <br />
        <a runat="server" id="link" target="_blank">Go to reports</a>
    </div>
    </form>
</body>
</html>
