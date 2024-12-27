public abstract class PdfElement
{
    public int X { get; set; } = 0;  // X position (left-to-right)
    public int Y { get; set; } = 0;  // Y position (bottom-to-top)
    public double Width { get; set; } = 0;  // Element width
    public double Height { get; set; } = 0;  // Element height
    public int ObjectNumber { get; set; }  // PDF object number (assigned by PdfDocument)
    public List<Style> GetStyles() => _styles;

    private List<Style> _styles = [];

    protected PdfElement(int x, int y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    // Each element must implement its own PDF rendering logic
    public abstract string Render();

    // Optionally render appearance on a page (like images or forms)
    public virtual string RenderAppearance()
    {
        return string.Empty;
    }

    public void AppendStyle(params Style[] styles)
    {
        foreach(var style in styles)
        {
            _styles.Add(style);
            ApplyStyle(style);
        }
    }

    private void ApplyStyle(Style style)
    {
        if (style is Margin margin)
        {
            ApplyMargin(margin);
        }
    }

    private void ApplyMargin(Margin margin)
    {
        X += margin.Left;
        Y += margin.Top;
        Width -= margin.Left + margin.Right;
        Height -= margin.Top + margin.Bottom;
    }
}
