<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Password_Edit.aspx.cs" Inherits="FileMgr_Password_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>使用者密碼變更</title>
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
    <script type="text/javascript">
        $.swapImage(".swapImage");
        $.swapImage(".swapImageClick", true, true, "click");
        $.swapImage(".swapImageDoubleClick", true, true, "dblclick");
        $.swapImage(".swapImageSingleClick", true, false, "click");
        $.swapImage(".swapImageDisjoint");
    </script>
    <form name="form1" runat="server">
        <asp:HiddenField ID="txtOrgID" runat="server" />
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;"/></a>
        </div>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        <asp:Literal ID="litTitle" runat="server">密碼修改</asp:Literal>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
           <tr>
               <th width="15%" align="right">使用者代號：</th>
               <td width="85%">
                   <asp:TextBox ID="txtUserID" Width="30mm" CssClass="font9T" ReadOnly="true" runat="server"></asp:TextBox>
               </td>
           </tr>
           <tr>
               <th align="right">使用者名稱：</th>
               <td>
                   <asp:TextBox ID="txtUserName" Width="30mm" CssClass="font9T" ReadOnly="true" runat="server"></asp:TextBox>
               </td>
           </tr>
           <tr>
               <th align="right"><font color="red">※舊密碼：</font> </th>
               <td>
                   <asp:TextBox ID="txtOldPassword" Width="30mm" MaxLength="20" runat="server" TextMode="Password"></asp:TextBox>
               </td>
           </tr>
           <tr>
               <th align="right"><font color="red">※新密碼：</font></th>
               <td>
                   <asp:TextBox ID="txtPassword" Width="30mm" MaxLength="20" runat="server" TextMode="Password"></asp:TextBox>
               </td>
           </tr>
           <tr>
               <th align="right"><font color="red">※確認密碼：</font></th>
               <td>
                   <asp:TextBox ID="txtCPassword" Width="30mm" MaxLength="20" runat="server" TextMode="Password"></asp:TextBox>
               </td>
           </tr>
           <tr>
               <td colspan="2" align="right">
                   <asp:Button runat="server" ID="btnUpdate" CssClass="button1 button1_save" Text="修改" onclick="btnUpdate_Click" />
               </td>
           </tr>
        </table>
    </form>
     <script type="text/javascript">
         function SwitchMenu() {
             var Switch_Status = parent.document.getElementsByTagName('frameset')[1].cols;
             if (Switch_Status == "200,*") {
                 parent.document.getElementsByTagName('frameset')[1].cols = '0,*';
             } else if (Switch_Status == "0,*") {
                 parent.document.getElementsByTagName('frameset')[1].cols = '200,*';
             }
         }
    </script>
</body>
</html>
