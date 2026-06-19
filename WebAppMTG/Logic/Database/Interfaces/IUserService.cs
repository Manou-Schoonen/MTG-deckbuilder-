using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppMTGLogic.Database.Models;

namespace WebAppMTGLogic.Database.Interfaces
{
    public interface IUserService
    {
        Task<UserModel?> GetUserByIdAsync(int id);
        Task<UserModel?> GetUserByEmailAsync(string email);
        Task<int> RegisterUserAsync(string name, string email, string password);
        Task<LoginResult> LoginAsync(string email, string password);
    }
}
