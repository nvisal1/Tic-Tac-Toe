public class Player {

    private char symbol;
    private char opponentSymbol;

    public Player(char symbol, char opponentSymbol)
    {
        this.symbol = symbol;
        this.opponentSymbol = opponentSymbol;
    }

    public char[] predictNextMove(GameBoard gameBoard)
    {
        // Query for empty tiles in the game board
        int[] emptyTileIndicies = gameBoard.getTileIndicies('?');

        var maxScore = -2;
        var gameBoardTiles = gameBoard.getGameBoardTilesCopy();
        var predictedGameBoardTiles = gameBoard.getGameBoardTilesCopy();

        // Iterate over all open spaces on the game board
        foreach (int emptyTileIndex in emptyTileIndicies)
        {
            var predictedMoveTiles = gameBoard.getGameBoardTilesCopy();
            predictedMoveTiles[emptyTileIndex] = this.symbol;
            int score = calculatePredictedMoveScore(predictedMoveTiles, true);
            
            if (score > maxScore)
            {
                maxScore = score;
                gameBoardTiles.CopyTo(predictedGameBoardTiles, 0);
                predictedGameBoardTiles[emptyTileIndex] = this.symbol;
            }
        }
        
        return predictedGameBoardTiles;
    }

    private int calculatePredictedMoveScore(GameBoard gameBoard, bool isTurn)
    {
        // Get the symbol used by the player with the current turn
        char currentSymbol = isTurn ? this.symbol : this.opponentSymbol;

        var gameBoardTiles = gameBoard.getGameBoardTilesCopy();

        // Query for empty tiles in the game board
        int[] emptyTileIndicies = gameBoard.getTileIndicies('?');

        // If this move results in a win, return 1
        // If this move results in a loss, return -1
        // If this move results in a draw, continue
        int winner = gameBoard.hasWinner(this.symbol, this.opponentSymbol, '?');
        if (winner != 0 || (winner == 0 && emptyTileIndicies.Length == 0)) return winner;

        if (isTurn)
        {
            var bestScore = 0;
            foreach (int emptyTileIndex in emptyTileIndicies)
            {
                // Assign the player's symbol to the specified empty tile
                gameBoardTiles[emptyTileIndex] = currentSymbol;
                var tempGameBoard = new char[9];
                gameBoard.CopyTo(tempGameBoard, 0);
                currentScore = calculatePredictedMoveScore(tempGameBoard, false);
                gameBoard[emptyTileIndex] = '?';
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                }
            }
            return bestScore;
        } else {
            var bestScore = 100;
            foreach (int emptyTileIndex in emptyTileIndicies)
            {
                // Assign the player's symbol to the specified empty tile
                gameBoard[emptyTileIndex] = currentSymbol;
                var tempGameBoard = new char[9];
                gameBoard.CopyTo(tempGameBoard, 0);
                currentScore = calculatePredictedMoveScore(tempGameBoard, true);
                gameBoard[emptyTileIndex] = '?';
                if (currentScore < bestScore)
                {
                    bestScore = currentScore;
                }
   
            }
            return bestScore;
        }  
    }

}