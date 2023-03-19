namespace ShoppingCartService.Dtos
{
    public class ShoppingCartItemReadDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int quantity { get; set; }
    }
}
