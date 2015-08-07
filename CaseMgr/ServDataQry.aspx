<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="ServDataQry.aspx.cs"
    Inherits="CaseMgr_ServDataQry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>志願服務紀錄</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript">
        // menu控制
        $(document).ready(function () {

            InitMenu();

        });

        //新增時不要出現的物件在此控制
        var Mode = $('#HFD_Mode').val();
        if (Mode == 'ADD') {
            $('.HideWhenAdd').hide()
        }
    </script>
    <style type="text/css">
        .style1 {
            height: 28px;
        }
    </style>
</head>
<body class="body">
    <form id="Form1" runat="server" submitdisabledcontrols="False">
        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        </div>
        <input type="hidden" id="RefValue" name="RefValue" runat="server" />
        <asp:Label runat="server" ID="Toolbar"></asp:Label>

        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            <asp:Literal ID="litTitle" runat="server" Text="志願服務"></asp:Literal>
        </h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">

            <tr>
                <th align="right" width="5%">志工編號：</th>
                <td align="left" width="10%">
                    <asp:TextBox runat="server" ID="txtName" Width="40mm" CssClass="font9"></asp:TextBox>
                    <td align="left" width="30%">
                        <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                        <%--<asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" 
                onclick="btnPrint_Click" /> --%>
                    </td>

                    <tr>
                        <td colspan="7">
                            <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
        </table>
    </form>
</body>
</html>
