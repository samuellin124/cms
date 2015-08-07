<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="AttendanceHours.aspx.cs"
    Inherits="SysMgr_AttendanceHours" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>出勤管理系統</title>
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

            //初始化日曆
            //initCalendar(['ServDate']);

        });

        //新增時不要出現的物件在此控制
        var Mode = $('#HFD_Mode').val();
        if (Mode == 'ADD') {
            $('.HideWhenAdd').hide()
        }
        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "txtlogtDate",   // id of the input field
                button: "imglogtDate"     // 與觸發動作的物件ID相同

            });
            Calendar.setup({
                inputField: "txtLogoutDate",   // id of the input field
                button: "imgLogoutDate"     // 與觸發動作的物件ID相同

            });
            Calendar.showYearsCombo(true);
        }
        function dateDiff(date1, date2) {
            date1 = document.getElementById("txtlogtDate").value + ' ' + document.getElementById("ddlstrHour").value + ':' + document.getElementById("ddlstrMin").value + ':00';
            date2 = document.getElementById("txtlogtDate").value + ' ' + document.getElementById("ddlEndHour").value + ':' + document.getElementById("ddlEndMin").value + ':00';

            var type1 = typeof date1, type2 = typeof date2;
            if (type1 == 'string')
                date1 = stringToTime(date1);
            else if (date1.getTime)
                date1 = date1.getTime();
            if (type2 == 'string')
                date2 = stringToTime(date2);
            else if (date2.getTime)
                date2 = date2.getTime();
            //return (date1 - date2) / 1000; //結果是秒}
            var m = parseInt(((date2 - date1) / 1000) / 60);
            //document.getElementById("lblTime").innerHTML = '通話時間共' + m + '分' + ((date2 - date1) / 1000) % 60 + '秒';
            document.getElementById("lblTime").innerHTML = '通話時間共' + m + '分';
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
        <input type="hidden" id="RefValue" name="RefValue" runat="server" />
        <asp:HiddenField ID="HFD_UID" runat="server" />
        <asp:HiddenField ID="HFD_Mode" runat="server" />
        <asp:HiddenField ID="HFD_UserID" runat="server" />
        <asp:Label runat="server" ID="Toolbar"></asp:Label>
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            <asp:Literal ID="litTitle" runat="server" Text="出勤時數統計"></asp:Literal>
        </h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th align="right" width="15%">志工名稱：
                </th>
                <td width="15%" align="left">
                    <asp:DropDownList ID="ddlUserName" runat="server">
                    </asp:DropDownList>
                </td>


                <th align="right" width="10%">日期：</th>
                <td align="left" width="25%">
                    <asp:TextBox ID="txtlogtDate" runat="server" Width="120">
                    </asp:TextBox>
                    <img id="imglogtDate" alt="" src="../images/date.gif" />

                    <asp:TextBox ID="txtLogoutDate" runat="server" Width="120">
                    </asp:TextBox>
                    <img id="imgLogoutDate" alt="" src="../images/date.gif" />
                </td>
                <td align="left" width="20%">
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                    <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" OnClick="btnPrint_Click" />
                </td>
            </tr>

            <tr>
                <td colspan="5">
                    <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <div>
        <asp:Label ID="lblRpt" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
