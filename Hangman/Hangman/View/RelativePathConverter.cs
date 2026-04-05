using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Hangman.View
{
    public class RelativePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string relativePath && !string.IsNullOrEmpty(relativePath))
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}