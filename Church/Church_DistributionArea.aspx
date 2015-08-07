<%@ Page language="c#"  AutoEventWireup="true"  CodeFile="Church_DistributionArea.aspx.cs" Inherits="Church_DistributionArea" %>
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

        // menu控制
        $(document).ready(function () {
            InitMenu();
        });
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
    <div id="menucontrol">
        <a href="#"><img id="menu_hide" alt="" src="../images/menu_hide.gif" width="15" height="60" class="swapImage {src:'../images/menu_hide_over.gif'}" onclick="SwitchMenu();return false;" /></a>
        <a href="#"><img id="menu_show" alt="" src="../images/menu_show.gif" width="15" height="60" class="swapImage {src:'../images/menu_show_over.gif'}" onclick="SwitchMenu();return false;"/></a>
    </div>
       <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        <asp:Label ID="Label22" runat="server" Text="轉介非基督教個案分布區域"></asp:Label>
       </h1>

	<table width="100%" class="table_v">
        <tr>
             <th align="center" width="50px">新北市</th>
             <th align="center" width="50px">桃竹區</th>
             <th align="center" width="50px">中彰區</th>
             <th align="center" width="50px">嘉南區</th>
             <th align="center" width="50px">高雄區</th>
             <th align="center" width="50px">基宜花</th>
             <th align="center" class="auto-style32">台北市</th>
            <th align="center" width="50px">總計</th>
       </tr>        
        <tr>
            <td width="50px" align="center">
                <asp:Label ID="LB_NTCity" runat="server" Text=""></asp:Label>
            </td>
            <td width="50px"align="center">
                <asp:Label ID="LB_THArea" runat="server" Text=""></asp:Label>
            </td>
            <td width="50px"align="center">
                <asp:Label ID="LB_TCArea" runat="server" Text=""></asp:Label>
            </td>
            <td width="50px"align="center">
                <asp:Label ID="LB_CTArea" runat="server" Text=""></asp:Label>
            </td>
            <td width="50px"align="center">
                <asp:Label ID="LB_KaohsiungArea" runat="server" Text=""></asp:Label>
            </td>
            <td width="50px"align="center">
                <asp:Label ID="LB_KIHArea" runat="server" Text=""></asp:Label>
            </td>
            <td align="center" class="auto-style32">
                <asp:Label ID="LB_TaipeiCity" runat="server" Text=""></asp:Label>
            </td>
            <td width="50px"align="center">
                <asp:Label ID="citysum" runat="server" Text=""></asp:Label>
            </td>
            <td width="50px"align="center">
               <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" 
                onclick="btnPrint_Click" />
            </td>
       </tr>
				<tr>
					<td class="auto-style4" colspan="9">
						<asp:CHART id="Chart1" runat="server" Palette="BrightPastel" BackColor="WhiteSmoke" Height="296px" Width="600px" BorderlineDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
							<titles>
								<asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Text="轉介個案區域比例" Name="Title1" ForeColor="26, 59, 105"></asp:Title>
							</titles>
							<legends>
								<asp:Legend BackColor="Transparent" Alignment="Center" Docking="Bottom" Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" Name="Default" LegendStyle="Row"></asp:Legend>
							</legends>
							<borderskin SkinStyle="Emboss"></borderskin>
							<series>
								<asp:Series Name="Default" ChartType="Pie" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240"></asp:Series>
							</series>
							<chartareas>
								<asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent" BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
									<area3dstyle Rotation="0" />
									<axisy LineColor="64, 64, 64, 64">
										<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisy>
									<axisx LineColor="64, 64, 64, 64">
										<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
										<MajorGrid LineColor="64, 64, 64, 64" />
									</axisx>
								</asp:ChartArea>
							</chartareas>
						</asp:CHART>
					</td>
					<td valign="top">
						<table class="controls" cellpadding="4">
							<%--<tr>
								<td class="label">圖表類型:</td>
								<td><asp:dropdownlist id="ChartTypeList" runat="server" AutoPostBack="True" CssClass="spaceright" Width="112px">
										<asp:ListItem Value="Doughnut">Doughnut</asp:ListItem>
										<asp:ListItem Value="Pie">Pie</asp:ListItem>
									</asp:dropdownlist>
								</td>
							</tr>
							<tr>
								<td class="label">標籤樣式:</td>
								<td>
                                       <asp:dropdownlist id="LabelStyleList" runat="server" AutoPostBack="True" CssClass="spaceright" Width="112px">
										<asp:ListItem Value="Inside" Selected="True">Inside</asp:ListItem>
										<asp:ListItem Value="Outside">Outside</asp:ListItem>
										<asp:ListItem Value="Disabled">不顯示</asp:ListItem>
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
								<td class="label">圓環半徑(%):</td>
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
								<td class="label">顯示圖例:</td>
								<td><asp:checkbox id="ShowLegend" runat="server" AutoPostBack="True" Checked="True" Text=""></asp:checkbox></td>
							</tr>
							<tr>
								<TD class="label">繪畫風格:</td>
								<td>
									<asp:dropdownlist id="Dropdownlist1" runat="server" Width="112px" CssClass="spaceright" AutoPostBack="True">
										<asp:ListItem Value="Default">Default</asp:ListItem>
										<asp:ListItem Value="SoftEdge" Selected="True">SoftEdge</asp:ListItem>
										<asp:ListItem Value="Concave">Concave</asp:ListItem>
									</asp:dropdownlist></td>
							</tr>
							<tr>
								<td class="label">顯示為3D:</td>
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
