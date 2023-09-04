using Pdf2xNet.Enums.Xpdf;
using Pdf2xNet.Extensions;
using Pdf2xNet.Helpers;
using Pdf2xNet.Interfaces.Converters;
using Pdf2xNet.Models.Xpdf;
using Pdf2xNet.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Converter.Xpdf
{
    /// <summary>
    /// A converter class for converting PDF files to plain text using Xpdf tools.
    /// </summary>
    public class Pdf2TextConverter : IDocumentConverter<Pdf2Text>
    {
        private readonly XpdfUtilities tool;

        public Pdf2TextConverter()
        {
            tool = new XpdfUtilities(XpdfTool.Pdf2Text);
        }

        /// <summary>
        /// Creates the command line parameters for the Pdf2Text conversion based on the provided options.
        /// </summary>
        private List<string> CreateParameters(Pdf2Text options)
        {
            if (options.EndOfLine == null)
            {
                var os = PlatformHelper.GetOperatingSystem();

                if (os == OSPlatform.OSX)
                    options.EndOfLine = Eol.Mac;
                else if (os == OSPlatform.Linux)
                    options.EndOfLine = Eol.Unix;
                else
                    options.EndOfLine = Eol.Dos;
            }

            var args = new List<string>();

            if (options.FirstPage > 0) args.Add($"-f {options.FirstPage}");
            if (options.LastPage > 0) args.Add($"-l {options.LastPage}");
            if (options.Layout) args.Add("-layout");
            if (options.Simple) args.Add("-simple");
            if (options.Simple2) args.Add("-simple2");
            if (options.Table) args.Add("-table");
            if (options.LinePrinter) args.Add("-lineprinter");
            if (options.Raw) args.Add("-raw");

            args.Add($"-enc {options.EncodingName}");
            args.Add($"-eol {options.EndOfLine.ToString().ToLower()}");

            if (options.NoDiagonal) args.Add("-nodiag");
            if (options.Clip) args.Add("-clip");
            if (options.NoPageBreaks) args.Add("-nopgbrk");

            if (!string.IsNullOrEmpty(options.OwnerPassword))
                args.Add($"-opw {options.OwnerPassword}");

            if (!string.IsNullOrEmpty(options.UserPassword))
                args.Add($"-upw {options.UserPassword}");

            return args;
        }

        /// <summary>
        /// Converts the PDF file to a list of plain text strings asynchronously.
        /// </summary>
        public async Task<List<string>> ConvertAsync(Pdf2Text options, [NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            var tempFilePath = Path.Combine(tool.GetRootTempFolderPath(), $"{Path.GetFileNameWithoutExtension(filePath)}-{DateTimeOffset.Now.ToUnixTimeSeconds()}.txt");

            try
            {
                await ConvertAndSaveAsync(options, filePath, tempFilePath, cancellationToken);

                var text = await File.ReadAllTextAsync(tempFilePath, cancellationToken).ConfigureAwait(false);
                File.Delete(tempFilePath);

                return !string.IsNullOrEmpty(text) ? PrepareTextResponse(text, options.NoPageBreaks) : new List<string>();
            }
            catch (Exception)
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
                throw;
            }
        }

        /// <summary>
        /// Converts the PDF file to plain text and saves it to the specified output path asynchronously.
        /// </summary>
        public async Task ConvertAndSaveAsync(Pdf2Text options, [NotNull] string filePath, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            var result = await tool.RunAsyc(CreateParameters(options), filePath, outputPath, cancellationToken).ConfigureAwait(false);

            if (result != ExitCodes.NoError)
                throw new PdfConversionException($"Exit Code: [{result}] {result.ToDescriptionString()}");
        }

        /// <summary>
        /// Prepares the plain text response by splitting it into lines based on page breaks.
        /// </summary>
        private List<string> PrepareTextResponse(string text, bool noPageBreaks)
        {
            return noPageBreaks ? text.Split('\f', (char)StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string> { text };
        }
    }
}
