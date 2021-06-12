using System;
using System.ComponentModel;
using System.Reflection;

namespace Shared.Enums
{
    public class Enums
    {
        public enum StatusProtocolo
        {
            [Description("Em Upload")]
            Upload,
            [Description("Em Preenchimento")]
            Preenchimento,
            [Description("Finalizado")]
            Finalizado
        }
        public enum OpcaoTributaria
        {
            [Description("Lucro Presumido")]
            Lucro_Presumido = 1,
            [Description("Lucro Real")]
            Lucro_Real = 2,
            [Description("Simples Nacional")]
            Simples_Nacional = 3,
            [Description("Outros")]
            Outros = 4
        }

        public enum CRT
        {
            [Description("Simples Nacional")]
            Simples_Nacional = 1,
            [Description("Simples Nacional - Excesso de sublimite de receita bruta")]
            Simples_Nacional_sublimite = 2,
            [Description("Regime Normal")]
            Regime_Normal = 3

        }

        public enum TipoDashboard
        {
            [Description("Nota Fiscal Eletronica")]
            NFe = 1,
            [Description("Nota Fiscal de Serviço Eletronica")]
            NFSe = 2,
            [Description("CTe")]
            CTe = 3
        }

        public enum Estados
        {
            //[Description("Acre")]
            //AC,
            //[Description("Alagoas")]
            //AL,
            //[Description("Amazonas")]
            //AM,
            //[Description("Amapá")]
            //AP,
            //[Description("Bahia")]
            //BA,
            //[Description("Ceará")]
            //CE,
            //[Description("Distrito Federal")]
            //DF,
            //[Description("Espírito Santo")]
            //ES,
            //[Description("Goiás")]
            //GO,
            //[Description("Maranhão")]
            //MA,
            //[Description("Minas Gerais")]
            //MG,
            //[Description("Mato Grosso do Sul")]
            //MS,
            //[Description("Mato Grosso")]
            //MT,
            //[Description("Pará")]
            //PA,
            //[Description("Paraíba")]
            //PB,
            //[Description("Pernambuco")]
            //PE,
            //[Description("Piauí")]
            //PI,
            //[Description("Paraná")]
            //PR,
            //[Description("Rio de Janeiro")]
            //RJ,
            //[Description("Rio Grande do Norte")]
            //RN,
            //[Description("Rondônia")]
            //RO,
            //[Description("Roraima")]
            //RR,
            //[Description("Rio Grande do Sul")]
            //RS,
            //[Description("Santa Catarina")]
            //SC,
            //[Description("Sergipe")]
            //SE,
            //[Description("São Paulo")]
            //SP,
            //[Description("Tocantins")]
            //TO
            [Description("AC")]
            AC,
            [Description("AL")]
            AL,
            [Description("AM")]
            AM,
            [Description("AP")]
            AP,
            [Description("BA")]
            BA,
            [Description("CE")]
            CE,
            [Description("DF")]
            DF,
            [Description("ES")]
            ES,
            [Description("GO")]
            GO,
            [Description("MA")]
            MA,
            [Description("MG")]
            MG,
            [Description("MS")]
            MS,
            [Description("MT")]
            MT,
            [Description("PA")]
            PA,
            [Description("PB")]
            PB,
            [Description("PE")]
            PE,
            [Description("PI")]
            PI,
            [Description("PR")]
            PR,
            [Description("RJ")]
            RJ,
            [Description("RN")]
            RN,
            [Description("RO")]
            RO,
            [Description("RR")]
            RR,
            [Description("RS")]
            RS,
            [Description("SC")]
            SC,
            [Description("SE")]
            SE,
            [Description("SP")]
            SP,
            [Description("TO")]
            TO,
            [Description("BR")]
            BR
        }
        public enum NotaEstoque
        {
            A_Definir,
            Estoque,
            Nao_Estoque
        }
        public enum TiposDocSEFAZ
        {
            resNFe,
            procNFe,
            resEvento,
            procEventoNFe,
            Outros,
            CTE,
            procEventoCTE,
            retEnvEvento,
            resCanc,
            resCCe,
            protNFe,
            protCTe
        }
        public enum AtoresCTE
        {
            Emitente,
            Remetente,
            Expedidor,
            Recebedor,
            Destinatario,
            tomador
        }
        public enum ERP
        {
            NaoEspecificado,
            Protheus,
            Oracle,
            BPCS,
            SIGS,
            JDE,
            AX,
            Oracle12,
            BAAN,
            DataSul,
            JDESincrono,
            JDESincronoProd
        }
        public enum TipoRecebimentoCTE
        {
            PorCFOP,
            PorTipoFornecedor,
            FreteCompraVenda
        }
        public enum TipoRecebimentoNFSE
        {
            PorCodServico,
            PorTipoFornecedor
        }
        public enum TipoRecebimentoNFE
        {
            PorCFOP,
            PorTipoFornecedor,
            PorCFOPRecebimentoFiscalMisto
        }
        public enum TipoRecebimentoForncedor
        {
            SemPedido,
            ComPedido
        }
        public enum TipoBuscaTipoOC
        {
            IN,
            NOT_IN
        }
        public enum FlagsRFE
        {
            NaoProcessado,
            Processado,
            Processado_Com_Erro
        }
        public enum EStatusConversao
        {
            A_Definir,
            Erro_ao_tentar_identificar,
            Recebido_em_xml,
            Nao_Converter,
            Pendente_de_envio,
            Enviado_para_conversao,
            Convertido,
            Rejeitado,
            Importado_Para_Produto,
            Erro_Importacao,
            Nao_Converter_Email_Possui_XML_Valido,
            Divergente,
            Divergencia_Aceita
        }
        public enum ETipoConversao
        {
            Anexo,
            Link,
            Email_Generico
        }

