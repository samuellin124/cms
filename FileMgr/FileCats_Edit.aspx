<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileCats_Edit.aspx.cs" Inherits="FileMgr_FileCats_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改資料夾</title>
    <meta content="text/html; charset=big5" http-equiv="Content-Type" />
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
</head>
<body class="body">
    <form name="form" action="" method="post" runat="server">
        <div align="center">
            <center>
                <table border="0" width="100%" class="table_v">
                    <tr>
                        <td width="100%" class="font11" style="background-color: rgb(55,111,135); color: rgb(255,255,255);
                            font-size: 16pt;
                            filter: Alpha(Opacity=100, style=1)" height="25">
                            <font color="#ffffff">修改資料夾</font></td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <asp:HiddenField ID="HFD_FileCat_ParentID" runat="server" />
                            <asp:HiddenField ID="HFD_Filecat_Id" runat="server" />
                            <div align="center">
                                <center>
                                    <table width="100%" border="1">
                                        <tr>
                                            <th align="right" width="15%">
                                                資料夾名稱:
                                            </th>
                                            <td valign="center" width="85%" align="left">
                                                <asp:TextBox ID="FD_FileCat_Name" Width="240" runat="server" MaxLength="30"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="right" width="15%">
                                                排序號:
                                            </th>
                                            <td align="left" width="85%">
                                                <asp:TextBox ID="FD_FileCat_SrotID" Width="24px" CssClass="font9" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="function">
                                        <asp:Button ID="btnSave" runat="server" Text="存檔" OnClick="btnSave_Click" 
                                             CssClass="npoButton npoButton_Modify" />
                                        <asp:Button ID="btnCancle" runat="server" Text="取消" OnClientClick="javascript:window.close();"
                                             CssClass="npoButton npoButton_Exit" UseSubmitBehavior="False" />
                                    </div>
                                </center>
                            </div>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </form>
</body>
</html>
