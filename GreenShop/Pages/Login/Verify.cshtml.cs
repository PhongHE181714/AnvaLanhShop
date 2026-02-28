using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using GreenShop.Service;

namespace GreenShop.Pages.Login
{
    public class VerifyModel : PageModel
    {
        private readonly GreenShopContext _context;
        private readonly AuthServicecs _authService;
        private readonly EmailService _emailService;

        public VerifyModel(GreenShopContext context, AuthServicecs authService, EmailService emailService)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
        }

        [BindProperty(SupportsGet = true)]
        public string Username { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ExpectedCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StartTime { get; set; }

        [BindProperty]
        public string VerificationCode { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public bool IsCodeVerified { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet(string email, string username, string code, string time)
        {
            Email = email;
            Username = username;
            ExpectedCode = code;
            StartTime = time;
        }

        public async Task<IActionResult> OnPostResendAsync()
        {
            // Generate new verification code
            ExpectedCode = new Random().Next(100000, 999999).ToString();
            StartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

            // Send verification email
            try
            {
                await _emailService.SendVerificationCodeEmailAsync(Email, ExpectedCode);
                ErrorMessage = "A new verification code has been sent to your email.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to send verification email: {ex.Message}.";
            }

            // Stay on the same verification stage
            IsCodeVerified = false;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Parse StartTime for 60-second expiry check
            if (!long.TryParse(StartTime, out long startUnixMs))
            {
                ErrorMessage = "Invalid session information.";
                return Page();
            }

            // Always verify the code is correct against the expected code
            if (string.IsNullOrWhiteSpace(VerificationCode) || VerificationCode != ExpectedCode)
            {
                ErrorMessage = "Invalid verification code.";
                return Page();
            }

            // Stage 1: Initial Code Verification
            if (!IsCodeVerified)
            {
                // check for 60-second expiry ONLY during the first stage submission
                var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                if (startUnixMs + 60000 < now)
                {
                    ErrorMessage = "Verification code expired (valid for 60s). Please register again.";
                    return Page();
                }

                IsCodeVerified = true;
                ModelState.Clear();
                return Page();
            }

            // Stage 2: Final Step - Set Password and Save to DB
            if (string.IsNullOrEmpty(Password) || Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return Page();
            }

            if (!_authService.ValidatePassword(Password))
            {
                ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
                return Page();
            }

            // Final safety check for duplicate username/email right before saving
            if (await _authService.IsEmailTaken(Email) || await _authService.IsUsernameTaken(Username))
            {
                ErrorMessage = "Username or Email has been taken since you started registration. Please try again.";
                return RedirectToPage("/Login/SignUp");
            }

            // CREATE USER IN DATABASE
            GreenShop.Models.User user = new GreenShop.Models.User();
            user.Id = Guid.NewGuid();
            user.Email = Email;
            user.Username = Username;
            user.Password = BCrypt.Net.BCrypt.HashPassword(Password);
            user.CreateAt = DateTime.Now;
            user.Status = 1; 
            user.RoleId = Guid.Parse("f0634a25-d7aa-4b28-ab22-9bcc82756584"); 

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Log the user in
            var token = await _authService.CreateToken(user);
            Response.Cookies.Append("AuthToken", token);

            return RedirectToPage("/Shop/Index");
        }
    }
}
