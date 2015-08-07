<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConsultFirst.aspx.cs" Inherits="CaseMgr_ConsultFirst" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../include/jquery-1.7.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../include/jquery.metadata.min.js"></script>
    <script type="text/javascript" src="../include/jquery.address.js"></script>
    <script type="text/javascript" src="../include/jquery.field.js"></script>
</head>
<body class="body">
    <form id="form1" runat="server">
                 <h1>
        <img src="../images/h1_arr.gif" alt="" width="10" height="10" />
        <asp:Literal ID="litTitle" runat="server" Text="接聽作業"></asp:Literal>
    </h1>
     <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_v">

     <tr>
     <td>
     <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                <asp:Button ID="btnBound" runat="server" Text="來  電" onclick="btnBound_Click" Height="30px" Width="60px" Font-Bold="True" Font-Size="Smaller" Visible="true" />
                
         <asp:Label ID="lblsta" runat="server"></asp:Label>
                
     </td>
     </tr>
        <tr>
          
            <td align ="center"  height="500">
             
                <asp:ImageButton ID="ImgBtnBound" runat="server" ImageUrl="~/images/pickupphone01.png"  Width="450px" onclick="ImgBtnBound_Click" />
            </td>
            </tr>
            </table>
 
                 <asp:HiddenField ID="HFD_Phone" runat="server" />
                 <asp:HiddenField ID="HFD_UID" runat="server" />
 
                  <asp:Button ID="btnSend" runat="server" onclick="btnSend_Click" Text="諮詢作業" Visible="False" />
               
 
    </form>
</body>
</html>
