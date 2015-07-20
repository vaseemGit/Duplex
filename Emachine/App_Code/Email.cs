using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;


/// <summary>
/// Summary description for Email
/// </summary>
public class Email
{
    public static string userName = ConfigurationManager.AppSettings["userName"].ToString();
    public static string password = ConfigurationManager.AppSettings["password"].ToString();
    public static string mailFrom = ConfigurationManager.AppSettings["mailFrom"].ToString();
    public static string mailTo = ConfigurationManager.AppSettings["mailTo"].ToString();
    public static string bccAddress = ConfigurationManager.AppSettings["bccAddress"].ToString();
    public static string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
    public static Int32 Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
    #region Method : SendEmail by Om shankar Trivedi
    public static void SendEmail(string message, string subject)
    {
        MailMessage msg = new MailMessage(mailFrom, mailTo, subject, message);
        msg.IsBodyHtml = true;
        SetUserCredentialAndProcessMail(msg);
    }
    #endregion
    #region Method SetUserCredentialAndProcessMail
    private static void SetUserCredentialAndProcessMail(MailMessage msg)
    {
        NetworkCredential cred = new NetworkCredential(userName, password);
        SmtpClient mailClient = new SmtpClient(smtpServer);
        mailClient.EnableSsl = true;
        mailClient.Port = Port;
        mailClient.UseDefaultCredentials = false;
        mailClient.Credentials = cred;
        try
        {
            MailMessage mailMessage = new MailMessage(mailFrom, msg.To.ToString(), msg.Subject, msg.Body);
            mailMessage.Bcc.Add(bccAddress);
            mailMessage.IsBodyHtml = msg.IsBodyHtml;
            if (msg.Attachments != null && msg.Attachments.Count > 0)
            {
                AttachmentCollection attachment = msg.Attachments;
                foreach (Attachment item in attachment)
                {
                    mailMessage.Attachments.Add(item);
                }
            }
            mailClient.Send(mailMessage);
        }
        catch (Exception)
        {

        }
    }
    #endregion

}