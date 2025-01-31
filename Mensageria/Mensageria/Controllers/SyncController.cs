using MensageriaDominio;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Http;

namespace Mensageria.Controllers
{
    public class SyncController : ApiController
    {
        private readonly RedisService _redisService;

        public SyncController()
        {
            var connectionString = ConfigurationManager.AppSettings["Redis:ConnectionString"];
            var queueName = ConfigurationManager.AppSettings["Redis:QueueName"];

            _redisService = new RedisService(connectionString);
        }

        [HttpPost]
        [Route("api/somar")]
        public IHttpActionResult SomarValores([FromBody] SumMessage message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);

            _redisService.PublishMessage("fila_somar", jsonMessage);

            return Ok($"Valores '{message.Value1}' e '{message.Value2}' enviados para processamento.");
        }

        [HttpPost]
        [Route("api/multiplicar")]
        public IHttpActionResult MultiplicarValores([FromBody] SumMessage message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);

            _redisService.PublishMessage("fila_multiplicar", jsonMessage);

            return Ok($"Valores '{message.Value1}' e '{message.Value2}' enviados para processamento.");
        }
    }
}