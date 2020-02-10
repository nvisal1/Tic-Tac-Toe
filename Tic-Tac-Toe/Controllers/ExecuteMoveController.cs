using System;
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
        if (!messagePayload.IsExpectedFormat())
        {
            return BadRequest();
        }

        GameBoard gameBoard = new GameBoard(messagePayload.gameBoard);
        string earlyGameState = gameBoard.GetGameState();
        if (earlyGameState != Constants.INCONCLUSIVE)
        {
            return GenerateExecuteMoveResponse(earlyGameState, messagePayload.move, gameBoard, true, messagePayload.azurePlayerSymbol, messagePayload.humanPlayerSymbol);
        }
     
        Player azure = new Player(messagePayload.azurePlayerSymbol, messagePayload.humanPlayerSymbol, gameBoard);
        GameBoard newGameBoard = azure.ExecuteMove();

        string gameState = newGameBoard.GetGameState();
        return GenerateExecuteMoveResponse(gameState, azure.lastMove, newGameBoard, false, messagePayload.azurePlayerSymbol, messagePayload.humanPlayerSymbol);
    }

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