﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Verdon.Data;
using Verdon.Models.Auth;
using Verdon.Models.Utility;

namespace Verdon.Seeder
{
    public class UserSeeder
    {   
        public AccessKey Access { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly UserManager<VerdonUser> userManager;
        public readonly RoleManager<VerdonRole> roleManager;
        public UserSeeder(ApplicationDbContext context, RoleManager<VerdonRole> _roleManager, UserManager<VerdonUser> _userManager)
        {
            _context = context;
            roleManager = _roleManager;
            userManager = _userManager;
        }

        public void SeedData()
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

     

        public async void SeedUsers (UserManager<VerdonUser> userManager)
        {

            try
            {
                if (userManager.FindByNameAsync
                 ("tutor@gmail.com").Result == null)
                {
                    var user = new VerdonUser();
                    user.Image = "main/img/avatars/1.png";
                    user.UserName = "tutor@gmail.com";
                    user.Email = "tutor@gmail.com";
                    user.Type = "SingleTutor";
                    user.FirstName = "Bode";
                    user.LastName = "Barl";
                    user.EmailConfirmed = true;
                    IdentityResult result = userManager.CreateAsync
                    (user, "Solomon12!").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Tutor").Wait();
                         var entity = await _context.AccessKeys.AddAsync(new AccessKey
                        {
                            Students = 400,
                            Tutors = 40,
                            ExpiryDate = DateTime.Now.AddMonths(1),
                        });
                        Access = entity.Entity;
                        await _context.UserAccess.AddAsync(new UserAccess
                        {
                            Type ="tutor",
                            AccessKeyId = entity.Entity.Id,
                            VerdonUserId = user.Id,
                            ExpiryDate = entity.Entity.ExpiryDate
                        });
                         await _context.SaveChangesAsync();
                    }
                }
                if (userManager.FindByNameAsync
                 ("student@gmail.com").Result == null)
                {
                    var user = new VerdonUser();
                    user.Image = "main/img/avatars/1.png";
                    user.UserName = "student@gmail.com";
                    user.Email = "student@gmail.com";
                    user.Type = "student";
                    user.FirstName = "Babas";
                    user.LastName = "Cacas";
                    user.EmailConfirmed = true;
                    IdentityResult result = userManager.CreateAsync
                    (user, "Solomon12!").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Student").Wait();
                        await _context.UserAccess.AddAsync(new UserAccess
                        {
                            Type = "student",
                            AccessKeyId = Access.Id,
                            VerdonUserId = user.Id,
                            ExpiryDate = Access.ExpiryDate
                        });
                        await _context.SaveChangesAsync();
                    }
                }
                
                  if (userManager.FindByNameAsync
                 ("admin@gmail.com").Result == null)
                {
                    var user = new VerdonUser();
                    user.UserName = "admin@gmail.com";
                    user.Email = "admin@gmail.com";
                    user.EmailConfirmed = true;
                    IdentityResult result = userManager.CreateAsync
                    (user, "Solomon12!").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }

               

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }



        public async void SeedRoles(RoleManager<VerdonRole> roleManager)
        {
            try 
            {

                if (!roleManager.RoleExistsAsync
                   ("Tutor").Result)
                {
                    var role = new VerdonRole();
                    role.Name = "Tutor";
                    IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
                }


                if (!roleManager.RoleExistsAsync
                    ("Student").Result)
                {
                    var role = new VerdonRole();
                    role.Name = "Student";
                    IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
                }
                
                if (!roleManager.RoleExistsAsync
                    ("Admin").Result)
                {
                    var role = new VerdonRole();
                    role.Name = "Admin";
                    IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
