using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebWordGame.Models;

namespace WebWordGame.Controllers
{
    public class AdminZoneController : Controller
    {
        private readonly DataBaseContext _dataBaseContext;
        public AdminZoneController(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult addWordAtDb()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult addWordAtDb(string word, string verifyWord)
        {
            if (word != null || verifyWord != null)
            {
                word = word.ToLower();
                verifyWord = verifyWord.ToLower();

                if (word == verifyWord)
                {
                    if (_dataBaseContext.Words.FirstOrDefault(w => w.Name == word) == null)
                    {
                        WordModel newWord = new WordModel
                        {
                            Name = word,
                            NumberOfUses = 1
                        };

                        _dataBaseContext.Words.Add(newWord);
                        _dataBaseContext.SaveChanges();
                        ViewBag.Alert = "Слово успешно добавлено";
                        return View();

                    }
                    else
                    {
                        ViewBag.Alert = "Слово уже есть в базе";
                        return View();

                    }

                }
                else
                {
                    ViewBag.Alert = "Слова не совпадают";
                    return View();
                }
            }
            else
            {
                ViewBag.Alert = "Оба поля должны быть заполнены";
                return View();
            }
            

        }
    }
}
