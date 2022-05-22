<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form1.aspx.cs" Inherits="LD5.Form1" %>

<!DOCTYPE html>
<link href="Style1.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divStart"  runat="server">
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Vykdyti" />
            <br />
        </div>
        <div id="divData" runat="server">
            <br />
        </div>
        <div id="divResults" runat="server">
            <br />
            Pigiausios vertybės:<asp:Table ID="Table1" runat="server">
            </asp:Table>
            <br />
            Užsakymo riba:<br />
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
            <br />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Atrinkti" />
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Label" Visible="False"></asp:Label>
            <asp:Table ID="Table2" runat="server" Visible="False">
            </asp:Table>
            <asp:Label ID="Label3" runat="server" Text="Label" Visible="False"></asp:Label>
            <br />
        </div>
    </form>
    
</body>
</html>
