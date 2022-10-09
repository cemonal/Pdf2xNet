
using System.ComponentModel;

namespace Pdf2xNet.Infrastructure.Enums.Xpdf
{
    public enum ExitCodes : ushort
    {
        [Description("No error.")]
        NoError = 0,

        [Description("Error opening a PDF file.")]
        PdfAccessDenied = 1,

        [Description("Error opening an output file.")]
        OutputAccessDenied = 2,

        [Description("Error related to PDF permissions.")]
        PdfPermissionDenied = 3,

        [Description("Other error.")]
        Other = 99
    }
}