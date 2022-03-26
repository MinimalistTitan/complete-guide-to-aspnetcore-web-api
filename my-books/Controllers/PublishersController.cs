using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.ActionResults;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.EntityModels;
using my_books.Exceptions;
using System.Text.RegularExpressions;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        public PublishersService _publisherService;
        private readonly ILogger<PublishersController> _logger;
        public PublishersController(PublishersService publishersService, ILogger<PublishersController> logger)
        {
            _publisherService = publishersService;
            _logger = logger;
        }

        [HttpGet("get-publisher-books-with-authors/{id}")]
        public IActionResult GetPublisherData(int id)
        {
            var response = _publisherService.GetPublisherData(id);

            return Ok(response);
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public /*Publisher*/ /*CustomActionResult*/ IActionResult /*ActionResult<Publishers>*/ GetPublisherById(int id)
        {
            //throw new Exception("This is an exception will be handled by middleware");
            var _respone = _publisherService.GetPublisherById(id);

            if (_respone != null)
            {
                //return _respone;
                return Ok(_respone);

                //var _responseObj = new CustomActionResultVM()
                //{
                //    Publisher = _respone
                //};

                //return new CustomActionResult(_responseObj);
            }
            else
            {
                var _responseObj = new CustomActionResultVM()
                {
                    Exception = new Exception("This is comming from Publisher controller")
                };
                //return null;
                return NotFound();
                //return new CustomActionResult(_responseObj);
            }
        }

        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublisher(string sortBy, string searchString, int pageNumber)
        {
            //throw new Exception("This is exception from GetAllPublisher() in Publisher Controller");

            try
            {
                _logger.LogInformation("This is just a log in GetAllPublisher()");
                var _result = _publisherService.GetAllPublisher(sortBy, searchString, pageNumber);
                return Ok(_result);
            }
            catch (Exception)
            {
                return BadRequest("Sorry, we could not load the publishers");
            }
           
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody]PublisherVM publisher)
        {
            try
            {
                var newPublisher = _publisherService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newPublisher);
            }
            catch (PublisherNameException ex)
            {
                return BadRequest($"{ex.Message}, Publisher name: {ex.PublisherName}");
            }
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
           
            try
            {
                //throw new Exception("A error occur at publisher controller");
                _publisherService.DeletePublisherById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
