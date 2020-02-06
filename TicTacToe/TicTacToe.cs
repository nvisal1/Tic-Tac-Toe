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
    
        var predictNextMoveResponse = currentPlayer.predictNextMove(opponentPlayer, gameBoard, neutralSymbol);
        var newGameBoard = predictNextMoveResponse.gameBoard;
        var newMove = predictNextMoveResponse.move;

        var winner = newGameBoard.getWinner(playerOne.symbol, playerTwo.symbol, neutralSymbol);

        
        int[] winPositions = new int[9];
        bool hasWinner = false;
        if (winner.Equals(playerOne.symbol))
        {
            winPositions = newGameBoard.getTileIndicies(playerOne.symbol);
            hasWinner = true;
        }
        else if (winner.Equals(playerTwo.symbol))
        {
            winPositions = newGameBoard.getTileIndicies(playerTwo.symbol);
            hasWinner = true;
        }

        return new ExecuteMoveResponse()
        {
            move = predictNextMoveResponse.move,
            winner = winner,
            winPositions = hasWinner ? winPositions : null,
            gameBoard = newGameBoard.gameBoardTiles,
        };
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


