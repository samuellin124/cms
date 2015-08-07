<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Organization_Edit.aspx.cs"
    Inherits="SysMgr_Organization_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>機構部門管理</title>
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
    <asp:HiddenField ID="HFD_OrgUID" runat="server" />
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;"/></a>
    </div>
    <div id="container">
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            修改機構資料
        </h1>
        <table width="100%" class="table_v">
            <tr>
                <th width="20%">
                    機構代碼:&nbsp;
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOrgID" Width="80" ReadOnly="true"
                        MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    機構名稱:&nbsp;
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOrgName" Width="160" MaxLength="60"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    機構簡稱:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOrgShortName" Width="160" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    連絡人:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtContactor" Width="160" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    信箱:&nbsp;
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtEmail" Width="250" MaxLength="40"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    住址:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtAddress" Width="250" MaxLength="60"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    電話:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtTel" Width="80" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    傳真:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtFax" Width="80" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    網站位址:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtIP" Width="250" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20%">
                    密碼使用天數:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPasswordDay" Width="12" MaxLength="3">0</asp:TextBox>
                    (若設為 0 則密碼可無限期使用)
                </td>
            </tr>
            <tr>
                <th width="20%">
                    服務紀錄輸入截止日:
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtLimitDay" Width="30" >0</asp:TextBox>
                </td>
                </tr>
        </table>
        <div class="function">
            <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" 
                Text="新增" onclick="btnAdd_Click"/>
            <asp:Button ID="btnUpdate" class="npoButton npoButton_Modify" runat="server" 
                Text="修改" onclick="btnUpdate_Click"/>
            <asp:Button ID="btnDelete" class="npoButton npoButton_Del" runat="server" 
                Text="刪除" onclick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');"/>
            <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" 
                Text="離開" onclick="btnExit_Click"/>
        </div>
    </div>
    </form>
</body>
</html>
