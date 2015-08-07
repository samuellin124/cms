<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Schedule.aspx.cs" Inherits="ScheduleMgr_Schedule" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html  xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="x-ua-compatible" content="IE=9" />
    
<title>排班管理</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <style type="text/css">
        .AddTable
        {
            border-collapse: collapse;
        }
        
        .AddTable tr td
        {
            border: 1px solid #000000;
        }
                
         .Printtd
          {
              border:1px solid #aaa;
            
          }
              .PrinttdColor
          {
              border:1px solid #aaa;
              background:#CCFFFF;
          }
            
        .style3
        {
            height: 26px;
        }
            
    </style>

    <script type ="text/javascript">
        $(document).ready(function () {
            //InitMenu();
            //LoadInit();
           

        });           


        //selectUser
        function selectUser(lblName,DayOfWeek,periodID) {
            var rDayOfWeek = DayOfWeek;
            window.open("selectVolunteer.aspx?Uid=" + $('#HFD_Uid').val()
            + "&lblName=" + lblName
            + "&Year=" + document.getElementById("ddlYear").value
            + "&Querter=" + document.getElementById("ddlQuerter").value
            + "&DayOfWeek="+DayOfWeek
            + "&periodID="+periodID,
            "選擇志工", "help=no,status=no,resizable=no,scrollbars=no,center=no,width=700px,height=500px");
            return false;
        }

        function LoadVolunteer(lblName) {
            tType = lblName;
            var a1 = $('#lblMon1').html();
            var a2 = $('#lblMon2').html();
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=LoadVolunteer' + '&tType=' + tType
                        + '&Uid=' + $('#HFD_Uid').val()
                        + '&Parting=' + $('#ddlAssetsChange').val()
                          + '&SubUid=' + $('#HFD_SubUid').val(),
                async: false,
                success: function (result) {

                    $('#' + tType + '').html(result);
                 },
                error: function () { alert('ajax failed'); }
            })
        }

        function LoadInit() {
            tType = 'lblMon1';
            var a1 = $('#lblMon1').html();
            var a2 = $('#lblMon2').html();
            $.ajax({
                type: 'post',
                url: "../common/ajax.aspx",
                data: 'Type=LoadVolunteer' + '&tType=' + tType
                        + '&Uid=' + $('#HFD_Uid').val()
                        + '&Parting=' + $('#ddlAssetsChange').val()
                          + '&SubUid=' + $('#HFD_SubUid').val(),
                async: false,
                success: function (result) {

                    $('#' + tType + '').html(result);
                },
                error: function () { alert('ajax failed'); }
            })
        }

        function PrintReport() {
            var GuardianshipUID ='1';
            var UID = '1';
            var PrintType = "";

            window.open('../Report/ScheduleRpt.aspx?Uid=' + UID +
                                                  '&PrintType=' + PrintType,

                        'ServiceRecordPrn', 'width=900,height=650,scrollbars=yes,menubar=yes,location =no,status=no,toolbar=no', '');
            return false;
        }

        //window.onload = initCalendar;
        //function initCalendar() {
        //    Calendar.setup({
        //        inputField: "txtBegCreateDate",   // id of the input field
        //        button: "imgBegCreateDate"     // 與觸發動作的物件ID相同

        //    });
        //    Calendar.setup({

        //        inputField: "txtEndCreateDate",   // id of the input field
        //        button: "imgEndCreateDate"     // 與觸發動作的物件ID相同
        //    });
        //    Calendar.showYearsCombo(true);
        //}
    </script>
</head>

<body class="body">
<form id="form1" runat ="server">
    <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        志工排班作業
    </h1>
    <table>
     <tr align ="center">
            <th align="right" width="10%">年度：</th>
            <td width="15%" align="center">
                <asp:DropDownList ID="ddlYear" runat="server">
                  
                </asp:DropDownList>
             </td>
             <th width="5%" align="right">季：</th>
             <td width="10%" align="center">
                 <asp:DropDownList ID="ddlQuerter" runat="server"> </asp:DropDownList>
             </td>
                     <%-- <th width="15%" align="right">選擇日期：</th>
         <td>
                <asp:TextBox ID="txtBegCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                <img id="imgBegCreateDate" alt="" src="../images/date.gif" />
                <asp:TextBox ID="txtEndCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                <img id="imgEndCreateDate" alt="" src="../images/date.gif" />
            </td>--%>
             <td  align="center">
                 <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server" Text="查詢" onclick="btnQuery_Click"/>
                 <asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" onclick="btnPrint_Click" /> 
             </td>
        </tr>
 
    </table>
