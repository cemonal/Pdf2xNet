using Pdf2xNet.Converter.Xpdf;
using Pdf2xNet.Infrastructure.Models.Xpdf;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Converter
{
    public class XpdfConverter
    {
        public async Task<List<string>> ConvertToPngAsync(Pdf2Png options, string filePath, CancellationToken cancellationToken = default)
        {
            return await new Pdf2PngConverter().ExtractAsync(options, filePath, cancellationToken);
        }

        public async Task<List<string>> ConvertToHtmlAsync(Pdf2Html options, string filePath, CancellationToken cancellationToken = default)
        {
            return await new Pdf2HtmlConverter().ExtractAsync(options, filePath, cancellationToken);
        }

        public async Task<List<string>> ConvertToTextAsync(Pdf2Text options, string filePath, CancellationToken cancellationToken = default)
        {
            return await new Pdf2TextConverter().ExtractAsync(options, filePath, cancellationToken);
        }
    }
}