import '../css/Navigation.css';
import { Link } from 'react-router-dom';

export default function Navigation() {
    return (
        <div className='container-fulid'>
            <div className='nav'>
                <Link to="/">Home</Link>
                <Link to="/AddIngredient">Add Ingredient</Link>
                <Link to="/AddRecipe">Add Recipe</Link>
            </div>
        </div>
    )
}
