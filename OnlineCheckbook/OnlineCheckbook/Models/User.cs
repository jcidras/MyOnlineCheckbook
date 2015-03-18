using System;
using System.Collections.Generic;

namespace OnlineCheckbook.Models
{
    public class User
    {
        // Primary Key
        public int? UserID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Bank> Banks { get; set; }
    }
}
