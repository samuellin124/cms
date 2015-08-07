<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminRight.aspx.cs" Inherits="SysMgr_AdminRight" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>使用權限設定</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        $(document).ready(function (e) {
            InitMenu();

            if ($('#HFD_UpdateRight').val() == 'False') {
                $('#btnSave').hide();
            }

            $('#CB_AddColAll').bind("click", function (e) {
                SetCol('Add', $(this).prop('checked'));
            });
            $('#CB_UpdateColAll').bind("click", function (e) {
                SetCol('Update', $(this).prop('checked'));
            });
            $('#CB_DeleteColAll').bind("click", function (e) {
                SetCol('Delete', $(this).prop('checked'));
            });
            $('#CB_QueryColAll').bind("click", function (e) {
                SetCol('Query', $(this).prop('checked'));
            });
            $('#CB_PrintColAll').bind("click", function (e) {
                SetCol('Print', $(this).prop('checked'));
            });
            $('#CB_FocusColAll').bind("click", function (e) {
                SetCol('Focus', $(this).prop('checked'));
            });
            $('#CB_Examine1ColAll').bind("click", function (e) {
                SetCol('Examine1', $(this).prop('checked'));
            });
            $('#CB_Examine2ColAll').bind("click", function (e) {
                SetCol('Examine2', $(this).prop('checked'));
            });
            $('#CB_Examine3ColAll').bind("click", function (e) {
                SetCol('Examine3', $(this).prop('checked'));
            });

            $('#CB_SelectAll').bind("click", function (e) {
                $(this).prop('checked', true)
                $('#CB_UnSelectAll').prop('checked', false)
                $('#CB_AddColAll').prop('checked', true)
                $('#CB_UpdateColAll').prop('checked', true);
                $('#CB_DeleteColAll').prop('checked', true);
                $('#CB_QueryColAll').prop('checked', true);
                $('#CB_PrintColAll').prop('checked', true);
                $('#CB_FocusColAll').prop('checked', true);
                $('#CB_Examine1ColAll').prop('checked', true);
                $('#CB_Examine2ColAll').prop('checked', true);
                $('#CB_Examine3ColAll').prop('checked', true);

                SetAll(true);
            });
            $('#CB_UnSelectAll').bind("click", function (e) {
                $(this).prop('checked', true)
                $('#CB_SelectAll').prop('checked', false)
                $('#CB_AddColAll').prop('checked', false)
                $('#CB_UpdateColAll').prop('checked', false);
                $('#CB_DeleteColAll').prop('checked', false);
                $('#CB_QueryColAll').prop('checked', false);
                $('#CB_PrintColAll').prop('checked', false);
                $('#CB_FocusColAll').prop('checked', false);
                $('#CB_Examine1ColAll').prop('checked', false);
                $('#CB_Examine2ColAll').prop('checked', false);
                $('#CB_Examine3ColAll').prop('checked', false);

                SetAll(false);
            });

            $('input:checkbox[id*=CB_RowSelectAll]').bind("click", function (e) {
                var s = 'input:checkbox[value=' + $(this).val() + ']';
                $(s).prop('checked', true);
                s = '#CB_RowUnSelectAll' + $(this).val();
                $(s).prop('checked', false);
            });

            $('input:checkbox[id*=CB_RowUnSelectAll]').bind("click", function (e) {
                var s = 'input:checkbox[value=' + $(this).val() + ']';
                $(s).prop('checked', false);
                s = '#CB_RowSelectAll' + $(this).val();
                $(s).prop('checked', false);
            });

            function SetCol(name, checked) {
                var s = 'input:checkbox[id*=CB_Col' + name + ']';
                $(s).prop('checked', checked);
            }
            function SetAll(checked) {
                SetCol('Add', checked);
                SetCol('Update', checked);
                SetCol('Delete', checked);
                SetCol('Query', checked);
                SetCol('Print', checked);
                SetCol('Focus', checked);
                SetCol('Examine1', checked);
                SetCol('Examine2', checked);
                SetCol('Examine3', checked);
            }
        });
    </script>
    <script type="text/javascript">

        function SaveAllValue() {
            var SumCnt = $('#HFD_TotalMenuItem').val();
            var SetupValue = '';
            for (var i = 0; i < SumCnt; i++) {
                var MenuID = $('#MenuID' + i).prop('value');
                SetupValue += MenuID + ",";
                SetupValue += GetValue('#CB_ColAdd' + i) + ',';
                SetupValue += GetValue('#CB_ColUpdate' + i) + ',';
                SetupValue += GetValue('#CB_ColDelete' + i) + ',';
                SetupValue += GetValue('#CB_ColQuery' + i) + ',';
                SetupValue += GetValue('#CB_ColPrint' + i) + ',';
                SetupValue += GetValue('#CB_ColFocus' + i) + ',';
                SetupValue += GetValue('#CB_ColExamine1' + i) + ',';
                SetupValue += GetValue('#CB_ColExamine2' + i) + ',';
                SetupValue += GetValue('#CB_ColExamine3' + i);

                if (i < SumCnt - 1) {
                    SetupValue += '|';
                }
            }
            $('#HFD_SetupValue').val(SetupValue);
            document.getElementById("KeyFocus").click();

        }
        function GetValue(selector) {
            if ($(selector).length != 1) {
                return '';
            }
            if ($(selector).prop('checked') == true) {
                return '1';
            }
            else {
                return '0';
            }
        }
    </script>
</head>
<body class="body">
    <form id="Form1" runat="server">
    <asp:HiddenField runat="server" ID="HFD_GroupUid" />
    <asp:HiddenField runat="server" ID="HFD_TotalMenuItem" />
    <asp:HiddenField runat="server" ID="HFD_SetupValue" />
    <asp:HiddenField runat="server" ID="HFD_UpdateRight" />
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
    </div>
    <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        使用權限設定
    </h1>
    <table class="table_v" width="100%" align="center">
        <tr>
            <th width="10%" align="left">
                群組：
            </th>
            <td align="left">
                <asp:DropDownList ID="ddlGroup" DataTextField="GroupName" DataValueField="uid" Width="180px"
                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td width="30%" align="left">
            </td>
            <td width="40%" align="left">
                <!--<asp:ImageButton ID="btnQuery"  Style="font-size: 9pt" CssClass="button3-bg" ImageUrl="~/images/search.jpg" runat="server" OnClick="btnQuery_Click"></asp:ImageButton>-->
            </td>
        </tr>
    </table>
    <div>
        <asp:Button ID="KeyFocus" Text="" BackColor="#ffffff" BorderColor="#cff6d8" OnClick="KeyFocus_OnClick"
            BorderStyle="None" runat="server" />
    </div>
    <div id="Title" style="width:782px">
        <asp:Label ID="LB_Title" runat="server"></asp:Label>
    </div>
    <div id="mainTable" style="overflow: scroll; height: 400px; width:800px">
        <asp:Label ID="LB_Grid" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
