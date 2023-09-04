using System;
using System.ComponentModel;
using System.Linq;

namespace Pdf2xNet.Extensions
{
    /// <summary>
    /// Provides extension methods for working with enums.
    /// </summary>
    internal static class EnumExtensions
    {
        /// <summary>
        /// Gets the description associated with an enum value using the DescriptionAttribute.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="val">The enum value.</param>
        /// <returns>The description associated with the enum value, or an empty string if not found.</returns>
        public static string ToDescriptionString<T>(this T val) where T : Enum
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.FirstOrDefault()?.Description ?? string.Empty;
        }
    }
}
