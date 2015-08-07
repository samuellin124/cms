<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News_Add.aspx.cs" Inherits="FileMgr_News_Add" validateRequest="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>訊息管理 【新增資料】</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <link rel="stylesheet" type="text/css" href="../include/dms.css" />
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script  type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" language="Javascript1.2">
<!-- // load htmlarea
_editor_url = "../htmlarea/";           // URL to htmlarea files
var win_ie_ver = parseFloat(navigator.appVersion.split("MSIE")[1]);
if (navigator.userAgent.indexOf('Mac')        >= 0) { win_ie_ver = 0; }
if (navigator.userAgent.indexOf('Windows CE') >= 0) { win_ie_ver = 0; }
if (navigator.userAgent.indexOf('Opera')      >= 0) { win_ie_ver = 0; }
if (win_ie_ver >= 5.5) {
  document.write('<scr' + 'ipt src="' +_editor_url+ 'editor.js"');
  document.write(' language="Javascript1.2"></scr' + 'ipt>');  
} else { document.write('<scr'+'ipt>function editor_generate() { return false; }</scr'+'ipt>'); }
// -->
    </script>
<script type="text/javascript">
     window.onload =initCalendar;
     function initCalendar() {
         Calendar.setup({
            inputField     :    "FD_news_RegDate",   // id of the input field
            ifFormat       :    "%Y/%m/%d",       // format of the input field
            showsTime      :    false,
            timeFormat     :    "24"
        }); 
        
         Calendar.setup({
            inputField     :    "FD_news_BeginDate",   // id of the input field
            ifFormat       :    "%Y/%m/%d",       // format of the input field
            showsTime      :    false,
            timeFormat     :    "24"
        }); 
        
         Calendar.setup({
            inputField     :    "FD_news_EndDate",   // id of the input field
            ifFormat       :    "%Y/%m/%d",       // format of the input field
            showsTime      :    false,
            timeFormat     :    "24"
        }); 
     }     
</script>
<script type="text/javascript">
    function check_all(checked)
    {
        var InputTagItems = document.getElementsByTagName("INPUT");
        var i;
        for(i = 0;i< InputTagItems.length ; i ++)
        {
            InputItem = InputTagItems[i];
            if(InputItem.type == 'checkbox')
            {
                 InputItem.checked = checked;
            }
        }
    }
