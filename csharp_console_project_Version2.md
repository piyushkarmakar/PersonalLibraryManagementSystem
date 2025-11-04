# C# Console Project Assignment: Personal Library Management System

## Project Overview

**Trainee:** Piyush  
**Project Title:** Personal Library Management System  
**Duration:** 2 Weeks
**Difficulty Level:** Beginner to Intermediate  

### What You'll Build

You will create a console-based Personal Library Management System that 
helps users organize their book collection, track reading progress, 
and manage lending/borrowing activities with friends. This system will 
be entirely text-based and file-driven, making it perfect for demonstrating 
C# fundamentals.

## Project Description

Imagine you're an avid book reader who wants to digitally organize 
your personal library. Your console application will allow users to:

- Manage a collection of books with detailed information
- Track reading progress and status
- Handle book lending to friends
- Save and load data from text files
- Search and filter books by different criteria

This isn't just another CRUD application - it's a practical tool that book enthusiasts would actually use!

## Core Features to Implement

### 1. Book Management
- Add new books to the library
- View all books with pagination
- Update book information
- Remove books from the library
- Search books by title, author, or genre

### 2. Reading Progress Tracking
- Mark books as "To Read," "Currently Reading," or "Finished"
- Track reading start and completion dates

### 3. Friend & Lending System
- Manage a list of friends
- Lend books to friends with due dates
- Track borrowed books and return dates

### 4. Data Persistence
- Save all data to text files
- Load data when the application starts

## Technical Requirements & C# Fundamentals Coverage

### 1. Variables and Data Types ✅
- Use appropriate data types for book properties (string, int, DateTime, decimal, bool)
- Implement enums for book status, genres, and rating scales
- Use constants for application settings (max lending days, file paths, etc.)

```csharp
// Example enum usage
public enum BookStatus { ToRead, CurrentlyReading, Finished }
public enum Genre { Fiction, NonFiction, Biography, SciFi, Romance, Mystery, Technical }
```

### 2. Conditional Statements ✅
- Implement menu navigation using if/else and switch statements
- Validate user input with conditional logic
- Handle different book statuses and lending conditions

### 3. Loops ✅
- Use `for` loops for pagination and array processing
- Use `while` loops for menu systems and user input validation
- Use `foreach` loops for iterating through collections of books and friends

### 4. Arrays and Collections ✅
- `List<Book>` for storing the book collection
- `List<Friend>` for managing friends
- `Dictionary<int, Book>` for quick book lookups by ID
- `List<LendingRecord>` for tracking lending history

### 5. Methods ✅
- Create methods with different parameter types and return values
- Implement method overloading for search functionality
- Use static utility methods for common operations

```csharp
// Example method signatures you should implement
public void AddBook(string title, string author, Genre genre)
public Book SearchBook(string title)
public List<Book> SearchBooks(Genre genre)
public bool LendBook(int bookId, string friendName, DateTime dueDate)
```

### 6. Classes and Objects ✅
Create the following classes with proper encapsulation:

#### Book Class
```csharp
public class Book
{
    // Properties: Id, Title, Author, Genre, Status, Rating, DateAdded, DateStarted, DateFinished, Review, IsLent, etc.
    // Methods: MarkAsReading(), FinishReading(), AddRating(), etc.
}
```

#### Friend Class
```csharp
public class Friend  
{
    // Properties: Name, Email, Phone, BooksCurrentlyBorrowed
    // Methods: BorrowBook(), ReturnBook(), etc.
}
```

#### LendingRecord Class
```csharp
public class LendingRecord
{
    // Properties: BookId, FriendName, LendDate, DueDate, ReturnDate, IsOverdue
    // Methods: CalculateOverdueDays(), MarkAsReturned(), etc.
}
```

### 7. Properties and Encapsulation ✅
- Use private fields with public properties
- Implement property validation in setters
- Use auto-implemented properties where appropriate

### 8. Constructors ✅
- Parameterless constructors for default object creation
- Parameterized constructors for object initialization
- Constructor chaining using `this()` keyword

### 9. Inheritance and Polymorphism ✅
Create a base `LibraryItem` class and inherit `Book` from it. Later extend with `Magazine` or `AudioBook` classes.

```csharp
public abstract class LibraryItem
{
    // Common properties and methods
    public abstract void DisplayDetails();
}

public class Book : LibraryItem
{
    public override void DisplayDetails() { /* Implementation */ }
}
```

### 10. Interfaces ✅
Implement interfaces for different functionalities:

```csharp
public interface ISearchable
{
    List<Book> Search(string criteria);
}

```

