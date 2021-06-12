using ActiveUp.Net.Mail;
using ReadMailBox.Interfaces;
using ReadMailBox.Model.SharedModels;
using System;

namespace ReadMailBox.Services
{
    public class MailHelper
    {
        private MailConfig _email;
        private IMailHandler _mailHandler;

        public void ConnectSMTP()
        {
            _mailHandler = new MailHandlerIMAP();
        }

        private void GetHandlerInstance(MailConfig email)
        {
            try
            {
                if (email.MailType == "IMAP")
                {
                    _mailHandler = new MailHandlerIMAP();
                }
                else
                {
                    _mailHandler = new MailHandlerPOP3();
                }

            }
            catch (Exception)
            {
            }
        }

        //public Resultado ConnectToMailBox(MailConfig email)
        //{
        //    try
        //    {
        //        GetHandlerInstance(email);

        //        _mailHandler.ConnectToMailBox(email);

        //        return new Resultado();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Resultado.Criar(ex, "ConnectToMailBox", "");
        //    }
        //}

        public int SearchByDate(DateTime date)
        {
            return _mailHandler.SearchByDate(date);
        }

        public Message GetMessageAt(int pos)
        {
            return _mailHandler.GetMessageAt(pos);
        }

        public int CountMessages
        {
            get
            {
                return _mailHandler.CountMessages;
            }
        }

        public void Disconnect()
        {
            _mailHandler.Disconnect();
        }

        public void MoveMailTo(string folder)
        {
            _mailHandler.MoveMailTo(folder);
        }
        public void DeleteMail()
        {
            _mailHandler.DeleteMail();
        }
        //public Resultado SendMailWithOneAttach(SMTPConfig smtpConfig, string[] sendTo, string[] copyTo, string subject, string body, byte[] attach, string attachName)
        //{
        //    return _mailHandler.SendMailWithOneAttach(smtpConfig, sendTo, copyTo, subject, body, attach, attachName);
        //}
    }
}
