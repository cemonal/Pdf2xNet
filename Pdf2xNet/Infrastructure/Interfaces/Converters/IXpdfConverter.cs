
using Pdf2xNet.Infrastructure.Enums;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Infrastructure.Interfaces.Converters
{
    public interface IXpdfConverter
    {
        Task<ExitCodes> ExtractAsync([NotNull] string filePath, [NotNull] string outputDirectory, CancellationToken cancellationToken = default);
        Task<List<string>> ExtractAsync([NotNull] string filePath, CancellationToken cancellationToken = default);
    }
}
