namespace lambdaValidarUsuario.Interfaces
{
    public interface IValidarUsuarioService
    {
        Task<bool> Validar(string documento);
    }
}
