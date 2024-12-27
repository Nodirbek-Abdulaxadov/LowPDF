using System.ComponentModel;
using System.Diagnostics;

var pdf = new PdfDocument();
var text1 = new PdfTextElement("Hello, PDF!", 0, 0, 16);
Margin margin = new(10);
text1.AppendStyle(margin);
pdf.AddElement(text1);
var text2 = new PdfTextElement("Hello, PDF2!", 0, 0, 16);
text2.AppendStyle(margin);
pdf.AddElement(text2);
pdf.Save("output.pdf");

#region HotReload
// kill all firefox processes
foreach (var process in Process.GetProcessesByName("firefox"))
{
    process.Kill();
}
try
{
    string pdfPath = Path.Combine(Environment.CurrentDirectory, "output.pdf");
    Process.Start(@"C:\Program Files\Mozilla Firefox\firefox.exe", pdfPath);
}
catch (Win32Exception ex)
{
    Console.WriteLine($"Error starting process: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
}
#endregion