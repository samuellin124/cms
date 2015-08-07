<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="LogtimeQry.aspx.cs"
    Inherits="CaseMgr_LogtimeQry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>出勤管理</title>
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
        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "txtlogtDate",   // id of the input field
                button: "imglogtDate"     // 與觸發動作的物件ID相同

            });
            Calendar.showYearsCombo(true);
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
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />出勤管理</h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th align="right" width="8%">志工名稱：</th>
                <td align="left" width="10%"><asp:TextBox runat="server" ID="txtUserName" Width="40mm" CssClass="font9"></asp:TextBox></td>
                <th width="8%" align="right">上班日期：</th>
                <td align="left" width="8%">
                    <asp:TextBox ID="txtlogtDate" runat="server" Width="120"></asp:TextBox>
                    <img id="imglogtDate" alt="" src="../images/date.gif" />
                </td>
                <td align="left" width="30%">
                    <script language="javascript" type="text/javascript">
                        function goAdd() {
                            var goUrl = "LogtimeEdit.aspx?Mode=ADD&UserID=" + document.getElementById("txtUserName").value;
                            window.open(goUrl, "", "target=_blank,help=no,status=yes,resizable=yes,scrollbars=yes,scrolling=yes,scroll=yes,center=yes,width=900px,height=500px", "")
                        }
                    </script>
                    <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增" OnClientClick="goAdd();return false;" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                    <asp:Button ID="btnQueryEx" class="npoButton npoButton_Search" runat="server" Text="查詢單日多重登入名單" style="width:180px;" OnClick="btnQueryEx_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblScript" runat="server" Text=""></asp:Label>
                    
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
