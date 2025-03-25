using Amazon.Lambda.Core;
using lambdaValidarUsuario.Interfaces;

namespace lambdaValidarUsuario.Services
{
    public class ValidarUsuarioService : IValidarUsuarioService
    {
        public Task<bool> Validar(string documento)
        {
           // _context.Logger.LogInformation("Validando o documento");
            return Task.FromResult(true);
        }
    }
}
