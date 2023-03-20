using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartService.Models
{
    public class ShoppingCartItem
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int quantity { get; set; }
    }
}
