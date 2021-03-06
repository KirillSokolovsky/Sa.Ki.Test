﻿namespace Sa.Ki.Test.WebAutomation.DesktopApp.Converters
{
    using ReactiveUI;
    using Sa.Ki.Test.DesktopApp.Models.SaKiMenu;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Controls;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(FrameLocatorType), typeof(bool))]
    public class NotFrameLocatorIsLocatorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FrameLocatorType flt) return flt != FrameLocatorType.Locator;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("NotFrameLocatorIsLocatorConverter doesn't support back convertation");
        }
    }
}
