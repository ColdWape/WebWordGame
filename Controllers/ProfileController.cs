using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebWordGame.Models;

namespace WebWordGame.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataBaseContext _dataBaseContext;

        public ProfileController(DataBaseContext dataBaseContext)
        {

            _dataBaseContext = dataBaseContext;
        }

        [Authorize(Roles = "admin, person")]
        [HttpGet]
        public IActionResult UserPage()
        {
            PersonModel person = _dataBaseContext.People.Include(i => i.ProfileImageId).Include(i => i.Games).Include(rg => rg.roomGamers).First(i => i.LoginName == User.Identity.Name);


            ViewBag.Human = person;
            if (person.Games.Count != 0)
            {
                ViewBag.Percent = Math.Round(100 * Convert.ToDouble(person.QuantityOfWins) / Convert.ToDouble(person.Games.Count), 2);
            }
            else
            {
                ViewBag.Percent = 0;
            }
            ViewBag.UserAvatar = _dataBaseContext.Images;



            return View();
        }

        [Authorize(Roles = "admin, person")]

        [HttpPost]
        public IActionResult UserPage(int PictureId)
        {
            if (PictureId != 0)
            {
                PersonModel person = _dataBaseContext.People.First(u => u.LoginName == User.Identity.Name);
                ImageModel newImage = _dataBaseContext.Images.First(u => u.Id == PictureId);

                person.ProfileImageId = newImage;
                _dataBaseContext.SaveChanges();
            }

            ViewBag.Human = _dataBaseContext.People.Include(i => i.ProfileImageId).Include(i => i.Games).First(i => i.LoginName == User.Identity.Name);

            ViewBag.UserAvatar = _dataBaseContext.Images;
            return View();
            //string test = $"Text: {person.ProfileImageId.ImageSource}";
            //return Content(test);
        }
        [Authorize(Roles = "admin, person")]

        public IActionResult allPlayedGames()
        {
            PersonModel person = _dataBaseContext.People.Include(g => g.Games).Include(r => r.roomGamers).FirstOrDefault(p => p.LoginName == User.Identity.Name);
            List <GameModel> games = new();
            if (person.Games.Count > 0)
            {
                List<RoomGamer> gamers = _dataBaseContext.RoomGamers.Where(p => p.PersonId == person.Id).ToList();
                foreach (var item in gamers)
                {
                    games.Add(_dataBaseContext.Games.Include(rg => rg.roomGamers).ThenInclude(p => p.Person).ThenInclude(img => img.ProfileImageId).First(i => i.Id == item.GameId));
                }
            }
            
            ViewBag.Games = games;
            return View();
        }


    }
}
