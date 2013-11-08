<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Client.Index" %>

<%@ Register assembly="WebEdition" namespace="ThinkGeo.MapSuite.WebEdition" tagprefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <cc1:Map ID="MainMap" runat="server" height="700px" width="1200px" CurrentScale="NaN">
        </cc1:Map>
        <asp:TextBox ID="TextBox" runat="server" Width="1000px"></asp:TextBox>
    </form>
</body>
</html>
