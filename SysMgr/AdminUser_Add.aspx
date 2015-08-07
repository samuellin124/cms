<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminUser_Add.aspx.cs" Inherits="SysMgr_AdminUser_Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新增使用者設定</title>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
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
        });
    </script>
    <script type="text/javascript">
        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "txtPwdValidDate",   // id of the input field
                ifFormat: "%Y/%m/%d",       // format of the input field
                showsTime: false,
                timeFormat: "24",
                button: "imgPwdValidDate"     // 與觸發動作的物件ID相同
            });

                Calendar.setup({

                    inputField: "txtStartData",   // id of the input field
                    button: "imgStartData"     // 與觸發動作的物件ID相同
                });
                Calendar.showYearsCombo(true);


            
        }
    </script>
</head>
<body class="body">
    <form id="Form1" runat="server">
    <div id="menucontrol">
        <a href="#">
            <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}"
                onclick="SwitchMenu();return false;" /></a> <a href="#">
                    <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}"
                        onclick="SwitchMenu();return false;" /></a>
    </div>
    <asp:HiddenField runat="server" ID="HFD_Uid" />
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            新增使用者設定
        </h1>
    <table width="100%" class="table_v">
        <tr>
            <th width="20%" align="right">
                帳號：
            </th>
            <td width="25%">
                <asp:TextBox runat="server" ID="txtUserID" ></asp:TextBox>
            </td>
            <th width="20%" align="right">
                使用者名稱：
            </th>
            <td width="25%">
                <asp:TextBox runat="server" ID="txtUserName" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th align="right">
                部門名稱：
            </th>
            <td align="left">
                <asp:DropDownList ID="ddlDept" DataTextField="DeptName" DataValueField="DeptID" runat="server">
                </asp:DropDownList>
            </td>
            <th width="15%" align="right">
                使用者群組：
            </th>
            <td width="25%" align="left">
                <asp:DropDownList ID="ddlGroup" DataTextField="GroupName" DataValueField="uid" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th align="right">
                密碼：
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtPassword" ></asp:TextBox>
            </td>
            <th align="right">
                再確認密碼：
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtRePassword" ></asp:TextBox>
            </td>
        </tr>
                    </tr>
            <tr>
                <th align="right">到職日：
                </th>
                <td>
                    <asp:TextBox ID="txtStartData" CssClass="font9" Width="30mm" runat="server"
                        onchange="CheckDateFormat(this, '到職日');"></asp:TextBox>
                    <img id="imgStartData" alt="" src="../images/date.gif" />
                </td>
                <th align="right">生日：
                </th>
                <td  align="left">
                    <asp:DropDownList ID="ddlMonth" runat="server" Height="25px"></asp:DropDownList>
                    月<asp:DropDownList ID="ddlDay" runat="server"></asp:DropDownList>日
                </td>
            </tr>
        <tr>
            <th align="right">
                密碼最後有效日期：
            </th>
            <td>
                <asp:TextBox ID="txtPwdValidDate" Width="30mm" runat="server" 
                    onchange="CheckDateFormat(this, '密碼最後有效日期');"></asp:TextBox>
                <img id="imgPwdValidDate" alt="" src="../images/date.gif" />
            </td>
            <th align="right">
                電子郵件地址：
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtEmail" Width="180px" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th align="right">
                手機號碼：
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtMobilNo"></asp:TextBox>
            </td>
            <th align="right">
                使用狀態：
            </th>
            <td align="left">
                <asp:RadioButtonList ID="rdoIsUse" runat="server" RepeatDirection="Horizontal" 
                    CssClass="table_noborder">
                    <asp:ListItem Selected="True">使用</asp:ListItem>
                    <asp:ListItem>停用</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th align="right">
                備註：
            </th>
            <td align="left">
                <asp:TextBox runat="server" ID="txtMemo" ></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="function">
        <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" 
            Text="新增" onclick="btnAdd_Click"/>
        <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" 
            Text="離開" onclick="btnExit_Click"/>
    </div>
    </form>
</body>
</html>
