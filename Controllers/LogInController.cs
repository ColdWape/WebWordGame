using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

       

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(PersonModel person)
        {

            _dataBaseContext.people.Add(person);
            _dataBaseContext.SaveChanges();

            return RedirectToAction("Registration");
        }



        
    }
}
