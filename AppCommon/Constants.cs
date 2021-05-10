namespace MyNotepad.AppCommon {
    /// <summary>
    /// constant definition
    /// </summary>
    internal class Constants {
        /// <summary>
        /// app setting file
        /// </summary>
        public static readonly string SettingsFile = OsnCsLib.Common.Util.GetAppPath() + @"app.settings";

        /// <summary>
        /// the extension of note files
        /// </summary>
        public static readonly string NoteExtension = "txt";
    }
}
