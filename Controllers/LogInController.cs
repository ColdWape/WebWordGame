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



        //[HttpPost]
        //public IActionResult Registration(RegisterModel register)
        //{
        //    PersonModel enableUser = _dataBaseContext.People.FirstOrDefault(p => p.LoginName == register.LoginName);
        //    if (enableUser.LoginName != null)
        //    {
        //        ModelState.AddModelError("LoginName", $"Пользователь с логином {enableUser.LoginName} уже зарегистрирован!"); //Спросить !
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _dataBaseContext.People.Add(register);
        //        _dataBaseContext.SaveChanges();
        //        return RedirectToAction("Login");
        //    }
        //    return View(register);

        //}



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










        private async Task Authenticate(PersonModel person)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
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
