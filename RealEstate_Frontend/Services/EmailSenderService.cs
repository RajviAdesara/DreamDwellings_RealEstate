using System.Net.Mail; // Namespace for email-related classes like SmtpClient and MailMessage.
using System.Net; // Namespace for network credentials and related functionality.

namespace RealEstate.Services
{
    public class EmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailSenderService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false)
        {
            // Retrieve the mail server (SMTP host) from the configuration.
            string? MailServer = _configuration["EmailSettings:MailServer"];
            // Retrieve the sender email address from the configuration.
            string? FromEmail = _configuration["EmailSettings:FromEmail"];
            // Retrieve the sender email password from the configuration.
            string? Password = _configuration["EmailSettings:Password"];
            // Retrieve the sender's display name from the configuration.
            string? SenderName = _configuration["EmailSettings:SenderName"];
            // Retrieve the SMTP port number from the configuration and convert it to an integer.
            int Port = Convert.ToInt32(_configuration["EmailSettings:MailPort"]);
            // Create a new instance of SmtpClient using the mail server and port number.
            var client = new SmtpClient(MailServer, Port)
            {
                // Set the credentials (email and password) for the SMTP server.
                Credentials = new NetworkCredential(FromEmail, Password),
                // Enable SSL for secure email communication.
                EnableSsl = true,
            };
            // Create a MailAddress object with the sender's email and display name.
            MailAddress fromAddress = new MailAddress(FromEmail, SenderName);
            // Create a new MailMessage object to define the email's properties.
            MailMessage mailMessage = new MailMessage
            {
                From = fromAddress, // Set the sender's email address with display name.
                Subject = Subject, // Set the email subject line.
                Body = Body, // Set the email body content.
                IsBodyHtml = IsBodyHtml // Specify whether the body content is in HTML format.
            };
            // Add the recipient's email address to the message.
            mailMessage.To.Add(ToEmail);
            // Send the email asynchronously using the SmtpClient instance.
            return client.SendMailAsync(mailMessage);
        }

        public async Task SendPromotionalEmailWithEmail(string userEmail)
        {
            if (!string.IsNullOrEmpty(userEmail))
            {
                string subject = "List Your Property for Free - DreamDwellings";
                string htmlBody = @"
<html>
    <head>
        <title>List Your Property</title>
        <style>
            body {
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                color: #333;
                padding: 20px;
            }
            .container {
                background-color: #fff;
                padding: 20px;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0,0,0,0.1);
                max-width: 600px;
                margin: 0 auto;
            }
            h1 {
                color: #4CAF50;
            }
            .button {
                display: inline-block;
                padding: 10px 20px;
                margin: 20px 0;
                font-size: 16px;
                color: #fff;
                background-color: #4CAF50;
                border: none;
                border-radius: 5px;
                text-decoration: none;
            }
            .footer {
                margin-top: 20px;
                font-size: 12px;
                color: #777;
            }
            .number {
                font-size: 24px;
                font-weight: bold;
                color: Black;
            }
            .featured-properties {
                margin-top: 20px;
                border-top: 1px solid #eee;
                padding-top: 15px;
            }
        </style>
    </head>
    <body>
        <div class=""container"">
            <h1> Exclusive Real Estate Deals Just for You!</h1>
            <p>Dear Valued Customer,</p>
            <p>We're excited to share our latest property listings and special offers tailored just for you. Our platform makes it easy to find your dream home with thousands of listings updated daily.</p>
            <a href=""http://localhost:49730/Property/PropertyList"" class=""button"">View Exclusive Listings</a>
            
            <div class=""featured-properties"">
                <h2>Did you know?</h2>
                <p>You can now list your house for sale or rent easily on our platform. Simply fill out a form and your property will be visible to thousands of potential buyers or renters.</p>
                <a href=""http://localhost:49730/Property/AddProperty"" class=""button"">List Your Property Now</a>
            </div>
            
            <p>Thank you for choosing DreamDwellings!</p>
            <p class=""footer"">DreamDwellings Team</p>
        </div>
    </body>
</html>";
                try
                {
                    await SendEmailAsync(userEmail, subject, htmlBody, true);
                    Console.WriteLine($"✅ Promotional email sent to: {userEmail}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Email sending failed: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("⚠️ No user email provided.");
            }
        }
    }
}