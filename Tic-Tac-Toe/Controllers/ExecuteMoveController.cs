using System;
using Microsoft.AspNetCore.Http;
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
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExecuteMoveResponse), StatusCodes.Status200OK)]
    public ActionResult<ExecuteMoveResponse> executeMove([FromBody] MessagePayload messagePayload)
    {
        // Check the format of the given message payload.
        // Return status 400 to the client if the format is incorrect.
        if (!messagePayload.IsExpectedFormat())
        {
            return BadRequest();
        }

        // Check the initial state of the given game board.
        //
        // If the given game board contains a conclusive state,
        // return the result to the client without taking another turn.
        GameBoard gameBoard = new GameBoard(messagePayload.gameBoard);
        string earlyGameState = gameBoard.GetGameState();
        if (earlyGameState != Constants.INCONCLUSIVE)
        {
            return GenerateExecuteMoveResponse(earlyGameState, messagePayload.move, gameBoard, true, messagePayload.azurePlayerSymbol, messagePayload.humanPlayerSymbol);
        }
     
        // If the given game board has an inconclusive state, tell Azure to take its turn.
        Player azure = new Player(messagePayload.azurePlayerSymbol, messagePayload.humanPlayerSymbol, gameBoard);
        GameBoard newGameBoard = azure.ExecuteMove();

        // Check the state of the game board after Azure's turn.
        // Use the new game state to generate and return the result to the client.
        string gameState = newGameBoard.GetGameState();
        return GenerateExecuteMoveResponse(gameState, azure.lastMove, newGameBoard, false, messagePayload.azurePlayerSymbol, messagePayload.humanPlayerSymbol);
    }


    /// <summary>
    /// GenerateExecuteMoveResponse creates a new ExecuteMoveResponse.
    /// 
    /// If earlyWin is set to true, move will be set to null.
    ///
    /// If gameState contains "tie" or "inconclusive", winPositions will be set to null.
    /// 
    /// If gameState contains a winning symbol, this function will find the game board indicies that
    /// contain the winning symbol.
    /// </summary>
    /// <param name="gameState">string value containing "X" || "O" || "tie" || "inconclusive" </param>
    /// <param name="move"> int value representing the index of the most recently changed game board tile </param>
    /// <param name="gameBoard"> GameBoard representing the current game board </param>
    /// <param name="earlyWin"> boolean value indicating if the given game board contained a conconclusive state before Azure's next turn </param>
    /// <param name="azurePlayerSymbol"> char representing Azure's symbol </param>
    /// <param name="humanPlayerSymbol"> char representing the human player's symbol </param>
    /// <returns> ExecuteMoveResponse </returns>
    private ExecuteMoveResponse GenerateExecuteMoveResponse(string gameState, int? move, GameBoard gameBoard, bool earlyWin, char azurePlayerSymbol, char humanPlayerSymbol)
    {
        return new ExecuteMoveResponse()
        {
            move = earlyWin ? null : move,
            winner = gameState,
            winPositions = gameState.Equals(Constants.TIE) || gameState.Equals(Constants.INCONCLUSIVE) ? null : gameBoard.getWinPositions(gameState, azurePlayerSymbol, humanPlayerSymbol),
            gameBoard = gameBoard.gameBoardTiles,
            azurePlayerSymbol = azurePlayerSymbol,
            humanPlayerSymbol = humanPlayerSymbol,
        };
    }
}