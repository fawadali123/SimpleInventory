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
    public class CheckOutItemsController : Controller
    {
        private InventoryEntities db = new InventoryEntities();

        // GET: CheckOutItems
        public ActionResult Index()
        {
            var checkOutItems = db.CheckOutItems.Include(c => c.Item).Include(c => c.Staff);
            return View(checkOutItems.ToList());
        }

        // GET: CheckOutItems/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckOutItem checkOutItem = db.CheckOutItems.Find(id);
            if (checkOutItem == null)
            {
                return HttpNotFound();
            }
            return View(checkOutItem);
        }

        // GET: CheckOutItems/Create
        public ActionResult Create()
        {
            var items = db.Items.Where(i => i.IsActive == true).ToList();
            List<Item> itemList = new List<Item>();
            itemList.Add(new Item() { Id = 0, Name = "-- SELECT ITEM --", Code = "00" });
            foreach (var item in items)
            {
                itemList.Add(item);
            }
            ViewBag.ItemId = new SelectList(itemList, "Id", "Name");
            ViewBag.IssueTo_StaffId = new SelectList(db.Staffs, "Id", "Name");
            return View();
        }

        // POST: CheckOutItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemId,Quantity,SellingPrice,IssueTo_StaffId,IssueDate")] CheckOutItem checkOutItem)
        {
            if (ModelState.IsValid)
            {
                db.CheckOutItems.Add(checkOutItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", checkOutItem.ItemId);
            ViewBag.IssueTo_StaffId = new SelectList(db.Staffs, "Id", "Name", checkOutItem.IssueTo_StaffId);
            return View(checkOutItem);
        }

        // GET: CheckOutItems/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckOutItem checkOutItem = db.CheckOutItems.Find(id);
            if (checkOutItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", checkOutItem.ItemId);
            ViewBag.IssueTo_StaffId = new SelectList(db.Staffs, "Id", "Name", checkOutItem.IssueTo_StaffId);
            return View(checkOutItem);
        }

        // POST: CheckOutItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemId,Quantity,SellingPrice,IssueTo_StaffId,IssueDate")] CheckOutItem checkOutItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkOutItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", checkOutItem.ItemId);
            ViewBag.IssueTo_StaffId = new SelectList(db.Staffs, "Id", "Name", checkOutItem.IssueTo_StaffId);
            return View(checkOutItem);
        }

        // GET: CheckOutItems/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckOutItem checkOutItem = db.CheckOutItems.Find(id);
            if (checkOutItem == null)
            {
                return HttpNotFound();
            }
            return View(checkOutItem);
        }

        // POST: CheckOutItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CheckOutItem checkOutItem = db.CheckOutItems.Find(id);
            db.CheckOutItems.Remove(checkOutItem);
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
