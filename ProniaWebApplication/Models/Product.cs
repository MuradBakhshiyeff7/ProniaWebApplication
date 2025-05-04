namespace ProniaWebApplication.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal Price { get; set; }

        public string? MainImage { get; set; } 
        public string? HoverImage { get; set; } 
        public string? Description { get; set; } 
        public bool IsPrime { get; set; }

        public int CategoryId { get; set; }

        
        public Category? Category { get; set; }

    }

}
