using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalLibraryManagementSystem.Enums;
using PersonalLibraryManagementSystem.Interfaces;
using PersonalLibraryManagementSystem.Models;


namespace PersonalLibraryManagementSystem.Services
{




        public class LibraryService : ISearchable<Book>, IManageable<Book>
        {
            private List<Book> books;

            public LibraryService(List<Book> books)
            {
                this.books = books;
            }



            public void Add(Book book)
            {
                books.Add(book);
            }


            public void Update(int id, Book updatedBook)
            {
                var book = GetById(id);
                if (book != null)
                {
                    book.Title = updatedBook.Title;
                    book.Author = updatedBook.Author;
                    book.Genre = updatedBook.Genre;
                    book.Status = updatedBook.Status;
                }
            }



            public void Remove(int id)
            {
                for (int i = 0; i < books.Count; i++)
                {
                    if (books[i].Id == id)
                    {
                        books.RemoveAt(i);
                        break;
                    }
                }
            }


            public Book GetById(int id)
            {
                foreach (Book b in books)
                {
                    if (b.Id == id)
                    {
                        return b;
                    }
                }
                return null;
            }



            public List<Book> Search(string criteria)
            {
                List<Book> results = new List<Book>();

                foreach (Book b in books)
                {
                    if ((b.Title != null && b.Title.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (b.Author != null && b.Author.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        results.Add(b);
                    }
                }

                return results;
            }


            public int GenerateNextBookId()
            {
                if (books.Count == 0)
                {
                    return 1;
                }

                int maxId = 0;
                foreach (Book b in books)
                {
                    if (b.Id > maxId)
                    {
                        maxId = b.Id;
                    }
                }

                return maxId + 1;
            }



        public List<Book> GetAllBooks()
        {
            return books;
        }

        public void StartReading(int id)
            {
                var book = GetById(id);
                if (book != null)
                    book.MarkAsReading();
            }



            public void FinishReading(int id)
            {
                var book = GetById(id);
                if (book != null)
                    book.Status = Enums.BookStatus.Finished;
            }



            public void AddRating(int id, int rating, string review)
            {
                var book = GetById(id);
                if (book != null)
                    book.FinishReading(rating, review);
            }


        }

    

}
