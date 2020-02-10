
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
}
