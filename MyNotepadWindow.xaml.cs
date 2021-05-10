using System.Windows;

namespace MyNotepad {
    /// <summary>
    /// Main window
    /// </summary>
    public partial class MyNotepadWindow : Window {

        #region Declaration 
        private MyNotepadViewModel _viewModel;
        #endregion

        #region Constructor
        public MyNotepadWindow() {
            InitializeComponent();
            // this._viewModel = new MyNotepadViewModel(this);
            this.DataContext = new MyNotepadViewModel(this);
        }
        #endregion
    }
}
