import "../css/Home.css"
import { useNavigate } from 'react-router-dom';

function Home() {
  const navigate = useNavigate();

  function redirectToLogin() {
    navigate('/starting5');
  }
  
  return <>
    <div className="main-container">
      <div className="home-page-content">

        <div className="title-container">
          <h1 className="title">DRUMMOND</h1>
          <hr></hr>
        </div>

        <div className = "game-options-container">
          <button className="game-option-button" onClick={redirectToLogin}>STARTING 5 DAILY</button>
        </div>

        <p className="footer">DETROIT BASKETBALL</p>
      </div>
    </div>
  </>
}

export default Home
