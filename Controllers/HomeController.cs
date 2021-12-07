using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebWordGame.Models;

namespace WebWordGame.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly DataBaseContext _dataBaseContext;

        
        public HomeController(DataBaseContext dataBaseContext)
        {
            
            _dataBaseContext = dataBaseContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin, person")]
        public IActionResult GameTypeChoosingPage()
        {
            return View();
        }
    }
}
