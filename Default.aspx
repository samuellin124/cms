<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<%--<link href="FAVICON.ICO" rel="FAVICON.ICO" />--%>
    <link rel="shortcut icon" href="FAVICON.ico">    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>諮詢服務系統</title>
    <link rel="stylesheet" type="text/css" href="include/index.css" />
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

        function MM_swapImage() { //v3.0
            var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
                if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
        }
        function MM_showHideLayers() { //v9.0
            var i, p, v, obj, args = MM_showHideLayers.arguments;
            for (i = 0; i < (args.length - 2); i += 3)
                with (document) if (getElementById && ((obj = getElementById(args[i])) != null)) {
                    v = args[i + 2];
                    if (obj.style) { obj = obj.style; v = (v == 'show') ? 'visible' : (v == 'hide') ? 'hidden' : v; }
                    obj.visibility = v;
                }
        }
    //-->
    </script>
</head>
<body class="body" onload="MM_preloadImages('images/index_login_bt_hover.gif');MM_showHideLayers('contact','','hide','copy','','hide')">
    <form id="Form1" runat="server" method="post" name="form" action="">
    <script type="text/javascript">
        function setCookie(c_name, value) {
            var exdate = new Date();
            //exdate.setDate(exdate.getDate() + 1);    //加一天的時限
            //exdate.setHours(exdate.getHours() + 1);   //加一小時的時限
            exdate.setMinutes(exdate.getMinutes() + 1); //加一分鐘的時限
            document.cookie = c_name + "=" + escape(value) + (";expires=" + exdate.toUTCString());
        }

        function getCookie(c_name) {
            if (document.cookie.length > 0) {
                c_start = document.cookie.indexOf(c_name + "=");
                if (c_start != -1) {
                    c_start = c_start + c_name.length + 1;
                    c_end = document.cookie.indexOf(";", c_start);
                    if (c_end == -1) c_end = document.cookie.length;
                    return unescape(document.cookie.substring(c_start, c_end));
                }
            }
            return "0";
        }
    </script>
    <div id="header">
        <!--<div id="contact">
        </div>
        <div id="copy">
        </div>
        <div id="nav">
            <ul>
                <li></li>
                <li><a href="#" onmouseover="MM_showHideLayers('contact','','show')" onmouseout="MM_showHideLayers('contact','','hide')">
                    聯絡我們</a></li>
                <li><a href="#" onmouseover="MM_showHideLayers('copy','','show')" onmouseout="MM_showHideLayers('copy','','hide')">
                    版權宣告</a></li>
            </ul>
        </div>-->
    </div>
    <div id="container">
        <div id="logo">
            <img src="images/logo1.png" width="249" height="60" align="absmiddle" style="padding-top: 39px;" /></div>
        <div id="login">
            <table border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <th width="50" height="30" align="center">
                        機 構
                    </th>
                    <td width="200" valign="middle">
                        <label>
                            <asp:DropDownList ID="ddlOrg" runat="server" Width="150">
                            </asp:DropDownList>
                        </label>
                    </td>
                </tr>
                <tr>
                    <th width="50" height="30" align="center">
                        帳 號
                    </th>
                    <td width="200" valign="middle">
                        <label>
                            <asp:TextBox ID="txtUserID" runat="server" Width="100"></asp:TextBox>
                        </label>
                    </td>
                </tr>
                <tr>
                    <th height="30" align="center">
                        密 碼
                    </th>
                    <td valign="middle">
                        <label>
                            <asp:TextBox ID="txtPassword" runat="server" Width="100" TextMode="Password"></asp:TextBox>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td height="90" colspan="2" align="center">
                        <a href="Javascript:document.forms[0].submit();">
                            <img src="images/index_login_bt_a.gif" name="LoginBotton" width="44" height="61"
                                id="LoginBotton" onmouseover="MM_swapImage('LoginBotton','','images/index_login_bt_hover.gif',1)"
                                onmouseout="MM_swapImgRestore()" /></a>
                    </td>
                </tr>
            </table>
        </div>
        <div id="sysname">
            <!--<div id="npois">
                <a href="http://www.npois.com.tw" target="_blank">
                    <img src="images/index_npois.gif" width="150" height="36" /></a></div>
            <div id="footer">
                Copyright &copy; 2010.<br />
                NPO Information Service Corporation.
                <br />
                All Rights Reserved.<br />
            </div>-->
        </div>
    </div>
    </form>
</body>
</html>
