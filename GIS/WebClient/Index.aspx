<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebClient.Index" %>

<%@ Register assembly="WebEdition" namespace="ThinkGeo.MapSuite.WebEdition" tagprefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="MyStyles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="centered marginTop" >
        Пункти харчування
    </div>
        <br/>
        <br/>
        <asp:Menu ID="Menu1" orientation="Horizontal" runat="server" OnMenuItemClick="Menu1_MenuItemClick">
            <Items>
                <asp:MenuItem Text="item1" Value="item1"></asp:MenuItem>
                <asp:MenuItem Text="item2" Value="item2"></asp:MenuItem>
                <asp:MenuItem Text="Item3" Value="Item3"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <br/>
        <asp:TreeView ID="TreeView1" runat="server" CssClass="fromLeft marginRight">
            <Nodes>
                <asp:TreeNode Text="shape1" Value="shape1" ShowCheckBox="True">
                </asp:TreeNode>
                <asp:TreeNode Text="shape2" Value="shape2" ShowCheckBox="True"></asp:TreeNode>
                <asp:TreeNode Text="shape3" Value="shape3" ShowCheckBox="True"></asp:TreeNode>
            </Nodes>
        </asp:TreeView>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <cc1:Map ID="MainMap" runat="server" height="773px" width="1018px">
        </cc1:Map>
    </form>
</body>
</html>
