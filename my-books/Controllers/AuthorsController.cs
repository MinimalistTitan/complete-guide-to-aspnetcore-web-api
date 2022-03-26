using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModels;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        public AuthorsService _authorService;

        public AuthorsController(AuthorsService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("get-author-with-books-by-id/{id}")]
        public IActionResult GetAuthorWithBooksById(int id)
        {
            var _authorWithBooks = _authorService.GetAuthorWithBooks(id);

            return Ok(_authorWithBooks);
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody]AuthorVM author)
        {
            _authorService.AddAuthor(author);
            return Ok();
        }
    }
}