        public enum ETipoXML
        {
            NFE,
            CTE,
            NFSe,
            NaoIdentificado,
            Ciencia,
            CTEOS,
            Documentos_Diversos,
            Resumo
        }
        public enum ETipoArquivo
        {
            SpedFiscal,
            SpedContribuicoes,
            ECD
        }
        public enum TipoCTE
        {
            CTE,
            CTE_OS
        }
        public enum EStatusRFe
        {
            Selecionar,
            Associar,
            Validar_Diversa,
            Portaria,
            Almoxarifado,
            Finalizada,
            Validar_Devolução,
            Nao_se_Aplica,
            Finalizada_Sem_Recebimento,
            Aguardando_Integracao,
            Finalizada_Sem_Integracao_Com_ERP

        }

        public enum StatusNDDFrete
        {
            [Description("Não Integrado")]
            Nao_Integrado,
            [Description("Integrado")]
            Integrado,
            [Description("Validado")]
            Validado,
            [Description("Invalidado")]
            Invalidado,
            [Description("Faturado")]
            Faturado,
            [Description("Cancelado")]
            Cancelado
        }
        public enum TipoDocNDDFrete
        {
            CTe,
            NFSe,
            Debito
        }
        public enum TipoIntegracao
        {
            [Description("Esta nota não esta pendente de integração no momento")]
            Nao_Pendente_de_Integracao = 0,
            [Description("Finalizar nota cancelada sem recebimento")]
            FinalizarSemRecebimento_Cancelada = 1,
            [Description("Finalizar nota desconhecida ou não realizada sem recebimento")]
            FinalizarSemRecebimento_Manifesto = 2,
            [Description("Erro de integração")]
            ERRO_INTEGRACAO = 3,
            [Description("Informar ERP sobre o recebimento da nova nota")]
            RfeSaidaEntradaNfe = 4,
            [Description("Integração Pre-Associação")]
            RfeSaidaPreAssociacaoNfe = 5,
            [Description("Integração Pre-Associação CTE")]
            RfeSaidaEntradaCTe = 6,
            [Description("Recebimento do CTE pelo RFE")]
            RfeRecebimentoXMLCTe = 7,
            [Description("Recebimento do NFSE pelo RFE")]
            RfeRecebimentoXmlNfseViewModel = 8,
            [Description("Integração Pre-Associação")]
            RfeSaidaPreAssociacaoNFSe = 9,
            [Description("Integração Validar Diversas Envio Portaria")]
            RfeSaidaNfePortaria = 10,
            [Description("Integração Validar Devolução")]
            RfeSaidaNfeDevolucao = 11,
            [Description("Integração Validar Diversas Finalizar")]
            RfeSaidaNfeDiversaFinalizar = 12
        }
        public enum StatusIntegracaoHomsoft
        {
            [Description("Status Não Integrado")]
            Nao_integrado = 0,
            [Description("Integrado")]
            Integrado = 1,
            [Description("Confirmado")]
            Confirmado = 2
        }
        public enum StatusIntegracaoERP
        {
            [Description("Status Não identificado")]
            Nao_identificado = 0,
            [Description("Concluído")]
            Concluido = 1,

