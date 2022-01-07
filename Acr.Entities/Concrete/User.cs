using System;
using Acr.Entities.Abstract;

namespace Acr.Entities.Concrete
{
    public class User : BaseEntity, ISoftDeletable
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public DateTime LastLoginDateTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}