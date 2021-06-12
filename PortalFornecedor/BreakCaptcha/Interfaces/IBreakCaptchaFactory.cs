using BreakCaptcha.Responses;
using System.Threading.Tasks;

namespace BreakCaptcha.Interfaces
{
    public interface IBreakCaptchaFactory
    {
        public Task<string> CaptchaProcessing(string img);
        public Task<ResponseSolved> CaptchaSolved(string guid);
    }
}
