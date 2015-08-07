<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileCats.aspx.cs" Inherits="FileMgr_FileCats" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>表單下載</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
         function openUploadWindow(Ap_Name)
         {
            var dept_id = document.getElementById("HFD_dept_id").value;
            var filecat_id = document.getElementById("HFD_filecat_id").value;
            
            window.open("FileUpLoad.aspx?dept_id="+dept_id+"&filecat_id="+filecat_id+"&Ap_Name="+Ap_Name,"upload","scrollbars=no,status=no,toolbar=no,top=100,left=120,width=720,height=180");
         }
         function openFileCatWindow(Ap_Name)
         { 
            var dept_id = document.getElementById("HFD_dept_id").value;
            var filecat_id = document.getElementById("HFD_filecat_id").value;      
            window.open("FileCats_Add.aspx?dept_id="+dept_id+"&filecat_id="+filecat_id,"filecat","scrollbars=no,status=no,toolbar=no,top=100,left=120,width=580,height=200");
         }
    </script>

</head>
<body class="body">
    <form id="Form1" name="form" runat="server">
        <asp:HiddenField ID="HFD_dept_id" runat="server" />
        <asp:HiddenField ID="HFD_filecat_id" runat="server" />
        <div style="vertical-align: middle; text-align: left; width: 100%; background-color: rgb(55,111,135);
            color: rgb(255,255,255); filter: Alpha(Opacity=100, style=1);">
            <font size="large" style="font-weight: bold">表單下載</font>
        </div>
        <!--內容頁面_START-->
        <table width="100%">
            <tr>
                <td width="100%"> 
                    <!--Query Table_Start-->                   
                    <table  width="100%" class="table_v">
                        <tr >
                            <td width="302">
                                <img alt="" border='0' src='../images/Icon_Upfolder.gif' width='16' height='16' align='absmiddle' />
                                <asp:HyperLink ID="LINK_FileCats" runat="server" style="font-size: 16pt; font-family: 新細明體">文件庫</asp:HyperLink>
                            </td>
                            <td width="144">
                                <asp:Label ID="lbl_FileCat_Parent" runat="server" Text=""></asp:Label>
                            </td>
                            <td align="right">
                                <asp:TextBox ID="FD_Keyword" Width="100" CssClass="font9" runat="server"></asp:TextBox>
                                <asp:Button ID="btnQuery" runat="server" CssClass="npoButton npoButton_Search" Text="查詢" OnClick="btnQuery_Click">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                    <!--Query Table_End-->
                </td>
            </tr>
            <tr>
                <td width="100%" colspan="3">
                    <!--List頁面_START-->
                    <div style="height: 440px; width: 100%; border-width: 0px; overflow: auto">
                        <table border="0" width="100%" id="table1" class="table_v">
                            <tr>
                                <td width="50%">
                                    <!--文件上下一層區塊-->
                                </td>
                                <td width="50%" align="right">
                                    <asp:Button runat="server" Text=" 上傳新檔案 " ID="btnNewFile" style="width:130px" CssClass="npoButton npoButton_Upload" OnClientClick="openUploadWindow('FileSys');return false;" />
                                    <asp:Button runat="server" Text=" 新增資料夾 " ID="btnNewFolder" style="width:130px" CssClass="npoButton npoButton_NewFolder" OnClientClick="openFileCatWindow('FileSys');return false;" />
                                </td>
                            </tr>
                        </table>
                        <!--資料夾列表-->
                        <div>
                            <asp:Label ID="Grid_List1" runat="server" Text=""></asp:Label>
                        </div>
                        <!--檔案列表-->
                        <div>
                            <asp:Label ID="Grid_List2" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <!--List頁面_END-->
                </td>
            </tr>
        </table>
        <!--內容頁面_END-->
    </form>
</body>
</html>
