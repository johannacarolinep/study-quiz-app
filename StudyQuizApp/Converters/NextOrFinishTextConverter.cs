using System;
using System.Globalization;
using System.Windows.Data;

namespace StudyQuizApp.Converters
{
    public class NextOrFinishTextConverter : IValueConverter
    {
        public string NextText { get; set; } = "Next";
        public string FinishText { get; set; } = "Finish";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isLast && isLast)
                return FinishText;
            return NextText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}