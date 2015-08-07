<%@ Page Language="c#" AutoEventWireup="true" CodeFile="StatusOfAnalysis.aspx.cs" EnableEventValidation="false" Inherits="StatusOfAnalysis" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=big5" />
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

        // menu����
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


            //PostBack��A���J�Ȧs�������
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

            //�H CityID ���o��m��C��
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
                button: "imgBegCreateDate"     // �PĲ�o�ʧ@������ID�ۦP

            });
            Calendar.setup({

                inputField: "txtEndCreateDate",   // id of the input field
                button: "imgEndCreateDate"     // �PĲ�o�ʧ@������ID�ۦP
            });
            Calendar.showYearsCombo(true);
        }
    </script>
    <style type="text/css">
        .auto-style4 {
            width: 412px;
        }
        .auto-style6 {
            width: 139px;
        }
        .auto-style7 {
            width: 469px;
        }
    </style>
</head>
<body class="body">
    <form id="Form1" name="form" runat="server">
        <asp:HiddenField ID="HFD_Area" runat="server" />
        <asp:HiddenField ID="HFD_ConsultantItem" runat="server" />
        <asp:HiddenField ID="HFD_Yvalue" runat="server" />
        
        <div id="menucontrol">
            <a href="#">
                <img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
            <a href="#">
                <img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        </div>
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            <asp:Label ID="Label22" runat="server" Text="�{�p���R"></asp:Label>
        </h1>

        <table width="100%" class="table_v">
            <tr>
                <th align="center" width="110px">�����G</th>
                <td colspan="1" class="style1">
                    <asp:DropDownList ID="ddlCity" runat="server" Height="25px"></asp:DropDownList>
                </td>
                        <th align="right">�ɶ��϶��G
        </th>
        <td colspan="2">
            <asp:TextBox ID="txtBegCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '���');"></asp:TextBox>
            <img id="imgBegCreateDate" alt="" src="../images/date.gif" />
            <asp:TextBox ID="txtEndCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '���');"></asp:TextBox>
            <img id="imgEndCreateDate" alt="" src="../images/date.gif" />
        </td>
                <%--<th align="center" width="110px">�ϰ�G</th>
                <td colspan="1" class="style1">
                    <asp:DropDownList ID="ddlArea" runat="server" Height="19px"></asp:DropDownList>
                </td>--%>
                <td align="left" class="auto-style7">
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="�ϰ�d��"
                        OnClick="btnQuery_Click" Width="105px" />
                    <asp:Button ID="btnAllQuery" class="npoButton npoButton_Search" runat="server" Text="�������d��"
                        OnClick="btnAllQuery_Click" Width="110px" />
                    <asp:Button ID="btnNCity" class="npoButton npoButton_Search" runat="server" Text="����a�}" Width="110px"
                OnClick="btnNCity_Click" />
                </td>
                
                <td width="50px" align="center">
                    <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="�C�L"
                        OnClick="btnPrint_Click" />
                </td>
            </tr>
            <tr>
                <th align="center" width="110px">���~</th>
                <th align="center" width="110px">í�w�E�|</th>
                <th align="center" width="110px">���ӻE�|</th>
                <th align="center" width="110px">���ֶi�J�з|</th>
                <th align="center" class="auto-style6">���h��</th>
            </tr>
            <tr>
                <td width="50px" align="center">
                    <asp:Label ID="LB_FollowUp1" runat="server" Text=""></asp:Label>
                </td>
                <td width="50px" align="center">
                    <asp:Label ID="LB_FollowUp2" runat="server" Text=""></asp:Label>
                </td>
                <td width="50px" align="center">
                    <asp:Label ID="LB_FollowUp3" runat="server" Text=""></asp:Label>
                </td>
                <td width="50px" align="center">
                    <asp:Label ID="LB_FollowUp4" runat="server" Text=""></asp:Label>
                </td>
                <td align="center" class="auto-style6">
                    <asp:Label ID="LB_FollowUp5" runat="server" Text=""></asp:Label>
                </td>

            </tr>
            <tr>
                <td width="50px" align="center">
                    <asp:Label ID="lblsum1" runat="server" Text=""></asp:Label>
                </td>
                <td width="50px" align="center">
                    <asp:Label ID="lblsum2" runat="server" Text=""></asp:Label>
                </td>
                <td width="50px" align="center">
                    <asp:Label ID="lblsum3" runat="server" Text=""></asp:Label>
                </td>
                <td width="50px" align="center">
                    <asp:Label ID="lblsum4" runat="server" Text=""></asp:Label>
                </td>
                <td align="center" class="auto-style6">
                    <asp:Label ID="lblsum5" runat="server" Text=""></asp:Label>
                </td>

            </tr>
            <tr>
                <td class="auto-style4" colspan="9">
                    <asp:Chart ID="Chart1" runat="server" Palette="BrightPastel" BackColor="WhiteSmoke" Height="1000px" Width="1100px" BorderlineDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                        <Titles>
                            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Text="�श�Ӯײ{�p���R" Name="Title1" ForeColor="26, 59, 105"></asp:Title>
                        </Titles>
                          <Legends>
                            <%--�ϧQ�r��--%>
                            <asp:Legend BackColor="Transparent" Alignment="Center" Docking="Bottom" Font="Arial Unicode MS, 12pt, style=Bold" IsTextAutoFit="False" Name="Default" LegendStyle="Row"></asp:Legend>
                        </Legends>
                        <BorderSkin SkinStyle="Emboss"></BorderSkin>
                        <Series>
                          <%--���ϸ̪��r��--%>
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
                    <table class="controls" cellpadding="4">
                        <%--<tr>
								<td class="label">�Ϫ�����:</td>
								<td><asp:dropdownlist id="ChartTypeList" runat="server" AutoPostBack="True" CssClass="spaceright" Width="112px">
										<asp:ListItem Value="Doughnut">Doughnut</asp:ListItem>
										<asp:ListItem Value="Pie">Pie</asp:ListItem>
									</asp:dropdownlist>
								</td>
							</tr>
							<tr>
								<td class="label">���Ҽ˦�:</td>
								<td>
                                       <asp:dropdownlist id="LabelStyleList" runat="server" AutoPostBack="True" CssClass="spaceright" Width="112px">
										<asp:ListItem Value="Inside" Selected="True">Inside</asp:ListItem>
										<asp:ListItem Value="Outside">Outside</asp:ListItem>
										<asp:ListItem Value="Disabled">�����</asp:ListItem>
									</asp:dropdownlist>
								</td>
							</tr>
							<tr>
								<td class="label">Exploded Point:</td>
								<td>
                                    <asp:dropdownlist id="ExplodedPointList" runat="server" AutoPostBack="True" CssClass="spaceright" Width="112px">
										<asp:ListItem Value="None" Selected="True">None</asp:ListItem>
										<asp:ListItem Value="France">France</asp:ListItem>
										<asp:ListItem Value="Canada">Canada</asp:ListItem>
										<asp:ListItem Value="UK">UK</asp:ListItem>
										<asp:ListItem Value="USA">USA</asp:ListItem>
										<asp:ListItem Value="Italy">Italy</asp:ListItem>
									</asp:dropdownlist>
								</td>
							</tr>
							<tr>
								<td class="label">�����b�|(%):</td>
								<td><asp:dropdownlist id="HoleSizeList" runat="server" AutoPostBack="True" Enabled="False" CssClass="spaceright" Width="112px">
										<asp:ListItem Value="20">20</asp:ListItem>
										<asp:ListItem Value="30">30</asp:ListItem>
										<asp:ListItem Value="40">40</asp:ListItem>
										<asp:ListItem Value="50">50</asp:ListItem>
										<asp:ListItem Value="60" Selected="True">60</asp:ListItem>
										<asp:ListItem Value="70">70</asp:ListItem>
									</asp:dropdownlist>
								</td>
							</tr>
							<tr>
								<td class="label">��ܹϨ�:</td>
								<td><asp:checkbox id="ShowLegend" runat="server" AutoPostBack="True" Checked="True" Text=""></asp:checkbox></td>
							</tr>
							<tr>
								<TD class="label">ø�e����:</td>
								<td>
									<asp:dropdownlist id="Dropdownlist1" runat="server" Width="112px" CssClass="spaceright" AutoPostBack="True">
										<asp:ListItem Value="Default">Default</asp:ListItem>
										<asp:ListItem Value="SoftEdge" Selected="True">SoftEdge</asp:ListItem>
										<asp:ListItem Value="Concave">Concave</asp:ListItem>
									</asp:dropdownlist></td>
							</tr>
							<tr>
								<td class="label">��ܬ�3D:</td>
								<td><asp:checkbox id="CheckboxShow3D" runat="server" AutoPostBack="True" Text=""></asp:checkbox></td>
							</tr>--%>
                    </table>
                </td>
            </tr>
        </table>
        <p class="dscr">&nbsp;</p>
    </form>
    <td>
        <asp:Label ID="lblRpt" runat="server"></asp:Label>
    </td>
</body>
</html>
