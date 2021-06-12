namespace ReadMailBox.Model.SharedModels
{
    public class SMTPConfig
    {
        public string SmtpHost { get; set; }
        public int SmtpPorta { get; set; }
        public bool SmtpSSL { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public bool SmtpUseDefaultCredentials { get; set; }
    }
}
