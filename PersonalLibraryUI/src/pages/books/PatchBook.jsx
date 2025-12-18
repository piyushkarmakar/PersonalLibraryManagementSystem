import { useState } from "react";
import api from "../../api/axios";

function PatchBook() {
  const [id, setId] = useState("");
  const [originalBook, setOriginalBook] = useState(null);

  // Editable fields
  const [title, setTitle] = useState("");
  const [author, setAuthor] = useState("");
  const [genre, setGenre] = useState("");
  const [status, setStatus] = useState("");
  const [rating, setRating] = useState("");
  const [review, setReview] = useState("");
  const [isLent, setIsLent] = useState(null);

  // -----------------------------------------
  // Fetch book when ID entered
  // -----------------------------------------
  const fetchBookById = async () => {
    if (!id) return;

    try {
      const res = await api.get(`/db/books/${id}`);
      setOriginalBook(res.data);
    } catch {
      alert("Book not found");
    }
  };

  // -----------------------------------------
  // Build PATCH payload (only changed fields)
  // -----------------------------------------
  const patchBook = async () => {
    if (!originalBook) {
      alert("Fetch book first");
      return;
    }

    const payload = {};

    if (title && title !== originalBook.title) payload.title = title;
    if (author && author !== originalBook.author) payload.author = author;
    if (genre && genre !== originalBook.genre) payload.genre = genre;
    if (status && status !== originalBook.status) payload.status = status;
    if (rating && Number(rating) !== originalBook.rating) payload.rating = Number(rating);
    if (review && review !== originalBook.review) payload.review = review;
    if (isLent !== null && isLent !== originalBook.isLent) payload.isLent = isLent;

    if (Object.keys(payload).length === 0) {
      alert("No changes detected");
      return;
    }

    await api.patch(`/db/books/${id}`, payload);
    alert("Book updated successfully");
  };

  return (
    <div className="form-card">
      <h3>Patch Book</h3>

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
          <label>Title (optional)</label>
          <input
            placeholder={originalBook?.title || "Leave unchanged"}
            value={title}
            onChange={e => setTitle(e.target.value)}
          />
        </div>

        {/* AUTHOR */}
        <div className="field">
          <label>Author (optional)</label>
          <input
            placeholder={originalBook?.author || "Leave unchanged"}
            value={author}
            onChange={e => setAuthor(e.target.value)}
          />
        </div>

        {/* GENRE */}
        <div className="field">
          <label>Genre (optional)</label>
          <select value={genre} onChange={e => setGenre(e.target.value)}>
            <option value="">Leave unchanged</option>
            <option>Fiction</option>
            <option>Mythology</option>
            <option>NonFiction</option>
            <option>Biography</option>
          </select>
        </div>

        {/* STATUS */}
        <div className="field">
          <label>Reading Status (optional)</label>
          <select value={status} onChange={e => setStatus(e.target.value)}>
            <option value="">Leave unchanged</option>
            <option>ToRead</option>
            <option>CurrentlyReading</option>
            <option>Completed</option>
          </select>
        </div>

        {/* RATING */}
        <div className="field">
          <label>Rating (optional)</label>
          <select value={rating} onChange={e => setRating(e.target.value)}>
            <option value="">Leave unchanged</option>
            <option value="1">⭐</option>
            <option value="2">⭐⭐</option>
            <option value="3">⭐⭐⭐</option>
            <option value="4">⭐⭐⭐⭐</option>
            <option value="5">⭐⭐⭐⭐⭐</option>
          </select>
        </div>
      </div>

      {/* REVIEW */}
      <div className="field full-width">
        <label>Review (optional)</label>
        <textarea
          className="review-field"
          placeholder={originalBook?.review || "Leave unchanged"}
          value={review}
          onChange={e => setReview(e.target.value)}
        />
      </div>

      {/* FOOTER */}
      <div className="form-footer">
        <label className="checkbox">
          <input
            type="checkbox"
            checked={isLent ?? originalBook?.isLent ?? false}
            onChange={e => setIsLent(e.target.checked)}
          />
          Update Lent Status
        </label>

        <button onClick={patchBook}>Patch Book</button>
      </div>
    </div>
  );
}

export default PatchBook;
