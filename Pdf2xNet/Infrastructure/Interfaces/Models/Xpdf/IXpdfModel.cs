
namespace Pdf2xNet.Infrastructure.Interfaces.Models.Xpdf
{
    public interface IXpdfModel
    {
        int FirstPage { get; set; }
        
        int LastPage { get; set; }

        string? OwnerPassword { get; set; }

        string? UserPassword { get; set; }
    }
}