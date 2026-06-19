using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppMTGLogic.Database.Models
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public UserModel? User { get; set; }
    }
}
