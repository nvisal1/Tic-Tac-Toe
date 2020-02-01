using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class ExecuteMove : ControllerBase
{
    public ExecuteMove()
    {
    }

    [HttpPost("/do")]
    public ActionResult<ExecuteMoveResponse> executeMove([FromBody] MessagePayload messagePayload)
    {

        char[] newGameBoard = predictNextMove(messagePayload.azurePlayerSymbol, messagePayload.humanPlayerSymbol, messagePayload.gameBoard);

        char winner = ' ';
        winner = checkForWinner(messagePayload.azurePlayerSymbol, newGameBoard) ? messagePayload.azurePlayerSymbol : ' ';
        winner = checkForWinner(messagePayload.humanPlayerSymbol, newGameBoard) ? messagePayload.humanPlayerSymbol : ' ';
        
        return new ExecuteMoveResponse()
        {
            move = messagePayload.move ++,
            azurePlayerSymbol = messagePayload.azurePlayerSymbol,
            humanPlayerSymbol = messagePayload.humanPlayerSymbol,
            winner = winner,
            winPositions = getEmptyTileIndicies(messagePayload.gameBoard),
            gameBoard = newGameBoard,
        };
    }

    private char[] predictNextMove(
        char azurePlayerSymbol,
        char humanPlayerSymbol,
        char[] gameBoard
    )
    {
        // Query for empty tiles in the game board
        int[] emptyTileIndicies = getEmptyTileIndicies(gameBoard);

        int maxScore = 0;
        char[] predictedMove = gameBoard;
    
        // Iterate over all open spaces on the game board
        foreach (int emptyTileIndex in emptyTileIndicies)
        {
            int score = calculatePredictedMoveScore(azurePlayerSymbol, humanPlayerSymbol, gameBoard, true, 0);
            if (score > maxScore)
            {
                maxScore = score;
                char[] newGameBoard = gameBoard;
                newGameBoard[emptyTileIndex] = azurePlayerSymbol;
                predictedMove = newGameBoard;
            }
        }
        
        return predictedMove;
    }

    private int calculatePredictedMoveScore(
        char azurePlayerSymbol,
        char humanPlayerSymbol,
        char[] gameBoard,
        bool isBotTurn,
        int currentScore
    )
    {
        // Get the symbol used by the player with the current turn
        char currentSymbol = isBotTurn ? azurePlayerSymbol : humanPlayerSymbol;

        // Query for empty tiles in the game board
        int[] emptyTileIndicies = getEmptyTileIndicies(gameBoard);
        
        // If there are no empty tiles return 0, indicating a draw
        if (emptyTileIndicies.Length == 0) return 0;

        // Iterate over all open spaces on the game board
        foreach (int emptyTileIndex in emptyTileIndicies)
        {
            // Assign the player's symbol to the specified empty tile
            gameBoard[emptyTileIndex] = currentSymbol;

            // If this move results in a win, return 1
            // If this move results in a loss, return -1
            // If this move results in a draw, continue
            if (checkForWinner(currentSymbol, gameBoard))
            {
                if(isBotTurn) return currentScore ++;
                return currentScore --;
            }
            currentScore = calculatePredictedMoveScore(azurePlayerSymbol, humanPlayerSymbol, gameBoard, !isBotTurn, currentScore);
        }
        return currentScore;
    }

    private bool checkForWinner(char symbol, char[] gameBoard)
    {

        Console.Write(gameBoard);

        int[,] winPositions = new int[,] { 
            { 0, 1, 2 }, { 3, 4, 5 },
            { 6, 7, 8 }, { 0, 3, 6 },
            { 1, 4, 7 }, { 2, 5, 8 },
            { 0, 4, 8 }, { 2, 4, 6 }
        };

        for (int i = 0; i < winPositions.GetLength(0); i++)
        {
            if (
                gameBoard[winPositions[i, 0]].Equals(symbol)
                && gameBoard[winPositions[i, 1]].Equals(symbol)
                && gameBoard[winPositions[i, 2]].Equals(symbol)
            )
            {
                return true;
            }
        }

        return false;
    }

    private int[] getEmptyTileIndicies(char[] gameBoard)
    {
            return gameBoard
                        .Select(( value, index ) => new { value, index })
                        .Where(( tile ) => tile.value.Equals('?'))
                        .Select(( tile ) => tile.index)
                        .ToArray();
    }
}