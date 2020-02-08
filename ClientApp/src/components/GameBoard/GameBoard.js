import React from 'react';
import './GameBoard.css';

export const GameBoard = ({ gameBoardTiles, isPlayerTurn, handleTileSelection }) => (
 
  <div className='game-board'>
    { renderTiles(gameBoardTiles, isPlayerTurn, handleTileSelection) }
  </div>
  
);

function renderTiles(gameBoardTiles, isPlayerTurn, handleTileSelection) {

  const renderedTiles = gameBoardTiles.map((symbol, index) => (

    <button 
      className='game-board__tile'
      disabled={ !isPlayerTurn }
      onClick={ () => handleTileSelection(index) }
      key={ symbol + index.toString() }
    >
      { symbol }
    </button>
  ));

  return renderedTiles;

}

