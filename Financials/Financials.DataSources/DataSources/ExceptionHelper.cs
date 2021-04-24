using Financials.DataSources.DataSources.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Financials.DataSources.DataSources
{
    internal static class ExceptionHelper
    {

        internal static string GetFormattedString(string name, params object[] args)
        {
            return GetFormattedString(CultureInfo.CurrentCulture, name, args);
        }

        internal static string GetFormattedString(CultureInfo culture, string name, params object[] args)
        {
            String format = StringResources.ResourceManager.GetString(name, culture);

            if ((args == null) || (args.Length == 0))
                return format;

            return String.Format(culture, format, args);
        }

        internal static string GetString(string name)
        {
            return GetString(CultureInfo.CurrentCulture, name);
        }

        internal static string GetString(CultureInfo culture, string name)
        {
            return StringResources.ResourceManager.GetString(name, culture);
        }
    }
}
