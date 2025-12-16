import { Link } from "react-router-dom";
import "./Navbar.css";

function Navbar() {
  return (
    <nav className="navbar">
      <h2 className="logo">Personal Library</h2>

      <ul className="nav-links">
        <li><Link to="/books">Books</Link></li>
        <li><Link to="/friends">Friends</Link></li>
        <li><Link to="/reading">Reading Progress</Link></li>
        <li><Link to="/lending">Lending</Link></li>
      </ul>
    </nav>
  );
}

export default Navbar;
