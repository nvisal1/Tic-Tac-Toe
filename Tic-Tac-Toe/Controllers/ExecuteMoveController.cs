using System;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

public class ExecuteMove : ControllerBase
{
    public ExecuteMove()
    {
    }

    /// <summary>
    /// This endpoint allows a user to submit a move and receive a bot's responding move
    /// </summary>
    [HttpPost("/executemove")]
    [Produces("application/json")]
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
        if (messagePayload.gameBoard.Length != 9)
        {
            return false;
        }

        // The gameBoard must have the correct number of player symbols.
        //
        // The following rules apply when the gameBoard is not empty...
        // The difference in symbol counts should be no greater than 1.
        // There should be less AzurePlayerSymbols on the gameBoard.
        // For example, if the AzurePlayerSymbol is X and there are
        // 3 X's on the gameBoard, there must be 4 O's on the gameBoard.
        GameBoard gameBoard = new GameBoard(messagePayload.gameBoard);
        var emptySymbolLength = gameBoard.getTileIndicies('?');
        if (emptySymbolLength.Length == 9)
        {
            return true;
        }
        var azurePlayerSymbolCount = gameBoard.getTileIndicies(messagePayload.azurePlayerSymbol).Length;
        var humanPlayerSymbolCount = gameBoard.getTileIndicies(messagePayload.humanPlayerSymbol).Length;
        
        if (Math.Abs(azurePlayerSymbolCount - humanPlayerSymbolCount) != 1)
        {
            return false;
        }

        if (azurePlayerSymbolCount >= humanPlayerSymbolCount)
        {
            return false;
        }

        return true;
    }
}