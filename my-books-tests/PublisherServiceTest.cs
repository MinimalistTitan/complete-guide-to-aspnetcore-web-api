using Microsoft.EntityFrameworkCore;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.EntityModels;
using my_books.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace my_books_tests
{
    public class PublisherServiceTest
    {
        private static DbContextOptions<MyBooksDbContext> dbContextOptions = new DbContextOptionsBuilder<MyBooksDbContext>()
            .UseInMemoryDatabase(databaseName:"BookDbTest")
            .Options;

        MyBooksDbContext context;
        PublishersService publishersService; 

        [OneTimeSetUp]
        public void Setup()
        {
            context = new MyBooksDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();

            publishersService = new PublishersService(context);
        }

        [Test, Order(1)]
        public void GetAllPublisher_WithNoSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublisher("", "", null);

            Assert.That(result.Count, Is.EqualTo(5));
            //Assert.AreEqual(result.Count, 5);
        }

        [Test, Order(2)]
        public void GetAllPublisher_WithNoSortBy_WithNoSearchString_WithPageNumber_Test()
        {
            var result = publishersService.GetAllPublisher("", "", 2);

            Assert.That(result.Count, Is.EqualTo(1));
            //Assert.AreEqual(result.Count, 3);
        }

        [Test, Order(3)]
        public void GetAllPublisher_WithNoSortBy_WithSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublisher("", "3", null);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 3"));
        }

        [Test, Order(4)]
        public void GetAllPublisher_WithSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublisher("name_desc", "", null);

            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 6"));
        }


        [Test]
        public void GetPublisherById_Test()
        {
            var result = publishersService.GetPublisherById(1);

            Assert.That(result.PublisherId, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Publisher 1"));
        }

        [Test]
        public void GetAllPublisherById_WithoutResponse_Test()
        {
            var result = publishersService.GetPublisherById(99);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddPublisher_WithException_Test()
        {
            var newPublisher = new PublisherVM
            {
                Name = "123 with exception"
            };

            Assert.That(() => publishersService.AddPublisher(newPublisher), Throws.Exception.TypeOf<PublisherNameException>().With.Message.EqualTo
                ("Name starts with number"));
        }

        [Test]
        public void AddPublisher_WithoutException_Test()
        {
            var newPublisher = new PublisherVM
            {
                Name = "without exception"
            };

            var result = publishersService.AddPublisher(newPublisher);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Does.StartWith("without"));
            Assert.That(result.PublisherId, Is.Not.Null);
        }

        [Test]
        public void GetPublisherData_Test()
        {

            var result = publishersService.GetPublisherData(1);

            Assert.That(result.Name, Is.EqualTo("Publisher 1"));
            Assert.That(result.BookAuthors.Count, Is.GreaterThan(0));
            Assert.That(result.BookAuthors, Is.Not.Empty);

            var firstBookName = result.BookAuthors.OrderBy(n => n.BookName).FirstOrDefault().BookName;
            Assert.That(firstBookName, Is.EqualTo("Book 1 Title"));
        }


        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        private void SeedDatabase()
        {
            var publishers = new List<Publishers>
            {
                new Publishers(){PublisherId = 1, Name = "Publisher 1"},
                new Publishers(){PublisherId = 2, Name = "Publisher 2"},
                new Publishers(){PublisherId = 3, Name = "Publisher 3"},
                new Publishers(){PublisherId = 4, Name = "Publisher 4"},
                new Publishers(){PublisherId = 5, Name = "Publisher 5"},
                new Publishers(){PublisherId = 6, Name = "Publisher 6"},
            };

            context.AddRange(publishers);


            var authors = new List<Authors>
            {
                new Authors(){Id = 1, FullName = "Author 1"},
                new Authors(){Id = 2, FullName = "Author 2"},
                new Authors(){Id = 3, FullName = "Author 3"},
                
            };

            context.AddRange(authors);

            var books = new List<Books>
            {
                new Books()
                {
                    Id = 1,
                    Title = "Book 1 Title",
                    Description = "Book 1 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1,
                },
                new Books()
                {
                    Id = 2,
                    Title = "Book 2 Title",
                    Description = "Book 2 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1,
                }
            };

            context.AddRange(books);

            var books_authors = new List<BookAuthors>
            {
                new BookAuthors
                {
                    Id = 1,
                    BookId = 1,
                    AuthorId = 1,
                },
                new BookAuthors
                {
                    Id = 2,
                    BookId = 1,
                    AuthorId = 2,
                },
                new BookAuthors
                {
                    Id = 3,
                    BookId = 2,
                    AuthorId = 2,
                },
            };

            context.AddRange(books_authors);

            context.SaveChanges();

        }
    }
}