using Newtonsoft.Json;
using ReceiverApi.Interfaces;
using ReceiverApi.Responses;
using Repository.Entities.RFE_ENTITIES;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverApi.Factory
{
    public class ReceiverApiFactory : IReceiverApiFactory
    {
        public static RestClient _httpClient = new RestClient();

        public ReceiverApiFactory()
        {

        }

        public ResponseToken GetToken()
        {
            ResponseToken _responseToken = new ResponseToken();

            string _urlPath = "http://localhost:62179/token";
            _httpClient.BaseUrl = new Uri(_urlPath);
            _httpClient.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Basic dXNyd3NfcGxwOk5rdzBAMUsjTjk4");
            request.AddParameter("username", "usrws_plp");
            request.AddParameter("grant_type", "password");
            request.AddParameter("password", "Nkw0@1K#N98");
            IRestResponse response = _httpClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                _responseToken = JsonConvert.DeserializeObject<ResponseToken>(response.Content);
            }

            return _responseToken;
        }
        public ResponseNotas NfsesSent(List<RFE_NFSE> notas)
        {
            ResponseNotas _response = new ResponseNotas();
            var _jsonNotas = JsonConvert.SerializeObject(notas);
            var client = new RestClient("http://localhost:62179/RecebimentoNfsePrefeituras");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var _token = GetToken().Access_Token;
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddParameter("application/json", _jsonNotas, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                _response = JsonConvert.DeserializeObject<ResponseNotas>(response.Content);
                return _response;
            }
            else
            {
                return new ResponseNotas
                {
                    Message = "Nao Foi possível completar a solicitação",
                    Sucesso = false
                };
            }

        }
    }
}
