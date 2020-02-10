/// <summary>
/// Defines the response for executemove
/// </summary>
public class ExecuteMoveResponse
{
    /// <summary>
    /// Indicates the position the Azure App Service chooses.
    /// If a tie or winner occurred because of the game state
    /// provided before Azure making a move the move property 
    /// shall be set to null.
    /// </summary>
    public int? move { get; set; }

    /// <summary>
    /// Shall be the same value the client sent
    /// </summary>
    public char azurePlayerSymbol { get; set; }

    /// <summary>
    /// Shall be the same value the client sent
    /// </summary>
    public char humanPlayerSymbol { get; set; }

    /// <summary>
    /// Indicates the gameboard status. There can be a
    /// winner, a tie, or no winner or tie yet (inconclusive) 
    /// and the game can continue.
    /// </summary>
    public string winner { get; set; }

    /// <summary>
    /// An array that lists the zero-based position index
    /// values of the win or null if no winner is present in
    /// the gameBoard array.
    /// </summary>
    public int[] winPositions { get; set; }

    /// <summary>
    /// An array containing all the positions on the board and 
    /// the mark(s) indicating the positions played including 
    /// the mark azure made if Azure made a move.
    /// </summary>
    public char[] gameBoard { get; set; }
}
