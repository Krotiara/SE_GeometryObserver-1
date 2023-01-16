using Extensions;
using SE_GeometryObserver.Interfaces;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SE_GeometryObserver.Models.Converters
{
    public class EnumToDisplayStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((GeomAndCoordsCompareResult)value).GetDisplayAttributeValue();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
