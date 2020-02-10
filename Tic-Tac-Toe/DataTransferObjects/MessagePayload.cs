
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

    public bool IsExpectedFormat()
    {
        // move should  be an intger between 0 and 8
        if (move < 0 || move > 8)
        {
            return false;
        }

        // azurePlayerSymbol must be either 'O' or 'X' 
        if (!(azurePlayerSymbol.Equals(Constants.POSSIBLE_SYMBOL_ONE)) && !(azurePlayerSymbol.Equals(Constants.POSSIBLE_SYMBOL_TWO)))
        {
            return false;
        }

        // humanPlayerSymbol must be either 'O' or 'X' 
        if (!(humanPlayerSymbol.Equals(Constants.POSSIBLE_SYMBOL_ONE)) && !(humanPlayerSymbol.Equals(Constants.POSSIBLE_SYMBOL_TWO)))
        {
            return false;
        }

        // azurePlayerSymbol and humanPlayerSymbol must be opposites
        if (azurePlayerSymbol.Equals(humanPlayerSymbol))
        {
            return false;
        }

        // gameBoard can only contain 'X', 'O', or '?'
        if (Array.Exists(gameBoard, tile => tile != Constants.POSSIBLE_SYMBOL_ONE && tile != Constants.POSSIBLE_SYMBOL_TWO && tile != Constants.NEUTRAL_SYMBOL))
        {
            return false;
        }

        // gameboard must have a length of 9 (0 - 8)
        if (gameBoard.Length != Constants.GAME_BOARD_LENGTH)
        {
            return false;
        }

        // The gameBoard must have the correct number of player symbols.
        //
        // The following rules apply when the gameBoard is not empty...
        // The difference in symbol counts should be no greater than 1.
        // There should be less AzurePlayerSymbols on the gameBoard.
        // For example, if the AzurePlayerSymbol is X and there are
        // 3 X's on the gameBoard, there must be 4 O's on the gameBoard.
        GameBoard tempGameBoard = new GameBoard(gameBoard);
        var emptySymbolLength = tempGameBoard.getTileIndicies(Constants.NEUTRAL_SYMBOL);
        if (emptySymbolLength.Length == Constants.GAME_BOARD_LENGTH)
        {
            return true;
        }
        var azurePlayerSymbolCount = tempGameBoard.getTileIndicies(azurePlayerSymbol).Length;
        var humanPlayerSymbolCount = tempGameBoard.getTileIndicies(humanPlayerSymbol).Length;

        if (Math.Abs(azurePlayerSymbolCount - humanPlayerSymbolCount) != 1)
        {
            return false;
        }

        if (azurePlayerSymbolCount >= humanPlayerSymbolCount)
        {
            return false;
        }

        return true;
    }
}
