namespace ThreeS.Routine.Data
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Defines the <see cref="ApplicationDbInitializer" />
    /// </summary>
    public static class ApplicationDbInitializer
    {
        /// <summary>
        /// The SeedData
        /// </summary>
        /// <param name="userManager">The userManager<see cref="UserManager{MyIdentityUser}"/></param>
        /// <param name="roleManager">The roleManager<see cref="RoleManager{MyIdentityRole}"/></param>
        public static void SeedData(UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        /// <summary>
        /// The SeedRoles
        /// </summary>
        /// <param name="roleManager">The roleManager<see cref="RoleManager{ApplicationRole}"/></param>
        public static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync
        ("NormalUser").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "NormalUser";
                role.Description = "Perform normal operations.";
                _ = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync
        ("Administrator").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Administrator";
                role.Description = "Perform all the operations.";
                _ = roleManager.CreateAsync(role).Result;
            }
        }

        /// <summary>
        /// The SeedUsers
        /// </summary>
        /// <param name="userManager">The userManager<see cref="UserManager{ApplicationUser}"/></param>
        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {

            if (userManager.FindByNameAsync
("user1").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "3s",
                    Email = "3s@localhost",
                    FirstName = "3S",
                    LastName = "User"

                };

                IdentityResult result = userManager.CreateAsync
                (user, "Host@5057").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "NormalUser").Wait();
                }
            }


            if (userManager.FindByNameAsync
        ("user2").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "user2@localhost",
                    FirstName = "3S",
                    LastName = "Admin"
                };

                IdentityResult result = userManager.CreateAsync
                (user, "Commonpw@5057").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "Administrator").Wait();
                }
            }
        }
    }
}
