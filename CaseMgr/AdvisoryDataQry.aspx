<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdvisoryDataQry.aspx.cs"    EnableEventValidation="false"
    Inherits="CaseMgr_AdvisoryDataQry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link rel="shortcut icon" href="../FAVICON.ico"> 
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>查詢作業</title>
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
            //initCalendar(['StartDate', 'EndDate']);
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
        //PostBack後，載入暫存之選取值
        function loadHfdData() {
            if (window.opener.$('#HFD_ContactorArea').val() != '') {
                var CityID = window.opener.$('#ddlContactorCity').val();
                $.ajax({
                    type: 'post',
                    url: "../common/ajax.aspx",
                    data: 'Type=1' + '&CityID=' + CityID,
                    success: function (result) {
                        window.opener.$('select[id$=ddlContactorArea]').html(result);
                        window.opener.$("#ddlContactorArea").val(window.opener.$('#HFD_ContactorArea').val());
                    },
                    error: function () { alert('ajax failed'); }
                })
            }
            if (window.opener.$('#HFD_Area').val() != '') {
                var CityID = window.opener.$('#ddlCity').val();
                $.ajax({
                    type: 'post',
                    url: "../common/ajax.aspx",
                    data: 'Type=1' + '&CityID=' + CityID,
                    success: function (result) {
                        window.opener.$('select[id$=ddlArea]').html(result);
                        window.opener.$("#ddlArea").val(window.opener.$('#HFD_Area').val());
                    },
                    error: function () { alert('ajax failed'); }
                })
            }
        }
     </script>

          <script type ="text/javascript">



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
            height: 46px;
        }
    </style>
</head>
<body class="body">
    <form id="Form1" runat="server">
    <asp:HiddenField ID="HFD_DonorName" runat="server" />
    <asp:HiddenField ID="HFD_TelHome" runat="server" />
    <asp:HiddenField ID="HFD_TelOffice" runat="server" />
    <asp:HiddenField ID="HFD_QryType" runat="server" />
    <asp:HiddenField ID="HFD_MemberID" runat="server" />
    <asp:HiddenField ID ="HFD_LoadFormData" runat ="server" />
    <asp:HiddenField ID ="HFD_Area" runat="server" />
    <asp:HiddenField ID ="HFD_SQL" runat="server" />
    <asp:HiddenField ID="HFD_ConsultantItem" runat="server" />
    <asp:HiddenField ID="HFD_EditServiceUser" runat="server" />
    <asp:HiddenField ID="HFD_MOD" runat="server" />
    <asp:HiddenField ID="HFD_Marry" runat="server" />
    <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
         查詢作業
    </h1>
     <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
         <tr>
        <th align="right">
           姓名：
            </th>
            <td>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </td>
         
            <th align="right">
               電話：
            </th>
            <td>
                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
            </td>
            
            
              <th align="right">
                建檔時間：
            </th>
            <td>
                <asp:TextBox ID="txtBegCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                <img id="imgBegCreateDate" alt="" src="../images/date.gif" />
                <asp:TextBox ID="txtEndCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                <img id="imgEndCreateDate" alt="" src="../images/date.gif" />
            </td>
        </tr>
        <tr>
           
            <th align="right" >
                志工：
            </th>
            <td colspan="1">
                <asp:TextBox runat="server" Width="50mm" ID="txtServiceUser"></asp:TextBox>
            </td>
             <th align="right">婚姻：</th>
                            <td  style="width :60mm">
                              <asp:Label ID="lblMarry" runat="server"></asp:Label>
                             
                            </td>
            
             <th align="right" class="weekend">
               事件關鍵字：
            </th>
            <td>
                <asp:TextBox ID="txtEvent" runat="server"></asp:TextBox>
            </td>
                 
        </tr>
         <tr>
                                <th align="left" style ="width:40mm" >
                                來電諮詢別(分項)：
                                </th>
                                <td colspan ="5">
                                   <asp:Label ID="lblConsultantItem" runat="server"></asp:Label>
                                </td>
                            </tr>
        <tr>
           <th align="right">地址：
                            </th>
                            <td  colspan="5">
                            縣市：<asp:DropDownList ID="ddlCity" runat="server" ></asp:DropDownList>
                                區鄉鎮：<asp:DropDownList ID="ddlArea" runat="server" ></asp:DropDownList>
                                郵遞區號：<asp:TextBox runat="server" ID="txtZipCode" Width="20mm" MaxLength="5"></asp:TextBox>
                                地址：<asp:TextBox ID="txtAddress" Width="60mm" runat="server"></asp:TextBox>
                            </td>  
                         
                            
        </tr>
        <tr>
            <td align="left" colspan ="5">
                &nbsp;</td>
            <td align="right" colspan ="1">
                <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server"
                    Text="查詢" OnClick="btnQuery_Click"/>
            </td>
        </tr>
              <tr>
            <td align="center" colspan="6" class="auto-style1">
                <asp:Label ID="Label1" runat="server" Font-Names="標楷體" Font-Size="X-Large" 
                    ForeColor="#006600" Text="若已有同姓名但還是要新增的話請按離開鍵---&gt;" Visible="False"></asp:Label>
                <asp:Button ID="btnCreate" runat="server" Height="40px" OnClick="btnCreate_Click"
                    Text="離開" Visible="False" 
                    ForeColor="Red" CssClass="npoButton npoButton_Exit" Font-Bold="False" 
                    Font-Size="Larger" />
            </td>
                 <%-- 以下為與來電個案可能重複,請判斷後選取或關閉視窗後新建個案資料--%>

        </tr>


    </table>
    
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <td width="80" colspan="7" align ="center" >
                    <asp:GridView ID="gdvDonorQry" runat="server"  
                        OnRowCommand="gdvDonorQry_RowCommand" AutoGenerateColumns="False" 
                        BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="3" GridLines="Horizontal" Width="900px">
                        <AlternatingRowStyle BackColor="#F7F7F7" />
                        <Columns>
                            <asp:ButtonField ButtonType="Button" CommandName="btnSelect" HeaderText="選取" 
                                Text="選取" DataTextFormatString="uid" />
                            <asp:BoundField DataField="姓名" HeaderText="姓名" >
                          
                            <ItemStyle Width="300px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="備註" HeaderText="備註">
                            <ItemStyle Width="300px" HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="電話" HeaderText="電話" HtmlEncode="False">
                            <ItemStyle Width="300px" HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="uid" SortExpression="uid" ShowHeader="False">
                            <ControlStyle Width="0px" />
                            <FooterStyle Width="1px" />
                            <HeaderStyle Width="1px" />
                            <ItemStyle Width="2px" ForeColor="White" />
                            </asp:BoundField>
                          
                        </Columns>
                        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                        <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                        <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                        <SortedAscendingCellStyle BackColor="#F4F4FD" />
                        <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                        <SortedDescendingCellStyle BackColor="#D8D8F0" />
                        <SortedDescendingHeaderStyle BackColor="#3E3277" />
                    </asp:GridView>
                    <br />
                    <%--<asp:Label ID="lblGridList" runat="server"></asp:Label>--%>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
