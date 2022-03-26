using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using my_books.Controllers;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.EntityModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_books_tests
{
    public class PublishersControllerTests
    {
        private static DbContextOptions<MyBooksDbContext> dbContextOptions = new DbContextOptionsBuilder<MyBooksDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbControllerTest")
            .Options;

        MyBooksDbContext context;
        PublishersService publishersService;
        PublishersController publisherController;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new MyBooksDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();

            publishersService = new PublishersService(context);
            publisherController = new PublishersController(publishersService, new NullLogger<PublishersController>());
        }

        [Test, Order(1)]
        public void HTTPGET_GetAllPublishers_WithSortBy_WithNSearchString_WithNPageNumber_ReturnOk_Test()
        {
            IActionResult actionResult = publisherController.GetAllPublisher("name_desc", "Publisher", 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as List<Publishers>;
            Assert.That(actionResultData.First().Name, Is.EqualTo("Publisher 6"));
            Assert.That(actionResultData.First().PublisherId, Is.EqualTo(6));
            Assert.That(actionResultData.Count(), Is.EqualTo(5));



            IActionResult actionResultSecondPage = publisherController.GetAllPublisher("name_desc", "Publisher", 2);
            Assert.That(actionResultSecondPage, Is.TypeOf<OkObjectResult>());
            var actionResultDataSecondPage = (actionResultSecondPage as OkObjectResult).Value as List<Publishers>;
            Assert.That(actionResultDataSecondPage.First().Name, Is.EqualTo("Publisher 1"));
            Assert.That(actionResultDataSecondPage.First().PublisherId, Is.EqualTo(1));
            Assert.That(actionResultDataSecondPage.Count(), Is.EqualTo(1));
        }

        [Test, Order(2)]
        public void HTTPGET_GetPublisherById_ReturnsOk_Test()
        {
            int publisherId = 1;

            IActionResult actionResult = publisherController.GetPublisherById(publisherId);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as Publishers;
            Assert.That(actionResultData.Name, Is.EqualTo("publisher 1").IgnoreCase);
            Assert.That(actionResultData.PublisherId, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public void HTTPGET_GetPublisherById_ReturnsNotFound_Test()
        {
            int publisherId = 99;

            IActionResult actionResult = publisherController.GetPublisherById(publisherId);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test, Order(4)]
        public void HTTPPOST_AddPublisher_ReturnsCreated_Test()
        {
            var newPublisherVM = new PublisherVM
            {
                Name = "New Publisher"
            };

            IActionResult actionResult = publisherController.AddPublisher(newPublisherVM);

            Assert.That(actionResult, Is.TypeOf<CreatedResult>());
        }

        [Test, Order(5)]
        public void HTTPPOST_AddPublisher_ReturnsBadRequest_Test()
        {
            var newPublisherVM = new PublisherVM
            {
                Name = "123 New Publisher"
            };

            IActionResult actionResult = publisherController.AddPublisher(newPublisherVM);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(6)]
        public void HTTPPOST_DeletePublisher_ReturnsOk_Test()
        {
            IActionResult actionResult = publisherController.DeletePublisherById(2);

            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public void HTTPPOST_DeletePublisher_ReturnsBadRequest_Test()
        {
            IActionResult actionResult = publisherController.DeletePublisherById(99);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
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

            context.SaveChanges();

        }
    }
}
