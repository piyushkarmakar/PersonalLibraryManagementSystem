import { useEffect, useState } from "react";
import api from "../../api/axios";
import BookCard from "./BookCard";

function ViewBooks() {
  const [books, setBooks] = useState([]);

  useEffect(() => {
    api.get("/db/books").then(res => setBooks(res.data));
  }, []);

  return (
    <div className="books-list">
      {books.map(book => (
        <BookCard key={book.id} book={book} />
      ))}
    </div>
  );
}

export default ViewBooks;
