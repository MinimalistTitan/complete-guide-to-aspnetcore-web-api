using my_books.Data.Models;
using my_books.EntityModels;

namespace my_books.Data.ViewModels
{
    public class CustomActionResultVM
    {
        public Exception Exception { get; set; }
        public Publishers Publisher { get; set; }
    }
}