            //JDE
            [Description("Aguardando Simulação")]
            Aguardando_Simulacao = 2,
            [Description("Aguardando Close Receipt")]
            Aguardando_CloseReceipt = 3,
            [Description("Aguardando Close NF")]
            Aguardando_CloseNF = 4,
            [Description("Aguardando confirmação do usuário")]
            Aguardando_Confirmacao_Usuario = 5,
            [Description("Aguardando Voucher Simulation")]
            Aguardando_Voucher_Simulation = 6

        }
        public enum ExecutarMetodo
        {
            [Description("Nenhum metodo extra deverá ser chamado")]
            Nenhum = 0,
            [Description("Executar o metodo Selecionar_AposIntegracao_1")]
            Selecionar_AposIntegracao_1 = 1,
            finalizarPreAssociacaoNotaCondicional_AposIntegracao = 2,
            CTE_salvar_TelaPreAssociacao_AposIntegracao_Finalizar = 3,
            CTE_salvar_TelaPreAssociacao_AposIntegracao_Confirmar = 4
        }
        public enum ELote
        {
            Não = 0,
            Sim = 1
        }

        public enum EStatusRFeNFSe
        {
            Associar,
            Validar_Diversa,
            Finalizada,
            Finalizada_Sem_Recebimento,
            Finalizada_Sem_Integracao_Com_ERP,
            Aprovacao,
            NaoClassificado
        }

        public enum EStatusRetornoAsync
        {
            Processado,
            Nao_Processado
        }

        public enum EStatusRFeDiversos
        {
            Associar,
            Validar_Diversa,
            Finalizada,
            Finalizada_Sem_Recebimento,
            Finalizada_Sem_Integracao_Com_ERP
        }
        public enum EStatusRFeCTe
        {
            Associar,
            Validar_Diversa,
            Finalizada,
            Finalizada_Sem_Integracao_Com_ERP
        }

        public enum EFinNFe
        {
            Normal = 1,
            Complementar = 2,
            Ajuste = 3,
            Devolucao = 4
        }

        public enum EStatus
        {
            Não_Manifestada,
            Ciência,
            Desconhecida,
            Não_Realizada,
            Realizada,
            Retomada,
            Nao_se_Aplica
        }

        public enum EStatusManifestacao
        {
            Não_Manifestada,
            Manifestação_Reversível,
            Manifestação_Irreversível
        }

        public enum StatusBloqSelec
        {
            Não_Realizado,
            Bloqueado,
            Processado,
            Erro_Devolucao
        }

        public enum EEvento
        {
            Confirmacao_da_Operacao = 210200,
            Ciencia_da_Operacao = 210210,
            Desconhecimento_da_Operacao = 210220,
            Operacao_nao_Realizada = 210240
        }

        public enum ETipo
        {
            CTe,
            NFSe,
            NFe,
            NFeExclusao,
            controleExecucao
        }

        public enum EModFrete
        {
            Por_conta_do_emitente = 0,
            Por_conta_do_remetente = 1,
            Por_conta_de_terceiros = 2,
            Sem_frete = 9
        }

        public enum StatusEntrega
        {
            [Description("Portaria")]
            Portaria = 0,
            [Description("Pesagem de Entrada")]
            LiberarPesagem = 1,
            [Description("Almoxarifado")]
            Almoxarifado = 2,
            [Description("Pesagem de Saída")]
            LiberarSaida = 3,
            [Description("Finalizada")]
            Finalizada = 4,
            [Description("Todos")]
            Todos = 5
        }

        public enum StatusGerenciamentoExpedicao
        {
            [Description("Chegada Fábrica")]
            Chegada = 0,
            [Description("Pesagem de Entrada")]
            PesagemEntrada = 1,
            [Description("Aguardando Ship Confirm")]
            AguardandoShipConfirm = 2,
            [Description("Pesagem de Saída")]
            PesagemSaida = 3,
            [Description("Finalizada")]
            Finalizada = 4,
            [Description("Todos")]
            Todos = 5
        }

