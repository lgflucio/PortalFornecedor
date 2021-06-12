using PrefeiturasScrapping.Interfaces;
using RaspagemPrefeituras.ArquivosNFSe.Interfaces;
using Repository.Entities.RFE_ENTITIES;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace RaspagemPrefeituras.ArquivosNFSe
{
    public class ProcessarArquivosNFSe : IProcessarArquivosNFSe
    {
        private readonly IPageRJComponenteRepository _pageRJComponenteRepository;
        private List<RFE_REPOSITORIO> Repositorios = new List<RFE_REPOSITORIO>();

        public ProcessarArquivosNFSe()
        {
        }

        public ProcessarArquivosNFSe(IPageRJComponenteRepository pageRJComponenteRepository)
        {
            _pageRJComponenteRepository = pageRJComponenteRepository;
        }

        public void ProcessarNFSEBaixadas(string caminho)
        {
            string[] arquivos = Directory.GetFiles(caminho);
            RFE_REPOSITORIO repositorio;

            foreach (var arquivo in arquivos)
            {
                var xml = arquivo.CarregarXML();
                repositorio = new RFE_REPOSITORIO(xml);
                Repositorios.Add(repositorio);
                Console.WriteLine("Leitura do arquivo realizada..." + arquivo.PathName() + DateTime.Now.ToString());
            }

            _pageRJComponenteRepository.CreateRange(Repositorios);
            _pageRJComponenteRepository.Save();

            var a = 0;
        }

    }
}
