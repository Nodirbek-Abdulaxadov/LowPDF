public class PdfDocument
{
    private StringBuilder _content;
    private List<PdfElement> _elements;
    private int _objectCount;
    private StringBuilder _xref;
    private int _xrefStart;
    private int _currentOffset;
    private PaperSize _paperSize;

    public PdfDocument(PaperSize? paperSize = null)
    {
        _content = new StringBuilder();
        _elements = new List<PdfElement>();
        _xref = new StringBuilder();
        _objectCount = 1;

        _content.AppendLine("%PDF-1.7");
        _xref.AppendLine("xref\n0 1\n0000000000 65535 f ");
        _currentOffset = _content.Length;
        _paperSize = paperSize ?? PaperSize.A4;
    }

    public void AddElement(PdfElement element)
    {
        _elements.Add(element);
    }

    public void Save(string path)
    {
        // Catalog and Pages
        _content.AppendLine("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj");
        AddToXref();

        // Pages Object
        _content.AppendLine($"2 0 obj\n<< /Type /Pages /Count 1 /Kids [3 0 R] >>\nendobj");
        AddToXref();

        // Page Object
        _content.AppendLine($"3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {_paperSize.WidthPoints} {_paperSize.HeightPoints}] /Contents 4 0 R >>\nendobj");
        AddToXref();

        // Render Page Contents
        StringBuilder pageContent = new StringBuilder();
        pageContent.AppendLine("4 0 obj");
        pageContent.AppendLine("<< /Length 0 >>");
        pageContent.AppendLine("stream");

        foreach (var element in _elements)
        {
            pageContent.AppendLine(element.Render());
        }

        pageContent.AppendLine("endstream\nendobj");
        _content.Append(pageContent);
        AddToXref();

        // Finalize PDF
        _xrefStart = _currentOffset;
        _xref.Insert(5, $" {_objectCount}\n");

        var trailer = new StringBuilder();
        trailer.AppendLine("trailer");
        trailer.AppendLine($"<< /Size {_objectCount + 1} /Root 1 0 R >>");
        trailer.AppendLine("startxref");
        trailer.AppendLine(_xrefStart.ToString());
        trailer.AppendLine("%%EOF");

        _content.Append(_xref);
        _content.Append(trailer);

        File.WriteAllText(path, _content.ToString(), Encoding.ASCII);
    }

    private void AddToXref()
    {
        _xref.AppendLine($"{_currentOffset:D10} 00000 n ");
        _currentOffset = _content.Length;
        _objectCount++;
    }
}