using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MailKit;
//using System.Net;
//using System.Net.Mail;


using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Microsoft.Extensions.Primitives;

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
            try
            {
                email.From.Add(new MailboxAddress(senderName, "kommongkhun.2002@gmail.com"));
                email.To.Add(new MailboxAddress(receiverName, receiverEmail));

                email.Subject = $"Yinnaxs.co.th Inteview for position {role}";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = htmlTextEmail.interviewBody(senderName, receiverName, date, time)
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

        public bool sendEmail_Accept_Applicant(string senderName, string receiverName, string receiverEmail, string role, DateTime hire_date, bool aceept)
        {

            try
            {
                email.From.Add(new MailboxAddress(senderName, "kommongkhun.2002@gmail.com"));
                email.To.Add(new MailboxAddress(receiverName, receiverEmail));

                email.Subject = $"Interview Announcement for \"Internship {role}\" position";
                
                if (aceept == true)
                {
                    email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = htmlTextEmail.recruitment_HoldOnApplication(senderName, receiverName, role, hire_date)
                    };

                }else if (aceept == false)
                {
                    email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = htmlTextEmail.recruitment_HoldOfApplication(senderName, receiverName, role)
                    };
                }

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

