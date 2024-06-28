namespace LowPDF;

public class PdfDocument
    {
        private StringBuilder _content;
        private int _objectCount;
        private StringBuilder _pages;
        private StringBuilder _objects;
        private int _xrefStart;
        private int _currentOffset;
        private StringBuilder _xref;
        private PaperSize _paperSize;

        public PdfDocument(PaperSize? paperSize = null)
        {
            _content = new StringBuilder();
            _objectCount = 1;
            _pages = new StringBuilder();
            _objects = new StringBuilder();
            _xref = new StringBuilder();
            _content.AppendLine("%PDF-1.7");
            _xref.AppendLine("xref");
            _xref.AppendLine("0 1");
            _xref.AppendLine("0000000000 65535 f ");
            _currentOffset = _content.Length;

            // Default paper size is A4 if not specified
            _paperSize = paperSize ?? PaperSize.A4;
        }

        public void AddPage()
        {
            // Page object
            int pageObjectNumber = _objectCount + 3;
            int contentObjectNumber = _objectCount + 4;

            string pageObject = $"{pageObjectNumber} 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {_paperSize.WidthPoints} {_paperSize.HeightPoints}] /Contents {contentObjectNumber} 0 R /Resources << /Font << /F1 3 0 R >> >> >>\nendobj\n";
            _objects.Append(pageObject);
            _xref.AppendLine($"{_currentOffset:D10} 00000 n ");
            _currentOffset += Encoding.ASCII.GetByteCount(pageObject);

            // Append page reference to the pages dictionary
            _pages.Append($"{pageObjectNumber} 0 R ");
        }

        public void AddText(string text, int x, int y, int fontSize = 14)
        {
            // Text content with adjusted position and font size
            string content = $"BT /F1 {fontSize} Tf {x} {y} Td ({text}) Tj ET";

            // Page content object
            int contentObjectNumber = _objectCount + 4;
            string contentObject = $"{contentObjectNumber} 0 obj\n<< /Length {Encoding.ASCII.GetByteCount(content)} >>\nstream\n{content}\nendstream\nendobj\n";
            _objects.Append(contentObject);
            _xref.AppendLine($"{_currentOffset:D10} 00000 n ");
            _currentOffset += Encoding.ASCII.GetByteCount(contentObject);
        }

        public void Save(string path)
        {
            // Catalog object
            string catalogObject = "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n";
            _content.Append(catalogObject);
            _xref.AppendLine($"0000000010 00000 n ");
            _currentOffset += Encoding.ASCII.GetByteCount(catalogObject);

            // Pages object
            string pagesObject = $"2 0 obj\n<< /Type /Pages /Kids [{_pages.ToString()}] /Count {_pages.ToString().Split(' ').Length / 3} >>\nendobj\n";
            _content.Append(pagesObject);
            _xref.AppendLine($"{_currentOffset:D10} 00000 n ");
            _currentOffset += Encoding.ASCII.GetByteCount(pagesObject);

            // Font object
            string fontObject = "3 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n";
            _content.Append(fontObject);
            _xref.AppendLine($"{_currentOffset:D10} 00000 n ");
            _currentOffset += Encoding.ASCII.GetByteCount(fontObject);

            // Append objects
            _content.Append(_objects);

            // Cross-reference table
            _xrefStart = _currentOffset;
            _xref.Insert(5, $" {_objectCount + 5}\n");

            // Trailer
            var trailer = new StringBuilder();
            trailer.AppendLine("trailer");
            trailer.AppendLine($"<< /Size {_objectCount + 5} /Root 1 0 R >>");
            trailer.AppendLine("startxref");
            trailer.AppendLine(_xrefStart.ToString());
            trailer.AppendLine("%%EOF");

            // Combine all parts
            _content.Append(_xref);
            _content.Append(trailer);

            // Write to file
            File.WriteAllText(path, _content.ToString(), Encoding.ASCII);
        }
    }