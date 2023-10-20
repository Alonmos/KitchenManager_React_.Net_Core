import '../css/AddRecipe.css';
import React, { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Navigation from '../components/Navigation';

export default function AddRecipe() {
    const [name, setName] = useState('')
    const [imageurl, setImage] = useState('')
    const [cookingMethod, setCookingMethod] = useState('')
    const [time, setTime] = useState('')
    const [ingredientsList, setIngredientsList] = useState([])
    const [strIngredients, setStrIngredients] = useState('')
    const [show, setShow] = useState(false)
    const [strModal, setstrModal] = useState('')

    // Get ingredients from DB
    useEffect(() => {
        getIngredients()
    }, [])

    // Show modal on success post ingredient
    useEffect(() => {
        if (strModal !== '')
            handleShow()
    }, [strModal])

    // Get ingredients from DB
    const getIngredients = () => {
        const api = `https://localhost:7050/api/Ingredients`
        fetch(api)
            .then(res => {
                return res.json()
            })
            .then(
                (result) => {
                    console.log("success get ingredients= ", result);
                    let str = result.map((ing, indx) => {
                        return (
                            <div key={indx} className='col-sm-3'>
                                <div className='ingdiv'>
                                    <input
                                        id={ing.id}
                                        type='checkbox'
                                        onClick={(e) => changeIngredients(e)} />
                                    <br />
                                    <img
                                        className='imgIng'
                                        src={ing.imageURl}
                                    />
                                    <p>{ing.name}</p>
                                    <p>Calories: {ing.calories}</p>
                                </div>
                            </div>)
                    });
                    setStrIngredients(str)
                },
                (error) => {
                    console.log("err get ingredients=", error);
                })
    }

    // Post recipe to the DB
    const addRecipe = (event) => {
        event.preventDefault();
        if (ingredientsList.length === 0) {
            setstrModal(
                <div className='addResult'>
                    <p>Please Choose At Least One Ingredient</p>
                </div>
                )
            return
        }
        const RecipeJSON = {
            Id: 0,
            Name: name,
            ImageURl: imageurl,
            cookingMethod: cookingMethod,
            Time: time
        }
        const api = `https://localhost:7050/api/Recipes`
        fetch(api, {
            method: 'POST',
            body: JSON.stringify(RecipeJSON),
            headers: new Headers({
                'Content-type': 'application/json; charset=UTF-8',
                'Accept': 'application/json; charset=UTF-8',
            })
        })
            .then(response => {
                return response.json()
            })
            .then(
                (result) => {
                    console.log("success post recipe: ", result);
                    addIngredientsToRecipe(result)
                },
                (error) => {
                    console.log("err post recipe =", error)
                    setstrModal(
                        <div className='addResult'>
                            <p>Error Adding Recipe. Please Try Again Later.</p>
                        </div>
                    )
                })
    }

    // Post ingredients to recipe on the DB
    const addIngredientsToRecipe = (recipeid) => {
        const api = `https://localhost:7050/api/Recipes/recipeid/${recipeid}`
        fetch(api, {
            method: 'POST',
            body: JSON.stringify(ingredientsList),
            headers: new Headers({
                'Content-type': 'application/json; charset=UTF-8',
                'Accept': 'application/json; charset=UTF-8',
            })
        })

            .then(response => {
                console.log('res=', response);
                return response.json()
            })
            .then(
                (result) => {
                    console.log("succuss post ingredients= ", result)
                    setstrModal(
                        <div className='addResult'>
                            <p>Recipe Was Added Successfully</p>
                        </div>
                    )
                },
                (error) => {
                    console.log("err ingredient post=", error)
                    setstrModal(
                        <div className='addResult'>
                            <p>Recipe Was Added Successfully</p>
                            <p>Error At Adding The Ingredients</p>
                        </div>
                    )
                })
    }

    // Change ingredients array when user choose ingredient
    const changeIngredients = (e) => {
        if (e.target.checked) {
            setIngredientsList((previngredientsList) =>
                [...previngredientsList, e.target.id]);
        }
        else {
            setIngredientsList((previngredientsList) =>
                previngredientsList.filter((ing) => ing !== e.target.id))
        }
    }

    //Open modal
    const handleShow = () => {
        setShow(true)
    }

    //Close modal
    const handleClose = () => {
        setShow(false)
    }

    return (
        <div>
            <Navigation />
            <div className='content'>
                <div className='formDiv'>
                    <div className='header'>
                        <h2>Add Recipe</h2>
                    </div>
                    <form onSubmit={addRecipe}>
                        <label>Recipe Name:</label>
                        <br />
                        <input
                            type='text'
                            required
                            onChange={(e) => setName(e.target.value)} />
                        <br />
                        <label>Image URL:</label>
                        <br />
                        <input
                            type='text'
                            required
                            onChange={(e) => setImage(e.target.value)} />
                        <br />
                        <label>Cooking Method:</label>
                        <br />
                        <input
                            type='text'
                            required
                            onChange={(e) =>
                                setCookingMethod(e.target.value)} />
                        <br />
                        <label>Time:</label>
                        <br />
                        <input
                            type='number'
                            min='0'
                            required
                            onChange={(e) => setTime(e.target.value)} />
                        <br />
                        <hr />
                        <label>Choose Ingredients:</label>
                        <br />
                        <div className='row cardrow'>
                            {strIngredients}
                        </div>
                        <button className='recbtn' type='submit'>
                            Add Recipe!
                        </button>
                    </form>
                    <Modal show={show} onHide={handleClose} animation={false}>
                        <Modal.Body>
                            <div id='mdbody' className='row'>
                                {strModal}
                            </div>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button
                                id='modBTN'
                                variant="secondary"
                                onClick={handleClose}>
                                Close
                            </Button>
                        </Modal.Footer>
                    </Modal>
                </div>
            </div>
        </div>
    )
}
