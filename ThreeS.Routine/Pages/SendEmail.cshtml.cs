using Hangfire;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.ComponentModel.DataAnnotations;

using ThreeS.Jobs;

namespace ThreeS.Routine
{
    public class SendEmailModel : PageModel
    {
        private readonly IEmailSender _emailSender;

        public SendEmailModel(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public string EmailStatusMessage { get; set; }

        [Required]
        [BindProperty]
        public string Email { get; set; }

        [Required]
        [BindProperty, Display(Name = "Message"), DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Required]
        [BindProperty]
        public string Subject { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = Email;

            var subject = Subject;

            var message = Body;


            RecurringJob.AddOrUpdate($"{Subject}-{Email}", () => _emailSender.SendEmailAsync(email, subject, message), Cron.Daily(), timeZone: TimeZoneInfo.Local);
            EmailStatusMessage = "Send test email was successful.";

            return Page();
        }
    }
}