using MyNotepad.AppCommon;
using MyNotepad.Component;
using MyNotepad.Data;
using OsnCsLib.Common;
using OsnCsLib.File;
using OsnCsLib.WPFComponent.Bind;
using System.Collections.ObjectModel;

namespace MyNotepad {
    /// <summary>
    /// note pad view model
    /// </summary>
    internal class MyNotepadViewModel : BaseBindable{

        #region Declaration
        private readonly MyNotepadWindow _window;
        private readonly AppPreferenceRepo _preference;
        #endregion

        #region Pubic Property
        /// <summary>
        /// text
        /// </summary>
        private string _text;
        public string Text {
            set { base.SetProperty(ref this._text, value); }
            get { return this._text; }
        }

        /// <summary>
        /// list of text
        /// </summary>
        public ObservableCollection<string> TextList { set; get; } = new ObservableCollection<string>();
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
            this._window.Width =0 < this._preference.Width ? this._preference.Width : this._window.Width;
            this._window.Height = 0 < this._preference.Height ? this._preference.Height : this._window.Height;

            this._window.Closing += WindowClosing;

            this.ShowTextList();
        }

        /// <summary>
        /// show text list
        /// </summary>
        private void ShowTextList() {
            this.TextList.Clear();
            var files = System.IO.Directory.GetFiles(this._preference.Workspace, $"*.{Constants.NoteExtension}");
            foreach(var file in files) {
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

            }
        }

        /// <summary>
        /// -
        /// </summary>
        private void DeleteTextClick() {

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
        #endregion
    }
}
