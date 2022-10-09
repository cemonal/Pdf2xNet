using Pdf2xNet.Converter;
using Pdf2xNet.Converter.Xpdf;
using Pdf2xNet.Infrastructure.Models.Xpdf;
using System.IO;
using System.Reflection;
using System.Text;

class Program
{
    private readonly static string _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

    [STAThread]
    static async Task Main(string[] args)
    {
        Console.Title = "Pdf2xNet Demo Tool";
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        await Process();
    }

    private static string UploadFile()
    {
        string selectedPath = string.Empty;

        var t = new Thread(() =>
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open PDF Document",
                Filter = "PDF Document|*.pdf"
            };

            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    selectedPath = dialog.FileName;
            }
        });

        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        t.Join();

        return selectedPath;
    }

    private static string SelectPath()
    {
        string selectedPath = string.Empty;

        var t = new Thread(() =>
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = true
            };

            if (fbd.ShowDialog() == DialogResult.Cancel)
                return;

            selectedPath = fbd.SelectedPath;
        });

        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        t.Join();

        return selectedPath;
    }

    private async static Task GenerateHtml(string[] files, string exportFile)
    {
        var sb = new StringBuilder();
        sb.Append("<!DOCTYPE html>");
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\" />");
        sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");

        foreach (var file in files)
        {
            var bytes = File.ReadAllBytes(file);
            var src = Convert.ToBase64String(bytes);
            
            sb.AppendLine($"<img src='data:image/png;base64, {src}' /><br />");
        }

        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        await File.WriteAllTextAsync(exportFile, sb.ToString());
    }

    private async static Task Process()
    {
        try
        {
            Console.WriteLine($"\r\nWelcome to Pdf2xNet Demo Tool. v{_version}");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Please select the action you want to do:");
            Console.WriteLine("1      -  Pdf to HTML");
            Console.WriteLine("2      -  Pdf to Text");
            Console.WriteLine("3      -  Pdf to PNG");
            Console.WriteLine("4      -  Pdf to HTML (with PNG)");
            Console.WriteLine("clear  -  Clear screen");
            Console.WriteLine("exit   -  Exit the application");

            var action = Console.ReadLine();

            if (string.Equals(action, "exit", StringComparison.InvariantCultureIgnoreCase))
            {
                Environment.Exit(0);
            }
            else if (string.Equals(action, "clear", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Clear();
                await Process();
            }

            Console.WriteLine("\r\nPlease select the pdf file:");
            var file = UploadFile();
            Console.WriteLine("File imported!\r\n");

            string targetFolder = string.Empty;

            var option = new Pdf2Text
            {
                EncodingName = "UTF-8"
            };

            var xpdfConverter = new XpdfConverter();
            var result = await xpdfConverter.ConvertToTextAsync(option, file);

            Console.WriteLine("\r\n*********************************************************************************");
            Console.WriteLine("Process done!");
            Console.WriteLine("*********************************************************************************");
            Console.WriteLine(Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            await Process();
        }
    }
}