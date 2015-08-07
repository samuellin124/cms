<%@ Page Language="c#" AutoEventWireup="true" CodeFile="AreaAnalysis.aspx.cs" EnableEventValidation="false" Inherits="AreaAnalysis" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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
        .auto-style4 {
            width: 412px;
        }

        .auto-style32 {
            width: 50px;
        }
    </style>
</head>
<body class="body">
    <form id="Form1" name="form" runat="server">
        <asp:HiddenField ID="HFD_Area" runat="server" />
                <asp:HiddenField ID="HFD_ConsultantItem" runat="server" />
        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        </div>
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            <asp:Label ID="lblTitle" runat="server" Text="區域分析"></asp:Label>

        </h1>

        <td width="100%" class="table_v">
                <th align="center" width="50px">縣市：</th>
                <td colspan="1" class="style1" width="50px">
                    <asp:DropDownList ID="ddlCity" runat="server" Height="25px" Width="80px"></asp:DropDownList>
                </td>
        </td>
        <th align="right">時間區間：
        </th>
        <td>
            <asp:TextBox ID="txtBegCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
            <img id="imgBegCreateDate" alt="" src="../images/date.gif" />
            <asp:TextBox ID="txtEndCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
            <img id="imgEndCreateDate" alt="" src="../images/date.gif" />
        </td>
        <%--             <th align="center" width="50px">區域：</th>
             <td colspan="1" class="style1"  width="50px">
                    <asp:DropDownList ID="ddlArea" runat="server" Height="19px" width="80px" ></asp:DropDownList>
             </td>--%>
        <td width="50px" align="left">
            <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="區域查詢"
                OnClick="btnQuery_Click" Width="120px" />
        </td>
        <td width="50px" align="left">
            <asp:Button ID="btnQuerycity" class="npoButton npoButton_Search" runat="server" Text="全縣市查詢" Width="140px"
                OnClick="btnQuerycity_Click" />
            <asp:Button ID="btnNCity" class="npoButton npoButton_Search" runat="server" Text="未填地址" Width="110px"
                OnClick="btnNCity_Click" />
            <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" OnClick="btnPrint_Click" />
        </td>
        </tr>
            <tr>
                <td width="50px" align="left">
                    <asp:Label ID="lblRpt" runat="server"></asp:Label>
                </td>

            </tr>
        <tr>
            <td class="auto-style4" colspan="20">
                <asp:Chart ID="Chart1" runat="server" Palette="BrightPastel" BackColor="WhiteSmoke" Height="1000px" Width="1100px" BorderlineDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                    <Titles>
                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Arial Unicode MS, 14.25pt, style=Bold" ShadowOffset="3" Text="轉介個案區域分析" Name="Title1" ForeColor="26, 59, 105"></asp:Title>
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
            <td valign="top">
                <%-- <table class="controls" cellpadding="4">
                       <%--    <tr>
                            <td class="label">圖表類型:</td>
                            <td>
                                <asp:DropDownList ID="ChartTypeList" runat="server" AutoPostBack="True" CssClass="spaceright" Width="112px">
                                    <asp:ListItem Value="Doughnut">Doughnut</asp:ListItem>
                                    <asp:ListItem Value="Pie">Pie</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="label">標籤樣式:</td>
                            <td>
                                <asp:DropDownList ID="LabelStyleList" runat="server" AutoPostBack="True" CssClass="spaceright" Width="112px">
                                    <asp:ListItem Value="Inside" Selected="True">Inside</asp:ListItem>
                                    <asp:ListItem Value="Outside">Outside</asp:ListItem>
                                    <asp:ListItem Value="Disabled">不顯示</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="label">Exploded Point:</td>
                            <td>
                                <asp:DropDownList ID="ExplodedPointList" runat="server" AutoPostBack="True" CssClass="spaceright" Width="112px">
                                    <asp:ListItem Value="None" Selected="True">None</asp:ListItem>
                                    <asp:ListItem Value="France">France</asp:ListItem>
                                    <asp:ListItem Value="Canada">Canada</asp:ListItem>
                                    <asp:ListItem Value="UK">UK</asp:ListItem>
                                    <asp:ListItem Value="USA">USA</asp:ListItem>
                                    <asp:ListItem Value="Italy">Italy</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="label">圓環半徑(%):</td>
                            <td>
                                <asp:DropDownList ID="HoleSizeList" runat="server" AutoPostBack="True" Enabled="False" CssClass="spaceright" Width="112px">
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="30">30</asp:ListItem>
                                    <asp:ListItem Value="40">40</asp:ListItem>
                                    <asp:ListItem Value="50">50</asp:ListItem>
                                    <asp:ListItem Value="60" Selected="True">60</asp:ListItem>
                                    <asp:ListItem Value="70">70</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="label">顯示圖例:</td>
                            <td>
                                <asp:CheckBox ID="ShowLegend" runat="server" AutoPostBack="True" Checked="True" Text=""></asp:CheckBox></td>
                        </tr>
                        <tr>
                            <td class="label">繪畫風格:</td>
                            <td>
                                <asp:DropDownList ID="Dropdownlist1" runat="server" Width="112px" CssClass="spaceright" AutoPostBack="True">
                                    <asp:ListItem Value="Default">Default</asp:ListItem>
                                    <asp:ListItem Value="SoftEdge" Selected="True">SoftEdge</asp:ListItem>
                                    <asp:ListItem Value="Concave">Concave</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="label">顯示為3D:</td>
                            <td>
                                <asp:CheckBox ID="CheckboxShow3D" runat="server" AutoPostBack="True" Text=""></asp:CheckBox></td>
                        </tr>
                    </table>--%>
            </td>
        </tr>
        <table width="100%">
        </table>
        <p class="dscr">&nbsp;</p>
    </form>



</body>
</html>
