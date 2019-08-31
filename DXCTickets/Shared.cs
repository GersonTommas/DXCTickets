using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Windows;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Data;
using System.Globalization;

namespace DXCTickets
{
    #region Shared
    class Shared
    {
        #region Variables
        public static string lang_ => Properties.Settings.Default.AllWindowLanguage;
        public static string windowsTitle_ = Properties.Settings.Default.Company + " - TG Software";
        #endregion // Variables
    }
    #endregion // Shared


    #region Command
    public class Command : ICommand
    {
        public delegate void ICommandOnExecute();
        public delegate bool ICommandOnCanExecute();

        private ICommandOnExecute _execute;
        private ICommandOnCanExecute _canExecute;

        public Command(ICommandOnExecute onExecuteMethod, ICommandOnCanExecute onCanExecuteMethod = null)
        { _execute = onExecuteMethod; _canExecute = onCanExecuteMethod; }

        #region ICommand Members
        public event EventHandler CanExecuteChanged
        { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

        public bool CanExecute(object parameter) { return _canExecute?.Invoke() ?? true; }
        public void Execute(object parameter) { _execute?.Invoke(); }
        #endregion
    }
    #endregion // Command


    #region Property Changed
    public abstract class ObservableClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
    }
    #endregion // Property Changed


    #region Converters
    public class BoolInvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { try { return !(bool)value; } catch { return value; } }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException("Not implemented."); }
    }

    public class BoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { try { if ((bool)value == true) { return Visibility.Visible; } else { return Visibility.Collapsed; } } catch { return Visibility.Collapsed; } }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException("Not implemented."); }
    }

    public class BoolToVisInvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { try { if ((bool)value == true) { return Visibility.Collapsed; } else { return Visibility.Visible; } } catch { return Visibility.Visible; } }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException("Not implemented."); }
    }

    public class StringOrderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Inserted": return Properties.Resources.OrderOption1;
                case "Description": return Properties.Resources.OrderOption2;
                case "Ticket": return Properties.Resources.OrderOption3;
                case "Date": return Properties.Resources.OrderOption4;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Orden de ingreso": case "As created": case "Ordem de entrada": return "Inserted";
                case "Descripción": case "Description": case "Descrição": return "Description";
                case "Número de Ticket": case "Ticket number": case "Número do Ticket": return "Ticket";
                case "Fecha / Hora": case "Date / Time": case "Data / Hora": return "Date";
            }
            return null;
        }
    }

    public class StringCompanyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { switch (value) { case "MRP": return "Molinos - MRP"; case "MOA": return "Molinos - MOA"; } return "Brinks";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { switch (value) { case "Molinos - MRP": return "MRP"; case "Molinos - MOA": return "MOA"; } return "Brinks"; }
    }

    public class StringLangConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { switch (value) { case "en": return "English"; case "es": return "Español"; case "pt": return "Português"; } return "English"; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { switch (value) { case "English": return "en"; case "Español": return "es"; case "Português": return "pt"; } return "en"; }
    }

    public class ComparerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (string)value == (string)parameter ? Visibility.Visible : Visibility.Collapsed; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException("Not Implemented."); }
    }

    public class ComparerVisibilityInvertedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (string)value != (string)parameter ? Visibility.Visible : Visibility.Collapsed; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException("Not Implemented"); }
    }
    #endregion // Converters
}
