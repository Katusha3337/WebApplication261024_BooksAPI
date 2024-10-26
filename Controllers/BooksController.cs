using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication261024_BooksAPI.Data;
using WebApplication261024_BooksAPI.Models;

namespace WebApplication261024_BooksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationContext db;

        public BooksController(ApplicationContext context)
        {
            db = context;
            if (!db.Books.Any())
            {
                db.Books.Add(new Book { Title = "Love in the City", Author = "Jane Doe", Genre = "Romance" });
                db.Books.Add(new Book { Title = "Moonlit Nights", Author = "John Smith", Genre = "Romance" });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            return await db.Books.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var book = await db.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return new ObjectResult(book);
        }

        [HttpPost("cart/{userId}")]
        public async Task<ActionResult> AddToCart(int userId, [FromBody] int bookId)
        {
            var cart = await db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                db.Carts.Add(cart);
            }
            cart.BookIds.Add(bookId);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("cart/{userId}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetCart(int userId)
        {
            var cart = await db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                return NotFound();
            }
            var cartBooks = await db.Books.Where(b => cart.BookIds.Contains(b.Id)).ToListAsync();
            return cartBooks;
        }

        [HttpPost("order/{userId}")]
        public async Task<ActionResult> CreateOrder(int userId)
        {
            var cart = await db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || !cart.BookIds.Any())
            {
                return BadRequest("Cart is empty");
            }
            var order = new Order
            {
                UserId = userId,
                BookIds = new List<int>(cart.BookIds),
                OrderDate = DateTime.Now
            };
            db.Orders.Add(order);
            cart.BookIds.Clear();
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("orders/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int userId)
        {
            var userOrders = await db.Orders.Where(o => o.UserId == userId).ToListAsync();
            if (!userOrders.Any())
            {
                return NotFound();
            }
            return userOrders;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooks([FromQuery] string title = "", [FromQuery] string author = "", [FromQuery] string genre = "")
        {
            var books = await db.Books.ToListAsync();

            if (!string.IsNullOrEmpty(title))
            {
                books = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(author))
            {
                books = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(genre))
            {
                books = books.Where(b => b.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return books;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Post(Book book)
        {
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut]
        public async Task<ActionResult<Book>> Put(Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }
            if (!db.Books.Any(x => x.Id == book.Id))
            {
                return NotFound();
            }
            db.Update(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> Delete(int id)
        {
            var book = db.Books.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }
    }
}