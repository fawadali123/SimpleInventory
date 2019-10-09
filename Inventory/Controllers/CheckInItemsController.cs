using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;

namespace Inventory.Controllers
{
    [Authorize]
    public class CheckInItemsController : Controller
    {
        private InventoryEntities db = new InventoryEntities();

        // GET: CheckInItems
        public ActionResult Index()
        {
            var checkInItems = db.CheckInItems.Include(c => c.Item).Include(c => c.Vendor);
            return View(checkInItems.ToList());
        }

        // GET: CheckInItems/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckInItem checkInItem = db.CheckInItems.Find(id);
            if (checkInItem == null)
            {
                return HttpNotFound();
            }
            return View(checkInItem);
        }

        // GET: CheckInItems/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name");
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name");
            return View();
        }

        // POST: CheckInItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemId,Quantity,BuyingPrice,VendorId,ReceivingDate")] CheckInItem checkInItem)
        {
            if (ModelState.IsValid)
            {
                db.CheckInItems.Add(checkInItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", checkInItem.ItemId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", checkInItem.VendorId);
            return View(checkInItem);
        }

        // GET: CheckInItems/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckInItem checkInItem = db.CheckInItems.Find(id);
            if (checkInItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", checkInItem.ItemId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", checkInItem.VendorId);
            return View(checkInItem);
        }

        // POST: CheckInItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemId,Quantity,BuyingPrice,VendorId,ReceivingDate")] CheckInItem checkInItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkInItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", checkInItem.ItemId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", checkInItem.VendorId);
            return View(checkInItem);
        }

        // GET: CheckInItems/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckInItem checkInItem = db.CheckInItems.Find(id);
            if (checkInItem == null)
            {
                return HttpNotFound();
            }
            return View(checkInItem);
        }

        // POST: CheckInItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CheckInItem checkInItem = db.CheckInItems.Find(id);
            db.CheckInItems.Remove(checkInItem);
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
