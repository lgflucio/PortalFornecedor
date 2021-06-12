using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using Services.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Services.Services
{
    public class PortalNfseAppService : IPortalNfseAppService
    {
        private readonly IMapper _mapper;
        private readonly IPortalNfseRepository _repository;
        private readonly IPortalFornecedorRepository _repositoryFornecedor;
        private readonly IPortalNfseAnexosRepository _repositoryNfseAnexos;
        public PortalNfseAppService(IMapper mapper,
                                    IPortalNfseRepository repository,
                                    IPortalFornecedorRepository repositoryFornecedor,
                                    IPortalNfseAnexosRepository repositoryNfseAnexos)
        {
            _mapper = mapper;
            _repository = repository;
            _repositoryFornecedor = repositoryFornecedor;
            _repositoryNfseAnexos = repositoryNfseAnexos;
        }

        public List<PortalNfseViewModel> Get()
        {

            List<PortalNfseViewModel> _nfses = _repository.Query(wh => wh.ID != 0)
                                                          .ProjectTo<PortalNfseViewModel>(_mapper.ConfigurationProvider).ToList();

            _nfses.ForEach(nota =>
            {
                RFE_PORTAL_FORNECEDOR _fornec = _repositoryFornecedor.Find(f => f.CNPJ == nota.CnpjFornecedor);
                if (_fornec != null)
                    nota.RazaoSocial = _fornec.RAZAO;
            });

            return _nfses;
        }

        public PortalNfseDetalhesViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public FileContentResult DownloadFile(int idAnexo)
        {
            RFE_ANEXOS_NFSE _arquivo = _repositoryNfseAnexos.Find(f => f.ID == idAnexo);
            if (_arquivo == null)
                return null;

            byte[] _file = Convert.FromBase64String(_arquivo.FILE_BASE64);

            string contentType = _arquivo.CONTENT_TYPE;

            FileContentResult _result = new FileContentResult(_file, contentType);
            _result.FileDownloadName = _arquivo.FILE_NAME;
            return _result;
        }
    }
}
