using ActiveUp.Net.Mail;
using ReadMailBox.Interfaces;
using ReadMailBox.Model.SharedModels;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ReadMailBox.Services
{
    public class MailHandlerIMAP : IMailHandler
    {
        private Imap4Client _imap;
        private MailConfig _email;
        private Mailbox _mailBox;
        private string _prefixo;
        private int[] _messages_ids;
        private int _currentMessageID;
        private Mailbox _caixa;

        public int CountMessages
        {
            get
            {
                return _messages_ids == null ? 0 : _messages_ids.Count();
            }
        }

        void IMailHandler.ConnectToMailBox(MailConfig email)
        {
            try
            {
                _email = email;
                _messages_ids = null;
                _mailBox = null;
                _prefixo = "";
                _currentMessageID = -1;
                _imap = new Imap4Client();
                _imap.SendTimeout = 900000;
                _imap.ReceiveTimeout = 900000;

                if (email.MailSSL)
                {
                    _imap.ConnectSsl(email.MailHost, Convert.ToInt32(email.MailPorta));
                }
                else
                {
                    _imap.Connect(email.MailHost, Convert.ToInt32(email.MailPorta));
                }

                _imap.Login(email.MailUser, email.MailPassword);

                Get_MailBox();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void IMailHandler.Disconnect()
        {
            try
            {
                _imap?.Disconnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteMail()
        {
            _imap.Command($"store {_messages_ids[_currentMessageID]} +FLAGS (\\Deleted)");
            _imap.Command("expunge");
        }

        void IMailHandler.MoveMailTo(string folder)
        {
            folder = string.IsNullOrWhiteSpace(_prefixo) ? folder : $"{_prefixo}.{folder}";
            _imap.Command($"copy {_messages_ids[_currentMessageID]} {folder}");
            DeleteMail();
        }

        int IMailHandler.SearchByDate(DateTime DataAPartirDe)
        {
            _caixa = _imap.SelectMailbox(_email.MailBox);

            _messages_ids = _caixa.Search(string.Format("SINCE \"{0}\"", DataAPartirDe.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture))); //08-Mar-2016 (Uso o invariantCulture para trazer sempre em inglês a data) 
            Array.Reverse(_messages_ids);
            return _messages_ids.Count();
        }
        Message IMailHandler.GetMessageAt(int pos)
        {
            if (_messages_ids == null)
            {
                return null;
            }

            if (pos >= _messages_ids.Count())
            {
                return null;
            }

            _currentMessageID = pos;

            Message _email = _caixa.Fetch.MessageObject(_messages_ids[pos]);
            _email.Subject = TratarAssunto(_email);
            return _email;
        }
        private void Get_MailBox()
        {
            try
            {
                _mailBox = _imap.SelectMailbox("RFE");
            }
            catch
            {
                try
                {
                    _mailBox = _imap.SelectMailbox(_email.MailBox + ".RFE");
                    this._prefixo = _email.MailBox;
                }
                catch
                {
                    try
                    {
                        _mailBox = _imap.CreateMailbox("RFE");
                    }
                    catch
                    {
                        _mailBox = _imap.CreateMailbox(_email.MailBox + ".RFE");
                        this._prefixo = _email.MailBox;
                    }
                }
            }
        }
        //Resultado IMailHandler.SendMailWithOneAttach(SMTPConfig smtpConfig, string[] sendTo, string[] copyTo, string subject, string body, byte[] attach, string attachName)
        //{
        //    try
        //    {
        //        SmtpClient _cliente = new SmtpClient();

        //        _cliente.Host = smtpConfig.SmtpHost;
        //        _cliente.Port = smtpConfig.SmtpPorta;
        //        _cliente.EnableSsl = smtpConfig.SmtpSSL;


        //        _cliente.UseDefaultCredentials = smtpConfig.SmtpUseDefaultCredentials;

        //        NetworkCredential _credenciais = new NetworkCredential(smtpConfig.SmtpUser, smtpConfig.SmtpPassword);
        //        _cliente.Credentials = _credenciais;

        //        MailMessage _mensagem = new MailMessage();
        //        _mensagem.From = new MailAddress(smtpConfig.SmtpUser);

        //        foreach (var to in sendTo)
        //        {
        //            _mensagem.To.Add(to);
        //        }

        //        if (copyTo != null)
        //        {
        //            foreach (var cpTo in copyTo)
        //            {
        //                _mensagem.CC.Add(cpTo);
        //            }
        //        }

        //        _mensagem.Subject = subject;
        //        _mensagem.Body = body;
        //        _mensagem.IsBodyHtml = true;


        //        string _filePath = "";
        //        if (attach != null)
        //        {
        //            _filePath = Path.Combine(Path.GetTempPath(), attachName);
        //            File.WriteAllBytes(_filePath, attach);
        //            _mensagem.Attachments.Add(new System.Net.Mail.Attachment(_filePath));
        //        }

        //        _cliente.Send(_mensagem);

        //        Resultado _resultado = new Resultado();
        //        _resultado.Sucesso = true;

        //        _mensagem.Dispose();
        //        _mensagem = null;
        //        _cliente.Dispose();
        //        _cliente = null;

        //        if (attach != null)
        //        {
        //            if (!String.IsNullOrWhiteSpace(_filePath))
        //            {
        //                File.Delete(_filePath);
        //            }
        //        }

        //        return _resultado;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Resultado.Criar(ex, "", "");
        //    }
        //}

        private string TratarAssunto(Message mail)
        {
            string _subject = (mail.Subject.Contains("=?iso-8859-1") || mail.Subject.Contains("=?") || mail.Subject.Contains("?=")) ? DecodeEncodedWordValue(mail.Subject) : mail.Subject;
            _subject = _subject.Replace("Pré-Ocorr??ncia", "Pré-Ocorrência");
            return _subject.Length > 100 ? _subject.Substring(0, 100) : _subject;

        }

        private string DecodeEncodedWordValue(string mimeString)
        {
            Regex _regex = new Regex(@"=\?(?<charset>.*?)\?(?<encoding>[qQbB])\?(?<value>.*?)\?=");
            string _encodedString = mimeString;
            string _decodedString = string.Empty;

            while (_encodedString.Length > 0)
            {
                Match _match = _regex.Match(_encodedString);
                if (_match.Success)
                {
                    // If the match isn't at the start of the string, copy the initial few chars to the output
                    _decodedString += _encodedString.Substring(0, _match.Index);

                    string _charset = _match.Groups["charset"].Value;
                    string _encoding = _match.Groups["encoding"].Value.ToUpper();
                    string _value = _match.Groups["value"].Value;

                    if (_encoding.Equals("B"))
                    {
                        // Encoded value is Base-64
                        byte[] _bytes = Convert.FromBase64String(_value);
                        _decodedString += Encoding.GetEncoding(_charset).GetString(_bytes);
                    }
                    else if (_encoding.Equals("Q"))
                    {
                        // Encoded value is Quoted-Printable
                        // Parse looking for =XX where XX is hexadecimal
                        Regex _regx = new Regex("(\\=([0-9A-F][0-9A-F]))", RegexOptions.IgnoreCase);
                        _decodedString += _regx.Replace(_value, new MatchEvaluator(delegate (Match m)
                        {
                            string _hex = m.Groups[2].Value;
                            int _iHex = Convert.ToInt32(_hex, 16);

                            // Return the string in the charset defined
                            byte[] _bytes = new byte[1];
                            _bytes[0] = Convert.ToByte(_iHex);
                            return Encoding.GetEncoding(_charset).GetString(_bytes);
                        }));
                        _decodedString = _decodedString.Replace('_', ' ');
                    }
                    else
                    {
                        // Encoded value not known, return original string
                        // (Match should not be successful in this case, so this code may never get hit)
                        _decodedString += _encodedString;
                        break;
                    }

                    // Trim off up to and including the match, then we'll loop and try matching again.
                    _encodedString = _encodedString.Substring(_match.Index + _match.Length);
                }
                else
                {
                    // No match, not encoded, return original string
                    _decodedString += _encodedString;
                    break;
                }
            }
            return _decodedString;
        }
    }
}
