import React from 'react';

export const NewGameForm = ({ handleFormSubmit }) => (
    <section>
        <h3>New Game Settings</h3>
        <form onSubmit={ (e) => { e.preventDefault(); handleFormSubmit(e.target.elements.symbol.value); }}>
            <label>Choose your symbol</label>
            <input type="radio" name="symbol" value="X" checked /> X
            <input type="radio" name="symbol" value="O" /> O
            <button>Start New Game</button>
        </form>
    </section>
);