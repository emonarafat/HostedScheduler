namespace ThreeS.Routine.Data
{
    using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="ApplicationDbContext" />
    /// </summary>
    public class ApplicationDbContext
     : IdentityDbContext<
         ApplicationUser, ApplicationRole, string,
         IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
         IdentityRoleClaim<string>, IdentityUserToken<string>>, IDataProtectionKeyContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{ApplicationDbContext}"/></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets the DataProtectionKeys
        /// </summary>
        public DbSet<DataProtectionKey> DataProtectionKeys { get; }

        /// <summary>
        /// The OnModelCreating
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("MyUsers");
            });

            modelBuilder.Entity<ApplicationUserClaim>(b =>
            {
                b.ToTable("MyUserClaims");
            });

            modelBuilder.Entity<ApplicationUserLogin>(b =>
            {
                b.ToTable("MyUserLogins");
            });

            modelBuilder.Entity<ApplicationUserToken>(b =>
            {
                b.ToTable("MyUserTokens");
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("MyRoles");
            });

            modelBuilder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToTable("MyRoleClaims");
            });

            modelBuilder.Entity<ApplicationUserRole>(b =>
            {
                b.ToTable("MyUserRoles");
            });
        }
    }
}
