using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        private readonly LogsService logsService;

        public ExceptionController(LogsService logsService)
        {
            this.logsService = logsService;
        }

        [HttpGet("get-all-logs-from-db")]
        public IActionResult GetAllLog()
        {
            try
            {
                var allLogs = this.logsService.GetAllLogsFromDB();
                return Ok(allLogs);
            }
            catch (Exception)
            {
                return BadRequest("Sorry, we could not load the logs from database");
            }
        }
    }
}
