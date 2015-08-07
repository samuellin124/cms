<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QryReport.aspx.cs" Inherits="CaseMgr_QryReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <%--<script type="text/javascript" src="../include/HFPChurch.js"></script>--%>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.field.js"></script>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table>
    <tr>
        <td align="center">  <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" 
            Text="離開" onclick="btnExit_Click"/>
        </td>
    </tr>
        <tr>
            <td>
              <asp:Label ID="lblReport" runat="server" ></asp:Label>
            </td>
        </tr>
     <tr>
        <td align="center">  <asp:Button ID="btnExit2" class="npoButton npoButton_Exit" runat="server" 
            Text="離開" onclick="btnExit_Click"/>
        </td>
    </tr>
    </table>
    <asp:HiddenField ID="HFD_MemberID" runat="server" />
    </form>
</body>
</html>
