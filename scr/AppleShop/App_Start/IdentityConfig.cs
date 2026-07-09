using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using AppleShop.Models;

namespace AppleShop
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, Microsoft.Owin.Security.IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store) : base(store)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }

    /// <summary>
    /// Tạo sẵn vai trò Admin/Customer và 2 tài khoản mặc định khi ứng dụng khởi động lần đầu.
    /// Admin:    admin@appleshop.vn / Admin@123456
    /// Customer: customer@gmail.com / Customer@123
    /// </summary>
    public static class IdentitySeeder
    {
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public static void Seed()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                foreach (var role in new[] { AdminRole, CustomerRole })
                {
                    if (!roleManager.RoleExists(role))
                        roleManager.Create(new IdentityRole(role));
                }

                EnsureUser(userManager, "admin@appleshop.vn", "Admin@123456", "Quản trị viên AppleShop", AdminRole);
                EnsureUser(userManager, "customer@gmail.com", "Customer@123", "Khách hàng mẫu", CustomerRole);
            }
        }

        private static void EnsureUser(UserManager<ApplicationUser> userManager, string email, string password, string fullName, string role)
        {
            var user = userManager.FindByEmail(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = fullName,
                    CreatedAt = DateTime.Now
                };
                var result = userManager.Create(user, password);
                if (!result.Succeeded)
                    throw new InvalidOperationException("Không thể tạo tài khoản mặc định " + email + ": " + string.Join("; ", result.Errors));
            }

            if (!userManager.IsInRole(user.Id, role))
                userManager.AddToRole(user.Id, role);
        }
    }
}
