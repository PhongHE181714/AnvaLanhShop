using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GreenShop.Service
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetUrl)
        {
            string subject = "GreenShop - Reset Your Password";
            string htmlBody = $@"
                    <html>
                        <body style='font-family: Arial, sans-serif;'>
                            <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                                <h2 style='color: #28a745;'>Reset Your Password</h2>
                                <p>You requested to reset your password for your GreenShop account.</p>
                                <p>Click the button below to reset your password:</p>
                                <div style='margin: 30px 0;'>
                                    <a href='{resetUrl}'
                                       style='background-color: #28a745;
                                              color: white;
                                              padding: 12px 30px;
                                              text-decoration: none;
                                              border-radius: 5px;
                                              display: inline-block;'>
                                        Reset Password
                                    </a>
                                </div>
                                <p style='color: #666;'>
                                    <small>This link will expire in 30 minutes.</small>
                                </p>
                                <hr style='margin: 20px 0; border: none; border-top: 1px solid #ddd;'>
                                <p style='color: #999; font-size: 12px;'>
                                    GreenShop Team
                                </p>
                            </div>
                        </body>
                    </html>";
            
            await SendEmailAsync(toEmail, subject, htmlBody);
        }

        public async Task SendVerificationCodeEmailAsync(string toEmail, string code)
        {
            string subject = "GreenShop - Verify Your Email";
            string htmlBody = $@"
                    <html>
                        <body style='font-family: Arial, sans-serif;'>
                            <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                                <h2 style='color: #28a745;'>Verify Your Email</h2>
                                <p>Thank you for registering with GreenShop. Please use the following verification code to complete your registration:</p>
                                <div style='margin: 30px 0; text-align: center;'>
                                    <h1 style='color: #28a745; letter-spacing: 5px;'>{code}</h1>
                                </div>
                                <p style='color: #666;'>
                                    This code will expire in 10 minutes.
                                </p>
                                <hr style='margin: 20px 0; border: none; border-top: 1px solid #ddd;'>
                                <p style='color: #999; font-size: 12px;'>
                                    GreenShop Team
                                </p>
                            </div>
                        </body>
                    </html>";

            await SendEmailAsync(toEmail, subject, htmlBody);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            var fromEmail = _configuration["EmailSettings:SenderEmail"];
            var fromName = _configuration["EmailSettings:FromName"] ?? "GreenShop";

            if (string.IsNullOrEmpty(fromEmail))
                throw new Exception("Email configuration error: 'EmailSettings:SenderEmail' is missing in appsettings.json.");
            
            if (string.IsNullOrEmpty(toEmail))
                throw new Exception("Email error: Recipient address (toEmail) is null or empty.");

            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPortStr = _configuration["EmailSettings:Port"];
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];

                if (string.IsNullOrEmpty(smtpServer)) throw new Exception("SMTP Server not configured.");
                if (string.IsNullOrEmpty(smtpPortStr)) throw new Exception("SMTP Port not configured.");
                if (string.IsNullOrEmpty(username)) throw new Exception("SMTP Username not configured.");
                if (string.IsNullOrEmpty(password)) throw new Exception("SMTP Password not configured.");

                int smtpPort = int.Parse(smtpPortStr);

                // For Gmail Port 587, use StartTls
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }
    }
}
