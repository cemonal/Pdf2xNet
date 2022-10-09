using System;
using System.ComponentModel;
using System.Linq;

namespace Pdf2xNet.Infrastructure.Extensions
{
    internal static class EnumExtensions
    {
        public static string ToDescriptionString<T>(this T val) where T : Enum
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
 
            return attributes.Any() ? attributes[0].Description : string.Empty;
        }
    }
}