
public class ExecuteMoveResponse
{
    public int move { get; set; }

    public char azurePlayerSymbol { get; set; }

    public char humanPlayerSymbol { get; set; }

    public string winner { get; set; }

    public int[] winPositions { get; set; }

    public char[] gameBoard { get; set; }
}
