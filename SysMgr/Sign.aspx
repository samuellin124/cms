<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sign.aspx.cs" Inherits="SysMgr_Sign" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.address.js"></script>
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

        });  //end of ready()

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
    </script>

</head>
<body class="body">
    <form id="Form1" runat="server">
        <asp:HiddenField ID="HFD_HouseUID" runat="server" />
        <asp:HiddenField ID="HFD_UID" runat="server" />
        <asp:HiddenField ID="HFD_Area" runat="server" />
        <asp:HiddenField ID="HFD_ChurchUid" runat="server" />
        <asp:HiddenField ID="HFD_ActUid" runat="server" />
        <asp:HiddenField ID="HFD_UserUid" runat="server" />

        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            <asp:Literal ID="lblHeaderText" runat="server"></asp:Literal>
        </h1>
     <%--   <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th align="right" width="5%">查詢地址：
                </th>
                <td colspan="1" align="left" width="15%" >縣市：<asp:DropDownList ID="ddlCity" runat="server" Height="25px"  ></asp:DropDownList>
                    區鄉鎮：<asp:DropDownList ID="ddlArea" runat="server"></asp:DropDownList>
                </td>
                <th align="right" width="5%">地址：
                </th>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                </td>
                <th align="right"  width="5%">教會：
                </th>
                <td>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right">聯絡窗口：
                </th>
                <td colspan="1">
                    <asp:TextBox ID="txtContact" runat="server"  ></asp:TextBox>
                </td>
                <th align="right"  width="5%">主任牧師：
                </th>
                <td colspan="1">
                    <asp:TextBox ID="txtpriest" runat="server"></asp:TextBox>
                </td>

                <td align="right" colspan="1">
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server"
                        Text="查詢" OnClick="btnQuery_Click" />
                </td>
                <td align="left" colspan="1">
                    <asp:Button ID="btnChange" class="npoButton npoButton_Submit" runat="server"
                        Text="轉介單" OnClick="btnChange_Click" />
                      <asp:Button ID="btnEmail" class="npoButton npoButton_Submit" runat="server"
                        Text="Email" OnClick="btnEmail_Click" />       
                  
                </td>
            </tr>
            <tr>
                <td  colspan="6">
                    
                </td>
            </tr>
        </table>--%>  
        <style>
            .行距
            {
                line-height: 105%;
            }
			
			.table_v TD {
				line-height: 28px;
			}
			
        </style>
        <asp:Label ID="lblGridList" runat="server"></asp:Label>
        <table width="100%" border="0" align="center" cellpadding="1" cellspacing="0" class="table_v">
            <tr>
                <td width="100%" align ="center" style="line-height:105%" >
                    <asp:GridView ID="gdvDonorQry" runat="server"  
                        OnRowCommand="gdvDonorQry_RowCommand" AutoGenerateColumns="False" 
                        BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="2px" 
                        CellPadding="1" GridLines="Horizontal" Width="100%"
                       >
                        <AlternatingRowStyle BackColor="#F7F7F7" />
                        <Columns>
                            
                            <asp:ButtonField ButtonType="Button" CommandName="btnSelect" HeaderText="選取"  Text="選取" DataTextFormatString="uid" Visible="False" />
                            <asp:BoundField DataField="類別" HeaderText="類別">
                            <ItemStyle HorizontalAlign="Center" Wrap="false" Width="20%" CssClass="行距" />
                            </asp:BoundField>
                            <asp:BoundField DataField="項目" HeaderText="項目">
                            <ItemStyle HorizontalAlign="left" Wrap="true" Width="60%" />
                            </asp:BoundField>

                            <asp:BoundField DataField="結束時間" HeaderText="結束時間" HtmlEncode="False" >
                            <ItemStyle HorizontalAlign="Center" Wrap="false"  Width="10%" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="參加">
                                <ItemStyle HorizontalAlign="left" Wrap="false" Width="10%" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server"  
                                        Checked='<%# Eval("selected") %>' />
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                          
                            <asp:BoundField DataField="uid" SortExpression="uid" ShowHeader="False">
                            <ControlStyle Width="0px" />
                            <FooterStyle Width="1px" />
                            <HeaderStyle Width="1px" />
                            <ItemStyle Width="2px" ForeColor="White" Font-Size="1px" />
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
        <div class="function">
            <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" 
                Text="確定" OnClick="btnAdd_Click" OnClientClick="return true;" Visible="False" />
            <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" 
                Text="離開" OnClick="btnExit_Click" Visible="False" />
              <asp:Button ID="btnGridView" runat="server" onclick="btnGridView_Click" 
                        Text="GridView" Visible="False" />
                    <asp:Button ID="btnGvSelect" class="npoButton npoButton_Save" runat="server" onclick="btnGvSelect_Click" 
                        Text="儲存" />
        </div>
    </form>
</body>
</html>
