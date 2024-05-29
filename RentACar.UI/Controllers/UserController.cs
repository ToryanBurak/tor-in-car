using RentACar.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentACar.UI.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}