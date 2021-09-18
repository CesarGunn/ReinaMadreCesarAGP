using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GEmpresasEMM.Models;

namespace GEmpresasEMM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                var NombreUsr = User.Identity.Name;
                var id = User.Identity.GetUserId();

                using (DataBase db = new DataBase())
                {
                    var usuario = db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
                    var emailconf = usuario.EmailConfirmed; 
                     
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account"); 
            };
           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Examen solicitado por la empresa medica reina madre.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Pagina de contacto";

            return View();
        }
    }
}