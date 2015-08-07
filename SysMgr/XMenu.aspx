<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XMenu.aspx.cs" Inherits="XMenu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>系統選單</title>
    <link href="../include/frame.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //隱藏所有第二層選單
            $('.submenu').hide();
            $('.menu').click(function () {
                var _this = $(this);
                if (_this.next('.submenu').is(":visible")) {
                    //隱藏指定第二層選單
                    _this.next('.submenu').hide();
                } else {
                    //隱藏所有第二層選單
                    $('.submenu').hide();
                    //顯示指定第二層選單
                    _this.next('.submenu').show();
                }
            });
        });					   
    </script>
</head>
<body class="menu_body">
    <asp:literal ID="lblMenuContainer" runat="server"></asp:literal>
    <br/>
    <center>
        <asp:Label ID="lblRemoteAddr" runat ="server" ></asp:Label>
    </center>
</body>
</html>


