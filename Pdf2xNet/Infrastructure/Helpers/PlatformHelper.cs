using System;
using System.Runtime.InteropServices;

namespace Pdf2xNet.Infrastructure.Helpers
{
    internal static class PlatformHelper
    {
        public static OSPlatform GetOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OSPlatform.OSX;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return OSPlatform.Linux;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return OSPlatform.Windows;

            throw new NotSupportedException("Cannot determine operating system!");
        }
    }
}