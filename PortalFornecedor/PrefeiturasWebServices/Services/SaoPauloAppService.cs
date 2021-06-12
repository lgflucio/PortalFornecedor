using Newtonsoft.Json;
using PrefeiturasWebServices.Interfaces;
using PrefeiturasWebServices.ViewModels.SaoPaulo;
using ReceiverApi.Interfaces;
using ReceiverApi.Responses;
using Repository.DTOs;
using Repository.Entities.PREFEITURAS_ENTITIES;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using Services.Interfaces;
using Services.Mapper;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using static Shared.Enums.Enums;

namespace PrefeiturasWebServices.Services
{
    public class SaoPauloAppService : ISaoPauloAppService
    {
        private readonly ISaoPauloFactory _factory;
        private readonly ICiaRepository _repositoryCia;
        private readonly ILogRepository _repositoryLog;
        private readonly IMapperObj _mapperObj;
        private readonly INfseRepository _repositoryNfse;
        private readonly IHistoricoRepository _repositoryHistorico;
        private readonly INfseAppService _serviceNfse;
        private readonly INfsePrefeiturasAppService _serviceNfsePrefeituras;
        private readonly INfsePrefeiturasRepository _repositoryNfsePrefeituras;
        private readonly IReceiverApiFactory _factoryReceiverApi;

        public SaoPauloAppService(ISaoPauloFactory factory,
                                  ICiaRepository repositoryCia,
                                  ILogRepository repositoryLog,
                                  IMapperObj mapperObj,
                                  INfseRepository repositoryNfse,
                                  IHistoricoRepository repositoryHistorico,
                                  INfseAppService serviceNfse,
                                  INfsePrefeiturasRepository repositoryNfsePrefeituras,
                                  INfsePrefeiturasAppService serviceNfsePrefeituras,
                                  IReceiverApiFactory factoryReceiverApi)
        {
            _factory = factory;
            _repositoryCia = repositoryCia;
            _repositoryLog = repositoryLog;
            _mapperObj = mapperObj;
            _repositoryNfse = repositoryNfse;
            _repositoryHistorico = repositoryHistorico;
            _serviceNfse = serviceNfse;
            _repositoryNfsePrefeituras = repositoryNfsePrefeituras;
            _serviceNfsePrefeituras = serviceNfsePrefeituras;
            _factoryReceiverApi = factoryReceiverApi;
        }
        public void Get(int periodoConsulta)
        {
            List<RFE_CIA> _cias = _repositoryCia.Get();
            List<XmlNfseViewModel> _nfses = GetByCias(_cias, periodoConsulta);
            List<RFE_NFSE> _nfsesRFE = new List<RFE_NFSE>();
            List<NotasServicos> _notas = new List<NotasServicos>();

            _nfses.ForEach(nfse =>
            {
                RFE_NFSE _nfseCast = _mapperObj.MapToNfse(nfse);
                _nfseCast.MUNGER = CodigoIbgeDTO.SaoPaulo;
                if (!_repositoryNfse.ExisteNotaNaHeaderNfse(_nfseCast.CHNFSE))
                    _nfsesRFE.Add(_nfseCast);
            });

            ///METODO PARA INSERCAO NO BANCO DE PREFEITURAS
            //_nfses.ForEach(nfse =>
            //{
            //    NotasServicos _nfseCast = _mapperObj.MapingToNfse(nfse);
            //    _nfseCast.MunicipioGerador = CodigoIbgeDTO.SaoPaulo;
            //    if (!_repositoryNfsePrefeituras.ExistsNota(_nfseCast.Chave))
            //        _notas.Add(_nfseCast);
            //});

            //_serviceNfsePrefeituras.InsertWithItensAndContainer(_notas);

            if (_nfsesRFE.Count() > 0)
            {
                _serviceNfse.InsertWithItensAndRepositorio(_nfsesRFE);


                _nfsesRFE.ForEach(nfse =>
                {
                    RFE_HISTORICO _historico = new RFE_HISTORICO()
                    {
                        DESCRICAO = "NFSe inserida através de busca automática WebService São Paulo.",
                        EDITOR = "GSW Busca Automática.",
                        REPOSITORIO = nfse.RFE_REPOSITORIO,
                        TIPO = EHistorico.Evento
                    };
                    _repositoryHistorico.Create(_historico);
                });
                _repositoryLog.Create(new RFE_LOG
                {
                    EDITOR = "GSW",
                    MODIFICADO = DateTime.Now,
                    Mensagem = $"{_nfses.Count} inseridas com sucesso, em {DateTime.Now}.",
                    Servico = Servico.Busca_Automatica_NFSE_Municipio,
                    TipoLog = TipoLog.Informacao
                });

                //FAZ A CHAMADA NA API DO RECEIVER.
                ResponseNotas _receiverApi = _factoryReceiverApi.NfsesSent(_nfsesRFE);
                if(_receiverApi.Sucesso)
                {
                    _repositoryLog.Create(new RFE_LOG
                    {
                        EDITOR = "GSW",
                        MODIFICADO = DateTime.Now,
                        Mensagem = $"{_nfses.Count} Efetuada regra de recebimento pelo Receiver, em {DateTime.Now}.",
                        Servico = Servico.Busca_Automatica_NFSE_Municipio,
                        TipoLog = TipoLog.Informacao
                    });
                }
                else
                {
                    _repositoryLog.Create(new RFE_LOG
                    {
                        EDITOR = "GSW",
                        MODIFICADO = DateTime.Now,
                        Mensagem = $"{_nfses.Count} deu merda no receiver, em {DateTime.Now}.",
                        Servico = Servico.Busca_Automatica_NFSE_Municipio,
                        TipoLog = TipoLog.Informacao
                    });
                }
            }
        }

