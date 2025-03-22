namespace lambda.ValidarUsuario.Interfaces
{
    public interface IValidarUsuarioService
    {
        Task<bool> Validar(string documento);
    }
}
