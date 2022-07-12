using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebWordGame.Controllers;

namespace WebWordGame.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<PersonModel> People { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<GameModel> Games { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<GameInfoModel> GameInfo { get; set; }
        public DbSet<RoomGamer> RoomGamers { get; set; }
        public DbSet<WordModel> Words { get; set; }


        public DataBaseContext(DbContextOptions<DataBaseContext> dbContextOptions) 
            : base(dbContextOptions)
        {
            Database.EnsureCreated();
            if (!Images.Any())
            {
                Images.Add(new ImageModel { ImageSource = "../images/starting_profile_images/image_anime_girl.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/starting_profile_images/image_bodybilder.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/starting_profile_images/image_girl_in_headphones.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/starting_profile_images/image_girl_with_skate.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/starting_profile_images/image_lotus.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/starting_profile_images/image_nissan_r35.jpg" });
                Images.Add(new ImageModel { ImageSource = "../images/starting_profile_images/image_sunrise.jpeg" });
                Images.Add(new ImageModel { ImageSource = "../images/starting_profile_images/default_image.jpg" });



            }
            if (!GameInfo.Any()) 
            {
                GameInfo.Add(new GameInfoModel { CurrentlyOnline = 0, PeopleInQueue = 0, QuantityOfGames = 0, QuantityOfPlayers = 0 });
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
            PersonModel adminUser = new PersonModel { Id = 1, LoginName = adminLogin, Password = Crypto.Hash(adminPassword), RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<PersonModel>().HasData(new PersonModel[] { adminUser });

            modelBuilder
               .Entity<GameModel>()
               .HasMany(c => c.People)
               .WithMany(s => s.Games)
               .UsingEntity<RoomGamer>(
                  j => j
                   .HasOne(pt => pt.Person)
                   .WithMany(t => t.roomGamers)
                   .HasForeignKey(pt => pt.PersonId),
               j => j
                   .HasOne(pt => pt.Game)
                   .WithMany(p => p.roomGamers)
                   .HasForeignKey(pt => pt.GameId),
               j =>
               {
                   j.Property(pt => pt.IsWinner).HasDefaultValue(false);
                   j.Property(pt => pt.IsActive).HasDefaultValue(false);
                   j.Property(pt => pt.TimeToMove).HasDefaultValue(30);
                   j.Property(pt => pt.ConnectId).HasDefaultValue(null);
                   j.Property(pt => pt.OrderOfTheMove).HasDefaultValue(null);
                   j.Property(pt => pt.Score).HasDefaultValue(0);
                   j.Property(pt => pt.ConnectedToTheGame).HasDefaultValue(false);
                   j.HasKey(t => new { t.GameId, t.PersonId });
                   j.ToTable("RoomGamers");
               }

               
               );

            modelBuilder.Entity<WordModel>().Property(p => p.NumberOfUses).HasDefaultValue(1);
            base.OnModelCreating(modelBuilder);
        }
    }
}
