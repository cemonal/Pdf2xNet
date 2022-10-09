using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Pdf2xNet.Infrastructure.Utilities
{
    internal static class XpdfUtility
    {
        public static string FindTool(string fileName)
        {
            var os = PlatformUtility.GetOperatingSystem();
            string folder = "Windows";
            string extension = "exe";

            if (os == OSPlatform.Linux)
            {
                folder = "Linux";
                extension = "linux";
            }
            else if (os == OSPlatform.OSX)
            {
                folder = "Mac";
                extension = "mac";
            }

            return Path.Combine("Lib", "Xpdf", folder, Environment.Is64BitOperatingSystem ? "x64" : "x86", $"{fileName}.{extension}");
        }
    }
}