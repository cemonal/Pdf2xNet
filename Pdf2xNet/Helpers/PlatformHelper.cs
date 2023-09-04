using System;
using System.Runtime.InteropServices;

namespace Pdf2xNet.Helpers
{
    /// <summary>
    /// Helper class for retrieving information about the current operating system platform.
    /// </summary>
    internal static class PlatformHelper
    {
        /// <summary>
        /// Gets the current operating system platform.
        /// </summary>
        /// <returns>The operating system platform.</returns>
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