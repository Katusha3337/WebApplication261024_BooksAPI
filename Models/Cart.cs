namespace WebApplication261024_BooksAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<int> BookIds { get; set; } = new List<int>();
    }
}
