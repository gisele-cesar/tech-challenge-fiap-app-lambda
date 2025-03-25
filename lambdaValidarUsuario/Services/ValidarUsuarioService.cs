using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using lambdaValidarUsuario.Interfaces;
using lambdaValidarUsuario.Model;

namespace lambdaValidarUsuario.Services
{
    public class ValidarUsuarioService : IValidarUsuarioService
    {
        private readonly IAmazonCognitoIdentityProvider _amazonCognitoIdentityProvider;
        public ValidarUsuarioService(IAmazonCognitoIdentityProvider amazonCognitoIdentityProvider)
        {
            _amazonCognitoIdentityProvider = amazonCognitoIdentityProvider;
        }

        public async Task<SignUpResponse> Validar(UserCognito user)
        {
            var userSignUpRequest = new SignUpRequest
            {
                ClientId = "your_client_id",
                Password = user.Password,
                Username = user.Email
            };

            var attributeNickname = new AttributeType
            {
                Name = "nickname",
                Value = user.NickName
            };
            userSignUpRequest.UserAttributes.Add(attributeNickname);

            var attribute = new AttributeType
            {
                Name = "custom:UserRole",
                /// Value = user.Role
            };
            userSignUpRequest.UserAttributes.Add(attribute);

            var userCreated = await _amazonCognitoIdentityProvider.SignUpAsync(userSignUpRequest);

            return userCreated;
        }
    }

   
}
