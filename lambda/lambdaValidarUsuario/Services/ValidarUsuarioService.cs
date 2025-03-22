using lambdaValidarUsuario.Interfaces;

namespace lambdaValidarUsuario.Services
{
    public class ValidarUsuarioService : IValidarUsuarioService
    {
        public Task<bool> Validar(string documento)
        {
            return Task.FromResult(true);
        }
    }
}
