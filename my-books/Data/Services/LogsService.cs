using my_books.Data.Models;
using my_books.EntityModels;

namespace my_books.Data.Services
{
    public class LogsService
    {
        private readonly MyBooksDbContext _context;

        public LogsService(MyBooksDbContext context)
        {
            _context = context;
        }

        public List<LogEvents> GetAllLogsFromDB() => _context.LogEvents.ToList();
    }
}
