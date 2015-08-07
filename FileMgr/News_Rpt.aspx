<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News_Rpt.aspx.cs" Inherits="FileMgr_News_Rpt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html  xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Language" content="zh-tw" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>公佈欄</title>
<style type ="text/css" >
<!--

table
	{
	 font-size: 12pt;
	 font-family: 新細明體;
	}


-->
</style>
</head>

<body>
<form id="form" runat ="server">
    <asp:HiddenField ID="HFD_SER_NO" runat="server" />
<p align="center"><font face="標楷體" size="5">公告訊息</font></p>
<div align="center">
	<table border="2" width="720" id="table1" cellspacing="0" cellpadding="2" style="border-collapse: collapse;" bordercolor="#111111">
		<tr>
			<td>
			<table border="1" width="100%" id="table2" cellspacing="0" cellpadding="2" style="border-collapse: collapse;" bordercolor="#111111">
				<tr>
					<td height="35" align="left">主題 : 
                        <asp:Label ID="FD_news_subject" runat="server" Text=""></asp:Label></td>
				</tr>
				<tr>
					<td height="35"  align="left">發佈日期 : 
                        <asp:Label ID="FD_news_RegDate" runat="server" Text=""></asp:Label></td>
				</tr>
				<tr>
					<td height="35"  align="left">發佈單位 : 
                        <asp:Label ID="FD_dept_desc" runat="server" Text=""></asp:Label></td>
				</tr>
				<tr>
					<td  align="left">內容 :<br>
					<table border="0" width="100%" id="table3" cellspacing="0" cellpadding="6" height="850">
						<tr>
							<td valign="top">
							<table border="0" cellpadding="0" cellspacing="12" style="border-collapse: collapse" bordercolor="#111111" width="100%" id="AutoNumber14">
               	  <tr>
                	<td width="100%" style="LINE-HEIGHT: 120%;letter-spacing:1;">
                        <asp:Label ID="lbl_FileDownLoad" runat="server" Text=""></asp:Label>
                	</td>
                 </tr>
                </table>
							</td>
						</tr>
					</table>
					</td>
				</tr>
			</table>
			</td>
		</tr>
	</table>
</div>
</form>
</body>

</html>