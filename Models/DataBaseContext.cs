using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<PersonModel> People { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ImageModel> Images { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> dbContextOptions) 
            : base(dbContextOptions)
        {
            Database.EnsureCreated();
            if (!Images.Any())
            {
                Images.Add(new ImageModel { ImageSource = "../images/OpenPass.png" });
                Images.Add(new ImageModel { ImageSource = "../images/image_anime_girl.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/image_bodybilder.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/image_girl_in_headphones.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/image_girl_with_skate.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/image_lotus.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/image_nissan_r35.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/image_sunrise.jpeg" });



            }
            SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "person";

            string adminLogin= "admin";
            string adminPassword = "password";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            PersonModel adminUser = new PersonModel { Id = 1, LoginName = adminLogin, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<PersonModel>().HasData(new PersonModel[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
