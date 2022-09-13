using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SurfsUpProjekt.Core;
using SurfsUpProjekt.Data;
using SurfsUpProjekt.Models;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SurfsUpProjektContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<SurfsUpProjektContext>>()))
            {
                //if (context.Board.Any())
                //{
                //    return;   // DB has been seeded
                //}
                //if (context.User.Any())
                //{
                //    return;
                //}

                #region boards
                //context.Board.AddRange(
                //    new Board
                //    {
                //        Name = "The Minilog",
                //        Length = 6,
                //        Width = 21,
                //        Thickness = 2.75,
                //        Volume = 38.8,
                //        Type = "Shortboard",
                //        Price = 565,
                //        Image = "https://www.surfline.dk/pub/media/catalog/product/cache/cfa80b4db01644a142c5bb77a163cbc9/b/i/bic_5_10x.jpg"
                //    },

                //    new Board
                //    {
                //        Name = "The Wide Glider",
                //        Length = 7.1,
                //        Width = 21.75,
                //        Thickness = 2.75,
                //        Volume = 44.16,
                //        Type = "Funboard",
                //        Price = 685,
                //        Image = "https://havsstore.dk/images/IgniteMiniMal80_Blue_Navy_Deck-p.jpg"
                //    },

                //    new Board
                //    {
                //        Name = "The Golden Ratio",
                //        Length = 6.3,
                //        Width = 21.85,
                //        Thickness = 2.9,
                //        Volume = 29.39,
                //        Type = "Funboard",
                //        Price = 695,
                //        Image = "https://cdn.shopify.com/s/files/1/0280/0235/3239/products/what-top_03a2b77c-f2e4-4e47-83b7-54a7d0a28c3d_1600x.jpg?v=1648664678"
                //    },

                //    new Board
                //    {
                //        Name = "Mahi Mahi",
                //        Length = 5.4,
                //        Width = 20.75,
                //        Thickness = 2.3,
                //        Volume = 44.16,
                //        Type = "Fish",
                //        Price = 645,
                //        Image = "https://images.blue-tomato.com/is/image/bluetomato/304736908_front.jpg-H4PsPhTNHmbDVcLjz0z9rBUUEp0/6+6+Softtop+Surfboard.jpg?$b8$"
                //    },

                //    new Board
                //    {
                //        Name = "The Emerald Glider",
                //        Length = 9.2,
                //        Width = 22.8,
                //        Thickness = 2.8,
                //        Volume = 65.4,
                //        Type = "Longboard",
                //        Price = 895,
                //        Image = "https://westwind.dk/images/VISION_SURFBOARD_XPS_SPARK_6_7_8_9_SOFTTOP.JPG"
                //    },

                //    new Board
                //    {
                //        Name = "The Bomb",
                //        Length = 5.5,
                //        Width = 21,
                //        Thickness = 2.5,
                //        Volume = 33.7,
                //        Type = "Shortboard",
                //        Price = 645,
                //        Image = "https://media.cleanlinesurf.com/mf_webp/jpg/media/catalog/product/cache/d2790183301c0fd7d2cc67391fc7a96f/f/i/firewire-seaside-machado-helium-surfboard-deck.webp"
                //    },

                //    new Board
                //    {
                //        Name = "Walden Magic",
                //        Length = 9.6,
                //        Width = 19.4,
                //        Thickness = 3,
                //        Volume = 80,
                //        Type = "Longboard",
                //        Price = 1025,
                //        Image = "https://medieserver.jemogfix.dk/fotoweb/dk/varer/700/2101%209038090.jpg"
                //    },

                //    new Board
                //    {
                //        Name = "Naish One",
                //        Length = 12.6,
                //        Width = 30,
                //        Thickness = 6,
                //        Volume = 301,
                //        Type = "SUP",
                //        Price = 854,
                //        Equipment = "Paddle",
                //        Image = "https://www.costco.co.uk/medias/sys_master/images/h31/hf3/116346594131998.jpg"
                //    },

                //    new Board
                //    {
                //        Name = "Six Tourer",
                //        Length = 11.6,
                //        Width = 32,
                //        Thickness = 6,
                //        Volume = 270,
                //        Type = "SUP",
                //        Price = 611,
                //        Equipment = "Paddle, Paddle, Pump, Leash",
                //        Image = "https://www.kitemana.com/_images/h537/global-2021-surfboard-284474.jpg"
                //    },

                //    new Board
                //    {
                //        Name = "Naish Maliko",
                //        Length = 14,
                //        Width = 25,
                //        Thickness = 6,
                //        Volume = 330,
                //        Type = "SUP",
                //        Price = 1304,
                //        Equipment = "Paddle, Paddle, Pump, Leash",
                //        Image = "https://surf-ski.dk/media/catalog/product/cache/7/image/1000x1000/9df78eab33525d08d6e5fb8d27136e95/o/x/oxbow-surf_2020_dura-tec_7-9_103774_hr.jpg"
                //    }

                //);
                //context.SaveChanges();
                #endregion

                #region Users
                //context.Users.AddRange(
                //    new xxxxxxxxx
                //    {
                //        id = "1d95a11f-e551-408f-b9a8-04d817978758",
                //        UserName = "admin@mail.com",
                //        NormalizedUserName = "ADMIN@MAIL.COM",
                //        Email = "admin@mail.com",
                //        NormalizedEmail = "ADMIN@MAIL.COM",
                //        EmailConfirmed = false,
                //        PasswordHash = "AQAAAAEAACcQAAAAEFtnWms8K3FGlyXBrR3GLbOkRH32+gu4uTCQjIKCWuYLGckgg7z4su6B1ff6cqcIuw==",
                //        SecurityStamp = "OELH2XLIMFVQPVYFR6JT3OEKDGFG2KNM",
                //        ConcurrencyStamp = "c1a263a9-5792-4b1d-88a7-a58a010b86d3",
                //        //PhoneNumber = "INSERT VALUE",
                //        PhoneNumberConfirmed = false,
                //        //LockoutEnd = "INSERT VALUE",
                //        TwoFactoEnabled = false,
                //        LockoutEnabled = true,
                //        AccessFailedCount = 0,
                //    },
                //    new AspNetUsers
                //    {
                //        id = "eb948985-8037-4ef7-9ff2-cd6f30e41b9c",
                //        UserName = "test@mail.com",
                //        NormalizedUserName = "TEST@MAIL.COM",
                //        Email = "test@mail.com",
                //        NormalizedEmail = "TEST@MAIL.COM",
                //        EmailConfirmed = false,
                //        PasswordHash = "AQAAAAEAACcQAAAAEIwGR9mzMggdMm8m9oLcxis62GM6ZPCdQuaVBtKQ66d1puJk5L54yRxVuhntTslNKg==",
                //        SecurityStamp = "EQNG4OVWMNDKPUYWUBRWWDFEFOZBZRFT",
                //        ConcurrencyStamp = "e3cf4708-508b-4152-8ea9-42d02b5c1811",
                //        //PhoneNumber = "INSERT VALUE",
                //        PhoneNumberConfirmed = false,
                //        //LockoutEnd = "INSERT VALUE",
                //        TwoFactoEnabled = false,
                //        LockoutEnabled = true,
                //        AccessFailedCount = 0,
                //    }
                //);
                //context.SaveChanges();
                #endregion
            }
        }
        public static async Task InitializeRoles(IServiceProvider serviceProvider)
        {
            var context = new SurfsUpProjektContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<SurfsUpProjektContext>>());

            string[] Roles = { ConstantsRole.Roles.Administrator, ConstantsRole.Roles.Manager, ConstantsRole.Roles.User };

            var _roleStore = new RoleStore<IdentityRole>(context);

            foreach (var role in Roles)
            {
                if (!context.Roles.Any(s => s.Name == role))
                {
                    var newRole = new IdentityRole(role);
                    newRole.NormalizedName = role.ToUpper();
                    _roleStore.CreateAsync(newRole).GetAwaiter().GetResult();
                }
            }

            await context.SaveChangesAsync();
            
            //var _userStore = new UserStore<IdentityUser>(context);
            //_userStore.AddToRoleAsync(user, "admin");
        }
    }
}