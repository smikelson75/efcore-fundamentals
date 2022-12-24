namespace PublisherDomain;

public class Author
{
    public Author() {
        Books = new List<Book>();
    }

    // Id is an EF Core 6 convention that notifies the context that
    // the field is a primary key. So EF Core 6 is able to know
    // what the primary key is and build accordingly. Also since the field
    // is also a integer, it will use the Identity on the database.
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // Establishes a relationship between Author and Books
    public List<Book> Books { get; set; }
}
