namespace ProniaWebApplication.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Image { get; set; } 
        public string? RedirectUrl { get; set; }
        public bool IsActive { get; set; }
    }

}
