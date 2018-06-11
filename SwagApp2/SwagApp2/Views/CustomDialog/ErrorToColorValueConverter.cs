using System;
using System.Globalization;
using Prism.Unity;
using Xamarin.Forms;

namespace SwagApp2.Views.CustomDialog
{
    public class ErrorToColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return (Color)Prism.PrismApplicationBase.Current.Resources["WarningModalBackgroundColor"];

            return (Color)Prism.PrismApplicationBase.Current.Resources["SuccesModalBackgroundColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}