#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Volume.UI.Converter
{
    #region usings

    using System;
    using System.Globalization;
    using System.Windows.Data;

    #endregion

    public class IsNotNullConverter : IValueConverter
    {
        #region properties

        public bool Invert { get; set; }

        #endregion

        #region interface IValueConverter

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var result = value is null;
            if( !Invert )
                result = !result;

            return result;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}