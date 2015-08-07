<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="ConsultAdd.aspx.cs" Inherits="CaseMgr_ConsultAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>諮詢作業</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript" src="../include/jquery.address.js"></script>
    <script type="text/javascript" src="../include/jquery.field.js"></script>
    <script type="text/javascript">
        // menu控制
        $(document).ready(function () {
            $('#btnReload').hide();

            InitMenu();

            $("#ddlCity").bind("change", function (e) {
                ChangeArea(e, 'ddlArea');
            });

            $("#ddlContactorCity").bind("change", function (e) {
                ChangeArea(e, 'ddlContactorArea');
            });

            $("#ddlArea").bind("change", function (e) {
                var ZipCode = $(this).val();
                if (ZipCode != '0') {
                    $('#txtZipCode').val(ZipCode);
                    $('#HFD_Area').val(ZipCode);
                }
            });

            $("#ddlContactorArea").bind("change", function (e) {
                var ZipCode = $(this).val();
                if (ZipCode != '0') {
                    $('#txtContactorZipCode').val(ZipCode);
                    $('#HFD_ContactorArea').val(ZipCode);
                }
            });

            //新增時不要出現的物件在此控制
            var Mode = $('#HFD_Mode').val();
            if (Mode == 'ADD') {
                $('.HideWhenAdd').hide()
            }

            //*****************************************************************************************
            //婚姻狀況
            $('input:checkbox[id*=CHK_Marry]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="Marry"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_Marry').val(ret);
            });
            //來電諮詢別(大類)
            $('input:checkbox[id*=CHK_ConsultantMain]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="ConsultantMain"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_ConsultantMain').val(ret);
            });

            //來電諮詢別(分項)
            $('input:checkbox[id*=CHK_ConsultantItem]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="ConsultantItem"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_ConsultantItem').val(ret);
            });

            $('input:checkbox[id*=CHK_HarassPhone]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="HarassPhone"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_HarassPhone').val(ret);
            });

            $('input:checkbox[id*=CHK_IntroductionUnit]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="IntroductionUnit"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_IntroductionUnit').val(ret);
            });

            $('input:checkbox[id*=CHK_CrashIntroduction]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="CrashIntroduction"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_CrashIntroduction').val(ret);
            });
            //特殊諮詢
            $('input:checkbox[id*=CHK_Special]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="Special"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_Special').val(ret);
            });
            //海外地區
            $('input:checkbox[id*=CHK_Overseas]').bind("click", function (e) {
                var ret = '';
                $('input:checkbox:checked[name="Overseas"]').each(function (i) { ret += this.value + ','; });
                $('#HFD_Overseas').val(ret);
            });
            //*****************************************************************************************


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

            //同聯絡人地址用，reload鄉鎮市區
            function ChangeArea2(CityID, Area, NoticeArea) {
                $.ajax({
                    type: 'post',
                    url: "../common/ajax.aspx",
                    data: 'Type=GetAreaList' + '&CityID=' + CityID,
                    success: function (result) {
                        $('select[id$=' + NoticeArea + ']').html(result);
                        $("#ddlArea").val(Area);
                    },
                    error: function () { alert('ajax failed'); }
                })
            }
        })
        
     </script>

      <script type="text/javascript">
          /*-- (判斷日期格式是否正確) -------------------------*/
     
     
     
      
        function isDate(year, month, day){  
           var dateStr;  
           if (!month || !day) {  
               if (month == '') {  
                   dateStr = year + "/1/1"  
               }else if (day == '') {  
                   dateStr = year + '/' + month + '/1';  
               }else {  
                   dateStr = year.replace(/[.-]/g, '/');  
               }  
           }else {  
               dateStr = year + '/' + month + '/' + day;  
           }  
           dateStr = dateStr.replace(/\/0+/g, '/');  
  
           var accDate = new Date(dateStr);  
           var tempDate = accDate.getFullYear() + "/";  
           tempDate += (accDate.getMonth() + 1) + "/";  
           tempDate += accDate.getDate();  
  
           if (dateStr == tempDate) {  
               return true;  
           }  
           return false;
       }

       function selMember() {
           IsValue = window.open("AdvisoryDataQry.aspx",
            "選擇會友", "help=yes,status=yes,resizable=yes,scrollbars=yes,center=yes,width=700px,height=500px");
           return false;
       }

     </script>

     <script type="text/javascript">
         function ClearTB1() {
             //var NowDate = new Date()
             var d = new Date();

             var month = d.getMonth() + 1;
             var day = d.getDate();

             //var output = d.getFullYear() + '/' + (month < 10 ? '0' : '') + month + '/' + (day < 10 ? '0' : '') + day;
             var output = (d.getHours() < 10 ? '0' : '') + d.getHours() + ':' + (d.getMinutes() < 10 ? '0' : '') + d.getMinutes() + ':' + (d.getSeconds() < 10 ? '0' : '') + d.getSeconds();

             document.getElementById("txtEndTime").value = output;

             //return  false;
         }

         function dateDiff(date1, date2) {
             date1 = document.getElementById("txtCreateDate").value + ' ' + document.getElementById("txtCreateTime").value;
             date2 = document.getElementById("txtCreateDate").value + ' ' + document.getElementById("txtEndTime").value;
             
             var type1 = typeof date1, type2 = typeof date2;
             if (type1 == 'string')
                 date1 = stringToTime(date1);
             else if (date1.getTime)
                 date1 = date1.getTime();
             if (type2 == 'string')
                 date2 = stringToTime(date2);
             else if (date2.getTime)
                 date2 = date2.getTime();
             //return (date1 - date2) / 1000; //結果是秒}
              var m =parseInt(((date2 - date1) / 1000) / 60);
             document.getElementById("lblTime").innerHTML ='通話時間共' + m + '分' + ((date2 - date1) / 1000) % 60 + '秒' ;
         }

         function stringToTime(string) {

             var f = string.split(' ', 2);
             var d = (f[0] ? f[0] : '').split('-', 3);
             var t = (f[1] ? f[1] : '').split(':', 3);
             return (new Date(
                parseInt(d[0], 10) || null,
                (parseInt(d[1], 10) || 1) - 1,
                parseInt(d[2], 10) || null,
                parseInt(t[0], 10) || null,
                parseInt(t[1], 10) || null,
                parseInt(t[2], 10) || null
                )).getTime();
         } 
     </script>

    <script type="text/javascript">

        function ChkData() {
            var msg = '';
            var ConsultantMain = document.getElementById('HFD_ConsultantMain').value;
            var ConsultantItem = document.getElementById('HFD_ConsultantItem').value;
            var HarassPhone = document.getElementById('HFD_HarassPhone').value;
            var Overseas = document.getElementById('HFD_Overseas').value
            var Special = document.getElementById('HFD_Special').value;
            var Event = document.getElementById('txtEvent').value;
            var City = document.getElementById('ddlCity').value;
            var ServiceUser = document.getElementById('txtServiceUser').value;
            var CName = document.getElementById('txtCName').value;
            var Sex0 = document.getElementById('rdoSex_0').checked;
            var Sex1 = document.getElementById('rdoSex_1').checked;
            var Sex2 = document.getElementById('rdoSex_2').checked;
            var ChurchYN0 = document.getElementById('rdoChurchYN_0').checked;
            var ChurchYN1 = document.getElementById('rdoChurchYN_1').checked;
            var Christian0 = document.getElementById('rdoChristianYN_0').checked;
            var Christian1 = document.getElementById('rdoChristianYN_1').checked;
            var Marry = document.getElementById('HFD_Marry').value;
            var CreateDate = document.getElementById('txtCreateDate').value;
            var CreateTime = document.getElementById('txtCreateTime').value;
            var EndTime = document.getElementById('txtEndTime').value;

            if (City == '' && Overseas == '') {
                msg = msg + "地址(縣市)為必填 !\n";
            }
            if (HarassPhone == '' && Special=='') {
                if (ConsultantMain == '') {
                    msg = msg + "來電諮詢別(大類)為必填 !\n";
                }
                if (ConsultantItem == '') {
                    msg = msg + "來電諮詢別(分項)為必填 !\n";
                }
            }
            if (Event == '') {
                msg = msg + "事件為必填 !\n";
            }
            if (ServiceUser == '') {
                msg = msg + "志工為必填 !\n";
            }
            if (CName == '') {
                msg = msg + "姓名為必填 !\n";
            }

            if (Sex0 == false && Sex1 == false && Sex2 == false) {
                msg = msg + "性別為必填 !\n";
            }

            if (Christian0 == false && Christian1 == false) {
                msg = msg + "基督徒為必填 !\n";
            }
            if (Marry == '') {
                msg = msg + "婚姻為必填 !\n";
            }
            if (CreateDate == '' || CreateTime == '' || EndTime == '') {
                msg = msg + "建檔日期與時間為必填 !\n";
            }

            if (msg.length > 0) {
                alert(msg);
                
                //self.opener.LoadConsultQry('LoadGird');
               //return msg;
               // document.getElementById("Form1:btnUpdate").click();
               //return false;
                //$('#btnUpdate').click();
               // return false
            }
            else {
                //  document.getElementById("Form1:btnUpdate").click();
                
                return "完成";
               // document.getElementById("Form1:btnUpdate").click();
                //return window.confirm('是否要存檔 ?');
             }
                               
        }

     


        
    </script>
    <script type="text/javascript">


        ///-------以下IE 可以
        $(window).on('beforeunload', function () {
           $('#btnUpdate').trigger('click');
        });
