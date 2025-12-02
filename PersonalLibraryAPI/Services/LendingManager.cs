using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;

namespace PersonalLibraryAPI.Services
{
    public class LendingManager
    {
        private readonly IConfiguration _config;
        private readonly LibraryManager _libraryManager;
        private readonly FriendManager _friendManager;

        public LendingManager(IConfiguration config, LibraryManager libraryManager, FriendManager friendManager)
        {
            _config = config;
            _libraryManager = libraryManager;
            _friendManager = friendManager;
        }

        /// <summary>
        /// Returns DB-mode LendingService
        /// </summary>
        public LendingService CreateDbService()
        {
            string conn = _config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(conn))
                throw new System.Exception("DefaultConnection not set in appsettings.json");

            var libService = _libraryManager.CreateDbService();
            var friendService = _friendManager.CreateDbService();

            return new LendingService(conn, libService, friendService);
        }

        /// <summary>
        /// Returns File-mode LendingService
        /// </summary>
        public LendingService CreateFileService()
        {
            var lendingPath = _config["FilePaths:LendingFile"];
            var booksPath = _config["FilePaths:BooksFile"];
            var friendsPath = _config["FilePaths:FriendsFile"];

            if (string.IsNullOrEmpty(lendingPath) || string.IsNullOrEmpty(booksPath) || string.IsNullOrEmpty(friendsPath))
                throw new System.Exception("File paths for Lending/Books/Friends not set in appsettings.json");

            List<LendingRecord> records = FileService.LoadLendingRecords(lendingPath);
            List<Book> books = FileService.LoadBooks(booksPath);
            List<Friend> friends = FileService.LoadFriends(friendsPath);

            var libService = new LibraryService(books);
            var friendService = new FriendService(friends);

            return new LendingService(records, libService, friendService);
        }
    }
}
