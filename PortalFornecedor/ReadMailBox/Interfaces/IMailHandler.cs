using ActiveUp.Net.Mail;
using ReadMailBox.Model.SharedModels;
using System;

namespace ReadMailBox.Interfaces
{
    public interface IMailHandler
    {
        int CountMessages { get; }
        void ConnectToMailBox(MailConfig email);
        void Disconnect();
        int SearchByDate(DateTime date);
        void MoveMailTo(string folder);
        void DeleteMail();
        Message GetMessageAt(int pos);
        //Resultado SendMailWithOneAttach(SMTPConfig smtpConfig, string[] sendTo, string[] copyTo, string subject, string body, byte[] attach, string attachName);
    }
}
