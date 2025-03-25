using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using lambdaValidarUsuario.Interfaces;
using lambdaValidarUsuario.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace lambdaValidarUsuario
{
    public class LambdaHandler
    {
        private readonly ServiceProvider _serviceProvider;
        public LambdaHandler()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public async Task<APIGatewayProxyResponse> handleRequest(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                context.Logger.LogInformation("Iniciando processamento da requisição");

                context.Logger.LogInformation($"Recebendo requisição {JsonSerializer.Serialize(request)}");

                using var scope = _serviceProvider.CreateScope();
                var validarUsuario = scope.ServiceProvider.GetRequiredService<IValidarUsuarioService>();
                var item = new EntidadeTeste
                {
                    Id = Guid.NewGuid().ToString(),
                    respostaTeste = "Resposta do lambda",
                    request = JsonSerializer.Serialize(request)
                };

                if (await validarUsuario.Validar("00000"))
                {
                    context.Logger.LogInformation("ok");

                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 200,
                        Body = JsonSerializer.Serialize(request)
                    };
                }
                else
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 401
                    };
                }
            }
            catch(Exception ex)
            {
                context.Logger.LogError(ex, $"Erro na execução : {ex.Message}");

                return new APIGatewayProxyResponse
                {
                    StatusCode = 503,
                    Body = JsonSerializer.Serialize(request)
                };
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {

            serviceCollection.AddLogging();
            serviceCollection.AddScoped<IValidarUsuarioService, ValidarUsuarioService>();
        }

    //    public static void SetupLogger(bool isDevelopment, ILoggingBuilder logging, IConfiguration configuration)
    //    {
    //        if (logging == null)
    //        {
    //            throw new ArgumentNullException(nameof(logging));
    //        }

    //        // Create and populate LambdaLoggerOptions object
    //        var loggerOptions = = new LambdaLoggerOptions
    //        {
    //            IncludeCategory = true,
    //            IncludeLogLevel = true,
    //            IncludeNewline = true,
    //            IncludeEventId = true,
    //            IncludeException = true
    //        };

    //        // Configure Lambda logging
    //        logging.AddLambdaLogger(loggerOptions);
    //        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
    //    }
   }

    public class EntidadeTeste
    {
        public string Id { get; set; }
        public string respostaTeste { get; set; }
        public string request { get; set; }
    }
}
