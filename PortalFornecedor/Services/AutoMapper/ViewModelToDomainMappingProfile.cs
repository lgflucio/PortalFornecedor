using AutoMapper;
using Repository.Entities.RFE_ENTITIES;
using Services.ViewModels;
using System;

namespace Services.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            //CreateMap<BaseTableViewModel, BASE_TABLE>()
            //    .IncludeAllDerived()
            //    .ForMember(entity => entity.ID, opt => opt.MapFrom(viewModel => string.IsNullOrEmpty(viewModel.Id)
            //                        ? Guid.NewGuid() : Guid.Parse(viewModel.Id)));

            //CreateMap<XmlNfseViewModel, RFE_NFSE>();
        }
    }
}
