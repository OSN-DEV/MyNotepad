using MyNotepad.AppCommon;
using MyNotepad.Data;
using OsnCsLib.Common;
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
        public ObservableCollection<string> TextList { set; get; }
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

            Util.SetWindowXPosition(this._window, this._preference.X);
            Util.SetWindowYPosition(this._window, this._preference.Y);
            this._window.Width =0 < this._preference.Width ? this._preference.Width : this._window.Width;
            this._window.Height = 0 < this._preference.Height ? this._preference.Height : this._window.Height; 

        }
        #endregion

        #region Private/Public Method(Window Event)
        /// <summary>
        /// [File] - [New Workspace]
        /// </summary>
        private void NewWorkspaceClick() {

        }

        /// <summary>
        /// [File] - [Open Workspace]
        /// </summary>
        private void OpenWorkspaceClick() {

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

        }

        /// <summary>
        /// -
        /// </summary>
        private void DeleteTextClick() {

        }
        #endregion
    }
}
