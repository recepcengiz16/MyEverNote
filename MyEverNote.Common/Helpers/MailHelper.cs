using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace MyEverNote.Common.Helpers
{
    public class MailHelper
    {
        //.net ve .netmail using ile ekledim
        public static Boolean SendMail(String body,String to,String subject,Boolean isHtml=true)
        {
            return SendMail(body, new List<String>{ to }, subject, isHtml);
        }

        public static Boolean SendMail(String body,List<String> to,String subject,Boolean isHtml=true)
        {
            Boolean result = false;

            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(ConfigHelper.Get<String>("MailUser"));

                to.ForEach(x =>
                {
                    message.To.Add(new MailAddress(x));
                });

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;

                using (var smtp = new SmtpClient(ConfigHelper.Get<String>("MailHost"), ConfigHelper.Get<int>("MailPort")))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(ConfigHelper.Get<String>("MailUser"), ConfigHelper.Get<String>("MailPass"));

                    smtp.Send(message);
                    result = true;
                };
                
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
    }
}
