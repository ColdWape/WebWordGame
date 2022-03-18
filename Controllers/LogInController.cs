using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebWordGame.Models;

namespace WebWordGame.Controllers
{
    public class LogInController : Controller
    {
        private readonly DataBaseContext _dataBaseContext;



        public LogInController(DataBaseContext dataBaseContext)
        {

            _dataBaseContext = dataBaseContext;
        }


        //Методы для регистрации
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                PersonModel person = await _dataBaseContext.People.FirstOrDefaultAsync(u => u.LoginName == model.LoginName);
                PersonModel personEmail = await _dataBaseContext.People.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (person == null && personEmail == null)
                {
                    // добавляем пользователя в бд
                    person = new PersonModel { LoginName = model.LoginName, Email = model.Email, Password = model.Password };
                    Role userRole = await _dataBaseContext.Roles.FirstOrDefaultAsync(r => r.Name == "person");
                    if (userRole != null)
                        person.Role = userRole;

                    GameInfoModel quantityOfPeople = _dataBaseContext.GameInfo.First();
                    quantityOfPeople.QuantityOfPlayers += 1;

                    ImageModel userImage = await _dataBaseContext.Images.FirstOrDefaultAsync(i => i.ImageSource == "../images/starting_profile_images/default_image.jpg");
                    person.ProfileImageId = userImage;
                    _dataBaseContext.People.Add(person);
                    await _dataBaseContext.SaveChangesAsync();

                    await Authenticate(person); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (person != null)
                    {
                        ModelState.AddModelError("", "Пользователь с таким логином уже зарегистрирован!");

                    }
                    if (personEmail != null)
                    {
                        ModelState.AddModelError("", "Пользователь с такой почтой уже зарегистрирован!");

                    }
                }
            }
            return View(model);
        }



        //Методы для входа 

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                PersonModel person = await _dataBaseContext.People
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.LoginName == model.LoginName && u.Password == model.Password);
                if (person != null)
                {
                    await Authenticate(person); // аутентификация

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }







        private async Task Authenticate(PersonModel person)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.LoginName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }


}
