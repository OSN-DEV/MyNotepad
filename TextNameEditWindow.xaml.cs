using System.Windows;
using System.Windows.Controls;

namespace MyNotepad {
    /// <summary>
    /// Text Name Edit
    /// </summary>
    public partial class TextNameEditWindow : Window {

        #region Declaration
        private TextNameEditViewModel _viewModel;
        #endregion

        #region Public Property
        public string TextName {
            get { return this._viewModel.TextName; }
        }
        #endregion

        #region Constructor
        public TextNameEditWindow(Window owner, string TextName = "") {
            InitializeComponent();
            this.Owner = owner;
            this._viewModel = new TextNameEditViewModel(this, TextName);
            this.DataContext = this._viewModel;
        }
        #endregion

        #region Event
        private void TextName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            var textBox = sender as TextBox;
            this._viewModel.TextNameChanged(textBox.Text);
        }
        #endregion
    }
}