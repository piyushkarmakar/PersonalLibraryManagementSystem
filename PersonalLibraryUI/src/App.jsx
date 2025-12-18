import { Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";

// Main Pages
import Home from "./pages/Home";
import Books from "./pages/Books";
import Friends from "./pages/Friends";
import Reading from "./pages/Reading";
import Lending from "./pages/Lending";

// Book Sub Pages
import ViewBooks from "./pages/books/ViewBooks";
import AddBook from "./pages/books/AddBook";
import ViewBookById from "./pages/books/ViewBookById";
import UpdateBook from "./pages/books/UpdateBook";
import PatchBook from "./pages/books/PatchBook";
import DeleteBook from "./pages/books/DeleteBook";


function App() {
  return (
    <>
      <Navbar />

      <Routes>
        {/* Home */}
        <Route
          path="/"
          element={
            <Home
              title="Personal Library Management System"
              image="https://images.unsplash.com/photo-1524995997946-a1c2e315a42f"
            />
          }
        />

        {/* Books Parent Route */}
        <Route
          path="/books"
          element={
            <>
              <Home
                title="Books Management"
                image="https://images.unsplash.com/photo-1512820790803-83ca734da794"
              />
              <Books />
            </>
          }
        >
  <Route path="add" element={<AddBook />} />
  <Route path="all" element={<ViewBooks />} />
  <Route path="view" element={<ViewBookById />} />
  <Route path="update" element={<UpdateBook />} />
  <Route path="patch" element={<PatchBook />} />
  <Route path="delete" element={<DeleteBook />} />
</Route>

        {/* Friends */}
        <Route
          path="/friends"
          element={
            <>
              <Home
                title="Friends"
                image="https://images.unsplash.com/photo-1529156069898-49953e39b3ac"
              />
              <Friends />
            </>
          }
        />

        {/* Reading */}
        <Route
          path="/reading"
          element={
            <>
              <Home
                title="Reading Progress"
                image="https://images.unsplash.com/photo-1509021436665-8f07dbf5bf1d"
              />
              <Reading />
            </>
          }
        />

        {/* Lending */}
        <Route
          path="/lending"
          element={
            <>
              <Home
                title="Lending Records"
                image="https://images.unsplash.com/photo-1521587760476-6c12a4b040da"
              />
              <Lending />
            </>
          }
        />
      </Routes>
    </>
  );
}

export default App;
