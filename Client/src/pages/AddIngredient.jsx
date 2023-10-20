import React, { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Navigation from '../components/Navigation';

export default function AddIngredient() {
  const [name, setName] = useState('')
  const [imageurl, setImage] = useState('')
  const [calories, setCalories] = useState('')
  const [show, setShow] = useState(false)
  const [strModal, setstrModal] = useState('')

  // Show modal on success post ingredient
  useEffect(() => {
    if (strModal !== '')
      handleShow()
  }, [strModal])

  // Post new ingredient to DB
  const addIngredient = (event) => {
    event.preventDefault()
    const ingredientJSON = {
      Id: 0,
      Name: name,
      ImageURl: imageurl,
      Calories: calories
    }
    console.log(ingredientJSON)
    const api = `https://localhost:7050/api/Ingredients`
    fetch(api, {
      method: 'POST',
      body: JSON.stringify(ingredientJSON),
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
          console.log("success post= ", result);
          setstrModal(
            <div className='addResult'>
          <p>Ingredient Was Added Successfully</p>
          </div>
          )
        },
        (error) => {
          console.log("err post=", error);
          setstrModal(
            <div className='addResult'>
          <p>`Error: ${error}`</p>
          </div>
          )
        })
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
            <h2>Add Ingredient</h2>
          </div>
          <form onSubmit={addIngredient}>
            <label>Ingredient Name:</label>
            <br />
            <input
              type='text'
              required
              onChange={(e) => setName(e.target.value)}
            />
            <br />
            <label>Image URL:</label>
            <br />
            <input
              type='text'
              required
              onChange={(e) => setImage(e.target.value)} />
            <br />
            <label>Calories:</label>
            <br />
            <input
              type='number'
              min='0'
              required
              onChange={(e) => setCalories(e.target.value)} />
            <br />
            <button
              className='recbtn'
              type='submit'>
              Add Ingredient!
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
