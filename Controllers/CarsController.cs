using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Parking03.Models;

namespace Parking03.Controllers
{
    public class CarsController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: Cars
        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["ID"]);
            var userCar = from x in db.Cars
                          where x.cid == id
                          select x;
            return View(userCar);
            //var cars = db.Cars.Include(c => c.User);
            //return View(cars.ToList());
        }

        // GET: Cars/Details/5
        [Authorize(Roles = "Customer")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        [Authorize(Roles = "Customer")]
        public ActionResult Create()
        {
            //ViewBag.cid = new SelectList(db.Users, "Id", "pass");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult Create([Bind(Include = "plateno,carname,color,cid")] Car car)
        {
            if (ModelState.IsValid)
            {
                car.cid = Convert.ToInt32(Session["ID"]);
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cid = new SelectList(db.Users, "Id", "pass", car.cid);
            return View(car);
        }

        // GET: Cars/Edit/5
        [Authorize(Roles = "Customer")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.cid = new SelectList(db.Users, "Id", "pass", car.cid);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult Edit([Bind(Include = "plateno,carname,color,cid")] Car car)
        {
            if (ModelState.IsValid)
            {
                car.cid = Convert.ToInt32(Session["ID"]);
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cid = new SelectList(db.Users, "Id", "pass", car.cid);
            return View(car);
        }

        // GET: Cars/Delete/5
        [Authorize(Roles = "Customer")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult DeleteConfirmed(string id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
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
