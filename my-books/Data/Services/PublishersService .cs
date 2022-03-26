using Microsoft.EntityFrameworkCore;
using my_books.Data.Models;
using my_books.Data.Paging;
using my_books.Data.ViewModels;
using my_books.EntityModels;
using my_books.Exceptions;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace my_books.Data.Services
{
    public class PublishersService
    {
        private readonly MyBooksDbContext _context;

        public PublishersService(MyBooksDbContext context)
        {
            _context = context;
        }

        public Publishers AddPublisher(PublisherVM publisher)
        {
            if (StringStartsWithNumber(publisher.Name)) throw new PublisherNameException("Name starts with number", publisher.Name);

            var _publisher = new Publishers()
            {
                Name = publisher.Name
            };

            _context.Publishers.Add(_publisher);
            _context.SaveChanges();

            return _publisher;
        }

        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
        {
            var _publisherData = _context.Publishers.Where(n => n.PublisherId == publisherId)
                .Select(n => new PublisherWithBooksAndAuthorsVM()
                {
                    Name = n.Name,
                    BookAuthors = n.Books.Select(n => new BookAuthorVM()
                    {
                        BookName = n.Title,
                        BookAuthors = n.BookAuthors.Select(n => n.Author.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();

            return _publisherData;
        }

        public Publishers GetPublisherById(int publisherId) => _context.Publishers.FirstOrDefault(x => x.PublisherId == publisherId);

        public void DeletePublisherById(int id)
        {
            var _publisher = _context.Publishers.FirstOrDefault(x => x.PublisherId == id);

            if (_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"The publisher with id: {id} does not exist");
            }
        }

        public List<Publishers> GetAllPublisher(string sortBy, string searchString, int? pageNumber)
        {

            var allPublishers = _context.Publishers.OrderBy(n => n.Name).ToList();

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        allPublishers = _context.Publishers.OrderByDescending(n => n.Name).ToList();
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                allPublishers = allPublishers.Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            // Paging
            int pageSize = 5;
            allPublishers = PaginatedList<Publishers>.Create(allPublishers.AsQueryable(), pageNumber ?? 1, pageSize);

            return allPublishers;
        } 
       
        private bool StringStartsWithNumber(string name) => Regex.IsMatch(name, @"^\d");
    }
}
