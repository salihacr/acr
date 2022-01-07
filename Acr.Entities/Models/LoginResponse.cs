using System;

namespace Acr.Entities.Models
{
    public class LoginResponse : Token
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
    }
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
    }
}