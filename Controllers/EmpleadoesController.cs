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
    public class EmpleadoesController : Controller
    {
        private DataBase db = new DataBase();

        // GET: Empleadoes
        public ActionResult Index(string sortOrder, string searchString)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var emple = from s in db.Empleado
                               select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    emple = emple.Where(s => s.Nombre.ToUpper().Contains(searchString.ToUpper())
                     || s.ApellidoPat.ToUpper().Contains(searchString.ToUpper())
                     || s.ApellidoMat.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        emple = emple.OrderByDescending(s => s.Nombre);
                        break;
                    case "Date":
                        emple = emple.OrderBy(s => s.FechaAlta);
                        break;
                    case "date_desc":
                        emple = emple.OrderByDescending(s => s.FechaNacimiento);
                        break;
                    default:
                        emple = emple.OrderBy(s => s.Nombre);
                        break;
                }

                return View(emple.ToList());

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        // GET: Empleadoes/Details/5
        public ActionResult Details(Guid? id)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empleado empleado = db.Empleado.Find(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                return View(empleado);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Empleadoes/Create
        public ActionResult Create()
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                ViewBag.idDepartamento = new SelectList(db.Departamentos, "idDepartamento", "NombreDep");
                ViewBag.idEmpresa = new SelectList(db.Empresas, "idEmpresa", "NombreEmpresa");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Empleadoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmpleado,idEmpresa,idDepartamento,Nombre,ApellidoMat,ApellidoPat,TypeRol,direccion,CodigoPostal,Municipio,Estado,Pais,FechaNacimiento,Email,Sexo,Rol,Telefono1,Telefono2,FechaAlta")] Empleado empleado)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (ModelState.IsValid)
                {
                    empleado.idEmpleado = Guid.NewGuid();
                    db.Empleado.Add(empleado);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.idDepartamento = new SelectList(db.Departamentos, "idDepartamento", "NombreDep", empleado.idDepartamento);
                ViewBag.idEmpresa = new SelectList(db.Empresas, "idEmpresa", "NombreEmpresa", empleado.idEmpresa);
                return View(empleado);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Empleadoes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empleado empleado = db.Empleado.Find(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idDepartamento = new SelectList(db.Departamentos, "idDepartamento", "NombreDep", empleado.idDepartamento);
                ViewBag.idEmpresa = new SelectList(db.Empresas, "idEmpresa", "NombreEmpresa", empleado.idEmpresa);
                return View(empleado);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Empleadoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmpleado,idEmpresa,idDepartamento,Nombre,ApellidoMat,ApellidoPat,TypeRol,direccion,CodigoPostal,Municipio,Estado,Pais,FechaNacimiento,Email,Sexo,Rol,Telefono1,Telefono2,FechaAlta")] Empleado empleado)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(empleado).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.idDepartamento = new SelectList(db.Departamentos, "idDepartamento", "NombreDep", empleado.idDepartamento);
                ViewBag.idEmpresa = new SelectList(db.Empresas, "idEmpresa", "NombreEmpresa", empleado.idEmpresa);
                return View(empleado);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Empleadoes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            var estalogeado = User.Identity.IsAuthenticated;
            if (estalogeado)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empleado empleado = db.Empleado.Find(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                return View(empleado);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Empleado empleado = db.Empleado.Find(id);
            db.Empleado.Remove(empleado);
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
