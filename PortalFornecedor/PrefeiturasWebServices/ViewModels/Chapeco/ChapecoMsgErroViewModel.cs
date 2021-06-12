using System;
using System.Collections.Generic;
using System.Text;

namespace PrefeiturasWebServices.ViewModels.Chapeco
{

    // OBSERVAÇÃO: o código gerado pode exigir pelo menos .NET Framework 4.5 ou .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.publica.inf.br", IsNullable = false, ElementName = "ConsultaNfseRecebidaResposta")]
    public partial class ChapecoMsgErroViewModel
    {

        private ConsultaNfseRecebidaRespostaListaMensagemRetorno listaMensagemRetornoField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaListaMensagemRetorno ListaMensagemRetorno
        {
            get
            {
                return this.listaMensagemRetornoField;
            }
            set
            {
                this.listaMensagemRetornoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaListaMensagemRetorno
    {

        private ConsultaNfseRecebidaRespostaListaMensagemRetornoMensagemRetorno mensagemRetornoField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaListaMensagemRetornoMensagemRetorno MensagemRetorno
        {
            get
            {
                return this.mensagemRetornoField;
            }
            set
            {
                this.mensagemRetornoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaListaMensagemRetornoMensagemRetorno
    {

        private string codigoField;

        private string mensagemField;

        private string correcaoField;

        /// <remarks/>
        public string Codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }

        /// <remarks/>
        public string Mensagem
        {
            get
            {
                return this.mensagemField;
            }
            set
            {
                this.mensagemField = value;
            }
        }

        /// <remarks/>
        public string Correcao
        {
            get
            {
                return this.correcaoField;
            }
            set
            {
                this.correcaoField = value;
            }
        }
    }


}
