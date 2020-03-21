using BarboraLaikas.Models;
using RestSharp;
using System;
using System.Linq;
using System.Timers;
using System.Configuration;
using BarboraLaikas.Services;
using System.Text;

namespace BarboraLaikas
{
    class Program
    {
        private static IEmailService emailService;
        private static Timer timer;
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Barbora delivery time checker");
            emailService = new EmailService();

            CheckDeliveries();

            StartTimer();

            Console.ReadLine();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer elapsed. Checking");
            CheckDeliveries();
        }

        private static void StartTimer()
        {
            timer = new Timer();

            var interval = int.Parse(ConfigurationManager.AppSettings["interval"]);

            timer.Interval = interval * 1000;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;

            timer.Enabled = true;
        }

        public static void CheckDeliveries()
        {
            var client = new RestClient("https://www.barbora.lt/");
            var request = new RestRequest("api/eshop/v1/cart/deliveries", DataFormat.Json);

            request.AddParameter("Authorization", ConfigurationManager.AppSettings["authorizationToken"], ParameterType.HttpHeader);
            request.AddParameter(".BRBAUTH", ConfigurationManager.AppSettings["authCookie"], ParameterType.Cookie);

            var response = client.Get<Deliveries>(request);

            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Network error");
                return;
            }

            var availableDeliveries = response.Data
                .deliveries
                .SelectMany(x => x.@params.matrix)
                .SelectMany(x => x.hours)
                .Where(x => x.available);

            if (!availableDeliveries.Any())
            {
                Console.WriteLine("No available times");
            }
            else
            {
                var deliveryText = new StringBuilder();
                deliveryText.AppendLine("Available delivery times:");
                foreach (var time in availableDeliveries)
                {
                    Console.WriteLine($"Available delivery time: {time.deliveryTime}");
                    deliveryText.AppendLine($"Available delivery time: {time.deliveryTime}");
                }
                deliveryText.AppendLine("Thank you for using Barbora Delivery time checker");

                emailService.SendEmail("Delivery", deliveryText.ToString());
            }
        }
    }

}



