using API.DTOs;
using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Param;
using Google.Apis.Auth;
using System.Security.Cryptography;
using System.Text;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;

        public AccountService(IAccountRepository accountRepository, ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _tokenService = tokenService;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<UserDto> LoginByAdminOrStaff(LoginDto loginDto)
        {
            Account account = await _accountRepository.GetAccountByUsernameAsync(loginDto.Username);
            if (account == null)
                return null;
            else
            {
                using var hmac = new HMACSHA512(account.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != account.PasswordHash[i])
                    {
                        return null;
                    }
                }

                return new UserDto
                {
                    Id = account.AccountId,
                    Email = account.AccountEmail,
                    Token = _tokenService.CreateToken(account),
                    RoleId = account.RoleId,
                    AccountName = account.AccountName,
                    Username = account.Username
                };
            }
        }

        public async Task<UserDto> LoginGoogleByMember(LoginGoogleParam loginGoogleDto)
        {
            // validate token
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(loginGoogleDto.idTokenString);

            string userEmail = payload.Email;

            if (await _accountRepository.isEmailExisted(userEmail))
            {
                // login
                Account account = await _accountRepository.GetAccountByEmailAsync(userEmail);
                return new UserDto
                {
                    Id = account.AccountId,
                    Email = account.AccountEmail,
                    Token = _tokenService.CreateToken(account),
                    RoleId = account.RoleId,
                    AccountName = account.AccountName,
                    Username = account.Username
                };
            }
            else
            {
                // register
                using var hmac = new HMACSHA512();

                Account account = new Account();
                account.AccountEmail = userEmail;
                account.Username = payload.Name;
                account.AccountName = payload.Name;
                account.RoleId = 3;
                account.Date_Created = DateTime.UtcNow;
                account.Date_End = DateTime.MaxValue;

                await _accountRepository.CreateAsync(account);

                return new UserDto
                {
                    Id = account.AccountId,
                    Email = account.AccountEmail,
                    Token = _tokenService.CreateToken(account),
                    RoleId = account.RoleId,
                    AccountName = account.AccountName,
                    Username = account.Username
                };
            }
        }
    }
}
