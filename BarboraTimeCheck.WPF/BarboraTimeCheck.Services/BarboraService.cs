using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarboraTimeCheck.Services
{
    public class BarboraService
    {
        private RestClient client;
        public BarboraService()
        {
            client = new RestClient("https://www.barbora.lt/");
            client.AddDefaultHeader("Authorization", "Basic YXBpa2V5OlNlY3JldEtleQ==");
        }

        public void Login(string username, string password)
        {
            var request = new RestRequest("api/eshop/v1/user/login", DataFormat.Json);
            request.AddParameter("email", "otasssss@gmail.com", ParameterType.GetOrPost);
            request.AddParameter("password", "", ParameterType.GetOrPost);
            request.AddParameter("rememberMe", "true", ParameterType.GetOrPost);

            var response = client.Post(request);

        }
    }
}
