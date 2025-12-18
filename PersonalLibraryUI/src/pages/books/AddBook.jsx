import { useState } from "react";
import api from "../../api/axios";

function AddBook() {
  const [form, setForm] = useState({
    title: "",
    author: "",
    genre: "Fiction",
    status: "ToRead",
    rating: 1,
    dateAdded: "",
    dateStarted: "",
    dateFinished: "",
    review: "",
    isLent: false
  });

  const handleChange = e => {
    const { name, value, type, checked } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value
    }));
  };

  const handleSubmit = async e => {
    e.preventDefault();

    await api.post("/db/books", {
      ...form,
      rating: Number(form.rating)
    });

    alert("Book added successfully");
  };

  return (
    <div className="form-card">
  <h3>Add Book</h3>

<div className="form-grid">

  <div className="field">
    <label>Title</label>
    <input placeholder="Enter book title" />
  </div>

  <div className="field">
    <label>Author</label>
    <input placeholder="Enter author name" />
  </div>

  <div className="field">
    <label>Genre</label>
    <select>
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

  <div className="field">
    <label>Reading Status</label>
    <select>
      <option>ToRead</option>
      <option>CurrentlyReading</option>
      <option>Finished</option>
    </select>
  </div>

  <div className="field">
    <label>Rating</label>
    <select>
      <option value="1">⭐</option>
      <option value="2">⭐⭐</option>
      <option value="3">⭐⭐⭐</option>
      <option value="4">⭐⭐⭐⭐</option>
      <option value="5">⭐⭐⭐⭐⭐</option>
    </select>
  </div>

  <div className="field">
    <label>Date Added</label>
    <input type="datetime-local" />
  </div>

  <div className="field">
    <label>Date Started</label>
    <input type="datetime-local" />
  </div>

  <div className="field">
    <label>Date Finished</label>
    <input type="datetime-local" />
  </div>

</div>


<div className="field full-width">
  <label>Review</label>
  <textarea
    className="review-field"
    placeholder="Write your thoughts about the book..."
  />
</div>


  <div className="form-footer">
<label className="checkbox">
  <input type="checkbox" />
  Is this book currently lent to someone?
</label>

    <button>Add Book</button>
  </div>
</div>
  );
}

export default AddBook;
