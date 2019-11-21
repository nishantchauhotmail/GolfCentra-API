using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace GolfCentra.Core.Helper
{
    public class EmailNotification
    {
        /// <summary>
        /// Common Method to Send Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SendMail(dynamic model)
        {
            MailMessage message = new MailMessage();
            try
            {
                string toEmails = model.ToEmails;
                char[] chArray = new char[1] { ',' };
                foreach (string address in toEmails.Split(chArray))
                    message.To.Add(new MailAddress(address));
                message.From = new MailAddress(model.From);

                if (model.CCMail != "" && model.CCMail != null)
                {
                    foreach (string address in model.CCMail.Split(chArray))
                        message.CC.Add(new MailAddress(address));
                }

                message.Subject = model.Subject;
                if (model.Attachment != null)
                    message.Attachments.Add(new Attachment(Path.Combine(HostingEnvironment.MapPath(model.Path), Path.GetFileName(model.Attachment))));
                message.Body = model.Body;
                message.IsBodyHtml = true;
                new SmtpClient()
                {
                    Host = Constants.Smtp.SmtpHost,
                    Port = Constants.Smtp.SmtpPort,
                    UseDefaultCredentials = false,
                    Credentials = ((ICredentialsByHost)new NetworkCredential(Constants.Smtp.SmtpEmail, Constants.Smtp.SmtpPassword)),
                    EnableSsl = Constants.Smtp.SmtpSSL
                }.Send(message);
                message.Attachments.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                message.Attachments.Dispose();
                return false;
            }
        }
    }
}
