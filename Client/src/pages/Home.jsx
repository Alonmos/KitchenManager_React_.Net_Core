import '../css/Home.css';
import React from 'react'
import { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Navigation from '../components/Navigation';

export default function Home() {
    const [recipes, setRecipes] = useState([])
    const [strRecipes, setStrRecipes] = useState('')
    const [recipeIngs, setRecipeIng] = useState('')
    const [show, setShow] = useState(false)

    // Get all recipes from DB
    useEffect(() => {
        getRecipes()
    }, [])

    // Get all recipes from DB
    useEffect(() => {
        if (recipeIngs !== '')
            handleShow()
    }, [recipeIngs])

    // Render recipes from DB
    useEffect(() => {
        renderRecipes()
    }, [recipes])

    // Get all recipes from DB
    const getRecipes = () => {
        const api = 'https://localhost:7050/api/Recipes'
        fetch(api)
            .then(res => {
                console.log('res=', res);
                return res.json()
            })
            .then(
                (result) => {
                    console.log("success getRecipes= ", result);
                    setRecipes(result)
                },
                (error) => {
                    console.log("err getRecipes=", error);
                    setStrRecipes(
                        <div className='appMsg'>
                            <h2>
                                Server Error. Please try again later
                            </h2>
                        </div>
                    )
                }
            )
    }

    // Get recipe ingredient from DB
    const getIngredients = (recipeID) => {
        const api = `https://localhost:7050/api/Ingredients/id/${recipeID}`
        fetch(api)
            .then((res) => {
                return res.json()
            })
            .then(
                (result) => {
                    const str = result.map((ing, indx) => (
                        <div key={indx} className='col-6 col-md-4'>
                            <div className='ingdiv'>
                                <img
                                    className='imgIng'
                                    src={ing.imageURL}
                                />
                                <p>Name: {ing.name}</p>
                                <p>Calories: {ing.calories}</p>
                            </div>
                        </div>
                    ))
                    setRecipeIng(str)
                },
                (error) => {
                    console.error("Error:", error);
                    setStrRecipes(<h2>Error: {error.message}</h2>)
                }
            )
    }

    // Render recipes from DB
    const renderRecipes = () => {
        let str = ''
        if (recipes.length > 0) {
            str = recipes.map((rec) => {
                return (
                    <div className='col-md-6 col-lg-4'>
                        <div className='recdiv'>
                            <img className='imgrecipe'
                                src={rec.imageURl}
                            />
                            <div className='recName'>
                                <p>{rec.name}</p>
                            </div>
                            <p>{rec.cookingMethod}</p>
                            <p>Cooking Time: {rec.time} Minutes</p>
                            <button
                                className='recbtn'
                                variant="primary"
                                onClick={() => getIngredients(rec.id)}>
                                Show Ingredients
                            </button>
                            <br />
                        </div>
                    </div>
                    )
            })
        }
        else {
            str = <div className='appMsg'>
                <h2>No Recipes Yet</h2>
            </div>
        }
        setStrRecipes(str)
    }

    // Show modal
    const handleShow = () => {
        setShow(true)
    }

    // Close modal
    const handleClose = () => {
        setShow(false)
    }

    return (
        <div className='container-fulid'>
            <Navigation />
            <div className='content'>
                <div className='header'>
                    <h2>My Dishes</h2>
                </div>
                <div className='row'>
                    {strRecipes}
                </div>
                <Modal show={show}
                    onHide={handleClose}
                    animation={false}>
                    <Modal.Header closeButton>
                        <Modal.Title>
                            Ingredients
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <div className='row'>
                            {recipeIngs}
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
    )
}
