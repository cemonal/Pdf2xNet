using System;
using System.Runtime.Serialization;

namespace Pdf2xNet
{
    [Serializable]
    public class PdfConversionException : Exception
    {
        public PdfConversionException() { }

        public PdfConversionException(string message) : base(message) { }

        public PdfConversionException(string message, Exception innerException) : base(message, innerException) { }

        protected PdfConversionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
