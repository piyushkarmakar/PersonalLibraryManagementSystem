import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";

import Books from "./pages/Books";
import Friends from "./pages/Friends";
import Reading from "./pages/Reading";
import Lending from "./pages/Lending";
import Home from "./pages/Home";

function App() {
  return (
    <BrowserRouter>
      <Navbar />

      <Routes>
        <Route
          path="/"
          element={
            <Home
              title="Personal Library Management System"
              image="https://images.unsplash.com/photo-1524995997946-a1c2e315a42f"
            />
          }
        />

        <Route
          path="/books"
          element={
            <>
              <Home
                title="Books Collection"
                image="https://images.unsplash.com/photo-1512820790803-83ca734da794"
              />
              <Books />
            </>
          }
        />

        <Route
          path="/friends"
          element={
            <Home
              title="Friends"
              image="https://images.unsplash.com/photo-1529156069898-49953e39b3ac"
            />
          }
        />

        <Route
          path="/reading"
          element={
            <Home
              title="Reading Progress"
              image="https://images.unsplash.com/photo-1509021436665-8f07dbf5bf1d"
            />
          }
        />

        <Route
          path="/lending"
          element={
            <Home
              title="Lending Records"
              image="https://images.unsplash.com/photo-1521587760476-6c12a4b040da"
            />
          }
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
