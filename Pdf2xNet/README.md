# Pdf2xNet Library

Pdf2xNet is a .NET library that provides a convenient way to interact with Xpdf tools for PDF conversion tasks. It offers a set of classes and utilities to easily convert PDF documents into various formats like text, images, or HTML.

## Purpose

The primary purpose of Pdf2xNet is to simplify the process of using the Xpdf tools (https://www.xpdfreader.com/) for PDF conversions within .NET applications. It offers a high-level interface and abstraction to interact with Xpdf's features without the need for low-level command-line interactions.

## Features

- Convert PDF to Text: Extract text content from PDF documents.
- Convert PDF to Images: Convert PDF pages to images (PNG, JPEG, etc.).
- Convert PDF to HTML: Convert PDF documents into HTML format.
- Comprehensive Options: Pdf2xNet provides various options to customize conversion settings.

## Getting Started

### Usage

#### Pdf2Text Conversion Example

```csharp
using Pdf2xNet.Converter.Xpdf;

var converter = new Pdf2TextConverter();
var options = new Pdf2Text
{
    FirstPage = 1,
    LastPage = 5
};

var text = await converter.ConvertAsync(options, "input.pdf");
Console.WriteLine(text);
```

#### Pdf2Html Conversion Example

```csharp
using Pdf2xNet.Converter.Xpdf;

var converter = new Pdf2TextConverter();
var options = new Pdf2Text
{
    FirstPage = 1,
    LastPage = 5
};

var text = await converter.ConvertAsync(options, "input.pdf");
Console.WriteLine(text);
```

#### Pdf2Png Conversion Example

```csharp
using Pdf2xNet.Converter.Xpdf;

var converter = new Pdf2PngConverter();
var options = new Pdf2Png
{
    FirstPage = 1,
    LastPage = 1,
    Resolution = 300,
    Gray = true
};

var pngImages = await converter.ConvertAsync(options, "input.pdf");
foreach (var image in pngImages)
{
    Console.WriteLine(image);
}
```

## Contribution
Contributions to Pdf2xNet are welcome! If you have any improvements or fixes, feel free to create a pull request.
