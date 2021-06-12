using Microsoft.Extensions.Hosting;

public interface IMyHostedService : IHostedService
{
    void DoWork();
}