using Microsoft.EntityFrameworkCore;
using my_books.Data.Models;
using my_books.Data.ViewModels;
using my_books.EntityModels;
using System.Net;
using System.Threading;

namespace my_books.Data.Services
{
    public class BooksService
    {
        private readonly MyBooksDbContext _context;

        public BooksService(MyBooksDbContext context)
        {
            _context = context;
        }

        public void AddBookWithAuthors(BookVM book)
        {
            var _book = new Books()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.DateRead,
                Rate = book.Rate,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublishedId
            };

            _context.Books.Add(_book);
            _context.SaveChanges();

            foreach (var id in book.AuthorIds)
            {
                var _book_author = new BookAuthors()
                {
                    BookId = _book.Id,
                    AuthorId = id
                };

                _context.BookAuthors.Add(_book_author);
                _context.SaveChanges();
            }
        }

        public async Task<List<Books>> GetAllBooks() => await _context.Books.ToListAsync();

        public BookWithAuthorsVM GetBookById(int bookId) 
        {
            var _bookWithAuthors = _context.Books.Where(n => n.Id == bookId).Select(book => new BookWithAuthorsVM()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead ,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.Rate,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.BookAuthors.Select(n => n.Author.FullName).ToList()
            }).FirstOrDefault();

            return _bookWithAuthors;
        } 

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
    }
}
