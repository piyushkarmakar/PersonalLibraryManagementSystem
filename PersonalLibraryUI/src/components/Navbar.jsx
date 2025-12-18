import { Link } from "react-router-dom";
import "./Navbar.css";

function Navbar() {
  return (
    <nav className="navbar">
      <div className="logo">Personal Library</div>

      <ul className="nav-links">
        {/* BOOKS DROPDOWN */}
        <li className="dropdown">
          <span>Books ▾</span>
          <ul className="dropdown-menu">
            <li>
              <Link to="/books/add">Add Book</Link>
            </li>
            <li>
              <Link to="/books/all">View All Books</Link>
            </li>
            <li>
              <Link to="/books/view">View Book by ID</Link>
            </li>
            <li>
              <Link to="/books/update">Update Whole Book</Link>
            </li>
            <li>
              <Link to="/books/patch">Update Particular Field</Link>
            </li>
            <li>
              <Link to="/books/delete">Delete Book</Link>
            </li>
          </ul>
        </li>

        {/* OTHER NAV ITEMS */}
        <li>
          <Link to="/friends">Friends</Link>
        </li>
        <li>
          <Link to="/reading">Reading Progress</Link>
        </li>
        <li>
          <Link to="/lending">Lending</Link>
        </li>
      </ul>
    </nav>
  );
}

export default Navbar;
