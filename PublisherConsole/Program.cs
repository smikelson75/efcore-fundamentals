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
RetrieveAndUpdateAuthor();
RetrieveAndUpdateMultipleAuthors();
CoordinatedRetrieveAndUpdateAuthor();
DeleteAnAuthor();
InsertMultipleAuthors();

void AddAuthor() {
    // See https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers
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

void RetrieveAndUpdateAuthor() {
    using var context = new PubContext();
    // Query the database to get the first instance of the author
    // This adds Tracking data into EF Core
    var author = context.Authors.FirstOrDefault(a => a.FirstName == "Julie" && a.LastName == "Lerman");

    // If data returns from the call, update the first name to Julia (row is modifed) and save the changes. 
    // Otherwise, do nothing.
    if (author != null) {
        author.FirstName = "Julia";
        context.SaveChanges();
    }
}

void RetrieveAndUpdateMultipleAuthors() {
    using var context = new PubContext();
    // Returns all authors with a last name of Lerman (3)
    var authors = context.Authors.Where(a => a.LastName == "Lerman").ToList();

    // Walk through each author and update the name to "Lehrman"
    foreach (var author in authors) {
        author.LastName = "Lehrman";
    }

    // Show existing change tracker using debugging, review for changes, then display
    // the result using the debugging view
    Console.WriteLine("Before:" + context.ChangeTracker.DebugView.ShortView);
    context.ChangeTracker.DetectChanges();
    Console.WriteLine("After:" + context.ChangeTracker.DebugView.ShortView);

    // Finally, send the updated Last Name to the database
    context.SaveChanges();
}

#region UpdatingUntrackedObjects
void CoordinatedRetrieveAndUpdateAuthor() {
    // Call to get the author Object
    var author = FindTheAuthor(3);

    // if the author is returned and the FirstName equals "Julie"
    if (author?.FirstName == "Julie") {
        // Update the author FirstName to "Julia"
        // Make the call to save the Author Object
        author.FirstName = "Julia";
        SaveTheAuthor(author);
    }
}
Author FindTheAuthor(int authorId) {
    // context only exists within the function and gets disposed of
    // after function falls out of scope.
    using var context = new PubContext();
    return context.Authors.Find(authorId);
}

void SaveTheAuthor(Author author) {
    // Same here. The context is built within the function. And doesn't know
    // the author is previously existing
    using var context = new PubContext();
    // Explicitly tells the context to added as a modified value
    context.Authors.Update(author);
    // During the DetectChanges() call, an update is generated instead of an Insert
    // However, in this context, the entire row is sent for update because the context
    // doesn't know which field was updated.
    context.SaveChanges();
}
#endregion

void DeleteAnAuthor() {
    using var context = new PubContext();
    // Retrieve the existing author to remove
    var author = context.Authors.Find(3);

    // If the author was return... Otherwise, do nothing.
    if (author != null) {
        // notify the context that this author
        // should be Removed then save the changes. In this case the author has no
        // associated Book relationship (will cause an Exception if it did).  
        context.Authors.Remove(author);
        context.SaveChanges();
    }
}

void InsertMultipleAuthors() {
    using var context = new PubContext();

    // Create a new Array of Authors
    var authorList = new Author[] {
        new Author { FirstName = "Ruth", LastName = "Ozeki" },
        new Author { FirstName = "Sofia", LastName = "Segovia" },
        new Author { FirstName = "Ursula K.", LastName = "LeGuin"},
        new Author { FirstName = "Hugh", LastName = "Howey"},
        new Author { FirstName = "Isabelle", LastName = "Allende"}
    };

    // Add the new Array of Authors
    context.Authors.AddRange(authorList);
    
    // Store listing of Authors. No performance advantage over AddSomeMoreAuthors()
    context.SaveChanges();
}

#region InsertMultipleAuthorsUsingGenericList
void CoordinateNewAuthors() {
    // See https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/list-collection
    var authorList = new List<Author> {
        new Author { FirstName = "Ruth", LastName = "Ozeki" },
        new Author { FirstName = "Sofia", LastName = "Segovia" },
        new Author { FirstName = "Ursula K.", LastName = "LeGuin"},
        new Author { FirstName = "Hugh", LastName = "Howey"},
        new Author { FirstName = "Isabelle", LastName = "Allende"}
    };

    InsertMultipleAuthorsPassedIn(authorList);
}

void InsertMultipleAuthorsPassedIn(List<Author> listOfAuthors) {
    using var context = new PubContext();
    context.Authors.AddRange(listOfAuthors);
    context.SaveChanges();
}
#endregion