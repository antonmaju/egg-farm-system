using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class Account : Entity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public int RoleType { get; set; }          
    }

    public enum RoleType
    {
        User,
        Admin
    }
}
