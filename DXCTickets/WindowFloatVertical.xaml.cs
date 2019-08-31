using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

using System.Drawing;

namespace DXCTickets
{
    /// <summary>
    /// Interaction logic for WindowFloatHorizontal.xaml
    /// </summary>
    public partial class WindowFloatVertical : Window
    {
        #region Variables
        private WBinding Wbind = new WBinding();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        #endregion // Variables


        #region Initialize
        public WindowFloatVertical()
        { InitializeComponent(); dispatcherTimer.Tick += dispatcherTimer_Tick; dispatcherTimer.Interval = new TimeSpan(0, 0, 5); if (Properties.Settings.Default.MainAlwaysVisible) { Topmost = true; }; Db.DBConnect(); DataContext = Wbind; Wbind.ThisWindow = this; Wbind.UserStr = Environment.UserDomainName + "\\" + Environment.UserName; }
        #endregion // Initialize

        #region Click
        private void CheckBox_Click(object sender, RoutedEventArgs e) { var sndr = sender as CheckBox; Topmost = (bool)sndr.IsChecked; }
        #endregion // Click


        #region Binding
        private class WBinding : ObservableClass
        {
            #region Private
            private string _UserStr;
            private bool _TicketPage = false;
            private bool _ScreenShotSound => Properties.Settings.Default.ScreenShotSound;
            private string _SolLanguage = "En";
            private DBSolutionsClass _SelectedSolution;
            private bool _WindowMouseIn = false;

            OptionsWindow OpWindow;
            SolutionsWindow Slwindow;

            public WindowFloatVertical ThisWindow;
            #endregion // Private

            #region Public
            public string UserStr { get { return _UserStr; } set { if (_UserStr != value) { _UserStr = value; OnPropertyChanged("UserStr"); } } }
            public string WindowTitle => Shared.windowsTitle_;
            public string SolLanguage { get { return _SolLanguage; } set { if (_SolLanguage != value) { _SolLanguage = value; OnPropertyChanged("SolLanguage"); } } }

            public bool WindowMouseIn { get { return _WindowMouseIn; } set { if (_WindowMouseIn != value) { _WindowMouseIn = value; OnPropertyChanged("WindowMouseIn"); OnPropertyChanged("WindowMaxWidth"); } } }
            public int WindowMaxWidth { get { return !WindowMouseIn && WindowAutoHide ? 5 : 500; } }

            public bool TicketPage { get { return _TicketPage; } set { if (_TicketPage != value) { _TicketPage = value; OnPropertyChanged("TicketPage"); } } }

            public IOrderedQueryable<DBSolutionsClass> AllSolutionsList => Db.DBTickets.AllSolutionsTable.OrderBy(x => x.Name);
            public DBSolutionsClass SelectedSolution { get { return _SelectedSolution; } set { if (_SelectedSolution != value) { _SelectedSolution = value; OnPropertyChanged("SelectedSolution"); } } }

            public WindowStyle WindowClose { get { return Properties.Settings.Default.WindowClose ? WindowStyle.None : WindowStyle.SingleBorderWindow; } }
            public bool WindowAutoHide { get { return Properties.Settings.Default.WindowAutoHide; } }
            public Visibility CloseVisible { get { return Properties.Settings.Default.WindowClose ? Visibility.Visible : Visibility.Collapsed; } }
            #endregion // Public

            #region Helpers
            #endregion // Helpers

            #region Commands
            public Command ClipUserCommand { get { return new Command(() => { Clipboard.SetText(UserStr); }); } }

            public Command ClipDateCommand { get { return new Command(() => { Clipboard.SetText(DateTime.Now.ToString("yyyy/MM/dd HH:mm")); }); } }

            public Command NewSolutionCommand
            { get { return new Command(() => { if (!Application.Current.Windows.OfType<SolutionsWindow>().Any()) { Slwindow = new SolutionsWindow(Db.DBTickets.GlobalNewSolution); Slwindow.Show(); } }); } }

