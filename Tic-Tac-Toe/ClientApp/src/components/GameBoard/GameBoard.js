import React from 'react';
import './GameBoard.css';

export const GameBoard = ({ gameBoardTiles, isPlayerTurn, handleTileSelection }) => {
 
  return (
    <div className='game-board-container'>
      <div className='game-board-container__grid'>
        { renderTiles(gameBoardTiles, isPlayerTurn, handleTileSelection) }
      </div>
      <div className='game-board-container__dividers'>
        <div className='divider__vertical__left divider'></div>
        <div className='divider__vertical__right divider'></div>
        <div className='divider__horizontal__top divider'></div>
        <div className='divider__horizontal__bottom divider'></div>
      </div>
    </div>
  );
}

function renderTiles(gameBoardTiles, isPlayerTurn, handleTileSelection) {

  const renderedTiles = gameBoardTiles.map((symbol, index) => (

    <button 
      className='game-board-container__grid__tile'
      disabled={ !isPlayerTurn }
      onClick={ () => handleTileSelection(index) }
      key={ symbol + index.toString() }
    >
      { symbol }
    </button>
  ));

  return renderedTiles;

}