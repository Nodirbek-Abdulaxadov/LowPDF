public class Margin : Style
{
    public int Top { get; set; }
    public int Right { get; set; }
    public int Bottom { get; set; }
    public int Left { get; set; }

    public Margin(int all)
    {
        Top = Right = Bottom = Left = all;
    }

    public Margin(int vertical, int horizontal)
    {
        Top = Bottom = vertical;
        Right = Left = horizontal;
    }

    public Margin(int top, int right, int bottom, int left)
    {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
    }
}