        public enum EHistorico
        {
            Manifestação,
            Nota,
            Enviado_sem_OC,
            Evento,
            Evento_StatusAtual,
            DocDiverso
        }

        public enum TipoFiltro
        {
            NFe,
            ConsultaForn,
            ConsultaOCxForn,
            ConsultaNFe,
            ConsultaNFeGDE,
            ConsultaCTeGDE,
            ConsultaNFSeGDE,
            ConsultaDiversosGDE,
            CTe,
            CTe_PreAssociar,
            CTe_ValidarDiversas,
            ConsultaCTe,
            NFSe,
            Diversos,
            Selecionar,
            Associar,
            AssociarNFSe,
            Validar_Diversa,
            RecebimentoPortariaNFe,
            RecebimentoPortariaComPesagemNfe,
            RecebimentoAlmoxarifadoNFe,
            Controle,
            FatorConversao,
            ERP,
            ReprocessarNotas,
            Aguardando_Integracao,
            CockpitMK,
            CockpitMK_CTE,
            CockpitMK_NFSE,
            ConversaoEmails,
            ConsultaSpedFiscal,
            PF_ConsultaNFSE,
            PF_ConsultaFornecedor,
            PF_ConsultaDiversos,
            PF_PreenchimentoNFSE,
            Aprovacao_NFSE,
            CockpitGeralNFe,
            CockpitGeralNFSe,
            CockpitGeralCTe,
            CadastroPortaria,
            CFOP,
            GerenciamentoExpedicao,
            PendenciasNddFrete
        }

        public enum Direcao
        {
            None,
            Ascending,
            Descending,
        }

        public enum Paginas
        {
            Controle,
            GerenciarCFOPs,
            GerenciarFator,
            GerenciarUsuarios,
            GerenciarMunicipios,
            GerenciarFuncionalidades,
            GerenciarWebServicesCTe,
            AprovarDiversa,
            GerenciarWebServicesNFe,
            GerenciarCertificados,
            GerenciarTiposXml,
            Parametros,
            ParametrosGerais,
            Login,
            SelecionarCTe,
            SelecionarNFe,
            SelecionarNFSe,
            RecebimentoPortariaNFe,
            RecebimentoAlmoxarifadoNFe,
            PreAssociar,
            PreAssociarNFSe,
            PreAssociarCTe,
            SelecionarOCs,
            SelecionarItemPreAssociacao,
            Consulta,
            ConsultaNFeGDE,
            ConsultaCTeGDE,
            ConsultaNFSeGDE,
            ConsultaDiversosGDE,
            ConsultaCTe,
            ConsultaFornecedor,
            ConsultaOCxFornecedor,
            DetalhesCTe,
            ConsultaNFSe,
            DetalhesNFSe,
            DetalhesAlmoxarifado,
            ValidarDiversas,
            ValidarDiversasCTe,
            ValidarDevolucao,
            ReprocessarNotas,
            Aguadando_Integracao,
            CockPit_MK_NFE,
            CockPit_MK_CTE,
            CockPit_MK_NFSE,
            ConversaoEmails,
            AprovacaoNFSE,
            RecebimentoPortariaComPesagemNfe,
            NFeCockpit,
            CTeCockpit,
            NFSeCockpit,
            RetornarStatus,
            GerenciamentoExpedicao,
            PendenciasNddFrete

        }
        public enum FlgExecRegraNegocio
        {
            NaoProcessado = 0,
            Processado = 1,
            Erro = 2
        }
        public enum FlagServicos
        {
            [Description("Pendente de Envio")]
            Nenhum,
            [Description("Integrado")]
            ExportadoHomine,
            [Description("Confirmado")]
            ExportadoHomine_Confirmado,
            [Description("Erro")]
            Erro_De_Integração,
            [Description("Não se Aplica")]
            NA,
            [Description("Sem Certificado")]
            Sem_Certificado,
            [Description("Nota Inválida")]
            Nota_Invalida,
            [Description("Expotado para o TMS")]
            Exportado_TMS

        }
        public enum OrigemXML
        {
            Carga,
            DownloadReceita,
            UploadManual,
            Busca_Manual_NFSE_SP,
            Busca_Automatica_NFSE_SP,
            Servico_Leitura_de_Emails,
            Upload_Via_WebService,
            RaspagemDeDados
        }
        public enum TpCTE
        {
            [Description("CT-e Normal")]
            Normal = 0,
            [Description("CT-e de Complemento de Valores")]
            Complemento = 1,
            [Description("CT-e de Anulação")]
            Anulação = 2,
            [Description("CT-e Substituto")]
            Substituto = 3
        }
        public enum TipoOperacao
        {
            Entrada = 0,
            Saida = 1,
        }
        public enum TipoAgrupamentoSumario
        {
            Recebidas,
            Emitidas,
            Remet_Exped,
            Tomados_Receb_Dest,
            Total
        }
        public enum StatusSefaz
        {
            [Description("Não Identificado")]
            Não_Identificado = 0,
            [Description("Autorizada")]
            Autorizada = 100,
            [Description("Cancelada")]
            Cancelada = 101,
            [Description("Denegada")]
            Denegada = 302,
            [Description("Inutilizada")]
            Inutilizada = 102,
            [Description("Cancelada Fora do Prazo")]
            CanceladoForaPrazo = 151
        }
        public enum StatusEventos
        {
            Autorizada = 100,
            Cancelada = 135110111,
            Carta_de_Correcao = 135110110,
            Denegada = 302,
            Inutilizada = 102,
            Rejeicao = 236
        }
        public enum TipoDownloadPack
        {
            PDF,
            XML,
            CSV,
            XLS,
            XLS_VoceEmpreendedora,
            XLS_CFOP
        }


