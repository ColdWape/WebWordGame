using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public IActionResult BattleField()
        {
            return View();
        }
    }
}
