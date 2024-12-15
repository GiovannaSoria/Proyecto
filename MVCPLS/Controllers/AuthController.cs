using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPLS.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager _userManager = new UserManager();

        // GET: Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                var user = _userManager.Authenticate(username, password);
                Session["UserId"] = user.UserId;
                Session["Username"] = user.Username;
                Session["Role"] = user.Role;

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";
                return View();
            }
        }

        // GET: Auth/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
        public ActionResult Register(string username, string password, string email, string role)
        {
            try
            {
                _userManager.Register(username, password, email, role);
                return RedirectToAction("Login");
            }
            catch
            {
                ViewBag.ErrorMessage = "No se pudo registrar el usuario.";
                return View();
            }
        }

        // GET: Auth/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}