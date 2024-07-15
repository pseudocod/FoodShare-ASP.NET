using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using FoodShareApp.Models;
using System.Text.Json;
using System.IO;


namespace FoodShareApp
{
    public static class Function1

    {
        private const string CosmosConnectionString = "AccountEndpoint=https://informatica-db.documents.azure.com:443/;AccountKey=tsUTl3n97hjiAPqn3U9nYS4k9Z4NRMTsqhnUAERq3tKT1O0sqUiiQsUco58ZrZOCgemlF6CasOYcACDbu2uTrA==;";
        private const string dbName = "DHL";
        private const string containerName = "Delivery";
        [FunctionName("NewDeliveryFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                CosmosClient client = new CosmosClient(CosmosConnectionString);
                var database = client.GetDatabase(dbName);
                var container = database.GetContainer(containerName);


                //string body = await new StreamReader(req.Body).ReadToEndAsync();

                //DeliveryModel delivery = JsonSerializer.Deserialize<DeliveryModel>(body);

                //container.CreateItemAsync(delivery, new PartitionKey(delivery.city));
                DeliveryModel delivery = new DeliveryModel(true);


                await container.CreateItemAsync(delivery);



                return new OkObjectResult("Success");
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult("Error");
            }







        }
    }
}
