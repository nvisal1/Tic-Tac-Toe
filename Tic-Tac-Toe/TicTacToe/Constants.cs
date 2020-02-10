public static class Constants
{
    public const string TIE = "tie";
    public const string INCONCLUSIVE = "inconclusive";
    public static readonly int[,] WIN_POSITIONS = {
            { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 },
            { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 },
        };
    public const char POSSIBLE_SYMBOL_ONE = 'X';
    public const char POSSIBLE_SYMBOL_TWO = 'O';
    public const char NEUTRAL_SYMBOL = '?';
    public const int GAME_BOARD_LENGTH = 9;
}


