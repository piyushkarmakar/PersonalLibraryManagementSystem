using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;

namespace PersonalLibraryAPI.Services
{
    public class FriendManager
    {
        private readonly IConfiguration _config;

        public FriendManager(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Returns a FriendService connected to the database
        /// </summary>
        public FriendService CreateDbService()
        {
            string conn = _config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(conn))
                throw new System.Exception("DefaultConnection not set in appsettings.json");
            return new FriendService(conn);
        }

        /// <summary>
        /// Returns a FriendService working on file data
        /// </summary>
        public FriendService CreateFileService()
        {
            string path = _config["FilePaths:FriendsFile"];
            if (string.IsNullOrEmpty(path))
                throw new System.Exception("File path for FriendsFile not set in appsettings.json");

            List<Friend> friends = FileService.LoadFriends(path);
            return new FriendService(friends);
        }
    }
}
