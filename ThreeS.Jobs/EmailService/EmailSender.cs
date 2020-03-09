namespace ThreeS.Jobs
{
    using MailKit.Net.Smtp;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    using MimeKit;

    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="EmailSender" />
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Defines the _emailSettings
        /// </summary>
        private readonly EmailSettings _emailSettings;

        /// <summary>
        /// Defines the _env
        /// </summary>
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="emailSettings">The emailSettings<see cref="IOptions{EmailSettings}"/></param>
        /// <param name="env">The env<see cref="IHostingEnvironment"/></param>
        public EmailSender(IOptions<EmailSettings> emailSettings, IHostingEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
        }

        /// <summary>
        /// The SendEmailAsync
        /// </summary>
        /// <param name="email">The email<see cref="string"/></param>
        /// <param name="subject">The subject<see cref="string"/></param>
        /// <param name="message">The message<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

                mimeMessage.To.Add(new MailboxAddress(email));

                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    //if (_env.IsDevelopment())
                    //{
                    //    // The third parameter is useSSL (true if the client should make an SSL-wrapped
                    //    // connection to the server; otherwise, false).
                    //    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, true);
                    //}
                    //else
                    //{
                    await client.ConnectAsync(_emailSettings.MailServer);
                    //}

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
