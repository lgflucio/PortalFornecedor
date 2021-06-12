using ActiveUp.Net.Mail;
using ReadMailBox.Interfaces;
using ReadMailBox.Model.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ReadMailBox.Services
{
    public class MailHandlerPOP3 : IMailHandler
    {
        private Pop3Client _pop;
        private MailConfig _email;

        private string _prefixo;
        private int[] _messages_ids;
        private int _currentMessageID;
        private Mailbox _caixa;
        private List<Message> _messageCollection;
        private List<int> _indexCollection;

        int IMailHandler.CountMessages
        {
            get
            {
                //return _messageCollection == null ? 0 : _messageCollection.Count;
                return _indexCollection == null ? 0 : _indexCollection.Count();
            }
        }

        void IMailHandler.ConnectToMailBox(MailConfig email)
        {
            try
            {
                _email = email;
                _messages_ids = null;

                _prefixo = "";
                _currentMessageID = -1;
                _pop = new Pop3Client();
                _pop.SendTimeout = 900000;
                _pop.ReceiveTimeout = 900000;
                _messageCollection = new List<Message>();
                _indexCollection = new List<int>();

                if (email.MailSSL)
                {
                    _pop.ConnectSsl(_email.MailHost, _email.MailPorta, _email.MailUser, _email.MailPassword);
                }
                else
                {
                    _pop.Connect(_email.MailHost, _email.MailPorta, _email.MailUser, _email.MailPassword);
                }
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
                _pop?.Disconnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void IMailHandler.MoveMailTo(string folder)
        {

        }

        int IMailHandler.SearchByDate(DateTime DataAPartirDe)
        {
            //RepositoryDapper conDapper = new RepositoryDapper();
            //conDapper.connect();
            //RFE_PARAMETROS rParamQtd = null;
            //if (_email.TipoCaixaEmail == TipoCaixaEmail.Email_Integracao)
            //    rParamQtd = conDapper.returnParametro(Constantes.WebConfig.LerUltimosXXXXEmailIntegracao);
            //else if (_email.tipoCaixaEmail == Model.RFE_Enums.TipoCaixaEmail.Email_Finalizacao)
            //    rParamQtd = conDapper.returnParametro(Constantes.WebConfig.LerUltimosXXXXEmailFinalizado);

            int _lerQtd = 0;
            int _indexInicial = 1;
            //if (rParamQtd != null)
            //    LerQtd = string.IsNullOrWhiteSpace(rParamQtd.VALUE) ? 0 : int.Parse(rParamQtd.VALUE);
            //conDapper.disconnect();


            _indexCollection = new List<int>();
            int _qtdTotalCount = _pop.MessageCount;

            if (_qtdTotalCount > _lerQtd)
                _indexInicial = _qtdTotalCount - _lerQtd;

            //----var nuids = _pop.GetUniqueIds();
            //--for (int n = qtdTotalCount; n >= indexInicial; n--)

            for (int n = _indexInicial; n <= _qtdTotalCount; n++)
            {
                Message _newMessage = _pop.RetrieveMessageObject(n);

                DateTimeOffset _dt = DateTimeOffset.Parse(_newMessage.DateString);
                if (_dt >= DataAPartirDe)
                {
                    _indexCollection.Add(n);
                }
            }
            return _indexCollection.Count();
        }

        Message IMailHandler.GetMessageAt(int pos)
        {
            //if (_messageCollection.Count == 0)
            //    return null;
            //if (pos >= _messageCollection.Count)
            //    return null;

            if (_indexCollection.Count == 0)
                return null;
            if (pos >= _indexCollection.Count)
                return null;

            _currentMessageID = pos;
            int _index = _indexCollection.ElementAt(pos);
            Message _email = _pop.RetrieveMessageObject(_index);
            _email.Subject = TratarAssunto(_email);
            _email.BodyHtml.Text = ConvertTextFromPOP(_email.BodyHtml.Text);
            _email.BodyText.Text = ConvertTextFromPOP(_email.BodyText.Text);
            return _email;
        }

        //public Resultado SendMailWithOneAttach(SMTPConfig smptConfig, string[] sendTo, string[] copyTo, string subject, string body, byte[] attach, string attachName)
        //{
        //    throw new NotImplementedException();
        //}

        public void DeleteMail()
        {
            throw new NotImplementedException();
        }

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

        public static string ConvertTextFromPOP(string texto)
        {
            if (texto == null)
                return "";
            if (texto.IndexOf("=0A") == -1 && texto.IndexOf("=C3=") == -1)
                return texto;

            string _carac = '=' + System.Environment.NewLine;
            texto = texto.Replace(_carac, "");
            texto = texto.Replace(System.Environment.NewLine, "");

            //---------------------- Quando não tem =0A mas tem =C3=
            texto = texto.Replace("=C3=A7", "ç");
            texto = texto.Replace("=C3=87", "Ç");
            texto = texto.Replace("=C3=A3", "ã");
            texto = texto.Replace("=C3=83", "Ã");
            texto = texto.Replace("=C3=BA", "ú");
            texto = texto.Replace("=C3=A9", "é");
            texto = texto.Replace("=C3=AA", "ê");
            texto = texto.Replace("=C3=A1", "á");
            texto = texto.Replace("=C3=81", "Á");


            //---------------------- Quando POSSUI =0A mas tem =C3=
            texto = texto.Replace("=0A", "");
            texto = texto.Replace("=E7", "ç");
            texto = texto.Replace("=E3", "ã");
            texto = texto.Replace("=E1", "á");
            texto = texto.Replace("=FA", "ú");
            texto = texto.Replace("=E9", "é");
            texto = texto.Replace("=EA", "ê");

            return texto;
        }
    }
}
