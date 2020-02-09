import React from 'react';
import './GameBoard.css';
import posed from 'react-pose';

const DividerVerticalLeftAnimation = posed.div({
  enter: {
      y: 0,
      opacity: 1,
      delay: 300,
      transition: {
        y: { type: 'spring', stiffness: 300, damping: 12 },
        default: { duration: 300 }
      }
  },
  exit: {
      y: 50,
      opacity: 0,
      transition: { duration: 150 }
    }
});

let isVisible = false;

export const GameBoard = ({ gameBoardTiles, isPlayerTurn, handleTileSelection }) => {

  isVisible = true;
 
  return (
    <div className='game-board-container'>
      <div className='game-board-container__grid'>
        { renderTiles(gameBoardTiles, isPlayerTurn, handleTileSelection) }
      </div>
      <div className='game-board-container__dividers'>
        {/* <DividerVerticalLeftAnimation pose={ isVisible ? 'enter' : 'exit'}> */}
        <div className='divider__vertical__left divider'></div>
        {/* </DividerVerticalLeftAnimation> */}
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