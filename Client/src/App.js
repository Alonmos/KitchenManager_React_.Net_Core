import './css/App.css';
import { Routes, Route, Link } from 'react-router-dom';
import AddIngredient from './pages/AddIngredient';
import AddRecipe from './pages/AddRecipe';
import Home from './pages/Home';

function App() {
  return (
    <div className="App">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/AddIngredient" element={<AddIngredient />} />
            <Route path="/AddRecipe" element={<AddRecipe />} />
          </Routes>
    </div>
  )
}

export default App;
