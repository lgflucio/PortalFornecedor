using PrefeiturasWebServices.Interfaces;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using Services.Interfaces;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Utils;
using SaoPauloWebService;
using static SaoPauloWebService.LoteNFeSoapClient;

namespace PrefeiturasWebServices.Factorys
{
    public class SaoPauloFactory : ISaoPauloFactory
    {

        private readonly ICertificadosRepository _repositoryCertificado;
        private readonly IRepositorioRepository _repositoryRepositorio;
        private readonly ICertificadoAppService _serviceCertificado;

        public SaoPauloFactory(ICertificadosRepository repositoryCertificado, IRepositorioRepository repositoryRepositorio,
                               ICertificadoAppService serviceCertificado)
        {
            _repositoryCertificado = repositoryCertificado;
            _repositoryRepositorio = repositoryRepositorio;
            _serviceCertificado = serviceCertificado;
        }
        public string DownloadXmlNfseSp(string cnpjEstabelecimento, string im, DateTime dtInicio, DateTime dtFim, int numeroPagina)
        {
            try
            {
                X509Certificate2 _certificadoDigital = new X509Certificate2();
                RFE_CERTIFICADOS _certificado = _repositoryCertificado.GetByCnpj(cnpjEstabelecimento);
                if (_certificado == null)
                    return $"Certificado: Nao encontrado na base de dados";
                else
                    _certificadoDigital = Util.GetCertificate(_certificado.CERTIFICADO, _certificado.SENHA);

                var _mensagemXML = string.Format(
                    @"<?xml version='1.0' encoding='UTF-8'?>
                    <p1:PedidoConsultaNFePeriodo xmlns:p1='http://www.prefeitura.sp.gov.br/nfe' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                        <Cabecalho Versao='1'>
                            <CPFCNPJRemetente>
                                <CNPJ>{0}</CNPJ>
                            </CPFCNPJRemetente>
                            <CPFCNPJ>
                                <CNPJ>{0}</CNPJ>
                            </CPFCNPJ>
                            {1}
                            <dtInicio>{2}</dtInicio>
                            <dtFim>{3}</dtFim>
                            <NumeroPagina>{4}</NumeroPagina>
                        </Cabecalho>
                    </p1:PedidoConsultaNFePeriodo>"
                , cnpjEstabelecimento
                , string.IsNullOrEmpty(im) ? "" : string.Format("<Inscricao>{0}</Inscricao>", im)
                , dtInicio.ToString("yyyy-MM-dd")
                , dtFim.ToString("yyyy-MM-dd")
                , numeroPagina
                );


                XmlDocument xml = AssinarXmlEnvio(_mensagemXML, _certificadoDigital);

                LoteNFeSoapClient _webService = new LoteNFeSoapClient(EndpointConfiguration.LoteNFeSoap12);
                _webService.ClientCredentials.ClientCertificate.Certificate = _certificadoDigital;

                if (_certificadoDigital.NotAfter < DateTime.Now)
                    return $"Certificado: Vencido - {_certificadoDigital.NotAfter}";
                return _webService.ConsultaNFeRecebidas(1, xml.OuterXml);
            }
            catch (Exception ex)
            {
                return $"Certificado: Erro na operação - {ex.Message}";
            }
        }

        public XmlDocument AssinarXmlEnvio(string mensagemXML, X509Certificate2 certificado)
        {

            var xmlDoc = new XmlDocument();

            SignedXml SignedDocument;
            var keyInfo = new KeyInfo();

            xmlDoc.LoadXml(mensagemXML);

            //'Adiciona Certificado ao Key Info
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            SignedDocument = new SignedXml(xmlDoc)
            {
                //Permite exportar a PrivateKey do certificado digital
                SigningKey = certificado.GetRSAPrivateKey(),
                KeyInfo = keyInfo
            };
            //'Seta chaves
            //' Cria referencia
            var reference = new Reference { Uri = string.Empty };
            //' Adiciona transformacao a referencia
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform(false));
            //' Adiciona referencia ao xml
            SignedDocument.AddReference(reference);
            //' Calcula Assinatura
            SignedDocument.ComputeSignature();
            //' Pega representação da assinatura
            var xmlDigitalSignature = SignedDocument.GetXml();
            //' Adiciona ao doc XML
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
            return xmlDoc;
        }
    }
}
