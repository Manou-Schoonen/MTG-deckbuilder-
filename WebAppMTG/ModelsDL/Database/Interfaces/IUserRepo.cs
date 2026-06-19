using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppMTGModelsDL.Database.Models;

namespace WebAppMTGModelsDL.Database.Interfaces
{
    public interface IUserRepo
    {
        Task<UserRecord?> GetUserByEmailAsync(string email);
        Task<UserRecord?> GetUserByIdAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<int> CreateUserAsync(string name, string email, string passwordHash);
    }
}
