using BarboraTimeCheck.Services.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarboraTimeCheck.Services
{
    public class BarboraService
    {
        private RestClient client;
        private SettingsService settingsService;
        public BarboraService()
        {
            settingsService = new SettingsService();
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

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Error while logging into Barbora. Message: {response.Content}");
            }

            return response.Cookies.First(x => x.Name == ".BRBAUTH").Value;
        }

        public DeliveriesResult GetAvailableDeliveries()
        {
            var request = new RestRequest("api/eshop/v1/cart/deliveries", DataFormat.Json);
            request.AddParameter(".BRBAUTH", settingsService.GetAuthCookie(), ParameterType.Cookie);

            var response = client.Get<Deliveries>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Error while getting deliveries. Message: {response.Content}");
            }

            var deliveryHours = response.Data
                                              .deliveries
                                              .SelectMany(x => x.@params.matrix)
                                              .SelectMany(x => x.hours);

            var availableDeliveries = deliveryHours
                                                .Where(x => x.available)
                                                .ToList();
            return new DeliveriesResult
            {
                AvailableDeliveries = availableDeliveries,
                TotalDeliveries = deliveryHours.Count()
            };
        }
    }
}
