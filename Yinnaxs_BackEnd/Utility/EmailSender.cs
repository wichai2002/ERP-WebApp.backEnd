using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MailKit;
//using System.Net;
//using System.Net.Mail;


using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Yinnaxs_BackEnd.Utility
{
    public class EmailSender
    {
        private MimeMessage email;
        private HtmlTextEmail htmlTextEmail;
        private int port = 587;
        private string hostmail = "smtp.gmail.com";

        public EmailSender()
        {
            email = new MimeMessage();
            htmlTextEmail = new HtmlTextEmail();
        }

        public bool sendEmail_Invite_Interview_To_Applicant(string senderName, string receiverName, string receiverEmail, string role, DateTime date, string time)
        {
            receiverEmail = "64070230@kmitl.ac.th";

            try
            {
                email.From.Add(new MailboxAddress(senderName, "kommongkhun.2002@gmail.com"));
                email.To.Add(new MailboxAddress(receiverName, receiverEmail));

                email.Subject = $"Yinnaxs.co.th Inteview for position {role}";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = htmlTextEmail.intervieBody(senderName, receiverName, date, time)
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.Auto);
                    smtp.Authenticate("kommongkhun.2002@gmail.com", "oitg rlvz jtzc dipq");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                };

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}

//namespace Yinnaxs_BackEnd.Utility
//{
//    public class EmailSender

//    {
//        //private MimeMessage email;
//        private MailMessage email;
//        private HtmlTextEmail htmlTextEmail;

//        public EmailSender()
//        {
//            htmlTextEmail = new HtmlTextEmail();
//        }

//        public bool sendEmail_Invite_Interview_To_Applicant(string senderName, string receiverName, string receiverEmail, string role, DateTime date, string time)
//        {
//            try
//            {
//                receiverEmail = "kommongkhun.2002@gmail.com";
//                email = new MailMessage
//                {
//                    From = new MailAddress("kommongkhun.2002@gmail.com"),
//                    Subject = $"Yinnaxs.co.th Inteview for position {role}",
//                    Body = htmlTextEmail.intervieBody(senderName, receiverName, date, time),
//                    IsBodyHtml = true,
//                };

//                email.To.Add(receiverEmail);

//                //email.From.Add(new MailboxAddress(senderName, "kommongkhun.2002@gmail.com"));
//                //email.To.Add(new MailboxAddress(receiverName, receiverEmail));

//                //email.Subject = $"Yinnaxs.co.th Inteview for position {role}";
//                //email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
//                //{
//                //    Text = htmlTextEmail.intervieBody(senderName, receiverName, date, time)
//                //};

//                var smpt = new SmtpClient("smtp.gmail.com")
//                {
//                    Port = 587,
//                    Credentials = new NetworkCredential("kommongkhun.2002@gmail.com", "123456789echo"),
//                    EnableSsl = true,
//                };

//                smpt.Send(email);

//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }

//        }
//    }
//}