import React, { Component } from 'react';
import { GameBoard } from './components/GameBoard/GameBoard';
import * as request from 'request-promise';
import './App.css';

import './custom.css'

export default class App extends Component {

  state = {
    isComplete: false,
    playerSymbol: 'X',
    opponentSymbol: 'O',
    gameBoardTiles: ['?', '?', '?', '?', '?', '?', '?', '?', '?'],
    isPlayerTurn: true,
  };

  render () {
    const { isComplete, gameBoardTiles, playerSymbol, opponentSymbol, isPlayerTurn } = this.state;

    return (
      <div className='tic-tac-toe-board-container'>
        <GameBoard 
          gameBoardTiles={ gameBoardTiles }
          isPlayerTurn={ isPlayerTurn }
          handleTileSelection={ this.handleTileSelection }
        ></GameBoard>
      </div>
    );
  }

  handleTileSelection = (index) => {
    this.state.gameBoardTiles[index] = this.state.playerSymbol;
    this.state.isPlayerTurn = false;

    this.executeMove();

  }

  async executeMove() {
    const options = {
      method: 'POST',
      uri: 'https://localhost:5001/executemove',
      json: true,
      body: {
        move: 1,
        azurePlayerSymbol: 'X',
        humanPlayerSymbol: 'O',
        gameBoard: ['?', '?', '?', '?', '?', '?', '?', '?', '?'],
      }
    };

    const response = await request(options);
    console.log(response);
  }
}
