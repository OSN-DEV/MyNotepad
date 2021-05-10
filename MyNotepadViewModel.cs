using MyNotepad.AppCommon;
using MyNotepad.Component;
using MyNotepad.Data;
using OsnCsLib.Common;
using OsnCsLib.File;
using OsnCsLib.WPFComponent.Bind;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyNotepad {
    /// <summary>
    /// note pad view model
    /// </summary>
    internal class MyNotepadViewModel : BaseBindable {

        #region Declaration
        private readonly MyNotepadWindow _window;
        private readonly AppPreferenceRepo _preference;
        #endregion

        #region Pubic Property
        private int _currentIndex;
        public int CurrentIndex {
            set {
                base.SetProperty(ref this._currentIndex, value);
                base.SetProperty(nameof(this.IsDeleteEnabled));
            }
            get { return this._currentIndex; }
        }

        /// <summary>
        /// text
        /// </summary>
        private string _textData;
        public string TextData {
            set { base.SetProperty(ref this._textData, value); }
            get { return this._textData; }
        }

        /// <summary>
        /// list of text
        /// </summary>
        public ObservableCollection<string> TextList { set; get; } = new ObservableCollection<string>();

        /// <summary>
        /// delete text enabled
        /// </summary>
        public bool IsDeleteEnabled {
            get { return 0 <= this._currentIndex; }
        }
        #endregion

        #region Command
        /// <summary>
        /// Create new workspace
        /// </summary>
        public DelegateCommand NewWorkspaceCommand { set; get; }

        /// <summary>
        /// Open workspace
        /// </summary>
        public DelegateCommand OpenWorkspaceCommand { set; get; }

        /// <summary>
        /// quit app
        /// </summary>
        public DelegateCommand AppExitCommand { set; get; }

        /// <summary>
        /// toggle sidebar visiblity
        /// </summary>
        public DelegateCommand ToggleSidebarCommand { set; get; }

        /// <summary>
        /// add text
        /// </summary>
        public DelegateCommand AddTextCommand { set; get; }

        /// <summary>
        /// delete text
        /// </summary>
        public DelegateCommand DeleteTextCommand { set; get; }
        #endregion

        #region Constructor
        internal MyNotepadViewModel(MyNotepadWindow window) {
            this._window = window;
            this._preference = AppPreferenceRepo.Init(Constants.SettingsFile);

            this.Initialize();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize view model
        /// </summary>
        private void Initialize() {
            this.NewWorkspaceCommand = new DelegateCommand(this.NewWorkspaceClick);
            this.OpenWorkspaceCommand = new DelegateCommand(this.OpenWorkspaceClick);
            this.AppExitCommand = new DelegateCommand(this.AppExitClick);
            this.ToggleSidebarCommand = new DelegateCommand(this.ToggleSidebarClick);
            this.AddTextCommand = new DelegateCommand(this.AddTextClick);
            this.DeleteTextCommand = new DelegateCommand(this.DeleteTextClick);

            if (0 < this._preference.X) {
                Util.SetWindowXPosition(this._window, this._preference.X);
            }
            if (0 < this._preference.Y) {
                Util.SetWindowYPosition(this._window, this._preference.Y);
            }
            this._window.Width = 0 < this._preference.Width ? this._preference.Width : this._window.Width;
            this._window.Height = 0 < this._preference.Height ? this._preference.Height : this._window.Height;

            this._window.Closing += WindowClosing;
            this._window.KeyDown += WindowKeyDown;

            this.ShowTextList();
        }


        /// <summary>
        /// show text list
        /// </summary>
        private void ShowTextList() {
            this.TextList.Clear();
            var files = System.IO.Directory.GetFiles(this._preference.Workspace, $"*.{Constants.NoteExtension}");
            foreach (var file in files) {
                this.TextList.Add(new FileOperator(file).NameWithoutExtension);
            }
        }
        #endregion

        #region Private/Public Method(Window Event)
        /// <summary>
        /// [File] - [New Workspace]
        /// </summary>
        private void NewWorkspaceClick() {
            var dialog = new FolderSelectDialog();
            dialog.Title = "フォルダ選択";
            dialog.Path = this._preference.Workspace;
            if (dialog.ShowDialog(this._window)) {
                this._preference.Workspace = dialog.Path;
                this._preference.Save();
                this.ShowTextList();
            }
        }

        /// <summary>
        /// [File] - [Open Workspace]
        /// </summary>
        private void OpenWorkspaceClick() {
            // 現時点ではワークスペースの作成・オープンに差異はなし(将来的にも無いかも。。)
            this.NewWorkspaceClick();
        }

        /// <summary>
        /// [File] - [Exit]
        /// </summary>
        private void AppExitClick() {
            this._window.Close();
        }

        /// <summary>
        /// [View] - [Sidebar]
        /// </summary>
        private void ToggleSidebarClick() {

        }

        /// <summary>
        /// + 
        /// </summary>
        private void AddTextClick() {
            var dialog = new TextNameEditWindow(this._window);
            if (true == dialog.ShowDialog()) {
                var dest = this.GetFilePath(dialog.TextName);
                if (System.IO.File.Exists(dest)) {
                    Message.ShowError(this._window, Message.ErrId.Err002);
                    return;
                }
                System.IO.File.Create(dest).Close();
                this.TextList.Add(new FileOperator(dest).NameWithoutExtension);
                this.TextList = new ObservableCollection<string>(this.TextList.OrderBy(n => n));
                base.SetProperty(nameof(TextList));
                for (var i = 0; i < this.TextList.Count; i++) {
                    if (this.TextList[i] == dialog.TextName) {
                        this.CurrentIndex = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// -
        /// </summary>
        private void DeleteTextClick() {
            var file = this.TextList[this.CurrentIndex];
            System.IO.File.Delete(this.GetFilePath(file));
            this.TextList.RemoveAt(this.CurrentIndex);
            this.TextData = "";
        }

        /// <summary>
        /// window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e) {
            this._preference.X = this._window.Left;
            this._preference.Y = this._window.Top;
            this._preference.Width = this._window.Width;
            this._preference.Height = this._window.Height;
            this._preference.Save();
        }

        private void WindowKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (Util.IsModifierPressed(System.Windows.Input.ModifierKeys.Control) && e.Key == System.Windows.Input.Key.S) {
            } else {
                return;
            }
            e.Handled = true;
            var file = this.TextList[this.CurrentIndex];
            using (var op = new FileOperator(this.GetFilePath(file))) {
                op.OpenForWrite();
                op.Write(this.TextData);
            }
        }

        /// <summary>
        /// text name double click
        /// </summary>
        public void TextDoubleClick() {
            var textName = this.TextList[this.CurrentIndex];
            var dialog = new TextNameEditWindow(this._window, textName);
            if (true == dialog.ShowDialog()) {
                var dest = this.GetFilePath(dialog.TextName);
                if (System.IO.File.Exists(dest)) {
                    Message.ShowError(this._window, Message.ErrId.Err002);
                    return;
                }

                System.IO.File.Move(this.GetFilePath(textName), dest);
                this.TextList[this.CurrentIndex] = dialog.TextName;
                this.TextList = new ObservableCollection<string>(this.TextList.OrderBy(n => n));
                base.SetProperty(nameof(TextList));
                for (var i = 0; i < this.TextList.Count; i++) {
                    if (this.TextList[i] == dialog.TextName) {
                        this.CurrentIndex = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// selected text name is changed
        /// </summary>
        public void SelectedTextChanged() {
            if (0 <= this.CurrentIndex) {
                var file = this.TextList[this.CurrentIndex];
                using (var op = new FileOperator(this.GetFilePath(file))) {
                    this.TextData = op.ReadAll();
                }
            }
        }
        #endregion


        #region Private Method
        /// <summary>
        /// get file name from path
        /// </summary>
        /// <param name="name"></param>
        private string GetFilePath(string name) {
            return $@"{this._preference.Workspace}\{name}.{Constants.NoteExtension}";
        }
        #endregion
    }
}
