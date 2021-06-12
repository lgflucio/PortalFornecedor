using BreakCaptcha.DTO;
using BreakCaptcha.Interfaces;
using BreakCaptcha.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BreakCaptcha.Factory
{
    public class BreakCaptchaFactory : IBreakCaptchaFactory
    {
        public static HttpClient _httpClient = new HttpClient();

        public BreakCaptchaFactory()
        {
            string urlPath = "https://api-ml-lb.gswapp.com/";
            _httpClient.BaseAddress = new Uri(urlPath);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
        }
        public async Task<string> Authenticate()
        {

            ResponseLogin _responseLogin = new ResponseLogin();

            User _user = AuthenticateCaptcha.Authenticate("gswusr", "fggkOu46H7hs#4tgfd@xcvwo");

            string _userSerialized = JsonConvert.SerializeObject(_user);

            var _request = new HttpRequestMessage(HttpMethod.Post, "account/login");
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _request.Content = new StringContent(_userSerialized);
            _request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var _response = await _httpClient.SendAsync(_request);

            if (_response.StatusCode == HttpStatusCode.OK)
            {
                var content = await _response.Content.ReadAsStringAsync();
                _responseLogin = JsonConvert.DeserializeObject<ResponseLogin>(content);

                if (!string.IsNullOrEmpty(_responseLogin.Token))
                    return _responseLogin.Token;
                else
                {
                    return "Autenticacao falhou, não foi gerado token para requisição.";
                }
            }
            else
            {
                return $"Erro ao tentar fazer a Autenticacao da API: {_response.StatusCode.ToString()}";
            }

        }
        public async Task<string> CaptchaProcessing(string img)
        {

            ResponseProcess _responseProcess = new ResponseProcess();
            try
            {
                string token = await Authenticate();

                CaptchaProcessing _captchaProcess = new CaptchaProcessing()
                {
                    imgBase64 = img
                };

                string _base64Serialized = JsonConvert.SerializeObject(_captchaProcess);

                var _request = new HttpRequestMessage(HttpMethod.Post, "CaptchaProcessing");
                _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _request.Content = new StringContent(_base64Serialized);
                _request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var _response = await _httpClient.SendAsync(_request);

                if (_response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = await _response.Content.ReadAsStringAsync();
                    _responseProcess = JsonConvert.DeserializeObject<ResponseProcess>(content);
                    return _responseProcess.IdInserted;
                }
                else
                {
                    return $"Erro ao enviar img para quebrar captcha: {_response.StatusCode.ToString()}";
                }

                if (_response.StatusCode != System.Net.HttpStatusCode.OK && _response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception($"CaptchaProcessing => statusCode: {_response.StatusCode.ToString()}");

                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseSolved> CaptchaSolved(string guid)
        {
            ResponseSolved _responseSolved = new ResponseSolved();
            try
            {
                string token = await Authenticate();

                CaptchaSolved _captchaSolved = new CaptchaSolved()
                {
                    id = guid
                };

                string _idSerialized = JsonConvert.SerializeObject(_captchaSolved);

                var _request = new HttpRequestMessage(HttpMethod.Post, "CaptchaProcessing/CheckIfSolved");
                _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _request.Content = new StringContent(_idSerialized);
                _request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var _response = await _httpClient.SendAsync(_request);

                if (_response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = await _response.Content.ReadAsStringAsync();
                    _responseSolved = JsonConvert.DeserializeObject<ResponseSolved>(content);
                    if (string.IsNullOrEmpty(_responseSolved.captchaText))
                        _responseSolved.captchaSolved = false;
                }

                if (_response.StatusCode != System.Net.HttpStatusCode.OK && _response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception($"CaptchaSolved => statusCode: {_response.StatusCode.ToString()}");

                return _responseSolved;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
