using Pdf2xNet.Enums.Xpdf;
using Pdf2xNet.Extensions;
using Pdf2xNet.Interfaces.Converters;
using Pdf2xNet.Models.Xpdf;
using Pdf2xNet.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Converter.Xpdf
{
    /// <summary>
    /// A converter class for converting PDF files to HTML using Xpdf tools.
    /// </summary>
    public class Pdf2HtmlConverter : IDocumentConverter<Pdf2Html>
    {
        private readonly XpdfUtilities tool;

        public Pdf2HtmlConverter()
        {
            tool = new XpdfUtilities(XpdfTool.Pdf2Html);
        }

        /// <summary>
        /// Creates the command line parameters for the Pdf2Html conversion based on the provided options.
        /// </summary>
        private List<string> CreateParameters(Pdf2Html options)
        {
            var args = new List<string>();

            if (options.FirstPage > 0) args.Add($"-f {options.FirstPage}");
            if (options.LastPage > 0) args.Add($"-l {options.LastPage}");
            if (options.ZoomLevel != 1.0) args.Add($"-z {options.ZoomLevel}");
            if (options.Resolution > 0) args.Add($"-r {options.Resolution}");
            if (options.Vstretch != 1.0) args.Add($"-vstretch {options.Vstretch}");
            if (options.EmbedBackground) args.Add("-embedbackground");
            if (options.NoFonts) args.Add("-nofonts");
            if (options.EmbedFonts) args.Add("-embedfonts");
            if (options.SkipInvisible) args.Add("-skipinvisible");
            if (options.AllInvisible) args.Add("-allinvisible");
            if (options.FormFields) args.Add("-formfields");
            if (options.Table) args.Add("-table");

            if (!string.IsNullOrEmpty(options.OwnerPassword))
                args.Add($"-opw {options.OwnerPassword}");

            if (!string.IsNullOrEmpty(options.UserPassword))
                args.Add($"-upw {options.UserPassword}");

            return args;
        }

        /// <summary>
        /// Converts the PDF file to HTML asynchronously.
        /// </summary>
        public async Task<List<string>> ConvertAsync(Pdf2Html options, [NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            var rootTempFolderPath = tool.GetRootTempFolderPath();
            var tempFolder = Path.Combine(rootTempFolderPath, Guid.NewGuid().ToString());

            try
            {
                await ConvertAndSaveAsync(options, filePath, tempFolder, cancellationToken);

                var info = new DirectoryInfo(tempFolder);
                var files = info.GetFiles($"*.html").OrderBy(p => p.CreationTime).ToArray();

                var result = new List<string>(files.Length);

                foreach (var file in files)
                {
                    var content = await File.ReadAllTextAsync(file.FullName, cancellationToken).ConfigureAwait(false);
                    result.Add(content);
                }

                Directory.Delete(tempFolder, true);

                return result;
            }
            catch (Exception)
            {
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
                throw;
            }
        }

        /// <summary>
        /// Converts the PDF file to HTML and saves it to the specified output path asynchronously.
        /// </summary>
        public async Task ConvertAndSaveAsync(Pdf2Html options, [NotNull] string filePath, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            var result = await tool.RunAsyc(CreateParameters(options), filePath, outputPath, cancellationToken).ConfigureAwait(false);

            if (result != ExitCodes.NoError)
                throw new PdfConversionException($"Exit Code: [{result}] {result.ToDescriptionString()}");
        }
    }
}
