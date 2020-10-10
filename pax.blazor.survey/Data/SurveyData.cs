using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace pax.blazor.survey.Data
{
    public static class SurveyData
    {
        public static string configfile = "/data/surveyconfig.json";
        public static string CustomAnswerFormValidationMessage = "Character ';' is not allowed.";
        public static string DefaultPlaceholder = "Type your answer here";
        public const string fmFormContainer = "form-group row justify-content-start mb-1";
        public const string fmFormLabel = "col-sm-4 col-lg-2 col-xl-2 col-form-label text-muted";
        public const string fmFormInput = "col-9";
        public static string BasePath = string.Empty;

        /// <summary>
        /// Initialize Data
        /// </summary>
        public static async Task Init(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration conf)
        {
            await SeedAdminUser(userManager, roleManager, conf);

        }

        /// <summary>
        /// Seed Admin- and Mod User
        /// </summary>
        public static async Task SeedAdminUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration conf)
        {
            if (!await roleManager.RoleExistsAsync("NormalUser"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "NormalUser";
                var result = await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Moderator"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Moderator";
                var result = await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";
                var result = await roleManager.CreateAsync(role);
            }

            var username = conf["AdminUser:UserName"];
            var password = conf["AdminUser:Password"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = username;
                user.Email = username;
                user.EmailConfirmed = true;

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }

            username = conf["ModUser:UserName"];
            password = conf["ModUser:Password"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = username;
                user.Email = username;
                user.EmailConfirmed = true;

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Moderator");
                }
            }

        }
    }

    public enum Feedback
    {
        Default,
        Working,
        Success,
        Failed
    }
}
