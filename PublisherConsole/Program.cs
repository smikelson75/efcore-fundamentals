// See https://aka.ms/new-console-template for more information
using PublisherData;
using PublisherDomain;
using Microsoft.EntityFrameworkCore;

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

