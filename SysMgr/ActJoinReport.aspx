<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActJoinReport.aspx.cs" Inherits="SysMgr_ActJoinReport" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>參與活動列表</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript">
        // menu控制
        $(document).ready(function () {

            InitMenu();

        });

        //新增時不要出現的物件在此控制
        var Mode = $('#HFD_Mode').val();
        if (Mode == 'ADD') {
            $('.HideWhenAdd').hide()
        }
    </script>
    <style type="text/css">
        .style1 {
            height: 28px;
        }
    </style>
</head>
<body class="body">
    <form id="Form1" runat="server" submitdisabledcontrols="False">
        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        </div>
        <input type="hidden" id="RefValue" name="RefValue" runat="server" />
        <asp:Label runat="server" ID="Toolbar"></asp:Label>

        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />參與活動列表</h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">

                <tr>
                    <th width="12%" align="right">服務類別：
                    </th>
                    <td width="10%" align="left">
                        <asp:DropDownList ID="ddlclassname" runat="server" AutoPostBack="true" 
                            onselectedindexchanged="ddlclassname_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                       <th width="12%" align="right">項目類別：
                    </th>
                  
                    <td align="left" width="10%">
                              <asp:DropDownList ID="ddlitem" runat="server">
                        </asp:DropDownList>
                        <td align="left" width="30%">
                             <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                             <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" OnClick="btnPrint_Click" />
                            <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增" 
                                 OnClick="btnAdd_Click" Visible="False" />
                            <%--<asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" 
                onclick="btnPrint_Click" /> --%>
                        </td>

                        <tr>
                            <td colspan="7">
                                <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
        </table>
    </form>
</body>
</html>