</script>
</head>
<body class="gray">

    <form name="form" action="" method="post" runat="server">
        <p />
        <div align="center">
            <center>
                <table border="0" width="98%" style="border-collapse: collapse">
                    <tr>
                        <td width="100%" class="font11" height="25" style="background-color: rgb(55,111,135);
                            color: rgb(255,255,255); filter: Alpha(Opacity=100, style=1)" align ="left" >
                            <b>
                                <img alt="" border="0" src="../images/profile_li.gif" width="12" height="12" align="middle" />
                                公佈欄管理</b><span style="font-family: 新細明體"> <font size="2">【</font></span><span lang="zh-tw"
                                    style="font-family: 新細明體"><font size="2">新增資料</font></span><span style="font-family: 新細明體">
                                        <font size="2">】</font></span></td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <div align="center">
                                <center>
                                    <table border="1" width="100%" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table width="100%" border="1" cellspacing="0" style="border-collapse: collapse;
                                                    border-color: #E1DFCE">
                                                    <tr>
                                                        <td align="right" width="14%" height="22">
                                                            <font color="#000080">發佈單位：</font></td>
                                                        <td width="85%" height="22" colspan="3" align="left">
                                                            <input type="checkbox" value="all" name="all" id="all"  onclick ="check_all(this.checked);"/>全部機構 <br />                                                       
                                                            &nbsp;<asp:Label ID="CheckBox_List" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="14%" height="22">
                                                            <font color="#000080">訊息標題：</font>
                                                        </td>
                                                        <td width="85%" height="22" colspan="3" align="left">
                                                            <asp:TextBox ID="FD_new_subject" CssClass="font9" runat="server" Width="240" MaxLength="80"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="14%" height="22">
                                                            <font color="#000080"><span lang="zh-tw">訊息摘要</span>：</font></td>
                                                        <td width="85%" height="22" colspan="3" align="left">
                                                            <asp:TextBox ID="FD_news_brief" TextMode="MultiLine" Rows="3" Columns="60" CssClass="font9"
                                                                runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="14%" height="22">
                                                            <font color="#000080">發佈者：</font></td>
                                                        <td width="85%" height="22" colspan="2" align="left">
                                                            <asp:TextBox ID="FD_news_author" Width="120" CssClass="font9" MaxLength="40" runat="server"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="14%" height="22">
                                                            <font color="#000080">發佈日期：</font></td>
                                                        <td width="20%" height="22" align="left">
                                                            <font color="#000080">
                                                                <asp:TextBox ID="FD_news_RegDate" Width="80" CssClass="font9" Text="" runat="server"></asp:TextBox>
                                                            </font></td>
                                                        <td width="13%" height="22" align="right">
                                                            <font color="#000080">發佈期間：</font></td>
                                                        <td width="51%" height="22" align="left">
                                                            <asp:TextBox ID="FD_news_BeginDate" Width="80" CssClass="font9" Text="" runat="server"></asp:TextBox>                                               
                                                            ~
                                                            <asp:TextBox ID="FD_news_EndDate" Width="80" CssClass="font9" Text="" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="14%" class="td2" height="2">
                                                        </td>
                                                        <td width="85%" colspan="3" class="td2" height="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="14%" height="22">
                                                            <font color="#000080">訊息內容：</font></td>
                                                        <td width="85%" height="22" colspan="3" align="left">
                                                            <asp:TextBox ID="FD_news_content" Rows="8" Columns="80" CssClass="font9" TextMode="MultiLine"
                                                                runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" bgcolor="#FFFCE1" height="22" class="font9" colspan="4" align="left">
                                                            <asp:ImageButton ID="btnSave" Style='position: relative; left: 0; font-size: 9pt'
                                                                CssClass="button3-bg" ImageUrl="../images/add.jpg" runat="server" OnClick="btnSave_Click">
                                                            </asp:ImageButton>
                                                            <asp:ImageButton ID="btnCancel" Style='position: relative; left: 0; font-size: 9pt'
                                                                CssClass="button3-bg" ImageUrl="../images/back.jpg" runat="server" OnClientClick="history.go(-1);return false"
                                                                OnClick="btnCancel_Click"></asp:ImageButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </td>
                    </tr>
                </table>
            </center>
        </div>

        <script type="text/javascript" language="javascript1.2">
var config = new Object();    // create new config object

config.width = "100%";
config.height = "200px";
config.bodyStyle = 'background-color: white; font-family: "新細明體"; font-size: x-small;';
config.debug = 0;

// NOTE:  You can remove any of these blocks and use the default config!

config.fontnames = {
    "新細明體":        "新細明體",
    "Arial":           "arial, helvetica, sans-serif",
    "Courier New":     "courier new, courier, mono",
    "Georgia":         "Georgia, Times New Roman, Times, Serif",
    "Tahoma":          "Tahoma, Arial, Helvetica, sans-serif",
    "Times New Roman": "times new roman, times, serif",
    "Verdana":         "Verdana, Arial, Helvetica, sans-serif",
    "impact":          "impact",
    "WingDings":       "WingDings"
};
config.fontsizes = {
    "1 (8 pt)":  "1",
    "2 (10 pt)": "2",
    "3 (12 pt)": "3",
    "4 (14 pt)": "4",
    "5 (18 pt)": "5",
    "6 (24 pt)": "6",
    "7 (36 pt)": "7"
  };

//config.stylesheet = "http://www.domain.com/sample.css";
  
config.fontstyles = [   // make sure classNames are defined in the page the content is being display as well in or they won't work!
  { name: "headline",     className: "headline",  classStyle: "font-family: arial black, arial; font-size: 28px; letter-spacing: -2px;" },
  { name: "arial red",    className: "headline2", classStyle: "font-family: arial black, arial; font-size: 12px; letter-spacing: -2px; color:red" },
  { name: "verdana blue", className: "headline4", classStyle: "font-family: verdana; font-size: 18px; letter-spacing: -2px; color:blue" }

// leave classStyle blank if it's defined in config.stylesheet (above), like this:
//  { name: "verdana blue", className: "headline4", classStyle: "" }  
];

editor_generate('FD_news_content',config);
        </script>

    </form>
</body>
</html>
