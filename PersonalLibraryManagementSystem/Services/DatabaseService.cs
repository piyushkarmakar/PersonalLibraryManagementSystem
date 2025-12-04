using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Enums;
using Microsoft.Data.SqlClient;

namespace PersonalLibraryManagementSystem.Services
{
    public class DatabaseService
    {


        private string connectionString;
        public DatabaseService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // 🔹 Add Book to Database
        public void AddBook(Book book)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Books 
                                (Title, Author, Genre, Status, Rating, DateAdded, DateStarted, DateFinished, Review, IsLent)
                                VALUES (@Title, @Author, @Genre, @Status, @Rating, @DateAdded, @DateStarted, @DateFinished, @Review, @IsLent)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Genre", book.Genre.ToString());
                cmd.Parameters.AddWithValue("@Status", book.Status.ToString());
                cmd.Parameters.AddWithValue("@Rating", book.Rating);
                cmd.Parameters.AddWithValue("@DateAdded", book.DateAdded);
                cmd.Parameters.AddWithValue("@DateStarted", (object)book.DateStarted ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateFinished", (object)book.DateFinished ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Review", book.Review ?? "");
                cmd.Parameters.AddWithValue("@IsLent", book.IsLent);
                cmd.ExecuteNonQuery();
            }
        }

        // 🔹 Get All Books
        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Books", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Book book = new Book();
                    book.Id = Convert.ToInt32(reader["Id"]);
                    book.Title = reader["Title"].ToString();
                    book.Author = reader["Author"].ToString();

                    Genre genre;
                    Enum.TryParse(reader["Genre"].ToString(), out genre);
                    book.Genre = genre;

                    BookStatus status;
                    Enum.TryParse(reader["Status"].ToString(), out status);
                    book.Status = status;

                    book.Rating = Convert.ToInt32(reader["Rating"]);
                    book.DateAdded = Convert.ToDateTime(reader["DateAdded"]);
                    book.DateStarted = reader["DateStarted"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DateStarted"]);
                    book.DateFinished = reader["DateFinished"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DateFinished"]);
                    book.Review = reader["Review"].ToString();
                    book.IsLent = Convert.ToBoolean(reader["IsLent"]);

                    books.Add(book);
                }
            }

            return books;
        }

        // 🔹 Delete Book
        public void DeleteBook(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Books WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // 🔹 Update Book
        public void UpdateBook(Book book)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Books SET 
                                Title=@Title, Author=@Author, Genre=@Genre, Status=@Status, Rating=@Rating,
                                Review=@Review, IsLent=@IsLent, DateStarted=@DateStarted, DateFinished=@DateFinished WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", book.Id);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Genre", book.Genre.ToString());
                cmd.Parameters.AddWithValue("@Status", book.Status.ToString());
                cmd.Parameters.AddWithValue("@Rating", book.Rating);
                cmd.Parameters.AddWithValue("@Review", book.Review ?? "");
                cmd.Parameters.AddWithValue("@IsLent", book.IsLent);
                cmd.Parameters.AddWithValue("@DateStarted", (object)book.DateStarted ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateFinished", (object)book.DateFinished ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
        // 🔹 Add Friend
        public void AddFriend(Friend friend)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Friends (Name, Email, Phone) VALUES (@Name, @Email, @Phone)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", friend.Name);
                cmd.Parameters.AddWithValue("@Email", friend.Email);
                cmd.Parameters.AddWithValue("@Phone", friend.Phone);
                cmd.ExecuteNonQuery();
            }
        }
        // 🔹 Update Friend by Id
        public void UpdateFriendById(Friend friend)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Friends SET Name=@Name, Email=@Email, Phone=@Phone WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", friend.Id);
                cmd.Parameters.AddWithValue("@Name", friend.Name);
                cmd.Parameters.AddWithValue("@Email", friend.Email);
                cmd.Parameters.AddWithValue("@Phone", friend.Phone);
                cmd.ExecuteNonQuery();
            }
        }

        // 🔹 Delete Friend by Id
        public void DeleteFriendById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Friends WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteFriendByName(string name)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Friends WHERE Name=@name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
            }
        }



        // 🔹 Get All Friends
        public List<Friend> GetAllFriends()
        {
            List<Friend> friends = new List<Friend>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Friends", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Friend friend = new Friend();
                    friend.Id = Convert.ToInt32(reader["Id"]);
                    friend.Name = reader["Name"].ToString();
                    friend.Email = reader["Email"].ToString();
                    friend.Phone = reader["Phone"].ToString();
                    friends.Add(friend);
                }
            }
            return friends;
        }



        // 🔹 Add Lending Record
        public void AddLendingRecord(LendingRecord record)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO LendingRecords (BookId, BookName, FriendName, LendDate, DueDate, ReturnDate)
                         VALUES (@BookId, @BookName, @FriendName,
                                @LendDate, @DueDate, @ReturnDate)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookId", record.BookId);
                cmd.Parameters.AddWithValue("@BookName", record.BookName ?? "");
                cmd.Parameters.AddWithValue("@FriendName", record.FriendName);
                cmd.Parameters.AddWithValue("@LendDate", record.LendDate);
                cmd.Parameters.AddWithValue("@DueDate", record.DueDate);
                cmd.Parameters.AddWithValue("@ReturnDate", (object)record.ReturnDate ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        // 🔹 Get All Lending Records
        public List<LendingRecord> GetAllLendingRecords()
        {
            List<LendingRecord> records = new List<LendingRecord>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT BookId, BookName, FriendName,
                                                LendDate, DueDate, ReturnDate FROM LendingRecords", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    LendingRecord record = new LendingRecord();
                    record.BookId = Convert.ToInt32(reader["BookId"]);
                    record.BookName = reader["BookName"].ToString();
                    record.FriendName = reader["FriendName"].ToString();
                    record.LendDate = Convert.ToDateTime(reader["LendDate"]);
                    record.DueDate = Convert.ToDateTime(reader["DueDate"]);
                    record.ReturnDate = reader["ReturnDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ReturnDate"]);
                    records.Add(record);
                }
            }
            return records;
        }

        // 🔹 Mark Book as Returned
        public void MarkAsReturned(int bookId, string friendName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                                                    UPDATE LendingRecords
                                                    SET ReturnDate = GETDATE()
                                                    WHERE BookId = @BookId 
                                                    AND FriendName = @FriendName
                                                    AND ReturnDate IS NULL", conn);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                cmd.Parameters.AddWithValue("@FriendName", friendName);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Book> GetReadingProgress()
        {
            var books = new List<Book>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Title, Status, Rating, Review FROM Books";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Title = reader["Title"].ToString(),
                                Status = Enum.TryParse(reader["Status"].ToString(), out BookStatus st) ? st : BookStatus.ToRead,
                                Rating = reader["Rating"] != DBNull.Value ? Convert.ToInt32(reader["Rating"]) : 0,
                                Review = reader["Review"]?.ToString()
                            });
                        }
                    }
                }
            }
            return books;
        }

        public bool StartReading(int bookId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Books SET Status=@Status, DateStarted=@DateStarted WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", BookStatus.CurrentlyReading.ToString());
                    cmd.Parameters.AddWithValue("@DateStarted", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Id", bookId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0; // true if a row was updated
                }
            }
        }

        public bool FinishReading(int bookId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Books SET Status=@Status, DateFinished=@DateFinished WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", BookStatus.Finished.ToString());
                    cmd.Parameters.AddWithValue("@DateFinished", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Id", bookId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public bool AddRatingReview(int bookId, int rating, string review)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Books SET Rating=@Rating, Review=@Review WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.Parameters.AddWithValue("@Review", review ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Id", bookId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
        public Book GetBookById(int bookId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Books WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", bookId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Book
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Title = reader["Title"].ToString(),
                                Author = reader["Author"].ToString(),
                                Genre = Enum.TryParse(reader["Genre"].ToString(), out Genre g) ? g : Genre.Other,
                                Status = Enum.TryParse(reader["Status"].ToString(), out BookStatus st) ? st : BookStatus.ToRead,
                                Rating = reader["Rating"] != DBNull.Value ? Convert.ToInt32(reader["Rating"]) : 0,
                                Review = reader["Review"]?.ToString(),
                                DateAdded = Convert.ToDateTime(reader["DateAdded"]),
                                DateStarted = reader["DateStarted"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DateStarted"]),
                                DateFinished = reader["DateFinished"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DateFinished"]),
                                IsLent = Convert.ToBoolean(reader["IsLent"])
                            };
                        }
                        return null;
                    }
                }
            }
        }




    }
}

