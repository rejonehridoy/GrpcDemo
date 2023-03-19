namespace ShoppingCartService.Dtos
{
    public class ShoppingCartItemCreateDto
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
    }
}
