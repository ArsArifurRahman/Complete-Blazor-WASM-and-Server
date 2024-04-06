using API.Data;
using API.Entities.Models;
using API.Entities.ViewModels.Book;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
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
        public async Task<ActionResult<IEnumerable<BookRead>>> GetBooks()
        {
            return await _context
                .Books
                .Include(x => x.Author)
                .ProjectTo<BookRead>(_mapper
                .ConfigurationProvider)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetails>> GetBook(int id)
        {
            var book = await _context
                .Books
                .Include(x => x.Author)
                .ProjectTo<BookDetails>(_mapper
                .ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookCreate>> PostBook(BookCreate bookCreate)
        {
            var book = _mapper.Map<Book>(bookCreate);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdate bookUpdate)
        {
            if (id != bookUpdate.Id)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _mapper.Map(bookUpdate, book);
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

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
