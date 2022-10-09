using Pdf2xNet.Infrastructure.Enums.Xpdf;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Infrastructure.Interfaces.Converters
{
    public interface IXpdfConverter
    {
        Task<ExitCodes> ExtractAsync([NotNull] byte[] file, [NotNull] string outputPath, CancellationToken cancellationToken = default);
        Task<List<string>> ExtractAsync([NotNull] byte[] file, CancellationToken cancellationToken = default);
        Task<ExitCodes> ExtractAsync([NotNull] string filePath, [NotNull] string outputPath, CancellationToken cancellationToken = default);
        Task<List<string>> ExtractAsync([NotNull] string filePath, CancellationToken cancellationToken = default);
    }
}