        public enum Servico
        {
            [Description("Serviço de Carga")]
            Carga,
            [Description("Serviço de Leitura de Email")]
            CargaXML,
            [Description("Serviço de Carga NFe de Saída")]
            CargaXMLSaida,
            [Description("Serviço de Download de NFe da Receita Federal")]
            DownloadXMLReceita,
            [Description("Erro na camada de Business")]
            ErroCamadaBusiness,
            [Description("Upload Manual")]
            UploadManual,
            [Description("Interfaces ERP")]
            InterfacesERP,
            [Description("Busca Manual NFSE Município")]
            Busca_Manual_NFSE_Municipio,
            [Description("Busca Automática NFSE Município")]
            Busca_Automatica_NFSE_Municipio,
            [Description("Interface Retorno ERP")]
            Interface_Retorno_ERP,
            [Description("Serviço de integração HOMSOFT")]
            Integracao_HOMSOFT,
            [Description("Serviço de execução de regras de negocio do RFE")]
            Regras_De_Negocio,
            [Description("Serviço de carga Tabela FAST para RFE/GDE")]
            CargaFast_Para_RFEGDE,
            [Description("Serviço de leitura de e-mail 2.0")]
            LeituraEmail,
            [Description("Serviços em Background")]
            ServicosBackground,
            [Description("Download em Lote")]
            DownloadEmLote,
            [Description("Download Missing NSU na FAST")]
            Download_Missing_NSU_FAST,
            [Description("Upload via webservice")]
            UploadViaWebService,
            [Description("Portal de fornecedores")]
            PortalFornecedores,
            [Description("Serviço NDD Frete")]
            Serviço_NDD_Frete,
            [Description("Serviço CTe Diversas Finalizar")]
            servicoCTeDiversas,
            [Description("Serviço Diversas e Devolucao Automático")]
            servicoNFeDiversaDevolucao
        }
        public enum TipoCaixaEmail
        {
            EmailIntegracao,
            Conversao,
            Email_Finalizacao,
            Diretorio,
            Portal_NFSE,
            Portal_Diversos
        }

        public enum TipoLogLote
        {
            Erro
        }

        public enum TipoNota
        {
            NFe = 0,
            CTe = 1,
            NFSe = 2
        }

        public enum TipoDaNota
        {
            Intermediação = 1,
            Não_Intermediação = 2,
        }

        public enum TipoNFSE
        {
            [Description("Demais Serviços")]
            Demais_Servicos = 0,
            [Description("Serviço de Frete")]
            Frete = 1
        }

