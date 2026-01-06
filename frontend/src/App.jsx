import { Routes, Route} from 'react-router-dom'
import Home from './pages/Home'
import Dashboard from "./pages/Dashboard"
import Starting5 from './pages/Starting5'

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path = "/starting5" element = {<Starting5 />} />
        <Route path = "/dashboard" element = {<Dashboard />} />
      </Routes>
    </>
  )
}

export default App
