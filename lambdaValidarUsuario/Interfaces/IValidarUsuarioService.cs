using Amazon.CognitoIdentityProvider.Model;
using lambdaValidarUsuario.Model;

namespace lambdaValidarUsuario.Interfaces
{
    public interface IValidarUsuarioService
    {
        Task<SignUpResponse> Validar(UserCognito user);
    }
}
