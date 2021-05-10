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
            this._viewModel = new MyNotepadViewModel(this);
            this.DataContext = this._viewModel;
        }
        #endregion


        #region Event
        private void TextNameListMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            this._viewModel.TextDoubleClick();
        }

        private void TextNameListSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            this._viewModel.SelectedTextChanged();
        }
        #endregion
    }
}
