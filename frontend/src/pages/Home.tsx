import "../css/Home.css"
function Home() {
  return <>
    <div className="main-container">
      <div className="home-page-content">
        
        <div className="title-container">
          <h1 className="title">DRUMMOND</h1>
          <hr></hr>
        </div>

        <div className = "game-options-container">
          <button className="game-option-button">STARTING 5 BATTLES</button>
          <button className="game-option-button">GRID</button>
          <button className="game-option-disabled-button">COMING SOON</button>
        </div>

        <p className="footer">DETROIT BASKETBALL</p>
      </div>
    </div>
  </>
}

export default Home
