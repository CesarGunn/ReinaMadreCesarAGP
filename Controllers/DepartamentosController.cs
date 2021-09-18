using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GEmpresasEMM.Models;

namespace GEmpresasEMM.Controllers
{
    public class DepartamentosController : Controller
    {
        private DataBase db = new DataBase();

        // GET: Departamentos
        public ActionResult Index(string sortOrder, string searchString)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var depa = from s in db.Departamentos
                               select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    depa = depa.Where(s => s.NombreDep.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        depa = depa.OrderByDescending(s => s.NombreDep);
                        break;
                    case "Date":
                        depa = depa.OrderBy(s => s.FechaCreacion);
                        break;
                    case "date_desc":
                        depa = depa.OrderByDescending(s => s.fechaBaja);
                        break;
                    default:
                        depa = depa.OrderBy(s => s.NombreDep);
                        break;
                }

                return View(depa.ToList());

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Departamentos/Details/5
        public ActionResult Details(Guid? id)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Departamentos departamentos = db.Departamentos.Find(id);
                if (departamentos == null)
                {
                    return HttpNotFound();
                }
                return View(departamentos);
            }
            else
            {
                return RedirectToAction("Login", "Account"); ;
            };
}

        // GET: Departamentos/Create
        public ActionResult Create()
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                ViewBag.idEmpresa = new SelectList(db.Empresas, "idEmpresa", "NombreEmpresa");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account"); 
            }
        }

        // POST: Departamentos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDepartamento,idEmpresa,NombreDep,TipoArea,FechaCreacion,fechaBaja")] Departamentos departamentos)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (ModelState.IsValid)
                {
                    departamentos.idDepartamento = Guid.NewGuid();
                    db.Departamentos.Add(departamentos);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.idEmpresa = new SelectList(db.Empresas, "idEmpresa", "NombreEmpresa", departamentos.idEmpresa);
                return View(departamentos);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            
        }

        // GET: Departamentos/Edit/5
        public ActionResult Edit(Guid? id)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Departamentos departamentos = db.Departamentos.Find(id);
                if (departamentos == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idEmpresa = new SelectList(db.Empresas, "idEmpresa", "NombreEmpresa", departamentos.idEmpresa);
                return View(departamentos);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        // GET: Empresas/Departamentos/5
        public ActionResult DetallesEmpleadoDep(Guid? id, string sortOrder, string searchString)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var emp2 = from s in db.Empleado
                            where s.idDepartamento == id
                            select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    emp2 = emp2.Where(s => s.Nombre.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        emp2 = emp2.OrderByDescending(s => s.Nombre);
                        break;

                    default:
                        emp2 = emp2.OrderBy(s => s.Nombre);
                        break;
                }

                return View(emp2.ToList());

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // POST: Departamentos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDepartamento,idEmpresa,NombreDep,TipoArea,FechaCreacion,fechaBaja")] Departamentos departamentos)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(departamentos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.idEmpresa = new SelectList(db.Empresas, "idEmpresa", "NombreEmpresa", departamentos.idEmpresa);
                return View(departamentos);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Departamentos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Departamentos departamentos = db.Departamentos.Find(id);
                if (departamentos == null)
                {
                    return HttpNotFound();
                }
                return View(departamentos);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Departamentos departamentos = db.Departamentos.Find(id);
            db.Departamentos.Remove(departamentos);
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
