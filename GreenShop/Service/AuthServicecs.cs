using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GreenShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GreenShop.Service
{
    public class AuthServicecs
    {
        private readonly IConfiguration _configuration;
		private readonly GreenShopContext _context;
		public AuthServicecs(IConfiguration configuration, GreenShopContext context)
        {
            this._configuration = configuration;
			_context = context;

		}
       
        public async Task<string> CreateToken(User user)
        {
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user), "User cannot be null when creating token");
			}

			if (user.Status == 0) return "Inactive";

            //add email to claim
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new("userId", user.Id.ToString()),
                new("username",user.Username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Get role of user
            authClaims.Add(new Claim(ClaimTypes.Role, "User"));

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

            var jwtService = new JwtService(
                _configuration["JWT:Secret"]!,
                _configuration["JWT:ValidIssuer"]!,
                _configuration["JWT:ValidAudience"]!);
            ////Create token
            var token = jwtService.GenerateTokenLogin(authClaims);

            return token;
        }

        public string GenerateToken(string email)
        {
            var jwtService = new JwtService(
                _configuration["JWT:Secret"]!,
                _configuration["JWT:ValidIssuer"]!,
                _configuration["JWT:ValidAudience"]!);
            var token = jwtService.GenerateToken(email);
            return token;
        }
		
		public async Task<User> CreateNewUser2(string email,string username,string password)
		{
			Models.User user = new Models.User();
            user.Id = Guid.NewGuid();
            user.Email = email;
			user.Username = username;
			user.Password = BCrypt.Net.BCrypt.HashPassword(password);
			user.CreateAt = DateTime.Now;
			user.Status = 1;

            // Get RoleId for role "User"
            user.RoleId = Guid.Parse("f0634a25-d7aa-4b28-ab22-9bcc82756584");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
		public async Task<User> CreateNewUser(LoginGoogleModel googleModel)
		{
			Models.User user = new Models.User();
			user.Id = Guid.NewGuid();

			user.GoogleId = googleModel.GoogleId;
			user.Email = googleModel.Email;
			user.Username = googleModel.Username;
			user.Password = "1@113$2aMGs";
			user.CreateAt = DateTime.Now;
			user.Status = 1;
			user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
			// Get RoleId for role "User"
			user.RoleId = Guid.Parse("f0634a25-d7aa-4b28-ab22-9bcc82756584");

				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();

			return user;
		}

		public async Task<bool> IsEmailTaken(string email)
		{
			return await _context.Users.AnyAsync(u => u.Email == email);
		}

		public async Task<bool> IsUsernameTaken(string username)
		{
			return await _context.Users.AnyAsync(u => u.Username == username);
		}

		public bool ValidatePassword(string password)
		{
			if (string.IsNullOrEmpty(password) || password.Length < 8)
				return false;

			bool hasUpper = password.Any(char.IsUpper);
			bool hasLower = password.Any(char.IsLower);
			bool hasDigit = password.Any(char.IsDigit);
			bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

			return hasUpper && hasLower && hasDigit && hasSpecial;
		}
	}
}

