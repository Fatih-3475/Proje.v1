namespace Proje.v1.Models
{
    public class Note
    {
        public int Id { get; set; }
        public String? Title { get; set; }
        public String? Content { get; set; }
        public bool IsArchived { get; set; } = false;
        public DateTime CreatedDate { get; set; }
    }
}
