namespace ThreeS.Routine.Data
{
    public class SeedData
    {

        public SeedData()
        {

        }

        //public static async Task Seeding(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        //{
        //    if (!context.Roles.Any())
        //    {
        //        context.Roles.AddRange(
        //             new ApplicationRole
        //             {
        //                 Id = "b562e963-6e7e-4f41-8229-4390b1257hg6",
        //                 Description = "This Is AdminUser",
        //                 Name = "Admin",
        //                 NormalizedName = "ADMIN"

        //             });

        //        await context.SaveChangesAsync().ConfigureAwait(false);
        //    }


        //    if (!context.Users.Any())
        //    {
        //        ApplicationUser user = new ApplicationUser
        //        {
        //            FirstName = "MyFirstName",
        //            LastName = "MyLastName",
        //            PhoneNumber = "9998885554",
        //            UserName = "saedbfd",
        //            NormalizedUserName = "SAEDBFD",
        //            Email = "MyEmail@Email.com",
        //            NormalizedEmail = "MYEMAIL@EMAIL.COM",
        //            PasswordHash = "AQAAAAEAACcQAAAAEH9MTIiZG90QJrMLt62Zd4Z8O5o5MaeQYYc/53e2GbawhGcx2JNUSmF0pCz9H1AnoA==",
        //            LockoutEnabled = true,
        //            SecurityStamp = "aea97aa5-8fb4-40f2-ba33-1cb3fcd54720"
        //        };

        //        context.Users.Add(user);
        //        await context.SaveChangesAsync().ConfigureAwait(false);


        //        IdentityUserRole<string> ur = new IdentityUserRole<string>();
        //        ur.RoleId = "b562e963-6e7e-4f41-8229-4390b1257hg6";
        //        ur.UserId = user.Id;

        //        context.UserRoles.Add(ur);
        //        await context.SaveChangesAsync().ConfigureAwait(false);

        //    }
        //}
    }
}