<table  width="100%" class="table_v" cellspacing="0" >
 
    <tr style="background-color:Orange">
        <td class="Printtd">星期</td>
        <td class="Printtd">Mon</td>
        <td class="Printtd">Tue</td>
        <td class="Printtd">Wed</td>
        <td class="Printtd">Thu</td>
        <td class="Printtd">Fri</td>
        <td class="Printtd">Sat</td>
        <td class="Printtd">Sun</td>
    </tr>
    <tr>
      <td  class="Printtd"rowspan ="2">早班</td>
      <td class="Printtd"><asp:Button ID="btnMon1" runat="server" Text="編輯早班" OnClientClick="selectUser('lblMon1','Mon','A');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnTue1" runat="server" Text="編輯早班" OnClientClick="selectUser('lblTue1','Tue','A');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnWed1" runat="server" Text="編輯早班" OnClientClick="selectUser('lblWed1','Wed','A');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnThu1" runat="server" Text="編輯早班" OnClientClick="selectUser('lblThu1','Thu','A');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnFri1" runat="server" Text="編輯早班" OnClientClick="selectUser('lblFri1','Fri','A');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnSat1" runat="server" Text="編輯早班" OnClientClick="selectUser('lblSat1','Sat','A');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnSun1" runat="server" Text="編輯早班" OnClientClick="selectUser('lblSun1','Sun','A');return false;" /></td>
    </tr>

   <tr>
      <td class="PrinttdColor"><asp:Label ID="lblMon1" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblTue1" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblWed1" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblThu1" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblFri1" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblSat1" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblSun1" runat="server"></asp:Label></td>
    </tr>
         <tr>
      <td  class="Printtd"rowspan ="2">中班</td>
      <td class="Printtd"><asp:Button ID="btnMon2" runat="server" Text="編輯中班" OnClientClick="selectUser('lblMon2','Mon','B');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnTue2" runat="server" Text="編輯中班" OnClientClick="selectUser('lblTue2','Tue','B');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnWed2" runat="server" Text="編輯中班" OnClientClick="selectUser('lblWed2','Wed','B');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnThu2" runat="server" Text="編輯中班" OnClientClick="selectUser('lblThu2','Thu','B');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnFri2" runat="server" Text="編輯中班" OnClientClick="selectUser('lblFri2','Fri','B');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnSat2" runat="server" Text="編輯中班" OnClientClick="selectUser('lblSat2','Sat','B');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnSun2" runat="server" Text="編輯中班" OnClientClick="selectUser('lblSun2','Sun','B');return false;" /></td>
    </tr>

   <tr>
      <td class="PrinttdColor"><asp:Label ID="lblMon2" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblTue2" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblWed2" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblThu2" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblFri2" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblSat2" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblSun2" runat="server"></asp:Label></td>
    </tr>
    <tr>
      <td  class="Printtd"rowspan ="2">晚班</td>
      <td class="Printtd"><asp:Button ID="btnMon3" runat="server" Text="編輯晚班" OnClientClick="selectUser('lblMon3','Mon','C');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnTue3" runat="server" Text="編輯晚班" OnClientClick="selectUser('lblTue3','Tue','C');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnWed3" runat="server" Text="編輯晚班" OnClientClick="selectUser('lblWed3','Wed','C');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnThu3" runat="server" Text="編輯晚班" OnClientClick="selectUser('lblThu3','Thu','C');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnFri3" runat="server" Text="編輯晚班" OnClientClick="selectUser('lblFri3','Fri','C');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnSat3" runat="server" Text="編輯晚班" OnClientClick="selectUser('lblSat3','Sat','C');return false;" /></td>
      <td class="Printtd"><asp:Button ID="btnSun3" runat="server" Text="編輯晚班" OnClientClick="selectUser('lblSun3','Sun','C');return false;" /></td>
    </tr>

   <tr>
      <td class="PrinttdColor"><asp:Label ID="lblMon3" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblTue3" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblWed3" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblThu3" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblFri3" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblSat3" runat="server"></asp:Label></td>
      <td class="PrinttdColor"><asp:Label ID="lblSun3" runat="server"></asp:Label></td>
    </tr>

    <tr>
        <td align="right" colspan ="8" class="style3">
            <font color="Red" style="font-weight: bold">※存檔後無法再修改班表，請確定排班完成再儲存</font>  
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="儲存" CssClass="npoButton npoButton_Save" OnClientClick="return confirm ('存檔後將無法再修改班表，是否要儲存?');" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblRpt" runat="server"></asp:Label>
        </td>
    </tr>
</table>

    
<table>
    
</table>
    

</form>
</body>
</html>