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
    public class ItemsController : Controller
    {
        private InventoryEntities db = new InventoryEntities();

        // GET: Items
        public ActionResult Index()
        {
            var items = db.Items.Include(i => i.Category);
            return View(items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,Description,IsActive,CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", item.CategoryId);
            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", item.CategoryId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,Description,IsActive,CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", item.CategoryId);
            return View(item);
        }
        public ActionResult Balance()
        {
            List<InventoryBalanceViewModel> result = GetBalance(0);
            return View(result);
        }
        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetItemBalance(int? itemId)
        {
            return Json(GetBalance(itemId));
        }
        public List<InventoryBalanceViewModel> GetBalance(int? itemId)
        {
            string query = $@"SELECT i.Id, i.Code, i.Name, i.IsActive,
       i.CategoryId, c.Name                  Category, ISNULL(checkin.Quantity,0.0)        CheckInQty,
       ISNULL(checkout.Quantity,0.0)       CheckOutQty, ISNULL(checkin.Quantity,0.0) - ISNULL(checkout.Quantity,0.0) Balance
        FROM   Item                 AS i
       INNER JOIN Category  AS c
            ON  c.Id = i.CategoryId
       LEFT JOIN (
                SELECT SUM(cii.Quantity)     Quantity,
                       cii.ItemId
                FROM   CheckInItem        AS cii
                GROUP BY
                       cii.ItemId
            ) checkIn
            ON  checkIn.ItemId = i.Id
       LEFT JOIN (
                SELECT SUM(coi.Quantity)     Quantity,
                       coi.ItemId
                FROM   CheckOutItem       AS coi
                GROUP BY
                       coi.ItemId
            ) checkout
            ON  checkOut.ItemId = i.Id WHERE i.Id={itemId} OR {itemId}=0";
            var result = db.Database.SqlQuery<InventoryBalanceViewModel>(query).ToList();
            return result;
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


