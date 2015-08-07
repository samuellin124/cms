<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainTop.aspx.cs" Inherits="MainTop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="refresh" content="299" />    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>加百列基金會</title>
    <link href="../include/frame.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    <!--
        function MM_preloadImages() { //v3.0
            var d = document; if (d.images) {
                if (!d.MM_p) d.MM_p = new Array();
                var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
                    if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; } 
            }
        }
        function MM_swapImgRestore() { //v3.0
            var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
        }

        function MM_findObj(n, d) { //v4.01
            var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
                d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
            }
            if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
            for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
            if (!x && d.getElementById) x = d.getElementById(n); return x;
        }
    //-->
    </script>
</head>

<body class="title_body" onload="MM_preloadImages('images/title_logout_bt_hover.gif')">
    <form id="form" runat="server">
    <div id="header" style="width:100%;">
        <div id="logo" style="width:380px;">
    <%--    <img src="../images/logo1.png" width="1" height="0" />--%>
        
            <asp:ImageButton ID="ImageButton1" runat="server" 
                ImageUrl="~/images/LOGO1.PNG"  Width="300px" 
                Height="76px" onclick="ImageButton1_Click" />       
                <asp:Label ID="lblversion" runat="server" Text=""><font color=red>Ver.2.1測試版</font></asp:Label>     
        </div>
        <div id="nav" style="width:380px;">
            <table border="0" align="center" cellpadding="8" cellspacing="0" >
                <tr style="line-height:1px">
                      <td width="85" align="left" valign="top">
                        單位：</td>
                    <td>
                        <asp:Label runat="server" ID="txtDeptName"></asp:Label>
                    </td>
                    <td width="42" rowspan="4" align="center" valign="bottom">
                        <asp:ImageButton runat="server" ID="btnLogout" ImageUrl="~/images/title_logout_bt.gif" Width="29" Height="42" onclick="btnLogout_Click" OnClientClick="if(!confirm('是否確定要登出 ?')){return false;}" />
                    </td>
                </tr>
                <tr style="line-height:4px">
                    <td width="45" align="left" valign="top">
                        帳號：
                    </td>
                    <td width="300" align="left" valign="top">
                        <asp:Label runat="server" ID="txtUserGroup"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <table>
        <tr  valign="bottom">
        <td >
                  
        </td>
        </tr>
        </table>
 
    </div>
    
    </form>
    <script type="text/javascript" >
        function EvImageOverChange(name, action) {
            switch (action) {
                case 'in':
                    name.src = "../images/title_logout_bt_hover.gif";
                    break;
                case 'out':
                    name.src = "../images/title_logout_bt.gif";
                    break;
            }
        }
    </script>
</body>
</html>
