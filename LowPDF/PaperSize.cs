namespace LowPDF;

public class PaperSize
{
    public int WidthMM { get; private set; }
    public int HeightMM { get; private set; }
    public int WidthPoints { get; private set; }
    public int HeightPoints { get; private set; }

    private const double MmToInch = 25.4;
    private const double PointsPerInch = 72.0;

    public PaperSize(int widthMM, int heightMM)
    {
        WidthMM = widthMM;
        HeightMM = heightMM;
        WidthPoints = (int)Math.Round((widthMM / MmToInch) * PointsPerInch);
        HeightPoints = (int)Math.Round((heightMM / MmToInch) * PointsPerInch);
    }

    // Standard paper sizes
    public static PaperSize A4 { get; } = new PaperSize(210, 297);
    public static PaperSize A3 { get; } = new PaperSize(297, 420);
    public static PaperSize A2 { get; } = new PaperSize(420, 594);
    public static PaperSize A1 { get; } = new PaperSize(594, 841);
    public static PaperSize A0 { get; } = new PaperSize(841, 1189);
    public static PaperSize Letter { get; } = new PaperSize(216, 279);
}