using System;
using System.Linq;

public class GameBoard {

    public char[] gameBoardTiles { get; set; }
    
    public GameBoard(char[] gameBoardTiles)
    {
        this.gameBoardTiles = gameBoardTiles;
    }

    /// <summary>
    /// 
    /// GetGameState iterates over the already-defined win positions in order to check
    /// for a win.
    /// 
    /// If there in no winner and the game board does not contain any empty spaces
    /// return "tie". 
    /// 
    /// If there is no winner and the game board does contain empty spaces,
    /// return "inconclusive".
    /// 
    /// </summary>
    /// <returns> string ("tie" || "inconclusive" || "X" || "O") </returns>
    public string GetGameState()
    {
        for (int i = 0; i < Constants.WIN_POSITIONS.GetLength(0); i++)
        {
            // Check if the current win positions only contain symbol one.
            if (
                gameBoardTiles[Constants.WIN_POSITIONS[i, 0]].Equals(Constants.POSSIBLE_SYMBOL_ONE)
                && gameBoardTiles[Constants.WIN_POSITIONS[i, 1]].Equals(Constants.POSSIBLE_SYMBOL_ONE)
                && gameBoardTiles[Constants.WIN_POSITIONS[i, 2]].Equals(Constants.POSSIBLE_SYMBOL_ONE)
            )
            {
                return Constants.POSSIBLE_SYMBOL_ONE.ToString();
            }

            // Check if the current win positions only contain symbol two.
            else if (
                gameBoardTiles[Constants.WIN_POSITIONS[i, 0]].Equals(Constants.POSSIBLE_SYMBOL_TWO)
                && gameBoardTiles[Constants.WIN_POSITIONS[i, 1]].Equals(Constants.POSSIBLE_SYMBOL_TWO)
                && gameBoardTiles[Constants.WIN_POSITIONS[i, 2]].Equals(Constants.POSSIBLE_SYMBOL_TWO)
            )
            {
                return Constants.POSSIBLE_SYMBOL_TWO.ToString();
            }

        }

        if (GetTileIndicies(Constants.NEUTRAL_SYMBOL).Length == 0) {
            return Constants.TIE;
        }

        return Constants.INCONCLUSIVE;
        
    }

    /// <summary>
    /// 
    /// GetVictoryTile searches the game board for a tile
    /// that will lead to an immediate victory for the given
    /// symbol. 
    /// 
    /// If a victory tile exists, return it.
    /// Else, return -1
    /// 
    /// A victory tile is defined as the last tile
    /// needed to complete a single win position.
    /// 
    /// Refer to Constants for more context about win positions
    /// 
    /// Example:
    /// 
    /// X O ?
    /// X ? O
    /// ? ? ?
    /// 
    /// 6 will be return if searching for a victory tile for "X".
    /// -1 will be returned if searching for a victory tile for "O".
    /// 
    /// </summary>
    /// <param name="symbol"> char representing the symbol to search for on the game board </param>
    /// <returns> int (-1 if no victory tile exists) </returns>
    public int GetVictoryTile(char symbol)
    {
        for (int i = 0; i < Constants.WIN_POSITIONS.GetLength(0); i++)
        {
            var counter = 0;
            var emptyTileIndex = -1;
            for (int j = 0; j < 3; j++)
            {
                if (gameBoardTiles[Constants.WIN_POSITIONS[i, j]].Equals(symbol))
                {
                    counter++;
                }
                else if (gameBoardTiles[Constants.WIN_POSITIONS[i, j]].Equals(Constants.NEUTRAL_SYMBOL))
                {
                    emptyTileIndex = Constants.WIN_POSITIONS[i, j];
                }

            }
            if (counter == 2 && emptyTileIndex != -1)
            {
                return emptyTileIndex;
            }

        }

        return -1;
    }

    /// <summary>
    /// 
    /// FindOptimalTile goes through 3 cases to determine
    /// the best tile to select. This function does not
    /// take immediate wins into consideration. GetVictoryTile
    /// should be called before this function for better
    /// bot performance.
    /// 
    /// Case 1. If the player does not own any
    /// tile on the game board, select a random
    /// starting point.
    /// 
    /// Case 2. If the bot has at least one tile
    /// on the game board, iterate over all win positions.
    /// If the player owns a tile for a win position and 
    /// the opponent does not, select an empty index within
    /// that win position.
    /// 
    /// Example:
    /// 
    /// X ? ?
    /// ? O ?
    /// O ? ?
    /// 
    /// 1 will be returned when searching for an optimal tile
    /// for "X". This is because "X" owns a tile within win
    /// position [0, 1, 2] and "O" does not.
    /// 
    /// Case 3. If a tile was not found in the previous cases,
    /// return the next empty tile. Case 3 usually occurs in the
    /// event of a tie.
    /// 
    /// </summary>
    /// <param name="playerSymbol"></param>
    /// <param name="opponentSymbol"></param>
    /// <returns> int </returns>
    public int FindOptimalTile(char playerSymbol, char opponentSymbol)
    {
        var playerTiles = GetTileIndicies(playerSymbol);
        var emptyTiles = GetTileIndicies(Constants.NEUTRAL_SYMBOL);

        // Case 1
        if (playerTiles.Length == 0)
        {
            Random rnd = new Random();
            return emptyTiles[rnd.Next(0, emptyTiles.Length - 1)];
        }

        // Case 2
        for (int i = 0; i < Constants.WIN_POSITIONS.GetLength(0); i++)
        {
            var opponentCount = 0;
            var botCount = 0;
            var emptyIndex = -1;
            for (int j = 0; j < 3; j++)
            {
                if (gameBoardTiles[Constants.WIN_POSITIONS[i, j]].Equals(playerSymbol))
                {
                    botCount++;
                }
                else if (gameBoardTiles[Constants.WIN_POSITIONS[i, j]].Equals(opponentSymbol))
                {
                    opponentCount++;
                }
                else
                {
                    emptyIndex = Constants.WIN_POSITIONS[i, j];
                }
            }

            if (opponentCount == 0 && botCount > 0)
            {
                return emptyIndex;
            }
        }

        // Case 3
        return emptyTiles[0];
    }

    /// <summary>
    /// 
    /// GetTileIndicies return all of the tiles
    /// that contain the given symbol
    /// 
    /// </summary>
    /// <param name="symbol"> char representing the symbol to search for on the game board </param>
    /// <returns></returns>
    public int[] GetTileIndicies(char symbol)
    {
        return gameBoardTiles
                    .Select(( value, index ) => new { value, index })
                    .Where(( tile ) => tile.value.Equals(symbol))
                    .Select(( tile ) => tile.index)
                    .ToArray();
    }

}