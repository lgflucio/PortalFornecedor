using ReadMailBox.Model.SharedModels;
using Services.ViewModels;
using System;
using static Shared.Enums.Enums;

namespace ReadMailBox.Services
{
    public class LeitorCaixasEmail : CaixasEmail
    {
        public LeitorCaixasEmail(SmtpConfigViewModel smtpConfigView)
        {
            LoadEmailConfig_Integracao(smtpConfigView);

            try
            {
                if (string.IsNullOrWhiteSpace(smtpConfigView.SmtpHost))
                {
                    SmtpConfig = null;
                    return;
                }

                SmtpConfig = new SMTPConfig();
                SmtpConfig.SmtpSSL = smtpConfigView.SmtpSSL;
                SmtpConfig.SmtpHost = smtpConfigView.SmtpHost;
                SmtpConfig.SmtpPorta = smtpConfigView.SmtpPorta;
                SmtpConfig.SmtpUser = smtpConfigView.SmtpUser;
                SmtpConfig.SmtpPassword = smtpConfigView.SmtpPassword;
                SmtpConfig.SmtpUseDefaultCredentials = smtpConfigView.SmtpUseDefaultCredentials;
            }
            catch (Exception)
            {

            }
        }
        private void LoadEmailConfig_Integracao(SmtpConfigViewModel smtpConfigView)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(smtpConfigView.MailBox))
                {
                    return;
                }

                MailConfig _caixa = new MailConfig
                {
                    MailBox = smtpConfigView.MailBox.Trim(),
                    MailHost = smtpConfigView.MailHost,
                    MailPorta = smtpConfigView.MailPorta,
                    MailUser = smtpConfigView.MailUser,
                    MailPassword = smtpConfigView.MailPassword,
                    MailSSL = smtpConfigView.MailSSL,
                    MailType = smtpConfigView.MailType,
                    TipoCaixaEmail = TipoCaixaEmail.EmailIntegracao
                };
                MailBoxes.Add(_caixa);
            }
            catch (Exception)
            {

            }
        }
    }
}
