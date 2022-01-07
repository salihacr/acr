using Acr.Business.Abstract;
using Acr.Core.Helpers;
using Acr.Core.Models.Request;
using Acr.Core.Result;
using Acr.DataAccess.Abstract;
using Acr.Entities.Concrete;
using Acr.Entities.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Acr.Business.Concrete
{
    public class AuthManager : BaseManager<User>, IAuthManager
    {
        public AuthManager(IRepository<User> repository, ILogRepository logRepository) : base(repository, logRepository)
        {
        }
        public TaskResult CreateUser(User entity, int userId)
        {
            entity.IsActive = true;
            entity.Password = entity.Password.ComputeSHA256();
            return base.Add(entity, userId);
        }
        public TaskResult UpdateUser(User entity, int userId)
        {
            entity.Password = entity.Password.ComputeSHA256();
            return base.Update(entity, userId);
        }

        public TaskResult Login(LoginRequest loginRequest)
        {
            var user = _repository.Get(x => (x.Email == loginRequest.UsernameOrEmail || x.Username == loginRequest.UsernameOrEmail) && x.Password == loginRequest.Password.ComputeSHA256());
            if (user == null)
                return TaskResult.NotFound("Username or password incorrect");
            else
            {
                var token = CreateToken(user);
                var response = new LoginResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    Expiration = token.Expiration
                };
                return DataResult<LoginResponse>.Successful(response);
            }
        }

        private Token CreateToken(User user)
        {
            string secret = Constant.SecretString;
            byte[] key = Encoding.ASCII.GetBytes(secret);

            var tokenModel = new Token();
            tokenModel.Expiration = DateTime.Now.AddHours(10);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("userId", user.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Username),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.Now.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(securityToken);

            tokenModel.AccessToken = tokenString;
            tokenModel.RefreshToken = CreateRefreshToken();
            return tokenModel;
        }
        private string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }
    }
}