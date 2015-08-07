<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="ExamRecordEdit.aspx.cs"
    Inherits="CaseMgr_ExamRecordEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>志願服務紀錄</title>
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <link rel="stylesheet" type="text/css" href="../include/calendar-win2k-cold-1.css" />
    <script type="text/javascript" src="../include/calendar.js"></script>
    <script type="text/javascript" src="../include/calendar-big5.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.swapimage.min.js"></script>
    <script type="text/javascript">

        // menu控制
        $(document).ready(function () {

            InitMenu();

            //初始化日曆
            //initCalendar(['ServDate']);

        });

        //新增時不要出現的物件在此控制
        var Mode = $('#HFD_Mode').val();
        if (Mode == 'ADD') {
            $('.HideWhenAdd').hide()
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
            Calendar.setup({

                inputField: "txtFillData",   // id of the input field
                button: "imgFillData"     // 與觸發動作的物件ID相同
            });
            Calendar.showYearsCombo(true);
        }

        function btnSum_Click() {
            btnSum_Click()
         return false;
        }
    </script>
    <style type="text/css">
        .style1 {
            height: 28px;
        }

        .auto-style1 {
            width: 1%;
        }

        .auto-style2 {
            width: 6%;
        }

        .auto-style3 {
            width: 1%;
            height: 103px;
        }

        .auto-style4 {
            height: 103px;
        }
    </style>
