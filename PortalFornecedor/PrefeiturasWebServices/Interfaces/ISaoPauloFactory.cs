using System;

namespace PrefeiturasWebServices.Interfaces
{
    public interface ISaoPauloFactory
    {
        string DownloadXmlNfseSp(string cnpjEstabelecimento, string im, DateTime dtInicio, DateTime dtFim, int numeroPagina);
    }
}
