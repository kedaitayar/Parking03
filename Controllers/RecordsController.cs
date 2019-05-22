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
    public class RecordsController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: Records
        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["ID"]);
            var userRecord = from x in db.Records
                          where x.cid == id
                          select x;
            //var records = db.Records.Include(r => r.Car).Include(r => r.Parking).Include(r => r.User);
            return View(userRecord);
        }

        // GET: Records/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // GET: Records/Create
        [Authorize(Roles = "Customer")]
        public ActionResult Create()
        {
            int id = Convert.ToInt32(Session["ID"]);
            ViewBag.plateno = from x in db.Cars
                              where x.cid == id
                              select x;
            ViewBag.parkid = from y in db.Parkings
                              select y;
            
            //var userCar = from x in db.Cars
            //              where x.cid == id
            //              select x;
            //ViewBag.plateno = new SelectList(db.Cars, "plateno", "carname", userCar);
            //ViewBag.parkid = new SelectList(db.Parkings, "parkid", "areaname");
            ViewBag.cid = new SelectList(db.Users, "Id", "pass");
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult Create([Bind(Include = "id,date,duration,parkid,plateno,price,cid")] Record record)
        {
            if (ModelState.IsValid)
            {
                record.cid = Convert.ToInt32(Session["ID"]);
                var rate = (from x in db.Parkings
                            where x.parkid == record.parkid
                            select x.rate).FirstOrDefault();
                record.price = record.duration * rate;
                record.date = DateTime.Now;
                db.Records.Add(record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.plateno = new SelectList(db.Cars, "plateno", "carname", record.plateno);
            //ViewBag.parkid = new SelectList(db.Parkings, "parkid", "areaname", record.parkid);
            //ViewBag.cid = new SelectList(db.Users, "Id", "pass", record.cid);
            return View(record);
        }

        //[HttpGet]
        //public ActionResult ConfirmRecord()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult ConfirmRecord([Bind(Include = "id,date,duration,parkid,plateno,price,cid")] Record record)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //string date = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
        //        var rate = (from x in db.Parkings
        //                   where x.parkid == record.parkid
        //                   select x.rate).FirstOrDefault();

        //        var price = record.duration;
        //        record.duration = 10;
        //        record.price = 5554;
        //        record.date = DateTime.Now;
        //        record.cid = Convert.ToInt32(Session["ID"]);
        //        db.Records.Add(record);
        //        db.SaveChanges();
        //        //return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        // GET: Records/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            ViewBag.plateno = new SelectList(db.Cars, "plateno", "carname", record.plateno);
            ViewBag.parkid = new SelectList(db.Parkings, "parkid", "areaname", record.parkid);
            ViewBag.cid = new SelectList(db.Users, "Id", "pass", record.cid);
            return View(record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,date,duration,parkid,plateno,price,cid")] Record record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.plateno = new SelectList(db.Cars, "plateno", "carname", record.plateno);
            ViewBag.parkid = new SelectList(db.Parkings, "parkid", "areaname", record.parkid);
            ViewBag.cid = new SelectList(db.Users, "Id", "pass", record.cid);
            return View(record);
        }

        // GET: Records/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Record record = db.Records.Find(id);
            db.Records.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult CheckCar()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult CheckCar(string plateno)
        //{
        //    var result = (from x in db.Records
        //                 where x.plateno == plateno
        //                 select x).FirstOrDefault();
        //    return RedirectToAction("Details", new { id=plateno});
        //}

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult CheckCarDetails()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult CheckCarDetails(string plateno)
        {
            var result = (from x in db.Records
                          where x.plateno == plateno
                          select x).FirstOrDefault();
            return View(result);
        }

        [Authorize(Roles = "Staff, Admin")]
        public ActionResult AllRecords()
        {
            var userRecord = from x in db.Records
                             select x;
            //var records = db.Records.Include(r => r.Car).Include(r => r.Parking).Include(r => r.User);
            return View(userRecord);
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
