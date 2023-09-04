using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Interfaces.Converters
{
    /// <summary>
    /// Interface for converting PDF files to various formats using Xpdf tools.
    /// </summary>
    /// <typeparam name="TOptions">The type of conversion options specific to the converter.</typeparam>
    public interface IDocumentConverter<TOptions>
    {
        /// <summary>
        /// Converts the content of the PDF document and returns it in the specified format.
        /// </summary>
        /// <param name="options">The conversion options.</param>
        /// <param name="filePath">The path to the PDF file.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A list of converted content.</returns>
        Task<List<string>> ConvertAsync(TOptions options, string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Converts the content of the PDF document and saves it to the specified output path in the specified format.
        /// </summary>
        /// <param name="options">The conversion options.</param>
        /// <param name="filePath">The path to the PDF file.</param>
        /// <param name="outputPath">The path to save the converted content.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>>
        Task ConvertAndSaveAsync(TOptions options, string filePath, string outputPath, CancellationToken cancellationToken = default);
    }
}
