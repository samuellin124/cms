using System;
using System.Net.Mail;
using System.Collections.Generic;

/* 使用方法 Demo
    protected void btnSend_Click(object sender, EventArgs e)
    {
        SendEMailObject mailObj = new SendEMailObject();
        
        mailObj.MailFrom = "just@sino1.com.tw";
        mailObj.MailFromName = "系統問題回報";
        mailObj.MailTo = txtEmailTo.Text;
        mailObj.Subject = txtSubject.Text;
        mailObj.Body = txtBody.Text;

        mailObj.SmtpServer = "210.65.244.3";
        mailObj.SmtpAccount = "just";
        mailObj.SmtpPassword = "Abc123";
        int ErrorCode = mailObj.SendMail();
        
        //若需附加檔案，將檔案路徑及檔名加入List<string>
        List<string> strAttFile = new List<string>();
        strAttFile.Add(@"C:\Users\User\Desktop\head2.jpg");
        strAttFile.Add(@"D:\Test\CCRA_Access.zip");
        objSend.AttachPath = strAttFile;
 
        if (ErrorCode == 0)
        {
            Session["Msg"] = "發送Mail成功!!!";
        }
        else
        {
            Session["Msg"] = "發送Mail失敗 [" + mailObj.ErrorMessage + "]";
        }
        ShowSysMsg();
    }
    //---------------------------------------------------------------------------------------------
*/

/// <summary>
/// 處理寄信功能程式
/// </summary>
public class SendEMailObject
{
    public string MailFrom = "";     //寄信人 mail address
    public string MailFromName = ""; //寄信人的顯示名稱
    public string MailTo = "";       //收信人 mail address, 多個收信人以 ';' 分隔, 可自訂(delimiterChars)
    public string Subject = "";
    public string Body = "";
    public bool IsBodyHtml = true;
    public int SleepTime = 0;        //如果需要的話, 設定此值 (毫秒)
    char[] delimiterChars = { ';' }; 

    public string SmtpServer = "";
    public string SmtpAccount = "";
    public string SmtpPassword = "";
    public int ErrorCode = 0;
    public string ErrorMessage = "";
    public List<string> lstAttachPath = new List<string>();
    //---------------------------------------------------------------------------------------------
    public int SendMail()
    {
        if (MailFrom == "")
        {
            ErrorCode = 10;
            ErrorMessage = "寄信人欄位為必填欄位";
            return ErrorCode;
        }
        if (MailTo == "")
        {
            ErrorCode = 11;
            ErrorMessage = "收信人欄位為必填欄位";
            return ErrorCode;
        }
        if (Subject == "")
        {
            ErrorCode = 12;
            ErrorMessage = "Subject 為必填欄位";
            return ErrorCode;
        }
        if (Body == "")
        {
            ErrorCode = 13;
            ErrorMessage = "Body 為必填欄位";
            return ErrorCode;
        }
        if (SmtpServer == "")
        {
            ErrorCode = 20;
            ErrorMessage = "SMTP Server 為必填欄位";
            return ErrorCode;
        }

        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
        mailMessage.From = new System.Net.Mail.MailAddress(MailFrom, MailFromName);
        mailMessage.Subject = Subject;
        mailMessage.IsBodyHtml = IsBodyHtml;
        mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
        mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
        mailMessage.Body = Body;

        //若有附加檔
        if (lstAttachPath.Count > 0)
        {
            foreach (string AttachPath in lstAttachPath)
            {
                if (!string.IsNullOrEmpty(AttachPath.Trim()))
                {
                    Attachment file = new Attachment(AttachPath.Trim());
                    //加入信件的夾帶檔案
                    mailMessage.Attachments.Add(file);

                }

            }
        }

        //這邊多人可以回圈
        string[] mailToArray = MailTo.Split(delimiterChars);
        foreach (string mail in mailToArray)
        {
            if (mail.Trim() != "")
            {
                mailMessage.To.Add(mail);
            }
        }
        System.Net.Mail.SmtpClient SMTPServer = new System.Net.Mail.SmtpClient(SmtpServer);
        if (SmtpAccount != "")
        {
            SMTPServer.Credentials = new System.Net.NetworkCredential(SmtpAccount, SmtpPassword);
        }
        try
        {
            SMTPServer.Send(mailMessage);
            if (SleepTime != 0)
            {
                System.Threading.Thread.Sleep(2);
            }
            ErrorCode = 0;
            ErrorMessage = "";
            return ErrorCode;
        }
        catch (ArgumentNullException ex)
        {
            ErrorCode = 21;
            ErrorMessage = ex.Message;
            return ErrorCode;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            ErrorCode = 22;
            ErrorMessage = ex.Message;
            return ErrorCode;
        }
        catch (InvalidOperationException ex)
        {
            ErrorCode = 23;
            ErrorMessage = ex.Message;
            return ErrorCode;
        }
        catch (SmtpFailedRecipientsException ex)
        {
            ErrorCode = 24;
            ErrorMessage = ex.Message;
            return ErrorCode;
        }
        catch (SmtpException ex)
        {
            ErrorCode = 25;
            SmtpStatusCode status = ex.StatusCode;

            if (status == SmtpStatusCode.MailboxBusy)
            {
                ErrorMessage = "MailboxBusy";
            }
            else if (status == SmtpStatusCode.MailboxUnavailable)
            {
                ErrorMessage = "MailboxUnavailable";
            }
            else
            {
                ErrorMessage = "Failed to deliver message " + status.ToString();
            }
            return ErrorCode;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            ErrorCode = 100;
            return ErrorCode;
        }
    }

    public List<string> AttachPath { get; set; }
}
