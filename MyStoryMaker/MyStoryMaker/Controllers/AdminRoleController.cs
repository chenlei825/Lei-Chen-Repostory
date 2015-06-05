using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyStoryMaker.Models;

namespace MyStoryMaker.Controllers
{
    public class AdminRoleController : Controller
    {
        private StoryContext db = new StoryContext();

        // GET: /AdminRole/
        public ActionResult Index()
        {
            return View(db.AdminRoles.ToList());
        }

        // GET: /AdminRole/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminRole adminrole = db.AdminRoles.Find(id);
            if (adminrole == null)
            {
                return HttpNotFound();
            }
            return View(adminrole);
        }

        // GET: /AdminRole/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /AdminRole/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,role")] AdminRole adminrole)
        {
            if (ModelState.IsValid)
            {
                db.AdminRoles.Add(adminrole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminrole);
        }

        // GET: /AdminRole/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminRole adminrole = db.AdminRoles.Find(id);
            if (adminrole == null)
            {
                return HttpNotFound();
            }
            return View(adminrole);
        }

        // POST: /AdminRole/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,role")] AdminRole adminrole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminrole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminrole);
        }

        // GET: /AdminRole/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminRole adminrole = db.AdminRoles.Find(id);
            if (adminrole == null)
            {
                return HttpNotFound();
            }
            return View(adminrole);
        }

        // POST: /AdminRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminRole adminrole = db.AdminRoles.Find(id);
            db.AdminRoles.Remove(adminrole);
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
