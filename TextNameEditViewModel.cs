using OsnCsLib.WPFComponent.Bind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyNotepad {
    internal class TextNameEditViewModel : BaseBindable{

        #region Declaration
        Window _window;
        #endregion

        #region Public Property
        /// <summary>
        /// text name
        /// </summary>
        private string _textName = "";
        public string TextName {
            set { 
                base.SetProperty(ref this._textName, value);
                base.SetProperty(nameof(OKEnabled));
            }
            get { return this._textName; }
        }

        public bool OKEnabled {
            get { return 0 < this.TextName.Length;  }
        }
        #endregion

        #region Command
        public DelegateCommand OKComand { set; get; }
        #endregion

        #region Constructor
        internal TextNameEditViewModel(Window window, string TextName) {
            this._window = window;
            this.TextName = TextName;
            this.OKComand = new DelegateCommand(this.OKClick);

            this._window.KeyDown += WindowKeyDown;
        }

        #endregion

        #region Public Method
        public void TextNameChanged(string text) {
            this.TextName = text;
        }
        #endregion

        #region Private Method(Event)
        /// <summary>
        /// ok click
        /// </summary>
        private void OKClick() {
            this._window.DialogResult = true;
        }

        /// <summary>
        /// Key event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            switch(e.Key) {
                case System.Windows.Input.Key.Enter:
                    e.Handled = true;
                    if (this.OKEnabled) {
                        this.OKClick();
                    }
                    break;
            }
        }
        #endregion

    }
}
