using System.Linq;

public class GameBoard {

    public char[] gameBoardTiles { get; set; }
    
    public GameBoard(char[] gameBoardTiles)
    {
        this.gameBoardTiles = gameBoardTiles;
    }

    public string getWinner(char playerOneSymbol, char playerTwoSymbol, char neutralSymbol)
    {
        int[,] winPositions = new int[,] { 
            { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 },
            { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 },
        };

        for (int i = 0; i < winPositions.GetLength(0); i++)
        {
            if (
                this.gameBoardTiles[winPositions[i, 0]].Equals(playerOneSymbol)
                && this.gameBoardTiles[winPositions[i, 1]].Equals(playerOneSymbol)
                && this.gameBoardTiles[winPositions[i, 2]].Equals(playerOneSymbol)
            )
            {
                return playerOneSymbol.ToString();
            }
            else if (
                this.gameBoardTiles[winPositions[i, 0]].Equals(playerTwoSymbol)
                && this.gameBoardTiles[winPositions[i, 1]].Equals(playerTwoSymbol)
                && this.gameBoardTiles[winPositions[i, 2]].Equals(playerTwoSymbol)
            )
            {
                return playerTwoSymbol.ToString();
            }

        }

        if (getTileIndicies(neutralSymbol).Length == 0) {
            return "tie";
        }

        return "inconclusive";
        
    }

    public char[] getGameBoardTilesCopy()
    {
        var gameBoardTilesCopy = new char[9];
        this.gameBoardTiles.CopyTo(gameBoardTilesCopy, 0);
        return gameBoardTilesCopy;
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