using AusgabenTracker.ViewModels;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AusgabenTracker.Helpers
{
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is LimitStatus status
                ? status switch
                {
                    LimitStatus.Ok => Brushes.Green,
                    LimitStatus.Warning => Brushes.Orange,
                    LimitStatus.Over => Brushes.Red,
                    _ => Brushes.Gray
                }
                : Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}

