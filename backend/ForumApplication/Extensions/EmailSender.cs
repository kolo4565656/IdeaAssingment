using MailKit.Net.Smtp;
using MimeKit;

namespace ForumApplication.Extensions
{
    public class EmailSender
    {
        public string Subject { get; set; }
        public string EmailReceiver { get; set; }
        public EmailSender(string subject, string emailReceiver) { 
            Subject = subject;
            EmailReceiver = emailReceiver;
        }
        public void Send(string content) {
            var senderUsername = "greenwichwebassignment@gmail.com";
            var senderPassword = "pcbvvdgskelkfarc";
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress("Admin", senderUsername);
            message.From.Add(from);
            MailboxAddress to = new MailboxAddress("User", EmailReceiver);
            message.To.Add(to);
            message.Subject = Subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = content;
            message.Body = bodyBuilder.ToMessageBody();
            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(senderUsername, senderPassword);
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}
