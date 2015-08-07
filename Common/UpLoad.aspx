<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpLoad.aspx.cs" Inherits="Common_UpLoad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>檔案上傳 </title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
</head>
<body class="body">
    <form id="Form1" runat="server">
        <asp:HiddenField ID="HFD_UploadPath" runat="server" />
        <asp:HiddenField ID="HFD_ObjectID" runat="server" />
        <asp:HiddenField ID="HFD_ApName" runat="server" />
        <asp:HiddenField ID="HFD_AllowType" runat="server" />
                <h1 style="height:2mm;"><img src="../images/h1_arr.gif" width="10" height="10" /> 檔案上傳 (檔案大小限制 10MB)</h1>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
                    <tr>
                        <th align="right" width="15%">檔案說明：</th>
                        <td align="left" width="85%" height="18">
                            &nbsp;<asp:TextBox ID="txtFileDesc" runat="server" Width="385px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="right" width="15%">上傳檔案：</th>
                        <td align="left" width="85%" height="18">
                            &nbsp;<asp:FileUpload ID="FileUpload" runat="server" Width="462px" />
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" colspan="2" align="center">
                            <asp:Button ID="btnSave" class="npoButton npoButton_Save" runat="server" 
                                 Text="存檔" onclick="btnSave_Click"/>
                            <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" 
                                 Text="離開" OnClientClick="window.close();" />
                        </td>
                    </tr>
                </table>
    </form>
</body>
</html>
