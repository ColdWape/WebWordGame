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
        public DataBaseContext(DbContextOptions<DataBaseContext> dbContextOptions) 
            : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "person";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            PersonModel adminUser = new PersonModel { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<PersonModel>().HasData(new PersonModel[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
