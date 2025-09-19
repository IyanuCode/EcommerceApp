namespace Ecommerce.Data.Models
{
    public class EcommerceStore
    {
        public int Id { set; get; }
        public string? Product { get; set; }
        public string? Category { get; set; }
        public string? Orders { get; set; }

        public override string ToString()
        {
            return $"Id:{Id}, Product:{Product}, Category:{Category}, Orders:{Orders}";
        }
    }
}