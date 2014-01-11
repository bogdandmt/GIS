<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SQLQuery.aspx.cs" Inherits="WebClient.SQLQuery" %>

<%@ Register assembly="WebEdition" namespace="ThinkGeo.MapSuite.WebEdition" tagprefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 157px;
        }
        .auto-style2 {
            width: 145px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <center>
        <div>

            <table style="width: 500px;">
                <tr>
                    <td class="auto-style1">
                        <asp:ListBox ID="atrrListBox" runat="server" Width="100px"></asp:ListBox>
                    </td>
                    <td class="auto-style2">
                        <asp:Button ID="eqButton" runat="server" Text="=" Width="40px" OnClick="EqButtonClick" style="height: 26px" />
                        <asp:Button ID="notEqButton" runat="server" Text="&lt;&gt;" Width="40px" OnClick="NotEqButtonClick" />
                        <br />
                        <asp:Button ID="greaterButton" runat="server" Text="&gt;" Width="40px" OnClick="GreaterButtonClick" />
                        <asp:Button ID="greaterEqButton" runat="server" Text="&gt;=" Width="40px" OnClick="GreaterEqButtonClick" />
                        <br />
                        <asp:Button ID="lessButton" runat="server" Text="&lt;" Width="40px" OnClick="LessButtonClick" />
                        <asp:Button ID="lessEqButton" runat="server" Text="&lt;=" Width="40px" OnClick="LessEqButtonClick" />
                    </td>
                    <td>
                        <asp:TextBox ID="valueTextBox" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
            </table>

            <br />
            <asp:ScriptManager ID="ScriptManager" runat="server">
            </asp:ScriptManager>
            <cc1:Map ID="map" runat="server" BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px" height="480px" width="100%">
            </cc1:Map>

        </div>
            </center>
    </form>
</body>
</html>
