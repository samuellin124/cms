<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VolunteerSchedule.aspx.cs"   Inherits="VolunteerSchedule" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>排班管理</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        // menu控制
        $(document).ready(function () {
            InitMenu();
            $('#ddlYear').val($('#HFD_Year').val());
            $('#ddlYear').bind('change', function () {
                $('#HFD_Year').val($(this).val());
            });
            $('#ddlMonth').val($('#HFD_Month').val());
            $('#ddlMonth').bind('change', function () {
                $('#HFD_Month').val($(this).val());
            });
        });
    </script>
</head>
<body class="body">
    <form id="Form1" runat="server">
    <asp:HiddenField ID ="HFD_Year" runat ="server" />
    <asp:HiddenField ID ="HFD_Month" runat ="server" />
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;"/></a>
    </div>
    <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        班表管理
    </h1>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
        <tr>
            <th align="right">
                志工姓名：
            </th>
            <td align="left">
                <asp:DropDownList ID="ddlVoluntee" runat="server" ></asp:DropDownList>
            </td>
            <th align="right">
                排班月份：
            </th>
            <td align="left">
                <asp:Label ID="lblYearMonth" runat="server" ></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" 
                    Text="查詢" OnClick="btnQuery_Click"/>
                <asp:Button ID="btnUpdate" class="npoButton npoButton_Modify" runat="server" 
                    Text="編輯" onclick="btnUpdate_Click"/>
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="7">
                <asp:Label ID="lblGridList" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="function">
        <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" 
            Text="存檔" onclick="btnAdd_Click"/>
    </div>
    </form>
</body>
</html>
