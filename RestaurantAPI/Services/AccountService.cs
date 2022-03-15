using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantDAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IPasswordHasher<RegisterUserDto> _hasherRegister;
        private readonly IPasswordHasher<User> _hasherUser;
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterUserDto> _validator;

        public AccountService(RestaurantDbContext dbContext, IMapper mapper, IPasswordHasher<RegisterUserDto> hasherRegister, IPasswordHasher<User> hasherUser, IValidator<RegisterUserDto> validator, AuthenticationSettings authenticationSettings)
        {
            _authenticationSettings = authenticationSettings;
            _hasherRegister = hasherRegister;
            _hasherUser = hasherUser;
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
        }

        private User GetUser(int userId)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found");

            return user;
        }

        public void DeleteUser(int userId)
        {
            var user = GetUser(userId);
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public void RegisterUser(RegisterUserDto createUserDto)
        {
            var validationResult = _validator.Validate(createUserDto);

            if(!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var hashedpassword = _hasherRegister.HashPassword(createUserDto, createUserDto.PasswordHash);
            createUserDto.PasswordHash = hashedpassword;

            var mappedUser = _mapper.Map<User>(createUserDto);

            _dbContext.Users.Add(mappedUser);
            _dbContext.SaveChanges();
        }

        public void UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var user = GetUser(userId);

            user.RoleId = updateUserDto.RoleId;
            user.FirstName = updateUserDto.FirstName;
            user.BirthDate = updateUserDto.BirthDate;
            user.PasswordHash = updateUserDto.PasswordHash;
            user.Email = updateUserDto.Email;
            user.Nationality = updateUserDto.Nationality;

            _dbContext.SaveChanges();
        }

        public IEnumerable<ViewUserDto> DisplayUsers()
        {
            var users = _dbContext.Users.ToList();
            var viewUsers = _mapper.Map<List<ViewUserDto>>(users);

            return viewUsers;
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(r => r.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
                throw new BadRequestException("Invalid username or password");

            var result = _hasherUser.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if(result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid username or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("BirthDate", user.BirthDate.Value.ToString("yyyy-MM-dd")) 
            };

            if (!string.IsNullOrEmpty(user.Nationality))
                claims.Add(new Claim("Nationality", user.Nationality));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            
            return tokenHandler.WriteToken(token);
        }
    }
}
