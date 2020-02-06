public class Player {

    public char symbol;

    public Player(char symbol)
    {
        this.symbol = symbol;
    }

    public PredictNextMoveResponse predictNextMove(Player opponent, GameBoard gameBoard, char neutralSymbol)
    {
        // Query for empty tiles in the game board
        int[] emptyTileIndicies = gameBoard.getTileIndicies(neutralSymbol);

        var maxScore = -2;
        var gameBoardTiles = gameBoard.gameBoardTiles;
        var predictedGameBoardTiles = gameBoard.getGameBoardTilesCopy();
        int move = 0;

        // Iterate over all open spaces on the game board
        foreach (int emptyTileIndex in emptyTileIndicies)
        {
            var tempTiles = gameBoard.getGameBoardTilesCopy();
            var tempGameBoard = new GameBoard(tempTiles);
            tempGameBoard.gameBoardTiles[emptyTileIndex] = this.symbol;
            int score = calculatePredictedMoveScore(tempGameBoard, opponent, neutralSymbol, true);
            
            if (score > maxScore)
            {
                maxScore = score;
                gameBoardTiles.CopyTo(predictedGameBoardTiles, 0);
                predictedGameBoardTiles[emptyTileIndex] = this.symbol;
                move = emptyTileIndex;
            }
        }
        
        var predictedGameBoard = new GameBoard(predictedGameBoardTiles);
        return new PredictNextMoveResponse() { move = move, gameBoard = predictedGameBoard };
    }

    private int calculatePredictedMoveScore(GameBoard gameBoard, Player opponent, char neutralSymbol, bool isTurn)
    {
        // Get the symbol used by the player with the current turn
        char currentSymbol = isTurn ? symbol : opponent.symbol;

        // Query for empty tiles in the game board
        int[] emptyTileIndicies = gameBoard.getTileIndicies(neutralSymbol);

        // If this move results in a win, return 1
        // If this move results in a loss, return -1
        // If this move results in a draw, continue
        string winner = gameBoard.getWinner(symbol, opponent.symbol, neutralSymbol);
        if (winner.Equals(symbol.ToString())) return 1;
        else if (winner.Equals(opponent.symbol.ToString()) || winner.Equals("tie")) return 0;

        if (isTurn)
        {
            var bestScore = 0;
            foreach (int emptyTileIndex in emptyTileIndicies)
            {
                // Assign the player's symbol to the specified empty tile
                var tempGameBoardTiles = gameBoard.getGameBoardTilesCopy();
                var tempGameBoard = new GameBoard(tempGameBoardTiles);
                tempGameBoard.gameBoardTiles[emptyTileIndex] = currentSymbol;
                int currentScore = calculatePredictedMoveScore(tempGameBoard, opponent, neutralSymbol, false);
                tempGameBoard.gameBoardTiles[emptyTileIndex] = neutralSymbol;
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
                var tempGameBoardTiles = gameBoard.getGameBoardTilesCopy();
                var tempGameBoard = new GameBoard(tempGameBoardTiles);
                tempGameBoard.gameBoardTiles[emptyTileIndex] = currentSymbol;
                int currentScore = calculatePredictedMoveScore(tempGameBoard, opponent, neutralSymbol, true);
                tempGameBoard.gameBoardTiles[emptyTileIndex] = neutralSymbol;
                if (currentScore < bestScore)
                {
                    bestScore = currentScore;
                }
   
            }
            return bestScore;
        }  
    }

}