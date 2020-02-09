import React from 'react';
import './NewGameForm.css';

export const NewGameForm = ({ handleFormSubmit }) => (
    <section className='new-game-form'>
        <h3>New Game Settings</h3>
        <form onSubmit={ (e) => { e.preventDefault(); handleFormSubmit(e.target.elements.symbol.value); }}>
            <div className='new-game-form__body'>
                <label className='new-game-form__body__label'>Choose your symbol</label>
                <div>
                    <input type="radio" name="symbol" value="X" checked /> X
                    <br></br>
                    <input type="radio" name="symbol" value="O" /> O
                </div>
            </div>
        
            <button className='new-game-form__button'>Start New Game</button>
        </form>
    </section>
);