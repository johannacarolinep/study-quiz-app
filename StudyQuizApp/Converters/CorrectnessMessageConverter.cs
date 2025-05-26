using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StudyQuizApp.Converters
{
    public class CorrectnessMessageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return "";

            bool? isCorrect = values[0] as bool?;
            string selectedOption = values[1] as string ?? "";

            if (isCorrect == true)
                return "Your answer was correct";
            else
                return $"Your answer was incorrect. You answered: {selectedOption}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
