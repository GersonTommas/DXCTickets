using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using System.Drawing;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace DXCTickets
{
    /// <summary>
    /// Interaction logic for FloatWindow.xaml
    /// </summary>
    public partial class FloatWindow : Window
    {
        #region Variables



        #endregion


        #region Initialize
        /// <summary>
        /// Default Constructor
        /// </summary>
        public FloatWindow()
        {
            InitializeComponent();

            // Sets the window to never be hidden behind another program or Window
            Topmost = true;

            // Connects to the Database
            Db.DBConnect();

            // Binds the Window to the View Model
            DataContext = new WBinding(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
        }


        #region Window styles

        [Flags]
        public enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        public enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion


        #endregion


        #region Binding
        /// <summary>
        /// View Model
        /// </summary>
        private class WBinding : ObservableClass
        {
            #region Private

            // Timer for the Window Auto-Hide
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();


            // User Name
            private string _UserStr = Environment.UserDomainName + "\\" + Environment.UserName;

            // Sets the sound for the screenshot
            private bool _ScreenShotSound => Properties.Settings.Default.ScreenShotSound;

            // Language of the solution, Default English
            private string _SolLanguage = "En";

            // PlaceHolder for the Selected solution
            private DBSolutionsClass _SelectedSolution;

            // PlaceHolder for THIS Window
            private Window ThisWindow;

            private bool _WindowMouseIn = false;

            private bool _WindowVertical = Properties.Settings.Default.WindowVertical;

            #endregion


            #region Public
            // Language of the solution
            public string SolLanguage { get { return _SolLanguage; } set { if (_SolLanguage != value) { _SolLanguage = value; OnPropertyChanged(nameof(SolLanguage)); } } }

            // Changes when the Mouse Enters or Leaves the Window
            public bool WindowMouseIn { get { return _WindowMouseIn; } set { if (_WindowMouseIn != value) { _WindowMouseIn = value; OnPropertyChanged(nameof(WindowMouseIn)); OnPropertyChanged(nameof(WindowMaxHeight)); OnPropertyChanged(nameof(WindowMaxWidth)); } } }

            // Selected Solution
            public DBSolutionsClass SelectedSolution { get { return _SelectedSolution; } set { if (_SelectedSolution != value) { _SelectedSolution = value; OnPropertyChanged(nameof(SelectedSolution)); } } }

            #endregion


            #region ReadOnly

            // List of all the solutions created by the User
            public IOrderedQueryable<DBSolutionsClass> AllSolutionsList => Db.DBTickets.AllSolutionsTable.OrderBy(x => x.Name);

            // Makes the Close Window button visible or invisible depending on the User options
            public Visibility CloseVisible { get { return Properties.Settings.Default.WindowClose ? Visibility.Visible : Visibility.Collapsed; } }

            // User Name
            public string UserStr => _UserStr;

            // Makes the Window AutoHide depending on User options
            public bool WindowAutoHide { get { return Properties.Settings.Default.WindowAutoHide; } }

            // Makes the Window have normal Minimize/Maximize/Close buttons, or hide them depending on User options
            public WindowStyle WindowClose { get { return Properties.Settings.Default.WindowClose ? WindowStyle.None : WindowStyle.SingleBorderWindow; } }

            // Sets the Window's minimum Width depending on the Mouse position
            public int WindowMaxHeight { get { return !WindowMouseIn && WindowAutoHide && !_WindowVertical ? 5 : 500; } }

            // Sets the Window's Maximum Height depending on the Mouse Position
            public int WindowMaxWidth { get { return !WindowMouseIn && WindowAutoHide && _WindowVertical ? 5 : 500; } }

            // Sets the Window Title
            public string WindowTitle => Shared.windowsTitle_;

            #endregion


            #region Constructor

            /// <summary>
            /// Window's Default Constructor
            /// </summary>
            /// <param name="w">THIS window</param>
            public WBinding(Window w)
            {
                // Sets the variable "ThisWindow" to the actual Window
                ThisWindow = w;

                w.WindowStyle = Properties.Settings.Default.WindowClose ? WindowStyle.None : WindowStyle.SingleBorderWindow;

                // Timer for the Window Auto-Hide
                dispatcherTimer.Tick += dispatcherTimer_Tick; dispatcherTimer.Interval = new TimeSpan(0, 0, 5);

                w.MouseEnter += W_MouseEnter; w.MouseLeave += W_MouseLeave;
            }

            #endregion


            #region Helpers

            // Timer for the Window Auto-Hide
            private void dispatcherTimer_Tick(object sender, EventArgs e) { WindowMouseIn = false; dispatcherTimer.Stop(); }

            #endregion


            #region Commands


            #region User and DateTime Commands

            // Sets the UserName to the Clipboard ready to be pasted
            public Command ClipUserCommand { get { return new Command(() => { Clipboard.SetText(UserStr); }); } }

            // Sets the Date and Time to the Clipboard ready to be pasted
            public Command ClipDateCommand { get { return new Command(() => { Clipboard.SetText(DateTime.Now.ToString("yyyy/MM/dd HH:mm")); }); } }

            #endregion


            #region Options Commands

            // Opens the Options Window
            public Command OptionsPageCommand
            { get { return new Command(() => { if (!Application.Current.Windows.OfType<OptionsWindow>().Any()) { var OpWindow = new OptionsWindow(); OpWindow.Show(); ThisWindow.Close(); } }); } }

            #endregion


            #region Screenshot Commands

            // Takes a screenshot and saves it to the User selected folder
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

            #endregion


            #region Solution Commands

            // Opens the Solution Window
            public Command NewSolutionCommand
            { get { return new Command(() => { if (!Application.Current.Windows.OfType<SolutionsWindow>().Any()) { var Slwindow = new SolutionsWindow(Db.DBTickets.GlobalNewSolution); Slwindow.Show(); } }); } }

            // Reloads all the Solutions
            public Command SolutionsReloadCommand { get { return new Command(() => { OnPropertyChanged(nameof(AllSolutionsList)); }); } }

            // Opens the Solution Window with the selected solution loaded to be Edited
            public Command EditSolutionCommand
            { get { return new Command(() => { if (!Application.Current.Windows.OfType<SolutionsWindow>().Any()) { var Slwindow = new SolutionsWindow(SelectedSolution, true); Slwindow.Show(); } }, () => { return SelectedSolution != null; }); } }

            // Loops between the solutions Languages
            public Command SolutionsLanguageCommand
            {
                get
                {
                    return new Command(() =>
                    { switch (SolLanguage) { case "En": SolLanguage = "Es"; break; case "Es": SolLanguage = "Pt"; break; case "Pt": SolLanguage = "En"; break; } });
                }
            }

            // Copies the Selected Solution to the Clipboard, ready to be pasted (English)
            public Command ButtonOneEnCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EnOne); }, () => { try { return SelectedSolution.EnOne.Length != 0; } catch { return false; } }); } }
            public Command ButtonTwoEnCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EnTwo); }, () => { try { return SelectedSolution.EnTwo.Length != 0; } catch { return false; } }); } }
            public Command ButtonThreeEnCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EnThree); }, () => { try { return SelectedSolution.EnThree.Length != 0; } catch { return false; } }); } }

            // Copies the Selected Solution to the Clipboard, ready to be pasted (Spanish)
            public Command ButtonOneEsCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EsOne); }, () => { try { return SelectedSolution.EsOne.Length != 0; } catch { return false; } }); } }
            public Command ButtonTwoEsCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EsTwo); }, () => { try { return SelectedSolution.EsTwo.Length != 0; } catch { return false; } }); } }
            public Command ButtonThreeEsCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.EsThree); }, () => { try { return SelectedSolution.EsThree.Length != 0; } catch { return false; } }); } }

            // Copies the Selected Solution to the Clipboard, ready to be pasted (Poprtugues)
            public Command ButtonOnePtCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.PtOne); }, () => { try { return SelectedSolution.PtOne.Length != 0; } catch { return false; } }); } }
            public Command ButtonTwoPtCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.PtTwo); }, () => { try { return SelectedSolution.PtTwo.Length != 0; } catch { return false; } }); } }
            public Command ButtonThreePtCommand
            { get { return new Command(() => { Clipboard.SetText(SelectedSolution.PtThree); }, () => { try { return SelectedSolution.PtThree.Length != 0; } catch { return false; } }); } }

            #endregion


            #region Window Commands

            // Stops the Timer when the Mouse is over the application
            private void W_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            { dispatcherTimer.Stop(); dispatcherTimer.Start(); }

            // Restarts the Timer when the Mouse leaves the Application
            private void W_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            { dispatcherTimer.Stop(); WindowMouseIn = true; }

            // Closes the Window
            public Command CloseCommand { get { return new Command(() => { ThisWindow.Close(); }); } }

            #endregion

            #endregion

        }

        #endregion

        // Drags the window
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) { if (Properties.Settings.Default.WindowClose == true) { this.DragMove(); } }
    }
}