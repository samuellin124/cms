<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainDefault.aspx.cs" Inherits="MainDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title></title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.field.js"></script>
    <script type="text/javascript">
        // menu控制
        $(document).ready(function () {
            InitMenu();
        });
    </script>

    <script type="text/javascript">
        //-------------
        function LoadConsultQry(tType) {
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=LoadMainDefault' + '&ServiceUser=' + $('#HFD_UserID').val()
                      + '&Phone='
                      + '&CName='
                      + '&BegCreateDate='
                      + '&EndCreateDate='
                      + '&GroupID=' + $('#HFD_GroupID').val()
                      + '&Event=&Marry=&City=&ZipCode=&Address=&ServiceUser=&ConsultantItem=',

                async: false,
                success: function (result) {

                    $('#lblConsult').html(result);
                    // alert("完成!");
                    //                  
                },
                error: function () { alert('ajax failed'); }
            })
        }
        //***************

    </script>
</head>
<body class="body">
    <form id="Form1" name="form" runat="server">
        <asp:HiddenField ID="HFD_GroupID" runat="server" />
        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        </div>
        <h1 style="padding-bottom: 0px;">
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            公告 
        </h1>
        <table width="100%" class="table_v">
            <tr>
                <td align="left">&nbsp;</td>
            </tr>
        </table>

        <table width="100%" class="table_v">
            <tr>
                <td align="left"></td>
            </tr>
            <tr>
                <td width="100%" valign="top" align="center">

                    <asp:Label ID="lblNews" runat="server"></asp:Label>


                    <a href="../filemgr/News_Show_List.aspx">
                        <img border="0" src="../images/more.gif" alt="" width="63" height="21" align="right" />


                        &nbsp;&nbsp;</a></td>
            </tr>
        </table>
        <h1 style="padding-bottom: 0px;">
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            生日壽星
        </h1>
        <table width="100%" class="table_v">
            <tr>
                <td align="left">&nbsp;</td>
            </tr>
        </table>

        <table width="100%" class="table_v">
            <tr>
                <th align="center" width="50%">本月壽星
                </th>
                <th align="center" width="50%">下月壽星
                </th>
            </tr>
            <tr>
                <td width="50%" valign="top" align="center">

                    <asp:Label ID="lblTBirth" runat="server"></asp:Label></td>
                <td width="50%" valign="top" align="center">
                    <asp:Label ID="lblNBirth" runat="server"></asp:Label></td>
            </tr>
        </table>

        <table width="100%" class="table_v">
            <h1 style="padding-bottom: 0px;">
                <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
                未完成清單
            </h1>
            <tr>
                <td align="left"></td>
            </tr>
            <tr>
                <td width="100%" valign="top" align="center">



                    <asp:Label ID="lblConsult" runat="server"></asp:Label>
                    <a href="../filemgr/News_Show_List.aspx">
                        <%--<img border="0" src="../images/more.gif" alt="" width="0" height="0" align="right" />--%>

                  
                &nbsp;&nbsp;</a></td>
            </tr>
        </table>
        <asp:HiddenField ID="HFD_UserID" runat="server" />
    </form>
</body>
</html>
