using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Entities.Author;
using BookStore.API.Extensions;
using BookStore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(DataContext context, IMapper mapper, ILogger<AuthorController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadViewModel>>> GetAuthors()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<AuthorReadViewModel>>(await _context.Authors.ToListAsync()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing GET in {nameof(GetAuthors)} operation");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadViewModel>> GetAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    _logger.LogWarning($"Record not found: {nameof(GetAuthor)} - ID: {id}");
                    return NotFound();
                }

                return Ok(_mapper.Map<AuthorReadViewModel>(author));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing GET in {nameof(GetAuthor)} operation");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AuthorCreateViewModel>> PostAuthor(AuthorCreateViewModel authorCreateViewModel)
        {
            try
            {
                var author = _mapper.Map<Author>(authorCreateViewModel);

                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing POST in {nameof(PostAuthor)}", authorCreateViewModel);
                return StatusCode(500, Messages.Error500Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateViewModel authorUpdateViewModel)
        {
            if (id != authorUpdateViewModel.Id)
            {
                _logger.LogWarning($"Update ID invalid in {nameof(PutAuthor)} - ID: {id}");
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                _logger.LogWarning($"{nameof(Author)} record not found: {nameof(PutAuthor)} - ID: {id}");
                return NotFound();
            }

            _mapper.Map(authorUpdateViewModel, author);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"Error performing GET in {nameof(PutAuthor)} operation");
                    return StatusCode(500, Messages.Error500Message);
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id) => _context.Authors.Any(e => e.Id == id);
    }
}
