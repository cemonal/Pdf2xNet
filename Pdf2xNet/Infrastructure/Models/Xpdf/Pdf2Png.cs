using Pdf2xNet.Infrastructure.Enums.Xpdf;
using Pdf2xNet.Infrastructure.Interfaces.Models.Xpdf;

namespace Pdf2xNet.Infrastructure.Models.Xpdf
{
    public class Pdf2Png : IXpdfModel
    {
        /// <summary>
        /// Specifies the first page to convert.
        /// </summary>
        public int FirstPage { get; set; }

        /// <summary>
        /// Specifies the last page to convert.
        /// </summary>
        public int LastPage { get; set; }

        /// <summary>
        /// Specifies the resolution, in DPI. The default is 150 DPI.
        /// </summary>
        public int Resolution { get; set; } = 150;

        /// <summary>
        /// Generate a monochrome image (instead of a color image).
        /// </summary>
        public bool Mono { get; set; }

        /// <summary>
        /// Generate a grayscale image (instead of a color image).
        /// </summary>
        public bool Gray { get; set; }

        /// <summary>
        /// Generate an alpha channel in the PNG file. This is only useful with PDF files that have been constructed with a transparent background. 
        /// The −alpha flag cannot be used with −mono.
        /// </summary>
        public bool Alpha { get; set; }

        /// <summary>
        /// Rotate pages by 0 (the default), 90, 180, or 270 degrees.
        /// </summary>
        public Rotate Rotate { get; set; } = Rotate.None;

        /// <summary>
        /// Enable or disable FreeType (a TrueType / Type 1 font rasterizer). This defaults to "yes".
        /// </summary>
        public bool FreeType { get; set; } = true;

        /// <summary>
        /// Enable or disable font anti-aliasing. This defaults to "yes".
        /// </summary>
        public bool FontAntiAliasing { get; set; } = true;

        /// <summary>
        /// Enable or disable vector anti-aliasing. This defaults to "yes".
        /// </summary>
        public bool VectorAntiAliasing { get; set; } = true;

        /// <summary>
        /// Specify the owner password for the PDF file. Providing this will bypass all security restrictions.
        /// </summary>
        public string? OwnerPassword { get; set; }

        /// <summary>
        /// Specify the user password for the PDF file.
        /// </summary>
        public string? UserPassword { get; set; }
    }
}
