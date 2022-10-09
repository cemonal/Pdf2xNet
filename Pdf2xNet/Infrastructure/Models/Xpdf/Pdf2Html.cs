
using Pdf2xNet.Infrastructure.Interfaces.Models.Xpdf;

namespace Pdf2xNet.Infrastructure.Models.Xpdf
{
    public class Pdf2Html : IXpdfModel
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
        /// Specifies the initial zoom level. The default is 1.0, which means 72dpi, i.e., 1 point in the PDF file will be 1 pixel in the HTML. 
        /// Using ´-z 1.5’, for example, will make the initial view 50% larger.
        /// </summary>
        public double ZoomLevel { get; set; } = 1.0;

        /// <summary>
        /// Specifies the resolution, in DPI, for background images. This controls the pixel size of the background image files. 
        /// The initial zoom level is controlled by the ´-z’ option. Specifying a larger ´-r’ value will allow the viewer to zoom in farther without 
        /// upscaling artifacts in the background.
        /// </summary>
        public int Resolution { get; set; }

        /// <summary>
        /// Specifies a vertical stretch factor. Setting this to a value greater than 1.0 will stretch each page vertically, spreading out the lines. 
        /// This also stretches the background image to match.
        /// </summary>
        public double Vstretch { get; set; } = 1.0;

        /// <summary>
        /// Embeds the background image as base64-encoded data directly in the HTML file, rather than storing it as a separate file.
        /// </summary>
        public bool EmbedBackground { get; set; }

        /// <summary>
        /// Disable extraction of embedded fonts. By default, pdftohtml extracts TrueType and OpenType fonts. 
        /// Disabling extraction can work around problems with buggy fonts.
        /// </summary>
        public bool NoFonts { get; set; }

        /// <summary>
        /// Embeds any extracted fonts as base64-encoded data directly in the HTML file, rather than storing them as separate files.
        /// </summary>
        public bool EmbedFonts { get; set; }

        /// <summary>
        /// Don’t draw invisible text. By default, invisible text (commonly used in OCR’ed PDF files) is drawn as transparent (alpha=0) HTML text. 
        /// This option tells pdftohtml to discard invisible text entirely.
        /// </summary>
        public bool SkipInvisible { get; set; }

        /// <summary>
        /// Treat all text as invisible. By default, regular (non-invisible) text is not drawn in the background image, 
        /// and is instead drawn with HTML on top of the image. 
        /// This option tells pdftohtml to include the regular text in the background image, and then draw it as transparent (alpha=0) HTML text.
        /// </summary>
        public bool AllInvisible { get; set; }

        /// <summary>
        /// Convert AcroForm text and checkbox fields to HTML input elements. This also removes text (e.g., underscore characters) 
        /// and erases background image content (e.g., lines or boxes) in the field areas.
        /// </summary>
        public bool FormFields { get; set; }

        /// <summary>
        /// Use table mode when performing the underlying text extraction. This will generally produce better output when the PDF content is a full-page table. 
        /// NB: This does not generate HTML tables; it just changes the way text is split up.
        /// </summary>
        public bool Table { get; set; }

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