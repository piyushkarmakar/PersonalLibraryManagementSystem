import { useState } from "react";
import api from "../../api/axios";

function DeleteBook() {
  const [id, setId] = useState("");
  const [book, setBook] = useState(null);
  const [error, setError] = useState("");

  // -----------------------------------------
  // Fetch book by ID
  // -----------------------------------------
  const fetchBook = async () => {
    if (!id) return;

    try {
      const res = await api.get(`/db/books/${id}`);
      setBook(res.data);
      setError("");
    } catch {
      setBook(null);
      setError("Book not found");
    }
  };

  // -----------------------------------------
  // Delete book
  // -----------------------------------------
  const deleteBook = async () => {
    if (!book) return;

    const confirmDelete = window.confirm(
      `Are you sure you want to delete "${book.title}" by ${book.author}?`
    );

    if (!confirmDelete) return;

    await api.delete(`/db/books/${id}`);
    alert("Book deleted successfully");

    // Reset
    setId("");
    setBook(null);
  };

  return (
    <div className="form-card">
      <h3>Delete Book</h3>

      {/* BOOK ID INPUT */}
      <div className="field full-width">
        <label>Book ID</label>
        <input
          placeholder="Enter Book ID"
          value={id}
          onChange={e => setId(e.target.value)}
          onBlur={fetchBook}
        />
      </div>

      {/* ERROR */}
      {error && <p style={{ color: "#ff6b6b" }}>{error}</p>}

      {/* BOOK PREVIEW */}
      {book && (
        <div
          style={{
            marginTop: "16px",
            padding: "16px",
            borderRadius: "12px",
            background: "rgba(255,255,255,0.05)"
          }}
        >
          <p><strong>Title:</strong> {book.title}</p>
          <p><strong>Author:</strong> {book.author}</p>
        </div>
      )}

      {/* DELETE BUTTON */}
      {book && (
        <div className="form-footer" style={{ marginTop: "20px" }}>
          <button
            onClick={deleteBook}
            style={{
              background: "linear-gradient(to right, #d32f2f, #b71c1c)"
            }}
          >
            Delete Book
          </button>
        </div>
      )}
    </div>
  );
}

export default DeleteBook;
