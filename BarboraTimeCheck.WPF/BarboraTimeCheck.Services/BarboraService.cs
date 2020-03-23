using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public string Login(string email, string password)
        {
            var request = new RestRequest("api/eshop/v1/user/login", DataFormat.Json);
            request.AddParameter("email", email, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);
            request.AddParameter("rememberMe", "true", ParameterType.GetOrPost);

            var response = client.Post(request);

            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Error while logging into Barbora. Message: {response.Content}");
            }

            return response.Cookies.First(x => x.Name == ".BRBAUTH").Value;
        }
    }
}
