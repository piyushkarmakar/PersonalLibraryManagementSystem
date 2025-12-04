using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalLibraryAPI.DTOs;
using PersonalLibraryAPI.Services;
using PersonalLibraryManagementSystem.Enums;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;
using System;
using System.Collections.Generic;

namespace PersonalLibraryAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class ReadingController : ControllerBase
    {
        private readonly DatabaseService _dbService;
        private readonly IConfiguration _config;
        private readonly ILogger<ReadingController> _logger;

        public ReadingController(DatabaseService dbService, IConfiguration config, ILogger<ReadingController> logger)
        {
            _dbService = dbService;
            _config = config;
            _logger = logger;
        }

        // -----------------------------
        // DATABASE MODE (api/db/...)
        // -----------------------------

        [HttpGet("db/readingProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<BookStatusDto>> GetReadingStatusDb()
        {
            try
            {
                var books = _dbService.GetAllBooks(); // full book list
                var result = books.ConvertAll(b => new BookStatusDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Status = b.Status // enum, no .ToString()
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reading status from DB");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("db/reading/start/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult StartReadingDb(int id)
        {
            try
            {
                bool ok = _dbService.StartReading(id);
                if (!ok) return NotFound("Book not found.");
                return Ok("Book status set to CurrentlyReading.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting reading in DB for book {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("db/reading/finish/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult FinishReadingDb(int id)
        {
            try
            {
                bool ok = _dbService.FinishReading(id);
                if (!ok) return NotFound("Book not found.");
                return Ok("Book status set to Finished.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finishing reading in DB for book {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("db/reading/rating/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult AddRatingDb(int id, [FromBody] RatingReviewDto dto)
        {
            try
            {
                if (dto == null) return BadRequest("Body required.");
                if (dto.Rating < 0 || dto.Rating > 5) return BadRequest("Rating must be 0-5.");

                bool ok = _dbService.AddRatingReview(id, dto.Rating, dto.Review);
                if (!ok) return NotFound("Book not found.");

                return Ok("Rating & review saved.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding rating in DB for book {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("db/reading/review/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RatingReviewDto> GetBookReviewDb(int id)
        {
            try
            {
                var book = _dbService.GetBookById(id);
                if (book == null) return NotFound("Book not found.");

                var dto = new RatingReviewDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Rating = book.Rating,
                    Review = book.Review
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching review from DB for book {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }


        // -----------------------------
        // FILE MODE (api/file/...)
        // -----------------------------

        private string BooksFilePath => _config["FilePaths:BooksFile"];

        [HttpGet("file/readingProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<BookStatusDto>> GetReadingStatusFile()
        {
            try
            {
                var books = FileService.LoadBooks(BooksFilePath);
                var result = books.ConvertAll(b => new BookStatusDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Status = b.Status // enum, no .ToString()
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reading status from file");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("file/reading/start/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult StartReadingFile(int id)
        {
            try
            {
                bool ok = FileService.UpdateBookStatusFile(BooksFilePath, id, BookStatus.CurrentlyReading);
                if (!ok) return NotFound("Book not found in file.");
                return Ok("Book status set to CurrentlyReading (file).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting reading in file for book {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("file/reading/finish/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult FinishReadingFile(int id)
        {
            try
            {
                bool ok = FileService.UpdateBookStatusFile(BooksFilePath, id, BookStatus.Finished, setDateFinished: true);
                if (!ok) return NotFound("Book not found in file.");
                return Ok("Book status set to Finished (file).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finishing reading in file for book {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("file/reading/rating/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult AddRatingFile(int id, [FromBody] RatingReviewDto dto)
        {
            try
            {
                if (dto == null) return BadRequest("Body required.");
                if (dto.Rating < 0 || dto.Rating > 5) return BadRequest("Rating must be 0-5.");

                bool ok = FileService.UpdateRatingReviewFile(BooksFilePath, id, dto.Rating, dto.Review);
                if (!ok) return NotFound("Book not found in file.");

                return Ok("Rating & review saved (file).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding rating in file for book {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("file/reading/review/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RatingReviewDto> GetBookReviewFile(int id)
        {
            try
            {
                var books = FileService.LoadBooks(BooksFilePath);
                var book = books.Find(b => b.Id == id);
                if (book == null) return NotFound("Book not found.");

                var dto = new RatingReviewDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Rating = book.Rating,
                    Review = book.Review
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching review from file for book {id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }


    }
}
