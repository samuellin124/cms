<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InBound.aspx.cs" Inherits="CaseMgr_InBound" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

 <title></title>

 <script type="text/javascript">
     function closewin() {
         window.open('', '_self', ''); window.close();
     }
</script>

</head>
<body>
    <form id="form1" runat="server">
   
     <div align="center">
   
       <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr>
     
                <td  align ="center"  valign="top" >
                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                   <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/images/電話響.gif" Height="143px" Width="131px"  />--%>
                       &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" 
                        ImageUrl="~/images/電話響.gif" OnClientClick="closewin();" Height="533px" 
                        Width="795px" />
                
                 
                </td>
            </tr>
        </table>

    
    </div>
    </form>
</body>
</html>
