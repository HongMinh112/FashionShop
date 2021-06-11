using FashionWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionWebsite.Controllers
{
    public class AdminController : Controller
    {
        WebShoseEntities context = new WebShoseEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if(Session["EMLOYEE"]==null)
            {
                return RedirectToAction("Login");
            }    
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(EMLOYEE eMLOYEE)
        {
            if (eMLOYEE == null)
            {
                return null;
            }
            else
            {
                try
                {
                    EMLOYEE emloyeeCheck = context.EMLOYEEs.Single(x => x.EMAIL == eMLOYEE.EMAIL && x.PASSWORD == eMLOYEE.PASSWORD);
                    Session["EMLOYEE"] = emloyeeCheck;
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
        }
        
    }
}