using Repository.Entities.RFE_ENTITIES;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Shared.Enums.Enums;

namespace Services.ViewModels
{
    public class PortalNfseViewModel
    {
        public PortalNfseViewModel()
        {
            //ITENS_NOTA = new List<RFE_PORTAL_ITEM_NOTA>();
        }
        public int Id { get; set; }
        public string CnpjFornecedor { get; set; }
        public string RazaoSocial { get; set; }
        public string Pedido { get; set; }
        public string Linha { get; set; }
        public string Protocolo { get; set; }
        public long NumeroNota { get; set; }
        public string Serie { get; set; }
        public string Arquivo { get; set; }
        public int IdAnexo { get; set; }
        public StatusProtocolo StatusProtocolo { get; set; }
        public DateTimeOffset DataUpload { get; set; }
        public DateTimeOffset? DataEmissao { get; set; }
        public DateTimeOffset? DataPreenchimento { get; set; }
    }
}
