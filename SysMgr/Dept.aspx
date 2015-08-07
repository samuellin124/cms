<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dept.aspx.cs" Inherits="SysMgr_Dept" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>部門管理</title>
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
    <form id="Form1" name="form" runat="server">
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;"/></a>
    </div>
    <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        部門管理
    </h1>
    <table width="100%" class="table_v">
        <tr>
            <th align="right" width="15%">部門名稱：</th>
            <td width="20%" align="left">
                <asp:TextBox runat="server" ID="txtDeptName" Width="60mm" CssClass="font9"></asp:TextBox>
             </td>
             <th width="15%" align="right">部門描述：</th>
             <td width="20%" align="left">
                 <asp:TextBox runat="server" ID="txtDeptDesc" Width="60mm" CssClass="font9"></asp:TextBox>
             </td>
             <td width="30%" align="right">
                 <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="查詢" OnClick="btnQuery_Click"/>
                 <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增" OnClick="btnAdd_Click"/>
             </td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
