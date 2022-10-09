using Pdf2xNet.Infrastructure.Enums;
using Pdf2xNet.Infrastructure.Interfaces.Models.Xpdf;

namespace Pdf2xNet.Infrastructure.Models.Xpdf
{
    public class Pdf2Text : IPdf2x
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
        /// Maintain (as best as possible) the original physical layout of the text. The default is to ´undo’ physical layout (columns, hyphenation, etc.) 
        /// and output the text in reading order. If the −fixed option is given, character spacing within each line will be determined by the specified 
        /// character pitch.
        /// </summary>
        public bool Layout { get; set; }

        /// <summary>
        /// Similar to −layout, but optimized for simple one-column pages. This mode will do a better job of maintaining horizontal spacing, 
        /// but it will only work properly with a single column of text.
        /// </summary>
        public bool Simple { get; set; }

        /// <summary>
        /// Similar to −simple, but handles slightly rotated text (e.g., OCR output) better. Only works for pages with a single column of text.
        /// </summary>
        public bool Simple2 { get; set; }

        /// <summary>
        /// Table mode is similar to physical layout mode, but optimized for tabular data, with the goal of keeping rows and columns aligned 
        /// (at the expense of inserting extra whitespace). If the −fixed option is given, character spacing within each line will be determined 
        /// by the specified character pitch.
        /// </summary>
        public bool Table { get; set; }

        /// <summary>
        /// Line printer mode uses a strict fixed-character-pitch and -height layout. 
        /// That is, the page is broken into a grid, and characters are placed into that grid. 
        /// If the grid spacing is too small for the actual characters, the result is extra whitespace. 
        /// If the grid spacing is too large, the result is missing whitespace. The grid spacing can be specified using the −fixed and −linespacing options. 
        /// If one or both are not given on the command line, pdftotext will attempt to compute appropriate value(s).
        /// </summary>
        public bool LinePrinter { get; set; }

        /// <summary>
        /// Keep the text in content stream order. Depending on how the PDF file was generated, this may or may not be useful.
        /// </summary>
        public bool Raw { get; set; }

        /// <summary>
        /// Sets the encoding to use for text output. The encoding−name must be defined with the unicodeMap command (see xpdfrc(5)). 
        /// The encoding name is case-sensitive. This defaults to "Latin1" (which is a built-in encoding).
        /// </summary>
        public string EncodingName { get; set; } = "Latin1";

        /// <summary>
        /// Don’t insert a page breaks (form feed character) at the end of each page.
        /// </summary>
        public bool NoPageBreaks { get; set; }

        /// <summary>
        /// Text which is hidden because of clipping is removed before doing layout, and then added back in. 
        /// This can be helpful for tables where clipped (invisible) text would overlap the next column.
        /// </summary>
        public bool Clip { get; set; }

        /// <summary>
        /// Diagonal text, i.e., text that is not close to one of the 0, 90, 180, or 270 degree axes, is discarded. 
        /// This is useful to skip watermarks drawn on top of body text, etc.
        /// </summary>
        public bool NoDiagonal { get; set; }

        /// <summary>
        /// Insert a Unicode byte order marker (BOM) at the start of the text output.
        /// </summary>
        public bool Bom { get; set; }

        /// <summary>
        /// Sets the end-of-line convention to use for text output.
        /// </summary>
        public Eol? EndOfLine { get; set; }

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
