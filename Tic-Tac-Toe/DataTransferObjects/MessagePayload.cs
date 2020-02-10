
using System;
/// <summary>
/// Defines the expected payload for executemove
/// </summary>
public class MessagePayload
{
    /// <summary>
    /// Indicates the position the human chooses.
    /// </summary>
    public int move { get; set; }

    /// <summary>
    /// Is either the letter X or the letter O indicating the
    /// symbol used by Azure.
    /// Must be the opposite of humanPlayerSymbol
    /// </summary>
    public char azurePlayerSymbol { get; set; }

    /// <summary>
    /// Is either the letter X or the letter O indicating the
    /// symbol used by the human.
    /// Must be the opposite of azurePlayerSymbol
    /// </summary>
    public char humanPlayerSymbol { get; set; }

    /// <summary>
    /// ? = A space on the game board
    /// X = Player letter X has a piece on that game board location
    /// O = Player letter O has a piece on that game board location
    /// </summary>
    public char[] gameBoard { get; set; }

    public void IsExpectedFormat()
    {
        // move should be an intger between 0 and 8
        if (move < 0 || move > 8)
        {
            throw new ArgumentException("move should  be an intger between 0 and 8");
        }

        // azurePlayerSymbol must be either 'O' or 'X' 
        if (!(azurePlayerSymbol.Equals(Constants.POSSIBLE_SYMBOL_ONE)) && !(azurePlayerSymbol.Equals(Constants.POSSIBLE_SYMBOL_TWO)))
        {
            throw new ArgumentException("azurePlayerSymbol must be either 'O' or 'X'");
        }

        // humanPlayerSymbol must be either 'O' or 'X' 
        if (!(humanPlayerSymbol.Equals(Constants.POSSIBLE_SYMBOL_ONE)) && !(humanPlayerSymbol.Equals(Constants.POSSIBLE_SYMBOL_TWO)))
        {
            throw new ArgumentException("humanPlayerSymbol must be either 'O' or 'X'");
        }

        // azurePlayerSymbol and humanPlayerSymbol must be opposites
        if (azurePlayerSymbol.Equals(humanPlayerSymbol))
        {
            throw new ArgumentException("azurePlayerSymbol and humanPlayerSymbol must be opposites");
        }

        // gameBoard can only contain 'X', 'O', or '?'
        if (Array.Exists(gameBoard, tile => tile != Constants.POSSIBLE_SYMBOL_ONE && tile != Constants.POSSIBLE_SYMBOL_TWO && tile != Constants.NEUTRAL_SYMBOL))
        {
            throw new ArgumentException("gameBoard can only contain 'X', 'O', or '?'");
        }

        // gameboard must have a length of 9 (0 - 8)
        if (gameBoard.Length != Constants.GAME_BOARD_LENGTH)
        {
            throw new ArgumentException("gameboard must have a length of 9 (0 - 8)");
        }

        // The gameBoard must have the correct number of player symbols.
        //
        // The following rules apply when the gameBoard is not empty...
        // The difference in symbol counts should be no greater than 1.
        // There should be less AzurePlayerSymbols on the gameBoard.
        // For example, if the AzurePlayerSymbol is X and there are
        // 3 X's on the gameBoard, there must be 4 O's on the gameBoard.
        GameBoard tempGameBoard = new GameBoard(gameBoard);
        var emptySymbolLength = tempGameBoard.GetTileIndicies(Constants.NEUTRAL_SYMBOL);
        if (emptySymbolLength.Length == Constants.GAME_BOARD_LENGTH)
        {
            return;
        }
        var azurePlayerSymbolCount = tempGameBoard.GetTileIndicies(azurePlayerSymbol).Length;
        var humanPlayerSymbolCount = tempGameBoard.GetTileIndicies(humanPlayerSymbol).Length;

        if (Math.Abs(azurePlayerSymbolCount - humanPlayerSymbolCount) != 1)
        {
            throw new ArgumentException("The difference in symbol counts should be no greater than 1");
        }

        if (azurePlayerSymbolCount >= humanPlayerSymbolCount)
        {
            throw new ArgumentException("There should be less AzurePlayerSymbols than humanPlayerSymbols on the gameBoard");
        }
    }
}
