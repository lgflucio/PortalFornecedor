using System;
using System.Collections.Generic;
using System.Text;

namespace PrefeiturasScrapping.ViewModels
{
    public class BlumenauNfseViewModel
    {
        public BlumenauNfseViewModel()
        {
            Nota = new Nota();
        }
        public Nota Nota { get; set; }
    }

    public class Nota
    {
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string Cnpj { get; set; }
        public string Inscricao_Municical { get; set; }
        public string Pre_Inscricao_Estadual { get; set; }
        public string Cd_Verificacao { get; set; }
        public string Cd_Natureza_Operacao { get; set; }
        public string Cd_Regime_Especial_Tributacao { get; set; }
        public string Sn_Optante_Simples_Nacional { get; set; }
        public string Cd_Status { get; set; }
        public string Dt_Cancelamento { get; set; }
        public string Motivo_Cancelamento { get; set; }
        public string Ar_Nfse_Substituta { get; set; }
        public string Serie_Substituta { get; set; }
        public string Dt_Competencia { get; set; }
        public string Outras_Informacoes { get; set; }
        public string Es_Cnae { get; set; }
        public string Es_Item_Lista_Servico { get; set; }
        public string Cd_Tributacao_Municipio { get; set; }
        public string Discriminacao { get; set; }
        public string Es_Municipio { get; set; }
        public string Vl_Servico { get; set; }
        public string Vl_Deducao { get; set; }
        public string Vl_Pis { get; set; }
        public string Vl_Cofins { get; set; }
        public string Vl_Inss { get; set; }
        public string Vl_Ir { get; set; }
        public string Vl_Csll { get; set; }
        public string Sn_Iss_Retido { get; set; }
        public string Vl_Iss { get; set; }
        public string Vl_Iss_Retido { get; set; }
        public string Vl_Outras_Retencoes { get; set; }
        public string Vl_Base_Calculo { get; set; }
        public string Vl_Aliquota { get; set; }
        public string Vl_Liquido_Nfse { get; set; }
        public string Vl_Desconto_Incondicionado { get; set; }
        public string Vl_Desconto_Condicionado { get; set; }
        public string Al_Pis { get; set; }
        public string Al_Cofins { get; set; }
        public string Al_Inss { get; set; }
        public string Al_Ir { get; set; }
        public string Al_Csll { get; set; }
        public string Pre_Razao_Social { get; set; }
        public string Pre_Nome_Fantasia { get; set; }
        public string Pre_Endereco { get; set; }
        public string Pre_Endereco_Numero { get; set; }
        public string Pre_Endereco_Complemento { get; set; }
        public string Pre_Endereco_Bairro { get; set; }
        public string Pre_Endereco_Uf { get; set; }
        public string Pre_Endereco_Cep { get; set; }
        public string Pre_Endereco_Es_Municipio { get; set; }
        public string Pre_Telefone { get; set; }
        public string Pre_Email { get; set; }
        public string Tom_Cpf_Cnpj { get; set; }
        public string Tom_Inscricao_Municipal { get; set; }
        public string Tom_Inscricao_Estadual { get; set; }
        public string Tom_Razao_Social { get; set; }
        public string Tom_Endereco { get; set; }
        public string Tom_Endereco_Numero { get; set; }
        public string Tom_Endereco_Complemento { get; set; }
        public string Tom_Endereco_Bairro { get; set; }
        public string Tom_Endereco_Uf { get; set; }
        public string Tom_Endereco_Cep { get; set; }
        public string Tom_Endereco_Es_Municipio { get; set; }
        public string Tom_Telefone { get; set; }
        public string Tom_Email { get; set; }
        public string Intermediario_Razao_Social { get; set; }
        public string Intermediario_Cpf_Cnpj { get; set; }
        public string Intermediario_Cd_Cpf_Cnpj_Tipo { get; set; }
        public string Intermediario_Inscricao_Municipal { get; set; }
        public string Orgao_Gerador_Es_Municipio { get; set; }
        public string Orgao_Gerador_Uf { get; set; }
        public string Construcao_Civil_Cd_Obra { get; set; }
        public string Construcao_Civil_Art { get; set; }
        public string Es_Rps { get; set; }
        public string Vl_Aliquota_Aproximada { get; set; }
        public string Vl_Aliquota_Aproximada_Total { get; set; }
        
    }
}
