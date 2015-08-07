<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectionData_Edit.aspx.cs" Inherits="SysMgr_SelectionData_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>選項資料管理</title>
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
    <form id="Form1" name="form" method="POST" runat="server">
    <asp:HiddenField ID="HFD_Mode" runat="server" />
    <asp:HiddenField ID="HFD_DataUID" runat="server" />
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
    </div>
    <div id="container">
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            修改選項資料
        </h1>
        <table width="100%" class="table_v">
            <tr>
                <th width="30mm" align="right">
                    選項群組:
                </th>
                <td width="10%">
                    <asp:TextBox runat="server" ID="txtCaseGroup" Width="40mm" MaxLength="10" ReadOnly="true"></asp:TextBox>
                </td>
                <th width="30mm" align="right">
                    排序:
                </th>
                <td width="10%">
                    <asp:TextBox runat="server" ID="txtCaseID" Width="30mm" MaxLength="10" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="30mm" align="right">
                    選項名稱:
                </th>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtCaseName" Width="180mm" TextMode ="MultiLine"  Rows ="3"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="function">
            <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增"
                OnClick="btnAdd_Click" />
            <asp:Button ID="btnUpdate" class="npoButton npoButton_Modify" runat="server" Text="修改"
                OnClick="btnUpdate_Click" />
            <asp:Button ID="btnDelete" class="npoButton npoButton_Del" runat="server" Text="刪除"
                OnClick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');" />
            <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" Text="離開"
                OnClick="btnExit_Click" />
        </div>
    </div>
    </form>
</body>
</html>
