<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="CasesNumber.aspx.cs" Inherits="CasesNumber" %>


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

            //婚姻狀況
            $('input:checkbox[id*=CHK_Marry]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="Marry"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_Marry').val(ret);
            });

            //來電諮詢別(分項)
            $('input:checkbox[id*=CHK_ConsultantItem]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="ConsultantItem"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_ConsultantItem').val(ret);
            });

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

    </script>
    <script type="text/javascript">
        //-------------
        function LoadConsultQry(tType) {
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=LoadConsultQry' + '&tType=' + tType
                        + '&Phone=' + $('#txtPhone').val()
                        + '&CName=' + $('#txtName').val()
                        + '&BegCreateDate=' + $('#txtBegCreateDate').val()
                        + '&EndCreateDate=' + $('#txtEndCreateDate').val()
                        + '&EditServiceUser=' + $('#HFD_EditServiceUser').val()
                        + '&Event=&Marry=&City=&ZipCode=&Address=&ServiceUser=&ConsultantItem=',

                async: false,
                success: function (result) {

                    $('#lblGridList').html(result);
                    // alert("完成!");
                    //                  
                },
                error: function () { alert('ajax failed'); }
            })
        }
        //***************

    </script>



    <script type="text/javascript">


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
        <asp:HiddenField ID="HFD_MemberID" runat="server" />
        <asp:HiddenField ID="HFD_LoadFormData" runat="server" />
        <asp:HiddenField ID="HFD_Area" runat="server" />
        <asp:HiddenField ID="HFD_SQL" runat="server" />
        <asp:HiddenField ID="HFD_ConsultantItem" runat="server" />
        <asp:HiddenField ID="HFD_EditServiceUser" runat="server" />
        <asp:HiddenField ID="HFD_Marry" runat="server" />

        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" onclick="SwitchMenu();return false;" /></a>
        </div>
        <asp:Label runat="server" ID="Toolbar"></asp:Label>
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            <asp:Literal ID="litTitle" runat="server" Text="個案數區域分佈"></asp:Literal>
        </h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <td width="100%" colspan="10">
                    <asp:Literal runat="server" ID="MainMenu"></asp:Literal>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th align="right" nowrap>姓名：
                </th>
                <td>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
                <th align="right" nowrap>電話：
                </th>
                <td>
                    <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                </td>
                <th align="right" nowrap>年齡： 
                </th>
                <td nowrap>約:<asp:DropDownList ID="ddlAge" runat="server">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>0~10</asp:ListItem>
                    <asp:ListItem>11~15</asp:ListItem>
                    <asp:ListItem>16~20</asp:ListItem>
                    <asp:ListItem>21~25</asp:ListItem>
                    <asp:ListItem>26~30</asp:ListItem>
                    <asp:ListItem>31~35</asp:ListItem>
                    <asp:ListItem>36~40</asp:ListItem>
                    <asp:ListItem>41~45</asp:ListItem>
                    <asp:ListItem>46~50</asp:ListItem>
                    <asp:ListItem>51~55</asp:ListItem>
                    <asp:ListItem>56~60</asp:ListItem>
                    <asp:ListItem>61~65</asp:ListItem>
                    <asp:ListItem>66~70</asp:ListItem>
                    <asp:ListItem>71~75</asp:ListItem>
                    <asp:ListItem>76~80</asp:ListItem>
                    <asp:ListItem>81~85</asp:ListItem>
                    <asp:ListItem>86~90</asp:ListItem>
                    <asp:ListItem>91~95</asp:ListItem>
                    <asp:ListItem>96~100</asp:ListItem>
                    <asp:ListItem>未明</asp:ListItem>
                </asp:DropDownList>
                    歲
                </td>
                <th align="right" nowrap>性別：
                </th>
                <td nowrap>
                    <asp:RadioButtonList ID="rdoSex" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem>男</asp:ListItem>
                        <asp:ListItem>女</asp:ListItem>
                        <asp:ListItem>全部</asp:ListItem>

                    </asp:RadioButtonList>
                </td>
                <td align="right">
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server"
                        Text="查詢" OnClick="btnQuery_Click" />
                </td>
                
            </tr>
            <tr>
                <th align="right" nowrap>查詢地址：
                </th>
                <td colspan="1" align="left">縣市：<asp:DropDownList ID="ddlCity" runat="server" Height="25px"></asp:DropDownList>
                    <%--區鄉鎮：<asp:DropDownList ID="ddlArea" runat="server" ></asp:DropDownList>--%>
                </td>
                <th align="right" nowrap>時間區間：
                </th>
                <td nowrap>
                    <asp:TextBox ID="txtBegCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgBegCreateDate" alt="" src="../images/date.gif" />
                    <asp:TextBox ID="txtEndCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgEndCreateDate" alt="" src="../images/date.gif" />
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="CKB_Firstphone" runat="server" Text="第一次來電" />
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="CKB_Christian" runat="server" Text="是基督徒" />
                </td>
                <td align="right">
                    <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印"
                        OnClick="btnPrint_Click" />
                </td>
            </tr>
    </table>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblnumber" runat="server" Text=""></asp:Label>
                </td>
            </tr>
    </table>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <td class="auto-style4" colspan="20">
                    <asp:Chart ID="Chart1" runat="server" BackColor="WhiteSmoke" Height="1000px" Width="1100px" BorderlineDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" BorderColor="#1A3B69" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                        <Titles>
                            <asp:Title runat="server" ShadowColor="32, 0, 0, 0" Font="Arial Unicode MS, 14.25pt, style=Bold" ShadowOffset="3" Text="個案數區域分佈" Name="Title1" ForeColor="26, 59, 105"></asp:Title>
                        </Titles>
                        <Legends>
                            <%--圖利字型--%>
                            <asp:Legend BackColor="Transparent" Alignment="Center" Docking="Bottom" Font="Arial Unicode MS, 12pt, style=Bold" IsTextAutoFit="False" Name="Default" LegendStyle="Row"></asp:Legend>
                        </Legends>
                        <BorderSkin SkinStyle="Emboss"></BorderSkin>
                        <Series>
                            <%--圓餅圖裡的字型--%>
                            <asp:Series Name="Default" ChartType="Pie" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" Font="Arial Unicode MS, 12pt"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent" BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
                                <Area3DStyle Rotation="0" />
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>

            </tr>
            <tr>
                <td width="100%" colspan="11">

                    <asp:Label ID="lblGridList" runat="server"></asp:Label>
                    <%--  <asp:Label ID="lblReport" runat="server" CssClass="Button"></asp:Label>--%>
                </td>
            </tr>


        </table>
    </form>
    <td>
        <asp:Label ID="lblRpt" runat="server"></asp:Label>
    </td>
</body>
</html>
