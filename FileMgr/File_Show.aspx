<%@ Page Language="C#" AutoEventWireup="true" CodeFile="File_Show.aspx.cs" Inherits="FileMgr_File_Show" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html  xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>檔案顯示</title>
</head>
<body bgcolor="#dcdcba">
<form id="form1" runat ="server">
<a href="#" onclick="window.close()">
<img alt="" border="0" src="../images/icon-close.gif" width="92" height="26" align="right" />
</a>
<p align="center">
    <asp:Label ID="lblFileDownLoad" runat="server" Text=""></asp:Label>
 </p>
 </form>
</body>
</html>