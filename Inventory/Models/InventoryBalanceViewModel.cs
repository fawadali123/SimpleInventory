using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Models
{
    public class InventoryBalanceViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public double CheckInQty { get; set; }
        public double CheckOutQty { get; set; }
        public double Balance { get; set; }

    }
}