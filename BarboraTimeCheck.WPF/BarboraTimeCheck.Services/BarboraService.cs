using BarboraTimeCheck.Services.Models;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BarboraService()
        {
            settingsService = new SettingsService();
            client = new RestClient("https://barbora.lt/");
            client.AddDefaultHeader("Authorization", "Basic YXBpa2V5OlNlY3JldEtleQ==");
        }

        public string Login(string email, string password)
        {
            log.Info("Starting login");
            var request = new RestRequest("api/eshop/v1/user/login", DataFormat.Json);
            request.AddParameter("email", email, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);
            request.AddParameter("rememberMe", "true", ParameterType.GetOrPost);
            request.AddParameter("region", "barbora.lt", ParameterType.Cookie);

            var response = client.Post(request);

            log.Info($"Login response data: {response.Content}");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                log.Error($"Error while logging in. Used parameters: {email} {password}");
                throw new Exception($"Error while logging into Barbora. Message: {response.Content}");
            }

            var brAuthCookie = response.Cookies.First(x => x.Name == ".BRBAUTH");
            log.Error($".BRBAUTH Cookie: {brAuthCookie.Value}");
            return brAuthCookie.Value;
        }

        public DeliveriesResult GetAvailableDeliveries()
        {
            log.Info("Starting to get deliveries");
            var request = new RestRequest("api/eshop/v1/cart/deliveries", DataFormat.Json);
            request.AddParameter(".BRBAUTH", settingsService.GetAuthCookie(), ParameterType.Cookie);
            request.AddParameter("region", "barbora.lt", ParameterType.Cookie);

            var response = client.Get<Deliveries>(request);

            log.Info($"Deliveries response: {response.Content}");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                log.Error($"Error getting deliveries. AuthCookie: {settingsService.GetAuthCookie()}");
                throw new Exception($"Error while getting deliveries. Message: {response.Content}");
            }

            var deliveryHours = response.Data
                                              .deliveries
                                              .SelectMany(x => x.@params.matrix)
                                              .SelectMany(x => x.hours);

            var availableDeliveries = deliveryHours
                                                .Where(x => x.available)
                                                .ToList();

            log.Info($"Deliveries found: {deliveryHours.Count()}");
            log.Info($"Available deliveries: {availableDeliveries.Count}");
            return new DeliveriesResult
            {
                AvailableDeliveries = availableDeliveries,
                TotalDeliveries = deliveryHours.Count()
            };
        }
    }
}
