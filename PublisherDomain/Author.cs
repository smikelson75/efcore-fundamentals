namespace PublisherDomain;

public class Author
{
    public Author() {
        Books = new List<Book>();
    }

    // EF Core 6 convention, Id property is tagged as
    // a primary key.
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Book> Books { get; set; }
}
