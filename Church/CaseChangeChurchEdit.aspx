<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CaseChangeChurchEdit.aspx.cs" Inherits="Church_CaseChangeChurchEdit" %>

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
     <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
        });   //end of ready()

        window.onload = initCalendar;
        function initCalendar() {
            Calendar.setup({
                inputField: "txt_changeChurchDate",   // id of the input field
                button: "imgchangeChurchDate"     // 與觸發動作的物件ID相同

            });
            Calendar.showYearsCombo(true);
        }
    </script>

    <style type="text/css">
        .auto-style1 {
            width: 212px;
        }

        .auto-style2 {
            width: 212px;
            height: 33px;
        }

        .auto-style3 {
            height: 33px;
        }
    </style>

</head>
<body class="body">
    <form id="Form1" runat="server">
        <asp:HiddenField ID="HFD_HouseUID" runat="server" />
        <asp:HiddenField ID="HFD_UID" runat="server" />
        <asp:HiddenField ID="HFD_ChurchUid" runat="server" />

        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            <asp:Literal ID="lblHeaderText" runat="server"></asp:Literal>
        </h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <th align="left" class="auto-style2">個案姓名：
                </th>
                <td class="auto-style3">
                    <asp:TextBox ID="txtCName" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                </td>
                <th align="left" class="auto-style2">轉介日期：
                </th>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_changeChurchDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgchangeChurchDate" alt="" src="../images/date.gif" />
                </td>
            </tr>
            <tr>
                <th align="left" class="auto-style1">A、問題摘要：
                </th>
                <td>
                    <asp:TextBox ID="txtProblem" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <th align="left" class="auto-style1">教會名稱：
                </th>
                <td>
                    <asp:TextBox ID="txtChurch" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                    <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server"
                        Text="查詢教會" OnClick="btnQueryChurch_Click" Width="147px" />
                    <asp:Button ID="btnQuery0" class="npoButton npoButton_New" runat="server"
                        Text="清除教會" OnClick="btnClear_Click" Width="147px" />
                </td>
            </tr>
            <tr>
                <th align="left" class="auto-style1">關懷簡述：
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtBrief" runat="server" TextMode="MultiLine" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="left" class="auto-style1" rowspan="2">B、後續跟進：
                </th>
                <td colspan="3">
                    <asp:CheckBox ID="CKB_FollowUp1" runat="server" Text="受洗" />
                    <asp:CheckBox ID="CKB_FollowUp2" runat="server" Text="穩定聚會(有參加主日或其他小組聚會)" />
                    <asp:CheckBox ID="CKB_FollowUp3" runat="server" Text="偶爾參加聚會" />
                    <asp:CheckBox ID="CKB_FollowUp4" runat="server" Text="不肯進入教會，原因 : " OnCheckedChanged="CKB_FollowUp4_CheckedChanged" />
                    <asp:TextBox ID="txt_FollowUp4" runat="server" Rows="8" TextMode="MultiLine" Width="200mm" MaxLength="1000"></asp:TextBox>
            </tr>
            <tr>
                </td>
                    <td colspan="3">
                        <asp:CheckBox ID="CKB_FollowUp5" runat="server" Text="關懷中 : " OnCheckedChanged="CKB_FollowUp5_CheckedChanged" />
                        <asp:TextBox ID="txt_FollowUp5" runat="server" Width="300px" 
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <th align="left" class="auto-style1">C、關懷方式：
                </th>
                <td colspan="3">
                    <asp:CheckBox ID="CKB_Caring1" runat="server" Text="進入教會的關懷系統(小組/團契等)" />
                    <asp:CheckBox ID="CKB_Caring2" runat="server" Text="繼續保持聯繫(電話或探訪)" />
                    <asp:CheckBox ID="CKB_Caring3" runat="server" Text="曾經連繫過但目前中斷，原因 : " />
                    <asp:CheckBox ID="CKB_Caring4" runat="server" Text="其他 : "  Visible="False" />
                    <asp:TextBox ID="txt_Caring4" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="left" class="auto-style1">D、建議：
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtProposal" runat="server" Rows="8" TextMode="MultiLine" Width="200mm"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="100%" colspan="6">
                    <asp:Label ID="lblGridList" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <div class="function">
            <asp:Button ID="btnsave" class="npoButton npoButton_New" runat="server" Text="存檔" OnClick="btnsave_Click" OnClientClick="return true;" />
            <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" Text="離開" OnClick="btnExit_Click" />
                        <asp:Button ID="btnDelete" class="npoButton npoButton_Del" runat="server" Text="刪除"
                OnClick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');" />
        </div>
    </form>
</body>
</html>
