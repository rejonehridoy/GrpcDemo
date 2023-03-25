using System.Collections.Generic;

namespace ShoppingCartService.Models
{
    public class GrpcResponseModel<T> where T: class
    {
        public T Data { get; set; }
        public bool Success{ get; set; }
        public List<string> ErrorList { get; set; } = new List<string>();
    }
}
