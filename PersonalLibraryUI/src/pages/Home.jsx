import "./Home.css";

function Home({ title, image }) {
  return (
    <div
      className="home-banner"
      style={{ backgroundImage: `url(${image})` }}
    >
      <h1 className="home-title">{title}</h1>
    </div>
  );
}

export default Home;
