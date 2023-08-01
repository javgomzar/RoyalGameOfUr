

public static class GameColor {
    public enum Color {Black, White};

    public static Color OppositeColor(Color color) {
        if (color == Color.Black) {
            return Color.White;
        }
        else {
            return Color.Black;
        }
    }

    public static string GetString(Color color) {
        return System.Enum.GetName(typeof(Color), color);
    }
}
