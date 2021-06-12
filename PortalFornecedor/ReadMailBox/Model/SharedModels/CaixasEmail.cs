using System.Collections.Generic;

namespace ReadMailBox.Model.SharedModels
{
    public class CaixasEmail
    {
        public List<MailConfig> MailBoxes { get; set; }
        public SMTPConfig SmtpConfig { get; set; }

        public CaixasEmail()
        {
            MailBoxes = new List<MailConfig>();
        }
    }
}
