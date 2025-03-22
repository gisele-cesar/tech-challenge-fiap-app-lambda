using lambda.ValidarUsuario.Interfaces;

namespace lambda.ValidarUsuario.Services
{
    public class ValidarUsuarioService : IValidarUsuarioService
    {
        public Task<bool> Validar(string documento)
        {
            return Task.FromResult(true);
        }
    }
}
