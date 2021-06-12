namespace PrefeiturasScrapping.Interfaces
{
    public interface IRioDeJaneiroAppService
    {
        void Start(int periodoConsulta, string diretorioDownload = "");
    }
}
