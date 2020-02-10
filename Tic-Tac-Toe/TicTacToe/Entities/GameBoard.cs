using System;
using System.Linq;

public class GameBoard {

    public char[] gameBoardTiles { get; set; }
    
    public GameBoard(char[] gameBoardTiles)
    {
        this.gameBoardTiles = gameBoardTiles;
    }

    public string GetGameState()
    {
        for (int i = 0; i < Constants.WIN_POSITIONS.GetLength(0); i++)
        {
            if (
                this.gameBoardTiles[Constants.WIN_POSITIONS[i, 0]].Equals(Constants.POSSIBLE_SYMBOL_ONE)
                && this.gameBoardTiles[Constants.WIN_POSITIONS[i, 1]].Equals(Constants.POSSIBLE_SYMBOL_ONE)
                && this.gameBoardTiles[Constants.WIN_POSITIONS[i, 2]].Equals(Constants.POSSIBLE_SYMBOL_ONE)
            )
            {
                return Constants.POSSIBLE_SYMBOL_ONE.ToString();
            }
            else if (
                this.gameBoardTiles[Constants.WIN_POSITIONS[i, 0]].Equals(Constants.POSSIBLE_SYMBOL_TWO)
                && this.gameBoardTiles[Constants.WIN_POSITIONS[i, 1]].Equals(Constants.POSSIBLE_SYMBOL_TWO)
                && this.gameBoardTiles[Constants.WIN_POSITIONS[i, 2]].Equals(Constants.POSSIBLE_SYMBOL_TWO)
            )
            {
                return Constants.POSSIBLE_SYMBOL_TWO.ToString();
            }

        }

        if (getTileIndicies(Constants.NEUTRAL_SYMBOL).Length == 0) {
            return Constants.TIE;
        }

        return Constants.INCONCLUSIVE;
        
    }

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

    public int FindOptimalTile(char playerSymbol, char opponentSymbol)
    {
        var playerTiles = getTileIndicies(playerSymbol);
        var emptyTiles = getTileIndicies(Constants.NEUTRAL_SYMBOL);

        // If the bot does not have any tiles, pick
        // a random starting point.
        if (playerTiles.Length == 0)
        {
            Random rnd = new Random();
            return emptyTiles[rnd.Next(0, emptyTiles.Length - 1)];
        }

        // If the bot does have at least one tile
        for (int i = 0; i < Constants.WIN_POSITIONS.GetLength(0); i++)
        {
            var opponentCount = 0;
            var botCount = 0;
            var emptyIndex = -1;
            for (int j = 0; j < 3; j++)
            {
                // If the bot owns a tile that is associated with a particular rule,
                // check if it can pursue the path
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

        return emptyTiles[0];
    }

    public int[] getWinPositions(string gameState, char symbolOne, char symbolTwo)
    {
        int[] winPositions = new int[9];
        if (gameState.Equals(symbolOne.ToString()))
        {
            winPositions = getTileIndicies(symbolOne);
        }
        else if (gameState.Equals(symbolTwo.ToString()))
        {
            winPositions = getTileIndicies(symbolTwo);
        }

        return winPositions;
    }

    public int[] getTileIndicies(char symbol)
    {
        return this.gameBoardTiles
                    .Select(( value, index ) => new { value, index })
                    .Where(( tile ) => tile.value.Equals(symbol))
                    .Select(( tile ) => tile.index)
                    .ToArray();
    }

}