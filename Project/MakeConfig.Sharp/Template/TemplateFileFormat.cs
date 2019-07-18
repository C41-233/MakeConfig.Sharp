using System;
using System.Reflection;
using MakeConfig.Configs;

namespace MakeConfig.Template
{

    internal class ConfigStub
    {
    }

    internal class TemplateFileFormat : IFormatProvider, ICustomFormatter
    {
        
        object IFormatProvider.GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }

        string ICustomFormatter.Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is ConfigStub)
            {
                var field = typeof(Config).GetField(format, BindingFlags.Public | BindingFlags.Static);
                if (field == null)
                {
                    throw new FormatException(format);
                }

                return field.GetValue(null)?.ToString();
            }
            
            throw new FormatException(format);
        }
    }
}
