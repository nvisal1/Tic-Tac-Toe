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
        char winnerMapping = 'T';

        char[] newGameBoard = predictNextMove(messagePayload.azurePlayerSymbol, messagePayload.humanPlayerSymbol, messagePayload.gameBoard);

        int winner = checkForWinner(messagePayload.azurePlayerSymbol, messagePayload.humanPlayerSymbol, newGameBoard, 0);

        if (winner == 1) winnerMapping = messagePayload.azurePlayerSymbol;
        if (winner == -1) winnerMapping = messagePayload.humanPlayerSymbol;

        return new ExecuteMoveResponse()
        {
            move = messagePayload.move += 1,
            azurePlayerSymbol = messagePayload.azurePlayerSymbol,
            humanPlayerSymbol = messagePayload.humanPlayerSymbol,
            winner = winnerMapping,
            winPositions = getTileIndicies(messagePayload.gameBoard, winnerMapping),
            gameBoard = newGameBoard,
        };
    }


  

    
}