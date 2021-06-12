using PrefeiturasScrapping.Interfaces;
using ReadMailBox.Model.SharedModels;
using ReadMailBox.Services;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using static Shared.Enums.Enums;

namespace PrefeiturasScrapping.Services
{
    public class PortoAlegreAppService : IPortoAlegreAppService
    {
        public void Get(int v)
        {
            SmtpConfigViewModel _configemail = GetConfigEmailBox();

            LeitorCaixasEmail _caixasEmails = new LeitorCaixasEmail(_configemail);

            LerEmails(_caixasEmails);
        }

        public SmtpConfigViewModel GetConfigEmailBox()
        {
            return new SmtpConfigViewModel()
            {
                MailType = SmtpConfigViewModel.TypeImap,
                MailHost = "outlook.office365.com",
                MailPorta = 993,
                MailBox = "Inbox",
                MailSSL = true,
                MailUser = "rfe-integracao@gsw.com.br",
                MailPassword = "vnAgy4Z2",
                TipoCaixaEmail = TipoCaixaEmail.EmailIntegracao,
                SmtpUseDefaultCredentials = false,
                SmtpHost = "outlook.office365.com",
                SmtpPorta = 587,
                SmtpSSL = true,
                SmtpUser = "rfe-integracao@gsw.com.br",
                SmtpPassword = "vnAgy4Z2"
            };
        }

        public void LerEmails(LeitorCaixasEmail caixasEmails)
        {
            try
            {
                MailHelper _mailHandler = new MailHelper();
                IEnumerable<MailConfig> _mailBoxes = caixasEmails.MailBoxes.Where(x => x.TipoCaixaEmail == TipoCaixaEmail.EmailIntegracao || x.TipoCaixaEmail == TipoCaixaEmail.Email_Finalizacao);

                foreach (MailConfig mailBox in _mailBoxes)
                {
                    ReadMailBox(_mailHandler, mailBox, caixasEmails);
                }
            }
            catch (Exception)
            {
                //LogManager.Logar(Servico.LeituraEmail, LogType.Error, "Ocorreu um erro no metodo lerEmails :", ex);
                ////logService.Logar("Ocorreu um erro no metodo lerEmails :" + ex.Message + " stackTrace " + ex.StackTrace.ToString(), TipoLog.Erro, Servico.LeituraEmail);
            }

        }
        public void ReadMailBox(MailHelper mailHandler, MailConfig mailBox, LeitorCaixasEmail CaixasEmails)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mailBox.MailUser) || string.IsNullOrWhiteSpace(mailBox.MailPassword))
                {
                    return;
                }

                //mailHandler.ConnectToMailBox(mailBox);

                //EntradaEmails entradaEmail = new EntradaEmails(_ParamServ, logService, mailBox, conDapper, validator);

                DateTime _dt = GetLastDate(mailBox.TipoCaixaEmail);

                //Resultado _resultado = new Resultado();
                mailHandler.SearchByDate(_dt);

                for (int i = 0; i < mailHandler.CountMessages; i++)
                {
                    try
                    {
                        if (mailBox.TipoCaixaEmail == TipoCaixaEmail.EmailIntegracao)
                        {
                            //_resultado = entradaEmail.SalvarEmail(mailHandler.GetMessageAt(i));
                        }

                        //Console.WriteLine($"Email: {i.ToString()}/{_mailHandler.countMessages.ToString()}");

                        //if (_resultado.Sucesso)
                        //{
                        //    mailHandler.MoveMailTo("RFE");
                        //}
                        //else
                        //{
                        //    sendAlert(CaixasEmails.SmtpConfig, mailBox, _mailHandler.GetMessageAt(i), resultado);
                        //}
                    }
                    catch (Exception)
                    {
                        //logService.Logar(ex, Servico.LeituraEmail);
                    }
                }

                SetLastDate(mailBox.TipoCaixaEmail);
            }
            catch (Exception)
            {
                //LogManager.Logar(Servico.LeituraEmail, LogType.Error, "Ocorreu um erro no metodo ReadMailBox :", ex);
                ////logService.Logar("Ocorreu um erro no metodo ReadMailBox :" + ex.Message + " stackTrace " + ex.StackTrace.ToString(), TipoLog.Erro, Servico.LeituraEmail);
            }
        }

        private DateTime GetLastDate(TipoCaixaEmail tipoCaixa)
        {
            DateTime _dt = new DateTime();
            if (tipoCaixa == TipoCaixaEmail.EmailIntegracao)
            {
                //dt = _ParamServ.getParameterValueDateTime(Constantes.WebConfig.DataUltimaLeituraEmailIntegracao);
            }

            if (default(DateTime) == _dt)
            {
                return DateTime.Now.Date;
            }

            return _dt;
        }

        private void SetLastDate(TipoCaixaEmail tipoCaixa)
        {
            DateTime _dt = DateTime.Now;
            _dt = _dt.AddMinutes(-30); //Volta 30 minutos na hora atual para o caso de ter chego um novo e-mail durante a execução do programa após o mesmo já ter puxado a lista de e-mails disponíveis
            string sDate = _dt.ToString("yyyy-MM-dd HH:mm:ss");

            if (tipoCaixa == TipoCaixaEmail.EmailIntegracao)
            {
                //_ParamServ.saveParameter(Constantes.WebConfig.DataUltimaLeituraEmailIntegracao, sDate, "RFE", conDapper);
            }
        }
    }
}
