namespace Pdf2xNet.Interfaces.Models.Xpdf
{
    /// <summary>
    /// Represents the common options for Xpdf conversion.
    /// </summary>
    public interface IXpdfModel
    {
        /// <summary>
        /// Gets or sets the first page to convert.
        /// </summary>
        int FirstPage { get; set; }

        /// <summary>
        /// Gets or sets the last page to convert.
        /// </summary>
        int LastPage { get; set; }

        /// <summary>
        /// Gets or sets the owner password for the PDF file. Providing this will bypass all security restrictions.
        /// </summary>
        string? OwnerPassword { get; set; }

        /// <summary>
        /// Gets or sets the user password for the PDF file.
        /// </summary>
        string? UserPassword { get; set; }
    }
}