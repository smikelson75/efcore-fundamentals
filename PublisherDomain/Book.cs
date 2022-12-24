namespace PublisherDomain;

public class Book {
    // BookId contains the words Id, so EF Core 6 is able
    // to mark the field as a primary key. Also since the field
    // is also a integer, it will use the Identity on the database.
    public int BookId { get; set; }
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
    public decimal BasePrice { get; set; }
    // Establishes a relationship between Author and Books
    public Author Author { get; set; }
    // AuthorId stores the Id from the author's table
    public int AuthorId { get; set; }
}