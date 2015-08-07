<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileUpLoad_Edit.aspx.cs"
    Inherits="FileMgr_FileUpLoad_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>檔案更新上傳</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <link rel="stylesheet" type="text/css" href="../include/dms.css" />
</head>
<body class="gray">
    <form name="form" action="" method="post" runat="server">
        <div align="center">
            <asp:HiddenField ID="HFD_UPLOAD_ID" runat="server" />
            <center>
                <table border="0" width="100%" cellspacing="0" cellpadding="0" style="border-collapse: collapse;border-color:#1111111;">
                    <tr>
                        <td width="100%" class="font11" style="background-color: rgb(55,111,135); color: rgb(255,255,255);
                            filter: Alpha(Opacity=100, style=1)" height="25">
                            <font color="#ffffff">檔案更新上傳 (檔案大小限制 10MB)</font>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <div align="center">
                                <center>
                                    <table width="100%" border="1" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td align="right" width="15%" height="22">
                                                <font color="#000080">檔案說明:</font>
                                            </td>
                                            <td valign="middle" width="85%" height="18" align="left">
                                                &nbsp;<asp:TextBox ID="FD_Upload_Desc" runat="server" Width="385px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" width="15%" height="22">
                                                <font color="#000080">上傳檔案:</font>
                                            </td>
                                            <td valign="middle" width="85%" height="18" align="left">
                                                &nbsp;<asp:FileUpload ID="FileUpload1" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%" height="15" class="font9" colspan="2" align="left">
                                                <asp:Button ID="btnSave" runat="server" Text="存檔" OnClick="btnSave_Click" CssClass="cbutton" />
                                                <asp:Button ID="btnCancle" runat="server" Text="取消" OnClientClick="window.close();"
                                                    CssClass="cbutton" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                            <div style="text-align:left;">
                                <asp:Label ID="FD_ShowTip" runat="server"></asp:Label>      
                            </div>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </form>
</body>
</html>