        private List<XmlNfseViewModel> GetByCias(List<RFE_CIA> cias, int periodoConsulta = 0)
        {
            List<XmlNfseViewModel> _nfses = new List<XmlNfseViewModel>();
            cias.ForEach(cia =>
            {
                _repositoryLog.Create(new RFE_LOG
                {
                    EDITOR = "Serviço Prefeituras",
                    Mensagem = $"INICIOU DOWNLOAD DE NFSE [MUNICIPIO: SÃO PAULO] E [CNPJ: {cia.CNPJ}]",
                    MODIFICADO = DateTime.Now,
                    TipoLog = TipoLog.Informacao,
                    Servico = Servico.Busca_Automatica_NFSE_Municipio
                });

                int _paginaDaBusca = 1, _quantidadeDeXmls = 0;

                XmlDocument _xmlRetornoDoWebService = new XmlDocument();

                do
                {
                    string _result = _factory.DownloadXmlNfseSp(cia.CNPJ, "", DateTime.Now.AddDays(-periodoConsulta), DateTime.Now, _paginaDaBusca);
                    if (!_result.Contains("Certificado: "))
                    {
                        _xmlRetornoDoWebService.LoadXml(_result);

                        XDocument xDoc = XDocument.Parse(_xmlRetornoDoWebService.OuterXml);

                        bool _sucesso = bool.Parse(xDoc.Root.Descendants("Sucesso").FirstOrDefault().Value);

                        if (!_sucesso)
                        {
                            _repositoryLog.Create(new RFE_LOG
                            {
                                EDITOR = "Serviço Prefeituras",
                                Mensagem = $"ERRO NO DOWNLOAD [MUNICIPIO: SÃO PAULO] E [CNPJ: {cia.CNPJ}]",
                                MODIFICADO = DateTime.Now,
                                TipoLog = TipoLog.Informacao,
                                Servico = Servico.Busca_Automatica_NFSE_Municipio
                            });
                        }

                        XmlNodeList xmls = _xmlRetornoDoWebService.GetElementsByTagName("NFe");
                        _quantidadeDeXmls = xmls.Count;
                        for (var i = 0; i < _quantidadeDeXmls; i++)
                        {
                            XmlNode xml = xmls.Item(i);
                            XmlDocument _reader = new XmlDocument();
                            _reader.LoadXml(xml.OuterXml);

                            try
                            {
                                XDocument _xmlDocSingle = XDocument.Parse(_reader.OuterXml);

                                string _statusRetorno = _xmlDocSingle.Root.Descendants("StatusNFe").FirstOrDefault().Value;
                                if (_statusRetorno == "C")
                                    continue;

                                XmlNfseViewModel _xmlDefault = ConverterXmlToXmlDefault(_reader.OuterXml);
                                _nfses.Add(_xmlDefault);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        if (_quantidadeDeXmls == 50)
                            _paginaDaBusca++;
                    }
                    else
                    {
                        _repositoryLog.Create(new RFE_LOG
                        {
                            EDITOR = "Serviço Prefeituras",
                            Mensagem = $"ERRO NO DOWNLOAD [MUNICIPIO: SÃO PAULO] E [CNPJ: {cia.CNPJ}], CERTIFICADO VENCIDO",
                            MODIFICADO = DateTime.Now,
                            TipoLog = TipoLog.Informacao,
                            Servico = Servico.Busca_Automatica_NFSE_Municipio
                        });
                    }

                } while (_quantidadeDeXmls == 50);

            });

            return _nfses;
        }

        private XmlNfseViewModel ConverterXmlToXmlDefault(string xml)
        {
            XDocument _xmlDefault = XDocument.Parse(xml);

            XmlDocument _xml = new XmlDocument();
            _xml.LoadXml(xml);

            var notaJson = JsonConvert.SerializeXmlNode(_xml);

            var _nfseSp = JsonConvert.DeserializeObject<SaoPauloNfseViewModel>(notaJson);

            XmlNfseViewModel _nfse = new XmlNfseViewModel();

            DateTime.TryParse(_nfseSp.NFe.DataEmissaoNFe, out DateTime _dataHoraEmissao);
            DateTime.TryParse(_nfseSp.NFe.DataEmissaoRPS, out DateTime _dataHoraEmissaoRps);

            _nfse.XmlOriginal = xml;
            _nfse.DtEmissao = _dataHoraEmissao.ToString();
            _nfse.Numero = _nfseSp.NFe.ChaveNFe.NumeroNFe;
            _nfse.Serie = null; //nao existe para SP;
            _nfse.Rps.Numero = _nfseSp.NFe.ChaveRPS.NumeroRPS;
            _nfse.Rps.Serie = _nfseSp.NFe.ChaveRPS.SerieRPS;
            _nfse.Rps.Tipo = _nfseSp.NFe.TipoRPS;
            _nfse.Rps.DtEmissao = _dataHoraEmissaoRps.ToString();
            _nfse.DadosPrestador.InscricaoEstadual = _nfseSp.NFe.ChaveNFe.InscricaoPrestador;
            _nfse.DadosPrestador.Cnpj = _nfseSp.NFe.CPFCNPJPrestador.CNPJ;
            _nfse.DadosPrestador.Cpf = _nfseSp.NFe.CPFCNPJPrestador.CPF;
            _nfse.DadosPrestador.RazaoSocial = _nfseSp.NFe.RazaoSocialPrestador;
            _nfse.DadosPrestador.Endereco.Endereco = _nfseSp.NFe.EnderecoPrestador.TipoLogradouro + " " + _nfseSp.NFe.EnderecoPrestador.Logradouro;
            _nfse.DadosPrestador.Endereco.Numero = _nfseSp.NFe.EnderecoPrestador.NumeroEndereco;
            _nfse.DadosPrestador.Endereco.Complemento = _nfseSp.NFe.EnderecoPrestador.ComplementoEndereco;
            _nfse.DadosPrestador.Endereco.Bairro = _nfseSp.NFe.EnderecoPrestador.Bairro;
            _nfse.DadosPrestador.Endereco.Municipio = _nfseSp.NFe.EnderecoPrestador.Cidade;
            _nfse.DadosPrestador.Endereco.Uf = _nfseSp.NFe.EnderecoPrestador.UF;
            _nfse.DadosPrestador.Endereco.Cep = _nfseSp.NFe.EnderecoPrestador.CEP;
            _nfse.DadosPrestador.Nome = _nfseSp.NFe.RazaoSocialPrestador;
            _nfse.Tributacao = _nfseSp.NFe.TributacaoNFe;
            //_nfse.CodigoTributacaoMunicipio = _nfseSp.NFe.
            _nfse.OptanteSimplesNacional = _nfseSp.NFe.OpcaoSimples == "0" ? XmlNfseSimNao.N : XmlNfseSimNao.S;
            _nfse.NumeroGuia = _nfseSp.NFe.NumeroGuia;
            _nfse.DtPagamento = _nfseSp.NFe.DataQuitacaoGuia;
            _nfse.VlrServicos = _nfseSp.NFe.ValorServicos;
            _nfse.CodigoVerificacao = _nfseSp.NFe.ChaveNFe.CodigoVerificacao;
            _nfse.Iss.Aliquota = _nfseSp.NFe.AliquotaServicos;
            _nfse.Iss.Vlr = _nfseSp.NFe.ValorISS;
            //_nfse.VlrCredito = XmlExtention.ValueAtPath(_xmlDefault.Root, _tags.FirstOrDefault(x => x.TagDefault == "VlrServicos").Tag);
            _nfse.MesCompetencia = _dataHoraEmissao.ToString("MM/yyyy");
            _nfse.ItemListaServico = _nfseSp.NFe.CodigoServico;
            _nfse.DadosTomador.Cnpj = _nfseSp.NFe.CPFCNPJTomador.CNPJ;
            //_nfse.DadosTomador.InscricaoMunicipal = XmlExtention.ValueAtPath(_xmlDefault.Root, _tags.FirstOrDefault(x => x.TagDefault == "VlrServicos").Tag);
            _nfse.DadosTomador.RazaoSocial = _nfseSp.NFe.RazaoSocialTomador;
            _nfse.DadosTomador.Endereco.Endereco = _nfseSp.NFe.EnderecoTomador.TipoLogradouro + "" + _nfseSp.NFe.EnderecoTomador.Logradouro;
            _nfse.DadosTomador.Endereco.Numero = _nfseSp.NFe.EnderecoTomador.NumeroEndereco;
            _nfse.DadosTomador.Endereco.Bairro = _nfseSp.NFe.EnderecoTomador.Bairro;
            _nfse.DadosTomador.Endereco.Municipio = _nfseSp.NFe.EnderecoTomador.Cidade;
            _nfse.DadosTomador.Endereco.Uf = _nfseSp.NFe.EnderecoTomador.UF;
            _nfse.DadosTomador.Endereco.Cep = _nfseSp.NFe.EnderecoTomador.CEP;
            _nfse.DiscriminacaoServico = _nfseSp.NFe.Discriminacao;
            _nfse.DescricaoTipoServico = _nfseSp.NFe.Discriminacao;
            _nfse.VlrTotal = decimal.Parse(_nfseSp.NFe.ValorServicos.Replace(".", ","));

            _nfse.Itens.Add(new XmlNfseServicoItem
            {
                Descricao = _nfseSp.NFe.Discriminacao,
                Qtde = "1",
                VlrUnitario = _nfseSp.NFe.ValorServicos,
                VlrTotal = _nfseSp.NFe.ValorServicos,
                Tributavel = XmlNfseSimNao.N
            });

            return _nfse;
        }

        private void SaveLastPage(string chave, int pagina)
        {
            //_ParametrosService.saveParameter(chave, pagina.ToString(), "SERVIÇO AUTOMATICO NFSE");
            //_ParametrosService.saveParameterValue2(chave, DateTime.Now.ToString("yyyy-MM-dd"), "SERVIÇO AUTOMATICO NFSE");

        }
        public int getLastPage(string chave)
        {
            ////CRIAR UM RESET DO LAST PAGE NA PRIMEIRA EXECUÇÃO DO DIA!
            //var valor = _ParametrosService.getParameterValue(chave);

            //if (string.IsNullOrWhiteSpace(valor))
            //{
            //    saveLastPage(chave, 1);
            //    valor = _ParametrosService.getParameterValue(chave);
            //}

            //var data_last_reset = _ParametrosService.getParameterValue2(chave);
            //DateTime dataLastReset;
            //if (!string.IsNullOrWhiteSpace(data_last_reset) && DateTime.TryParse(data_last_reset, out dataLastReset))
            //{
            //    //resetar a paginação para 1 caso a ultima data de atualização seja diferente da data atual
            //    if (DateTime.Now.Date != dataLastReset.Date)
            //    {
            //        saveLastPage(chave, 1);
            //        valor = _ParametrosService.getParameterValue(chave);
            //    }
            //}

            //return String.IsNullOrWhiteSpace(valor) ? 1 : int.Parse(valor);
            return 1;
        }
    }
}

