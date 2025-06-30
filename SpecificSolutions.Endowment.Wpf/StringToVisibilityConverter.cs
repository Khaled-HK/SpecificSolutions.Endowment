using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SpecificSolutions.Endowment.Wpf;
public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.IsNullOrEmpty(value as string) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

//  <SpaProxyServerUrl>https://localhost:59658</SpaProxyServerUrl>
