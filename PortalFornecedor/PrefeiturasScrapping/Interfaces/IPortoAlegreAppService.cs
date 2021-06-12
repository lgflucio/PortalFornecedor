using Services.ViewModels;

namespace PrefeiturasScrapping.Interfaces
{
    public interface IPortoAlegreAppService
    {
        public SmtpConfigViewModel GetConfigEmailBox();
        public void Get(int v);
    }
}
