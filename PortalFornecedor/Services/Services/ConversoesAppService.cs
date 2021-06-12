using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using Services.Interfaces;
using System;
using static Shared.Enums.Enums;

namespace Services.Services
{
    public class ConversoesAppService : IConversoesAppService
    {
        private readonly IConversoesRepository _repository;
        public ConversoesAppService(IConversoesRepository repository)
        {
            _repository = repository;
        }
        public string FilaConversao(int id)
        {
            var _jaEstaNaFilaDeConversao = _repository.Find(f => f.ID_NFSE == id);
            if (_jaEstaNaFilaDeConversao != null)
                return $"Está nota já foi enviada para conversão - número de protocolo: {_jaEstaNaFilaDeConversao.PROTOCOLO}";

            string _dataProtocolo = DateTime.Now.ToString("yyMMddHHmm");
            Random _numRandom = new Random();
            string _random1 = _numRandom.Next(100000, 999999).ToString();
            string _random2 = _numRandom.Next(0, 100000).ToString().PadLeft(6, '0');
            string _protocolo = string.Format("{0}{1}{2}", _dataProtocolo, _random1, _random2);

            RFE_PORTAL_CONVERSOES _conversao = new RFE_PORTAL_CONVERSOES();
            _conversao.ID_NFSE = id;
            _conversao.MODIFICADO = DateTime.Now;
            _conversao.PROTOCOLO = _protocolo;
            _conversao.STATUS = StatusConversao.NaoIniciado;
            _conversao.EDITOR = "GSW";

            _repository.Create(_conversao);

            if (_conversao.ID != 0)
                return $"{_conversao.PROTOCOLO}";
            else
                return "Erro";
        }
    }
}
