<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Act_Edit.aspx.cs" Inherits="SysMgr_Act_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>好消息熱線中心志願服務紀錄表</title>
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
                inputField: "txtBegDate",   // id of the input field
                button: "imgBegDate",     // 與觸發動作的物件ID相同
            });
          
            Calendar.setup({
                inputField: "txtEndDate",   // id of the input field
                button: "imgEndDate"     // 與觸發動作的物件ID相同

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
        <asp:HiddenField ID="HFD_UID" runat="server" />
        <asp:HiddenField ID="HFD_Mode" runat="server" />
        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        </div>
        <input type="hidden" id="RefValue" name="RefValue" runat="server" />
        <asp:Label runat="server" ID="Toolbar"></asp:Label>

        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />活動管理</h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
           <%-- <tr>
                <th width="12%" align="right">志工：
                </th>
                <td width="20%" align="left">
                    <asp:DropDownList ID="ddlUserName" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>--%>
            <tr>
                <th  align="right" >服務類別：
                </th>
                <td align="left"   width="50%">
                    <asp:DropDownList ID="ddlClassUid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlClassUid_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
                <th align="right">服務項目：
                </th>
                <td  align="left">
                    <asp:DropDownList ID="ddlItemUid" runat="server">
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
                <th align="right" width="12%">活動日期：
                </th>
                <td align="left" width="10%">
                    <asp:TextBox ID="txtBegDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgBegDate" alt="" src="../images/date.gif" />
                </td>
            </tr>

            <tr>
                <th align="right" >結束日期：
                </th>
             <td align="left" >
                    <asp:TextBox ID="txtEndDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgEndDate" alt="" src="../images/date.gif" />
                </td>
            </tr>

             <tr>
                <th align="right" >說明：</th>
                <td align="left" >
                    <asp:TextBox ID="txtRemark" CssClass="font9" Width="200mm" runat="server" 
                        TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <th align="right" >名額：</th>
                <td align="left" >
                    <asp:TextBox ID="txtPlaces" CssClass="font9" Width="28mm" runat="server"></asp:TextBox>
                </td>
            </tr>



            <tr>
                <td align="right"  colspan="8">
                    <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnUpdate" class="npoButton npoButton_Save" runat="server" Text="儲存"
                        OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnDelete" class="npoButton npoButton_Del" runat="server" Text="刪除"
                        OnClick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');" />
                    <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" Text="離開"
                OnClick="btnExit_Click" />
                    <%--<asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" 
                onclick="btnPrint_Click" /> --%>
                </td>
            </tr>
            </table>
    </form>
</body>
</html>
