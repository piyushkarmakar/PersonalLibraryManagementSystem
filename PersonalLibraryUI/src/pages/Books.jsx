import { Link, Outlet } from "react-router-dom";
import "./Books.css";

function Books() {
  return (
    <div className="books-page">
      {/* Page Header */}
      <div className="books-header">
        <p>Manage your personal library collection</p>
      </div>

      {/* Action Bar */}
      <div className="books-actions">
        <Link to="/books/add">Add Book</Link>
        <Link to="/books/all">View All</Link>
        <Link to="/books/view">View by ID</Link>
        <Link to="/books/update">Update</Link>
        <Link to="/books/patch">Patch</Link>
        <Link to="/books/delete">Delete</Link>
      </div>

      {/* Content Area */}
      <div className="books-content">
        <Outlet />
      </div>
    </div>
  );
}

export default Books;
