using static Shared.Enums.Enums;

namespace ReadMailBox.Model.SharedModels
{
    public class MailConfig
    {
        public string MailType { get; set; }
        public string MailHost { get; set; }
        public int MailPorta { get; set; }
        public string MailBox { get; set; }
        public bool MailSSL { get; set; }
        public string MailUser { get; set; }
        public string MailPassword { get; set; }
        public string UserDefaultCredentials { get; set; }
        public TipoCaixaEmail TipoCaixaEmail { get; set; }
    }
}
