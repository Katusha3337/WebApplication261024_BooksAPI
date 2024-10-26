namespace WebApplication261024_BooksAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<int> BookIds { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
