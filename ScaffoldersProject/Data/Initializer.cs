using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Data
{
    //Creating Roles and add them to database
    public static class Initializer
    {
        public static async Task Initial(IApplicationBuilder app)
        {
            //Dependency Injection (we also added in the Program.cs the .UserDefaultServivesProvider)
            RoleManager<IdentityRole> roleManager = app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var users = new IdentityRole("Admin");
                await roleManager.CreateAsync(users);                
            }

            if (!await roleManager.RoleExistsAsync("Member"))
            {
                var users = new IdentityRole("Member");
                await roleManager.CreateAsync(users);
            }

            if (!await roleManager.RoleExistsAsync("Client"))
            {
                var users = new IdentityRole("Client");
                await roleManager.CreateAsync(users);
            }
        }
    }
}
