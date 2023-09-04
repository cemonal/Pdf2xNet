using Pdf2xNet.Enums.Xpdf;
using Pdf2xNet.Interfaces.Models.Xpdf;

namespace Pdf2xNet.Models.Xpdf
{
    /// <summary>
    /// Represents options for converting PDF to plain text using Xpdf tools.
    /// </summary>
    public class Pdf2Text : IXpdfModel
    {
        /// <inheritdoc/>
        public int FirstPage { get; set; }

        /// <inheritdoc/>
        public int LastPage { get; set; }

        /// <summary>
        /// Gets or sets whether to maintain the original physical layout of the text.
        /// </summary>
        public bool Layout { get; set; }

        /// <summary>
        /// Gets or sets whether to optimize for simple one-column pages.
        /// </summary>
        public bool Simple { get; set; }

        /// <summary>
        /// Gets or sets whether to optimize for simple one-column pages with slightly rotated text.
        /// </summary>
        public bool Simple2 { get; set; }

        /// <summary>
        /// Gets or sets whether to optimize for tabular data.
        /// </summary>
        public bool Table { get; set; }

        /// <summary>
        /// Gets or sets whether to use line printer mode with fixed character pitch and height layout.
        /// </summary>
        public bool LinePrinter { get; set; }

        /// <summary>
        /// Gets or sets whether to keep the text in content stream order.
        /// </summary>
        public bool Raw { get; set; }

        /// <summary>
        /// Gets or sets the encoding to use for text output.
        /// </summary>
        public string EncodingName { get; set; } = "Latin1";

        /// <summary>
        /// Gets or sets whether to insert a Unicode byte order marker (BOM) at the start of the text output.
        /// </summary>
        public bool Bom { get; set; }

        /// <summary>
        /// Gets or sets the end-of-line convention to use for text output.
        /// </summary>
        public Eol? EndOfLine { get; set; }

        /// <inheritdoc/>
        public string? OwnerPassword { get; set; }

        ///<inheritdoc/>
        public string? UserPassword { get; set; }

        /// <summary>
        /// Gets or sets whether to skip page breaks at the end of each page.
        /// </summary>
        public bool NoPageBreaks { get; set; }

        /// <summary>
        /// Gets or sets whether to remove text hidden due to clipping.
        /// </summary>
        public bool Clip { get; set; }

        /// <summary>
        /// Gets or sets whether to discard diagonal text.
        /// </summary>
        public bool NoDiagonal { get; set; }

        /// <summary>
        /// Gets or sets the left margin in points. Text within this margin is discarded.
        /// </summary>
        public double MarginLeft { get; set; }

        /// <summary>
        /// Gets or sets the right margin in points. Text within this margin is discarded.
        /// </summary>
        public double MarginRight { get; set; }

        /// <summary>
        /// Gets or sets the top margin in points. Text within this margin is discarded.
        /// </summary>
        public double MarginTop { get; set; }

        /// <summary>
        /// Gets or sets the bottom margin in points. Text within this margin is discarded.
        /// </summary>
        public double MarginBottom { get; set; }
    }
}
