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
        <cc1:Map ID="MainMap" runat="server" height="480px" width="640px">
        </cc1:Map>
    </form>
</body>
</html>
