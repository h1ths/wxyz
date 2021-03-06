﻿using System;
using System.Windows.Data;

namespace uvwxyz.view.Converter
{
    /// <summary>  
    /// 渠道单选钮的转换类  
    /// </summary>  
    public class RadioBtnChannelConverter : IValueConverter
    {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                bool reuslt = false;
                if (value == null || parameter == null)
                {
                    return reuslt;
                }

                if (value.ToString() == parameter.ToString())
                {
                    reuslt = true;
                }
                else
                {
                    reuslt = false;
                }
                return reuslt;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value == null || parameter == null)
                {
                    return null;
                }

                bool usevalue = (bool)value;
                if (usevalue)
                {
                    return parameter.ToString();
                }
                else
                {
                    return null;
                }
            }
    }
}
