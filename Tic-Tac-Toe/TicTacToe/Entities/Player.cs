using System;

public class Player {

    private readonly char symbol;
    private readonly char opponentSymbol;
    private readonly GameBoard gameBoard;

    // lastMove is the index of the tile that the player
    // changed during the current turn. A value of -1
    // indicates that the player has not yet
    // selected a tile in this turn. 
    //
    // This is mainly used to avoid diffing the
    // game boards to find the new move index
    // after each turn.
    public int lastMove = -1;

    public Player(char symbol, char opponentSymbol, GameBoard gameBoard)
    {
        this.symbol = symbol;
        this.opponentSymbol = opponentSymbol;
        this.gameBoard = gameBoard;
    }


    /// <summary>
    /// 
    /// ExecuteMove goes through 3 cases to determine the next move
    /// and returns a corresponding GameBoard.
    /// 
    /// Case 1. Check for a tile that leads
    /// to an immediate victory. If one exists,
    /// select it.
    /// 
    /// Case 2. Check for a tile that leads
    /// to an opponent victory next turn.
    /// If one exists, block it.
    /// 
    /// Case 3. Determine the best tile
    /// and select it.
    /// 
    /// </summary>
    /// <returns> GameBoard </returns>
    public GameBoard ExecuteMove()
    {
        // Case 1
        SelectVictoryTile(symbol);
        if (lastMove != -1)
        {
            return gameBoard;
        }

        // Case 2
        SelectVictoryTile(opponentSymbol);
        if (lastMove != -1)
        {
            return gameBoard;
        }

        // Case 3
        SelectOptimalTile();
        return gameBoard;
    }

    /// <summary>
    /// 
    /// SelectVictoryTile searches for a tile that
    /// leads to an immediate victory. If one exists,
    /// it is marked with the player's symbol.
    /// 
    /// Refer to GameBoard GetVictoryTile for more context.
    /// 
    /// </summary>
    /// <param name="symbol"> char representing the symbol to search for on the game board </param>
    private void SelectVictoryTile(char symbol)
    {
        int victoryIndex = gameBoard.GetVictoryTile(symbol);
        if (victoryIndex != -1)
        {
            SelectTile(victoryIndex);
        }
    }
    
    /// <summary>
    /// 
    /// SelectOptimalTile searches for the best
    /// possible tile to select and marks it
    /// with the player's symbol.
    /// 
    /// This function does not take immediate wins into
    /// consideration.
    /// 
    /// Refer to GameBoard FindOptimalTile for more context.
    /// 
    /// </summary>
    private void SelectOptimalTile()
    {
        int optimalTileIndex = gameBoard.FindOptimalTile(symbol, opponentSymbol);
        SelectTile(optimalTileIndex);
    }

    /// <summary>
    /// 
    /// SelectTile marks the given tile of the game board
    /// with the player's symbol.
    /// 
    /// Side Effect: This function will also set the
    /// lastMove variable to the given index.
    /// 
    /// </summary>
    /// <param name="index"> int value representing the index on the game board to mark with the player's symbol </param>
    public void SelectTile(int index)
    {
        gameBoard.gameBoardTiles[index] = symbol;
        lastMove = index;
    }
}