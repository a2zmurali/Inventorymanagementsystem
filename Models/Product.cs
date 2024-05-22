
using System.ComponentModel.DataAnnotations;

namespace TevaInventorymanagementsystem.Models
{
    public class Product
    {
        public int Id { get; set; }        
        public string? Name { get; set; }        
        public string? Description { get; set; }      
        public int? Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
