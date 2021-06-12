using AutoMapper;
using Repository.Entities.PREFEITURAS_ENTITIES;
using Repository.Entities.RFE_ENTITIES;
using Services.Interfaces;
using Services.ViewModels;
using System.Linq;
using Utils;

namespace Services.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<BASE_TABLE, BaseTableViewModel>()
                .IncludeAllDerived()
                .ForMember(viewmodel => viewmodel.Id, opt =>
                        opt.MapFrom(entity => entity.ID.ToString()));

            //        CreateMap<Entity, EntityViewModel>()
            //            .IncludeAllDerived()
            //            .ForMember(viewmodel => viewmodel.Id, opt =>
            //                    opt.MapFrom(entity => entity.Id.ToString()));

            //        CreateMap<RFE_CIA, CiaViewModel>()
            //            .ForMember(vm => vm.DescricaoCia, opt => opt.MapFrom(entity => entity.DESCRICAO));

            //        CreateMap<Municipios, MunicipiosViewModel>();

            //        CreateMap<TagsXmls, TagsXmlsViewModel>();

            CreateMap<RFE_PORTAL_HEADER_NOTA, PortalNfseViewModel>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(entity => entity.ID))
                .ForMember(vm => vm.CnpjFornecedor, opt => opt.MapFrom(entity => entity.CNPJ_FORNECEDOR))
                .ForMember(vm => vm.Protocolo, opt => opt.MapFrom(entity => entity.PROTOCOLO))
                .ForMember(vm => vm.NumeroNota, opt => opt.MapFrom(entity => entity.NUMERO_NOTA))
                .ForMember(vm => vm.Serie, opt => opt.MapFrom(entity => entity.SERIE))
                .ForMember(vm => vm.Arquivo, opt => opt.MapFrom(entity => entity.FILE_NAME))
                .ForMember(vm => vm.StatusProtocolo, opt => opt.MapFrom(entity => entity.STATUS_PROTOCOLO))
                .ForMember(vm => vm.DataUpload, opt => opt.MapFrom(entity => entity.DT_UPLOAD))
                .ForMember(vm => vm.DataEmissao, opt => opt.MapFrom(entity => entity.DT_EMISSAO))
                .ForMember(vm => vm.DataPreenchimento, opt => opt.MapFrom(entity => entity.DT_PREENCHIMENTO_FINALIZADO))
                .ForMember(vm => vm.Pedido, opt => opt.MapFrom(entity => entity.ITENS_NOTA.FirstOrDefault().NUMERO_OC))
                .ForMember(vm => vm.Linha, opt => opt.MapFrom(entity => entity.ITENS_NOTA.FirstOrDefault().LINHA_OC.ToString()))
                .ForMember(vm => vm.IdAnexo, opt => opt.MapFrom(entity => entity.ID_ANEXO_NFSE));


            //        CreateMap<RFE_ITEM_NFSE, NfseItemViewModel>()
            //            .ForMember(vm => vm.CodigoServico, opt => opt.MapFrom(entity => entity.CODIGO_SERVICO))
            //            .ForMember(vm => vm.Discriminacao, opt => opt.MapFrom(entity => entity.DESCRICAO_SERVICO))
            //            .ForMember(vm => vm.QuantidadeServico, opt => opt.MapFrom(entity => entity.QTD_ITEM_SERVICO))
            //            .ForMember(vm => vm.Tributado, opt => opt.MapFrom(entity => entity.TRIBUTADO))
            //            .ForMember(vm => vm.ValorDesconto, opt => opt.MapFrom(entity => entity.VALOR_DESCONTO))
            //            .ForMember(vm => vm.ValorTotal, opt => opt.MapFrom(entity => entity.VALOR_TOTAL))
            //            .ForMember(vm => vm.ValorUnitario, opt => opt.MapFrom(entity => entity.VALOR_UNITARIO));

            ////---------------------------------------------------------------------------------------------------------------------------------------------------------
            //        CreateMap<RFE_NFSE, NfseDetalhesViewModel>()
            //            .ForMember(vm => vm.DataEmissao, opt => opt.MapFrom(entity => entity.DHEMI))
            //            .ForMember(vm => vm.DataEntradaRepositorio, opt => opt.MapFrom(entity => entity.DT_ENTRADA_REPOSIT))
            //            .ForMember(vm => vm.CodigoServico, opt => opt.MapFrom(entity => entity.Itens.FirstOrDefault().CODIGO_SERVICO))
            //            .ForMember(vm => vm.Estabelecimento, opt => opt.MapFrom(entity => entity.TOMADORCNPJ + " - " + entity.TOMADORRAZAO))
            //            .ForMember(vm => vm.Fornecedor, opt => opt.MapFrom(entity => entity.PRESTADORCNPJ + " - " + entity.PRESTADORRAZAO))
            //            .ForMember(vm => vm.Numero, opt => opt.MapFrom(entity => entity.NUNFSE))
            //            .ForMember(vm => vm.ValorTotal, opt => opt.MapFrom(entity => entity.Itens.FirstOrDefault().VALOR_TOTAL))
            //            .ForMember(vm=>vm.Aliquota, opt=>opt.MapFrom(entity => entity.ALIQUOTA))
            //            .ForMember(vm=>vm.Competencia, opt=>opt.MapFrom(entity => entity.DHEMI.Value.ToString("MM/yyyy")))
            //            .ForMember(vm=>vm.IssRetido, opt=>opt.MapFrom(entity => entity.ISSRETIDO))
            //            .ForMember(vm=>vm.MunicipioGerador, opt=>opt.MapFrom(entity => entity.MUNGER))
            //            .ForMember(vm=>vm.NaturezaOperacao, opt=>opt.MapFrom(entity => entity.NATUREZAOPERACAO))
            //            .ForMember(vm=>vm.OptanteSimples, opt=>opt.MapFrom(entity => entity.OPTANTESIMPLES))
            //            .ForMember(vm=>vm.RegimeEspecialTributacao, opt=>opt.MapFrom(entity => entity.REGIMEESPECIALTRIBUTACAO))
            //            .ForMember(vm=>vm.Serie, opt=>opt.MapFrom(entity => entity.SERIENFSE))
            //            .ForMember(vm=>vm.Tipo, opt=>opt.MapFrom(entity => entity.TIPO))
            //            .ForMember(vm=>vm.ValorBaseCalculo, opt=>opt.MapFrom(entity => entity.BASECALCULO))
            //            .ForMember(vm=>vm.ValorCofins, opt=>opt.MapFrom(entity => entity.VALORCOFINS))
            //            .ForMember(vm=>vm.ValorCsll, opt=>opt.MapFrom(entity => entity.VALORCSLL))
            //            .ForMember(vm=>vm.ValorDeducoes, opt=>opt.MapFrom(entity => entity.VALORDEDUCOES))
            //            .ForMember(vm=>vm.ValorDesconto, opt=>opt.MapFrom(entity => entity.VALOR_DESCONTO))
            //            .ForMember(vm=>vm.ValorInss, opt=>opt.MapFrom(entity => entity.VALORINSS))
            //            .ForMember(vm=>vm.ValorIrr, opt=>opt.MapFrom(entity => entity.VALORIR))
            //            .ForMember(vm=>vm.ValorIss, opt=>opt.MapFrom(entity => entity.VALORISS))
            //            .ForMember(vm=>vm.ValorLiquido, opt=>opt.MapFrom(entity => entity.VALORLIQUIDO))
            //            .ForMember(vm=>vm.ValorOutrasRetencoes, opt=>opt.MapFrom(entity => entity.OUTRASRETENCOES))
            //            .ForMember(vm=>vm.ValorPis, opt=>opt.MapFrom(entity => entity.VALORPIS))
            //            .ForMember(vm=>vm.ValorServicos, opt=>opt.MapFrom(entity => entity.VALORSERVICO))
            //            .ForMember(vm => vm.Itens, opt => opt.MapFrom(entity => entity.Itens));
        }
    }
}