</head>
<body class="body">
    <form id="Form1" runat="server" submitdisabledcontrols="False">
        <input type="hidden" id="RefValue" name="RefValue" runat="server" />
        <asp:HiddenField ID="HFD_UID" runat="server" />
        <asp:HiddenField ID="HFD_Mode" runat="server" />
        <asp:HiddenField ID="HFD_AdminUserUID" runat="server" />
        <asp:Label runat="server" ID="Toolbar"></asp:Label>
        <h1>
            <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
            好消息熱線中心在線服務評核表</h1>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
            <tr>
                <td colspan="4" align="right"><asp:Button ID="btnPrint" class="npoButton npoButton_Print" runat="server" Text="列印" OnClick="btnPrint_Click" /></td>
            </tr>
            <tr>
                <th align="right" width="10%">評核期間：
                </th>
                <td align="left">
                    <asp:TextBox ID="txtBegCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgBegCreateDate" alt="" src="../images/date.gif" />
                    <asp:TextBox ID="txtEndCreateDate" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgEndCreateDate" alt="" src="../images/date.gif" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="" value=""></asp:Label>
                </td>
                <th align="right" width="10%">填寫日期：
                </th>
                <td align="left">
                    <asp:TextBox ID="txtFillData" CssClass="font9" Width="28mm" runat="server" onchange="CheckDateFormat(this, '日期');"></asp:TextBox>
                    <img id="imgFillData" alt="" src="../images/date.gif" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">
                        <tr>
                            <th align="center">志工編號
                            </th>
                            <th align="center" colspan="2">志工姓名</th>
                            <th align="center" colspan="2">到職日期
                            </th>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox runat="server" ID="txtUserID" Width="40mm" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td align="center" width="5%" colspan="2">
                                <asp:TextBox runat="server" ID="txtUserName" Width="40mm" ReadOnly="True"></asp:TextBox>
                                <asp:DropDownList ID="ddlUserName" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td align="center" width="5%" colspan="2">
                                <asp:TextBox runat="server" ID="txtStartData" Width="40mm" ReadOnly="True"></asp:TextBox>
                   
                               <%-- <img id="imgStartData" alt="" src="../images/date.gif" />--%>
                            </td>
                        </tr>
                        <%--一、出勤狀況--%>
                        <tr>
                            <th align="left" style="width: 40mm">一、出勤狀況</th>
                        </tr>
                        <tr>
                            <th align="center" class="auto-style1">評核項目</th>
                            <th align="center" style="width: 40mm">說明</th>
                            <th align="center" style="width: 40mm" colspan="1">評核標準</th>
                            <th align="center" style="width: 40mm" colspan="1">輔導員評核</th>
                        </tr>
                        <tr>
                            <%--A：出勤狀況--%>
                            <td align="center" class="auto-style1">A：出勤狀況</td>
                            <td align="left" class="auto-style2">是否出勤狀況良好，無遲到早退或偶而遲到早退或經常遲到早退或其他如：未到班亦未事先通知等</td>
                            <td align="center" width="5%">
                                <asp:DropDownList ID="ddlAttendance" runat="server">
                                    <asp:ListItem Value="5">優21-25</asp:ListItem>
                                    <asp:ListItem Value="4">佳16-20</asp:ListItem>
                                    <asp:ListItem Value="3">良11-15</asp:ListItem>
                                    <asp:ListItem Value="2">可6-10</asp:ListItem>
                                    <asp:ListItem Value="1">待改善0-5</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="center" width="5%" colspan="2">
                                <asp:TextBox runat="server" ID="txtAttendance" Width="100mm" Rows="5"
                                    TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                            </td>
                        </tr>
                        <%--二、服務評核--%>
                        <tr>
                            <th align="left" style="width: 40mm">二、服務評核</th>
                        </tr>
                        <tr>
                            <th align="center" class="auto-style1">評核項目</th>
                            <th align="center" style="width: 40mm">說明</th>
                            <th align="center" style="width: 40mm" colspan="1">評核標準</th>
                            <th align="center" style="width: 40mm" colspan="1">輔導員評核</th>
                        </tr>
                        <tr>
                            <%--B：專業助人能力--%>
                            <td align="center" class="auto-style1">B：專業助人能力</td>
                            <td align="left" class="auto-style2">具備專業助人者的特質、態度與個人哲學，及具備回應不同類型的個案語言與非語言諮商技巧能力</td>
                            <td align="center" width="5%">
                                <asp:DropDownList ID="ddlProfessional" runat="server">
                                    <asp:ListItem Value="5">優21-25</asp:ListItem>
                                    <asp:ListItem Value="4">佳16-20</asp:ListItem>
                                    <asp:ListItem Value="3">良11-15</asp:ListItem>
                                    <asp:ListItem Value="2">可6-10</asp:ListItem>
                                    <asp:ListItem Value="1">待改善0-5</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="center" width="5%" colspan="2">
                                <asp:TextBox runat="server" ID="txtProfessional" Width="100mm" Rows="5"
                                    TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%-- C：服務與學習意願--%>
                            <td align="center" class="auto-style1">C：服務與學習意願</td>
                            <td align="left" class="auto-style2">具備旺盛的企圖心與成長動機，積極充實自我，並積極參與熱線中心內部各項訓練課程</td>
                            <td align="center" width="5%">
                                <asp:DropDownList ID="ddlService" runat="server">
                                    <asp:ListItem Value="5">優9-10</asp:ListItem>
                                    <asp:ListItem Value="4">佳7-8</asp:ListItem>
                                    <asp:ListItem Value="3">良5-6</asp:ListItem>
                                    <asp:ListItem Value="2">可3-4</asp:ListItem>
                                    <asp:ListItem Value="1">待改善1-2</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="center" width="5%" colspan="2">
                                <asp:TextBox runat="server" ID="txtService" Width="100mm" Rows="5"
                                    TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%-- D：專業助人倫理--%>
                            <td align="center" class="auto-style1">D：專業助人倫理</td>
                            <td align="left" class="auto-style2">包括具備1.助人專業人員的哲學、價值觀、專業倫理意識及專業技術與利弊得失的判斷能力；2.能確實遵行熱線中心
                                的倫理規範與工作規定；3.尊重被服務者當事人的福祉與權益，能有效發展與當事人、及其他志工間的互關係
                            </td>
                            <td align="center" width="5%">
                                <asp:DropDownList ID="ddlEthics" runat="server">
                                    <asp:ListItem Value="5">優9-10</asp:ListItem>
                                    <asp:ListItem Value="4">佳7-8</asp:ListItem>
                                    <asp:ListItem Value="3">良5-6</asp:ListItem>
                                    <asp:ListItem Value="2">可3-4</asp:ListItem>
                                    <asp:ListItem Value="1">待改善1-2</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="center" width="5%" colspan="2">
                                <asp:TextBox runat="server" ID="txtEthics" Width="100mm" Rows="5"
                                    TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%-- E：危機處理能力--%>
                            <td align="center" class="auto-style1">E：危機處理能力</td>
                            <td align="left" class="auto-style2">面對危機時，能有承擔力及應變能力，並於平時即有預備，讓危機發生時的風享能降到最低。
                            </td>
                            <td align="center" width="5%">
                                <asp:DropDownList ID="ddlCrisis" runat="server">
                                    <asp:ListItem Value="5">優9-10</asp:ListItem>
                                    <asp:ListItem Value="4">佳7-8</asp:ListItem>
                                    <asp:ListItem Value="3">良5-6</asp:ListItem>
                                    <asp:ListItem Value="2">可3-4</asp:ListItem>
                                    <asp:ListItem Value="1">待改善1-2</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="center" width="5%" colspan="2">
                                <asp:TextBox runat="server" ID="txtCrisis" Width="100mm" Rows="5"
                                    TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%-- F：團隊合作--%>
                            <td align="center" class="auto-style1">F：團隊合作</td>
                            <td align="left" class="auto-style2">能與中心內所有志工合作並主動提出協助，能更善用發揮團隊效益之工具與方法，
                                願意分享資源、資訊和專業知識，團隊利益優先並追求團隊與個人之雙贏。
                            </td>
                            <td align="center" width="5%">
                                <asp:DropDownList ID="ddlTeam" runat="server">
                                    <asp:ListItem Value="5">優9-10</asp:ListItem>
                                    <asp:ListItem Value="4">佳7-8</asp:ListItem>
                                    <asp:ListItem Value="3">良5-6</asp:ListItem>
                                    <asp:ListItem Value="2">可3-4</asp:ListItem>
                                    <asp:ListItem Value="1">待改善1-2</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="center" width="5%" colspan="2">
                                <asp:TextBox runat="server" ID="txtTeam" Width="100mm" Rows="5"
                                    TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%-- G：資源運用--%>
                            <td align="center" class="auto-style3">G：資源運用</td>
                            <td align="left" class="auto-style2">能確定熱線中心所需之核心資源、並有效引進與運用核心資源，幫助熱線中心在組織/制度/專業/系統
                                及對教會關係發展與政府引介關係上，有正向之助益。
                            </td>
                            <td align="center" width="5%" class="auto-style4">
                                <asp:DropDownList ID="ddlResources" runat="server">
                                    <asp:ListItem Value="5">優9-10</asp:ListItem>
                                    <asp:ListItem Value="4">佳7-8</asp:ListItem>
                                    <asp:ListItem Value="3">良5-6</asp:ListItem>
                                    <asp:ListItem Value="2">可3-4</asp:ListItem>
                                    <asp:ListItem Value="1">待改善1-2</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="center" width="5%" colspan="2" class="auto-style4">
                                <asp:TextBox runat="server" ID="txtResources" Width="100mm" Rows="5"
                                    TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                            </td>
                        </tr>
                        <%--三、特殊事項與總評--%>
                        <tr>
                            <th align="left">三、特殊事項與總評</th>
                        </tr>
                        <tr>
                            <%-- H：其他事項(特別卓越或足以影響績效者)--%>
                            <td align="center" class="auto-style1" rowspan="3">H：其他事項(特別卓越或足以影響績效者)</td>
                        </tr>
                        <tr>
                            <th align="center" class="auto-style1" colspan="2">實際表現說明</th>
                            <th align="left" class="auto-style1">評核分數
                            </th>
                        </tr>
                        <tr>
                            <td align="center" width="5%" colspan="2">
                                <asp:TextBox runat="server" ID="txtOther" Width="100mm" Rows="5"
                                    TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                            </td>
                            <td align="center" width="5%">
                                <asp:TextBox runat="server" ID="txtOtherSore" Width="100mm" Rows="5"
                                    TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                            </td>
                            <%--評核總分--%>
                            <tr>
                                <th align="center">評核總分</th>
                            </tr>
                            <td align="left" width="30%">
                                <asp:Button ID="btnSum" runat="server" Text="計算" OnClientClick="return btnSum_Click();" OnClick="btnSum_Click"  />
                                <asp:Label ID="lblSum" runat="server" Text=""></asp:Label>
                            </td>
                            <%--評核等第--%>
                            <tr>
                                <th align="center">評核等第</th>

                                <td class="auto-style6" colspan="3">
                                    <asp:RadioButtonList ID="RBL_Rating" runat="server">
                                         <asp:ListItem Value="A+">A+級(90分以上)：優秀：服務表現遠超過接線要求，並有突出之成果</asp:ListItem>
                                         <asp:ListItem Value="A">A級(80~89分以上)：勝任：經常有超越接線要求之服務表現</asp:ListItem>
                                         <asp:ListItem Value="B">B級(70~79分以上)：標準：符合接線要求</asp:ListItem>
                                         <asp:ListItem Value="C">C級(60~69分以上)：勉強：表現僅能符合接線之最低要求</asp:ListItem>
                                         <asp:ListItem Value="D">D級(60分以下)：不適任：無法或偶爾符合接線要求</asp:ListItem>
                                    </asp:RadioButtonList>
                                      </td>
                            </tr>


                            <%--輔導員總評--%>
                            <tr>
                                <th align="left" style="width: 40mm" rowspan="3">輔導員總評</th>
                            </tr>
                            <tr>
                                <td align="center" width="5%" colspan="3">
                                    <asp:TextBox runat="server" ID="txtCounselor" Width="200mm" Rows="5"
                                        TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 40mm"></td>

                                <td align="center" style="width: 40mm" colspan="2">輔導員簽名:　　　　　　　</td>
                            </tr>



                            <%--主任總評--%>
                            <tr>
                                <th align="left" style="width: 40mm" rowspan="3">主任總評</th>
                            </tr>
                            <tr>
                                <td align="center" width="5%" colspan="3">
                                    <asp:TextBox runat="server" ID="txtDirector" Width="200mm" Rows="5"
                                        TextMode="MultiLine" MaxLength="50000"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 40mm"></td>

                                <td align="center" style="width: 40mm" colspan="2">主任簽名:　　　　　　　</td>
                            </tr>
                    </table>
                </td>
            </tr>
        </table>
        
        <div class="function">
            <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="新增"
                OnClick="btnAdd_Click" />
            <asp:Button ID="btnUpdate" class="npoButton npoButton_Save" runat="server" Text="儲存"
                OnClick="btnUpdate_Click" />
            <%--<asp:Button ID="btnDelete" class="npoButton npoButton_Del" runat="server" Text="刪除"
                OnClick="btnDelete_Click" OnClientClick="return window.confirm ('您是否確定要刪除 ?');" />--%>
            <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" Text="離開"
                OnClick="btnExit_Click" />
        </div>
    </form>
</body>
</html>
