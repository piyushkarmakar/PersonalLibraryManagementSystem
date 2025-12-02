//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using PersonalLibraryAPI.Services;
//using PersonalLibraryManagementSystem.Models;
//using PersonalLibraryManagementSystem.Services;
//using System;
//using System.Collections.Generic;

//namespace PersonalLibraryAPI.Controllers
//{
//    [ApiController]
//    [Route("api")]
//    public class LendingController : ControllerBase
//    {
//        private readonly IConfiguration _config;
//        private readonly ILogger<LendingController> _logger;
//        private readonly LendingManager _lendingManager;

//        public LendingController(IConfiguration config, ILogger<LendingController> logger)
//        {
//            _config = config;
//            _logger = logger;
//            var libraryManager = new LibraryManager(config);
//            var friendManager = new FriendManager(config);

//            _lendingManager = new LendingManager(config, libraryManager, friendManager);
//        }

//        private string LendingFilePath => _config["FilePaths:LendingFile"];

//        // --------------------------------------------------------------------
//        //                           DATABASE APIs
//        // --------------------------------------------------------------------

//        [HttpGet("db/lending")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public ActionResult<List<LendingRecord>> GetAllFromDb()
//        {
//            try
//            {
//                _logger.LogInformation("Fetching all lending records from DB...");
//                var lendingService = _lendingManager.CreateDbService();
//                var records = lendingService.GetAllRecords();
//                return Ok(records);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error fetching lending records from DB");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpGet("db/lending/{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public ActionResult<LendingRecord> GetByIdFromDb(int id)
//        {
//            try
//            {
//                var lendingService = _lendingManager.CreateDbService();
//                var record = lendingService.GetRecordById(id);

//                if (record == null)
//                    return NotFound($"Lending record with ID {id} not found.");

//                return Ok(record);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error fetching lending record from DB");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpPost("db/lending")]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        public ActionResult AddToDb([FromBody] LendingRecord record)
//        {
//            try
//            {
//                if (record == null)
//                    return BadRequest("Lending record cannot be null.");

//                var lendingService = _lendingManager.CreateDbService();
//                lendingService.AddRecord(record);

//                return CreatedAtAction(nameof(GetByIdFromDb), new { id = record.Id }, record);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error adding lending record to DB");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpPut("db/lending/{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public ActionResult UpdateDb(int id, [FromBody] LendingRecord updated)
//        {
//            try
//            {
//                if (updated == null)
//                    return BadRequest("Updated record cannot be null.");

//                var lendingService = _lendingManager.CreateDbService();
//                updated.Id = id;
//                lendingService.UpdateRecord(updated);

//                return Ok("Lending record updated.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating lending record in DB");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpPatch("db/lending/{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public ActionResult PatchDb(int id, [FromBody] LendingRecord patch)
//        {
//            try
//            {
//                var lendingService = _lendingManager.CreateDbService();
//                var record = lendingService.GetRecordById(id);

//                if (record == null)
//                    return NotFound($"Lending record with ID {id} not found.");

//                // Partial update
//                if (patch.BookId != 0) record.BookId = patch.BookId;
//                if (patch.FriendId != 0) record.FriendId = patch.FriendId;
//                if (patch.DateLent != DateTime.MinValue) record.DateLent = patch.DateLent;
//                if (patch.DateReturned != DateTime.MinValue) record.DateReturned = patch.DateReturned;

//                lendingService.UpdateRecord(record);
//                return Ok("Lending record patched.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error patching lending record in DB");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpDelete("db/lending/{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public ActionResult DeleteDb(int id)
//        {
//            try
//            {
//                var lendingService = _lendingManager.CreateDbService();
//                lendingService.DeleteRecord(id);
//                return Ok($"Lending record with ID {id} deleted.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting lending record from DB");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        // --------------------------------------------------------------------
//        //                               FILE APIs
//        // --------------------------------------------------------------------

//        [HttpGet("file/lending")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public ActionResult<List<LendingRecord>> GetAllFromFile()
//        {
//            try
//            {
//                var lendingService = _lendingManager.CreateFileService();
//                var records = lendingService.GetAllRecords();
//                return Ok(records);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error fetching lending records from file");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpGet("file/lending/{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public ActionResult<LendingRecord> GetByIdFromFile(int id)
//        {
//            try
//            {
//                var lendingService = _lendingManager.CreateFileService();
//                var record = lendingService.GetRecordById(id);

//                if (record == null)
//                    return NotFound($"Lending record with ID {id} not found.");

//                return Ok(record);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error fetching lending record from file");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpPost("file/lending")]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        public ActionResult AddToFile([FromBody] LendingRecord record)
//        {
//            try
//            {
//                if (record == null)
//                    return BadRequest("Lending record cannot be null.");

//                var lendingService = _lendingManager.CreateFileService();
//                lendingService.AddRecord(record);

//                return CreatedAtAction(nameof(GetByIdFromFile), new { id = record.Id }, record);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error adding lending record to file");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpPut("file/lending/{id}")]
//        public ActionResult UpdateFile(int id, [FromBody] LendingRecord updated)
//        {
//            try
//            {
//                var lendingService = _lendingManager.CreateFileService();
//                updated.Id = id;
//                lendingService.UpdateRecord(updated);

//                return Ok("Lending record updated in file.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating lending record in file");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpPatch("file/lending/{id}")]
//        public ActionResult PatchFile(int id, [FromBody] LendingRecord patch)
//        {
//            try
//            {
//                var lendingService = _lendingManager.CreateFileService();
//                var record = lendingService.GetRecordById(id);

//                if (record == null)
//                    return NotFound($"Lending record with ID {id} not found.");

//                if (patch.BookId != 0) record.BookId = patch.BookId;
//                if (patch.FriendId != 0) record.FriendId = patch.FriendId;
//                if (patch.DateLent != DateTime.MinValue) record.DateLent = patch.DateLent;
//                if (patch.DateReturned != DateTime.MinValue) record.DateReturned = patch.DateReturned;

//                lendingService.UpdateRecord(record);
//                return Ok("Lending record patched in file.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error patching lending record in file");
//                return StatusCode(500, "Internal server error.");
//            }
//        }

//        [HttpDelete("file/lending/{id}")]
//        public ActionResult DeleteFile(int id)
//        {
//            try
//            {
//                var lendingService = _lendingManager.CreateFileService();
//                lendingService.DeleteRecord(id);
//                return Ok($"Lending record with ID {id} deleted from file.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting lending record from file");
//                return StatusCode(500, "Internal server error.");
//            }
//        }
//    }
//}
