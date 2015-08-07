<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Church_Edit.aspx.cs"  EnableEventValidation="false" Inherits="SysMgr_Church_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>教會資料管理</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">
        // menu控制
        $(document).ready(function () {
            InitMenu();

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


        //PostBack後，載入暫存之選取值
        function loadHfdData() {
            if ($('#HFD_ContactorArea').val() != '') {
                var CityID = $('#ddlContactorCity').val();
                $.ajax({
                    type: 'post',
                    url: "../common/ajax.aspx",
                    data: 'Type=GetAreaList' + '&CityID=' + CityID,
                    success: function (result) {
                        $('select[id$=ddlContactorArea]').html(result);
                        $("#ddlContactorArea").val($('#HFD_ContactorArea').val());
                    },
                    error: function () { alert('ajax failed'); }
                })
            }
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

        //以 CityID 取得其鄉鎮列表
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

        });
        
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 32px;
        }
        .auto-style2 {
            height: 32px;
            width: 10%;
        }
        .auto-style3 {
            width: 11%;
        }
        .auto-style4 {
            width: 17%;
        }
        .auto-style5 {
            height: 32px;
            width: 15%;
        }
        .auto-style6 {
            height: 32px;
            width: 17%;
        }
        .auto-style7 {
            height: 32px;
            width: 11%;
        }
    </style>
</head>
<body class="body">
    <form id="Form1" name="form" method="POST" runat="server">
    <asp:HiddenField ID="HFD_Mode" runat="server" />
    <asp:HiddenField ID="HFD_ChurchUID" runat="server" />
         <asp:HiddenField ID="HFD_Area" runat="server" />
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
    </div>
    <div id="container">
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            修改教會資料
        </h1>
        <table width="100%" class="table_v">

            <tr>
                <th align="right" class="auto-style3">
                    教會名稱:
                </th>
                <td class="auto-style4">
                    <asp:TextBox runat="server" ID="txtName" Width="180" MaxLength="30" ></asp:TextBox>
                </td>
                <th align="right" class="auto-style2">
                    教會電話:
                </th>
                <td colspac="3" class="auto-style5">
                      <!--OnKeyPress限制輸入數字-->
                    <asp:TextBox runat="server" ID="txtChurchPhone" Width="180" ></asp:TextBox>
                </td>
            </tr>
             <tr>
                <th width="10%" align="right">
                    教會地址:
                </th>  
                <td colspan="5" class="style1">
                    縣市：<asp:DropDownList ID="ddlCity" runat="server" Height="25px" ></asp:DropDownList>
                    區鄉鎮：<asp:DropDownList ID="ddlArea" runat="server" ></asp:DropDownList>
                    郵遞區號：<asp:TextBox runat="server" ID="txtZipCode" Width="20mm" MaxLength="5"></asp:TextBox>
                    地址：<asp:TextBox ID="txtAddress" Width="60mm" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" class="auto-style7">
                    教會Email:
                </th>
                <td colspac="3" class="auto-style6">
                    <asp:TextBox runat="server" ID="txtChurchEmail2" Width="180" MaxLength="50"  ></asp:TextBox>
                </td>
                <th align="right" class="auto-style2">
                    主任牧師:
                </th>
                <td colspac="3" class="auto-style1">
                    <asp:TextBox runat="server" ID="txtPriest" Width="180" MaxLength="30"  ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" class="auto-style7">
                    主任牧師電話:
                </th>
                <td colspac="3" class="auto-style6">
                      <!--OnKeyPress限制輸入數字-->
                    <asp:TextBox runat="server" ID="txtPriestPhone" Width="180" ></asp:TextBox>
                </td>
                <th align="right" class="auto-style2">
                    主任牧師Email:
                </th>
                <td colspac="3" class="auto-style1">
                    <asp:TextBox runat="server" ID="txtPriestEmail" Width="180" MaxLength="30"  ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" class="auto-style7">
                    聯絡窗口:
                </th>
                <td colspac="3" class="auto-style6">
                    <asp:TextBox runat="server" ID="txtContact" Width="180" MaxLength="50"  ></asp:TextBox>
                </td>
                <th align="right" class="auto-style2">
                    窗口手機:
                </th>
                <td colspac="3" class="auto-style5">
                    <!--OnKeyPress限制輸入數字-->
                    <asp:TextBox runat="server" ID="txtChurchMphone" Width="180"></asp:TextBox>
                </td>
             </tr>
            <tr>
                 <th align="right" class="auto-style7">
                    窗口Email:
                </th>
                <td colspan="3" class="auto-style6">
                    <asp:TextBox runat="server" ID="txtChurchEmail" Width="700"  ></asp:TextBox>
                </td>
            </tr>
       
            <tr>
                <th align="right" class="auto-style7">
                    教會事工:
                </th>
                <td  class="auto-style6">
                    <asp:CheckBox ID="CHK_Mministry1" Text="婚前" runat="server" />
                    <asp:CheckBox ID="CHK_Mministry2" Text="婚姻" runat="server" />
                    <asp:CheckBox ID="CHK_Mministry3" Text="青少" runat="server" />
                    <asp:CheckBox ID="CHK_Mministry4" Text="老人" runat="server" />
                    </td>
             </tr>
            <tr>
               <th align="right" class="auto-style7">
                </th>
                 <td colspan="2" class="auto-style6">
                    <asp:CheckBox ID="CHK_Mministry5" Text="家庭關懷" runat="server" />
                    <asp:CheckBox ID="CHK_Mministry6" Text="協談輔導" runat="server" />
                    <asp:CheckBox ID="CHK_Mministry7" Text="其他" runat="server" OnCheckedChanged="CHK_Mministry7_CheckedChanged" AutoPostBack="True" />
                     <asp:TextBox ID="txt_Mministry7" runat="server" OnTextChanged="txt_Mministry7_TextChanged" AutoPostBack="True"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                 <th align="right" class="auto-style7">
                    加入時間:
                </th>
                <td colspan="3" class="auto-style6">
                    <asp:TextBox runat="server" ID="txtInsertDate" Width="200" ReadOnly="true" BackColor="#ffffcc"></asp:TextBox>
                </td>
            </tr>
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
            <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" 
                onclick="btnPrint_Click" />                 
             
        </div>
    </div>
    </form>
            <td>
            <asp:Label ID="lblRpt" runat="server"></asp:Label>
        </td>
</body>
</html>
