using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;
using PersonalLibraryAPI.Services;
using System;
using System.Collections.Generic;

namespace PersonalLibraryAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class LendingController : ControllerBase
    {
        private readonly LendingManager _lendingManager;
        private readonly ILogger<LendingController> _logger;
        private readonly IConfiguration _config;
        private string LendingFilePath => _config["FilePaths:LendingFile"];

        public LendingController(
            LendingManager lendingManager,
            IConfiguration config,
            ILogger<LendingController> logger)
        {
            _lendingManager = lendingManager;
            _config = config;
            _logger = logger;
        }

        // =====================================================================
        //                        DATABASE MODE FUNCTIONS
        // =====================================================================

        [HttpGet("db/lending")]
        public ActionResult<List<LendingRecord>> GetAllLendingDb()
        {
            try
            {
                var allRecords = _lendingManager.CreateDbService().GetAllRecords();
                // OverdueDays is already a calculated property
                return Ok(allRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllLendingDb Error");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("db/lending/lent")]
        public ActionResult<List<LendingRecord>> GetAllLentBooksDb()
        {
            try
            {
                var lentBooks = _lendingManager.CreateDbService().GetAllLentBooks();
                return Ok(lentBooks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllLentBooksDb Error");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("db/lending/overdue")]
        public ActionResult<List<LendingRecord>> GetOverdueDb()
        {
            try
            {
                var overdue = _lendingManager.CreateDbService().GetOverdueBooks();
                return Ok(overdue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetOverdueDb Error");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // =====================================================================
        //                             LEND BOOK (POST)
        // =====================================================================

        public class LendRequest
        {
            public int BookId { get; set; }
            public string BookName { get; set; }   
            public string FriendName { get; set; }
            public DateTime DueDate { get; set; }
        }

        [HttpPost("db/lending/lend")]
        public ActionResult LendBookDb([FromBody] LendRequest req)
        {
            try
            {
                if (req == null) return BadRequest("Request cannot be null.");

                var service = _lendingManager.CreateDbService();

                // If BookId not provided, convert BookName → BookId
                if (req.BookId == 0 && !string.IsNullOrEmpty(req.BookName))
                {
                    var book = service.GetBookByName(req.BookName);
                    if (book == null)
                        return NotFound($"Book '{req.BookName}' not found.");

                    req.BookId = book.Id;
                }

                if (req.BookId == 0)
                    return BadRequest("BookId or BookName must be provided.");

                bool result = service.LendBook(req.BookId, req.FriendName, req.DueDate);

                if (!result)
                    return BadRequest("Failed to lend book. Check book or friend.");

                return Ok("Book lent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LendBookDb Error");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // =====================================================================
        //                           RETURN BOOK (PATCH)
        // =====================================================================

        public class ReturnRequest
        {
            public int BookId { get; set; }
            public string BookName { get; set; }  
            public string FriendName { get; set; }
        }

        [HttpPatch("db/lending/return")]
        public ActionResult ReturnBookDb([FromBody] ReturnRequest req)
        {
            try
            {
                if (req == null) return BadRequest("Request cannot be null.");

                var service = _lendingManager.CreateDbService();

                if (req.BookId == 0 && !string.IsNullOrEmpty(req.BookName))
                {
                    var book = service.GetBookByName(req.BookName);
                    if (book == null)
                        return NotFound($"Book '{req.BookName}' not found.");

                    req.BookId = book.Id;
                }

                if (req.BookId == 0)
                    return BadRequest("BookId or BookName must be provided.");

                bool result = service.ReturnBook(req.BookId, req.FriendName);

                if (!result)
                    return BadRequest("Return failed. Lending record not found.");

                return Ok("Book returned successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReturnBookDb Error");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // =====================================================================
        //                        FILE MODE FUNCTIONS
        // =====================================================================

        [HttpGet("file/lending")]
        public ActionResult<List<LendingRecord>> GetAllLendingFile()
        {
            try
            {
                var allRecords = _lendingManager.CreateFileService().GetAllRecords();
                return Ok(allRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllLendingFile Error");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("file/lending/lent")]
        public ActionResult<List<LendingRecord>> GetAllLentFile()
        {
            try
            {
                var lentBooks = _lendingManager.CreateFileService().GetAllLentBooks();
                return Ok(lentBooks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllLentFile Error");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("file/lending/overdue")]
        public ActionResult<List<LendingRecord>> GetOverdueFile()
        {
            try
            {
                var overdueBooks = _lendingManager.CreateFileService().GetOverdueBooks();
                return Ok(overdueBooks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetOverdueFile Error");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("file/lending/lend")]
        public ActionResult LendBookFile([FromBody] LendRequest req)
        {
            try
            {
                var service = _lendingManager.CreateFileService();

                // BookName → BookId support
                if (req.BookId == 0 && !string.IsNullOrEmpty(req.BookName))
                {
                    var book = service.GetBookByName(req.BookName);
                    if (book == null) return NotFound($"Book '{req.BookName}' not found.");
                    req.BookId = book.Id;
                }

                if (req.BookId == 0) return BadRequest("BookId or BookName must be provided.");

                bool result = service.LendBook(req.BookId, req.FriendName, req.DueDate);
                if (!result) return BadRequest("Lend failed.");

                // ✅ Save immediately
                FileService.SaveLendingRecords(service.GetAllRecords(), LendingFilePath);

                return Ok("Book lent successfully (file mode).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LendBookFile Error");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPatch("file/lending/return")]
        public ActionResult ReturnBookFile([FromBody] ReturnRequest req)
        {
            try
            {
                var service = _lendingManager.CreateFileService();

                if (req.BookId == 0 && !string.IsNullOrEmpty(req.BookName))
                {
                    var book = service.GetBookByName(req.BookName);
                    if (book == null) return NotFound($"Book '{req.BookName}' not found.");
                    req.BookId = book.Id;
                }

                if (req.BookId == 0) return BadRequest("BookId or BookName must be provided.");

                bool result = service.ReturnBook(req.BookId, req.FriendName);
                if (!result) return BadRequest("Return failed.");

                FileService.SaveLendingRecords(service.GetAllRecords(), LendingFilePath);

                return Ok("Book returned successfully (file mode).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReturnBookFile Error");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
