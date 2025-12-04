using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;
using System;
using System.Collections.Generic;

namespace PersonalLibraryAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class BooksController : ControllerBase
    {
        private readonly DatabaseService _dbService;
        private readonly IConfiguration _config;
        private readonly ILogger<BooksController> _logger;

        // Constructor receives DatabaseService, IConfiguration, and ILogger<BooksController>
        public BooksController(DatabaseService dbService, IConfiguration config, ILogger<BooksController> logger)
        {
            _dbService = dbService;
            _config = config;
            _logger = logger;
        }

        // --------------------------------------------------------------------
        //                           DATABASE APIs
        // --------------------------------------------------------------------

        /// <summary>
        /// Returns all books from the database.
        /// </summary>
        [HttpGet("db/books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Book>> GetAllBooksFromDb()
        {
            try
            {
                // OLD:
                // var books = _dbService.GetAllBooks();
                // return Ok(books);

                _logger.LogInformation("Fetching all books from database...");
                var books = _dbService.GetAllBooks();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching books from DB");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Returns one book by ID from the database.
        /// </summary>
        [HttpGet("db/books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Book> GetBookFromDb(int id)
        {
            try
            {
                var books = _dbService.GetAllBooks();
                var book = books.Find(b => b.Id == id);

                if (book == null)
                {
                    _logger.LogWarning("Book with ID {id} not found in DB", id);
                    return NotFound("Book not found.");
                }
                _logger.LogInformation($"Book with ID {id} found in DB");
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Book ID {id} from DB", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        [HttpPost("db/books")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddBookToDb([FromBody] Book book)
        {
            try
            {
                if (book == null)
                    return BadRequest("Book cannot be null.");

                book.DateAdded = DateTime.Now;

                // OLD:
                // _dbService.AddBook(book);
                // return Ok("Book added to database.");

                _dbService.AddBook(book);
                _logger.LogInformation("Book Added Succesfully in DB");

                // CreatedAtAction is better than returning a plain OK string.
                return CreatedAtAction(nameof(GetBookFromDb), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book to DB");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Replaces an existing book in the database (full update).
        /// </summary>
        [HttpPut("db/books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateBookInDb(int id, [FromBody] Book updated)
        {
            try
            {
                if (updated == null)
                    return BadRequest("Updated book cannot be null.");

                updated.Id = id;

                // OLD:
                // _dbService.UpdateBook(updated);
                // return Ok("Book updated in DB.");

                _dbService.UpdateBook(updated);
                _logger.LogInformation("Updated Book ID {id}", id);

                return Ok("Book updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Book ID {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Partially updates an existing book in the database (PATCH).
        /// Only non-null/non-default fields in the 'patch' body will be applied.
        /// </summary>
        [HttpPatch("db/books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult PatchBookInDb(int id, [FromBody] Book patch)
        {
            try
            {
                if (patch == null)
                    return BadRequest("Patch body cannot be null.");

                var books = _dbService.GetAllBooks();
                var book = books.Find(b => b.Id == id);

                if (book == null)
                {
                    _logger.LogWarning("PATCH failed - Book ID {id} not found", id);
                    return NotFound("Book not found.");
                }

                // PARTIAL UPDATE (update only fields that are provided)
                if (!string.IsNullOrWhiteSpace(patch.Title))
                    book.Title = patch.Title;

                if (!string.IsNullOrWhiteSpace(patch.Author))
                    book.Author = patch.Author;

                // For enum fields, prefer to check patch.Genre against default value.
                // The default enum value will be 0 (Fiction in your enum). If you want to allow setting to Fiction,
                // you may need a different approach (e.g., dedicated DTO where fields are nullable).
                if (Enum.IsDefined(typeof(PersonalLibraryManagementSystem.Enums.Genre), patch.Genre))
                    book.Genre = patch.Genre;

                if (Enum.IsDefined(typeof(PersonalLibraryManagementSystem.Enums.BookStatus), patch.Status))
                    book.Status = patch.Status;

                if (patch.Rating != 0)
                    book.Rating = patch.Rating;

                if (!string.IsNullOrWhiteSpace(patch.Review))
                    book.Review = patch.Review;

                // If you want to update DateStarted / DateFinished via PATCH, add similar checks.

                _dbService.UpdateBook(book);
                _logger.LogInformation("Book ID {id} patched.", id);

                return Ok("Book partially updated in DB (PATCH).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching Book ID {id} in DB", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        [HttpDelete("db/books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult DeleteBookFromDb(int id)
        {
            try
            {
                _dbService.DeleteBook(id);
                _logger.LogInformation($"Book with ID {id} deleted from DB");
                return Ok($"Book with ID {id} deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Book ID {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        // --------------------------------------------------------------------
        //                           FILE APIs
        // --------------------------------------------------------------------

        private string BooksFilePath => _config["FilePaths:BooksFile"];

        /// <summary>
        /// Returns all books from file storage.
        /// </summary>
        [HttpGet("file/books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Book>> GetAllBooksFromFile()
        {
            try
            {
                var books = FileService.LoadBooks(BooksFilePath);
                _logger.LogInformation("Fetching all books from BookFile...");
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading books from file");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Returns a specific book by ID from file.
        /// </summary>
        [HttpGet("file/books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Book> GetBookFromFile(int id)
        {
            try
            {
                var books = FileService.LoadBooks(BooksFilePath);

                var book = books.Find(b => b.Id == id);
                if (book == null)
                    return NotFound("Book not found.");
                _logger.LogInformation($"Book with ID {id} found in BookFile");

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Book ID {id} from file", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Adds a new book to file.
        /// </summary>
        [HttpPost("file/books")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult AddBookToFile([FromBody] Book book)
        {
            try
            {
                if (book == null)
                    return BadRequest("Book cannot be null.");

                var books = FileService.LoadBooks(BooksFilePath);

                book.Id = books.Count > 0 ? books[books.Count - 1].Id + 1 : 1;
                book.DateAdded = DateTime.Now;

                books.Add(book);
                _logger.LogInformation($"Book having Id {book.Id} Added Successfully");
                FileService.SaveBooks(books, BooksFilePath);

                return CreatedAtAction(nameof(GetBookFromFile), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book to file");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Replaces an existing book in file (full update).
        /// </summary>
        [HttpPut("file/books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult UpdateBookInFile(int id, [FromBody] Book updated)
        {
            try
            {
                if (updated == null)
                    return BadRequest("Updated book cannot be null.");

                var books = FileService.LoadBooks(BooksFilePath);

                var book = books.Find(b => b.Id == id);
                if (book == null)
                    return NotFound("Book not found.");

                book.Title = updated.Title;
                book.Author = updated.Author;
                book.Genre = updated.Genre;
                book.Status = updated.Status;
                book.Rating = updated.Rating;
                book.Review = updated.Review;
                _logger.LogInformation($"Book having Id {book.Id} Updated Successfully");

                FileService.SaveBooks(books, BooksFilePath);

                return Ok("Book updated in file.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Book ID {id} in file", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Partially updates a book in file (PATCH).
        /// Only non-null/non-default fields in the 'patch' body will be applied.
        /// </summary>
        [HttpPatch("file/books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult PatchBookInFile(int id, [FromBody] Book patch)
        {
            try
            {
                if (patch == null)
                    return BadRequest("Patch body cannot be null.");

                var books = FileService.LoadBooks(BooksFilePath);
                var book = books.Find(b => b.Id == id);

                if (book == null)
                {
                    _logger.LogWarning("PATCH failed - Book ID {id} not found in file", id);
                    return NotFound("Book not found.");
                }

                // PARTIAL UPDATE (update only fields that are provided)
                if (!string.IsNullOrWhiteSpace(patch.Title))
                    book.Title = patch.Title;

                if (!string.IsNullOrWhiteSpace(patch.Author))
                    book.Author = patch.Author;

                if (Enum.IsDefined(typeof(PersonalLibraryManagementSystem.Enums.Genre), patch.Genre))
                    book.Genre = patch.Genre;

                if (Enum.IsDefined(typeof(PersonalLibraryManagementSystem.Enums.BookStatus), patch.Status))
                    book.Status = patch.Status;

                if (patch.Rating != 0)
                    book.Rating = patch.Rating;

                if (!string.IsNullOrWhiteSpace(patch.Review))
                    book.Review = patch.Review;

                FileService.SaveBooks(books, BooksFilePath);
                _logger.LogInformation("Book ID {id} patched in file.", id);

                return Ok("Book partially updated in file (PATCH).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching Book ID {id} in file", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Deletes a book from file.
        /// </summary>
        [HttpDelete("file/books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult DeleteBookFromFile(int id)
        {
            try
            {
                var books = FileService.LoadBooks(BooksFilePath);

                books.RemoveAll(b => b.Id == id);
                _logger.LogInformation($"Book having Id {id} Deleted Successfully");

                FileService.SaveBooks(books, BooksFilePath);


                return Ok($"Book with ID {id} deleted from file.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Book ID {id} from file", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
