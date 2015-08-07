<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="ServicePoints.aspx.cs" Inherits="SysMgr_ServicePoints" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>志工服務紀錄表</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript">

        // menu控制
        $(document).ready(function () {
        }); //end of ready()

        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "txtsDate",   // id of the input field
                button: "imgBegCreateDate"     // 與觸發動作的物件ID相同

            });
            Calendar.setup({

                inputField: "txteDate",   // id of the input field
                button: "imgEndCreateDate"     // 與觸發動作的物件ID相同
            });
            Calendar.showYearsCombo(true);
        }
        </script>
    </head>
<body class="body">
    <form id="Form1" name="form" runat="server">
        <asp:HiddenField ID="HFD_Area" runat="server" />
        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        </div>
        <h1><img src="../images/h1_arr.gif" alt="" width="10" height="10" /> 志工服務紀錄表</h1>

        <table width="100%" class="table_v">
            <tr>
                <th align="right" width="15%">志工姓名：</th>
                <td align="left" width="15%"><asp:DropDownList ID="ddlUserName" runat="server"></asp:DropDownList></td>
                <th align="right" width="15%">日期：</th>
                <td align="left" width="30%"">
                    <asp:TextBox ID="txtsDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgBegCreateDate" alt="" src="../images/date.gif" />～
                    <asp:TextBox ID="txteDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgEndCreateDate" alt="" src="../images/date.gif" />
                </td>
                <td align="left" width="25%">
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="查詢" OnClick="btnQuery_Click" />-
                    <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" OnClick="btnPrint_Click" />
                </td>
            </tr>
            </table>
            <table width="100%" class="table_v">
            <tr>
                <td >
                    <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </form>
    <td>
        <asp:Label ID="lblRpt" runat="server"></asp:Label>
    </td>
</body>
</html>