            public Command OptionsPageCommand
            { get { return new Command(() => { if (!Application.Current.Windows.OfType<OptionsWindow>().Any()) { OpWindow = new OptionsWindow(); OpWindow.Show(); ThisWindow.Close(); } }); } }

            public Command SolutionsReloadCommand { get { return new Command(() => { OnPropertyChanged("AllSolutionsList"); }); } }

            public Command EditSolutionCommand
            { get { return new Command(() => { if (!Application.Current.Windows.OfType<SolutionsWindow>().Any()) { Slwindow = new SolutionsWindow(SelectedSolution, true); Slwindow.Show(); } }, () => { return SelectedSolution != null; }); } }

            public Command SolutionsLanguageCommand
            {
                get
                {
                    return new Command(() =>
                    { switch (SolLanguage) { case "En": SolLanguage = "Es"; break; case "Es": SolLanguage = "Pt"; break; case "Pt": SolLanguage = "En"; break; } });
                }
            }

            public Command ScreenShotCommand
            {
                get
                {
                    return new Command(() =>
                    {
                        double screenLeft = SystemParameters.VirtualScreenLeft;
                        double screenTop = SystemParameters.VirtualScreenTop;
                        double screenWidth = SystemParameters.VirtualScreenWidth;
                        double screenHeight = SystemParameters.VirtualScreenHeight;

                        using (Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight))
                        {
                            using (Graphics g = Graphics.FromImage(bmp))
                            {
                                try
                                {
                                    string filename = Properties.Settings.Default.Company + " - " + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".png";
                                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                                    bmp.Save(Properties.Settings.Default.ScreenShotFolder + "\\" + filename);
                                    if (_ScreenShotSound) { System.Media.SystemSounds.Beep.Play(); }
                                }
                                catch { }
                            }
                        }
                    });
                }
            }

            public Command CloseCommand { get { return new Command(() => { ThisWindow.Close(); }); } }

            public Command ButtonOneEnCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EnOne); }, () => { try { return SelectedSolution.EnOne.Length != 0; } catch { return false; } }); } }
            public Command ButtonTwoEnCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EnTwo); }, () => { try { return SelectedSolution.EnTwo.Length != 0; } catch { return false; } }); } }
            public Command ButtonThreeEnCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EnThree); }, () => { try { return SelectedSolution.EnThree.Length != 0; } catch { return false; } }); } }

            public Command ButtonOneEsCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EsOne); }, () => { try { return SelectedSolution.EsOne.Length != 0; } catch { return false; } }); } }
            public Command ButtonTwoEsCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EsTwo); }, () => { try { return SelectedSolution.EsTwo.Length != 0; } catch { return false; } }); } }
            public Command ButtonThreeEsCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EsThree); }, () => { try { return SelectedSolution.EsThree.Length != 0; } catch { return false; } }); } }

            public Command ButtonOnePtCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.PtOne); }, () => { try { return SelectedSolution.PtOne.Length != 0; } catch { return false; } }); } }
            public Command ButtonTwoPtCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.PtTwo); }, () => { try { return SelectedSolution.PtTwo.Length != 0; } catch { return false; } }); } }
            public Command ButtonThreePtCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.PtThree); }, () => { try { return SelectedSolution.PtThree.Length != 0; } catch { return false; } }); } }
            #endregion // Commands
        }
        #endregion // Binding

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) { if (Properties.Settings.Default.WindowClose == true) { this.DragMove(); } }
        private void CloseButton_Click(object sender, RoutedEventArgs e) { Close(); }

        private void Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) { dispatcherTimer.Stop(); Wbind.WindowMouseIn = true; }

        private void Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) { dispatcherTimer.Stop(); dispatcherTimer.Start(); }

        private void dispatcherTimer_Tick(object sender, EventArgs e) { Wbind.WindowMouseIn = false; dispatcherTimer.Stop(); }
    }
}