<%@ Page Language="C#" AutoEventWireup="true"  EnableEventValidation="false" CodeFile="Church.aspx.cs" Inherits="SysMgr_Church" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>教會資料</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript">

        // menu控制
        $(document).ready(function () {
            InitAjaxLoading();
            //-----------------------------------------------------------------------
            // menu控制
            InitMenu();
            //-----------------------------------------------------------------------
            //初始化日曆
            //initCalendar(['StartDate', 'EndDate']);
            //-----------------------------------------------------------------------
            BindNumeric();

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

        }); //end of ready()




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
    <style type="text/css">
        .auto-style1 {
            width: 20%;
        }

        .auto-style2 {
            width: 13%;
        }

        .auto-style3 {
            width: 26%;
        }
    </style>
</head>
<body class="body">
    <form id="Form1" name="form" runat="server">
        <asp:HiddenField ID="HFD_Area" runat="server" />
        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        </div>
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            教會管理
        </h1>
        <table width="100%" class="table_v">
            <tr>
                <th align="right" width="10%">教會名稱：</th>
                <td align="left" class="auto-style3">
                    <asp:TextBox runat="server" ID="txtName" Width="40mm" CssClass="font9"></asp:TextBox>
                </td>
                <th width="10%" align="right">聯絡窗口：</th>
                <td align="left" class="auto-style1" width="10%">
                    <asp:TextBox runat="server" ID="txtContact" Width="30mm" CssClass="font9"></asp:TextBox>
                </td>

                <td align="left" width="30%" colspan="3">
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                    <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印"
                        OnClick="btnPrint_Click" />
                </td>
            </tr>
            <tr>
                <th width="10%" align="right">主任牧師：</th>
                <td align="left" class="auto-style1">
                    <asp:TextBox runat="server" ID="txtPriest" Width="30mm" CssClass="font9"></asp:TextBox>
                </td>
                <th align="right">查詢地址：
                </th>
                <td colspan="2" align="left">縣市：<asp:DropDownList ID="ddlCity" runat="server" Height="25px"></asp:DropDownList>
                    區鄉鎮：<asp:DropDownList ID="ddlArea" runat="server"></asp:DropDownList>
                </td>
                <th width="7%" align="right">日期：</th>
                <td class="auto-style3">
                    <asp:TextBox ID="txtBegCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgBegCreateDate" alt="" src="../images/date.gif" />
                    <asp:TextBox ID="txtEndCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgEndCreateDate" alt="" src="../images/date.gif" />
                </td>

            </tr>
            <tr>
                <td colspan="10">
                    <asp:Label ID="lblGridList" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </form>
    <td>
        <asp:Label ID="lblRpt" runat="server"></asp:Label>
    </td>
</body>
</html>
