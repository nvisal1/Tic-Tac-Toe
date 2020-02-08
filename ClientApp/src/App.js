import React, { Component } from 'react';
import { GameBoard } from './components/GameBoard/GameBoard';
import { NewGameForm } from './components/NewGameForm/NewGameForm';
import * as request from 'request-promise';
import './App.css';

import './custom.css'
import { GameStateMessage } from './components/GameStateMessage/GameStateMessage';

export default class App extends Component {

  state = {
    winner: 'inconclusive',
    winPositions: [],
    playerSymbol: 'X',
    opponentSymbol: 'O',
    gameBoardTiles: ['?', '?', '?', '?', '?', '?', '?', '?', '?'],
    isPlayerTurn: true,
  };

  render () {
    const { winner, gameBoardTiles, playerSymbol, opponentSymbol, isPlayerTurn } = this.state;

    return (
      <div>
        <div>
          <GameStateMessage
            winner={ winner }
            isPlayerTurn={ isPlayerTurn }
          ></GameStateMessage>
        </div>
        <div className='game-board-container'>
          <GameBoard 
            gameBoardTiles={ gameBoardTiles }
            isPlayerTurn={ isPlayerTurn }
            handleTileSelection={ this.handleTileSelection }
          ></GameBoard>
        </div>
        <div>
          <NewGameForm handleFormSubmit={ this.handleNewGameStart }></NewGameForm>
        </div>
      </div>
      
    );
  }

  handleTileSelection = async (index) => {
    this.state.gameBoardTiles[index] = this.state.playerSymbol;
    this.state.isPlayerTurn = false;

    const executeMoveResponse = await this.executeMove(index);
    const { gameBoard, winner, winPositions, move } = executeMoveResponse;
    this.setState({ gameBoardTiles: gameBoard, winner, winPositions, isPlayerTurn: true });
  }

  handleNewGameStart = (symbol) => {
    this.setState({
      winner: 'inconclusive',
      winPositions: [],
      playerSymbol: symbol,
      opponentSymbol: symbol === 'X' ? 'O' : 'X',
      gameBoardTiles: ['?', '?', '?', '?', '?', '?', '?', '?', '?'],
      isPlayerTurn: true,
    });
  }

  async executeMove(index) {
    const options = {
      method: 'POST',
      uri: 'https://localhost:5001/executemove',
      json: true,
      body: {
        move: index,
        azurePlayerSymbol: this.state.playerSymbol,
        humanPlayerSymbol: this.state.opponentSymbol,
        gameBoard: this.state.gameBoardTiles,
      }
    };

    const response = await request(options);
    console.log(response);
    return response;
  }
}
