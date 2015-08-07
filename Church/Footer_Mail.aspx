<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Footer_Mail.aspx.cs" Inherits="sysMgr_Footer_Mail" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Email</title>
    <base target="_self" />
    <link href="../include/main.css" rel="stylesheet" type="text/css" />
    <link href="../include/table.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="../ckeditor/ckeditor.js"></script>
<script type="text/javascript">
    function SelfClose() {
          window.opener = null;
        window.open('', '_self', '');
        window.close();
    }
   
</script>
    <style type="text/css">
        .font9T {}
    </style>
</head>
<body >
    <form id="form2" runat="server"   >
    <asp:HiddenField ID="HFD_ChurchUid" runat="server" />
            <asp:HiddenField ID="HFD_UID" runat="server" />

         <br />
       <table  width="100%" border="0" align="center"   cellpadding="0" cellspacing="1" class="table_v">
        <tr>
           <th  style="width:25%" align="right">標題：</th>
                 <td align="left"   >
                          <asp:TextBox ID="txtSubject" ValidationGroup="sendSysMail" runat="server" Width="99%"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSubject"
                            ErrorMessage="請輸入標題" ValidationGroup="sendSysMail"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
              <th  align="right">Email Address：</th>
                 <td align="left"   >
                         <asp:TextBox ID="txtEmailTo"  runat="server" CssClass="font9T"  Width="99%"></asp:TextBox>
                      </td>
                </tr>
           <tr>
              <th  align="right">上傳檔案：</th>
                 <td align="left"   >
                         <asp:FileUpload ID="FileUPAttachPath" runat="server" />
                      </td>
                </tr>
           <tr>
               <th align="right">
                   內容：
               </th>
               <td align="left">
                   <asp:TextBox ID="txtBody" ValidationGroup="sendSysMail" runat="server" Height="45px"
                       TextMode="MultiLine" Width="99%"></asp:TextBox>
                        <script type="text/javascript">
                            CKEDITOR.replace('txtBody',
                        {
                            skin: 'v2'
                        });
                       </script>
                  <%-- <td width="100%" colspan="1" align="left">
                      
                   </td>--%>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBody"
                       ErrorMessage="請輸入內容" ValidationGroup="sendSysMail"></asp:RequiredFieldValidator>
                   <asp:Literal ID="Literal1" runat="server" Visible="False"></asp:Literal>
               </td>
           </tr>
                <tr>
                     <td align="left" colspan="2" >
                        <asp:Button ID="btnSend" ValidationGroup="sendSysMail" runat="server" Text="送出" OnClick="btnSend_Click"   />
                         <asp:Button ID="btn_addcancel" OnClientClick="SelfClose();" runat="server" Text="關閉" />
                          <asp:Button ID="btn_backcancel" runat="server" Text="回上一頁" OnClick="backcancel_Click" />
                    </td>
                </tr>
            </table>
      </form>
</body>
</html>
