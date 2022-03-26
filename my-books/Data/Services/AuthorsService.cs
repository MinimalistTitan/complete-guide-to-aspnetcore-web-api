using Microsoft.EntityFrameworkCore;
using my_books.Data.ViewModels;
using my_books.EntityModels;
using System.Net;
using System.Threading;

namespace my_books.Data.Services
{
    public class AuthorsService
    {
        private readonly MyBooksDbContext _context;

        public AuthorsService(MyBooksDbContext context)
        {
            _context = context;
        }

        public void AddAuthor(AuthorVM author)
        {
            var _author = new Authors()
            {
                FullName = author.FullName
            };

            _context.Authors.Add(_author);
            _context.SaveChanges();
        }

        public async Task<List<Books>> GetAllBooks() => await _context.Books.ToListAsync();

        public async Task<Books> GetBookById(int bookId) => await _context.Books.FirstOrDefaultAsync(x => x.Id == bookId);

        public Books UpdateBookById(int bookId, BookVM book)
        {
            var _book = _context.Books.FirstOrDefault(x => x.Id == bookId);

            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.DateRead;
                _book.Rate = book.Rate;
                _book.Genre = book.Genre;
                _book.CoverUrl = book.CoverUrl;

                _context.SaveChanges();
            }
            return _book;
        }

        public void DeleteBookById(int id)
{
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public AuthorWithBooksVM GetAuthorWithBooks(int authorId)
        {
            var _author = _context.Authors.Select(n => new AuthorWithBooksVM()
            {
                FullName = n.FullName,
                BookTitles = n.BookAuthors.Select(n => n.Book.Title).ToList()
            }).FirstOrDefault();

            return _author;
        }
    }
}
