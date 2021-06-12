using PrefeiturasWebServices.Interfaces;
using Repository.DTOs;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using SantosWebService;
using Services.Interfaces;
using Services.Mapper;
using Services.ViewModels;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Utils;
using static Shared.Enums.Enums;

namespace PrefeiturasWebServices.Services
{
    public class SantosAppService : ISantosAppService
    {
        public readonly ICiaRepository _repositoryCia;
        private readonly ILogRepository _repositoryLog;
        private readonly INfseRepository _repositoryNfse;
        private readonly IHistoricoRepository _repositoryHistorico;
        private readonly ICertificadosRepository _repositoryCertificado;
        private readonly INfseAppService _serviceNfse;
        private readonly IMapperObj _mapperObj;

        public SantosAppService(ICiaRepository repositoryCia,
                                 ILogRepository repositoryLog,
                                 INfseRepository repositoryNfse,
                                 ICertificadosRepository repositoryCertificado,
                                 INfseAppService serviceNfse,
                                 IHistoricoRepository repositoryHistorico,
                                 IMapperObj mapperObj)
        {
            _repositoryCia = repositoryCia;
            _repositoryLog = repositoryLog;
            _repositoryNfse = repositoryNfse;
            _repositoryCertificado = repositoryCertificado;
            _serviceNfse = serviceNfse;
            _repositoryHistorico = repositoryHistorico;
            _mapperObj = mapperObj;
        }
        public void Get()
        {
            List<RFE_CIA> _cias = _repositoryCia.Get();

            List<RFE_NFSE> _nfses = new List<RFE_NFSE>();
            List<string> _erros = new List<string>();
            _cias.ForEach(cia =>
            {
                try
                {
                    List<XmlNfseViewModel> _newNfses = GetByCnpj(cia.CNPJ);
                    List<RFE_NFSE> _nfsesRFE = new List<RFE_NFSE>();

                    _newNfses.ForEach(nfse =>
                    {
                        RFE_NFSE _nfseCast = _mapperObj.MapToNfse(nfse);
                        _nfseCast.MUNGER = CodigoIbgeDTO.Santos;
                        if (!_repositoryNfse.ExisteNotaNaHeaderNfse(_nfseCast.CHNFSE))
                            _nfsesRFE.Add(_nfseCast);
                    });

                    if (_nfsesRFE.Count() > 0)
                    {
                        _serviceNfse.InsertWithItensAndRepositorio(_nfsesRFE);

                        _nfsesRFE.ForEach(nfse =>
                        {
                            RFE_HISTORICO historico = new RFE_HISTORICO()
                            {
                                DESCRICAO = "NFSe inserida através de busca automática WebService Santos.",
                                EDITOR = "GSW Busca Automática.",
                                REPOSITORIO = nfse.RFE_REPOSITORIO,
                                TIPO = EHistorico.Evento
                            };
                            _repositoryHistorico.Create(historico);
                        });
                    }
                }
                catch (Exception e)
                {
                    _erros.Add(e.Message);
                }

            });
            RFE_LOG _log = new RFE_LOG();

            _repositoryLog.Create(new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = $"Numero de notas para ser inserida = {_nfses.Count}", Servico = Enums.Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Informacao });

            try
            {
                _repositoryNfse.Create(_nfses.FirstOrDefault());
            }
            catch (Exception e)
            {
                _log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = e.Message, Servico = Enums.Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Erro };
                _repositoryLog.Create(_log);
            }
            string resultado = _nfses.ToString();
        }

        public List<XmlNfseViewModel> GetByCnpj(string cnpj)
        {
            RFE_CERTIFICADOS _rfeCertificado = _repositoryCertificado.GetByCnpj(cnpj);
            X509Certificate2 _certificado = Util.GetCertificate(@"C:\Certificados\45985371000108_000001010131997.pfx", "br018726");
            //_certificado = Util.GetCertificate(@"C:\Certificados\45985371000108_000001010131997.pfx", "br018726");

            ServiceGinfesImplClient _webService = new ServiceGinfesImplClient();
            string _xmlEnvio = $@"<?xml version='1.0' encoding='utf-8'?>
                                <ConsultarNfseEnvio Id='Id1' xmlns='http://www.ginfes.com.br/servico_consultar_nfse_envio_v03.xsd'>
                                      <PeriodoEmissao>
                                        <DataInicial>{DateTime.Now.AddMonths(-5).ToString("yyyy-MM-dd")}</DataInicial>
                                        <DataFinal>{DateTime.Now.ToString("yyyy-MM-dd")}</DataFinal>
                                      </PeriodoEmissao>
                                      <Tomador>
                                        <CpfCnpj xmlns='http://www.ginfes.com.br/tipos_v03.xsd'>
                                          <Cpf>{cnpj}</Cpf>
                                        </CpfCnpj>
                                      </Tomador>
                                </ConsultarNfseEnvio>";
            XmlDocument xmlAssinado = AssinarXmlEnvio(_xmlEnvio, _certificado);
            if (_certificado.NotAfter < DateTime.Now)
                return new List<XmlNfseViewModel>();
            string resposta = _webService.ConsultarNfse(xmlAssinado.OuterXml);

            return new List<XmlNfseViewModel>();
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
