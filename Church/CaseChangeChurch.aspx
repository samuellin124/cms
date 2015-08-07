<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="CaseChangeChurch.aspx.cs" Inherits="Church_CaseChangeChurch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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


    <script type="text/javascript">
        $(document).ready(function () {
            InitAjaxLoading();
            //LoadChurchMember();
            //-----------------------------------------------------------------------
            // menu控制
            InitMenu();
            //-----------------------------------------------------------------------
            //初始化日曆
            //initCalendar(['StartDate', 'EndDate']);
            //-----------------------------------------------------------------------
            BindNumeric();
            //-----------------------------------------------------------------------
            $("#ddlCity").bind("change", function (e) {
                ChangeArea(e, 'ddlArea');
            });


            $("#ddlArea").bind("change", function (e) {
                var ZipCode = $(this).val();
                if (ZipCode != '0') {
                    $('#txtZipCode').val(ZipCode);
                    $('#HFD_Area').val(ZipCode);
                }
            });

            loadHfdData();
        });  //end of ready()

        function loadHfdData() {

            if ($('#HFD_Area').val() != '') {
                var CityID = $('#ddlCity').val();
                $.ajax({
                    type: 'post',
                    url: "../common/ajax.aspx",
                    data: 'Type=GetAreaList' + '&CityID=' + CityID,
                    success: function (result) {
                        $('select[id$=ddlArea]').html(result);
                        $("#ddlArea").val($('#HFD_Area').val());
                    },
                    error: function () { alert('ajax failed'); }
                })
            }
        }
    </script>

    <script type="text/javascript">
        function LoadChurchMember(tType) {
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=LoadChurchMember' + '&FileName=' + $('#txtFileName').val()
                        + '&Name=' + $('#txtName').val()
                        + '&ChangeChurchSta=' + $('#ddlChangeChurchSta').val()
                        + '&Event=' + $('#txtEvent').val()
                        + '&ServiceUser=' + $('#txtServiceUser').val()
                        + '&Phone=' + $('#txtPhone').val()
                        + '&BegCreateDate=' + $('#txtBegCreateDate').val()
                        + '&EndCreateDate=' + $('#txtEndCreateDate').val()
                        + '&City=' + $('#ddlCity').val()
                        + '&Address=' + $('#txtAddress').val()
                        + '&ZipCode=' + $('#txtZipCode').val(),
                async: false,
                success: function (result) {

                    $('#lblGridList').html(result);
                    //alert("完成!");
                },
                error: function () { alert('ajax failed'); }
            })
        }


        function ChangeArea(e, NoticeArea) {
            var CityID = $(e.target).val();
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=GetAreaList' + '&CityID=' + CityID,
                success: function (result) {
                    $('select[id$=' + NoticeArea + ']').html(result);
                    $('#HFD_ZipCode').val('');
                    var a = $('#txtZipCode').val();
                    $('#txtZipCode').val('');
                },
                error: function () { alert('ajax failed'); }
            })
        }

        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "txtBegCreateDate",   // id of the input field
                button: "imgBegCreateDate"     // 與觸發動作的物件ID相同

            });
            Calendar.setup({

                inputField: "txtEndCreateDate",   // id of the input field
                button: "imgEndCreateDate"     // 與觸發動作的物件ID相同
            });
            Calendar.showYearsCombo(true);
        }
    </script>

</head>
<body class="body">
    <form id="Form1" runat="server">
        <asp:HiddenField ID="HFD_LoadFormData" runat="server" />
        <asp:HiddenField ID="HFD_Area" runat="server" />

        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" onclick="SwitchMenu();return false;" /></a>
        </div>
        <asp:Label runat="server" ID="Toolbar"></asp:Label>
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            個案轉介管理</h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <td width="100%" colspan="10">
                    <asp:Literal runat="server" ID="MainMenu"></asp:Literal>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th align="right">姓名：
                </th>
                <td>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
                <th align="right">教會：
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtChurch"></asp:TextBox>
                </td>
                <td align="right">
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                    <%--<asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增" Visible="False" />--%>
                    <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印"
                        OnClick="btnPrint_Click" />
                </td>
            </tr>
            <tr>
                <th align="right">時間區間：
                </th>
                <td colspan="2">
                    <asp:TextBox ID="txtBegCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgBegCreateDate" alt="" src="../images/date.gif" />
                    <asp:TextBox ID="txtEndCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgEndCreateDate" alt="" src="../images/date.gif" />
                </td>
            </tr>
            <tr>
                <td width="100%" colspan="6">
                    <asp:Label ID="lblGridList" runat="server"></asp:Label>
                    <asp:Label ID="lblReport" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