        public enum TipoLog
        {
            Erro,
            Informacao
        }
        public enum TipoThread
        {
            DownloadEmLoteNFE,
            ManifestacaoDeCiencia,
            DownloadEmLoteCTE,
            DownloadEmLoteNFSe,
            DownloadEmLoteDiversos,
            DownloadEscrituracaoNFE_CSV,
            DownloadEscrituracaoCTE_CSV,
            DownloadEscrituracaoCTE_EMITENTE_CSV,
            Relatorio,
            DownloadEmLoteConvEmails
        }
        public enum StatusThreadInfo
        {
            Aguardando = 0,
            Processando = 1,
            Concluído = 2,
            Cancelado = 3,
            Erro = 4
        }
        public enum CodigoIBGEuf
        {
            Nenhum = 0,
            Acre = 12,
            Alagoas = 27,
            Amapá = 16,
            Amazonas = 13,
            Bahia = 29,
            Ceará = 23,
            Distrito_Federal = 53,
            Espírito_Santo = 32,
            Goiás = 52,
            Maranhão = 21,
            Mato_Grosso = 51,
            Mato_Grosso_do_Sul = 50,
            Minas_Gerais = 31,
            Pará = 15,
            Paraíba = 25,
            Paraná = 41,
            Pernambuco = 26,
            Piauí = 22,
            Rio_de_Janeiro = 33,
            Rio_Grande_do_Norte = 24,
            Rio_Grande_do_Sul = 43,
            Rondônia = 11,
            Roraima = 14,
            Santa_Catarina = 42,
            São_Paulo = 35,
            Sergipe = 28,
            Tocantins = 17
        }
        public enum TiposXMLEscrituracao
        {
            NFE,
            CTE,
            NFSe,
            CTE_Emitente
        }
        public enum PF_TipoUpload
        {
            NFSe,
            Diversos
        }
        public enum EscriturarSomente
        {
            [Description("Somente notas autorizadas")]
            SomenteNotasAutorizadas,
            [Description("Somente notas canceladas")]
            SomenteNotasCanceladas
        }
        public enum TIPO_DE_OPERACAO
        {
            NFSE_PARA_CONVERSAO,
            NFE_DE_ENTRADA,
            CTE_DE_ENTRADA,
            NFSE_DE_ENTRADA,
            EMAIL_CONVERSAO
        }
        public enum ApprovalStatusEnumMIDAS
        {
            Processing = 0,
            Denied = 1,
            Returned = 2,
            Waiting = 3,
            Finished = 4,
            ProcessingInERP = 5,
            Error = 6,
            NaoIdentificado = 99
        }
        public enum ManifestacaoWS
        {
            [Description("Aguardando Processamento")]
            Aguardando,
            [Description("Processado")]
            Processado,
            [Description("Erro Reprocessar")]
            Erro_reprocessar,
            [Description("Erro")]
            Erro,
            [Description("Duplicidade")]
            Duplicidade
        }

        public enum StatusCard
        {
            [Description("Em Preenchimento")]
            EmPreenchimento,
            [Description("Finalizado")]
            Finalizado
        }

        public enum StatusExportacaoPortal
        {
            [Description("Pendente de Envio")]
            Pendente,
            [Description("Exportado")]
            Exportado,
            [Description("Erro ao Processar")]
            Erro

        }
        public enum PF_StatusFornecedor
        {
            [Description("Pendente de Ativação")]
            Pendente,
            [Description("Ativo")]
            Ativo,
            [Description("Inativo")]
            Inativo,
            [Description("Email Não Confirmado")]
            EmailNaoConfirmado
        }
        public enum PF_TipoDocDiversos
        {
            [Description("Boleto")]
            Boleto = 1,
            [Description("Fatura")]
            Fatura = 2,
            [Description("Nota de Débito")]
            Nota_de_Débito = 3,
            [Description("Reembolso")]
            Reembolso = 4,
            [Description("Outros")]
            Outros = 5
        }
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
        public enum Validacao
        {
            [Description("Falso")]
            Inativo,
            [Description("Verdadeiro")]
            Ativo
        }
        public enum TipoDesvioRecebimento
        {
            [Description("Desvia Etapa Portaria")]
            EtapaPortaria,
            [Description("Finaliza Nota")]
            FinalizaNota,
            [Description("Não se Aplica")]
            NaoSeAplica
        }
        public enum RecebimentoComplementar
        {
            [Description("Validar Pré Associação")]
            ValidarPreAssociacao,
            [Description("Validar Diversas")]
            ValidarDiversas
        }

        public enum StatusConversao
        {
            [Description("Não Iniciado")]
            NaoIniciado,
            [Description("Em Andamento")]
            EmAndamento,
            [Description("Concluído")]
            Concluido
        }
    }
}