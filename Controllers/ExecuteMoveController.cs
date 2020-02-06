using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class ExecuteMove : ControllerBase
{
    public ExecuteMove()
    {
    }

    [HttpPost("/executemove")]
    public ActionResult<ExecuteMoveResponse> executeMove([FromBody] MessagePayload messagePayload)
    {
        if (!isExpectedPayload(messagePayload))
        {
            return BadRequest();
        }

        char neutralSymbol = '?';
        char azurePlayerSymbol = messagePayload.azurePlayerSymbol;
        char humanPlayerSymbol = messagePayload.humanPlayerSymbol;
        char[] gameBoardTiles = messagePayload.gameBoard;
        int move = messagePayload.move;

        var game = new TicTacToe(neutralSymbol, azurePlayerSymbol, humanPlayerSymbol, gameBoardTiles);

        

        var executeMoveResponse = game.playNextTurn(move);

        executeMoveResponse.azurePlayerSymbol = azurePlayerSymbol;
        executeMoveResponse.humanPlayerSymbol = humanPlayerSymbol;

        return executeMoveResponse;
    }

    private bool isExpectedPayload(MessagePayload messagePayload)
    {
            // move should  be an intger between 0 and 8
            if (messagePayload.move < 0 || messagePayload.move > 8)
            {
                return false;
            }

            // azurePlayerSymbol must be either 'O' or 'X' 
            if (!(messagePayload.azurePlayerSymbol.Equals('X')) && !(messagePayload.azurePlayerSymbol.Equals('O')))
            {
                return false;
            }

            // humanPlayerSymbol must be either 'O' or 'X' 
            if (!(messagePayload.humanPlayerSymbol.Equals('X')) && !(messagePayload.humanPlayerSymbol.Equals('O')))
            {
                return false;
            }

            // azurePlayerSymbol and humanPlayerSymbol must be opposites
            if (messagePayload.azurePlayerSymbol.Equals(messagePayload.humanPlayerSymbol))
            {
                return false;
            }

            // gameBoard can only contain 'X', 'O', or '?'
            if (Array.Exists(messagePayload.gameBoard, element => element != 'X' && element != 'O' && element != '?'))
            {
                return false;
            }

            // gameboard must have a length of 9 (0 - 8)
            if (messagePayload.gameBoard.Length > 9)
            {
                return false;
            }
        
        return true;
    }
}