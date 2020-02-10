public class TicTacToe
{
    private Player playerOne;
    private Player playerTwo;
    private GameBoard gameBoard;

    private char neutralSymbol;
    public TicTacToe(char neutralSymbol, char playerOneSymbol, char playerTwoSymbol, char[] gameBoardTiles)
    {
        this.neutralSymbol = neutralSymbol;
        this.playerOne = new Player(playerOneSymbol);
        this.playerTwo = new Player(playerTwoSymbol);
        this.gameBoard = new GameBoard(gameBoardTiles);
    }

    public ExecuteMoveResponse playNextTurn(int move)
    {
        var playerRoles = getPlayerRoles(move);
        var currentPlayer = playerRoles.currentPlayer;
        var opponentPlayer = playerRoles.opponentPlayer;

        var earlyWinner = gameBoard.getWinner(playerOne.symbol, playerTwo.symbol, neutralSymbol);

        if (!(earlyWinner.Equals("inconclusive")))
        {
            int[] earlyWinPositions = new int[9];
            if (earlyWinner.Equals(playerOne.symbol.ToString()))
            {
                earlyWinPositions = gameBoard.getTileIndicies(playerOne.symbol);
            }
            else if (earlyWinner.Equals(playerTwo.symbol.ToString()))
            {
                earlyWinPositions = gameBoard.getTileIndicies(playerTwo.symbol);
            }

            return new ExecuteMoveResponse()
            {
                move = null,
                winner = earlyWinner,
                winPositions = earlyWinner.Equals("tie") ? null : earlyWinPositions,
                gameBoard = gameBoard.gameBoardTiles,
            };
        }

    
        // var predictNextMoveResponse = currentPlayer.predictNextMove(opponentPlayer, gameBoard, neutralSymbol);
        var blockingMoveIndex = currentPlayer.getOpponentVictoryTile(gameBoard, opponentPlayer, neutralSymbol);
        if (blockingMoveIndex != -1)
        {
            var newGameBoard = currentPlayer.selectTile(gameBoard, blockingMoveIndex);
            var winner = newGameBoard.getWinner(playerOne.symbol, playerTwo.symbol, neutralSymbol);

            int[] winPositions = new int[9];
            bool hasWinner = false;
            if (winner.Equals(playerOne.symbol.ToString()))
            {
                winPositions = newGameBoard.getTileIndicies(playerOne.symbol);
                hasWinner = true;
            }
            else if (winner.Equals(playerTwo.symbol.ToString()))
            {
                winPositions = newGameBoard.getTileIndicies(playerTwo.symbol);
                hasWinner = true;
            }

            return new ExecuteMoveResponse()
            {
                move = blockingMoveIndex,
                winner = winner,
                winPositions = hasWinner ? winPositions : null,
                gameBoard = newGameBoard.gameBoardTiles,
            };
        }
        else {
            var optimalTileIndex = currentPlayer.findOptimalTile(gameBoard, neutralSymbol, opponentPlayer);
            var newGameBoard = currentPlayer.selectTile(gameBoard, optimalTileIndex);

            var winner = newGameBoard.getWinner(playerOne.symbol, playerTwo.symbol, neutralSymbol);

            
            int[] winPositions = new int[9];
            bool hasWinner = false;
            if (winner.Equals(playerOne.symbol.ToString()))
            {
                winPositions = newGameBoard.getTileIndicies(playerOne.symbol);
                hasWinner = true;
            }
            else if (winner.Equals(playerTwo.symbol.ToString()))
            {
                winPositions = newGameBoard.getTileIndicies(playerTwo.symbol);
                hasWinner = true;
            }

            return new ExecuteMoveResponse()
            {
                move = optimalTileIndex,
                winner = winner,
                winPositions = hasWinner ? winPositions : null,
                gameBoard = newGameBoard.gameBoardTiles,
            };
        }
      
    }

    private PlayerRoles getPlayerRoles(int move)
    {
        var opponentSymbol = gameBoard.gameBoardTiles[move];

        if (opponentSymbol.Equals(playerOne.symbol))
        {
            return new PlayerRoles() { currentPlayer = playerTwo, opponentPlayer = playerOne };
        }

        return new PlayerRoles() { currentPlayer = playerOne, opponentPlayer = playerTwo };
    }

}


