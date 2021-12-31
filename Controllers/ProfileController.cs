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
            ViewBag.Human = _dataBaseContext.People.Include( i => i.ProfileImageId).First(i => i.LoginName == User.Identity.Name);
            ViewBag.UserAvatar = _dataBaseContext.Images;
            return View();
        }


        [HttpPost]
        public IActionResult UserPage(int PictureId) {
            if (PictureId != 0)
            {
                PersonModel person = _dataBaseContext.People.First(u => u.LoginName == User.Identity.Name);
                ImageModel newImage = _dataBaseContext.Images.First(u => u.Id == PictureId);

                person.ProfileImageId = newImage;
                _dataBaseContext.SaveChanges();
            }

            ViewBag.Human = _dataBaseContext.People.Include(i => i.ProfileImageId).First(i => i.LoginName == User.Identity.Name);
                ViewBag.UserAvatar = _dataBaseContext.Images;
            return View();
            //string test = $"Text: {person.ProfileImageId.ImageSource}";
            //return Content(test);
        }


    }
}
