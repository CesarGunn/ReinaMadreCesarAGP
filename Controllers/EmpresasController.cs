using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GEmpresasEMM.Models;
using Microsoft.AspNet.Identity;

namespace GEmpresasEMM.Controllers
{
    public class EmpresasController : Controller
    {
        private DataBase db = new DataBase();

        // GET: Empresas

        public ActionResult Index(string sortOrder, string searchString)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var empresas = from s in db.Empresas
                               select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    empresas = empresas.Where(s => s.NombreEmpresa.ToUpper().Contains(searchString.ToUpper())
                     || s.Sobrenombre.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        empresas = empresas.OrderByDescending(s => s.NombreEmpresa);
                        break;
                    case "Date":
                        empresas = empresas.OrderBy(s => s.FechaCreacion);
                        break;
                    case "date_desc":
                        empresas = empresas.OrderByDescending(s => s.FechaBaja);
                        break;
                    default:
                        empresas = empresas.OrderBy(s => s.NombreEmpresa);
                        break;
                }

                return View(empresas.ToList());

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

         


        // GET: Empresas/Details/5
        public ActionResult Details(Guid? id)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                var NombreUsr = User.Identity.Name;
                var idU = User.Identity.GetUserId();

                using (DataBase db = new DataBase())
                {
                    var usuario = db.AspNetUsers.Where(x => x.Id == idU).FirstOrDefault();
                    var emailconf = usuario.EmailConfirmed;

                }
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empresas empresas = db.Empresas.Find(id);
                if (empresas == null)
                {
                    return HttpNotFound();
                }
                return View(empresas);
            }
            else
            {
                return RedirectToAction("Login", "Account"); ;
            };

            
        }

        // GET: Empresas/Create
        public ActionResult Create()
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
                return RedirectToAction("Login", "Account"); ;
            };
            
        }

        // POST: Empresas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmpresa,NombreEmpresa,Direccion,CodigoPostal,Municipio,Estado,Pais,Sobrenombre,FechaCreacion,RFC,FechaBaja,observaciones")] Empresas empresas)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {

                if (ModelState.IsValid)
                {
                    empresas.idEmpresa = Guid.NewGuid();
                    db.Empresas.Add(empresas);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(empresas);
            }
            else
            {
                return RedirectToAction("Login", "Account"); ;
            };
            
        }

        // GET: Empresas/Edit/5
        public ActionResult Edit(Guid? id)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {

                 if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresas empresas = db.Empresas.Find(id);
            if (empresas == null)
            {
                return HttpNotFound();
            }
            return View(empresas);
            }
            else
            {
                return RedirectToAction("Login", "Account"); ;
            };
            
        }

        // GET: Empresas/Departamentos/5
        public ActionResult DetalleDepartamentosEmp(Guid? id, string sortOrder, string searchString)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var depa2 = from s in db.Departamentos
                            where s.idEmpresa == id
                            select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    depa2 = depa2.Where(s => s.NombreDep.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        depa2 = depa2.OrderByDescending(s => s.NombreDep);
                        break;
                     
                    default:
                        depa2 = depa2.OrderBy(s => s.NombreDep);
                        break;
                }

                return View(depa2.ToList());

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // POST: Empresas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmpresa,NombreEmpresa,Direccion,CodigoPostal,Municipio,Estado,Pais,Sobrenombre,FechaCreacion,RFC,FechaBaja,observaciones")] Empresas empresas)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(empresas).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(empresas);

            }
            else
            {
                return RedirectToAction("Login", "Account"); ;
            };

           
        }

        // GET: Empresas/Delete/5
        public ActionResult Delete(Guid? id)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empresas empresas = db.Empresas.Find(id);
                if (empresas == null)
                {
                    return HttpNotFound();
                }
                return View(empresas);

            }
            else
            {
                return RedirectToAction("Login", "Account"); ;
            };
            
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Empresas empresas = db.Empresas.Find(id);
            db.Empresas.Remove(empresas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
