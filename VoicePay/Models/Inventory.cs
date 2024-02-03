using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VoicePay.Models
{
    public class Inventory
    {
        public int? InventoryID { get; set; }
        public string? ItemName { get; set; }
        public int? Quantity { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierContactNo { get; set; }

        public string? AccId { get; set; }      
    }
}
