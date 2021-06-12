namespace PrefeiturasWebServices.ViewModels.SaoPaulo
{
    public class SaoPauloNfseViewModel
    {
        public NFe NFe { get; set; }
    }
    public class NFe
    {
        public NFe()
        {
            ChaveNFe = new ChaveNFe();
            ChaveRPS = new ChaveRPS();
            EnderecoPrestador = new EnderecoPrestador();
            CPFCNPJPrestador = new CPFCNPJPrestador();
            CPFCNPJTomador = new CPFCNPJTomador();
            EnderecoTomador = new EnderecoTomador();
        }
        public string XmlOriginal { get; set; }
        public ChaveNFe ChaveNFe { get; set; }
        public ChaveRPS ChaveRPS { get; set; }
        public EnderecoPrestador EnderecoPrestador { get; set; }
        public CPFCNPJPrestador CPFCNPJPrestador { get; set; }
        public CPFCNPJTomador CPFCNPJTomador { get; set; }
        public EnderecoTomador EnderecoTomador { get; set; }
        public string DataEmissaoNFe { get; set; }
        public string NumeroLote { get; set; }
        public string TipoRPS { get; set; }
        public string DataEmissaoRPS { get; set; }
        public string NumeroGuia { get; set; }
        public string DataQuitacaoGuia { get; set; }
        public string RazaoSocialPrestador { get; set; }
        public string EmailPrestador { get; set; }
        public string StatusNFe { get; set; }
        public string TributacaoNFe { get; set; }
        public string OpcaoSimples { get; set; }
        public string ValorServicos { get; set; }
        public string ValorPIS { get; set; }
        public string ValorCOFINS { get; set; }
        public string ValorIR { get; set; }
        public string ValorCSLL { get; set; }
        public string CodigoServico { get; set; }
        public string AliquotaServicos { get; set; }
        public string ValorISS { get; set; }
        public string ValorCredito { get; set; }
        public string ISSRetido { get; set; }
        public string RazaoSocialTomador { get; set; }
        public string Discriminacao { get; set; }
        public string ValorCargaTributaria { get; set; }
        public string PercentualCargaTributaria { get; set; }
        public string FonteCargaTributaria { get; set; }
    }
    public class ChaveNFe
    {
        public string InscricaoPrestador { get; set; }
        public string NumeroNFe { get; set; }
        public string CodigoVerificacao { get; set; }
    }
    public class EnderecoPrestador
    {
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string NumeroEndereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string ComplementoEndereco { get; set; }
    }
    public class CPFCNPJPrestador
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }

    }
    public class CPFCNPJTomador
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }
    }
    public class EnderecoTomador
    {
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string NumeroEndereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }

    }

    public class ChaveRPS
    {
        public string InscricaoPrestador { get; set; }
        public string SerieRPS { get; set; }
        public string NumeroRPS { get; set; }
    }
}
