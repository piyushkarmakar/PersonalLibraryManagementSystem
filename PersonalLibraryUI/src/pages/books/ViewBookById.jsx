import { useState } from "react";
import api from "../../api/axios";
import BookCard from "./BookCard";

function ViewBookById() {
  const [id, setId] = useState("");
  const [book, setBook] = useState(null);
  const [error, setError] = useState(null);

  const fetchBook = async () => {
    try {
      const res = await api.get(`/db/books/${id}`);
      setBook(res.data);
      setError(null);
    } catch {
      setBook(null);
      setError("Book not found");
    }
  };

  return (
    <div>
      <h3>View Book by ID</h3>

      <input
        placeholder="Book ID"
        value={id}
        onChange={e => setId(e.target.value)}
      />
      <button onClick={fetchBook}>Fetch</button>

      {error && <p>{error}</p>}
      {book && <BookCard book={book} />}
    </div>
  );
}

export default ViewBookById;
