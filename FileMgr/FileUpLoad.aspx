<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileUpLoad.aspx.cs" Inherits="FileMgr_FileUpLoad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>�ɮפW�� </title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
</head>
<body class="body">
    <form id="form2" method="post" runat="server">
        <asp:HiddenField ID="HFD_Ap_Name" runat="server" />
        <asp:HiddenField ID="HFD_dept_id" runat="server" Value="KTA" />
        <asp:HiddenField ID="HFD_filecat_id" runat="server" Value="0" />
        <div align="center">
            <center>
                <table border="0" width="100%" class="table_v">
                    <tr>
                        <td width="100%"  style="background-color: rgb(55,111,135); color: rgb(255,255,255);
                            font-size: 16pt;
                            filter: Alpha(Opacity=100, style=1)" height="25">
                            <font color="#ffffff">�ɮפW�� (�ɮפj�p���� 10MB)</font></td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <div align="center">
                                <center>
                                    <table width="100%" border="1" class="table_v">
                                        <tr>
                                            <th align="right" width="15%" height="22">
                                                �ɮ׻���:
                                            </th>
                                            <td align="left" width="85%" height="18">
                                                &nbsp;<asp:TextBox ID="FD_Upload_Desc" runat="server" Width="385px"></asp:TextBox>
                                                (�W���ɮױN�|�H����r���)
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="right" width="15%" height="22">
                                                �W���ɮ�:
                                            </th>
                                            <td align="left" width="85%" height="18">
                                                &nbsp;<asp:FileUpload ID="FileUpload1" runat="server" Width="462px" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="function">
                    <asp:Button ID="btnSave" runat="server" Text="�s��" OnClick="btnSave_Click" 
                        CssClass="npoButton npoButton_New" />
                    <asp:Button ID="btnCancle" runat="server" Text="����" OnClientClick="window.close();"
                        CssClass="npoButton npoButton_Exit" />
                </div>
            </center>
        </div>
    </form>
</body>
</html>
