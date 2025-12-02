using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;

namespace PersonalLibraryAPI.Services
{
    public class LibraryManager
    {
        private readonly IConfiguration _config;

        public LibraryManager(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Returns DB-mode LibraryService
        /// </summary>
        public LibraryService CreateDbService()
        {
            string conn = _config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(conn))
                throw new System.Exception("DefaultConnection not set in appsettings.json");

            return new LibraryService(conn);
        }

        /// <summary>
        /// Returns File-mode LibraryService
        /// </summary>
        public LibraryService CreateFileService()
        {
            var booksPath = _config["FilePaths:BooksFile"];
            if (string.IsNullOrEmpty(booksPath))
                throw new System.Exception("File path for BooksFile not set in appsettings.json");

            List<Book> books = FileService.LoadBooks(booksPath);
            return new LibraryService(books);
        }
    }
}
