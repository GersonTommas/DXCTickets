//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace DXCTickets
{
    /// <summary>
    /// Interaction logic for SolutionsWindow.xaml
    /// </summary>
    public partial class SolutionsWindow : Window
    {
        #region Initialize

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="EditSol">A solution to Edit, or a New Blank Solution</param>
        /// <param name="Editing">This will be True if the solution is not a New Blank Solution</param>
        public SolutionsWindow(DBSolutionsClass EditSol, bool Editing = false)
        {
            InitializeComponent();
            // Binds the View Model to the Window
            DataContext = new WBinding(this, EditSol, Editing);
        }

        #endregion


        #region Binding

        private class WBinding : ObservableClass
        {
            #region Private

            // A solution to Edit, or a New Blank Solution
            private DBSolutionsClass _SelectedSolution;

            // This will be True if the solution is not a New Blank Solution
            private bool _Editing = false;

            // THIS Window
            private Window ThisWindow;

            #endregion


            #region Public
            // The Tile of the Solition Window
            public string WindowTitle => Shared.windowsTitle_;

            // A solution to Edit, or a New Blank Solution
            public DBSolutionsClass SelectedSolution { get { return _SelectedSolution; } set { if (_SelectedSolution != value) { _SelectedSolution = value; OnPropertyChanged("SelectedSolution"); } } }
            #endregion


            #region Constructor

            /// <summary>
            /// Window's Default Constructor
            /// </summary>
            /// <param name="w">THIS window</param>
            public WBinding(Window w, DBSolutionsClass s, bool e = false)
            {
                // Sets the variable "ThisWindow" to the actual Window
                ThisWindow = w; _Editing = e; _SelectedSolution = s;
            }

            #endregion


            #region Commands

            /// <summary>
            /// Saves all Changes and closes the Solution Window
            /// </summary>
            public Command SaveCommand
            { get { return new Command(() => { if (_Editing == false) { Db.DBTickets.AllSolutionsTable.InsertOnSubmit(SelectedSolution); } Db.DBTickets.SubmitChanges(); ThisWindow.Close(); }, () => { return !string.IsNullOrWhiteSpace(SelectedSolution.Name); }); } }

            /// <summary>
            /// Closes the Solution Window without saving Changes
            /// </summary>
            public Command CancelCommand
            { get { return new Command(() => { ThisWindow.Close(); }); } }

            #endregion
        }

        #endregion
    }
}
