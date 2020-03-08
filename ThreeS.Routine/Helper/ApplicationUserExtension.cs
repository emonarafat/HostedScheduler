using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace ThreeS.Routine.Helper
{
    public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpcontext = context.GetHttpContext();
            return httpcontext.User.Identity.IsAuthenticated;
        }
    }
    public static class ApplicationUserExtension
    {
        //public static ApplicationUser GeneratePasswordHash(this ApplicationUser user)
        //{
        //    var phasher = new PasswordHasher<ApplicationUser>();
        //    //var user= new ApplicationUser
        //    //{
        //    //    Id = 1,
        //    //    Email = "admin@3shostt.net",
        //    //    NormalizedEmail = "ADMIN@3SHOSTT.NET",
        //    //    EmailConfirmed = true,
        //    //    FirstName = "3S",
        //    //    LastName = "Admin",
        //    //    PhoneNumber = "",
        //    //    UserName = "admin",
        //    //    NormalizedUserName = "ADMIN",
        //    //};
        //    user.PasswordHash = phasher.HashPassword(user, "Dhaka987");
        //    return user;
        //}
        //public static ApplicationUser GeneratePasswordHash()
        //{
        //    var userManager =
        //    HttpContext.Current.GetOwinContext()
        //        .GetUserManager<ApplicationUserManager>();
        //    var phasher = new PasswordHasher<ApplicationUser>();
        //    var user = new ApplicationUser
        //    {
        //        Id = 1,
        //        Email = "admin@3shostt.net",
        //        NormalizedEmail = "ADMIN@3SHOSTT.NET",
        //        EmailConfirmed = true,
        //        FirstName = "3S",
        //        LastName = "Admin",
        //        PhoneNumber = "",
        //        UserName = "admin",
        //        NormalizedUserName = "ADMIN",

        //    };
        //    user.PasswordHash = phasher.HashPassword(user, "Dhaka987");
        //    return user;
        //}
    }
}
