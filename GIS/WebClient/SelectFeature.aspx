<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectFeature.aspx.cs" Inherits="WebClient.SelectFeature" %>

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
        <cc1:Map ID="Map1" runat="server" height="480px" OnClick="Map1_Click" width="640px">
        </cc1:Map>
    </form>
</body>
</html>
