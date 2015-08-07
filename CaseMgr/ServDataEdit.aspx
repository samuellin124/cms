<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="ServDataEdit.aspx.cs"
    Inherits="CaseMgr_ServDataEdit" %>

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

            //初始化日曆
            initCalendar(['ServDate']);

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
        <input type="hidden" id="RefValue" name="RefValue" runat="server" />
        <asp:HiddenField ID="HFD_UID" runat="server" />
        <asp:HiddenField ID="HFD_Mode" runat="server" />
        <asp:HiddenField ID="HFD_ServDataUID" runat="server" />
        <asp:Label runat="server" ID="Toolbar"></asp:Label>
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            <asp:Literal ID="litTitle" runat="server" Text="志願服務紀錄"></asp:Literal>
        </h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th align="center" colspan="3">【志願服務紀錄】
                </th>
            </tr>
            <tr>
                <th align="right" style="width: 40mm">志工編號：
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtCName"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="center">服務日期：
                </th>
                <td colspan="2" class="style1">
                    <asp:TextBox ID="txtServDate" runat="server" onchange="CheckDateFormat(this, '服務日期'); ">
                    </asp:TextBox>
                    <img id="imgServDate" alt="" src="../images/date.gif" />
                </td>
            </tr>
            <tr>
                <th align="center" rowspan="2">行政服務
                </th>
                <th align="right">接聽真情之夜：
                </th>
                <td style="width: 150mm">
                    <asp:CheckBox ID="CK_Administrative1" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="right">資料整合鍵入：
                </th>
                <td>
                    <asp:CheckBox ID="CK_Administrative2" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="center" rowspan="4">活動服務
                </th>
                <th align="right">讀書會 ：
           
                </th>
                <td>
                    <asp:CheckBox ID="CK_Event1" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="right">在職教育：
                </th>
                <td>
                    <asp:CheckBox ID="CK_Event2" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="right">培訓服務：
                </th>
                <td>
                    <asp:CheckBox ID="CK_Event3" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="right">參與活動：
                </th>
                <td>
                    <asp:CheckBox ID="CK_Event4" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="center" rowspan="3">志工幹部服務
                </th>
                <th align="right">擔任組長：
                </th>
                <td>
                    <asp:CheckBox ID="CK_VolunteerCadre1" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="right">讀書會帶領人：
                </th>
                <td>
                    <asp:CheckBox ID="CK_VolunteerCadre2" runat="server" />
                </td>
            </tr>
            <tr>
                <th align="right" style="width: 45mm">擔任夜間掌門人：
                </th>
                <td>
                    <asp:CheckBox ID="CK_VolunteerCadre3" runat="server" />
                </td>
            </tr>
        </table>
        <div class="function">
            <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增"
                OnClick="btnAdd_Click" />
            <asp:Button ID="btnUpdate" class="npoButton npoButton_Save" runat="server" Text="儲存"
                OnClick="btnUpdate_Click" OnClientClick="return ChkData();" />
            <asp:Button ID="btnDelete" class="npoButton npoButton_Del" runat="server" Text="刪除"
                OnClick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');" />
            <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" Text="離開"
                OnClick="btnExit_Click" />
        </div>
    </form>
</body>
</html>
