using System;

public class Player {

    public char symbol;

    public Player(char symbol)
    {
        this.symbol = symbol;
    }

    public int getOpponentVictoryTile(GameBoard gameBoard, Player opponent, char neutralSymbol)
    {
        var opponentTiles = gameBoard.getTileIndicies(opponent.symbol);

        int[,] winPositions = new int[,] { 
            { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 },
            { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 },
        };

        for (int i = 0; i < winPositions.GetLength(0); i++)
        {
            var counter = 0;
            var emptyTileIndex = -1;
            for (int j = 0; j < 3; j++)
            {
                if (gameBoard.gameBoardTiles[winPositions[i, j]].Equals(opponent.symbol))
                {
                    counter++;
                } else if (gameBoard.gameBoardTiles[winPositions[i, j]].Equals(neutralSymbol)) {
                    emptyTileIndex = winPositions[i, j];
                }

            }
            if (counter == 2 && emptyTileIndex != -1)
            {
                return emptyTileIndex;
            }

        }

        return -1;
    }

    public int findOptimalTile(GameBoard gameBoard, char neutralSymbol, Player opponent)
    {
        int[,] winPositions = new int[,] { 
            { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 },
            { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 },
        };

        var myTiles = gameBoard.getTileIndicies(symbol);
        var emptyTiles = gameBoard.getTileIndicies(neutralSymbol);

        // If the bot does not have any tiles, pick
        // a random starting point.
        if (myTiles.Length == 0)
        {
            Random rnd = new Random();
            return emptyTiles[rnd.Next(0, emptyTiles.Length - 1)];
        }

        // If the bot does have at least one tile
        for (int i = 0; i < winPositions.GetLength(0); i++)
        {
            var opponentCount = 0;
            var botCount = 0;
            var emptyIndex = -1;
            for (int j = 0; j < 3; j++)
            {
                // If the bot owns a tile that is associated with a particular rule,
                // check if it can pursue the path
                if (gameBoard.gameBoardTiles[winPositions[i, j]].Equals(symbol)) 
                {
                    botCount++;
                } 
                else if (gameBoard.gameBoardTiles[winPositions[i, j]].Equals(opponent.symbol))
                {
                    opponentCount++;
                } 
                else
                {
                    emptyIndex = winPositions[i, j];
                }
            }

            if (opponentCount == 0 && botCount > 0) {
                return emptyIndex;
            }
        }

        return emptyTiles[0];

    }

    public GameBoard selectTile(GameBoard gameBoard, int index)
    {
        gameBoard.gameBoardTiles[index] = symbol;
        return gameBoard;
    }
}


// Check if the opponent is about to win
// If so, block the winning tile

// Else, check if the bot has any tiles

// If so, find a winning rule associated 
// with each owned tile and check to
// see if it can be pursued

// Else, pick a random starting point

// If best case if a tie, take next
// available tile



