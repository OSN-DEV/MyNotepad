using OsnCsLib.Data;

namespace MyNotepad.Data {
    /// <summary>
    /// app setting data preference
    /// </summary>
    public class AppPreferenceRepo : AppDataBase<AppPreferenceRepo> {

        #region Declaration
        private static string _file;    // setting file
        #endregion

        #region Public Property
        /// <summary>
        /// window x position
        /// </summary>
        public double X { set; get; }

        /// <summary>
        /// windows y position
        /// </summary>
        public double Y { set; get; }

        /// <summary>
        /// window width
        /// </summary>
        public double Width { set; get; }

        /// <summary>
        /// window height
        /// </summary>
        public double Height { set; get; }

        /// <summary>
        /// last workspace
        /// </summary>
        public string Workspace { set; get; }
        #endregion

        #region Public Method
        /// <summary>
        /// initialize repo
        /// </summary>
        /// <param name="file">setting file</param>
        /// <returns>instance</returns>
        public static AppPreferenceRepo Init(string file) {
            _file = file;
            GetInstanceBase(file);
            if (!System.IO.File.Exists(file)) {
                _instance.Save();
            }
            return _instance;
        }

        /// <summary>
        /// get instance
        /// </summary>
        /// <returns>instance</returns>
        public static AppPreferenceRepo GetInstance() {
            return GetInstanceBase();
        }

        /// <summary>
        /// save data
        /// </summary>
        public void Save() {
            GetInstanceBase().SaveToXml(_file);
        }
        #endregion
    }
}
