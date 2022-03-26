namespace my_books.Data.Models
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        public string Name { get; set; }

        //Navigation properties
        public List<Book> Books { get; set; }
    }
}
