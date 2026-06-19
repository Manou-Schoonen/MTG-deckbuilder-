using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppMTGLogic.Database.Interfaces;
using WebAppMTGLogic.Database.Models;
using WebAppMTGModelsDL.Database.Interfaces;
using WebAppMTGModelsDL.Database.Models;
using Microsoft.AspNetCore.Identity;

namespace WebAppMTGLogic.Database.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly PasswordHasher<object> _passwordHasher;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
            _passwordHasher = new PasswordHasher<object>();
        }

        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("User id moet groter zijn dan 0.");

            var userRecord = await _userRepo.GetUserByIdAsync(id);
            if (userRecord == null)
                return null;

            return MapToModel(userRecord);
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("E-mail is verplicht.");

            var normalizedEmail = email.Trim().ToLower();
            var userRecord = await _userRepo.GetUserByEmailAsync(normalizedEmail);

            if (userRecord == null)
                return null;

            return MapToModel(userRecord);
        }

        public async Task<int> RegisterUserAsync(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Naam is verplicht.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("E-mail is verplicht.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Wachtwoord is verplicht.");

            if (password.Length < 6)
                throw new ArgumentException("Wachtwoord moet minstens 6 karakters bevatten.");

            var normalizedName = name.Trim();
            var normalizedEmail = email.Trim().ToLower();

            if (await _userRepo.EmailExistsAsync(normalizedEmail))
                throw new InvalidOperationException("Dit e-mailadres is al in gebruik.");

            var passwordHash = _passwordHasher.HashPassword(null!, password);

            return await _userRepo.CreateUserAsync(normalizedName, normalizedEmail, passwordHash);
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "E-mail is verplicht."
                };
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Wachtwoord is verplicht."
                };
            }

            var normalizedEmail = email.Trim().ToLower();
            var userRecord = await _userRepo.GetUserByEmailAsync(normalizedEmail);

            if (userRecord == null)
            {
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Ongeldige inloggegevens."
                };
            }

            var verifyResult = _passwordHasher.VerifyHashedPassword(
                null!,
                userRecord.PasswordHash,
                password
            );

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Ongeldige inloggegevens."
                };
            }

            return new LoginResult
            {
                Success = true,
                User = MapToModel(userRecord)
            };
        }

        private static UserModel MapToModel(UserRecord userRecord)
        {
            return new UserModel
            {
                Id = userRecord.Id.ToString(),
                Name = userRecord.Name,
                Email = userRecord.Email
            };
        }
    }
}
