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
    /// A converter class for converting PDF files to PNG images using Xpdf tools.
    /// </summary>
    public class Pdf2PngConverter : IDocumentConverter<Pdf2Png>
    {
        private readonly XpdfUtilities tool;

        public Pdf2PngConverter()
        {
            tool = new XpdfUtilities(XpdfTool.Pdf2Png);
        }

        /// <summary>
        /// Creates the command line parameters for the Pdf2Png conversion based on the provided options.
        /// </summary>
        private List<string> CreateParameters(Pdf2Png options)
        {
            var args = new List<string>();

            if (options.FirstPage > 0) args.Add($"-f {options.FirstPage}");
            if (options.LastPage > 0) args.Add($"-l {options.LastPage}");
            if (options.Mono) args.Add("-mono");
            if (options.Gray) args.Add("-gray");
            if (options.Alpha) args.Add("-alpha");
            if (options.Resolution != 150) args.Add($"-r {options.Resolution}");

            args.Add($"-rot {options.Rotate}");

            if (options.FreeType) args.Add("-freetype yes");
            if (options.FontAntiAliasing) args.Add("-aa yes");
            if (options.VectorAntiAliasing) args.Add("-aaVector yes");

            if (!string.IsNullOrEmpty(options.OwnerPassword))
                args.Add($"-opw {options.OwnerPassword}");

            if (!string.IsNullOrEmpty(options.UserPassword))
                args.Add($"-upw {options.UserPassword}");

            return args;
        }

        /// <summary>
        /// Converts the PDF file to a list of base64-encoded PNG images asynchronously.
        /// </summary>
        public async Task<List<string>> ConvertAsync(Pdf2Png options, [NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            var rootTempFolderPath = tool.GetRootTempFolderPath();
            var tempFolder = Path.Combine(rootTempFolderPath, Guid.NewGuid().ToString());

            try
            {
                await ConvertAndSaveAsync(options, filePath, tempFolder, cancellationToken);

                var info = new DirectoryInfo(tempFolder);
                var files = info.GetFiles($"*.png").OrderBy(p => p.CreationTime).ToArray();

                var result = new List<string>(files.Length);

                foreach (var file in files)
                {
                    var bytes = await File.ReadAllBytesAsync(file.FullName, cancellationToken).ConfigureAwait(false);
                    result.Add(Convert.ToBase64String(bytes));
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
        /// Converts the PDF file to PNG images and saves them to the specified output path asynchronously.
        /// </summary>
        public async Task ConvertAndSaveAsync(Pdf2Png options, [NotNull] string filePath, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            var result = await tool.RunAsyc(CreateParameters(options), filePath, outputPath, cancellationToken).ConfigureAwait(false);

            if (result != ExitCodes.NoError)
                throw new PdfConversionException($"Exit Code: [{result}] {result.ToDescriptionString()}");
        }
    }
}
