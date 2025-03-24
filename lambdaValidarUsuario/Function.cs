using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using lambdaValidarUsuario.Interfaces;
using lambdaValidarUsuario.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Reflection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace lambdaValidarUsuario
{
    public class LambdaHandler
    {


        public async Task<APIGatewayProxyResponse> handleRequest(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogInformation("Iniciando processamento da requisição");

            context.Logger.LogInformation($"Recebendo requisição {JsonConvert.SerializeObject(request)}");

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var validarUsuario = serviceProvider.GetService<ValidarUsuarioService>();


            ///  var service = new EntidadeTesteService(new AmazonDynamoDBClient());
            var item = new EntidadeTeste
            {
                Id = Guid.NewGuid().ToString(),
                respostaTeste = "Resposta do lambda",
                request = JsonConvert.SerializeObject(request)
            };
            /// await service.Create(item);

            if (await validarUsuario.Validar("00000"))
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(item)
                };
            }
            else
                return new APIGatewayProxyResponse
                {
                    StatusCode = 401
                };
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IValidarUsuarioService, ValidarUsuarioService>();
        }
    }

    public class EntidadeTesteService : IEntidadeTesteService
    {
        private readonly IAmazonDynamoDB dynamoDB;
        private readonly string tableName = "EntidadeTeste";
        public EntidadeTesteService(IAmazonDynamoDB dynamoDB)
        {
            this.dynamoDB = dynamoDB;
        }

        public async Task Create(EntidadeTeste item)
        {

            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>()
                  {
                      { "Id", new AttributeValue { S = item.Id }},
                      { "respostaTeste", new AttributeValue { S = item.respostaTeste }},
                      { "request", new AttributeValue { S = item.request }},
                  }
            };

            await dynamoDB.PutItemAsync(request);
        }
    }

    public class EntidadeTeste
    {
        public string Id { get; set; }
        public string respostaTeste { get; set; }
        public string request { get; set; }
    }
}
