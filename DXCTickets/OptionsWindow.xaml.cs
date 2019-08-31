using System.Collections.Generic;
using System.Windows;


namespace DXCTickets
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        #region Initialize

        /// <summary>
        /// Default Window Contstructor
        /// </summary>
        public OptionsWindow()
        {
            InitializeComponent();
            // Binds the View Model to the Window
            DataContext = new WBinding(this);
        }

        #endregion


        #region Binding

        private class WBinding : ObservableClass
        {
            #region Private

            #region Window Box

            // Window Background Color
            private string _BackColorCmbBxSelItm = Properties.Settings.Default.AllWindowBackground;
            // Application Language
            private string _LangCmbBxSelItm = Properties.Settings.Default.AllWindowLanguage;

            #endregion


            #region Style Box

            // Window Font Color
            private string _FontColorCmbBxSelItm = Properties.Settings.Default.AllWindowFontColor;
            // Window Font Size
            private int _FontSizeCmbBxSelItm = Properties.Settings.Default.AllWindowFontSize;

            #endregion


            #region Buttons Box

            // Button Background Color
            private string _BackColorBtnCmbBxSelItm = Properties.Settings.Default.AllWindowButtonColor;
            // Button Font Color
            private string _FontColorBtnCmbBxSelItm = Properties.Settings.Default.AllWindowButtonFontColor;

            #endregion


            #region Window Box

            // Window Small (True) or Normal (False) Version
            private bool _WindowCloseChckBxIsChckd = Properties.Settings.Default.WindowClose;
            // AutoHide On (True) or Off (False)
            private bool _WindowAutoHideIsChckd = Properties.Settings.Default.WindowAutoHide;
            // Is Window Docked Yes (True) or No (False) NOT IMPLEMENTED YET
            private bool _WindowDockedChckBxIsChckd = Properties.Settings.Default.WindowDocked;
            // Window Orientation Vertical (True) or Horizontal (False)
            private bool _WindowVerticalIsChckd = Properties.Settings.Default.WindowVertical;

            #endregion


            #region Print Box

            // Path to the sound to use when taking a screenshot
            private string _FolderLblCntnt = Properties.Settings.Default.ScreenShotFolder;
            // Screenshot Sound On (True) or Off (False)
            private bool _SoundchckBxChkd = Properties.Settings.Default.ScreenShotSound;

            private string _ErrorLabelCntnt;

            private Window ThisWindow;

            #endregion

            #endregion


            #region Public

            #region Window Box

            // Window Background Color
            public string BackColorCmbBxSelItm { get { return _BackColorCmbBxSelItm; } set { if (_BackColorCmbBxSelItm != value) { _BackColorCmbBxSelItm = value; OnPropertyChanged("BackColorCmbBxSelItm"); } } }
            // Application Language
            public string LangCmbBxSelItm { get { return _LangCmbBxSelItm; } set { if (_LangCmbBxSelItm != value) { _LangCmbBxSelItm = value; OnPropertyChanged("LangCmbBxSelItm"); } } }

            #endregion


            #region Style Box

            // Window Font Color
            public string FontColorCmbBxSelItm { get { return _FontColorCmbBxSelItm; } set { if (_FontColorCmbBxSelItm != value) { _FontColorCmbBxSelItm = value; OnPropertyChanged("FontColorCmbBxSelItm"); } } }
            // Window Font Size
            public int FontSizeCmbBxSelItm { get { return _FontSizeCmbBxSelItm; } set { if (_FontSizeCmbBxSelItm != value) { _FontSizeCmbBxSelItm = value; OnPropertyChanged("FontSizeCmbBxSelItm"); } } }

            #endregion


            #region Button Box

            // Button Background Color
            public string BackColorBtnCmbBxSelItm { get { return _BackColorBtnCmbBxSelItm; } set { if (_BackColorBtnCmbBxSelItm != value) { _BackColorBtnCmbBxSelItm = value; OnPropertyChanged("BackColorBtnCmbBxSelItm"); } } }
            // Button Font Color
            public string FontColorBtnCmbBxSelItm { get { return _FontColorBtnCmbBxSelItm; } set { if (_FontColorBtnCmbBxSelItm != value) { _FontColorBtnCmbBxSelItm = value; OnPropertyChanged("FontColorBtnCmbBxSelItm"); } } }

            #endregion


            #region Window Box

            // Window Small (True) or Normal (False) Version
            public bool WindowCloseChckBxIsChckd { get { return _WindowCloseChckBxIsChckd; } set { if (_WindowCloseChckBxIsChckd != value) { _WindowCloseChckBxIsChckd = value; OnPropertyChanged("WindowCloseChckBxIsChckd"); } } }
            // AutoHide On (True) or Off (False)
            public bool WindowAutoHideIsChckd { get { return _WindowAutoHideIsChckd; } set { if (_WindowAutoHideIsChckd != value) { _WindowAutoHideIsChckd = value; OnPropertyChanged("WindowAutoHideIsChckd"); } } }
            // Is Window Docked Yes (True) or No (False) NOT IMPLEMENTED YET
            public bool WindowDockedChckBxIsChckd { get { return _WindowDockedChckBxIsChckd; } set { if (_WindowDockedChckBxIsChckd != value) { _WindowDockedChckBxIsChckd = value; OnPropertyChanged("WindowDockedChckBxIsChckd"); } } }
            // Window Orientation Vertical (True) or Horizontal (False)
            public bool WindowVerticalIsChckd { get { return _WindowVerticalIsChckd; } set { if (_WindowVerticalIsChckd != value) { _WindowVerticalIsChckd = value; OnPropertyChanged("WindowVerticalIsChckd"); } } }

            #endregion


            #region Print Box

            // Path to the sound to use when taking a screenshot
            public string FolderLblCntnt { get { return _FolderLblCntnt; } set { if (_FolderLblCntnt != value) { _FolderLblCntnt = value; OnPropertyChanged("FolderLblCntnt"); } } }
            // Screenshot Sound On (True) or Off (False)
            public bool SoundchckBxChkd { get { return _SoundchckBxChkd; } set { if (_SoundchckBxChkd != value) { _SoundchckBxChkd = value; OnPropertyChanged("SoundchckBxChkd"); } } }

            public string ErrorLabelCntnt { get { return _ErrorLabelCntnt; } set { if (_ErrorLabelCntnt != value) { _ErrorLabelCntnt = value; OnPropertyChanged("ErrorLabelCntnt"); } } }

            #endregion

            #endregion


            #region ReadOnly

            /// <summary>
            /// READONLY - List of Colors for the Backgrounds
            /// </summary>
            public List<string> OpBackColorCmbBxSource { get { return new List<string> { "AntiqueWhite", "Azure", "Beige", "Bisque", "BurlyWood", "CadetBlue", "Chocolate", "CornflowerBlue", "DarkGoldenrod", "DarkGray", "DarkKhaki", "DarkSalmon", "DarkSeaGreen", "Gray", "Honeydew", "Khaki", "Lavender", "LightGray" }; } }

            /// <summary>
            /// READONLY - List of Colors for the Fonts
            /// </summary>
            public List<string> OpFontColorCmbBxSource { get { return new List<string> { "Black", "Blue", "BlueViolet", "Brown", "CadetBlue", "Coral", "CornflowerBlue", "Crimson", "DarkCyan", "DarkMagenta" }; } }

            /// <summary>
            /// READONLY - List of Languages
            /// </summary>
            public List<string> OpLangCmbBxSource { get { return new List<string> { "English", "Español", "Português" }; } }

            /// <summary>
            /// List of Sizes to choose from for the Fonts
            /// </summary>
            public List<int> OpFontSizeCmbBxSource { get { return new List<int> { 8, 10, 12, 14, 16, 18, 20 }; } }

            #endregion Public


            #region Constructor

            /// <summary>
            /// Default Contrstructor
            /// </summary>
            /// <param name="w">THIS Window</param>
            public WBinding(Window w)
            {
                ThisWindow = w;
            }

            #endregion


            #region Commands

            public Command SelectFolderSSCommand
            {
                get
                {
                    return new Command(() =>
                    {
                        using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                        {
                            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                            if (result.ToString() == "OK") { FolderLblCntnt = dialog.SelectedPath; }
                        }
                    });
                }
            }

            public Command OptionsDefaultCommand
            {
                get
                {
                    return new Command(() =>
                    {
                        BackColorCmbBxSelItm = Properties.Settings.Default.AllWindowBackground;
                        LangCmbBxSelItm = Properties.Settings.Default.AllWindowLanguage;
                        FontColorCmbBxSelItm = Properties.Settings.Default.AllWindowFontColor;
                        FontSizeCmbBxSelItm = Properties.Settings.Default.AllWindowFontSize;
                        BackColorBtnCmbBxSelItm = Properties.Settings.Default.AllWindowButtonColor;
                        FontColorBtnCmbBxSelItm = Properties.Settings.Default.AllWindowButtonFontColor;
                        WindowCloseChckBxIsChckd = Properties.Settings.Default.WindowClose;
                        WindowAutoHideIsChckd = Properties.Settings.Default.WindowAutoHide;
                        WindowDockedChckBxIsChckd = Properties.Settings.Default.WindowDocked;
                        WindowVerticalIsChckd = Properties.Settings.Default.WindowVertical;
                        FolderLblCntnt = Properties.Settings.Default.ScreenShotFolder;
                        SoundchckBxChkd = Properties.Settings.Default.ScreenShotSound;
                    });
                }
            }

            public Command OptionsSaveCommand
            {
                get
                {
                    return new Command(() =>
                    {
                        Properties.Settings.Default.AllWindowBackground = _BackColorCmbBxSelItm;
                        Properties.Settings.Default.AllWindowFontColor = _FontColorCmbBxSelItm;
                        Properties.Settings.Default.AllWindowFontSize = _FontSizeCmbBxSelItm;
                        Properties.Settings.Default.AllWindowButtonColor = _BackColorBtnCmbBxSelItm;
                        Properties.Settings.Default.AllWindowButtonFontColor = _FontColorBtnCmbBxSelItm;
                        Properties.Settings.Default.ScreenShotFolder = _FolderLblCntnt;
                        Properties.Settings.Default.ScreenShotSound = _SoundchckBxChkd;
                        Properties.Settings.Default.WindowClose = _WindowCloseChckBxIsChckd;
                        Properties.Settings.Default.WindowAutoHide = _WindowAutoHideIsChckd;
                        Properties.Settings.Default.WindowDocked = _WindowDockedChckBxIsChckd;
                        Properties.Settings.Default.WindowVertical = _WindowVerticalIsChckd;
                        Properties.Settings.Default.AllWindowLanguage = _LangCmbBxSelItm;
                        Properties.Settings.Default.Save();
                        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Shared.lang_);

                        var FH = new FloatWindow(); FH.Show();
                        ThisWindow.Close();
                    });
                }
            }

            Command CloseCommand { get { return new Command(() => { var FW = new FloatWindow(); FW.Show(); ThisWindow.Close(); }); } }

            #endregion Commands
        }
        #endregion Binding


        // Drags the window
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {  this.DragMove(); }
    }
}
