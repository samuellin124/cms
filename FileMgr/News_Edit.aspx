<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News_Edit.aspx.cs" ValidateRequest="false"
    Inherits="FileMgr_News_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>最新消息管理 【修改資料】</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        // menu控制
        $(document).ready(function () {
            InitMenu();
        });
    </script>
    <script type="text/javascript">
        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "txtNewsRegDate",   // id of the input field
                button: "imgNewsRegDate"     // 與觸發動作的物件ID相同
            });

            Calendar.setup({
                inputField: "txtNewsBeginDate",   // id of the input field
                button: "imgNewsBeginDate"     // 與觸發動作的物件ID相同
            });

            Calendar.setup({
                inputField: "txtNewsEndDate",   // id of the input field
                button: "imgNewsEndDate"     // 與觸發動作的物件ID相同
            });
        }
    </script>
    <script type="text/javascript">
        function CheckAll(checked) {
            $('input:checkbox[id*=CB_Dept]').prop('checked', checked);
        }
    </script>
    <script type="text/javascript" language="javascript">
        function openContentUpload() {
            var ser_no;
            var item;
            item = "news";
            ser_no = document.getElementById("HFD_NewsID").value;
            window.open("Content_Upload.aspx?item=" + item + "&ser_no=" + ser_no, "newsupload", "scrollbars=no,status=no,toolbar=no,top=100,left=120,width=580,height=140");
        }
    </script>
</head>
<body class="body">
    <form id="Form1" name="form" runat="server">
        <asp:HiddenField ID="HFD_Mode" runat="server" />
        <asp:HiddenField ID="HFD_NewsUID" runat="server" />
        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        </div>
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            訊息修改
        </h1>
        <div id="container">
            <table width="100%" class="table_v">
                <tr>
                    <th align="right" width="14%">發佈單位：
                    </th>
                    <td width="85%" colspan="4" align="left">
                        <input type="checkbox" value="all" name="all" id="all" onclick="CheckAll(this.checked);" />全部機構
                    <br />
                        <asp:Label ID="lblCheckBoxList" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="right" width="14%">訊息類別：
                    </th>
                    <td width="85%" colspan="4" align="left">
                        <asp:DropDownList ID="ddlNewsType" runat="server" CssClass="font9" Width="20mm">
                            <asp:ListItem>行政</asp:ListItem>
                            <asp:ListItem>活動</asp:ListItem>
                            <asp:ListItem>研習</asp:ListItem>
                            <asp:ListItem>會議</asp:ListItem>
                            <asp:ListItem>其他</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th align="right" width="14%">訊息標題：
                    </th>
                    <td width="85%" colspan="4" align="left">
                        <asp:TextBox ID="txtNewsSubject" runat="server" Width="240" MaxLength="80"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th align="right" width="14%" height="22">訊息摘要：
                    </th>
                    <td width="85%" height="22" colspan="4" align="left">
                        <asp:TextBox ID="txtNewsBrief" TextMode="MultiLine" Rows="3" Columns="120" runat="server"></asp:TextBox>
                    </td>
                    <script type="text/javascript">
                        CKEDITOR.replace('txtNewsBrief',
                        {
                            skin: 'v2'
                        });
                    </script>
                </tr>
                <tr>
                    <th align="right" width="14%">發佈者：
                    </th>
                    <td width="85%" colspan="4" align="left">
                        <asp:TextBox ID="txtNewsAuthor" Width="120" MaxLength="40" runat="server"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th align="right" width="14%">發佈日期
                    </th>
                    <td width="35%" colspan="2" align="left">
                        <asp:TextBox ID="txtNewsRegDate" Width="80" Text="" runat="server"></asp:TextBox>
                        <img id="imgNewsRegDate" alt="" src="../images/date.gif" />
                    </td>
                    <th width="13%" align="right">發佈期間：
                    </th>
                    <td width="35%" align="left">
                        <asp:TextBox ID="txtNewsBeginDate" Width="80" Text="" runat="server"></asp:TextBox>
                        <img id="imgNewsBeginDate" alt="" src="../images/date.gif" />
                        ~
                    <asp:TextBox ID="txtNewsEndDate" Width="80" runat="server"></asp:TextBox>
                        <img id="imgNewsEndDate" alt="" src="../images/date.gif" />
                    </td>
                </tr>
                <tr>
                    <th align="right" width="14%">
                        <font color="#000080">訊息內容：</font>
                    </th>
                    <td width="85%" colspan="4" align="left">
                        <asp:TextBox ID="txtNewsContent" Rows="8" Columns="100" TextMode="MultiLine"
                            runat="server"></asp:TextBox>
                        <script type="text/javascript">
                            CKEDITOR.replace('txtNewsContent',
                            {
                                skin: 'v2'
                            });
                        </script>
                    </td>
                </tr>
                <!--tr>
                <th align="right" width="14%" height="22">
                    附加圖檔：
                </th>
                <td width="14%" align="left" colspan ="4">
                    <input type="button" value=" 新 增 " name="add3" class="cbutton" onclick ="openContentUpload();" id="Button1"/>
                </td>
            </tr>
            <tr>
                <td align="right" width="14%">
                </td>
                <td colspan ="4">
                    <asp:Label ID ="Data_List"  runat ="server" ></asp:Label>
                </td>
            </tr-->
                <!--tr>
                <th width="13%" align="left">
                圖檔位置：<br />
                <span lang="zh-tw">
                    <asp:RadioButtonList  ID ="FD_ImgPosition" runat ="server" RepeatDirection="Horizontal" >
                         <asp:ListItem Text ="靠左" Value ="left" ></asp:ListItem>
                         <asp:ListItem Text ="靠右" Value ="right" ></asp:ListItem>
                         <asp:ListItem Text ="上面" Value ="top" ></asp:ListItem>
                         <asp:ListItem Text ="下面" Value ="bottom" ></asp:ListItem>
                    </asp:RadioButtonList>
                 </td>
            </tr-->
            </table>
            <div class="function">
                <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增"
                    OnClick="btnAdd_Click" />
                <asp:Button ID="btnUpdate" class="npoButton npoButton_Modify" runat="server" Text="修改"
                    OnClick="btnUpdate_Click" />
                <asp:Button ID="btnDelete" class="npoButton npoButton_Del" runat="server" Text="刪除"
                    OnClick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');" />
                <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" Text="離開"
                    OnClick="btnExit_Click" />
            </div>
        </div>
        <script type="text/javascript" language="javascript1.2">
        </script>
    </form>
</body>
</html>
