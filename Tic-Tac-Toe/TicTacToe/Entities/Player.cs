using System;

public class Player {

    private char symbol;
    private char opponentSymbol;
    public int lastMove = -1;
    private GameBoard gameBoard;

    public Player(char symbol, char opponentSymbol, GameBoard gameBoard)
    {
        this.symbol = symbol;
        this.opponentSymbol = opponentSymbol;
        this.gameBoard = gameBoard;
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
    public GameBoard ExecuteMove()
    {
        SelectVictoryTile(symbol);
        if (lastMove != -1)
        {
            return gameBoard;
        }

        SelectVictoryTile(opponentSymbol);
        if (lastMove != -1)
        {
            return gameBoard;
        }

        SelectOptimalTile();
        return gameBoard;
    }

    private void SelectVictoryTile(char symbol)
    {
        int victoryIndex = gameBoard.GetVictoryTile(symbol);
        if (victoryIndex != -1)
        {
            SelectTile(victoryIndex);
        }
    }

    private void SelectOptimalTile()
    {
        int optimalTileIndex = gameBoard.FindOptimalTile(symbol, opponentSymbol);
        SelectTile(optimalTileIndex);
    }

    public void SelectTile(int index)
    {
        gameBoard.gameBoardTiles[index] = symbol;
        lastMove = index;
    }
}


