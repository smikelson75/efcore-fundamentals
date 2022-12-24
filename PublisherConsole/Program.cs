// See https://aka.ms/new-console-template for more information
using PublisherData;
using PublisherDomain;
using Microsoft.EntityFrameworkCore;

// Using statement verifies that the database exists on the server specified.
// If it doesn't, it creates the database. Since the DbContext also contains models,
// those models will be translated to tables/columns, including any foreign keys.
// See https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement
using (PubContext context = new PubContext()) {
    context.Database.EnsureCreated();
}

GetAuthors();
AddAuthor();
GetAuthors();
GetAuthorsWithBooks();
AddAuthorWithBooks();
GetAuthorsWithBooks();
QueryFilter();
AddSomeMoreAuthors();
SkipAndTakeAuthors();
SortAuthors();

void AddAuthor() {
    var author = new Author { FirstName = "Josie", LastName = "Newf" };

    // See https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement
    using var context = new PubContext();

    context.Authors.Add(author);
    context.SaveChanges();
}

void GetAuthors() {
    // See https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement
    using var context = new PubContext();
    var authors = context.Authors.ToList();

    // See https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/using-foreach-with-arrays
    foreach (var author in authors) {
        Console.WriteLine(author.FirstName + " " + author.LastName);
    }
}

void AddAuthorWithBooks() {
    var author = new Author { FirstName = "Julie", LastName = "Lerman" };
    author.Books.Add(new Book { Title = "Programming Entity Framework", 
                                PublishDate = new DateTime(2009, 1, 1)
    });

    author.Books.Add(new Book { Title = "Programming Entity Framework 2nd Ed",
                                PublishDate = new DateTime(2010, 8, 1)
    });

    using var context = new PubContext();
    context.Authors.Add(author);
    context.SaveChanges();
}

void GetAuthorsWithBooks() {
    using var context = new PubContext();
    // .Include() Requires using Microsoft.EntityFrameworkCore;
    // (a => a.Books) : See https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
    var authors = context.Authors.Include(a => a.Books).ToList();

    // See https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/using-foreach-with-arrays
    foreach (var author in authors) {
        Console.WriteLine(author.FirstName + " " + author.LastName);
        foreach (var book in author.Books) {
            Console.WriteLine("*" + book.Title);
        }
    }
}

void QueryFilter() {
    using var context = new PubContext();

    // Generates a query where the name is hard-coded. It is a literal its trusted.
    var author = context.Authors.Where(s => s.FirstName=="Josie").ToList();

    var firstName = "Josie";
    // Generates a parameterized query to protect from SQL Injection. It sees that
    // the value is not a literal and can't be trusted.
    var unsafe_author = context.Authors.Where(s => s.FirstName == firstName);

    // Generates a LIKE query to search for text
    var search_author = context.Authors.Where(a => EF.Functions.Like(a.LastName, "L%"));

    // Look up a row using the primary key of the model.
    var single_author = context.Authors.Find(1);
}

void AddSomeMoreAuthors() {
    using var context = new PubContext();

    context.Authors.Add(new Author { FirstName = "Rhoda", LastName = "Lerman"});
    context.Authors.Add(new Author { FirstName = "Don", LastName = "Jones"});
    context.Authors.Add(new Author { FirstName = "Jim", LastName = "Christopher"});
    context.Authors.Add(new Author { FirstName = "Stephen", LastName = "Haunts"});
    context.SaveChanges();
}

// Pages through a result set without pulling all data from the table.
void SkipAndTakeAuthors() {
    using var context = new PubContext();
    var groupSize = 2;

    // See https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/branches-and-loops-local#work-with-the-for-loop
    for (int i = 0; i < 5; i ++) {
        var authors = context.Authors.Skip(groupSize * i).Take(groupSize).ToList();
        Console.WriteLine($"Group {i}:");
        
        // See https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/using-foreach-with-arrays
        foreach (var author in authors) {
            Console.WriteLine($" {author.FirstName} {author.LastName}");
        }
    }
}

void SortAuthors() {
    using var context = new PubContext();

    // Sort By Ascending, OrderBy for the first field, then add
    // ThenBy for each additional field in the sort.
    var authorsByLastName = context.Authors
        .OrderBy(a => a.FirstName)
        .ThenBy(a => a.LastName).ToList();

    foreach (var author in authorsByLastName) {
        Console.WriteLine($"{author.LastName}, {author.FirstName}");
    }

    Console.WriteLine("** Descending Last and First**");
    // Sort By Descending, again OrderByDescending for the first field,
    // then add ThenByDescending for each additional field
    var authorsByDescLastName = context.Authors
        .OrderByDescending(a => a.LastName)
        .ThenByDescending(a => a.FirstName).ToList();

    foreach (var author in authorsByDescLastName) {
        Console.WriteLine($"{author.LastName}, {author.FirstName}");
    }
}