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
    public class FriendsController : ControllerBase
    {
        private readonly DatabaseService _dbService;
        private readonly IConfiguration _config;
        private readonly ILogger<FriendsController> _logger;

        public FriendsController(DatabaseService dbService, IConfiguration config, ILogger<FriendsController> logger)
        {
            _dbService = dbService;
            _config = config;
            _logger = logger;
        }

        // --------------------------------------------------------------------
        //                           DATABASE APIs
        // --------------------------------------------------------------------

        [HttpGet("db/friends")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Friend>> GetAllFriendsFromDb()
        {
            try
            {
                _logger.LogInformation("Fetching all friends from database...");
                var friends = _dbService.GetAllFriends();
                return Ok(friends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching friends from DB");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("db/friends/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Friend> GetFriendFromDb(string name)
        {
            try
            {
                var friends = _dbService.GetAllFriends();
                var friend = friends.Find(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (friend == null)
                {
                    _logger.LogWarning("Friend with Name {name} not found in DB", name);
                    return NotFound("Friend not found.");
                }
                _logger.LogInformation($"Friends with Name : {name} found in DB");

                return Ok(friend);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Friend {name} from DB", name);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("db/friends")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddFriendToDb([FromBody] Friend friend)
        {
            try
            {
                if (friend == null)
                    return BadRequest("Friend cannot be null.");
                _logger.LogInformation($"Friend Added Successfully In DB");
                _dbService.AddFriend(friend);
                return CreatedAtAction(nameof(GetFriendFromDb), new { name = friend.Name }, friend);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding friend to DB");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("db/friends/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateFriendInDb(string name, [FromBody] Friend updated)
        {
            try
            {
                if (updated == null)
                    return BadRequest("Updated friend cannot be null.");

                var friends = _dbService.GetAllFriends();
                var friend = friends.Find(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (friend == null)
                    return NotFound("Friend not found.");

                // Full update
                friend.Name = updated.Name;
                friend.Email = updated.Email;
                friend.Phone = updated.Phone;

                // Re-save to DB
                _dbService.AddFriend(friend); // Or you can create an UpdateFriend method
                _logger.LogInformation("Friend {name} updated in DB", name);

                return Ok("Friend updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Friend {name} in DB", name);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPatch("db/friends/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult PatchFriendInDb(string name, [FromBody] Friend patch)
        {
            try
            {
                if (patch == null)
                    return BadRequest("Patch body cannot be null.");

                var friends = _dbService.GetAllFriends();
                var friend = friends.Find(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (friend == null)
                    return NotFound("Friend not found.");

                // Partial update
                if (!string.IsNullOrWhiteSpace(patch.Name))
                    friend.Name = patch.Name;

                if (!string.IsNullOrWhiteSpace(patch.Email))
                    friend.Email = patch.Email;

                if (!string.IsNullOrWhiteSpace(patch.Phone))
                    friend.Phone = patch.Phone;

                _dbService.AddFriend(friend); // Or UpdateFriend
                _logger.LogInformation("Friend {name} patched in DB", name);

                return Ok("Friend partially updated in DB (PATCH).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching Friend {name} in DB", name);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("db/friends/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult DeleteFriendFromDb(string name)
        {
            try
            {
                var friends = _dbService.GetAllFriends();
                var friend = friends.Find(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (friend == null)
                    return NotFound("Friend not found.");

                _dbService.DeleteFriendByName(name);
                _logger.LogInformation("Friend {name} deleted from DB", name);


                return Ok($"Friend {name} deleted from database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Friend {name} from DB", name);
                return StatusCode(500, "Internal server error.");
            }
        }

        // --------------------------------------------------------------------
        //                           FILE APIs
        // --------------------------------------------------------------------

        private string FriendsFilePath => _config["FilePaths:FriendsFile"];

        [HttpGet("file/friends")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Friend>> GetAllFriendsFromFile()
        {
            try
            {
                var friends = FileService.LoadFriends(FriendsFilePath);
                _logger.LogInformation("Fetching all Friends from FriendFile...");

                return Ok(friends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading friends from file");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("file/friends/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Friend> GetFriendFromFile(string name)
        {
            try
            {
                var friends = FileService.LoadFriends(FriendsFilePath);
                var friend = friends.Find(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (friend == null)
                    return NotFound("Friend not found.");
                _logger.LogInformation($"Fetching Friend Name {name} from FriendFile...");

                return Ok(friend);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading friend {name} from file", name);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("file/friends")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult AddFriendToFile([FromBody] Friend friend)
        {
            try
            {
                if (friend == null)
                    return BadRequest("Friend cannot be null.");

                var friends = FileService.LoadFriends(FriendsFilePath);
                friends.Add(friend);
                _logger.LogInformation($"Friend Added Successfully in FriendFile");

                FileService.SaveFriends(friends, FriendsFilePath);

                return CreatedAtAction(nameof(GetFriendFromFile), new { name = friend.Name }, friend);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding friend to file");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("file/friends/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateFriendInFile(string name, [FromBody] Friend updated)
        {
            try
            {
                var friends = FileService.LoadFriends(FriendsFilePath);
                var friend = friends.Find(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (friend == null)
                    return NotFound("Friend not found.");

                // Full update
                friend.Name = updated.Name;
                friend.Email = updated.Email;
                friend.Phone = updated.Phone;

                FileService.SaveFriends(friends, FriendsFilePath);
                _logger.LogInformation($"Updated Friend with Name {name} In FriendFile...");

                return Ok("Friend updated in file.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating friend {name} in file", name);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPatch("file/friends/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult PatchFriendInFile(string name, [FromBody] Friend patch)
        {
            try
            {
                var friends = FileService.LoadFriends(FriendsFilePath);
                var friend = friends.Find(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (friend == null)
                    return NotFound("Friend not found.");

                if (!string.IsNullOrWhiteSpace(patch.Name))
                    friend.Name = patch.Name;

                if (!string.IsNullOrWhiteSpace(patch.Email))
                    friend.Email = patch.Email;

                if (!string.IsNullOrWhiteSpace(patch.Phone))
                    friend.Phone = patch.Phone;

                FileService.SaveFriends(friends, FriendsFilePath);
                _logger.LogInformation($"Patched Friend with Name {name} In FriendFile...");

                return Ok("Friend partially updated in file (PATCH).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching friend {name} in file", name);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("file/friends/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult DeleteFriendFromFile(string name)
        {
            try
            {
                var friends = FileService.LoadFriends(FriendsFilePath);
                var friend = friends.Find(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (friend == null)
                    return NotFound("Friend not found.");

                friends.Remove(friend);
                FileService.SaveFriends(friends, FriendsFilePath);
                _logger.LogInformation($"Deleted Friend with Name {name} In FriendFile...");

                return Ok($"Friend {name} deleted from file.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting friend {name} from file", name);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