</script>

       


</head>
<body class="body"  >
    <form id="Form1" runat="server"  >
    <input type="hidden" id="RefValue" name="RefValue" runat="server" />
    <asp:HiddenField ID="HFD_UID" runat="server" />
    <asp:HiddenField ID="HFD_Mode" runat="server" />
    <asp:HiddenField ID="HFD_MemberID" runat="server" />
     <asp:HiddenField ID="HFD_Marry" runat="server" />
    <asp:HiddenField ID="HFD_ConsultingUID" runat="server" />
    <asp:HiddenField ID="HFD_ConsultantMain" runat="server" />
    <asp:HiddenField ID="HFD_ConsultantItem" runat="server" />
    <asp:HiddenField ID="HFD_HarassPhone" runat="server" />
    <asp:HiddenField ID="HFD_IntroductionUnit" runat="server" />
    <asp:HiddenField ID="HFD_CrashIntroduction" runat="server" />
    <asp:HiddenField ID="HFD_Area" runat="server" />
    <asp:HiddenField ID="HFD_Special" runat="server" />
    <asp:HiddenField ID="HFD_Overseas" runat="server" />
    <asp:Label runat="server" ID="Toolbar"></asp:Label>
  
    <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        <asp:Literal ID="litTitle" runat="server" Text="修改作業"></asp:Literal>
    </h1>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
        <tr>
            <%--<td width="100%" colspan="10" bgcolor="Pink" align="center">
                    <asp:Button ID="btnUpdateConsulting" runat="server" 
                onclick="btnUpdateConsulting_Click" Text="更新" Visible="False" />
                <B><asp:Label runat="server" ID="lblGreetingTitle" style="color: Red;font-size:16px;"></asp:Label></B>
                <br/>
                <br/>
                <B><asp:Label runat="server" ID="lblGreetingInbound" style="color: Red;font-size:16px;"></asp:Label></B>
                <br/>
                <br/>
                <B><asp:Label runat="server" ID="lblGreetingEnd" style="color: Red;font-size:16px;"></asp:Label></B>
            </td>--%>
        </tr>
        <tr>
            <td width="100%" colspan="10">
                <asp:Literal runat="server" ID="MainMenu" Visible="False"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <fieldset id="Fieldset1" runat="server" style="border-color: Orange">
                    <legend id="Legend4" runat="server" style="color: Red">個案基本資料修改</legend>
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
                        <tr>
                            <th align="center" colspan ="8">
                               【聯絡人資料】
                            </th>
                        </tr>
                      
                        <tr>
                         <th align="center"  rowspan ="4" style="Width:30mm">
                                <span class="necessary">個<br>案<br>基<br>本<br>資<br>料</span>
                            </th> 
                            <th align="left" style="Width:20mm">
                                <span>1.電話：</span>
                            </th>   
                            <td style="Width:50mm">
                                <asp:TextBox runat="server" ID="txtPhone" Width="40mm" ></asp:TextBox>
                                <asp:CheckBox ID="chkFirst" runat="server" Text ="第一次" Visible="False" />
                            </td>
                           
                           
                            <th align="left" style="Width:30mm">
                                4.年齡： 
                            </th>
                            <td>
                                約:<asp:TextBox runat="server" Width="10mm" ID="txtAge" Visible ="false"  ></asp:TextBox>
                                 <asp:DropDownList ID="ddlAge" runat="server">
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
                            <th align="left" style="Width:50mm">     7.轉介教會
                            
                               
                            </th>
                            <td >
                                <asp:RadioButtonList ID="rdoChurchYN" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem>是</asp:ListItem>
                                    <asp:ListItem>否</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th align="left"  style="Width:20mm"><font color="Red" style="font-weight: bold"> 2.姓名：</font>
                               
                            </th>
                            <td>
                               <asp:TextBox runat="server" Width="30mm" ID="txtCName" ></asp:TextBox>
                            </td>
                            <th align="left">
                                <font color="Red" style="font-weight: bold">5.婚姻：</font>
                            </th>
                            <td colspan ="3"  style="width :110mm">
                             <asp:Label ID="lblMarry" runat="server"></asp:Label>
                                <asp:RadioButtonList ID="rdoMarry" runat="server" RepeatDirection="Horizontal" Visible="False">
                                    <asp:ListItem>未婚</asp:ListItem>
                                    <asp:ListItem>已婚</asp:ListItem>
                                    <asp:ListItem>離婚</asp:ListItem>
                                    <asp:ListItem>喪偶</asp:ListItem>
                                    <asp:ListItem>未知</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                         
                        </tr>
                         <tr>
                            <th align="left"  style="Width:20mm">
                               <font color="Red" style="font-weight: bold"> 3.性別：</font>
                            </th>
                            <td>
                                 <asp:RadioButtonList ID="rdoSex" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem>男</asp:ListItem>
                                    <asp:ListItem>女</asp:ListItem>
                                    <asp:ListItem>自訂</asp:ListItem>
                                   
                                </asp:RadioButtonList>
                            </td>
                           
                            <th align="left">
                               <font color="Red" style="font-weight: bold"> 6.基督徒：</font>
                            </th>
                            <td>
                                <asp:RadioButtonList ID="rdoChristianYN" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem>是</asp:ListItem>
                                    <asp:ListItem>否</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                     
                     <tr>
                         <th align="left">
                                
                                  <font color="Red" style="font-weight: bold">8.地址：</font>
                            </th>
                            <td  colspan ="5">
                            縣市：<asp:DropDownList ID="ddlCity" runat="server" ></asp:DropDownList>
                                區鄉鎮：<asp:DropDownList ID="ddlArea" runat="server" ></asp:DropDownList>
                                郵遞區號：<asp:TextBox runat="server" ID="txtZipCode" Width="20mm" MaxLength="5"></asp:TextBox>
                                地址：<asp:TextBox ID="txtAddress" Width="60mm" runat="server"></asp:TextBox>
                                   <asp:Label ID="lblOverseas" runat="server"></asp:Label>
                            </td>
                     
                     </tr>
                     </table>
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
                        
                       <tr><th colspan ="7" align ="center">諮詢記錄</th></tr>
                      <tr>
                            <th align="left"  style="Width:20mm">
                                <font color="Red" style="font-weight: bold">志工：</font>
                            </th>
                            <td colspan ="1">
                               <asp:TextBox runat="server" Width="40mm" ID="txtServiceUser" ></asp:TextBox>
                            </td>
                          
                              <th align="left"  style="Width:30mm">
                                <font color="Red" style="font-weight: bold">建檔時間：</font>
                            </th>
                            <td  colspan ="1">
                            開始日期：<asp:TextBox  runat="server" Width="25mm" ID="txtCreateDate" MaxLength="10"></asp:TextBox>
                               時間：<asp:TextBox runat="server" Width="18mm" ID="txtCreateTime" MaxLength="8" ></asp:TextBox>
                            結束時間：
                              <asp:TextBox runat="server" Width="18mm" ID="txtEndTime" MaxLength="8" ></asp:TextBox>
                                <asp:Button ID="btnEndDate" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                  OnClientClick ="ClearTB1(); return dateDiff();"
                                    Text="產生結束時間"  />  <asp:Label ID="lblTime" runat="server"></asp:Label>
                            </td>
                            </tr>
                        </table>     
                       <!--第二段 -->
                         <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
                        <tr><th colspan ="7" align ="center">求助者的主訴(用第一人稱 "我" 敘述)</th></tr>
                            <tr>
                                <th align="center" rowspan="5" style="width: 23mm;font-size: 26px;">
                                    <span class="necessary">  <font color="Green" style="font-weight: bold">S</font></span>
                                </th>
                                <th align="left" style="width: 30mm">
                                <font color="Red" style="font-weight: bold">A(事件)：</font>
                                    
                                </th>
                                <td colspan ="5" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtEvent" Width="200mm" Rows="8" 
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                             
                             <tr>
                                <th align="left" style="width: 30mm">
                                    <span>B(想法)：</span>
                                </th>
                                <td colspan ="5" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtThink" Width="200mm" ></asp:TextBox>
                                </td>
                            </tr>
                              <tr>
                                <th align="left" style="width: 30mm">
                                    <span>C(感受)：</span>
                                </th>
                                <td colspan ="5" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtFeel" Width="200mm" ></asp:TextBox>
                                </td>
                            </tr>

                              <tr>
                                <th align="left" style="width: 30mm">
                                    <span>補充說明：</span>
                                </th>
                                <td colspan ="5" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtComment" Width="200mm" ></asp:TextBox>
                                </td>
                            </tr>

                              <tr>
                              
                                <td colspan ="6" style="width: 200mm">
                                <font color="" style="font-weight: bold"> 附註:建議可用5W(what/who/when/where/why)來提問，幫助了解困擾案主的問題 </font>
                                </td>
                            </tr>
                             </table>                            
                            <!--第三段 -->
                              <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
                                   <tr><th colspan ="7" align ="center">客觀分析(諮商類別)：必填</th></tr>
                            <tr>
                                <th align="center" rowspan="6" style="width: 30mm;font-size: 26px;">
                                    <span class="necessary"> <font color="Green" style="font-weight: bold">O</font></span>
                                </th>
                                <th align="left" style="width: 45mm">      
                                 <font color="Red" style="font-weight: bold">  來電諮詢別(大類)</font>  
                                  
                                </th>
                                <td colspan ="5" >
                                    <asp:Label ID="lblConsultantMain" runat="server"></asp:Label>
                                </td>
                            </tr>
                             
                             <tr>
                                <th align="left" >
                                  <font color="Red" style="font-weight: bold"> 來電諮詢別(分項)：</font>
                                
                                </th>
                                <td colspan ="5">
                                   <asp:Label ID="lblConsultantItem" runat="server"></asp:Label>
                                </td>
                              </tr>
                                 <tr>
                                <th align="left" >
                                   特殊諮詢：
                                
                                </th>
                                <td colspan ="5">
                                   <asp:Label ID="lblSpecial" runat="server"></asp:Label>
                                </td>
                             </tr>
                              <tr>
                                <th align="left" style="width: 30mm">
                                    騷擾電話
                                </th>
                                <td colspan ="5" >
                                    <asp:Label ID="lblHarassPhone" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtHarassOther" runat="server"></asp:TextBox>
                                </td>
                             </tr>

                              <tr>
                                <th align="left" style="width: 30mm">            轉介單位(告知電話)
                                 
                                </th>
                                <td colspan ="5" >
                                     <asp:Label ID="lblIntroductionUnit" runat="server"></asp:Label>
                                </td>
                            </tr>

                            
                              <tr>
                                <th align="left" style="width: 30mm">            緊急單位(撥打電話)
                                     
                                </th>
                                <td colspan ="5" >
                                     <asp:Label ID="lblCrashIntroduction" runat="server"></asp:Label>
                                </td>
                            </tr>

                            <!--第四段  -->
                              <tr><th colspan ="7" align ="center">問題評估</th></tr>
                            <tr>
                                <th align="center" rowspan="1" style="width: 30mm; font-size: 26px;" >
                                    <span class="necessary"> <font color="Green" style="font-weight: bold">A</font></span>
                                </th>
                                
                                <td colspan ="6" style="width: 180mm">
                                 <%--   1.因&nbsp;<asp:TextBox 
                                        runat="server" ID="txtReason1" Width="60mm"></asp:TextBox>&nbsp;造成
                                    <asp:TextBox runat="server" ID="txtTrouble1" Width="30mm"></asp:TextBox>&nbsp;問題
                                    <br />
                                       2.因&nbsp;<asp:TextBox runat="server" 
                                        ID="txtReason2" Width="60mm"></asp:TextBox>&nbsp;造成
                                    <asp:TextBox runat="server" ID="txtTrouble2" Width="30mm"></asp:TextBox>&nbsp;問題
                                    <br />
                                       3.因&nbsp;<asp:TextBox runat="server" 
                                        ID="txtReason3" Width="60mm"></asp:TextBox>&nbsp;造成
                                    <asp:TextBox runat="server" ID="txtTrouble3" Width="30mm"></asp:TextBox>&nbsp;問題--%>
                                    <asp:TextBox runat="server" ID="txtProblem" Width="200mm"  Rows="3" TextMode="MultiLine"></asp:TextBox>&nbsp;
                                </td>
                            </tr>
                             
                           </table>
                           <!--第五段 -->
                             <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
                                       <tr><th colspan ="7" align ="center">計畫</th></tr>
                            <tr>
                                <th align="center" rowspan="8" style="width: 12mm;font-size: 26px;">
                                    <span class="necessary"> <font color="Green" style="font-weight: bold">P</font></span>
                                </th>
                            </tr>
                            <tr>
                                <th align="left" style="width: 20mm">
                                    1.找出案主心中的假設
                                </th>
                                <td colspan ="1" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtFindAssume" Width="100mm" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                </tr>
                            <tr>
                                <th align="left" style="width: 20mm">
                                    2.與案主討論與解釋假設
                                </th>
                                <td colspan ="1" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtDiscuss" Width="100mm" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                </tr>
                            <tr>
                                <th align="left" style="width: 20mm">
                                    3.加入新的元素
                                </th>
                                <td colspan ="1" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtElement" Width="100mm" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                               <tr>
                                <th align="left" style="width: 20mm">
                                    4.改變案主原先的期待
                                </th>
                                <td colspan ="1" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtExpect" Width="100mm" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>

                               <tr>
                                <th align="left" style="width: 20mm">
                                    5.給予案主祝福與盼望
                                </th>
                                <td colspan ="1" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtBlessing" Width="100mm" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                             
                                <tr>
                                <th align="left" style="width: 20mm">
                                    6.帶領決志禱告
                                </th>
                                <td colspan ="1" style="width: 50mm">
                                    <asp:TextBox runat="server" ID="txtIntroductionChurch" Width="100mm" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                           
                              <tr>
                                <th align="left" style="width: 20mm">
                                    7.我對個案的幫助程度
                                </th>
                                <td colspan ="1" style="width: 50mm">
                                <asp:RadioButtonList ID="rdoHelpLvMark" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem>有改善</asp:ListItem>
                                    <asp:ListItem>仍陷困境</asp:ListItem>
                                </asp:RadioButtonList>

                           </td>
                            </tr>

                            <tr>
                                <th colspan ="2">小叮嚀</th>
                                <td colspan ="5"><asp:TextBox runat="server" ID="txtMemo" Width="100mm"></asp:TextBox></td>
                            </tr>
                           <!-- -->


                    </table>
                  </fieldset>
            </td>
        </tr>
    </table>

        <div class="function">
        <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" 
            Text="新增"  onclick="btnAdd_Click"/>
        <asp:Button ID="btnUpdate" class="npoButton npoButton_Save" runat="server" 
            Text="儲存" onclick="btnUpdate_Click" OnClientClick=" return ChkData();"/>
        <asp:Button ID="btnDelete" class="npoButton npoButton_Del" runat="server" 
            Text="刪除" onclick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');"/>
        <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" 
            Text="離開" onclick="btnExit_Click"/>
        <asp:Button ID="btnReload" runat="server" onclick="btnReload_Click" Text="btnReload" />
    </div>
<div> <asp:Label ID="lblRpt" runat="server" Visible="False"></asp:Label></div>
     
  
    </form>
</body>
</html>
