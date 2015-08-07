using System;
using System.Net.Mail;
using System.Collections.Generic;

/* �ϥΤ�k Demo
    protected void btnSend_Click(object sender, EventArgs e)
    {
        SendEMailObject mailObj = new SendEMailObject();
        
        mailObj.MailFrom = "just@sino1.com.tw";
        mailObj.MailFromName = "�t�ΰ��D�^��";
        mailObj.MailTo = txtEmailTo.Text;
        mailObj.Subject = txtSubject.Text;
        mailObj.Body = txtBody.Text;

        mailObj.SmtpServer = "210.65.244.3";
        mailObj.SmtpAccount = "just";
        mailObj.SmtpPassword = "Abc123";
        int ErrorCode = mailObj.SendMail();
        
        //�Y�ݪ��[�ɮסA�N�ɮ׸��|���ɦW�[�JList<string>
        List<string> strAttFile = new List<string>();
        strAttFile.Add(@"C:\Users\User\Desktop\head2.jpg");
        strAttFile.Add(@"D:\Test\CCRA_Access.zip");
        objSend.AttachPath = strAttFile;
 
        if (ErrorCode == 0)
        {
            Session["Msg"] = "�o�eMail���\!!!";
        }
        else
        {
            Session["Msg"] = "�o�eMail���� [" + mailObj.ErrorMessage + "]";
        }
        ShowSysMsg();
    }
    //---------------------------------------------------------------------------------------------
*/

/// <summary>
/// �B�z�H�H�\��{��
/// </summary>
public class SendEMailObject
{
    public string MailFrom = "";     //�H�H�H mail address
    public string MailFromName = ""; //�H�H�H����ܦW��
    public string MailTo = "";       //���H�H mail address, �h�Ӧ��H�H�H ';' ���j, �i�ۭq(delimiterChars)
    public string Subject = "";
    public string Body = "";
    public bool IsBodyHtml = true;
    public int SleepTime = 0;        //�p�G�ݭn����, �]�w���� (�@��)
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
            ErrorMessage = "�H�H�H��쬰�������";
            return ErrorCode;
        }
        if (MailTo == "")
        {
            ErrorCode = 11;
            ErrorMessage = "���H�H��쬰�������";
            return ErrorCode;
        }
        if (Subject == "")
        {
            ErrorCode = 12;
            ErrorMessage = "Subject ���������";
            return ErrorCode;
        }
        if (Body == "")
        {
            ErrorCode = 13;
            ErrorMessage = "Body ���������";
            return ErrorCode;
        }
        if (SmtpServer == "")
        {
            ErrorCode = 20;
            ErrorMessage = "SMTP Server ���������";
            return ErrorCode;
        }

        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
        mailMessage.From = new System.Net.Mail.MailAddress(MailFrom, MailFromName);
        mailMessage.Subject = Subject;
        mailMessage.IsBodyHtml = IsBodyHtml;
        mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
        mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
        mailMessage.Body = Body;

        //�Y�����[��
        if (lstAttachPath.Count > 0)
        {
            foreach (string AttachPath in lstAttachPath)
            {
                if (!string.IsNullOrEmpty(AttachPath.Trim()))
                {
                    Attachment file = new Attachment(AttachPath.Trim());
                    //�[�J�H�󪺧��a�ɮ�
                    mailMessage.Attachments.Add(file);

                }

            }
        }

        //�o��h�H�i�H�^��
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
