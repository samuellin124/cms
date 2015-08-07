<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MargerPeopleDetail_Edit.aspx.cs" Inherits="CaseMgr_MargerPeopleDetail_Edit" %>

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

        });   //end of ready()
    </script>
</head>
<body class="body">
    <form id="Form1" runat="server">
    <asp:HiddenField ID="HFD_LandUid" runat="server" />
    <asp:HiddenField ID="HFD_HouseUid" runat="server" />
    <asp:HiddenField ID="HFD_HouseUnitUid" runat="server" />
    <asp:HiddenField ID="HFD_RenterUid" runat="server" />
    <asp:HiddenField ID="HFD_Mode" runat="server" />
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
             備註：
            </th>
             <td>
                <asp:TextBox ID="txtMemo" runat="server"></asp:TextBox>
            </td>
            </tr> 
        <tr>
            <td align="center" colspan ="6">
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
