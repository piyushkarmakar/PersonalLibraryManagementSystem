function formatDate(dateString) {
  if (!dateString) return "—";
  return new Date(dateString).toLocaleDateString();
}

function BookCard({ book }) {
  if (!book) return null;

  return (
    <div className="book-card">
      <div className="book-header">
        <h4>{book.title}</h4>
        <span className={`status ${book.status}`}>
          {book.status}
        </span>
      </div>

      <div className="book-meta">
        <span><strong>Author:</strong> {book.author}</span>
        <span><strong>Genre:</strong> {book.genre}</span>
        <span><strong>Rating:</strong> ⭐ {book.rating}/5</span>
      </div>

      <p className="book-review">
        {book.review}
      </p>

      <div className="book-dates">
        <span>Added: {formatDate(book.dateAdded)}</span>
        {book.dateStarted && <span>Started: {formatDate(book.dateStarted)}</span>}
        {book.dateFinished && <span>Finished: {formatDate(book.dateFinished)}</span>}
        <span>Lent: {book.isLent ? "Yes" : "No"}</span>
      </div>
    </div>
  );
}

export default BookCard;
