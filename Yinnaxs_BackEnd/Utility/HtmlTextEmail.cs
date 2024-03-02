using System;
namespace Yinnaxs_BackEnd.Utility
{
	public class HtmlTextEmail
	{ 
		public HtmlTextEmail()
		{
		}

		public string interviewBody(string senderName, string receiverName, DateTime date, string time)
		{
			string message =
				$"<p>Dear Mr/Mrs {receiverName}</p>\n" +
				"<br>\n" +
				"<p>Thank you very much for your interest in working with Yinnaxs Co.th. We are delighted to invite you to an interview with our team.\n" +
				$"<p>We would like to schedule an interview with {receiverName} on {date.ToString("MMMM dd, yyyy")} at {time}</p></p>\n" +
				"<p>Please click the link to interview : https://join.skype.com/rzGdQmu0EGuR</p>\n <br>\n" +
				"<p>If you need more information, please do not hesitate to direct contact me.</p>\n" +
				"<p>Tel : 080-2535-xxx</p>\n" +
				"<br>\n" +
				$"<p>Best regards,</p>\n<p>{senderName}</p>\n" +
				"<p>(Team Recruiter)</p>\n" +
				"<svg width=\"103\" height=\"103\" viewBox=\"0 0 103 103\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">\n" +
				"<rect width=\"103\" height=\"103\" fill=\"url(#pattern0)\"/>\n  <defs>\n  <pattern id=\"pattern0\" patternContentUnits=\"objectBoundingBox\" width=\"1\" height=\"1\">\n" +
				"<use xlink:href=\"#image0_4_633\" transform=\"scale(0.015625)\"/>\n  </pattern>\n " +
				"<image id=\"image0_4_633\" width=\"64\" height=\"64\" xlink:href=\"data:image/png;base64,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\"/>\n" +
				"</defs>\n </svg>\n" +
				"<p>Yinnaxs co.th</p>\n" +
				"<p>1 Chalong Krung 1 Alley, Lat Krabang, Bangkok 10520</p>";
			return message;
        }

		public string recruitment_HoldOfApplication(string senderName, string receiverName, string role)
		{
			string message =
				$"<p>Dear Ms/Mrs {receiverName}"+
				$"<p>Thank you so much for your interest in Yinnaxs.co.th and thank you for taking the time to meet with us to discuss your interest in the {role} position at Yinnaxs.</p>\n" +
				"<p>We’d like to let you know that this is a very competitive position and after careful consideration, our hiring team has decided to “hold off” your application for further consideration as our Team.</p>\n" +
				"<p>We enjoyed meeting you and will keep your resume on file if any opportunities come up that are a better match for your experience and skills.</p>\n" +
				"<p>If you have any questions, please do not hesitate to contact me.</p>\n" +
				"<br>\n" +
				"<p>Tel : 080-2535-xxx</p>\n" +
				"<br>\n " +
				"<p>Best regards,</p>\n" +
				$"<p>{senderName}</p>"+
				"<p>(Team Recruiter)</p>\n" +
				"<br>\n" +
				"<p>Yinnaxs co.th</p>\n" +
				"<p>1 Chalong Krung 1 Alley, Lat Krabang, Bangkok 10520</p>";

			return message;
        }

        public string recruitment_HoldOnApplication(string senderName, string receiverName, string role, DateTime hire_date)
        {
            string message =
                $"<p>Dear Ms/Mrs {receiverName}" +
                $"<p>Thank you so much for your interest in Yinnaxs.co.th and thank you for taking the time to meet with us to discuss your interest in the {role} position at Yinnaxs.</p>\n" +
                "<p>We’d like to let you know that this is a very competitive position and after careful consideration, our hiring team has decided to “hold On” your application for further consideration as our Team.</p>\n" +
                $"<p>We enjoyed to work with you. We appointment to sign an employment contract at our office on {hire_date}</p>\n" +
                "<p>If you have any questions, please do not hesitate to contact me.</p>\n" +
                "<br>\n" +
                "<p>Tel : 080-2535-xxx</p>\n" +
                "<br>\n " +
                "<p>Best regards,</p>\n" +
                $"<p>{senderName}</p>" +
                "<p>(Team Recruiter)</p>\n" +
                "<br>\n" +
                "<p>Yinnaxs co.th</p>\n" +
                "<p>1 Chalong Krung 1 Alley, Lat Krabang, Bangkok 10520</p>";
            return message;
        }


    }
}

