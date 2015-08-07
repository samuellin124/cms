<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminGroup_Edit.aspx.cs"
    Inherits="SysMgr_AdminGroup_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>修改群組名稱設定</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        // menu控制
        $(document).ready(function () {
            InitMenu();
        });
    </script>
</head>
<body class="body">
    <form id="Form1" runat="server">
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;"/></a>
    </div>
    <asp:HiddenField runat="server" ID="HFD_Uid" />
    <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        修改群組名稱設定
    </h1>
    <table class="table_v" width="100%">
        <tr>
            <th width="15%" align="right">
                群組名稱：
            </th>
            <td width="20%">
                <asp:TextBox runat="server" ID="txtGroupName"></asp:TextBox>
            </td>
            <th width="15%" align="right">
                群組描述：
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtGroupDesc" Width="80mm"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="10%" align="right">
                群組範圍：
            </th>
            <td width="5%">
                <asp:DropDownList ID="ddlGroupArea" runat="server">
                    <asp:ListItem Selected="True" Value="全區">全區</asp:ListItem>
<%--                    <asp:ListItem Selected="True" Value="所屬分院">所屬分院</asp:ListItem>
                    <asp:ListItem Value="全台各院">全台各院</asp:ListItem>--%>
                </asp:DropDownList>
            </td>
            <th width="10%" align="right">
                是否使用：
            </th>
            <td align="left">
                <asp:RadioButtonList ID="rdoIsUse" runat="server" RepeatDirection="Horizontal" 
                    CssClass="table_noborder">
                    <asp:ListItem Selected="True">使用</asp:ListItem>
                    <asp:ListItem>禁用</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <div class="function">
        <asp:Button ID="btnUpdate" class="npoButton npoButton_Modify" runat="server" 
            Text="修改" onclick="btnUpdate_Click"/>
        <asp:Button ID="btnDelete" class="npoButton npoButton_Del" runat="server" 
            Text="刪除" onclick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');"/>
        <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" 
            Text="離開" onclick="btnExit_Click"/>
    </div>
    </form>
</body>
</html>
