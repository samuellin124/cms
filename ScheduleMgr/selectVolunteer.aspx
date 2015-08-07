<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectVolunteer.aspx.cs" Inherits="ScheduleMgr_selectVolunteer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>志工選擇</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <%--    <script type="text/javascript">
        // menu控制
        $(document).ready(function () {
            //InitMenu();
        });
    </script>--%>
</head>
<body class="body">
    <form id="Form1" runat="server">
    <asp:HiddenField runat="server" ID="HFD_Uid" />
    <asp:HiddenField runat="server" ID="HFD_Mode" />
    <asp:HiddenField runat="server" ID="HFD_lblName" />
    <asp:HiddenField runat="server" ID="HFD_ScheduleID" />
    <asp:HiddenField runat="server" ID="HFD_UserID" />
    <asp:HiddenField runat="server" ID="HFD_Year" />
    <asp:HiddenField runat="server" ID="HFD_Querter" />
    <asp:HiddenField runat="server" ID="HFD_DayOfWeek" />
    <asp:HiddenField runat="server" ID="HFD_periodID" />
    <div id="menucontrol">
        <a href="#">
            <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}"
                onclick="SwitchMenu();return false;" /></a> <a href="#">
                    <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}"
                        onclick="SwitchMenu();return false;" /></a>
    </div>
    <%--    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <h1>
                <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
                選擇志工
            </h1>
            <table width="100%" class="table_v">
                <tr>
                    <th align="right" rowspan="2">
                        已排班志工
                    </th>
                    <td width="25%" rowspan="2">
                        <asp:TextBox runat="server" ID="txtUserName" Visible="False"></asp:TextBox>
                        <asp:Button ID="btnSearch" Text="搜尋" runat="server" OnClick="btnSearch_Click" Visible="False" />
                        <br />
                        <asp:ListBox ID="lstScheduleUser" runat="server" Height="300px" Width="180px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                        <td align="left" rowspan="2">
                            <asp:Button ID="btnSelect" Text="加入" runat="server" OnClick="btnSelect_Click" />
                        </td>
                   <%-- <th width="20%" align="right">
                        群組名稱：
                    </th>--%>
                    <td width="25%">
                       <%-- <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" Visible="False">
                        </asp:DropDownList>--%>
                    </td>
                </tr>
                <tr>
                    <th align="right">
                        選擇志工
                    </th>
                    <td align="left">
                        <asp:ListBox ID="lstUser" runat="server" Height="300px" Width="180px" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                    <td align="left" rowspan="1">
                        <asp:Button ID="btnRemove" Text="移出" runat="server" OnClick="btnRemove_Click" />
                      
                    </td>
                </tr>
                 <tr>
    <td colspan ="6" align="right">
      <asp:Button ID="Button1" runat="server" onclick="btnSave_Click" Text="確認" CssClass="npoButton npoButton_Modify" />
    </td>
    </tr>
    </table>
  
    
    </form>
</body>
</html>
