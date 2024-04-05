using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.API.Data;
using BookStore.API.Entities.Book;
using BookStore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BookController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadViewModel>>> GetBooks()
        {
            return Ok(await _context.Books
                .Include(x => x.Author)
                .ProjectTo<BookReadViewModel>(_mapper
                .ConfigurationProvider)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context
                .Books
                .Include(x => x.Author)
                .ProjectTo<BookDetailsViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(y => y.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookCreateViewModel>> PostBook(BookCreateViewModel bookCreateViewModel)
        {
            var book = _mapper.Map<Book>(bookCreateViewModel);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateViewModel bookUpdateViewModel)
        {
            if (id != bookUpdateViewModel.Id)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _mapper.Map(bookUpdateViewModel, book);
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id) => _context.Books.Any(e => e.Id == id);
    }
}
