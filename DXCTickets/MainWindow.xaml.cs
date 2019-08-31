//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace DXCTickets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Initialize

        public MainWindow()
        {
            InitializeComponent();
            Db.DBConnect();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Shared.lang_);
            var FW = new FloatWindow(); FW.Show();
            Close();

        }

        #endregion
    }
}
