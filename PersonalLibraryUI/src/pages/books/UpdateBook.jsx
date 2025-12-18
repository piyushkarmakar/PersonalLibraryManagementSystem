import { useState } from "react";
import api from "../../api/axios";

function UpdateBook() {
  const [id, setId] = useState("");

  const [title, setTitle] = useState("");
  const [author, setAuthor] = useState("");
  const [genre, setGenre] = useState("Fiction");
  const [status, setStatus] = useState("ToRead");
  const [rating, setRating] = useState(1);

  const [dateAdded, setDateAdded] = useState("");
  const [dateStarted, setDateStarted] = useState("");
  const [dateFinished, setDateFinished] = useState("");

  const [review, setReview] = useState("");
  const [isLent, setIsLent] = useState(false);

  // -----------------------------------------
  // Fetch book details when ID is entered
  // -----------------------------------------
  const fetchBookById = async () => {
    if (!id) return;

    try {
      const res = await api.get(`/db/books/${id}`);
      const book = res.data;

      setTitle(book.title || "");
      setAuthor(book.author || "");
      setGenre(book.genre || "Fiction");
      setStatus(book.status || "ToRead");
      setRating(book.rating || 1);

      setDateAdded(book.dateAdded?.slice(0, 16) || "");
      setDateStarted(book.dateStarted?.slice(0, 16) || "");
      setDateFinished(book.dateFinished?.slice(0, 16) || "");

      setReview(book.review || "");
      setIsLent(book.isLent || false);
    } catch (err) {
      alert("Book not found");
    }
  };

  // -----------------------------------------
  // PUT update
  // -----------------------------------------
  const updateBook = async () => {
    if (!id) {
      alert("Book ID is required");
      return;
    }

    await api.put(`/db/books/${id}`, {
      title,
      author,
      genre,
      status,
      rating,
      dateAdded,
      dateStarted,
      dateFinished,
      review,
      isLent
    });

    alert("Book updated successfully");
  };

  return (
    <div className="form-card">
      <h3>Update Book</h3>

      <div className="form-grid">

        {/* BOOK ID */}
        <div className="field full-width">
          <label>Book ID</label>
          <input
            placeholder="Enter Book ID"
            value={id}
            onChange={e => setId(e.target.value)}
            onBlur={fetchBookById}
          />
        </div>

        {/* TITLE */}
        <div className="field">
          <label>Title</label>
          <input
            placeholder="Enter book title"
            value={title}
            onChange={e => setTitle(e.target.value)}
          />
        </div>

        {/* AUTHOR */}
        <div className="field">
          <label>Author</label>
          <input
            placeholder="Enter author name"
            value={author}
            onChange={e => setAuthor(e.target.value)}
          />
        </div>

        {/* GENRE */}
        <div className="field">
          <label>Genre</label>
          <select value={genre} onChange={e => setGenre(e.target.value)}>
          <option>Fiction</option>
          <option>NonFiction</option>
          <option>Biography</option>
          <option>SciFi</option>
          <option>Romance</option>
          <option>Mystery</option>
          <option>Technical</option>
          <option>Mythology</option>
          <option>Other</option>
          </select>
        </div>

        {/* STATUS */}
        <div className="field">
          <label>Reading Status</label>
          <select value={status} onChange={e => setStatus(e.target.value)}>
            <option>ToRead</option>
            <option>CurrentlyReading</option>
            <option>Finished</option>
          </select>
        </div>

        {/* RATING */}
        <div className="field">
          <label>Rating</label>
          <select value={rating} onChange={e => setRating(Number(e.target.value))}>
            <option value="1">⭐</option>
            <option value="2">⭐⭐</option>
            <option value="3">⭐⭐⭐</option>
            <option value="4">⭐⭐⭐⭐</option>
            <option value="5">⭐⭐⭐⭐⭐</option>
          </select>
        </div>

        {/* DATE ADDED */}
        <div className="field">
          <label>Date Added</label>
          <input
            type="datetime-local"
            value={dateAdded}
            onChange={e => setDateAdded(e.target.value)}
          />
        </div>

        {/* DATE STARTED */}
        <div className="field">
          <label>Date Started</label>
          <input
            type="datetime-local"
            value={dateStarted}
            onChange={e => setDateStarted(e.target.value)}
          />
        </div>

        {/* DATE FINISHED */}
        <div className="field">
          <label>Date Finished</label>
          <input
            type="datetime-local"
            value={dateFinished}
            onChange={e => setDateFinished(e.target.value)}
          />
        </div>
      </div>

      {/* REVIEW */}
      <div className="field full-width">
        <label>Review</label>
        <textarea
          className="review-field"
          placeholder="Write your thoughts about the book..."
          value={review}
          onChange={e => setReview(e.target.value)}
        />
      </div>

      {/* FOOTER */}
      <div className="form-footer">
        <label className="checkbox">
          <input
            type="checkbox"
            checked={isLent}
            onChange={e => setIsLent(e.target.checked)}
          />
          Is this book currently lent?
        </label>

        <button onClick={updateBook}>Update Book</button>
      </div>
    </div>
  );
}

export default UpdateBook;
