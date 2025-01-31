using MensageriaDominio;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Threading;

namespace MensageriaWorker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.AppSettings["Redis:ConnectionString"];
            var redisService = new RedisService(connectionString);

            var worker1 = new Thread(() => RunWorker1(redisService));
            var worker2 = new Thread(() => RunWorker2(redisService));

            worker1.Start();
            worker2.Start();

            Console.WriteLine("Workers iniciados. Pressione Enter para sair.");
            Console.ReadLine();
        }

        private static void RunWorker1(RedisService redisService)
        {
            Console.WriteLine("Worker 1 aguardando mensagens...");

            while (true)
            {
                var message = redisService.ConsumeMessage("fila_somar");
                if (!string.IsNullOrEmpty(message))
                {
                    var jsonString = JsonConvert.DeserializeObject<string>(message);

                    var service1Message = JsonConvert.DeserializeObject<SumMessage>(jsonString);
                    var result = service1Message.Value1 + service1Message.Value2;
                    Console.WriteLine($"Worker 1 processou: {service1Message.Value1} + {service1Message.Value2} = {result}");
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static void RunWorker2(RedisService redisService)
        {
            Console.WriteLine("Worker 2 aguardando mensagens...");

            while (true)
            {
                var message = redisService.ConsumeMessage("fila_multiplicar");
                if (!string.IsNullOrEmpty(message))
                {
                    var jsonString = JsonConvert.DeserializeObject<string>(message);

                    var service2Message = JsonConvert.DeserializeObject<SumMessage>(jsonString);
                    var result = service2Message.Value1 * service2Message.Value2;
                    Console.WriteLine($"Worker 2 processou: {service2Message.Value1} * {service2Message.Value2} = {result}");
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}