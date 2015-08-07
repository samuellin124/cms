<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectMember.aspx.cs" Inherits="CaseMgr_SelectMember" %>

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
        });   //end of ready()
    </script>

    <script type="text/javascript">
    </script>
    <style type="text/css">
        .style1
        {
            height: 24px;
        }
    </style>
</head>
<body class="body">
    <form id="Form1" runat="server">
    <asp:HiddenField ID="HFD_HouseUID" runat="server" />
    <asp:HiddenField ID="HFD_MemberID" runat="server" />
    <asp:HiddenField ID ="HFD_LoadFormData" runat ="server" />
    <asp:HiddenField ID ="HFD_Area" runat="server" />
    <asp:HiddenField ID ="HFD_SQL" runat="server" />
    <asp:HiddenField ID="HFD_ConsultantItem" runat="server" />
    <asp:HiddenField ID="HFD_EditServiceUser" runat="server" />
    <asp:HiddenField ID="HFD_Marry" runat="server" />
    <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        <asp:Literal ID="lblHeaderText" runat="server"></asp:Literal>
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
              <td align ="right" colspan="6">
                 <asp:Button ID="btnQuery" class="npoButton npoButton_Search" runat="server"  
                    Text="查詢" OnClick="btnQuery_Click"/>
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="6">
                <asp:Label ID="lblGridList" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="function">
       <asp:Button ID="btnAdd" class="npoButton npoButton_New" runat="server" Text="確定" OnClick="btnAdd_Click" OnClientClick="return true;" />
  　   <asp:Button ID="btnExit" class="npoButton npoButton_Exit" runat="server" Text="離開" OnClick="btnExit_Click" />
    </div>
    </form>
</body>
</html>