### 11. Exception Handling ✅
- Handle file I/O exceptions
- Validate user input with try-catch blocks
- Create custom exceptions for library-specific errors

```csharp
public class BookNotFoundException : Exception
{
    public BookNotFoundException(string message) : base(message) { }
}
```

### 12. File Handling ✅
- Save books to `books.txt`
- Save friends to `friends.txt`
- Save lending records to `lending_records.txt`
- Implement backup and restore functionality

## Application Menu Structure

```
========================================
    PERSONAL LIBRARY MANAGEMENT SYSTEM
========================================
1. Book Management
   1.1 Add New Book
   1.2 View All Books
   1.3 Search Books
   1.4 Update Book
   1.5 Remove Book
   
2. Reading Progress
   2.1 Start Reading a Book
   2.2 Finish Reading a Book
   2.3 Add Rating & Review
   
3. Friends & Lending
   3.1 Manage Friends
   3.2 Lend a Book
   3.3 Return a Book
   3.4 View Lent Books
      
   
0. Exit Application
```

## Sample Data Structure (Text Files)

### books.txt Format:
```
1|The Great Gatsby|F. Scott Fitzgerald|Fiction|Finished|5|2024-01-15|2024-01-20|2024-02-10|Amazing classic!|False
2|Clean Code|Robert Martin|Technical|CurrentlyReading|0|2024-02-01|2024-02-01||Great for developers|False
```

### friends.txt Format:
```
John Doe|john@email.com|123-456-7890|2
Jane Smith|jane@email.com|098-765-4321|0
```

### lending_records.txt Format:
```
1|John Doe|2024-02-15|2024-03-01|2024-02-28|False
3|Jane Smith|2024-02-20|2024-03-05||True
```

## Implementation Phases

### Phase 1: Core Foundation (Week 1)
1. Design and implement the basic classes (Book, Friend, LendingRecord)
2. Create the main menu system
3. Implement basic book management (add, view, search)
4. Add simple file saving and loading

### Phase 2: Advanced Features (Week 2)
1. Implement the lending system
2. Create search and filtering functionality
3. Implement exception handling
4. Implement data validation and error handling

## Evaluation Criteria

Your project will be evaluated on:

1. **Code Quality (25%)**
   - Proper use of OOP principles
   - Clean, readable code with good naming conventions
   - Appropriate use of access modifiers

2. **Functionality (35%)**
   - All core features working correctly
   - Proper error handling and validation
   - User-friendly console interface

3. **C# Fundamentals (25%)**
   - Correct implementation of required concepts
   - Efficient use of collections and data structures
   - Proper exception handling

4. **File Handling & Data Persistence (15%)**
   - Reliable save/load functionality
   - Data integrity maintenance

## Getting Started Tips

1. **Start Small:** Begin with the Book class and basic add/view functionality
2. **Test Frequently:** Test each feature as you build it
3. **Use Git:** Consider using version control to track your progress
4. **Plan Your Data:** Sketch out your file formats before implementing file I/O
5. **Handle Edge Cases:** Always validate user input and handle errors gracefully

## Sample Output Examples

### Adding a New Book:
```
========================================
           ADD NEW BOOK
========================================
Enter book title: Clean Architecture
Enter author name: Robert C. Martin
Select genre:
1. Fiction    2. NonFiction    3. Biography
4. SciFi      5. Romance       6. Mystery
7. Technical  8. Other
Your choice: 7

Book "Clean Architecture" by Robert C. Martin has been added to your library!
Book ID: 1001
Genre: Technical
Status: To Read
Date Added: 2024-02-15

Press any key to continue...
```

### Viewing Library Statistics:
```
========================================
        READING STATISTICS
========================================
Total Books in Library: 45
Books Read This Year: 12
Currently Reading: 2
Books To Read: 31

Average Rating: 4.2/5
Favorite Genre: Technical (8 books)
Reading Streak: 15 days

Books Lent Out: 3
Overdue Books: 1

Press any key to return to main menu...
```

## Submission Requirements

When submitting your project, include:

1. **Source Code:** All .cs files properly organized
2. **Sample Data:** Include sample text files with test data
3. **Documentation:** Brief README explaining how to run the application
4. **Test Cases:** Document the test scenarios you used
5. **Reflection:** A short document explaining what you learned and any challenges faced

## Resources & Help

- Use Visual Studio or Visual Studio Code for development
- Reference the official Microsoft C# documentation
- Test your application thoroughly with different scenarios
- Don't hesitate to ask questions if you get stuck!

---

**Good luck, Piyush! This project will give you hands-on experience with all the C# fundamentals 
while building something genuinely useful. Take your time, write clean code, and enjoy the learning process!** 📚✨