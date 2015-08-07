<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News_Show.aspx.cs" Inherits="FileMgr_News_Show" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公告訊息</title>
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
<form id="form1" runat ="server">
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;"/></a>
    </div>
    <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        公告訊息
    </h1>
    <asp:HiddenField ID="HFD_NewsUID" runat="server" />
<table border="0" width="98%" id="AutoNumber1" class="table_v">
    <tr>
        <td align="left" width="75%" height="10" style="font-size: 11pt;">
            <b>
                訊息標題：<asp:Label ID="lblNewsSubject" runat="server" Text=""></asp:Label>
            </b>
        </td>
        <!--td width="13%" height="10" align="right">
            <asp:HyperLink ID="FD_PRINT_LINK" runat="server" Target="_blank" CssClass="linkb"><img alt="" border="0" src="../images/print.gif" width="20" height="20" align="absmiddle"  /> 列印</asp:HyperLink>
        </td>
        <td width="11%" height="10" align="right">
            ‧<a href="javascript:history.back()" class="PrizeMenu">回上一頁</a>
        </td-->
    </tr>
    <tr>
        <td align="left" width="99%" colspan="3">
            訊息類別：<asp:Label ID="lblNewsType" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left" width="99%" colspan="3">
            發佈日期：<asp:Label ID="lblNewsRegDate" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left" width="99%" colspan="3">
            發佈區間：<asp:Label ID="lblPeriod" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left" width="99%" colspan="3">
            發佈單位：<asp:Label ID="lblDeptName" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left" width="99%" colspan="3">
            訊息內容：<asp:Label ID="lblNewsContent" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <!--tr>
        <td width="99%" colspan="3">
            <table border="0" cellpadding="0" cellspacing="12" style="border-collapse: collapse;
                border-color: #111111" width="100%" id="AutoNumber14">
                <tr>
                    <td width="100%" align="left">
                        <asp:Label ID="lbl_FileDownLoad" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="left" width="99%" colspan="3">
            <asp:Label ID="lbl_DocDownLoad" runat="server" Text=""></asp:Label>
        </td>
    </tr-->
</table>
</form>
</body>
</html>

