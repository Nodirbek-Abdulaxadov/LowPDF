public class PdfTextElement : PdfElement
{
    public string Text { get; }
    public int FontSize { get; set; } = 14;
    public string FontName { get; set; } = "Helvetica";

    public PdfTextElement(string text = "", int x = 0, int y = 0, int fontSize = 14)
        : base(x, y, text.Length * fontSize * 0.5, fontSize)
    {
        Text = text;
        FontSize = fontSize;
    }

    public override string Render()
    {
        string escapedText = EscapeText(Text);
        return $"BT /{FontName} {FontSize} Tf {X} {Y} Td ({escapedText}) Tj ET\n";
    }

    private string EscapeText(string text)
    {
        // Escape special characters for PDF compatibility
        return text.Replace("(", "\\(").Replace(")", "\\)");
    }
}