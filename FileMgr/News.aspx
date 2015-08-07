<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="FileMgr_News" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>訊息管理</title>
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
    <div id="container">
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            訊息管理
        </h1>
        <table width="100%" class="table_v">
            <tr>
                <th width="83" align="right">
                    <span lang="en-us">標題:</span>
                </th>
                <td width="145">
                    <asp:TextBox ID="txtNewsSubject" Text="" Width="120" runat="server"></asp:TextBox>
                </td>
                <th width="79" align="right">
                    發佈單位:
                </th>
                <td width="103">
                    <asp:DropDownList ID="ddlDept" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="320">
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" 
                        Text="查詢" OnClick="btnQuery_Click"/>
                    <asp:Button ID="btnHistoryQuery" style="width:130px"  CssClass="npoButton npoButton_Search" runat="server" 
                        Text="歷史資料查詢" OnClick="btnHistoryQuery_Click" />
                    <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" 
                        Text="新增" onclick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td colspan="6" width="100%" align="left">
                    <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
